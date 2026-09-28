namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0315C000</remarks>
[Class(0x0315C000)]
public partial class CGameCtnBlockInfoVariantGround : CGameCtnBlockInfoVariant, IClass
{
    [Hexadecimal] public static new uint Id => 0x0315C000;




    private CGameCtnAutoTerrain[]? autoTerrains;
    [AppliedWithChunk<Chunk0315C001>]
    public CGameCtnAutoTerrain[]? AutoTerrains { get => autoTerrains; set => autoTerrains = value; }

    private int autoTerrainHeightOffset;
    [AppliedWithChunk<Chunk0315C001>]
    public int AutoTerrainHeightOffset { get => autoTerrainHeightOffset; set => autoTerrainHeightOffset = value; }

    private EAutoTerrainPlaceType autoTerrainPlaceType;
    [AppliedWithChunk<Chunk0315C001>]
    public EAutoTerrainPlaceType AutoTerrainPlaceType { get => autoTerrainPlaceType; set => autoTerrainPlaceType = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnBlockInfoVariantGround"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnBlockInfoVariantGround() { }


    /// <summary>
    /// CGameCtnBlockInfoVariantGround 0x001 chunk
    /// </summary>
    [Chunk(0x0315C001)]
    [ChunkGameVersion(GameVersion.TMT | GameVersion.MP4, 0, 2)]
    public partial class Chunk0315C001 : Chunk<CGameCtnBlockInfoVariantGround>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0315C001;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMT | GameVersion.MP4;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnBlockInfoVariantGround n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayNodeRef_deprec<CGameCtnAutoTerrain>(ref n.autoTerrains!);
            rw.Int32(ref n.autoTerrainHeightOffset);
            rw.EnumInt32<EAutoTerrainPlaceType>(ref n.autoTerrainPlaceType);
        }
    }



    public enum EAutoTerrainPlaceType
    {
        Auto,
        Force,
        DoNotPlace,
        DoNotDestroy,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0315C001 => new Chunk0315C001(),
        _ => base.NewChunk(chunkId),
    };
}
