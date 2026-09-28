namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0901D000</remarks>
[Class(0x0901D000)]
public partial class CPlugLight : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x0901D000;




    private GxLight? light;
    [AppliedWithChunk<Chunk0901D000>]
    [AppliedWithChunk<Chunk0901D002>]
    [AppliedWithChunk<Chunk0901D004>]
    public GxLight? Light { get => light; set => light = value; }

    private CFuncLight? funcLight;
    [AppliedWithChunk<Chunk0901D000>]
    [AppliedWithChunk<Chunk0901D002>]
    public CFuncLight? FuncLight { get => funcLight; set => funcLight = value; }

    private CPlugBitmap? bitmapFlare;
    [AppliedWithChunk<Chunk0901D000>]
    [AppliedWithChunk<Chunk0901D002>]
    public CPlugBitmap? BitmapFlare { get => bitmapFlareFile?.GetNode(ref bitmapFlare) ?? bitmapFlare; set => bitmapFlare = value; }
    private Components.GbxRefTableFile? bitmapFlareFile;
    public Components.GbxRefTableFile? BitmapFlareFile { get => bitmapFlareFile; set => bitmapFlareFile = value; }
    public CPlugBitmap? GetBitmapFlare(GbxReadSettings settings = default, bool exceptions = false) => bitmapFlareFile?.GetNode(ref bitmapFlare, settings, exceptions) ?? bitmapFlare;

    private CPlugBitmap? bitmapProjector;
    [AppliedWithChunk<Chunk0901D000>]
    [AppliedWithChunk<Chunk0901D002>]
    public CPlugBitmap? BitmapProjector { get => bitmapProjectorFile?.GetNode(ref bitmapProjector) ?? bitmapProjector; set => bitmapProjector = value; }
    private Components.GbxRefTableFile? bitmapProjectorFile;
    public Components.GbxRefTableFile? BitmapProjectorFile { get => bitmapProjectorFile; set => bitmapProjectorFile = value; }
    public CPlugBitmap? GetBitmapProjector(GbxReadSettings settings = default, bool exceptions = false) => bitmapProjectorFile?.GetNode(ref bitmapProjector, settings, exceptions) ?? bitmapProjector;

    private EFlags flags;
    [AppliedWithChunk<Chunk0901D002>]
    public EFlags Flags { get => flags; set => flags = value; }

    private CPlugFileImg? imageAnim;
    [AppliedWithChunk<Chunk0901D003>]
    public CPlugFileImg? ImageAnim { get => imageAnimFile?.GetNode(ref imageAnim) ?? imageAnim; set => imageAnim = value; }
    private Components.GbxRefTableFile? imageAnimFile;
    public Components.GbxRefTableFile? ImageAnimFile { get => imageAnimFile; set => imageAnimFile = value; }
    public CPlugFileImg? GetImageAnim(GbxReadSettings settings = default, bool exceptions = false) => imageAnimFile?.GetNode(ref imageAnim, settings, exceptions) ?? imageAnim;

    private float animPeriodMin;
    [AppliedWithChunk<Chunk0901D003>]
    public float AnimPeriodMin { get => animPeriodMin; set => animPeriodMin = value; }

    private float animPeriodMax;
    [AppliedWithChunk<Chunk0901D003>]
    public float AnimPeriodMax { get => animPeriodMax; set => animPeriodMax = value; }

    private string? animTimerName;
    [AppliedWithChunk<Chunk0901D003>]
    public string? AnimTimerName { get => animTimerName; set => animTimerName = value; }


    /// <summary>
    /// CPlugLight 0x000 chunk
    /// </summary>
    [Chunk(0x0901D000)]
    public partial class Chunk0901D000 : Chunk<CPlugLight>
    {
        /// <inheritdoc />
        public override uint Id => 0x0901D000;


        public override void ReadWrite(CPlugLight n, GbxReaderWriter rw)
        {
            rw.NodeRef<GxLight>(ref n.light);
            rw.NodeRef<CFuncLight>(ref n.funcLight);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapFlare, ref n.bitmapFlareFile);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapProjector, ref n.bitmapProjectorFile);
        }
    }

    /// <summary>
    /// CPlugLight 0x002 chunk
    /// </summary>
    [Chunk(0x0901D002)]
    public partial class Chunk0901D002 : Chunk0901D000
    {
        /// <inheritdoc />
        public override uint Id => 0x0901D002;


        public override void ReadWrite(CPlugLight n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.EnumInt32<EFlags>(ref n.flags);
        }
    }

    /// <summary>
    /// CPlugLight 0x003 chunk
    /// </summary>
    [Chunk(0x0901D003)]
    public partial class Chunk0901D003 : Chunk<CPlugLight>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0901D003;

        public int Version { get; set; }


        public override void ReadWrite(CPlugLight n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugFileImg>(ref n.imageAnim, ref n.imageAnimFile);
            rw.Single(ref n.animPeriodMin);
            rw.Single(ref n.animPeriodMax);
            if (Version >= 1)
            {
                rw.Id(ref n.animTimerName);
            }
        }
    }

    /// <summary>
    /// CPlugLight 0x004 chunk
    /// </summary>
    [Chunk(0x0901D004)]
    public partial class Chunk0901D004 : Chunk<CPlugLight>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0901D004;

        public int Version { get; set; }

        public int U01;
        public int U02;
        public int U03;
        public int U04;
        public int U05;

        public override void ReadWrite(CPlugLight n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<GxLight>(ref n.light);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
        }
    }



    public enum EFlags
    {
        None,
        NightOnly,
        ReflectByGround,
        DuplicateGxLight = 4,
        SceneLightOnlyWhenTreeVisible = 8,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0901D000 => new Chunk0901D000(),
        0x0901D002 => new Chunk0901D002(),
        0x0901D003 => new Chunk0901D003(),
        0x0901D004 => new Chunk0901D004(),
        _ => base.NewChunk(chunkId),
    };
}
