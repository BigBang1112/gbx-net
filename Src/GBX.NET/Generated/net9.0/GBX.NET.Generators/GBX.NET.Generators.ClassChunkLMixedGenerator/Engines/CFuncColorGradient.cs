namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x05038000</remarks>
[Class(0x05038000)]
public partial class CFuncColorGradient : CFunc, IClass
{
    [Hexadecimal] public static new uint Id => 0x05038000;




    private Vec3 keyFrameValue0;
    [AppliedWithChunk<Chunk05038000>]
    [AppliedWithChunk<Chunk05038001>]
    public Vec3 KeyFrameValue0 { get => keyFrameValue0; set => keyFrameValue0 = value; }

    private Vec3 keyFrameValue1;
    [AppliedWithChunk<Chunk05038000>]
    [AppliedWithChunk<Chunk05038001>]
    public Vec3 KeyFrameValue1 { get => keyFrameValue1; set => keyFrameValue1 = value; }

    private Vec3 keyFrameValue2;
    [AppliedWithChunk<Chunk05038000>]
    [AppliedWithChunk<Chunk05038001>]
    public Vec3 KeyFrameValue2 { get => keyFrameValue2; set => keyFrameValue2 = value; }

    private Vec3 keyFrameValue3;
    [AppliedWithChunk<Chunk05038000>]
    [AppliedWithChunk<Chunk05038001>]
    public Vec3 KeyFrameValue3 { get => keyFrameValue3; set => keyFrameValue3 = value; }

    private float keyFramePos1;
    [AppliedWithChunk<Chunk05038000>]
    [AppliedWithChunk<Chunk05038001>]
    public float KeyFramePos1 { get => keyFramePos1; set => keyFramePos1 = value; }

    private float keyFramePos2;
    [AppliedWithChunk<Chunk05038000>]
    [AppliedWithChunk<Chunk05038001>]
    public float KeyFramePos2 { get => keyFramePos2; set => keyFramePos2 = value; }

    private EColorSpace colorSpace;
    [AppliedWithChunk<Chunk05038001>]
    public EColorSpace ColorSpace { get => colorSpace; set => colorSpace = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CFuncColorGradient"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CFuncColorGradient() { }


    /// <summary>
    /// CFuncColorGradient 0x000 chunk
    /// </summary>
    [Chunk(0x05038000)]
    public partial class Chunk05038000 : Chunk<CFuncColorGradient>
    {
        /// <inheritdoc />
        public override uint Id => 0x05038000;


        public override void ReadWrite(CFuncColorGradient n, GbxReaderWriter rw)
        {
            rw.Vec3(ref n.keyFrameValue0);
            rw.Vec3(ref n.keyFrameValue1);
            rw.Vec3(ref n.keyFrameValue2);
            rw.Vec3(ref n.keyFrameValue3);
            rw.Single(ref n.keyFramePos1);
            rw.Single(ref n.keyFramePos2);
        }
    }

    /// <summary>
    /// CFuncColorGradient 0x001 chunk
    /// </summary>
    [Chunk(0x05038001)]
    public partial class Chunk05038001 : Chunk05038000
    {
        /// <inheritdoc />
        public override uint Id => 0x05038001;


        public override void ReadWrite(CFuncColorGradient n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.EnumInt32<EColorSpace>(ref n.colorSpace);
        }
    }



    public enum EColorSpace
    {
        Linear,
        sRGB,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x05038000 => new Chunk05038000(),
        0x05038001 => new Chunk05038001(),
        _ => base.NewChunk(chunkId),
    };
}
