namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09119000</remarks>
[Class(0x09119000)]
public partial class CPlugPath : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x09119000;




    private CPlugPolyLine3[]? polyLines;
    [AppliedWithChunk<Chunk09119000>]
    public CPlugPolyLine3[]? PolyLines { get => polyLines; set => polyLines = value; }

    private byte[]? lineGroups;
    /// <summary>
    /// length must equal PolyLines
    /// </summary>
    [AppliedWithChunk<Chunk09119000>]
    public byte[]? LineGroups { get => lineGroups; set => lineGroups = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugPath"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugPath() { }


    /// <summary>
    /// CPlugPath 0x000 chunk
    /// </summary>
    [Chunk(0x09119000)]
    public partial class Chunk09119000 : Chunk<CPlugPath>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09119000;

        public int Version { get; set; } = 2;

        public bool U01;
        public byte U02;

        public override void ReadWrite(CPlugPath n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayNodeRef<CPlugPolyLine3>(ref n.polyLines!);
            if (Version >= 2)
            {
                rw.Boolean(ref U01);
                rw.Byte(ref U02);
                rw.Data(ref n.lineGroups); // length must equal PolyLines
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09119000 => new Chunk09119000(),
        _ => base.NewChunk(chunkId),
    };
}
