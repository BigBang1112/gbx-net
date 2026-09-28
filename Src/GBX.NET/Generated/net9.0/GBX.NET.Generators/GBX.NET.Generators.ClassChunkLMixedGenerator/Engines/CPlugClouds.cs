namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09180000</remarks>
[Class(0x09180000)]
public partial class CPlugClouds : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x09180000;




    private External<CPlugSolid>[]? solidFids;
    [AppliedWithChunk<Chunk09180000>]
    [AppliedWithChunk<Chunk09180004>]
    public External<CPlugSolid>[]? SolidFids { get => solidFids; set => solidFids = value; }

    private float bottomNearZ;
    [AppliedWithChunk<Chunk09180000>]
    [AppliedWithChunk<Chunk09180004>]
    public float BottomNearZ { get => bottomNearZ; set => bottomNearZ = value; }

    private float bottomFarZ;
    [AppliedWithChunk<Chunk09180000>]
    [AppliedWithChunk<Chunk09180004>]
    public float BottomFarZ { get => bottomFarZ; set => bottomFarZ = value; }

    private CPlugFileImg? imageColorMin;
    [AppliedWithChunk<Chunk09180000>]
    [AppliedWithChunk<Chunk09180004>]
    public CPlugFileImg? ImageColorMin { get => imageColorMinFile?.GetNode(ref imageColorMin) ?? imageColorMin; set => imageColorMin = value; }
    private Components.GbxRefTableFile? imageColorMinFile;
    public Components.GbxRefTableFile? ImageColorMinFile { get => imageColorMinFile; set => imageColorMinFile = value; }
    public CPlugFileImg? GetImageColorMin(GbxReadSettings settings = default, bool exceptions = false) => imageColorMinFile?.GetNode(ref imageColorMin, settings, exceptions) ?? imageColorMin;

    private CPlugFileImg? imageColorMax;
    [AppliedWithChunk<Chunk09180000>]
    [AppliedWithChunk<Chunk09180004>]
    public CPlugFileImg? ImageColorMax { get => imageColorMaxFile?.GetNode(ref imageColorMax) ?? imageColorMax; set => imageColorMax = value; }
    private Components.GbxRefTableFile? imageColorMaxFile;
    public Components.GbxRefTableFile? ImageColorMaxFile { get => imageColorMaxFile; set => imageColorMaxFile = value; }
    public CPlugFileImg? GetImageColorMax(GbxReadSettings settings = default, bool exceptions = false) => imageColorMaxFile?.GetNode(ref imageColorMax, settings, exceptions) ?? imageColorMax;

    private int lighting;
    [AppliedWithChunk<Chunk09180000>]
    [AppliedWithChunk<Chunk09180004>]
    public int Lighting { get => lighting; set => lighting = value; }

    private Vec2[]? pointHeights;
    [AppliedWithChunk<Chunk09180002>]
    public Vec2[]? PointHeights { get => pointHeights; set => pointHeights = value; }

    private int heightCenter;
    [AppliedWithChunk<Chunk09180003>]
    public int HeightCenter { get => heightCenter; set => heightCenter = value; }

    private Vec2 heightCenterXZ;
    [AppliedWithChunk<Chunk09180003>]
    public Vec2 HeightCenterXZ { get => heightCenterXZ; set => heightCenterXZ = value; }

    private float speedScale;
    [AppliedWithChunk<Chunk09180004>]
    public float SpeedScale { get => speedScale; set => speedScale = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugClouds"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugClouds() { }


    /// <summary>
    /// CPlugClouds 0x000 chunk
    /// </summary>
    [Chunk(0x09180000)]
    public partial class Chunk09180000 : Chunk<CPlugClouds>
    {
        /// <inheritdoc />
        public override uint Id => 0x09180000;


        public override void ReadWrite(CPlugClouds n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef<CPlugSolid>(ref n.solidFids!);
            rw.Single(ref n.bottomNearZ);
            rw.Single(ref n.bottomFarZ);
            rw.NodeRef<CPlugFileImg>(ref n.imageColorMin, ref n.imageColorMinFile);
            rw.NodeRef<CPlugFileImg>(ref n.imageColorMax, ref n.imageColorMaxFile);
            rw.Int32(ref n.lighting);
        }
    }

    /// <summary>
    /// CPlugClouds 0x002 chunk
    /// </summary>
    [Chunk(0x09180002)]
    public partial class Chunk09180002 : Chunk<CPlugClouds>
    {
        /// <inheritdoc />
        public override uint Id => 0x09180002;


        public override void ReadWrite(CPlugClouds n, GbxReaderWriter rw)
        {
            rw.Array<Vec2>(ref n.pointHeights!);
        }
    }

    /// <summary>
    /// CPlugClouds 0x003 chunk
    /// </summary>
    [Chunk(0x09180003)]
    public partial class Chunk09180003 : Chunk<CPlugClouds>
    {
        /// <inheritdoc />
        public override uint Id => 0x09180003;


        public override void ReadWrite(CPlugClouds n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.heightCenter);
            rw.Vec2(ref n.heightCenterXZ);
        }
    }

    /// <summary>
    /// CPlugClouds 0x004 chunk
    /// </summary>
    [Chunk(0x09180004)]
    public partial class Chunk09180004 : Chunk09180000
    {
        /// <inheritdoc />
        public override uint Id => 0x09180004;


        public override void ReadWrite(CPlugClouds n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.Single(ref n.speedScale);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09180000 => new Chunk09180000(),
        0x09180002 => new Chunk09180002(),
        0x09180003 => new Chunk09180003(),
        0x09180004 => new Chunk09180004(),
        _ => base.NewChunk(chunkId),
    };
}
