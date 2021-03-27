using GBX.NET.Engines.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace GBX.NET.Engines.Plug
{
    [Node(0x0911F000)]
    public class CPlugEntRecordData : Node
    {
        public ObservableCollection<GBX.NET.Sample> Samples { get; private set; }

        [Chunk(0x0911F000)]
        public class Chunk000 : Chunk<CPlugEntRecordData>
        {
            public int Version { get; set; }
            public int CompressedSize { get; private set; }
            public int UncompressedSize { get; private set; }
            public byte[] Data { get; private set; }

            public override void Read(CPlugEntRecordData n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();
                UncompressedSize = r.ReadInt32();
                CompressedSize = r.ReadInt32();
                Data = r.ReadBytes(CompressedSize);

                n.Samples = new ObservableCollection<Sample>();

                // ... WIP ...

                Task.Run(() =>
                {
                    using (var ms = new MemoryStream(Data))
                    using (var cs = new CompressedStream(ms, CompressionMode.Decompress))
                    using (var gbxr = new GameBoxReader(cs))
                    {
                        var u01 = gbxr.ReadInt32();
                        var ghostLength = gbxr.ReadInt32(); // milliseconds
                        var objects = gbxr.ReadArray<object>(i =>
                        {
                            var nodeId = gbxr.ReadUInt32();
                            Names.TryGetValue(nodeId, out string nodeName);

                            var obj_u01 = gbxr.ReadInt32();
                            var obj_u02 = gbxr.ReadInt32();
                            var obj_u03 = gbxr.ReadInt32();
                            var obj_mwbuffer = gbxr.ReadBytes();
                            var obj_u05 = gbxr.ReadInt32();

                            return new
                            {
                                id = nodeId,
                                name = nodeName,
                                rest = new object[] {
                                    obj_u01, obj_u02, obj_u03, obj_mwbuffer, obj_u05
                                }
                            };
                        });

                        if (Version >= 2)
                        {
                            var objcts2 = gbxr.ReadArray<object>(i =>
                            {
                                var u02 = gbxr.ReadInt32();
                                var u03 = gbxr.ReadInt32();

                                uint? clas = null;
                                string clasName = null;
                                if (Version >= 4)
                                {
                                    clas = gbxr.ReadUInt32();
                                    Names.TryGetValue(clas.Value, out clasName);
                                }

                                return new object[]
                                {
                                    u02,
                                    u03,
                                    clas,
                                    clasName
                                };
                            });
                        }

                        var u04 = gbxr.ReadByte();
                        while (u04 != 0)
                        {
                            var u05 = gbxr.ReadInt32();
                            var u06 = gbxr.ReadInt32();
                            var u07 = gbxr.ReadInt32();
                            var ghostLengthFinish = gbxr.ReadInt32(); // ms

                            if (Version < 6)
                            {
                                // temp_79f24995b2b->field_0x28 = temp_79f24995b2b->field_0xc
                            }
                            else
                            {
                                var u08 = gbxr.ReadInt32();
                            }

                            // Reads byte on every loop until the byte is 0, should be 1 otherwise
                            for (byte x; (x = gbxr.ReadByte()) != 0;)
                            {
                                var timestamp = gbxr.ReadInt32();
                                var u12 = gbxr.ReadBytes(); // MwBuffer

                                // There's bound to be a better way to detect this buffer
                                // but the size is unique enough for now.
                                if (u12.Length == 162)
                                {
                                    var sample = ReadSample(u12);
                                    sample.Timestamp = TimeSpan.FromMilliseconds(timestamp);
                                    n.Samples.Add(sample);
                                }
                            }

                            u04 = gbxr.ReadByte();

                            if (Version >= 2)
                            {
                                for (byte x; (x = gbxr.ReadByte()) != 0;)
                                {
                                    var u15 = gbxr.ReadInt32();
                                    var u16 = gbxr.ReadInt32();
                                    var u17 = gbxr.ReadBytes(); // MwBuffer
                                }
                            }
                        }

                        if (Version >= 3)
                        {
                            for (byte x; (x = gbxr.ReadByte()) != 0;)
                            {
                                var u19 = gbxr.ReadInt32();
                                var u20 = gbxr.ReadInt32();
                                var u21 = gbxr.ReadBytes(); // MwBuffer
                            }

                            if (Version == 7)
                            {
                                for (byte x; (x = gbxr.ReadByte()) != 0;)
                                {
                                    var u23 = gbxr.ReadInt32();
                                    var u24 = gbxr.ReadBytes(); // MwBuffer
                                }
                            }

                            if (Version >= 8)
                            {
                                var u23 = gbxr.ReadInt32();

                                if (u23 != 0)
                                {
                                    if (Version == 8)
                                    {
                                        for (byte x; (x = gbxr.ReadByte()) != 0;)
                                        {
                                            var u25 = gbxr.ReadInt32();
                                            var u26 = gbxr.ReadBytes(); // MwBuffer
                                        }
                                    }
                                    else
                                    {
                                        for (byte x; (x = gbxr.ReadByte()) != 0;)
                                        {
                                            var u28 = gbxr.ReadInt32();
                                            var u29 = gbxr.ReadBytes(); // MwBuffer
                                            var u30 = gbxr.ReadBytes(); // MwBuffer
                                        }

                                        if (Version >= 10)
                                        {
                                            var period = gbxr.ReadInt32();
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
            }

            public override void Write(CPlugEntRecordData n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);
                w.Write(UncompressedSize);
                w.Write(CompressedSize);
                w.Write(Data, 0, Data.Length);
            }

            private Sample ReadSample(byte[] u12)
            {
                using (var bufMs = new MemoryStream(u12))
                using (var bufR = new GameBoxReader(bufMs))
                {
                    bufMs.Seek(47, SeekOrigin.Begin); // vec3 starts at offset 47

                    var pos = bufR.ReadVec3();
                    var angle = bufR.ReadUInt16() / (double)ushort.MaxValue * Math.PI;
                    var axisHeading = bufR.ReadInt16() / (double)short.MaxValue * Math.PI;
                    var axisPitch = bufR.ReadInt16() / (double)short.MaxValue * Math.PI / 2;
                    var speed = (float)Math.Exp(bufR.ReadInt16() / 1000f);
                    var velocityHeading = bufR.ReadSByte() / (double)sbyte.MaxValue * Math.PI;
                    var velocityPitch = bufR.ReadSByte() / (double)sbyte.MaxValue * Math.PI / 2;

                    var axis = new Vec3((float)(Math.Sin(angle) * Math.Cos(axisPitch) * Math.Cos(axisHeading)),
                        (float)(Math.Sin(angle) * Math.Cos(axisPitch) * Math.Sin(axisHeading)),
                        (float)(Math.Sin(angle) * Math.Sin(axisPitch)));

                    var quaternion = new Quaternion(axis, (float)Math.Cos(angle));

                    var velocityVector = new Vec3((float)(speed * Math.Cos(velocityPitch) * Math.Cos(velocityHeading)),
                        (float)(speed * Math.Cos(velocityPitch) * Math.Sin(velocityHeading)),
                        (float)(speed * Math.Sin(velocityPitch)));

                    var unknownData = bufR.ReadBytes(4);

                    return new Sample()
                    {
                        Position = pos,
                        Rotation = quaternion,
                        Speed = speed * 3.6f,
                        Velocity = velocityVector,
                        Unknown = unknownData
                    };
                }
            }
        }
    }
}
