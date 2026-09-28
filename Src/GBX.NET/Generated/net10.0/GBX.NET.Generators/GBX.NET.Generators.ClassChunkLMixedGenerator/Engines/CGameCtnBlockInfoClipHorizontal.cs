namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0335B000</remarks>
[Class(0x0335B000)]
public partial class CGameCtnBlockInfoClipHorizontal : CGameCtnBlockInfoClip, IClass
{
    [Hexadecimal] public static new uint Id => 0x0335B000;




    private string? horizontalClipGroupId;
    [AppliedWithChunk<Chunk0335B000>]
    public string? HorizontalClipGroupId { get => horizontalClipGroupId; set => horizontalClipGroupId = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnBlockInfoClipHorizontal"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnBlockInfoClipHorizontal() { }


    /// <summary>
    /// CGameCtnBlockInfoClipHorizontal 0x000 chunk
    /// </summary>
    [Chunk(0x0335B000)]
    public partial class Chunk0335B000 : Chunk<CGameCtnBlockInfoClipHorizontal>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0335B000;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnBlockInfoClipHorizontal n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref n.horizontalClipGroupId);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0335B000 => new Chunk0335B000(),
        _ => base.NewChunk(chunkId),
    };
}
