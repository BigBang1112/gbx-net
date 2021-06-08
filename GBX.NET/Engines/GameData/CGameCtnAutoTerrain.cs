using GBX.NET.Engines.Game;
using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.GameData
{
    [Node(0x03120000)]
    public class CGameCtnAutoTerrain : CMwNod
    {
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
