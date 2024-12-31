using System.Collections.ObjectModel;

namespace GBX.NET.Engines.Game;

public partial class CGameGhost
{
    public partial class Data
    {
        private readonly byte[]? rawGhostData;
        private readonly CompressedData? compressedGhostData;

        public bool SamplesRequested { get; private set; }

        /// <summary>
        /// How much time is between each sample.
        /// </summary>
        public TimeInt32 SamplePeriod { get; set; }

        public ObservableCollection<Sample> Samples { get; private set; }
        public uint SavedMobilClassId { get; set; }
        public bool IsFixedTimeStep { get; set; }

        // CSceneVehicleVis_RestoreStaticState
        // 2 = SVehicleSimpleState_ReplayAfter211003 - 22 bytes
        // 4 = SVehicleSimpleState_ReplayAfter040104 - 28 bytes
        // 5 = SVehicleSimpleState_ReplayAfter100205 - 30 bytes
        // 7 = SVehicleSimpleState_ReplayAfter100205 - 30 bytes
        // 8 = SVehicleSimpleState_ReplayAfter081205 - 34 bytes
        // 9 = SVehicleSimpleState_ReplayAfter081205 - 35 bytes
        // 10 = SVehicleSimpleState_ReplayAfter230211 - 42 bytes
        // 11 = SVehicleSimpleState_ReplayAfter230211 - 42 bytes
        // 12 = SVehicleSimpleState_ReplayAfter230211 - 42 bytes + something
        // 13 = SVehicleSimpleNetState - 2 bytes
        // 14 = SVehicleSimpleState_ReplayAfter270115 - 44 bytes
        // 15 = SVehicleSimpleState_ReplayAfter270115 - 45 bytes
        // 16 = SVehicleSimpleState_ReplayAfter160216 - 47 bytes

        // NSceneMgr_Vehicle::StateArchive
        // 17 = SVehicleSimpleState_ReplayAfter100117
        // 18 = SVehicleSimpleState_ReplayAfter111217
        // 19 = SVehicleSimpleState_ReplayAfter111217 + 1 more byte
        // 20 = SVehicleSimpleState_ReplayAfter2018_03_09
        public int Version { get; set; }

        public int? U01 { get; set; }
        public int[]? Offsets { get; set; }

        internal Data(CompressedData ghostData)
        {
            compressedGhostData = ghostData;
            Samples = [];
        }

        internal Data(byte[] ghostData)
        {
            rawGhostData = ghostData;
            Samples = [];
        }

        internal void Parse()
        {
            if (SamplesRequested)
            {
                return;
            }

            SamplesRequested = true;

            if (compressedGhostData is not null)
            {
                Read(compressedGhostData);
                return;
            }

            if (Offsets is null)
            {
                throw new NotSupportedException("This type of ghost data is not supported.");
            }

            SavedMobilClassId = 0x0A02B000;

            if (rawGhostData is not null && Offsets.Length > 0)
            {
                Samples = [];

                using var ms = new MemoryStream(rawGhostData);
                using var r = new GbxReader(ms);

                var prevOffset = Offsets[0];

                for (int i = 1; i < Offsets.Length; i++)
                {
                    var offset = Offsets[i - 1];

                    Samples.Add(ReadSample(i - 1, sampleData: r.ReadBytes(offset - prevOffset)));

                    prevOffset = offset;
                }

                Samples.Add(ReadSample(Offsets.Length - 1, sampleData: r.ReadBytes((int)r.BaseStream.Length - Offsets[Offsets.Length - 1])));
            }
        }

        private void Read(CompressedData data)
        {
            using var ms = data.OpenDecompressedMemoryStream();
            using var r = new GbxReader(ms);

            Read(r);
        }

