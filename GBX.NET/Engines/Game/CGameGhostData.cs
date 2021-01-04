using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Engines.Game
{
    public class CGameGhostData
    {
        public Sample[] Samples { get; set; }
        public CompressionLevel Compression { get; set; }

        public CGameGhostData()
        {

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
                var magic = new byte[2];
                stream.Read(magic, 0, 2); // Needed for DeflateStream to work

                if (magic[0] != 0x78)
                    throw new Exception("Data isn't compressed with Deflate ZLIB");

                if (Enumerable.SequenceEqual(magic, new byte[] { 0x78, 0x01 }))
                {
                    Compression = CompressionLevel.NoCompression;
                    Debug.WriteLine("Deflate ZLIB - No compression");
                }
                else if (Enumerable.SequenceEqual(magic, new byte[] { 0x78, 0x9C }))
                {
                    Compression = CompressionLevel.DefaultCompression;
                    Debug.WriteLine("Deflate ZLIB - Default compression");
                }
                else if (Enumerable.SequenceEqual(magic, new byte[] { 0x78, 0xDA }))
                {
                    Compression = CompressionLevel.BestCompression;
                    Debug.WriteLine("Deflate ZLIB - Best compression");
                }
                else
                {
                    Compression = CompressionLevel.UnknownCompression;
                    Debug.WriteLine("Deflate ZLIB - Unknown compression");
                }

                using (var zlib = new DeflateStream(stream, CompressionMode.Decompress))
                using (var r = new GameBoxReader(zlib))
                    Read(r);
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
                var samplePeriod = r.ReadInt32();
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

                Samples = new Sample[numSamples];

                if (numSamples > 0)
                {
                    if (sizePerSample.HasValue)
                    {
                        using (var mssd = new MemoryStream(sampleData))
                        using (var sr = new GameBoxReader(mssd))
                        {
                            for (var i = 0; i < numSamples; i++)
                            {
                                var sampleProgress = mssd.Position;

                                var timestamp = TimeSpan.FromMilliseconds(samplePeriod * i);

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
                                    sizePerSample.Value - (int)(mssd.Position - sampleProgress));

                                Samples[i] = new Sample()
                                {
                                    Timestamp = timestamp,
                                    Position = pos,
                                    Rotation = quaternion,
                                    Speed = speed,
                                    Velocity = velocityVector,
                                    Unknown = unknownData
                                };
                            }
                        }
                    }
                }
            }
        }

        public class Sample
        {
            public TimeSpan Timestamp { get; set; }
            public Vec3 Position { get; set; }
            public Quaternion Rotation { get; set; }
            public Vec3 PitchYawRoll => Rotation.ToPitchYawRoll();
            public float Speed { get; set; }
            public Vec3 Velocity { get; set; }
            public byte[] Unknown { get; set; }

            public override string ToString() => $"Sample: {Timestamp:mm':'ss'.'fff} {Position}";
        }
    }
}
