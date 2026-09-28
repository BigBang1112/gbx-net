namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090F9000</remarks>
[Class(0x090F9000)]
public partial class CPlugLightUserModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090F9000;




    private Vec3 color;
    [AppliedWithChunk<Chunk090F9000>]
    public Vec3 Color { get => color; set => color = value; }

    private float intensity;
    [AppliedWithChunk<Chunk090F9000>]
    public float Intensity { get => intensity; set => intensity = value; }

    private float distance;
    [AppliedWithChunk<Chunk090F9000>]
    public float Distance { get => distance; set => distance = value; }

    private float pointEmissionRadius;
    [AppliedWithChunk<Chunk090F9000>]
    public float PointEmissionRadius { get => pointEmissionRadius; set => pointEmissionRadius = value; }

    private float pointEmissionLength;
    [AppliedWithChunk<Chunk090F9000>]
    public float PointEmissionLength { get => pointEmissionLength; set => pointEmissionLength = value; }

    private float spotInnerAngle;
    [AppliedWithChunk<Chunk090F9000>]
    public float SpotInnerAngle { get => spotInnerAngle; set => spotInnerAngle = value; }

    private float spotOuterAngle;
    [AppliedWithChunk<Chunk090F9000>]
    public float SpotOuterAngle { get => spotOuterAngle; set => spotOuterAngle = value; }

    private float spotEmissionSizeX;
    [AppliedWithChunk<Chunk090F9000>]
    public float SpotEmissionSizeX { get => spotEmissionSizeX; set => spotEmissionSizeX = value; }

    private float spotEmissionSizeY;
    [AppliedWithChunk<Chunk090F9000>]
    public float SpotEmissionSizeY { get => spotEmissionSizeY; set => spotEmissionSizeY = value; }

    private bool nightOnly;
    [AppliedWithChunk<Chunk090F9000>]
    public bool NightOnly { get => nightOnly; set => nightOnly = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugLightUserModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugLightUserModel() { }


    /// <summary>
    /// CPlugLightUserModel 0x000 chunk
    /// </summary>
    [Chunk(0x090F9000)]
    public partial class Chunk090F9000 : Chunk<CPlugLightUserModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090F9000;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CPlugLightUserModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.Vec3(ref n.color);
            rw.Single(ref n.intensity);
            rw.Single(ref n.distance);
            rw.Single(ref n.pointEmissionRadius);
            rw.Single(ref n.pointEmissionLength);
            rw.Single(ref n.spotInnerAngle);
            rw.Single(ref n.spotOuterAngle);
            rw.Single(ref n.spotEmissionSizeX);
            rw.Single(ref n.spotEmissionSizeY);
            if (Version >= 1)
            {
                rw.Boolean(ref n.nightOnly);
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090F9000 => new Chunk090F9000(),
        _ => base.NewChunk(chunkId),
    };
}
