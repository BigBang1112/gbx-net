namespace GBX.NET.Engines.Control;

/// <remarks>ID: 0x07001000</remarks>
[Class(0x07001000)]
public partial class CControlBase : CSceneToy, IClass
{
    [Hexadecimal] public static new uint Id => 0x07001000;




    private string? stackText;
    [AppliedWithChunk<Chunk0700100C>]
    public string? StackText { get => stackText; set => stackText = value; }

    private CControlLayout? layout;
    [AppliedWithChunk<Chunk0700100E>]
    public CControlLayout? Layout { get => layout; set => layout = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CControlBase"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CControlBase() { }


    /// <summary>
    /// CControlBase 0x00C chunk
    /// </summary>
    [Chunk(0x0700100C)]
    public partial class Chunk0700100C : Chunk<CControlBase>
    {
        /// <inheritdoc />
        public override uint Id => 0x0700100C;

        public int U01;
        public int U02;
        public int U03;
        public int U04;

        public override void ReadWrite(CControlBase n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.String(ref n.stackText);
        }
    }

    /// <summary>
    /// CControlBase 0x00E chunk
    /// </summary>
    [Chunk(0x0700100E)]
    public partial class Chunk0700100E : Chunk<CControlBase>
    {
        /// <inheritdoc />
        public override uint Id => 0x0700100E;

        public BoxAligned U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CControlBase n, GbxReaderWriter rw)
        {
            rw.BoxAligned(ref U01);
            rw.NodeRef<CControlLayout>(ref n.layout);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
        }
    }

    /// <summary>
    /// CControlBase 0x00F chunk
    /// </summary>
    [Chunk(0x0700100F)]
    public partial class Chunk0700100F : Chunk<CControlBase>
    {
        /// <inheritdoc />
        public override uint Id => 0x0700100F;

        public string? U01;

        public override void ReadWrite(CControlBase n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
        }
    }

    /// <summary>
    /// CControlBase 0x010 chunk
    /// </summary>
    [Chunk(0x07001010)]
    public partial class Chunk07001010 : Chunk<CControlBase>
    {
        /// <inheritdoc />
        public override uint Id => 0x07001010;

        public CMwNod? U01;
        public int? U02;

        public override void ReadWrite(CControlBase n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref U01);
            if (U01==null)
            {
                rw.Int32(ref U02);
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0700100C => new Chunk0700100C(),
        0x0700100E => new Chunk0700100E(),
        0x0700100F => new Chunk0700100F(),
        0x07001010 => new Chunk07001010(),
        _ => base.NewChunk(chunkId),
    };
}
