namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x0501A000</remarks>
[Class(0x0501A000)]
public partial class CFuncKeysReal : CFuncKeys, IClass
{
    [Hexadecimal] public static new uint Id => 0x0501A000;




    private float[]? ys;
    [AppliedWithChunk<Chunk0501A000>]
    [AppliedWithChunk<Chunk0501A001>]
    [AppliedWithChunk<Chunk0501A002>]
    public float[]? Ys { get => ys; set => ys = value; }

    private ERealInterp realInterp;
    [AppliedWithChunk<Chunk0501A001>]
    [AppliedWithChunk<Chunk0501A002>]
    public ERealInterp RealInterp { get => realInterp; set => realInterp = value; }

    private bool forceTangentMinX;
    [AppliedWithChunk<Chunk0501A002>]
    public bool ForceTangentMinX { get => forceTangentMinX; set => forceTangentMinX = value; }

    private bool forceTangentMaxX;
    [AppliedWithChunk<Chunk0501A002>]
    public bool ForceTangentMaxX { get => forceTangentMaxX; set => forceTangentMaxX = value; }

    private float forcedTangentMinX;
    [AppliedWithChunk<Chunk0501A002>]
    public float ForcedTangentMinX { get => forcedTangentMinX; set => forcedTangentMinX = value; }

    private float forcedTangentMaxX;
    [AppliedWithChunk<Chunk0501A002>]
    public float ForcedTangentMaxX { get => forcedTangentMaxX; set => forcedTangentMaxX = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CFuncKeysReal"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CFuncKeysReal() { }


    /// <summary>
    /// CFuncKeysReal 0x000 chunk
    /// </summary>
    [Chunk(0x0501A000)]
    public partial class Chunk0501A000 : Chunk<CFuncKeysReal>
    {
        /// <inheritdoc />
        public override uint Id => 0x0501A000;


        public override void ReadWrite(CFuncKeysReal n, GbxReaderWriter rw)
        {
            rw.Array<float>(ref n.ys!);
        }
    }

    /// <summary>
    /// CFuncKeysReal 0x001 chunk
    /// </summary>
    [Chunk(0x0501A001)]
    public partial class Chunk0501A001 : Chunk<CFuncKeysReal>
    {
        /// <inheritdoc />
        public override uint Id => 0x0501A001;


        public override void ReadWrite(CFuncKeysReal n, GbxReaderWriter rw)
        {
            rw.Array<float>(ref n.ys!);
            rw.EnumInt32<ERealInterp>(ref n.realInterp);
        }
    }

    /// <summary>
    /// CFuncKeysReal 0x002 chunk
    /// </summary>
    [Chunk(0x0501A002)]
    public partial class Chunk0501A002 : Chunk0501A001
    {
        /// <inheritdoc />
        public override uint Id => 0x0501A002;


        public override void ReadWrite(CFuncKeysReal n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.Boolean(ref n.forceTangentMinX);
            rw.Boolean(ref n.forceTangentMaxX);
            rw.Single(ref n.forcedTangentMinX);
            rw.Single(ref n.forcedTangentMaxX);
        }
    }



    public enum ERealInterp
    {
        None,
        Linear,
        Hermite,
        SmoothStep,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0501A000 => new Chunk0501A000(),
        0x0501A001 => new Chunk0501A001(),
        0x0501A002 => new Chunk0501A002(),
        _ => base.NewChunk(chunkId),
    };
}
