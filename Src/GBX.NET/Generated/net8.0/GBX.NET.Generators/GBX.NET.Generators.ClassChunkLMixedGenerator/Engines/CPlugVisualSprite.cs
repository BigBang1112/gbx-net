namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09010000</remarks>
[Class(0x09010000)]
public partial class CPlugVisualSprite : CPlugVisual3D, IClass
{
    [Hexadecimal] public static new uint Id => 0x09010000;




    private CPlugSpriteParam? spriteParam;
    [AppliedWithChunk<Chunk09010008>]
    public CPlugSpriteParam? SpriteParam { get => spriteParam; set => spriteParam = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugVisualSprite"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugVisualSprite() { }


    /// <summary>
    /// CPlugVisualSprite 0x005 chunk
    /// </summary>
    [Chunk(0x09010005)]
    public partial class Chunk09010005 : Chunk<CPlugVisualSprite>
    {
        /// <inheritdoc />
        public override uint Id => 0x09010005;

        public int U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;

        public override void ReadWrite(CPlugVisualSprite n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
        }
    }

    /// <summary>
    /// CPlugVisualSprite 0x006 chunk
    /// </summary>
    [Chunk(0x09010006)]
    public partial class Chunk09010006 : Chunk<CPlugVisualSprite>
    {
        /// <inheritdoc />
        public override uint Id => 0x09010006;

        public short U01;
        public short U02;

        public override void ReadWrite(CPlugVisualSprite n, GbxReaderWriter rw)
        {
            rw.Int16(ref U01);
            rw.Int16(ref U02);
        }
    }

    /// <summary>
    /// CPlugVisualSprite 0x008 chunk
    /// </summary>
    [Chunk(0x09010008)]
    public partial class Chunk09010008 : Chunk<CPlugVisualSprite>
    {
        /// <inheritdoc />
        public override uint Id => 0x09010008;


        public override void ReadWrite(CPlugVisualSprite n, GbxReaderWriter rw)
        {
            rw.Node<CPlugSpriteParam>(ref n.spriteParam);
        }
    }

    /// <summary>
    /// CPlugVisualSprite 0x009 chunk
    /// </summary>
    [Chunk(0x09010009)]
    public partial class Chunk09010009 : Chunk<CPlugVisualSprite>
    {
        /// <inheritdoc />
        public override uint Id => 0x09010009;

        public Rect[]? U01;

        public override void ReadWrite(CPlugVisualSprite n, GbxReaderWriter rw)
        {
            rw.Array<Rect>(ref U01!);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09010005 => new Chunk09010005(),
        0x09010006 => new Chunk09010006(),
        0x09010008 => new Chunk09010008(),
        0x09010009 => new Chunk09010009(),
        _ => base.NewChunk(chunkId),
    };
}
