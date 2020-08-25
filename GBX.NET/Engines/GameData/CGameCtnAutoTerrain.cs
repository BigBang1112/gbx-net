using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.GameData
{
    [Node(0x03120000)]
    public class CGameCtnAutoTerrain : Node
    {
        public CGameCtnAutoTerrain(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x03120001)]
        public class Chunk001 : Chunk
        {
            public Chunk001(CGameCtnAutoTerrain node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                var offset = r.ReadInt3();
                var genealogy = r.ReadNodeRef();
            }
        }
    }
}
