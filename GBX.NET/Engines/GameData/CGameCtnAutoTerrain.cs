using GBX.NET.Engines.Game;
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
        public class Chunk03120001 : Chunk<CGameCtnAutoTerrain>
        {
            public override void Read(CGameCtnAutoTerrain n, GameBoxReader r, GameBoxWriter unknownW)
            {
                var offset = r.ReadInt3();
                var genealogy = r.ReadNodeRef<CGameCtnZoneGenealogy>();
            }
        }
    }
}
