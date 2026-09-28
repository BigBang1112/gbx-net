namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x05002000</remarks>
[Class(0x05002000)]
public partial class CFuncKeys : CFunc, IClass
{
    [Hexadecimal] public static new uint Id => 0x05002000;




    private float[]? xs;
    [AppliedWithChunk<Chunk05002001>]
    public float[]? Xs { get => xs; set => xs = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CFuncKeys"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CFuncKeys() { }


    /// <summary>
    /// CFuncKeys 0x001 chunk
    /// </summary>
    [Chunk(0x05002001)]
    public partial class Chunk05002001 : Chunk<CFuncKeys>
    {
        /// <inheritdoc />
        public override uint Id => 0x05002001;


        public override void ReadWrite(CFuncKeys n, GbxReaderWriter rw)
        {
            rw.Array<float>(ref n.xs!);
        }
    }

    /// <summary>
    /// CFuncKeys 0x003 chunk
    /// </summary>
    [Chunk(0x05002003)]
    public partial class Chunk05002003 : Chunk<CFuncKeys>
    {
        /// <inheritdoc />
        public override uint Id => 0x05002003;

        public string? U01;

        public override void ReadWrite(CFuncKeys n, GbxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x05002001 => new Chunk05002001(),
        0x05002003 => new Chunk05002003(),
        _ => base.NewChunk(chunkId),
    };
}
