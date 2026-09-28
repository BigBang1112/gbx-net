namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0306E000</remarks>
[Class(0x0306E000)]
public partial class CGameControlCameraOrbital3d : CGameControlCameraTarget, IClass
{
    [Hexadecimal] public static new uint Id => 0x0306E000;




    private Vec3 radiusScale;
    [AppliedWithChunk<Chunk0306E001>]
    [AppliedWithChunk<Chunk0306E005>]
    public Vec3 RadiusScale { get => radiusScale; set => radiusScale = value; }

    private Vec2 rotateSpeed;
    [AppliedWithChunk<Chunk0306E001>]
    [AppliedWithChunk<Chunk0306E005>]
    public Vec2 RotateSpeed { get => rotateSpeed; set => rotateSpeed = value; }

    private bool occlusionIsEnable;
    [AppliedWithChunk<Chunk0306E001>]
    [AppliedWithChunk<Chunk0306E005>]
    public bool OcclusionIsEnable { get => occlusionIsEnable; set => occlusionIsEnable = value; }

    private float mouseBorderMoveSize;
    [AppliedWithChunk<Chunk0306E001>]
    [AppliedWithChunk<Chunk0306E005>]
    public float MouseBorderMoveSize { get => mouseBorderMoveSize; set => mouseBorderMoveSize = value; }

    private float occlusionTargetRadius;
    [AppliedWithChunk<Chunk0306E001>]
    [AppliedWithChunk<Chunk0306E005>]
    public float OcclusionTargetRadius { get => occlusionTargetRadius; set => occlusionTargetRadius = value; }

    private float occlusionDistFromHit;
    [AppliedWithChunk<Chunk0306E001>]
    [AppliedWithChunk<Chunk0306E005>]
    public float OcclusionDistFromHit { get => occlusionDistFromHit; set => occlusionDistFromHit = value; }

    private float radius;
    [AppliedWithChunk<Chunk0306E001>]
    [AppliedWithChunk<Chunk0306E005>]
    public float Radius { get => radius; set => radius = value; }

    private float latitude;
    [AppliedWithChunk<Chunk0306E001>]
    [AppliedWithChunk<Chunk0306E005>]
    public float Latitude { get => latitude; set => latitude = value; }

    private float longitude;
    [AppliedWithChunk<Chunk0306E001>]
    [AppliedWithChunk<Chunk0306E005>]
    public float Longitude { get => longitude; set => longitude = value; }

    private float radiusMin;
    [AppliedWithChunk<Chunk0306E001>]
    [AppliedWithChunk<Chunk0306E005>]
    public float RadiusMin { get => radiusMin; set => radiusMin = value; }

    private float radiusMax;
    [AppliedWithChunk<Chunk0306E001>]
    [AppliedWithChunk<Chunk0306E005>]
    public float RadiusMax { get => radiusMax; set => radiusMax = value; }

    private float latitudeMin;
    [AppliedWithChunk<Chunk0306E001>]
    [AppliedWithChunk<Chunk0306E005>]
    public float LatitudeMin { get => latitudeMin; set => latitudeMin = value; }

    private float latitudeMax;
    [AppliedWithChunk<Chunk0306E001>]
    [AppliedWithChunk<Chunk0306E005>]
    public float LatitudeMax { get => latitudeMax; set => latitudeMax = value; }

    private float wheelSensitivity;
    [AppliedWithChunk<Chunk0306E003>]
    [AppliedWithChunk<Chunk0306E005>]
    public float WheelSensitivity { get => wheelSensitivity; set => wheelSensitivity = value; }

    private float fovKeySensitivity;
    [AppliedWithChunk<Chunk0306E004>]
    [AppliedWithChunk<Chunk0306E005>]
    public float FovKeySensitivity { get => fovKeySensitivity; set => fovKeySensitivity = value; }

    private float zoomKeySensitivity;
    [AppliedWithChunk<Chunk0306E006>]
    public float ZoomKeySensitivity { get => zoomKeySensitivity; set => zoomKeySensitivity = value; }

    private float defaultRadius;
    [AppliedWithChunk<Chunk0306E007>]
    public float DefaultRadius { get => defaultRadius; set => defaultRadius = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameControlCameraOrbital3d"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameControlCameraOrbital3d() { }


    /// <summary>
    /// CGameControlCameraOrbital3d 0x001 chunk
    /// </summary>
    [Chunk(0x0306E001)]
    public partial class Chunk0306E001 : Chunk<CGameControlCameraOrbital3d>
    {
        /// <inheritdoc />
        public override uint Id => 0x0306E001;

        public bool U01;
        public bool U02;
        public float U03;
        public float U04;

        public override void ReadWrite(CGameControlCameraOrbital3d n, GbxReaderWriter rw)
        {
            rw.Vec3(ref n.radiusScale);
            rw.Vec2(ref n.rotateSpeed);
            rw.Boolean(ref n.occlusionIsEnable);
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.Single(ref n.mouseBorderMoveSize);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref n.occlusionTargetRadius);
            rw.Single(ref n.occlusionDistFromHit);
            rw.Single(ref n.radius);
            rw.Single(ref n.latitude);
            rw.Single(ref n.longitude);
            rw.Single(ref n.radiusMin);
            rw.Single(ref n.radiusMax);
            rw.Single(ref n.latitudeMin);
            rw.Single(ref n.latitudeMax);
        }
    }

