namespace GBX.NET.Engines.Hms;

/// <remarks>ID: 0x0600C000</remarks>
[Class(0x0600C000)]
public partial class CHmsLight : CHmsPocEmitter, IClass
{
    [Hexadecimal] public static new uint Id => 0x0600C000;




    private GxLight? mainGxLight;
    [AppliedWithChunk<Chunk0600C000>]
    [AppliedWithChunk<Chunk0600C001>]
    [AppliedWithChunk<Chunk0600C002>]
    [AppliedWithChunk<Chunk0600C003>]
    public GxLight? MainGxLight { get => mainGxLight; set => mainGxLight = value; }

    private CPlugBitmap? bitmapFlare;
    [AppliedWithChunk<Chunk0600C002>]
    [AppliedWithChunk<Chunk0600C003>]
    public CPlugBitmap? BitmapFlare { get => bitmapFlareFile?.GetNode(ref bitmapFlare) ?? bitmapFlare; set => bitmapFlare = value; }
    private Components.GbxRefTableFile? bitmapFlareFile;
    public Components.GbxRefTableFile? BitmapFlareFile { get => bitmapFlareFile; set => bitmapFlareFile = value; }
    public CPlugBitmap? GetBitmapFlare(GbxReadSettings settings = default, bool exceptions = false) => bitmapFlareFile?.GetNode(ref bitmapFlare, settings, exceptions) ?? bitmapFlare;

    private CPlugBitmap? bitmapSprite;
    [AppliedWithChunk<Chunk0600C003>]
    public CPlugBitmap? BitmapSprite { get => bitmapSpriteFile?.GetNode(ref bitmapSprite) ?? bitmapSprite; set => bitmapSprite = value; }
    private Components.GbxRefTableFile? bitmapSpriteFile;
    public Components.GbxRefTableFile? BitmapSpriteFile { get => bitmapSpriteFile; set => bitmapSpriteFile = value; }
    public CPlugBitmap? GetBitmapSprite(GbxReadSettings settings = default, bool exceptions = false) => bitmapSpriteFile?.GetNode(ref bitmapSprite, settings, exceptions) ?? bitmapSprite;

    /// <summary>
    /// Creates a new instance of <see cref="CHmsLight"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CHmsLight() { }


    /// <summary>
    /// CHmsLight 0x000 chunk
    /// </summary>
    [Chunk(0x0600C000)]
    public partial class Chunk0600C000 : Chunk<CHmsLight>
    {
        /// <inheritdoc />
        public override uint Id => 0x0600C000;

        public int U01;

        public override void ReadWrite(CHmsLight n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.NodeRef<GxLight>(ref n.mainGxLight);
        }
    }

    /// <summary>
    /// CHmsLight 0x001 chunk
    /// </summary>
    [Chunk(0x0600C001)]
    public partial class Chunk0600C001 : Chunk0600C000
    {
        /// <inheritdoc />
        public override uint Id => 0x0600C001;


        public override void ReadWrite(CHmsLight n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
        }
    }

    /// <summary>
    /// CHmsLight 0x002 chunk
    /// </summary>
    [Chunk(0x0600C002)]
    public partial class Chunk0600C002 : Chunk0600C001
    {
        /// <inheritdoc />
        public override uint Id => 0x0600C002;


        public override void ReadWrite(CHmsLight n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapFlare, ref n.bitmapFlareFile);
        }
    }

    /// <summary>
    /// CHmsLight 0x003 chunk
    /// </summary>
    [Chunk(0x0600C003)]
    public partial class Chunk0600C003 : Chunk0600C002
    {
        /// <inheritdoc />
        public override uint Id => 0x0600C003;


        public override void ReadWrite(CHmsLight n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapSprite, ref n.bitmapSpriteFile);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0600C000 => new Chunk0600C000(),
        0x0600C001 => new Chunk0600C001(),
        0x0600C002 => new Chunk0600C002(),
        0x0600C003 => new Chunk0600C003(),
        _ => base.NewChunk(chunkId),
    };
}
