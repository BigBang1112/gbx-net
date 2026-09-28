namespace GBX.NET.Engines.Hms;

/// <remarks>ID: 0x06004000</remarks>
[Class(0x06004000)]
public partial class CHmsZone : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x06004000;




    private CHmsFogPlane[]? fogPlanes;
    [AppliedWithChunk<Chunk06004002>]
    public CHmsFogPlane[]? FogPlanes { get => fogPlanes; set => fogPlanes = value; }

    private bool mRIsForced;
    [AppliedWithChunk<Chunk06004003>]
    public bool MRIsForced { get => mRIsForced; set => mRIsForced = value; }

    private Vec3 mRPoint;
    [AppliedWithChunk<Chunk06004003>]
    public Vec3 MRPoint { get => mRPoint; set => mRPoint = value; }

    private Vec3 mRNormal;
    [AppliedWithChunk<Chunk06004003>]
    public Vec3 MRNormal { get => mRNormal; set => mRNormal = value; }

    private CHmsPrecalcRender[]? precalcRenders;
    [AppliedWithChunk<Chunk06004005>]
    public CHmsPrecalcRender[]? PrecalcRenders { get => precalcRenders; set => precalcRenders = value; }

    private CPlugBitmap? bitmapCubeReflectHardSpecA;
    [AppliedWithChunk<Chunk06004006>]
    [AppliedWithChunk<Chunk06004008>]
    public CPlugBitmap? BitmapCubeReflectHardSpecA { get => bitmapCubeReflectHardSpecAFile?.GetNode(ref bitmapCubeReflectHardSpecA) ?? bitmapCubeReflectHardSpecA; set => bitmapCubeReflectHardSpecA = value; }
    private Components.GbxRefTableFile? bitmapCubeReflectHardSpecAFile;
    public Components.GbxRefTableFile? BitmapCubeReflectHardSpecAFile { get => bitmapCubeReflectHardSpecAFile; set => bitmapCubeReflectHardSpecAFile = value; }
    public CPlugBitmap? GetBitmapCubeReflectHardSpecA(GbxReadSettings settings = default, bool exceptions = false) => bitmapCubeReflectHardSpecAFile?.GetNode(ref bitmapCubeReflectHardSpecA, settings, exceptions) ?? bitmapCubeReflectHardSpecA;

    private CPlugBitmap? bitmapCubeReflectHdrAlpha2;
    [AppliedWithChunk<Chunk06004008>]
    public CPlugBitmap? BitmapCubeReflectHdrAlpha2 { get => bitmapCubeReflectHdrAlpha2File?.GetNode(ref bitmapCubeReflectHdrAlpha2) ?? bitmapCubeReflectHdrAlpha2; set => bitmapCubeReflectHdrAlpha2 = value; }
    private Components.GbxRefTableFile? bitmapCubeReflectHdrAlpha2File;
    public Components.GbxRefTableFile? BitmapCubeReflectHdrAlpha2File { get => bitmapCubeReflectHdrAlpha2File; set => bitmapCubeReflectHdrAlpha2File = value; }
    public CPlugBitmap? GetBitmapCubeReflectHdrAlpha2(GbxReadSettings settings = default, bool exceptions = false) => bitmapCubeReflectHdrAlpha2File?.GetNode(ref bitmapCubeReflectHdrAlpha2, settings, exceptions) ?? bitmapCubeReflectHdrAlpha2;

    /// <summary>
    /// Creates a new instance of <see cref="CHmsZone"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CHmsZone() { }


    /// <summary>
    /// CHmsZone 0x002 chunk
    /// </summary>
    [Chunk(0x06004002)]
    public partial class Chunk06004002 : Chunk<CHmsZone>
    {
        /// <inheritdoc />
        public override uint Id => 0x06004002;

        public byte[]? U01;

        public override void ReadWrite(CHmsZone n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CHmsFogPlane>(ref n.fogPlanes!);
            rw.Data(ref U01!, 28);
        }
    }

    /// <summary>
    /// CHmsZone 0x003 chunk
    /// </summary>
    [Chunk(0x06004003)]
    public partial class Chunk06004003 : Chunk<CHmsZone>
    {
        /// <inheritdoc />
        public override uint Id => 0x06004003;


        public override void ReadWrite(CHmsZone n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.mRIsForced);
            if (n.MRIsForced)
            {
                rw.Vec3(ref n.mRPoint);
                rw.Vec3(ref n.mRNormal);
            }
        }
    }

    /// <summary>
    /// CHmsZone 0x005 chunk
    /// </summary>
    [Chunk(0x06004005)]
    public partial class Chunk06004005 : Chunk<CHmsZone>
    {
        /// <inheritdoc />
        public override uint Id => 0x06004005;


        public override void ReadWrite(CHmsZone n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CHmsPrecalcRender>(ref n.precalcRenders!);
        }
    }

    /// <summary>
    /// CHmsZone 0x006 chunk
    /// </summary>
    [Chunk(0x06004006)]
    public partial class Chunk06004006 : Chunk<CHmsZone>
    {
        /// <inheritdoc />
        public override uint Id => 0x06004006;


        public override void ReadWrite(CHmsZone n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugBitmap>(ref n.bitmapCubeReflectHardSpecA, ref n.bitmapCubeReflectHardSpecAFile);
        }
    }

    /// <summary>
    /// CHmsZone 0x008 chunk
    /// </summary>
    [Chunk(0x06004008)]
    public partial class Chunk06004008 : Chunk<CHmsZone>
    {
        /// <inheritdoc />
        public override uint Id => 0x06004008;


        public override void ReadWrite(CHmsZone n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugBitmap>(ref n.bitmapCubeReflectHardSpecA, ref n.bitmapCubeReflectHardSpecAFile);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapCubeReflectHdrAlpha2, ref n.bitmapCubeReflectHdrAlpha2File);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x06004002 => new Chunk06004002(),
        0x06004003 => new Chunk06004003(),
        0x06004005 => new Chunk06004005(),
        0x06004006 => new Chunk06004006(),
        0x06004008 => new Chunk06004008(),
        _ => base.NewChunk(chunkId),
    };
}
