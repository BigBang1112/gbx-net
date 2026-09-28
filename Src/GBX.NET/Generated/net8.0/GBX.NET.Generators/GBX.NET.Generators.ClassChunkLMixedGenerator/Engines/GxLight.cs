namespace GBX.NET.Engines.Graphic;

/// <remarks>ID: 0x04001000</remarks>
[Class(0x04001000)]
public partial class GxLight : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x04001000;




    private Vec3 color;
    [AppliedWithChunk<Chunk04001008>]
    [AppliedWithChunk<Chunk04001009>]
    [AppliedWithChunk<Chunk0400100A>]
    public Vec3 Color { get => color; set => color = value; }

    private float intensity;
    [AppliedWithChunk<Chunk04001008>]
    [AppliedWithChunk<Chunk04001009>]
    [AppliedWithChunk<Chunk0400100A>]
    public float Intensity { get => intensity; set => intensity = value; }

    private EFlags flags;
    [AppliedWithChunk<Chunk04001008>]
    [AppliedWithChunk<Chunk04001009>]
    [AppliedWithChunk<Chunk0400100A>]
    public EFlags Flags { get => flags; set => flags = value; }

    private float shadowIntensity;
    [AppliedWithChunk<Chunk04001008>]
    [AppliedWithChunk<Chunk04001009>]
    [AppliedWithChunk<Chunk0400100A>]
    public float ShadowIntensity { get => shadowIntensity; set => shadowIntensity = value; }

    private float flareIntensity;
    [AppliedWithChunk<Chunk04001008>]
    [AppliedWithChunk<Chunk04001009>]
    [AppliedWithChunk<Chunk0400100A>]
    public float FlareIntensity { get => flareIntensity; set => flareIntensity = value; }

    private Vec3 shadowRGB;
    [AppliedWithChunk<Chunk04001008>]
    [AppliedWithChunk<Chunk04001009>]
    [AppliedWithChunk<Chunk0400100A>]
    public Vec3 ShadowRGB { get => shadowRGB; set => shadowRGB = value; }

    private float diffuseIntensity;
    [AppliedWithChunk<Chunk04001009>]
    [AppliedWithChunk<Chunk0400100A>]
    public float DiffuseIntensity { get => diffuseIntensity; set => diffuseIntensity = value; }

    private float specularIntens;
    [AppliedWithChunk<Chunk04001009>]
    public float SpecularIntens { get => specularIntens; set => specularIntens = value; }

    private float specularPower;
    [AppliedWithChunk<Chunk04001009>]
    public float SpecularPower { get => specularPower; set => specularPower = value; }


    /// <summary>
    /// GxLight 0x008 chunk
    /// </summary>
    [Chunk(0x04001008)]
    public partial class Chunk04001008 : Chunk<GxLight>
    {
        /// <inheritdoc />
        public override uint Id => 0x04001008;


        public override void ReadWrite(GxLight n, GbxReaderWriter rw)
        {
            rw.Vec3(ref n.color);
            rw.Single(ref n.intensity);
            rw.EnumInt32<EFlags>(ref n.flags);
            rw.Single(ref n.shadowIntensity);
            rw.Single(ref n.flareIntensity);
            rw.Vec3(ref n.shadowRGB);
        }
    }

    /// <summary>
    /// GxLight 0x009 chunk
    /// </summary>
    [Chunk(0x04001009)]
    public partial class Chunk04001009 : Chunk<GxLight>
    {
        /// <inheritdoc />
        public override uint Id => 0x04001009;


        public override void ReadWrite(GxLight n, GbxReaderWriter rw)
        {
            rw.Vec3(ref n.color);
            rw.EnumInt32<EFlags>(ref n.flags);
            rw.Single(ref n.intensity);
            rw.Single(ref n.diffuseIntensity);
            rw.Single(ref n.specularIntens);
            rw.Single(ref n.specularPower);
            rw.Single(ref n.shadowIntensity);
            rw.Single(ref n.flareIntensity);
            rw.Vec3(ref n.shadowRGB);
        }
    }

    /// <summary>
    /// GxLight 0x00A chunk
    /// </summary>
    [Chunk(0x0400100A)]
    public partial class Chunk0400100A : Chunk<GxLight>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0400100A;

        public int Version { get; set; }


        public override void ReadWrite(GxLight n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Vec3(ref n.color);
            rw.EnumInt32<EFlags>(ref n.flags);
            rw.Single(ref n.intensity);
            rw.Single(ref n.diffuseIntensity);
            rw.Single(ref n.shadowIntensity);
            rw.Single(ref n.flareIntensity);
            rw.Vec3(ref n.shadowRGB);
        }
    }



    public enum EFlags
    {
        None,
        DoLighting,
        LightMapOnly,
        ShadowGen = 4,
        Specular = 8,
        LensFlare = 16,
        Sprite = 32,
        EnableGroup0 = 64,
        EnableGroup1 = 128,
        EnableGroup2 = 256,
        EnableGroup3 = 512,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x04001008 => new Chunk04001008(),
        0x04001009 => new Chunk04001009(),
        0x0400100A => new Chunk0400100A(),
        _ => base.NewChunk(chunkId),
    };
}
