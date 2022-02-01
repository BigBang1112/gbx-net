namespace GBX.NET.Engines.Game;

[Node(0x0315C000)]
public class CGameCtnBlockInfoVariantGround : CGameCtnBlockInfoVariant
{
    private CGameCtnAutoTerrain?[]? autoTerrains;

    public CGameCtnAutoTerrain?[]? AutoTerrains
    {
        get => autoTerrains;
        set => autoTerrains = value;
    }

    protected CGameCtnBlockInfoVariantGround()
    {

    }

    [Chunk(0x0315C001)]
    public class Chunk0315C001 : Chunk<CGameCtnBlockInfoVariantGround>
    {
        public override void ReadWrite(CGameCtnBlockInfoVariantGround n, GameBoxReaderWriter rw)
        {
            rw.Int32();
            rw.Int32();
            rw.ArrayNode(ref n.autoTerrains);
            rw.Int32();
            rw.Int32();
        }
    }
}
