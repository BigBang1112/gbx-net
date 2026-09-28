namespace GBX.NET.Engines.Graphic;

/// <remarks>ID: 0x04007000</remarks>
[Class(0x04007000)]
public partial class GxLightDirectional : GxLightNotAmbient, IClass
{
    [Hexadecimal] public static new uint Id => 0x04007000;




    private bool useBoundaryHint;
    [AppliedWithChunk<Chunk04007001>]
    [AppliedWithChunk<Chunk04007002>]
    public bool UseBoundaryHint { get => useBoundaryHint; set => useBoundaryHint = value; }

    private Vec3 boundaryHintPos;
    [AppliedWithChunk<Chunk04007001>]
    [AppliedWithChunk<Chunk04007002>]
    public Vec3 BoundaryHintPos { get => boundaryHintPos; set => boundaryHintPos = value; }

    private float dazzleAngleMax;
    [AppliedWithChunk<Chunk04007002>]
    public float DazzleAngleMax { get => dazzleAngleMax; set => dazzleAngleMax = value; }

    private float dazzleIntensity;
    [AppliedWithChunk<Chunk04007002>]
    public float DazzleIntensity { get => dazzleIntensity; set => dazzleIntensity = value; }

    private Vec3 dblSidedRGB;
    [AppliedWithChunk<Chunk04007003>]
    public Vec3 DblSidedRGB { get => dblSidedRGB; set => dblSidedRGB = value; }

    private Vec3 reverseRGB;
    [AppliedWithChunk<Chunk04007004>]
    public Vec3 ReverseRGB { get => reverseRGB; set => reverseRGB = value; }

    private float reverseIntens;
    [AppliedWithChunk<Chunk04007004>]
    public float ReverseIntens { get => reverseIntens; set => reverseIntens = value; }

    private float emittAngularSize;
    [AppliedWithChunk<Chunk04007005>]
    public float EmittAngularSize { get => emittAngularSize; set => emittAngularSize = value; }

    private float flareAngularSize;
    [AppliedWithChunk<Chunk04007005>]
    public float FlareAngularSize { get => flareAngularSize; set => flareAngularSize = value; }

    /// <summary>
    /// Creates a new instance of <see cref="GxLightDirectional"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public GxLightDirectional() { }


    /// <summary>
    /// GxLightDirectional 0x001 chunk
    /// </summary>
    [Chunk(0x04007001)]
    public partial class Chunk04007001 : Chunk<GxLightDirectional>
    {
        /// <inheritdoc />
        public override uint Id => 0x04007001;


        public override void ReadWrite(GxLightDirectional n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.useBoundaryHint);
            rw.Vec3(ref n.boundaryHintPos);
        }
    }

    /// <summary>
    /// GxLightDirectional 0x002 chunk
    /// </summary>
    [Chunk(0x04007002)]
    public partial class Chunk04007002 : Chunk04007001
    {
        /// <inheritdoc />
        public override uint Id => 0x04007002;


        public override void ReadWrite(GxLightDirectional n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.Single(ref n.dazzleAngleMax);
            rw.Single(ref n.dazzleIntensity);
        }
    }

    /// <summary>
    /// GxLightDirectional 0x003 chunk
    /// </summary>
    [Chunk(0x04007003)]
    public partial class Chunk04007003 : Chunk<GxLightDirectional>
    {
        /// <inheritdoc />
        public override uint Id => 0x04007003;


        public override void ReadWrite(GxLightDirectional n, GbxReaderWriter rw)
        {
            rw.Vec3(ref n.dblSidedRGB);
        }
    }

    /// <summary>
    /// GxLightDirectional 0x004 chunk
    /// </summary>
    [Chunk(0x04007004)]
    public partial class Chunk04007004 : Chunk<GxLightDirectional>
    {
        /// <inheritdoc />
        public override uint Id => 0x04007004;


        public override void ReadWrite(GxLightDirectional n, GbxReaderWriter rw)
        {
            rw.Vec3(ref n.reverseRGB);
            rw.Single(ref n.reverseIntens);
        }
    }

    /// <summary>
    /// GxLightDirectional 0x005 chunk
    /// </summary>
    [Chunk(0x04007005)]
    public partial class Chunk04007005 : Chunk<GxLightDirectional>
    {
        /// <inheritdoc />
        public override uint Id => 0x04007005;


        public override void ReadWrite(GxLightDirectional n, GbxReaderWriter rw)
        {
            rw.Single(ref n.emittAngularSize);
            rw.Single(ref n.flareAngularSize);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x04007001 => new Chunk04007001(),
        0x04007002 => new Chunk04007002(),
        0x04007003 => new Chunk04007003(),
        0x04007004 => new Chunk04007004(),
        0x04007005 => new Chunk04007005(),
        _ => base.NewChunk(chunkId),
    };
}
