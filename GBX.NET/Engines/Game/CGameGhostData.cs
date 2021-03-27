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
            var classID = r.ReadInt32(); // CSceneVehicleCar
            if (classID != -1)
            {
                var bSkipList2 = r.ReadBoolean();
                var u01 = r.ReadInt32();
                SamplePeriod = TimeSpan.FromMilliseconds(r.ReadInt32());
                var u02 = r.ReadInt32();

                var size = r.ReadInt32();
                var sampleData = r.ReadBytes(size);

                int? sizePerSample = null;

                var numSamples = r.ReadInt32();
                if (numSamples > 0)
                {
                    var firstSampleOffset = r.ReadInt32();
                    if (numSamples > 1)
                    {
                        sizePerSample = r.ReadInt32();
                        if (sizePerSample == -1)
                        {
                            var sampleSizes = r.ReadArray<int>(numSamples - 1);
                            throw new NotSupportedException("Ghosts with different sample sizes aren't supported.");
                        }
                    }
                }

                if (!bSkipList2)
                {
                    var num = r.ReadInt32();
                    var sampleTimes = r.ReadArray<int>(num);
                }

                if (numSamples > 0)
                {
                    if (sizePerSample.HasValue)
                    {
                        using (var mssd = new MemoryStream(sampleData))
                        {
                            ReadSamples(mssd, numSamples, sizePerSample.Value);
                        }
                    }
                }
                else
                {
                    Samples = new ObservableCollection<CGameGhostDataSample>();
                    Samples.CollectionChanged += Samples_CollectionChanged;
                }
            }
        }

        public void ReadSamples(MemoryStream ms, int numSamples, int sizePerSample)
        {
            Samples = new ObservableCollection<CGameGhostDataSample>();
            Samples.CollectionChanged += Samples_CollectionChanged;

            using (var sr = new GameBoxReader(ms))
            {
                for (var i = 0; i < numSamples; i++)
                {
                    var sampleProgress = ms.Position;

                    var pos = sr.ReadVec3();
                    var angle = sr.ReadUInt16() / (double)ushort.MaxValue * Math.PI;
                    var axisHeading = sr.ReadInt16() / (double)short.MaxValue * Math.PI;
                    var axisPitch = sr.ReadInt16() / (double)short.MaxValue * Math.PI / 2;
                    var speed = (float)Math.Exp(sr.ReadInt16() / 1000f);
                    var velocityHeading = sr.ReadSByte() / (double)sbyte.MaxValue * Math.PI;
                    var velocityPitch = sr.ReadSByte() / (double)sbyte.MaxValue * Math.PI / 2;

                    var axis = new Vec3((float)(Math.Sin(angle) * Math.Cos(axisPitch) * Math.Cos(axisHeading)),
                        (float)(Math.Sin(angle) * Math.Cos(axisPitch) * Math.Sin(axisHeading)),
                        (float)(Math.Sin(angle) * Math.Sin(axisPitch)));

                    var quaternion = new Quaternion(axis, (float)Math.Cos(angle));

                    var velocityVector = new Vec3((float)(speed * Math.Cos(velocityPitch) * Math.Cos(velocityHeading)),
                        (float)(speed * Math.Cos(velocityPitch) * Math.Sin(velocityHeading)),
                        (float)(speed * Math.Sin(velocityPitch)));

                    var unknownData = sr.ReadBytes(
                        sizePerSample - (int)(ms.Position - sampleProgress));

                    var sample = new CGameGhostDataSample()
                    {
                        Position = pos,
                        Rotation = quaternion,
                        Speed = speed * 3.6f,
                        Velocity = velocityVector,
                        Unknown = unknownData
                    };

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

            public override string ToString()
            {
                if (Timestamp.HasValue)
                    return $"Sample: {Timestamp.Value.ToStringTM()} {Position}";
                return $"Sample: {Position}";
            }
        }
    }
}
