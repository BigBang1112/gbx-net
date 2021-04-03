using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Collections.Specialized;

namespace GBX.NET.Engines.Game
{
    public class CGameGhostData
    {
        private TimeSpan samplePeriod;

        /// <summary>
        /// How much time is between each sample.
        /// </summary>
        public TimeSpan SamplePeriod
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

        public ObservableCollection<CGameGhostDataSample> Samples { get; private set; }
        public CompressionLevel Compression { get; set; }
        public uint NodeID { get; set; }

        public CGameGhostData()
        {

        }

        private void Samples_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (CGameGhostDataSample sample in e.OldItems)
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
                using (var zlib = new CompressedStream(stream, CompressionMode.Decompress))
                {
                    Compression = zlib.Compression;
                    using (var r = new GameBoxReader(zlib))
                        Read(r);
                }
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
            using (var r = new GameBoxReader(stream))
                Read(r);
        }

        /// <summary>
        /// Read uncompressed ghost data with <see cref="GameBoxReader"/>.
        /// </summary>
        /// <param name="r">Reader.</param>
        public void Read(GameBoxReader r)
        {
            NodeID = r.ReadUInt32(); // CSceneVehicleCar or CSceneMobilCharVis
            if (NodeID != uint.MaxValue)
            {
                var bSkipList2 = r.ReadBoolean();
                var u01 = r.ReadInt32();
                SamplePeriod = TimeSpan.FromMilliseconds(r.ReadInt32());
                var u02 = r.ReadInt32();

                var sampleData = r.ReadBytes();

                int sizePerSample = -1;
                int[] sampleSizes = null;

                var numSamples = r.ReadInt32();
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

                int[] sampleTimes = null;

                if (!bSkipList2)
                {
                    sampleTimes = r.ReadArray<int>();
                }

                if (numSamples > 0)
                {
                    using (var mssd = new MemoryStream(sampleData))
                    {
                        ReadSamples(mssd, numSamples, sizePerSample, sampleSizes, sampleTimes);
                    }
                }
                else
                {
                    Samples = new ObservableCollection<CGameGhostDataSample>();
                    Samples.CollectionChanged += Samples_CollectionChanged;
                }
            }
        }

        public void ReadSamples(MemoryStream ms, int numSamples, int sizePerSample, int[] sizesPerSample = null, int[] sampleTimes = null)
        {
            Samples = new ObservableCollection<CGameGhostDataSample>();
            Samples.CollectionChanged += Samples_CollectionChanged;

            using (var r = new GameBoxReader(ms))
            {
                for (var i = 0; i < numSamples; i++)
                {
                    CGameGhostDataSample sample;

                    var sampleProgress = (int)ms.Position;

                    byte[] unknownData;
                    if (sizePerSample != -1)
                        unknownData = new byte[ms.Length / sizePerSample];
                    else if (sizesPerSample != null)
                    {
                        if (i == numSamples - 1) // Last sample size not included
                            unknownData = new byte[(int)(ms.Length - ms.Position)];
                        else
                            unknownData = new byte[sizesPerSample[i]];
                    }
                    else throw new Exception();

                    int? time = null;

                    if (sampleTimes != null)
                        time = sampleTimes[i];

                    switch (NodeID)
                    {
                        case 0x0A02B000: // CSceneVehicleCar
                            var transform02B = r.ReadTransform();

                            sample = new CGameGhostDataSample()
                            {
                                Position = transform02B.position,
                                Rotation = transform02B.rotation,
                                Speed = transform02B.speed * 3.6f,
                                Velocity = transform02B.velocity
                            };
                            break;
                        case 0x0A401000: // CSceneMobilCharVis
                            var bufferType = r.ReadByte();

                            switch (bufferType)
                            {
                                case 0:
                                    var unknownData401 = r.ReadBytes(14);
                                    Buffer.BlockCopy(unknownData401, 0, unknownData, 0, unknownData401.Length);

                                    var transform401 = r.ReadTransform();

                                    sample = new CGameGhostDataSample()
                                    {
                                        Position = transform401.position,
                                        Rotation = transform401.rotation,
                                        Speed = transform401.speed * 3.6f,
                                        Velocity = transform401.velocity
                                    };
                                    break;
                                case 1:
                                    sample = new CGameGhostDataSample();
                                    break;
                                default:
                                    sample = new CGameGhostDataSample();
                                    break;
                            }

                            sample.BufferType = bufferType;

                            break;
                        default:
                            sample = new CGameGhostDataSample()
                            {
                                BufferType = 255
                            };
                            break;
                    }

                    sampleProgress = (int)(ms.Position - sampleProgress);

                    if (sizePerSample != -1) // If the sample size is constant
                    {
                        var moreUnknownData = unknownData = r.ReadBytes(sizePerSample - sampleProgress);
                        Buffer.BlockCopy(moreUnknownData, 0, unknownData, sampleProgress, moreUnknownData.Length);
                    }
                    else if (sizesPerSample != null) // If sample sizes are different
                    {
                        var moreUnknownData = r.ReadBytes(unknownData.Length - sampleProgress);
                        Buffer.BlockCopy(moreUnknownData, 0, unknownData, sampleProgress, moreUnknownData.Length);
                    }
                    else throw new Exception();

                    sample.Unknown = unknownData;

                    Samples.Add(sample);

                    sample.AssignTo(this);
                }
            }
        }

