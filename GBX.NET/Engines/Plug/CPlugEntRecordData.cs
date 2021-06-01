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
        public Task<ObservableCollection<Sample>> Samples { get; private set; }

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

                n.Samples = Task.Run(() =>
                {
                    var samples = new ObservableCollection<Sample>();

                    using (var ms = new MemoryStream(Data))
                    using (var cs = new CompressedStream(ms, CompressionMode.Decompress))
                    using (var gbxr = new GameBoxReader(cs))
                    {
                        var u01 = gbxr.ReadInt32();
                        var ghostLength = gbxr.ReadInt32(); // milliseconds
                        var objects = gbxr.ReadArray<object>(r1 =>
                        {
                            var nodeId = r1.ReadUInt32();
                            Names.TryGetValue(nodeId, out string nodeName);

                            return new
                            {
                                nodeId,
                                nodeName,
                                obj_u01 = r1.ReadInt32(),
                                obj_u02 = r1.ReadInt32(),
                                obj_u03 = r1.ReadInt32(),
                                mwbuffer = r1.ReadInt32(),
                                obj_u05 = r1.ReadInt32()
                            };
                        });

                        if (Version >= 2)
                        {
                            var objcts2 = gbxr.ReadArray<object>(r1 =>
                            {
                                var u02 = r1.ReadInt32();
                                var u03 = r1.ReadInt32();

                                uint? clas = null;
                                string clasName = null;
                                if (Version >= 4)
                                {
                                    clas = r1.ReadUInt32();
                                    Names.TryGetValue(clas.Value, out clasName);
                                }

                                return new
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
                            var bufferType = gbxr.ReadInt32();
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
                                var buffer = gbxr.ReadBytes(); // MwBuffer

                                if (buffer.Length > 0)
                                {
                                    var unknownData = new byte[buffer.Length];

                                    using (var bufMs = new MemoryStream(buffer))
                                    using (var bufR = new GameBoxReader(bufMs))
                                    {
                                        var sampleProgress = (int)bufMs.Position;

                                        Sample sample = new Sample()
                                        {
                                            BufferType = (byte)bufferType
                                        };

                                        switch (bufferType)
                                        {
                                            case 0:
                                                break;
                                            case 2:
                                                var buf2unknownData = bufR.ReadBytes(5);
                                                Buffer.BlockCopy(buf2unknownData, 0, unknownData, 0, buf2unknownData.Length);

                                                var (position, rotation, speed, velocity) = bufR.ReadTransform(); // Only position matches

                                                sample.Timestamp = TimeSpan.FromMilliseconds(timestamp);
                                                sample.Position = position;
                                                sample.Rotation = rotation;
                                                sample.Speed = speed * 3.6f;
                                                sample.Velocity = velocity;

                                                break;
                                            case 4:
                                                var buf4unknownData = bufR.ReadBytes(47);
                                                Buffer.BlockCopy(buf4unknownData, 0, unknownData, 0, buf4unknownData.Length);

                                                var buf4transform = bufR.ReadTransform();

                                                var buf4unknownData2 = bufR.ReadBytes(4);

                                                sample.Timestamp = TimeSpan.FromMilliseconds(timestamp);
                                                sample.Position = buf4transform.position;
                                                sample.Rotation = buf4transform.rotation;
                                                sample.Speed = buf4transform.speed * 3.6f;
                                                sample.Velocity = buf4transform.velocity;
                                                sample.Unknown = buf4unknownData;

                                                break;
                                            case 10:
                                                break;
                                            default:
                                                break;
                                        }

                                        sampleProgress = (int)(bufMs.Position - sampleProgress);

                                        var moreUnknownData = bufR.ReadBytes((int)bufMs.Length - sampleProgress);
                                        Buffer.BlockCopy(moreUnknownData, 0, unknownData, sampleProgress, moreUnknownData.Length);

                                        sample.Unknown = unknownData;

                                        samples.Add(sample);
                                    }
                                }
                            }

                            u04 = gbxr.ReadByte();

                            if (Version >= 2)
                            {
                                for (byte x; (x = gbxr.ReadByte()) != 0;)
                                {
                                    var type = gbxr.ReadInt32();
                                    var timestamp = gbxr.ReadInt32();
                                    var buffer = gbxr.ReadBytes(); // MwBuffer
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

                    return samples;
                });
            }

            public override void Write(CPlugEntRecordData n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);
                w.Write(UncompressedSize);
                w.Write(CompressedSize);
                w.Write(Data, 0, Data.Length);
            }
        }
    }
}
