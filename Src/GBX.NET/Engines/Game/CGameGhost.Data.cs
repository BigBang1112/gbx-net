using GBX.NET.Managers;

namespace GBX.NET.Engines.Game;

public partial class CGameGhost
{
    public partial class Data : IReadable, IWritable
    {
        private int[] stateTimes = [];

        /// <summary>
        /// How much time is between each sample.
        /// </summary>
        public TimeInt32 SamplePeriod { get; set; }

        public List<Sample> Samples { get; set; } = [];
        [Hexadecimal]
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
        public int? U02 { get; set; }
        public int[]? Offsets { get; set; }

        public int? FirstSampleOffset { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            switch (v)
            {
                case 0: ReadOld(r); break;
                case 1: ReadNew(r); break;
                default: throw new NotSupportedException($"Version {v} is not supported.");
            }
        }

        public void Write(GbxWriter w, int v = 0)
        {
            switch (v)
            {
                case 0: WriteOld(w); break;
                case 1: WriteNew(w); break;
                default: throw new NotSupportedException($"Version {v} is not supported.");
            }
        }

        private void ReadOld(GbxReader r)
        {
            Offsets = r.ReadArray<int>();
            stateTimes = r.ReadArray<int>();
            IsFixedTimeStep = r.ReadBoolean();
            SamplePeriod = r.ReadTimeInt32();
            Version = r.ReadInt32();
        }

        private void WriteOld(GbxWriter w)
        {
            w.WriteArray(Offsets);
            w.WriteArray(stateTimes);
            w.Write(IsFixedTimeStep);
            w.Write(SamplePeriod);
            w.Write(Version);
        }

        internal void ReadSamplesOld(GbxReader r)
        {
            Samples = [];

            if (Offsets is null)
            {
                throw new NotSupportedException("This type of ghost data is not supported.");
            }

            var prevOffset = Offsets[0];

            for (int i = 1; i < Offsets.Length; i++)
            {
                var nextOffset = Offsets[i];

                Samples.Add(ReadSample(new TimeInt32((i - 1) * SamplePeriod.Milliseconds), sampleData: r.ReadBytes(nextOffset - prevOffset)));

                prevOffset = nextOffset;
            }

            Samples.Add(ReadSample(new TimeInt32((Offsets.Length - 1) * SamplePeriod.Milliseconds), sampleData: r.ReadBytes((int)r.BaseStream.Length - Offsets[Offsets.Length - 1])));

            if (r.BaseStream.Position != r.BaseStream.Length)
            {
                throw new InvalidDataException($"Not all data was read. {r.BaseStream.Length - r.BaseStream.Position} bytes remaining.");
            }
        }

        internal void WriteSamplesOld(GbxWriter w)
        {
            if (Offsets is null || Samples.Count == 0)
            {
                throw new NotSupportedException("This type of ghost data is not supported.");
            }

            foreach (var sample in Samples)
            {
                w.Write(GetSampleData(sample));
            }
        }

        private void ReadNew(GbxReader r)
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
                FirstSampleOffset = r.ReadInt32();

                if (numSamples > 1)
                {
                    sizePerSample = r.ReadInt32();

                    if (sizePerSample == -1)
                    {
                        sampleSizes = r.ReadArray<int>(numSamples - 1); // state offset deltas
                    }
                }
            }

            if (!IsFixedTimeStep)
            {
                // CGameGhostTMData::ArchiveStateTimes
                stateTimes = r.ReadArray<int>();
            }

            // guessed. there's some difference between TM1 and TM2 here, pls investigate soon
            if (Version >= 10)
            {
                U02 = r.ReadInt32();
            }

            if (r.BaseStream.Position != r.BaseStream.Length)
            {
                throw new InvalidDataException($"Not all data was read. {r.BaseStream.Length - r.BaseStream.Position} bytes remaining.");
            }

            Samples = [];

            if (numSamples <= 0)
            {
                return;
            }

            var currentTime = TimeInt32.Zero;

            using var stateBufferMs = new MemoryStream(stateBuffer);
            using var stateBufferR = new GbxReader(stateBufferMs);

