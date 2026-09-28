namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03340000</remarks>
[Class(0x03340000)]
public partial class CGameCtnBlockInfoClipVertical : CGameCtnBlockInfoClip, IClass
{
    [Hexadecimal] public static new uint Id => 0x03340000;




    private string? verticalClipGroupId;
    [AppliedWithChunk<Chunk03340000>]
    public string? VerticalClipGroupId { get => verticalClipGroupId; set => verticalClipGroupId = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnBlockInfoClipVertical"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnBlockInfoClipVertical() { }


    /// <summary>
    /// CGameCtnBlockInfoClipVertical 0x000 chunk
    /// </summary>
    [Chunk(0x03340000)]
    public partial class Chunk03340000 : Chunk<CGameCtnBlockInfoClipVertical>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03340000;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnBlockInfoClipVertical n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref n.verticalClipGroupId);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03340000 => new Chunk03340000(),
        _ => base.NewChunk(chunkId),
    };
}
