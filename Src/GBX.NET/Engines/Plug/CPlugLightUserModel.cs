namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090F9000</remarks>
[Node(0x090F9000)]
public class CPlugLightUserModel : CMwNod
{
    private Vec3 color;
    private float intensity;
    private float distance;
    private float pointEmissionRadius;
    private float pointEmissionLength;
    private float spotInnerAngle;
    private float spotOuterAngle;
    private float spotEmissionSizeX;
    private float spotEmissionSizeY;
    private bool nightOnly;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090F9000))]
    public Vec3 Color { get => color; set => color = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090F9000))]
    public float Intensity { get => intensity; set => intensity = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090F9000))]
    public float Distance { get => distance; set => distance = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090F9000))]
    public float PointEmissionRadius { get => pointEmissionRadius; set => pointEmissionRadius = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090F9000))]
    public float PointEmissionLength { get => pointEmissionLength; set => pointEmissionLength = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090F9000))]
    public float SpotInnerAngle { get => spotInnerAngle; set => spotInnerAngle = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090F9000))]
    public float SpotOuterAngle { get => spotOuterAngle; set => spotOuterAngle = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090F9000))]
    public float SpotEmissionSizeX { get => spotEmissionSizeX; set => spotEmissionSizeX = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090F9000))]
    public float SpotEmissionSizeY { get => spotEmissionSizeY; set => spotEmissionSizeY = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090F9000), sinceVersion: 1)]
    public bool NightOnly { get => nightOnly; set => nightOnly = value; }

    internal CPlugLightUserModel()
	{

	}

    #region 0x000 chunk

    /// <summary>
    /// CPlugLightUserModel 0x000 chunk
    /// </summary>
    [Chunk(0x090F9000)]
    public class Chunk090F9000 : Chunk<CPlugLightUserModel>, IVersionable
    {
        public int U01;

        public int Version { get; set; }

        public override void ReadWrite(CPlugLightUserModel n, GameBoxReaderWriter rw)
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

    #endregion
}
