namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09189000</remarks>
[Class(0x09189000)]
public partial class CPlugMediaClipList : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x09189000;




    private External<CMwNod>[]? mediaClipFids;
    [AppliedWithChunk<Chunk09189000>]
    public External<CMwNod>[]? MediaClipFids { get => mediaClipFids; set => mediaClipFids = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugMediaClipList"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugMediaClipList() { }


    /// <summary>
    /// CPlugMediaClipList 0x000 chunk
    /// </summary>
    [Chunk(0x09189000)]
    public partial class Chunk09189000 : Chunk<CPlugMediaClipList>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09189000;

        public int Version { get; set; }


        public override void ReadWrite(CPlugMediaClipList n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayNodeRef<CMwNod>(ref n.mediaClipFids!);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09189000 => new Chunk09189000(),
        _ => base.NewChunk(chunkId),
    };
}
