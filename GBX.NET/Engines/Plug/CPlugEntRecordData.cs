using GBX.NET.Engines.Game;
using System;
using System.Collections.Generic;
using System.IO;
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

                using (var ms = new MemoryStream(Data))
                using (var cs = new CompressedStream(ms, System.IO.Compression.CompressionMode.Decompress))
                using (var gbxr = new GameBoxReader(cs))
                {
                    var u01 = gbxr.ReadInt32();
                    var numSamples = gbxr.ReadInt32();
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
                    //var gsdg = new MemoryStream();
                    //cs.CopyTo(gsdg);
                    var bye = gbxr.ReadByte();
                    var gdsgdsg = gbxr.ReadArray<int>(4);
                    if(Version >= 6)
                    {
                        //var wtf = gbxr.ReadUInt32();
                        // close
                    }
                    var byfase = gbxr.ReadByte();
                    var bywhafase = gbxr.ReadByte();
                    if(Version >= 2)
                    {
                        var fasbywhafase = gbxr.ReadByte();
                        var gdgassgdasgasgasgsg = gbxr.ReadArray<int>(3);
                        var fafsafassbywhafase = gbxr.ReadByte();

                        if (Version >= 3)
                        {
                            var byfagsagsse = gbxr.ReadByte();

                            if (Version == 7)
                            {

                            }

                            if(Version >= 8)
                            {
                                var gdsgfsadagdsg = gbxr.ReadInt32();
                            }
                        }
                    }
                    
                    var gdsggsadsg = gbxr.ReadArray<int>(5);
                }
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
