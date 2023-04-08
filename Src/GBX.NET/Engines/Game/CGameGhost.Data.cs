using System.Collections.ObjectModel;
using System.IO.Compression;

namespace GBX.NET.Engines.Game;

public partial class CGameGhost
{
    public partial class Data
    {
        public byte[] GhostData { get; }
        public bool IsOldData { get; }
        public bool SamplesRequested { get; private set; }

        /// <summary>
        /// How much time is between each sample.
        /// </summary>
        public TimeInt32 SamplePeriod { get; set; }

        public ObservableCollection<Sample> Samples { get; private set; }
        public CompressionLevel Compression { get; set; }
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

        public Data(byte[] ghostData, bool isOldData)
        {
            GhostData = ghostData;
            IsOldData = isOldData;
            Samples = new ObservableCollection<Sample>();
        }

        internal void Parse()
        {
            if (SamplesRequested)
            {
                return;
            }

            SamplesRequested = true;

            using var ms = new MemoryStream(GhostData);

            if (IsOldData)
            {
                throw new NotSupportedException("Old ghost data is not supported.");
            }
            
            Read(ms, compressed: true);
        }

        /// <summary>
        /// Read compressed or uncompressed ghost data from <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <param name="compressed">If stream contains compressed data.</param>
        private void Read(Stream stream, bool compressed)
        {
            if (compressed)
            {
                using var zlib = new CompressedStream(stream, CompressionMode.Decompress);
                using var r = new GameBoxReader(zlib);

                Read(r);

                Compression = zlib.Compression.GetValueOrDefault();
            }
            else
            {
                Compression = CompressionLevel.NoCompression;
                Read(stream);
            }
        }

        /// <summary>
        /// Read uncompressed ghost data from <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        private void Read(Stream stream)
        {
            using var r = new GameBoxReader(stream);
            Read(r);
        }

        /// <summary>
        /// Read uncompressed ghost data with <see cref="GameBoxReader"/>.
        /// </summary>
        /// <param name="r">Reader.</param>
        private void Read(GameBoxReader r)
        {
            SavedMobilClassId = r.ReadUInt32(); // CSceneVehicleCar or CSceneMobilCharVis
            
            if (SavedMobilClassId == uint.MaxValue)
            {
                return;
            }

            IsFixedTimeStep = r.ReadBoolean(); // IsFixedTimeStep
            var u01 = r.ReadInt32();
            SamplePeriod = TimeInt32.FromMilliseconds(r.ReadInt32()); // SavedPeriod
            Version = r.ReadInt32();
            
            var stateBuffer = r.ReadBytes(); // CGameGhostTMData::ArchiveStateBuffer
            
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
                        sampleSizes = r.ReadArray<int>(numSamples - 1);
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
            
            Samples = new ObservableCollection<Sample>();

            if (numSamples <= 0)
            {
                return;
            }
            
            using var stateBufferMs = new MemoryStream(stateBuffer);
            using var stateBufferR = new GameBoxReader(stateBufferMs);

            for (var i = 0; i < numSamples; i++)
            {
                var sampleData = sizePerSample switch
                {
                    -1 => GetSampleDataFromDifferentSizes(stateBufferR, numSamples, sampleSizes, i),
                    _ => stateBufferR.ReadBytes(sizePerSample)
                };

                var time = new TimeInt32(sampleTimes?[i] ?? i * SamplePeriod.Milliseconds);

                Sample? sample = SavedMobilClassId switch
                {
                    0x0A02B000 => new CSceneVehicleCar.Sample(time, sampleData),
                    0x0A401000 => new CSceneMobilCharVis.Sample(time, sampleData),
                    _ => null
                };

                if (sample is null)
                {
                    continue;
                }

                if (sampleData.Length > 0)
                {
                    using var sampleMs = new MemoryStream(sampleData);
                    using var sampleR = new GameBoxReader(sampleMs);

                    sample.Read(sampleMs, sampleR, Version);

                    var sampleProgress = (int)sampleMs.Position;
                }

                Samples.Add(sample);
            }
        }

        private static byte[] GetSampleDataFromDifferentSizes(GameBoxReader reader, int numSamples, int[]? sizesPerSample, int i)
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
