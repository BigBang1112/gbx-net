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

        public void Read(MemoryStream ms)
        {
            var magic = new byte[2];
            ms.Read(magic, 0, 2); // Needed for DeflateStream to work

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

            using (var zlib = new DeflateStream(ms, CompressionMode.Decompress))
            using (var gbxr = new GameBoxReader(zlib))
            {
                var classID = gbxr.ReadInt32(); // CSceneVehicleCar
                if (classID != -1)
                {
                    var bSkipList2 = gbxr.ReadBoolean();
                    var u01 = gbxr.ReadInt32();
                    var samplePeriod = gbxr.ReadInt32();
                    var u02 = gbxr.ReadInt32();

                    var size = gbxr.ReadInt32();
                    var sampleData = gbxr.ReadBytes(size);

                    int? sizePerSample = null;

                    var numSamples = gbxr.ReadInt32();
                    if (numSamples > 0)
                    {
                        var firstSampleOffset = gbxr.ReadInt32();
                        if (numSamples > 1)
                        {
                            sizePerSample = gbxr.ReadInt32();
                            if (sizePerSample == -1)
                            {
                                var sampleSizes = gbxr.ReadArray<int>(numSamples - 1);
                                throw new NotSupportedException("Ghost with different sample sizes aren't supported.");
                            }
                        }
                    }

                    if (!bSkipList2)
                    {
                        var num = gbxr.ReadInt32();
                        var sampleTimes = gbxr.ReadArray<int>(num);
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
