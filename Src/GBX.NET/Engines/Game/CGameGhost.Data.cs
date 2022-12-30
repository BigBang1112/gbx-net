using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO.Compression;

namespace GBX.NET.Engines.Game;

public partial class CGameGhost
{
    public partial class Data
    {
        private TimeInt32 samplePeriod;

        /// <summary>
        /// How much time is between each sample.
        /// </summary>
        public TimeInt32 SamplePeriod
        {
            get => samplePeriod;
            set
            {
                samplePeriod = value;

                if (Samples != null)
                    foreach (var sample in Samples)
                        sample.UpdateTimestamp();
            }
        }

        public ObservableCollection<Sample> Samples { get; private set; }
        public CompressionLevel Compression { get; set; }
        public uint SavedMobilClassId { get; set; }

        public Data()
        {
            Samples = new ObservableCollection<Sample>();
        }

        private void Samples_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (Sample sample in e.OldItems)
                    sample.AssignTo(null);

            if (e.NewStartingIndex != Samples.Count - 1)
                foreach (var sample in Samples.Skip(e.NewStartingIndex))
                    sample.AssignTo(this);
        }

        /// <summary>
        /// Read compressed or uncompressed ghost data from <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <param name="compressed">If stream contains compressed data.</param>
        public void Read(Stream stream, bool compressed)
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
        public void Read(Stream stream)
        {
            using var r = new GameBoxReader(stream);
            Read(r);
        }

        /// <summary>
        /// Read uncompressed ghost data with <see cref="GameBoxReader"/>.
        /// </summary>
        /// <param name="r">Reader.</param>
        public void Read(GameBoxReader r)
        {
            SavedMobilClassId = r.ReadUInt32(); // CSceneVehicleCar or CSceneMobilCharVis
            
            if (SavedMobilClassId == uint.MaxValue)
            {
                return;
            }

            var bSkipList2 = r.ReadBoolean(); // IsFixedTimeStep
            var u01 = r.ReadInt32();
            SamplePeriod = TimeInt32.FromMilliseconds(r.ReadInt32()); // SavedPeriod
            var u02 = r.ReadInt32();
            
            var sampleData = r.ReadBytes(); // CGameGhostTMData::ArchiveStateBuffer
            
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

            if (!bSkipList2)
            {
                sampleTimes = r.ReadArray<int>();
            }
            //

            if (numSamples > 0)
            {
                using var mssd = new MemoryStream(sampleData);
                ReadSamples(mssd, numSamples, sizePerSample, sampleSizes, sampleTimes);
            }
            else
            {
                Samples = new ObservableCollection<Sample>();
                Samples.CollectionChanged += Samples_CollectionChanged;
            }
        }

        public void ReadSamples(MemoryStream ms, int numSamples, int sizePerSample, int[]? sizesPerSample = null, int[]? sampleTimes = null)
        {
            Samples = new ObservableCollection<Sample>();
            Samples.CollectionChanged += Samples_CollectionChanged;

            using var r = new GameBoxReader(ms);

            for (var i = 0; i < numSamples; i++)
            {
                var sampleData = sizePerSample switch
                {
                    -1 => GetSampleDataFromDifferentSizes(r, numSamples, sizesPerSample, i),
                    _  => r.ReadBytes(sizePerSample)
                };

                var sample = new Sample(sampleData);

                using var bufferMs = new MemoryStream(sampleData);
                using var bufferR = new GameBoxReader(bufferMs);

                var time = sampleTimes?[i];

                switch (SavedMobilClassId)
                {
                    case 0x0A02B000: // CSceneVehicleCar
                        {
                            var (position, rotation, speed, velocity) = bufferR.ReadTransform();

                            sample.Position = position;
                            sample.Rotation = rotation;
                            sample.Speed = speed * 3.6f;
                            sample.Velocity = velocity;

                            break;
                        }
                    case 0x0A401000: // CSceneMobilCharVis
                        var bufferType = r.ReadByte();

                        switch (bufferType)
                        {
                            case 0:
                                {
                                    bufferMs.Position = 14;

                                    var (position, rotation, speed, velocity) = bufferR.ReadTransform();

                                    sample.Position = position;
                                    sample.Rotation = rotation;
                                    sample.Speed = speed * 3.6f;
                                    sample.Velocity = velocity;

                                    break;
                                }
                            case 1:
                                break;
                            default:
                                break;
                        }

                        sample.BufferType = bufferType;

                        break;
                    default:
                        sample.BufferType = null;
                        break;
                }

                Samples.Add(sample);

                sample.AssignTo(this);
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
        /// Linearly interpolates <see cref="NET.Sample.Position"/>, <see cref="NET.Sample.Rotation"/> (<see cref="NET.Sample.PitchYawRoll"/>),
        /// <see cref="NET.Sample.Speed"/> and <see cref="NET.Sample.Velocity"/> between two samples. Unknown data is taken from sample A.
        /// </summary>
        /// <param name="timestamp">Any timestamp between the range of samples.</param>
        /// <returns>A new instance of <see cref="Sample"/> that has been linearly interpolated (<see cref="NET.Sample.Timestamp"/> will be null)
        /// or a reference to an existing sample if <paramref name="timestamp"/> matches an existing sample timestamp.
        /// Also returns null if there are no samples, or if <paramref name="timestamp"/> is outside of the sample range,
        /// or <see cref="SamplePeriod"/> is lower or equal to 0.</returns>
        public Sample? GetSampleLerp(TimeSingle timestamp)
        {
            if (Samples is null || Samples.Count == 0 || samplePeriod.Ticks <= 0)
                return null;

            var sampleKey = timestamp.TotalMilliseconds / samplePeriod.TotalMilliseconds;
            var a = Samples.ElementAtOrDefault((int)Math.Floor(sampleKey)); // Sample A
            var b = Samples.ElementAtOrDefault((int)Math.Ceiling(sampleKey)); // Sample B

            if (a == null) // Timestamp is outside of the range
                return null;

            if (b == null || a == b) // There's no second sample to interpolate with
                return a;

            var t = (float)(sampleKey - Math.Floor(sampleKey));

            return new Sample(a.Data)
            {
                Position = AdditionalMath.Lerp(a.Position, b.Position, t),
                Rotation = AdditionalMath.Lerp(a.Rotation, b.Rotation, t),
                Speed = AdditionalMath.Lerp(a.Speed, b.Speed, t),
                Velocity = AdditionalMath.Lerp(a.Velocity, b.Velocity, t)
            };
        }
    }
}
