namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0907E000</remarks>
[Class(0x0907E000)]
public partial class CPlugBitmapSampler : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x0907E000;




    private string? name;
    [AppliedWithChunk<Chunk0907E002>]
    [AppliedWithChunk<Chunk0907E005>]
    [AppliedWithChunk<Chunk0907E006>]
    [AppliedWithChunk<Chunk0907E007>]
    [AppliedWithChunk<Chunk0907E008>]
    [AppliedWithChunk<Chunk0907E00B>]
    public string? Name { get => name; set => name = value; }

    private CPlugBitmap? bitmap;
    [AppliedWithChunk<Chunk0907E002>]
    [AppliedWithChunk<Chunk0907E005>]
    [AppliedWithChunk<Chunk0907E006>]
    [AppliedWithChunk<Chunk0907E007>]
    [AppliedWithChunk<Chunk0907E008>]
    [AppliedWithChunk<Chunk0907E00B>]
    public CPlugBitmap? Bitmap { get => bitmapFile?.GetNode(ref bitmap) ?? bitmap; set => bitmap = value; }
    private Components.GbxRefTableFile? bitmapFile;
    public Components.GbxRefTableFile? BitmapFile { get => bitmapFile; set => bitmapFile = value; }
    public CPlugBitmap? GetBitmap(GbxReadSettings settings = default, bool exceptions = false) => bitmapFile?.GetNode(ref bitmap, settings, exceptions) ?? bitmap;


    /// <summary>
    /// CPlugBitmapSampler 0x002 chunk
    /// </summary>
    [Chunk(0x0907E002)]
    public partial class Chunk0907E002 : Chunk<CPlugBitmapSampler>
    {
        /// <inheritdoc />
        public override uint Id => 0x0907E002;

        public uint U01;
        public float U02;

        public override void ReadWrite(CPlugBitmapSampler n, GbxReaderWriter rw)
        {
            rw.Id(ref n.name);
            rw.NodeRef<CPlugBitmap>(ref n.bitmap, ref n.bitmapFile);
            rw.UInt32(ref U01);
            rw.Single(ref U02);
        }
    }

    /// <summary>
    /// CPlugBitmapSampler 0x005 chunk
    /// </summary>
    [Chunk(0x0907E005)]
    public partial class Chunk0907E005 : Chunk0907E002
    {
        /// <inheritdoc />
        public override uint Id => 0x0907E005;

    }

    /// <summary>
    /// CPlugBitmapSampler 0x006 chunk
    /// </summary>
    [Chunk(0x0907E006)]
    public partial class Chunk0907E006 : Chunk0907E002
    {
        /// <inheritdoc />
        public override uint Id => 0x0907E006;

    }

    /// <summary>
    /// CPlugBitmapSampler 0x007 chunk
    /// </summary>
    [Chunk(0x0907E007)]
    public partial class Chunk0907E007 : Chunk0907E002
    {
        /// <inheritdoc />
        public override uint Id => 0x0907E007;

    }

    /// <summary>
    /// CPlugBitmapSampler 0x008 chunk
    /// </summary>
    [Chunk(0x0907E008)]
    public partial class Chunk0907E008 : Chunk0907E002
    {
        /// <inheritdoc />
        public override uint Id => 0x0907E008;

    }

    /// <summary>
    /// CPlugBitmapSampler 0x00B chunk
    /// </summary>
    [Chunk(0x0907E00B)]
    public partial class Chunk0907E00B : Chunk0907E008, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0907E00B;

        public int Version { get; set; }


        public override void ReadWrite(CPlugBitmapSampler n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            base.ReadWrite(n, rw);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0907E002 => new Chunk0907E002(),
        0x0907E005 => new Chunk0907E005(),
        0x0907E006 => new Chunk0907E006(),
        0x0907E007 => new Chunk0907E007(),
        0x0907E008 => new Chunk0907E008(),
        0x0907E00B => new Chunk0907E00B(),
        _ => base.NewChunk(chunkId),
    };
}
