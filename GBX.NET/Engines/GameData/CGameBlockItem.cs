using System;
using System.Collections.Generic;
using System.Text;
using GBX.NET.Engines.Plug;

namespace GBX.NET.Engines.GameData
{
    [Node(0x2E025000)]
    public class CGameBlockItem : Node
    {
        public CGameBlockItem(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x2E025000)]
        public class Chunk000 : Chunk
        {
            public int Version { get; set; }
            public string ArchetypeBlockInfoId { get; set; }
            public int Unknown1 { get; set; }
            public CPlugCrystal[] Crystals { get; set; }

            public Chunk000(Node node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                ArchetypeBlockInfoId = rw.LookbackString(ArchetypeBlockInfoId);
                Unknown1 = rw.Int32(Unknown1);
                Crystals = rw.Array(Crystals, i =>
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
