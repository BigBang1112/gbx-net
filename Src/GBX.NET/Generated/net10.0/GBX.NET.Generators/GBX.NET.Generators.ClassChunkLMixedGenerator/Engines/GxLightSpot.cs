namespace GBX.NET.Engines.Graphic;

/// <remarks>ID: 0x0400B000</remarks>
[Class(0x0400B000)]
public partial class GxLightSpot : GxLightBall, IClass
{
    [Hexadecimal] public static new uint Id => 0x0400B000;




    private float angleInner;
    [AppliedWithChunk<Chunk0400B001>]
    [AppliedWithChunk<Chunk0400B002>]
    [AppliedWithChunk<Chunk0400B003>]
    public float AngleInner { get => angleInner; set => angleInner = value; }

    private float angleOuter;
    [AppliedWithChunk<Chunk0400B001>]
    [AppliedWithChunk<Chunk0400B002>]
    [AppliedWithChunk<Chunk0400B003>]
    public float AngleOuter { get => angleOuter; set => angleOuter = value; }

    private float angleFlare;
    [AppliedWithChunk<Chunk0400B001>]
    [AppliedWithChunk<Chunk0400B002>]
    [AppliedWithChunk<Chunk0400B003>]
    public float AngleFlare { get => angleFlare; set => angleFlare = value; }

    private float falloffExponent;
    [AppliedWithChunk<Chunk0400B001>]
    [AppliedWithChunk<Chunk0400B002>]
    [AppliedWithChunk<Chunk0400B003>]
    public float FalloffExponent { get => falloffExponent; set => falloffExponent = value; }

    private uint flags;
    [AppliedWithChunk<Chunk0400B002>]
    [AppliedWithChunk<Chunk0400B003>]
    public uint Flags { get => flags; set => flags = value; }

    private float angleInnerShadow;
    [AppliedWithChunk<Chunk0400B002>]
    [AppliedWithChunk<Chunk0400B003>]
    public float AngleInnerShadow { get => angleInnerShadow; set => angleInnerShadow = value; }

    private float angleOuterShadow;
    [AppliedWithChunk<Chunk0400B002>]
    [AppliedWithChunk<Chunk0400B003>]
    public float AngleOuterShadow { get => angleOuterShadow; set => angleOuterShadow = value; }

    /// <summary>
    /// Creates a new instance of <see cref="GxLightSpot"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public GxLightSpot() { }


    /// <summary>
    /// GxLightSpot 0x001 chunk
    /// </summary>
    [Chunk(0x0400B001)]
    public partial class Chunk0400B001 : Chunk<GxLightSpot>
    {
        /// <inheritdoc />
        public override uint Id => 0x0400B001;


        public override void ReadWrite(GxLightSpot n, GbxReaderWriter rw)
        {
            rw.Single(ref n.angleInner);
            rw.Single(ref n.angleOuter);
            rw.Single(ref n.angleFlare);
            rw.Single(ref n.falloffExponent);
        }
    }

    /// <summary>
    /// GxLightSpot 0x002 chunk
    /// </summary>
    [Chunk(0x0400B002)]
    public partial class Chunk0400B002 : Chunk<GxLightSpot>
    {
        /// <inheritdoc />
        public override uint Id => 0x0400B002;


        public override void ReadWrite(GxLightSpot n, GbxReaderWriter rw)
        {
            rw.UInt32(ref n.flags);
            rw.Single(ref n.angleInner);
            rw.Single(ref n.angleOuter);
            rw.Single(ref n.angleFlare);
            rw.Single(ref n.angleInnerShadow);
            rw.Single(ref n.angleOuterShadow);
            rw.Single(ref n.falloffExponent);
        }
    }

    /// <summary>
    /// GxLightSpot 0x003 chunk
    /// </summary>
    [Chunk(0x0400B003)]
    public partial class Chunk0400B003 : Chunk<GxLightSpot>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0400B003;

        public int Version { get; set; }

        public byte U01;
        public byte U02;
        public int U03;

        public override void ReadWrite(GxLightSpot n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.UInt32(ref n.flags);
            rw.Single(ref n.angleInner);
            rw.Single(ref n.angleOuter);
            rw.Single(ref n.angleFlare);
            rw.Single(ref n.angleInnerShadow);
            rw.Single(ref n.angleOuterShadow);
            rw.Single(ref n.falloffExponent);
            if (Version >= 1)
            {
                rw.Byte(ref U01);
                rw.Byte(ref U02);
            }
            if (Version == 0)
            {
                rw.Int32(ref U03);
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0400B001 => new Chunk0400B001(),
        0x0400B002 => new Chunk0400B002(),
        0x0400B003 => new Chunk0400B003(),
        _ => base.NewChunk(chunkId),
    };
}
