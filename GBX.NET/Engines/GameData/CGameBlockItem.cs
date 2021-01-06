using System;
using System.Collections.Generic;
using System.Text;
using GBX.NET.Engines.Plug;

namespace GBX.NET.Engines.GameData
{
    [Node(0x2E025000)]
    public class CGameBlockItem : Node
    {
        public string ArchetypeBlockInfoId { get; set; }
        public CPlugCrystal[] Crystals { get; set; }

        [Chunk(0x2E025000)]
        public class Chunk2E025000 : Chunk<CGameBlockItem>
        {
            public int Version { get; set; }
            public int Unknown1 { get; set; }

            public override void ReadWrite(CGameBlockItem n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                n.ArchetypeBlockInfoId = rw.Id(n.ArchetypeBlockInfoId);
                Unknown1 = rw.Int32(Unknown1);
                n.Crystals = rw.Array(n.Crystals, i =>
                {
                    var a = rw.Reader.ReadInt32();
                    return rw.Reader.ReadNodeRef<CPlugCrystal>();
                },
                x =>
                {
                    rw.Writer.Write(1000000);
                    rw.Writer.Write(x);
                });
            }
        }
    }
}
