using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
