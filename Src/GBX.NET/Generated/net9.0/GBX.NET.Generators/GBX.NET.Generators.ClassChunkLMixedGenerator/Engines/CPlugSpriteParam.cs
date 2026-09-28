namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090AC000</remarks>
[Class(0x090AC000)]
public partial class CPlugSpriteParam : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x090AC000;




    private Vec3 globalDirection;
    [AppliedWithChunk<Chunk090AC000>]
    [AppliedWithChunk<Chunk090AC001>]
    public Vec3 GlobalDirection { get => globalDirection; set => globalDirection = value; }

    private Vec2 pivotPoint;
    [AppliedWithChunk<Chunk090AC000>]
    [AppliedWithChunk<Chunk090AC001>]
    public Vec2 PivotPoint { get => pivotPoint; set => pivotPoint = value; }

    private float globalDirTiltFactor;
    [AppliedWithChunk<Chunk090AC000>]
    [AppliedWithChunk<Chunk090AC001>]
    public float GlobalDirTiltFactor { get => globalDirTiltFactor; set => globalDirTiltFactor = value; }

    private float textureHeightInWorld;
    [AppliedWithChunk<Chunk090AC001>]
    public float TextureHeightInWorld { get => textureHeightInWorld; set => textureHeightInWorld = value; }

    private float? visibleMaxDistAtFov90;
    [AppliedWithChunk<Chunk090AC001>]
    public float? VisibleMaxDistAtFov90 { get => visibleMaxDistAtFov90; set => visibleMaxDistAtFov90 = value; }

    private float? visibleMinScreenHeight01;
    [AppliedWithChunk<Chunk090AC001>]
    public float? VisibleMinScreenHeight01 { get => visibleMinScreenHeight01; set => visibleMinScreenHeight01 = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugSpriteParam"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugSpriteParam() { }


    /// <summary>
    /// CPlugSpriteParam 0x000 chunk
    /// </summary>
    [Chunk(0x090AC000)]
    public partial class Chunk090AC000 : Chunk<CPlugSpriteParam>
    {
        /// <inheritdoc />
        public override uint Id => 0x090AC000;

        public uint U01;

        public override void ReadWrite(CPlugSpriteParam n, GbxReaderWriter rw)
        {
            rw.UInt32(ref U01);
            rw.Vec3(ref n.globalDirection);
            rw.Vec2(ref n.pivotPoint);
            rw.Single(ref n.globalDirTiltFactor);
        }
    }

    /// <summary>
    /// CPlugSpriteParam 0x001 chunk
    /// </summary>
    [Chunk(0x090AC001)]
    public partial class Chunk090AC001 : Chunk090AC000, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090AC001;

        public int Version { get; set; }


        public override void ReadWrite(CPlugSpriteParam n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            base.ReadWrite(n, rw);
            rw.Single(ref n.textureHeightInWorld);
            if (Version >= 1)
            {
                rw.Single(ref n.visibleMaxDistAtFov90);
                rw.Single(ref n.visibleMinScreenHeight01);
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090AC000 => new Chunk090AC000(),
        0x090AC001 => new Chunk090AC001(),
        _ => base.NewChunk(chunkId),
    };
}