        /// <summary>
        /// Read uncompressed ghost data with <see cref="GbxReader"/>.
        /// </summary>
        /// <param name="r">Reader.</param>
        private void Read(GbxReader r)
        {
            SavedMobilClassId = r.ReadUInt32(); // CSceneVehicleCar or CSceneMobilCharVis

            if (SavedMobilClassId == uint.MaxValue)
            {
                return;
            }

            IsFixedTimeStep = r.ReadBoolean(); // IsFixedTimeStep
            U01 = r.ReadInt32();
            SamplePeriod = r.ReadTimeInt32(); // SavedPeriod
            Version = r.ReadInt32();

            var stateBuffer = r.ReadData(); // CGameGhostTMData::ArchiveStateBuffer

            // CGameGhostTMData::ArchiveStateOffsets
            var sizePerSample = -1;
            var sampleSizes = default(int[]);

            var numSamples = r.ReadInt32(); // StateOffsets count

            if (numSamples > 0)
            {
                var firstSampleOffset = r.ReadInt32();

                if (numSamples > 1)
                {
                    sizePerSample = r.ReadInt32();

                    if (sizePerSample == -1)
                    {
                        sampleSizes = r.ReadArray<int>(numSamples - 1); // state offset deltas
                    }
                }
            }
            //

            // CGameGhostTMData::ArchiveStateTimes
            var sampleTimes = default(int[]);

            if (!IsFixedTimeStep)
            {
                sampleTimes = r.ReadArray<int>();
            }
            //

            Samples = [];

            if (numSamples <= 0)
            {
                return;
            }

            using var stateBufferMs = new MemoryStream(stateBuffer);
            using var stateBufferR = new GbxReader(stateBufferMs);

            for (var i = 0; i < numSamples; i++)
            {
                var sampleData = sizePerSample switch
                {
                    -1 => GetSampleDataFromDifferentSizes(stateBufferR, numSamples, sampleSizes, i),
                    _ => stateBufferR.ReadBytes(sizePerSample)
                };

                Samples.Add(ReadSample(i, sampleData, sampleTimes));
            }
        }

        private Sample ReadSample(int i, byte[] sampleData, int[]? sampleTimes = null)
        {
            var time = new TimeInt32(sampleTimes?[i] ?? i * SamplePeriod.Milliseconds);

            Sample sample = SavedMobilClassId switch
            {
                0x0A02B000 => new CSceneVehicleCar.Sample(time, sampleData),
                0x0A401000 => new CSceneMobilCharVis.Sample(time, sampleData),
                _ => throw new NotSupportedException($"Class ID 0x{SavedMobilClassId:X8} is not supported.")
            };

            if (sampleData.Length == 0)
            {
                return sample;
            }

            using var sampleMs = new MemoryStream(sampleData);
            using var sampleR = new GbxReader(sampleMs);

            sample.Read(sampleMs, sampleR, Version);

            var sampleProgress = (int)sampleMs.Position;

            return sample;
        }

        private static byte[] GetSampleDataFromDifferentSizes(GbxReader reader, int numSamples, int[]? sizesPerSample, int i)
        {
            if (i == numSamples - 1) // Last sample size not included
                return reader.ReadToEnd();

            if (sizesPerSample is null)
                throw new ThisShouldNotHappenException();

            return reader.ReadBytes(sizesPerSample[i]);
        }

        /// <summary>
        /// Linearly interpolates <see cref="Sample.Position"/>, <see cref="Sample.Rotation"/>,
        /// <see cref="Sample.VelocitySpeed"/> and <see cref="Sample.Velocity"/> between two samples. Unknown data is taken from sample A.
        /// </summary>
        /// <param name="timestamp">Any timestamp between the range of samples.</param>
        /// <returns>A new instance of <see cref="Sample"/> that has been linearly interpolated (<see cref="Sample.Time"/> will be null)
        /// or a reference to an existing sample if <paramref name="timestamp"/> matches an existing sample timestamp.
        /// Also returns null if there are no samples, or if <paramref name="timestamp"/> is outside of the sample range,
        /// or <see cref="SamplePeriod"/> is lower or equal to 0.</returns>
        public Sample? GetSampleLerp(TimeSingle timestamp)
        {
            if (Samples is null || Samples.Count == 0 || SamplePeriod.Ticks <= 0)
                return null;

            var sampleKey = timestamp.TotalMilliseconds / SamplePeriod.TotalMilliseconds;
            var a = Samples.ElementAtOrDefault((int)Math.Floor(sampleKey)); // Sample A
            var b = Samples.ElementAtOrDefault((int)Math.Ceiling(sampleKey)); // Sample B

            if (a == null) // Timestamp is outside of the range
                return null;

            if (b == null || a == b) // There's no second sample to interpolate with
                return a;

            var t = (float)(sampleKey - Math.Floor(sampleKey));

            return new Sample(timestamp, a.Data)
            {
                Position = AdditionalMath.Lerp(a.Position, b.Position, t),
                Rotation = AdditionalMath.Lerp(a.Rotation, b.Rotation, t),
                Velocity = AdditionalMath.Lerp(a.Velocity, b.Velocity, t)
            };
        }
    }
}