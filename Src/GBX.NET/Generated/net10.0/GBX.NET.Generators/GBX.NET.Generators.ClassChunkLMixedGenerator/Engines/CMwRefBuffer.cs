namespace GBX.NET.Engines.MwFoundations;

/// <remarks>ID: 0x01026000</remarks>
[Class(0x01026000)]
public partial class CMwRefBuffer : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x01026000;




    private uint nodClassId;
    [AppliedWithChunk<Chunk01026000>]
    public uint NodClassId { get => nodClassId; set => nodClassId = value; }

    private bool useAddRefRelease;
    [AppliedWithChunk<Chunk01026000>]
    public bool UseAddRefRelease { get => useAddRefRelease; set => useAddRefRelease = value; }

    private External<CMwNod>[]? nods;
    [AppliedWithChunk<Chunk01026000>]
    public External<CMwNod>[]? Nods { get => nods; set => nods = value; }

    private string? bufferId;
    [AppliedWithChunk<Chunk01026001>]
    public string? BufferId { get => bufferId; set => bufferId = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CMwRefBuffer"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CMwRefBuffer() { }


    /// <summary>
    /// CMwRefBuffer 0x000 chunk
    /// </summary>
    [Chunk(0x01026000)]
    public partial class Chunk01026000 : Chunk<CMwRefBuffer>
    {
        /// <inheritdoc />
        public override uint Id => 0x01026000;


        public override void ReadWrite(CMwRefBuffer n, GbxReaderWriter rw)
        {
            rw.UInt32(ref n.nodClassId);
            rw.Boolean(ref n.useAddRefRelease);
            rw.ArrayNodeRef_deprec<CMwNod>(ref n.nods!);
        }
    }

    /// <summary>
    /// CMwRefBuffer 0x001 chunk
    /// </summary>
    [Chunk(0x01026001)]
    public partial class Chunk01026001 : Chunk<CMwRefBuffer>
    {
        /// <inheritdoc />
        public override uint Id => 0x01026001;


        public override void ReadWrite(CMwRefBuffer n, GbxReaderWriter rw)
        {
            rw.Id(ref n.bufferId);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x01026000 => new Chunk01026000(),
        0x01026001 => new Chunk01026001(),
        _ => base.NewChunk(chunkId),
    };
}
