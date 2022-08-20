namespace GBX.NET.Engines.Game;

[Node(0x0315C000)]
public class CGameCtnBlockInfoVariantGround : CGameCtnBlockInfoVariant
{
    #region Enums

    public enum EAutoTerrainPlaceType
    {
        Auto,
        Force,
        DoNotPlace,
        DoNotDestroy
    }

    #endregion

    #region Fields

    private CGameCtnAutoTerrain?[]? autoTerrains;
    private int autoTerrainHeightOffset;
    private EAutoTerrainPlaceType autoTerrainPlaceType;

    #endregion

    #region Properties

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0315C001))]
    public CGameCtnAutoTerrain?[]? AutoTerrains { get => autoTerrains; set => autoTerrains = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0315C001))]
    public int AutoTerrainHeightOffset { get => autoTerrainHeightOffset; set => autoTerrainHeightOffset = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0315C001))]
    public EAutoTerrainPlaceType AutoTerrainPlaceType { get => autoTerrainPlaceType; set => autoTerrainPlaceType = value; }

    #endregion

    #region Constructors

    protected CGameCtnBlockInfoVariantGround()
    {

    }

    #endregion

    #region Chunks

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnBlockInfoVariantGround 0x001 chunk
    /// </summary>
    [Chunk(0x0315C001)]
    public class Chunk0315C001 : Chunk<CGameCtnBlockInfoVariantGround>, IVersionable
    {
        private int version;
        private int listVersion = 10;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockInfoVariantGround n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Int32(ref listVersion);
            rw.ArrayNode<CGameCtnAutoTerrain>(ref n.autoTerrains);
            rw.Int32(ref n.autoTerrainHeightOffset);
            rw.EnumInt32<EAutoTerrainPlaceType>(ref n.autoTerrainPlaceType);
        }
    }

    #endregion

    #endregion
}
