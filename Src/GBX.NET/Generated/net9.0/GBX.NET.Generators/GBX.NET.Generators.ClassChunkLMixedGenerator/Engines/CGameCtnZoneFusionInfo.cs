namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03151000</remarks>
[Class(0x03151000)]
public partial class CGameCtnZoneFusionInfo : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03151000;




    private string? compatibleZoneId;
    [AppliedWithChunk<Chunk03151000>]
    public string? CompatibleZoneId { get => compatibleZoneId; set => compatibleZoneId = value; }

    private int fusionType;
    [AppliedWithChunk<Chunk03151000>]
    public int FusionType { get => fusionType; set => fusionType = value; }

    private string? mergedZoneId;
    [AppliedWithChunk<Chunk03151001>]
    public string? MergedZoneId { get => mergedZoneId; set => mergedZoneId = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnZoneFusionInfo"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnZoneFusionInfo() { }


    /// <summary>
    /// CGameCtnZoneFusionInfo 0x000 chunk
    /// </summary>
    [Chunk(0x03151000)]
    public partial class Chunk03151000 : Chunk<CGameCtnZoneFusionInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x03151000;


        public override void ReadWrite(CGameCtnZoneFusionInfo n, GbxReaderWriter rw)
        {
            rw.Id(ref n.compatibleZoneId);
            rw.Int32(ref n.fusionType);
        }
    }

    /// <summary>
    /// CGameCtnZoneFusionInfo 0x001 chunk
    /// </summary>
    [Chunk(0x03151001)]
    public partial class Chunk03151001 : Chunk<CGameCtnZoneFusionInfo>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03151001;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnZoneFusionInfo n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref n.mergedZoneId);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03151000 => new Chunk03151000(),
        0x03151001 => new Chunk03151001(),
        _ => base.NewChunk(chunkId),
    };
}