            for (var i = 0; i < numSamples; i++)
            {
                var sampleData = sizePerSample switch
                {
                    -1 => GetSampleDataFromDifferentSizes(stateBufferR, numSamples, sampleSizes, i),
                    _ => stateBufferR.ReadBytes(sizePerSample)
                };

                if (stateTimes.Length == 0)
                {
                    currentTime = new TimeInt32(i * SamplePeriod.Milliseconds);
                }
                else
                {
                    currentTime += new TimeInt32(stateTimes[i]);
                }

                Samples.Add(ReadSample(currentTime, sampleData));
            }
        }

        private void WriteNew(GbxWriter w)
        {
            w.Write(SavedMobilClassId);

            if (SavedMobilClassId == uint.MaxValue)
            {
                return;
            }

            w.Write(IsFixedTimeStep);
            w.Write(U01.GetValueOrDefault());
            w.Write(SamplePeriod);
            w.Write(Version);

            using var stateBufferMs = new MemoryStream();
            using var stateBufferW = new GbxWriter(stateBufferMs);

            var numSamples = Samples.Count;
            var sampleSizes = new int[Math.Max(0, numSamples - 1)];
            var sizePerSample = -1;
            var isUniformSize = true;
            var firstSize = 0;

            for (var i = 0; i < numSamples; i++)
            {
                var sampleData = GetSampleData(Samples[i]);
                stateBufferW.Write(sampleData);

                if (i == 0)
                    firstSize = sampleData.Length;
                else if (sampleData.Length != firstSize)
                    isUniformSize = false;

                if (i < numSamples - 1)
                {
                    sampleSizes[i] = sampleData.Length;
                }
            }

            if (numSamples > 1 && isUniformSize)
            {
                sizePerSample = firstSize;
            }

            w.WriteData(stateBufferMs.ToArray());

            w.Write(numSamples);

            if (numSamples > 0)
            {
                w.Write(FirstSampleOffset ?? 0);

                if (numSamples > 1)
                {
                    w.Write(sizePerSample);

                    if (sizePerSample == -1)
                    {
                        w.WriteArray<int>(sampleSizes, numSamples - 1);
                    }
                }
            }

            if (!IsFixedTimeStep)
            {
                // CGameGhostTMData::ArchiveStateTimes
                w.WriteArray(stateTimes);
            }

            if (Version >= 10)
            {
                w.Write(U02.GetValueOrDefault());
            }
        }

        private Sample ReadSample(TimeInt32 time, byte[] sampleData)
        {
            Sample sample = SavedMobilClassId switch
            {
                0x0A02B000 or 0x0A103000 => new CSceneVehicleCar.Sample(time, sampleData),
                0x0A401000 => new CSceneMobilCharVis.Sample(time, sampleData),
                _ => throw new NotSupportedException($"Class ID 0x{SavedMobilClassId:X8} is not supported.")
            };

            if (sampleData.Length == 0)
            {
                return sample;
            }

            using var sampleMs = new MemoryStream(sampleData);
            using var sampleR = new GbxReader(sampleMs);

            sample.Read(sampleR, Version);

            return sample;
        }

        private byte[] GetSampleData(Sample sample)
        {
            using var sampleMs = new MemoryStream();
            using var sampleW = new GbxWriter(sampleMs);

            sample.Write(sampleW, Version);

            return sampleMs.ToArray();
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
            {
                return null;
            }

            var sampleKey = timestamp.TotalMilliseconds / SamplePeriod.TotalMilliseconds;
            var a = Samples.ElementAtOrDefault((int)Math.Floor(sampleKey));
            var b = Samples.ElementAtOrDefault((int)Math.Ceiling(sampleKey));

            if (a is null) // Timestamp is outside of the range
            {
                return null;
            }

            if (b is null || a == b) // There's no second sample to interpolate with
            {
                return a;
            }

            var t = (float)(sampleKey - Math.Floor(sampleKey));

            return new Sample(timestamp, a.Data)
            {
                Position = AdditionalMath.Lerp(a.Position, b.Position, t),
                Rotation = AdditionalMath.Lerp(a.Rotation, b.Rotation, t),
                Velocity = AdditionalMath.Lerp(a.Velocity, b.Velocity, t)
            };
        }

        public override string ToString()
        {
            if (SavedMobilClassId == uint.MaxValue)
            {
                return "CGameGhost.Data (empty)";
            }

            return $"CGameGhost.Data ({ClassManager.GetName(SavedMobilClassId)} 0x{SavedMobilClassId:X8}, {Samples.Count} samples)";
        }
    }
}