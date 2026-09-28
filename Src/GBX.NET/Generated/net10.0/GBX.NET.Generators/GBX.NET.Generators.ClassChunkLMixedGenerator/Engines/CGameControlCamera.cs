namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0306B000</remarks>
[Class(0x0306B000)]
public partial class CGameControlCamera : CSceneController, IClass
{
    [Hexadecimal] public static new uint Id => 0x0306B000;




    private Vec3 relativeTargetPos;
    [AppliedWithChunk<Chunk0306B001>]
    public Vec3 RelativeTargetPos { get => relativeTargetPos; set => relativeTargetPos = value; }

    private bool isFirstPerson;
    [AppliedWithChunk<Chunk0306B001>]
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public bool IsFirstPerson { get => isFirstPerson; set => isFirstPerson = value; }

    private bool canCameraMove;
    [AppliedWithChunk<Chunk0306B001>]
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public bool CanCameraMove { get => canCameraMove; set => canCameraMove = value; }

    private bool isFollowing;
    [AppliedWithChunk<Chunk0306B001>]
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public bool IsFollowing { get => isFollowing; set => isFollowing = value; }

    private float maxSpeed;
    [AppliedWithChunk<Chunk0306B001>]
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }

    private float planeDist;
    [AppliedWithChunk<Chunk0306B001>]
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public float PlaneDist { get => planeDist; set => planeDist = value; }

    private float minDist;
    [AppliedWithChunk<Chunk0306B001>]
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public float MinDist { get => minDist; set => minDist = value; }

    private float maxDist;
    [AppliedWithChunk<Chunk0306B001>]
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public float MaxDist { get => maxDist; set => maxDist = value; }

    private float fov;
    [AppliedWithChunk<Chunk0306B001>]
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public float Fov { get => fov; set => fov = value; }

    private float defaultFov;
    [AppliedWithChunk<Chunk0306B001>]
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public float DefaultFov { get => defaultFov; set => defaultFov = value; }

    private bool useForcedLocation;
    [AppliedWithChunk<Chunk0306B001>]
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public bool UseForcedLocation { get => useForcedLocation; set => useForcedLocation = value; }

    private Iso4 forcedLocation;
    [AppliedWithChunk<Chunk0306B001>]
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public Iso4 ForcedLocation { get => forcedLocation; set => forcedLocation = value; }

    private bool useOnlyFollowedMobilPosition;
    [AppliedWithChunk<Chunk0306B002>]
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public bool UseOnlyFollowedMobilPosition { get => useOnlyFollowedMobilPosition; set => useOnlyFollowedMobilPosition = value; }

    private string? name;
    [AppliedWithChunk<Chunk0306B003>]
    [AppliedWithChunk<Chunk0306B004>]
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00A>]
    public string? Name { get => name; set => name = value; }

    private Vec3 relativeFollowedPos;
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public Vec3 RelativeFollowedPos { get => relativeFollowedPos; set => relativeFollowedPos = value; }

    private float minFov;
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public float MinFov { get => minFov; set => minFov = value; }

    private float maxFov;
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public float MaxFov { get => maxFov; set => maxFov = value; }

    private bool useForcedUp;
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public bool UseForcedUp { get => useForcedUp; set => useForcedUp = value; }

    private Vec3 forcedUp;
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public Vec3 ForcedUp { get => forcedUp; set => forcedUp = value; }

    private float defaultNearZ;
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public float DefaultNearZ { get => defaultNearZ; set => defaultNearZ = value; }

    private float minNearZ;
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public float MinNearZ { get => minNearZ; set => minNearZ = value; }

    private float maxNearZ;
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public float MaxNearZ { get => maxNearZ; set => maxNearZ = value; }

    private float defaultFarZ;
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public float DefaultFarZ { get => defaultFarZ; set => defaultFarZ = value; }

    private float minFarZ;
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public float MinFarZ { get => minFarZ; set => minFarZ = value; }

    private float maxFarZ;
    [AppliedWithChunk<Chunk0306B009>]
    [AppliedWithChunk<Chunk0306B00B>]
    public float MaxFarZ { get => maxFarZ; set => maxFarZ = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameControlCamera"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameControlCamera() { }


    /// <summary>
    /// CGameControlCamera 0x001 chunk
    /// </summary>
    [Chunk(0x0306B001)]
    public partial class Chunk0306B001 : Chunk<CGameControlCamera>
    {
        /// <inheritdoc />
        public override uint Id => 0x0306B001;


        public override void ReadWrite(CGameControlCamera n, GbxReaderWriter rw)
        {
            rw.Vec3(ref n.relativeTargetPos);
            rw.Boolean(ref n.isFirstPerson);
            rw.Boolean(ref n.canCameraMove);
            rw.Boolean(ref n.isFollowing);
            rw.Single(ref n.maxSpeed);
            rw.Single(ref n.planeDist);
            rw.Single(ref n.minDist);
            rw.Single(ref n.maxDist);
            rw.Single(ref n.fov);
            rw.Single(ref n.defaultFov);
            rw.Boolean(ref n.useForcedLocation);
            rw.Iso4(ref n.forcedLocation);
        }
    }

    /// <summary>
    /// CGameControlCamera 0x002 chunk
    /// </summary>
    [Chunk(0x0306B002)]
    public partial class Chunk0306B002 : Chunk<CGameControlCamera>
    {
        /// <inheritdoc />
        public override uint Id => 0x0306B002;


        public override void ReadWrite(CGameControlCamera n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.useOnlyFollowedMobilPosition);
        }
    }

    /// <summary>
    /// CGameControlCamera 0x003 chunk
    /// </summary>
    [Chunk(0x0306B003)]
    public partial class Chunk0306B003 : Chunk<CGameControlCamera>
    {
        /// <inheritdoc />
        public override uint Id => 0x0306B003;


        public override void ReadWrite(CGameControlCamera n, GbxReaderWriter rw)
        {
            rw.Id(ref n.name);
        }
    }

    /// <summary>
    /// CGameControlCamera 0x004 chunk
    /// </summary>
    [Chunk(0x0306B004)]
    public partial class Chunk0306B004 : Chunk<CGameControlCamera>
    {
        /// <inheritdoc />
        public override uint Id => 0x0306B004;


        public override void ReadWrite(CGameControlCamera n, GbxReaderWriter rw)
        {
            rw.String(ref n.name);
        }
    }

    /// <summary>
    /// CGameControlCamera 0x009 chunk
    /// </summary>
    [Chunk(0x0306B009)]
    public partial class Chunk0306B009 : Chunk<CGameControlCamera>
    {
        /// <inheritdoc />
        public override uint Id => 0x0306B009;

        public bool U01;

        public override void ReadWrite(CGameControlCamera n, GbxReaderWriter rw)
        {
            rw.Vec3(ref n.relativeFollowedPos);
            rw.Boolean(ref n.isFirstPerson);
            rw.Boolean(ref n.canCameraMove);
            rw.Boolean(ref n.isFollowing);
            rw.Single(ref n.maxSpeed);
            rw.Single(ref n.planeDist);
            rw.Single(ref n.minDist);
            rw.Single(ref n.maxDist);
            rw.Single(ref n.fov);
            rw.Single(ref n.defaultFov);
            rw.Boolean(ref n.useForcedLocation);
            rw.Iso4(ref n.forcedLocation);
            rw.Boolean(ref n.useOnlyFollowedMobilPosition);
            rw.Id(ref n.name);
            rw.String(ref n.name);
            rw.Single(ref n.minFov);
            rw.Single(ref n.maxFov);
            rw.Boolean(ref n.useForcedUp);
            rw.Vec3(ref n.forcedUp);
            rw.Single(ref n.defaultNearZ);
            rw.Single(ref n.minNearZ);
            rw.Single(ref n.maxNearZ);
            rw.Single(ref n.defaultFarZ);
            rw.Single(ref n.minFarZ);
            rw.Single(ref n.maxFarZ);
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CGameControlCamera 0x00A chunk
    /// </summary>
    [Chunk(0x0306B00A)]
    public partial class Chunk0306B00A : Chunk<CGameControlCamera>
    {
        /// <inheritdoc />
        public override uint Id => 0x0306B00A;


        public override void ReadWrite(CGameControlCamera n, GbxReaderWriter rw)
        {
            rw.Id(ref n.name);
        }
    }

    /// <summary>
    /// CGameControlCamera 0x00B chunk
    /// </summary>
    [Chunk(0x0306B00B)]
    public partial class Chunk0306B00B : Chunk<CGameControlCamera>
    {
        /// <inheritdoc />
        public override uint Id => 0x0306B00B;

        public bool U01;

        public override void ReadWrite(CGameControlCamera n, GbxReaderWriter rw)
        {
            rw.Vec3(ref n.relativeFollowedPos);
            rw.Boolean(ref n.isFirstPerson);
            rw.Boolean(ref n.canCameraMove);
            rw.Boolean(ref n.isFollowing);
            rw.Single(ref n.maxSpeed);
            rw.Single(ref n.planeDist);
            rw.Single(ref n.minDist);
            rw.Single(ref n.maxDist);
            rw.Single(ref n.fov);
            rw.Single(ref n.defaultFov);
            rw.Boolean(ref n.useForcedLocation);
            rw.Iso4(ref n.forcedLocation);
            rw.Boolean(ref n.useOnlyFollowedMobilPosition);
            rw.Single(ref n.minFov);
            rw.Single(ref n.maxFov);
            rw.Boolean(ref n.useForcedUp);
            rw.Vec3(ref n.forcedUp);
            rw.Single(ref n.defaultNearZ);
            rw.Single(ref n.minNearZ);
            rw.Single(ref n.maxNearZ);
            rw.Single(ref n.defaultFarZ);
            rw.Single(ref n.minFarZ);
            rw.Single(ref n.maxFarZ);
            rw.Boolean(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0306B001 => new Chunk0306B001(),
        0x0306B002 => new Chunk0306B002(),
        0x0306B003 => new Chunk0306B003(),
        0x0306B004 => new Chunk0306B004(),
        0x0306B009 => new Chunk0306B009(),
        0x0306B00A => new Chunk0306B00A(),
        0x0306B00B => new Chunk0306B00B(),
        _ => base.NewChunk(chunkId),
    };
}