    /// <summary>
    /// CGameControlCameraOrbital3d 0x002 chunk
    /// </summary>
    [Chunk(0x0306E002)]
    public partial class Chunk0306E002 : Chunk<CGameControlCameraOrbital3d>
    {
        /// <inheritdoc />
        public override uint Id => 0x0306E002;

        public float U01;

        public override void ReadWrite(CGameControlCameraOrbital3d n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CGameControlCameraOrbital3d 0x003 chunk
    /// </summary>
    [Chunk(0x0306E003)]
    public partial class Chunk0306E003 : Chunk<CGameControlCameraOrbital3d>
    {
        /// <inheritdoc />
        public override uint Id => 0x0306E003;


        public override void ReadWrite(CGameControlCameraOrbital3d n, GbxReaderWriter rw)
        {
            rw.Single(ref n.wheelSensitivity);
        }
    }

    /// <summary>
    /// CGameControlCameraOrbital3d 0x004 chunk
    /// </summary>
    [Chunk(0x0306E004)]
    public partial class Chunk0306E004 : Chunk<CGameControlCameraOrbital3d>
    {
        /// <inheritdoc />
        public override uint Id => 0x0306E004;


        public override void ReadWrite(CGameControlCameraOrbital3d n, GbxReaderWriter rw)
        {
            rw.Single(ref n.fovKeySensitivity);
        }
    }

    /// <summary>
    /// CGameControlCameraOrbital3d 0x005 chunk
    /// </summary>
    [Chunk(0x0306E005)]
    public partial class Chunk0306E005 : Chunk<CGameControlCameraOrbital3d>
    {
        /// <inheritdoc />
        public override uint Id => 0x0306E005;

        public bool U01;
        public bool U02;
        public float U03;

        public override void ReadWrite(CGameControlCameraOrbital3d n, GbxReaderWriter rw)
        {
            rw.Vec3(ref n.radiusScale);
            rw.Vec2(ref n.rotateSpeed);
            rw.Boolean(ref n.occlusionIsEnable);
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.Single(ref n.mouseBorderMoveSize);
            rw.Single(ref n.occlusionTargetRadius);
            rw.Single(ref n.occlusionDistFromHit);
            rw.Single(ref n.radius);
            rw.Single(ref n.latitude);
            rw.Single(ref n.longitude);
            rw.Single(ref n.radiusMin);
            rw.Single(ref n.radiusMax);
            rw.Single(ref n.latitudeMin);
            rw.Single(ref n.latitudeMax);
            rw.Single(ref U03);
            rw.Single(ref n.wheelSensitivity);
            rw.Single(ref n.fovKeySensitivity);
        }
    }

    /// <summary>
    /// CGameControlCameraOrbital3d 0x006 chunk
    /// </summary>
    [Chunk(0x0306E006)]
    public partial class Chunk0306E006 : Chunk<CGameControlCameraOrbital3d>
    {
        /// <inheritdoc />
        public override uint Id => 0x0306E006;


        public override void ReadWrite(CGameControlCameraOrbital3d n, GbxReaderWriter rw)
        {
            rw.Single(ref n.zoomKeySensitivity);
        }
    }

    /// <summary>
    /// CGameControlCameraOrbital3d 0x007 chunk
    /// </summary>
    [Chunk(0x0306E007)]
    public partial class Chunk0306E007 : Chunk<CGameControlCameraOrbital3d>
    {
        /// <inheritdoc />
        public override uint Id => 0x0306E007;


        public override void ReadWrite(CGameControlCameraOrbital3d n, GbxReaderWriter rw)
        {
            rw.Single(ref n.defaultRadius);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0306E001 => new Chunk0306E001(),
        0x0306E002 => new Chunk0306E002(),
        0x0306E003 => new Chunk0306E003(),
        0x0306E004 => new Chunk0306E004(),
        0x0306E005 => new Chunk0306E005(),
        0x0306E006 => new Chunk0306E006(),
        0x0306E007 => new Chunk0306E007(),
        _ => base.NewChunk(chunkId),
    };
}
