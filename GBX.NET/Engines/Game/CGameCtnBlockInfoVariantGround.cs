using GBX.NET.Engines.GameData;

namespace GBX.NET.Engines.Game
{
    [Node(0x0315C000)]
    public class CGameCtnBlockInfoVariantGround : CGameCtnBlockInfoVariant
    {
        public CGameCtnAutoTerrain[] AutoTerrains { get; set; }

        [Chunk(0x0315C001)]
        public class Chunk0315C001 : Chunk<CGameCtnBlockInfoVariantGround>
        {
            public override void ReadWrite(CGameCtnBlockInfoVariantGround n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                n.AutoTerrains = rw.Array(n.AutoTerrains,
                    i => rw.Reader.ReadNodeRef<CGameCtnAutoTerrain>(),
                    x => rw.Writer.Write(x));
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }
    }
}
