namespace GBX.NET.Engines.Graphic;

/// <remarks>ID: 0x04002000</remarks>
[Class(0x04002000)]
public partial class GxLightBall : GxLightPoint, IClass
{
    [Hexadecimal] public static new uint Id => 0x04002000;




    private float radius;
    [AppliedWithChunk<Chunk04002002>]
    [AppliedWithChunk<Chunk04002006>]
    [AppliedWithChunk<Chunk04002008>]
    public float Radius { get => radius; set => radius = value; }

    private float attenuation1;
    [AppliedWithChunk<Chunk04002002>]
    [AppliedWithChunk<Chunk04002006>]
    public float Attenuation1 { get => attenuation1; set => attenuation1 = value; }

    private float attenuation2;
    [AppliedWithChunk<Chunk04002002>]
    [AppliedWithChunk<Chunk04002006>]
    public float Attenuation2 { get => attenuation2; set => attenuation2 = value; }

    private float emittingRadius;
    [AppliedWithChunk<Chunk04002002>]
    [AppliedWithChunk<Chunk04002006>]
    [AppliedWithChunk<Chunk04002008>]
    public float EmittingRadius { get => emittingRadius; set => emittingRadius = value; }

    private Vec3 ambientRGB;
    [AppliedWithChunk<Chunk04002002>]
    [AppliedWithChunk<Chunk04002006>]
    [AppliedWithChunk<Chunk04002008>]
    public Vec3 AmbientRGB { get => ambientRGB; set => ambientRGB = value; }

    private uint flags;
    [AppliedWithChunk<Chunk04002006>]
    [AppliedWithChunk<Chunk04002008>]
    public uint Flags { get => flags; set => flags = value; }

    private float radiusSpecular;
    [AppliedWithChunk<Chunk04002006>]
    [AppliedWithChunk<Chunk04002008>]
    public float RadiusSpecular { get => radiusSpecular; set => radiusSpecular = value; }

    private float radiusShadow;
    [AppliedWithChunk<Chunk04002006>]
    [AppliedWithChunk<Chunk04002008>]
    public float RadiusShadow { get => radiusShadow; set => radiusShadow = value; }

    private float radiusFlare;
    [AppliedWithChunk<Chunk04002006>]
    [AppliedWithChunk<Chunk04002008>]
    public float RadiusFlare { get => radiusFlare; set => radiusFlare = value; }

    private float emittingCylinderLenZ;
    [AppliedWithChunk<Chunk04002008>]
    public float EmittingCylinderLenZ { get => emittingCylinderLenZ; set => emittingCylinderLenZ = value; }

    private float attHTnLR;
    [AppliedWithChunk<Chunk04002008>]
    public float AttHTnLR { get => attHTnLR; set => attHTnLR = value; }

    private float attHTnLR2;
    [AppliedWithChunk<Chunk04002008>]
    public float AttHTnLR2 { get => attHTnLR2; set => attHTnLR2 = value; }

    private float attHyper2DerivAt0;
    [AppliedWithChunk<Chunk04002008>]
    public float AttHyper2DerivAt0 { get => attHyper2DerivAt0; set => attHyper2DerivAt0 = value; }

    private float attHyper2Tension;
    [AppliedWithChunk<Chunk04002008>]
    public float AttHyper2Tension { get => attHyper2Tension; set => attHyper2Tension = value; }

    /// <summary>
    /// Creates a new instance of <see cref="GxLightBall"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public GxLightBall() { }


    /// <summary>
    /// GxLightBall 0x002 chunk
    /// </summary>
    [Chunk(0x04002002)]
    [ChunkGameVersion(GameVersion.TM10)]
    public partial class Chunk04002002 : Chunk<GxLightBall>
    {
        /// <inheritdoc />
        public override uint Id => 0x04002002;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10;


        public override void ReadWrite(GxLightBall n, GbxReaderWriter rw)
        {
            rw.Single(ref n.radius);
            rw.Single(ref n.attenuation1);
            rw.Single(ref n.attenuation2);
            rw.Single(ref n.emittingRadius);
            rw.Vec3(ref n.ambientRGB);
        }
    }

    /// <summary>
    /// GxLightBall 0x006 chunk
    /// </summary>
    [Chunk(0x04002006)]
    public partial class Chunk04002006 : Chunk<GxLightBall>
    {
        /// <inheritdoc />
        public override uint Id => 0x04002006;


        public override void ReadWrite(GxLightBall n, GbxReaderWriter rw)
        {
            rw.UInt32(ref n.flags);
            rw.Single(ref n.radius);
            rw.Single(ref n.radiusSpecular);
            rw.Single(ref n.radiusShadow);
            rw.Single(ref n.radiusFlare);
            rw.Single(ref n.emittingRadius);
            rw.Single(ref n.attenuation1);
            rw.Single(ref n.attenuation2);
            rw.Vec3(ref n.ambientRGB);
        }
    }

    /// <summary>
    /// GxLightBall 0x008 chunk
    /// </summary>
    [Chunk(0x04002008)]
    public partial class Chunk04002008 : Chunk<GxLightBall>
    {
        /// <inheritdoc />
        public override uint Id => 0x04002008;


        public override void ReadWrite(GxLightBall n, GbxReaderWriter rw)
        {
            rw.UInt32(ref n.flags);
            rw.Single(ref n.radius);
            rw.Single(ref n.radiusSpecular);
            rw.Single(ref n.radiusShadow);
            rw.Single(ref n.radiusFlare);
            rw.Single(ref n.emittingRadius);
            rw.Single(ref n.emittingCylinderLenZ);
            rw.Single(ref n.attHTnLR);
            rw.Single(ref n.attHTnLR2);
            rw.Vec3(ref n.ambientRGB);
            rw.Single(ref n.attHyper2DerivAt0);
            rw.Single(ref n.attHyper2Tension);
        }
    }

    /// <summary>
    /// GxLightBall 0x009 chunk
    /// </summary>
    [Chunk(0x04002009)]
    public partial class Chunk04002009 : Chunk<GxLightBall>
    {
        /// <inheritdoc />
        public override uint Id => 0x04002009;

        public float U01;

        public override void ReadWrite(GxLightBall n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// GxLightBall 0x00A chunk
    /// </summary>
    [Chunk(0x0400200A)]
    public partial class Chunk0400200A : Chunk<GxLightBall>
    {
        /// <inheritdoc />
        public override uint Id => 0x0400200A;

        public float U01;

        public override void ReadWrite(GxLightBall n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x04002002 => new Chunk04002002(),
        0x04002006 => new Chunk04002006(),
        0x04002008 => new Chunk04002008(),
        0x04002009 => new Chunk04002009(),
        0x0400200A => new Chunk0400200A(),
        _ => base.NewChunk(chunkId),
    };
}
