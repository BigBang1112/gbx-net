namespace GBX.NET.Engines.TrackMania;

/// <remarks>ID: 0x24085000</remarks>
[Class(0x24085000)]
public partial class CGameControlCameraTrackManiaRace : CGameControlCameraTarget, IClass
{
    [Hexadecimal] public static new uint Id => 0x24085000;




    private float coneAperture;
    [AppliedWithChunk<Chunk24085000>]
    public float ConeAperture { get => coneAperture; set => coneAperture = value; }

    private float coneMinSpeed;
    [AppliedWithChunk<Chunk24085000>]
    public float ConeMinSpeed { get => coneMinSpeed; set => coneMinSpeed = value; }

    private float coneMaxSpeed;
    [AppliedWithChunk<Chunk24085000>]
    public float ConeMaxSpeed { get => coneMaxSpeed; set => coneMaxSpeed = value; }

    private bool useSpeedDir;
    [AppliedWithChunk<Chunk24085000>]
    public bool UseSpeedDir { get => useSpeedDir; set => useSpeedDir = value; }

    private float carCameraHeight;
    [AppliedWithChunk<Chunk24085000>]
    public float CarCameraHeight { get => carCameraHeight; set => carCameraHeight = value; }

    private float carCameraDistance;
    [AppliedWithChunk<Chunk24085000>]
    public float CarCameraDistance { get => carCameraDistance; set => carCameraDistance = value; }

    private float carCameraTargetDistance;
    [AppliedWithChunk<Chunk24085000>]
    public float CarCameraTargetDistance { get => carCameraTargetDistance; set => carCameraTargetDistance = value; }

    private float carCameraAlign;
    [AppliedWithChunk<Chunk24085000>]
    public float CarCameraAlign { get => carCameraAlign; set => carCameraAlign = value; }

    private bool isSegmentCast;
    [AppliedWithChunk<Chunk24085000>]
    public bool IsSegmentCast { get => isSegmentCast; set => isSegmentCast = value; }

    private float segmentCastMinDist;
    [AppliedWithChunk<Chunk24085000>]
    public float SegmentCastMinDist { get => segmentCastMinDist; set => segmentCastMinDist = value; }

    private float segmentCastLength;
    [AppliedWithChunk<Chunk24085000>]
    public float SegmentCastLength { get => segmentCastLength; set => segmentCastLength = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameControlCameraTrackManiaRace"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameControlCameraTrackManiaRace() { }


    /// <summary>
    /// CGameControlCameraTrackManiaRace 0x000 chunk
    /// </summary>
    [Chunk(0x24085000)]
    public partial class Chunk24085000 : Chunk<CGameControlCameraTrackManiaRace>
    {
        /// <inheritdoc />
        public override uint Id => 0x24085000;

        public float U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CGameControlCameraTrackManiaRace n, GbxReaderWriter rw)
        {
            rw.Single(ref n.coneAperture);
            rw.Single(ref n.coneMinSpeed);
            rw.Single(ref n.coneMaxSpeed);
            rw.Boolean(ref n.useSpeedDir);
            rw.Single(ref n.carCameraHeight);
            rw.Single(ref n.carCameraDistance);
            rw.Single(ref n.carCameraTargetDistance);
            rw.Single(ref n.carCameraAlign);
            rw.Boolean(ref n.isSegmentCast);
            rw.Single(ref n.segmentCastMinDist);
            rw.Single(ref n.segmentCastLength);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x24085000 => new Chunk24085000(),
        _ => base.NewChunk(chunkId),
    };
}
