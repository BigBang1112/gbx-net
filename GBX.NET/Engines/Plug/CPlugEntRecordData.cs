using GBX.NET.Engines.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Engines.Plug
{
    [Node(0x0911F000)]
    public class CPlugEntRecordData : Node
    {
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

                            return new
                            {
                                id = nodeId,
                                name = nodeName,
                                rest = gbxr.ReadArray<int>(5)
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

                            var u09 = gbxr.ReadByte();
                            while (u09 != 0)
                            {
                                var u10 = gbxr.ReadInt32();
                                var u11 = gbxr.ReadInt32();
                                var u12 = gbxr.ReadInt32(); // Archive DoNat32 + DoData
                                u09 = gbxr.ReadByte();
                            }

                            u04 = gbxr.ReadByte(); // Probably
                            if (Version >= 2)
                            {
                                var u14 = gbxr.ReadByte();
                                while (u14 != 0)
                                {
                                    var u15 = gbxr.ReadInt32();
                                    var u16 = gbxr.ReadInt32();
                                    var u17 = gbxr.ReadInt32();
                                    u14 = gbxr.ReadByte();
                                }
                            }
                        }

                        if (Version >= 3)
                        {
                            var u18 = gbxr.ReadByte();
                            while (u18 != 0)
                            {
                                var u19 = gbxr.ReadInt32();
                                var u20 = gbxr.ReadInt32();
                                var u21 = gbxr.ReadInt32();
                                u18 = gbxr.ReadByte();
                            }

                            if (Version == 7)
                            {
                                var u22 = gbxr.ReadByte();
                                while (u22 != 0)
                                {
                                    var u23 = gbxr.ReadInt32();
                                    var u24 = gbxr.ReadInt32();
                                    u22 = gbxr.ReadByte();
                                }
                            }

                            if (Version >= 8)
                            {
                                var u23 = gbxr.ReadInt32();

                                if (Version == 8)
                                {
                                    var u24 = gbxr.ReadByte();
                                    while (u24 != 0)
                                    {
                                        var u25 = gbxr.ReadInt32();
                                        var u26 = gbxr.ReadInt32();
                                        u24 = gbxr.ReadByte();
                                    }
                                }
                                else
                                {
                                    var u27 = gbxr.ReadByte();
                                    while (u27 != 0)
                                    {
                                        var u28 = gbxr.ReadInt32();
                                        var u29 = gbxr.ReadInt32();
                                        var u30 = gbxr.ReadInt32();
                                        var wat = gbxr.ReadInt32();
                                        u27 = gbxr.ReadByte();
                                    }

                                    if (Version >= 10)
                                    {
                                        var period = gbxr.ReadInt32();
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
        }
    }
}