        /// <summary>
        /// Linearly interpolates <see cref="Sample.Position"/>, <see cref="Sample.Rotation"/> (<see cref="Sample.PitchYawRoll"/>),
        /// <see cref="Sample.Speed"/> and <see cref="Sample.Velocity"/> between two samples. Unknown data is taken from sample A.
        /// </summary>
        /// <param name="timestamp">Any timestamp between the range of samples.</param>
        /// <returns>A new instance of <see cref="Sample"/> that has been linearly interpolated (<see cref="Sample.Timestamp"/> will be null)
        /// or a reference to an existing sample if <paramref name="timestamp"/> matches an existing sample timestamp.
        /// Also returns null if there are no samples, or if <paramref name="timestamp"/> is outside of the sample range,
        /// or <see cref="SamplePeriod"/> is lower or equal to 0.</returns>
        public Sample GetSampleLerp(TimeSpan timestamp)
        {
            if (Samples?.Count > 0 && samplePeriod.Ticks > 0)
            {
                var sampleKey = timestamp.TotalMilliseconds / samplePeriod.TotalMilliseconds;
                var a = Samples.ElementAtOrDefault((int)Math.Floor(sampleKey)); // Sample A
                var b = Samples.ElementAtOrDefault((int)Math.Ceiling(sampleKey)); // Sample B

                if (a == null) // Timestamp is outside of the range
                    return null;

                if (b == null || a == b) // There's no second sample to interpolate with
                    return a;

                var t = (float)(sampleKey - Math.Floor(sampleKey));

                return new Sample()
                {
                    Position = AdditionalMath.Lerp(a.Position, b.Position, t),
                    Rotation = AdditionalMath.Lerp(a.Rotation, b.Rotation, t),
                    Speed = AdditionalMath.Lerp(a.Speed, b.Speed, t),
                    Velocity = AdditionalMath.Lerp(a.Velocity, b.Velocity, t),
                    Unknown = a.Unknown
                };
            }

            return null;
        }

        public class CGameGhostDataSample : Sample
        {
            private CGameGhostData owner;

            internal void AssignTo(CGameGhostData ghostData)
            {
                owner = ghostData;

                if (owner == null || owner.samplePeriod == null || owner.samplePeriod.TotalMilliseconds <= 0)
                {
                    Timestamp = null;
                    return;
                }

                UpdateTimestamp();
            }

            internal void UpdateTimestamp()
            {
                Timestamp = TimeSpan.FromMilliseconds(owner.samplePeriod.TotalMilliseconds * owner.Samples.IndexOf(this));
            }
        }
    }
}
