namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0910F000</remarks>
[Class(0x0910F000)]
public partial class CPlugTurret : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0910F000;




    private string? skelRef;
    [AppliedWithChunk<Chunk0910F000>]
    public string? SkelRef { get => skelRef; set => skelRef = value; }

    private CPlugSkel? skel;
    [AppliedWithChunk<Chunk0910F000>]
    public CPlugSkel? Skel { get => skel; set => skel = value; }

    private string? bulletModelRef;
    [AppliedWithChunk<Chunk0910F000>]
    public string? BulletModelRef { get => bulletModelRef; set => bulletModelRef = value; }

    private CPlugBulletModel? bulletModel;
    [AppliedWithChunk<Chunk0910F000>]
    public CPlugBulletModel? BulletModel { get => bulletModel; set => bulletModel = value; }

    private string? meshRef;
    [AppliedWithChunk<Chunk0910F000>]
    public string? MeshRef { get => meshRef; set => meshRef = value; }

    private CPlugSolid2Model? mesh;
    [AppliedWithChunk<Chunk0910F000>]
    public CPlugSolid2Model? Mesh { get => mesh; set => mesh = value; }

    private string? visEntFxRef;
    [AppliedWithChunk<Chunk0910F000>]
    public string? VisEntFxRef { get => visEntFxRef; set => visEntFxRef = value; }

    private CPlugVisEntFxModel? visEntFx;
    [AppliedWithChunk<Chunk0910F000>]
    public CPlugVisEntFxModel? VisEntFx { get => visEntFx; set => visEntFx = value; }

    private string? shapeRef;
    [AppliedWithChunk<Chunk0910F000>]
    public string? ShapeRef { get => shapeRef; set => shapeRef = value; }

    private CPlugSurface? shape;
    [AppliedWithChunk<Chunk0910F000>]
    public CPlugSurface? Shape { get => shape; set => shape = value; }

    private string? joint0Name;
    [AppliedWithChunk<Chunk0910F000>]
    public string? Joint0Name { get => joint0Name; set => joint0Name = value; }

    private string? joint1Name;
    [AppliedWithChunk<Chunk0910F000>]
    public string? Joint1Name { get => joint1Name; set => joint1Name = value; }

    private string? jointFireName;
    [AppliedWithChunk<Chunk0910F000>]
    public string? JointFireName { get => jointFireName; set => jointFireName = value; }

    private Vec3 joint0LocalAxis;
    [AppliedWithChunk<Chunk0910F000>]
    public Vec3 Joint0LocalAxis { get => joint0LocalAxis; set => joint0LocalAxis = value; }

    private Vec3 joint1LocalAxis;
    [AppliedWithChunk<Chunk0910F000>]
    public Vec3 Joint1LocalAxis { get => joint1LocalAxis; set => joint1LocalAxis = value; }

    private Vec3 jointFireLocalAxis;
    [AppliedWithChunk<Chunk0910F000>]
    public Vec3 JointFireLocalAxis { get => jointFireLocalAxis; set => jointFireLocalAxis = value; }

    private float joint0MinAngleDeg;
    [AppliedWithChunk<Chunk0910F000>]
    public float Joint0MinAngleDeg { get => joint0MinAngleDeg; set => joint0MinAngleDeg = value; }

    private float joint0MaxAngleDeg;
    [AppliedWithChunk<Chunk0910F000>]
    public float Joint0MaxAngleDeg { get => joint0MaxAngleDeg; set => joint0MaxAngleDeg = value; }

    private float joint1MinAngleDeg;
    [AppliedWithChunk<Chunk0910F000>]
    public float Joint1MinAngleDeg { get => joint1MinAngleDeg; set => joint1MinAngleDeg = value; }

    private float joint1MaxAngleDeg;
    [AppliedWithChunk<Chunk0910F000>]
    public float Joint1MaxAngleDeg { get => joint1MaxAngleDeg; set => joint1MaxAngleDeg = value; }

    private float joint0SpeedDegPerS;
    [AppliedWithChunk<Chunk0910F000>]
    public float Joint0SpeedDegPerS { get => joint0SpeedDegPerS; set => joint0SpeedDegPerS = value; }

    private float joint1SpeedDegPerS;
    [AppliedWithChunk<Chunk0910F000>]
    public float Joint1SpeedDegPerS { get => joint1SpeedDegPerS; set => joint1SpeedDegPerS = value; }

    private float aimDetectRadius;
    [AppliedWithChunk<Chunk0910F000>]
    public float AimDetectRadius { get => aimDetectRadius; set => aimDetectRadius = value; }

    private float aimDetectFOVDeg;
    [AppliedWithChunk<Chunk0910F000>]
    public float AimDetectFOVDeg { get => aimDetectFOVDeg; set => aimDetectFOVDeg = value; }

    private float aimMaxTrackDist;
    [AppliedWithChunk<Chunk0910F000>]
    public float AimMaxTrackDist { get => aimMaxTrackDist; set => aimMaxTrackDist = value; }

    private float aimAnticipation;
    [AppliedWithChunk<Chunk0910F000>]
    public float AimAnticipation { get => aimAnticipation; set => aimAnticipation = value; }

    private int aimKeepAimingDurationMs;
    [AppliedWithChunk<Chunk0910F000>]
    public int AimKeepAimingDurationMs { get => aimKeepAimingDurationMs; set => aimKeepAimingDurationMs = value; }

    private int aimFireTargetChangeDelayMs;
    [AppliedWithChunk<Chunk0910F000>]
    public int AimFireTargetChangeDelayMs { get => aimFireTargetChangeDelayMs; set => aimFireTargetChangeDelayMs = value; }

    private float aimFireMaxAngleDeg;
    [AppliedWithChunk<Chunk0910F000>]
    public float AimFireMaxAngleDeg { get => aimFireMaxAngleDeg; set => aimFireMaxAngleDeg = value; }

    private ETurretFixedAngleSignal fixedAngleSignal;
    [AppliedWithChunk<Chunk0910F000>]
    public ETurretFixedAngleSignal FixedAngleSignal { get => fixedAngleSignal; set => fixedAngleSignal = value; }

    private int fixedAnglePeriodMs;
    [AppliedWithChunk<Chunk0910F000>]
    public int FixedAnglePeriodMs { get => fixedAnglePeriodMs; set => fixedAnglePeriodMs = value; }

    private float fixedAngleMinDeg;
    [AppliedWithChunk<Chunk0910F000>]
    public float FixedAngleMinDeg { get => fixedAngleMinDeg; set => fixedAngleMinDeg = value; }

    private float fixedAngleMaxDeg;
    [AppliedWithChunk<Chunk0910F000>]
    public float FixedAngleMaxDeg { get => fixedAngleMaxDeg; set => fixedAngleMaxDeg = value; }

    private int firePeriodMs;
    [AppliedWithChunk<Chunk0910F000>]
    public int FirePeriodMs { get => firePeriodMs; set => firePeriodMs = value; }

    private string? rotateSound1Ref;
    [AppliedWithChunk<Chunk0910F000>]
    public string? RotateSound1Ref { get => rotateSound1Ref; set => rotateSound1Ref = value; }

    private CMwNod? rotateSound1;
    [AppliedWithChunk<Chunk0910F000>]
    public CMwNod? RotateSound1 { get => rotateSound1; set => rotateSound1 = value; }

    private float joint0NextJointUpdateAngleMaxDeg;
    [AppliedWithChunk<Chunk0910F000>]
    public float Joint0NextJointUpdateAngleMaxDeg { get => joint0NextJointUpdateAngleMaxDeg; set => joint0NextJointUpdateAngleMaxDeg = value; }

    private float joint1NextJointUpdateAngleMaxDeg;
    [AppliedWithChunk<Chunk0910F000>]
    public float Joint1NextJointUpdateAngleMaxDeg { get => joint1NextJointUpdateAngleMaxDeg; set => joint1NextJointUpdateAngleMaxDeg = value; }

    private float aimFireMaxDist;
    [AppliedWithChunk<Chunk0910F000>]
    public float AimFireMaxDist { get => aimFireMaxDist; set => aimFireMaxDist = value; }

    private bool aimEnabled;
    [AppliedWithChunk<Chunk0910F000>]
    public bool AimEnabled { get => aimEnabled; set => aimEnabled = value; }

    private string? jointRadarName;
    [AppliedWithChunk<Chunk0910F000>]
    public string? JointRadarName { get => jointRadarName; set => jointRadarName = value; }

    private int lifeArmorMax;
    [AppliedWithChunk<Chunk0910F000>]
    public int LifeArmorMax { get => lifeArmorMax; set => lifeArmorMax = value; }

    private EOnArmorEmtpy lifeOnArmorEmtpy;
    [AppliedWithChunk<Chunk0910F000>]
    public EOnArmorEmtpy LifeOnArmorEmtpy { get => lifeOnArmorEmtpy; set => lifeOnArmorEmtpy = value; }

    private int lifeDisabledDuration;
    [AppliedWithChunk<Chunk0910F000>]
    public int LifeDisabledDuration { get => lifeDisabledDuration; set => lifeDisabledDuration = value; }

    private bool isControllable;
    [AppliedWithChunk<Chunk0910F000>]
    public bool IsControllable { get => isControllable; set => isControllable = value; }

    private CPlugParticleEmitterModel? onFireParticle;
    [AppliedWithChunk<Chunk0910F000>]
    public CPlugParticleEmitterModel? OnFireParticle { get => onFireParticle; set => onFireParticle = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugTurret"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugTurret() { }


    /// <summary>
    /// CPlugTurret 0x000 chunk
    /// </summary>
    [Chunk(0x0910F000)]
    public partial class Chunk0910F000 : Chunk<CPlugTurret>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0910F000;

        public int Version { get; set; }

        public int U01;
        public int U02;
        public TransQuat[]? U03;
        public short[]? U04;
        public short[]? U05;
        public string[]? U06;
        public int[]? U07;
        public int U36;

        public override void ReadWrite(CPlugTurret n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version <= 4)
            {
                if (Version >= 3)
                {
                    rw.Int32(ref U01);
                }
                if (Version >= 2)
                {
                    rw.Int32(ref U02);
                }
                rw.Array<TransQuat>(ref U03!);
                rw.Array<short>(ref U04!);
                if (Version >= 1)
                {
                    rw.Array<short>(ref U05!);
                }
                rw.ArrayId(ref U06!);
                if (Version >= 4)
                {
                    rw.Array<int>(ref U07!);
                }
            }
            if (Version >= 5)
            {
                rw.String(ref n.skelRef);
                if (n.SkelRef==null||n.SkelRef=="")
                {
                    rw.NodeRef<CPlugSkel>(ref n.skel);
                }
                rw.String(ref n.bulletModelRef);
                if (n.BulletModelRef==null||n.BulletModelRef=="")
                {
                    rw.NodeRef<CPlugBulletModel>(ref n.bulletModel);
                }
                if (Version >= 6)
                {
                    rw.String(ref n.meshRef);
                    if (n.MeshRef==null||n.MeshRef=="")
                    {
                        rw.NodeRef<CPlugSolid2Model>(ref n.mesh);
                    }
                    if (Version >= 7)
                    {
                        rw.String(ref n.visEntFxRef);
                        if (n.VisEntFxRef==null||n.VisEntFxRef=="")
                        {
                            rw.NodeRef<CPlugVisEntFxModel>(ref n.visEntFx);
                        }
                        if (Version >= 8)
                        {
                            rw.String(ref n.shapeRef);
                            if (n.ShapeRef==null||n.ShapeRef=="")
                            {
                                rw.NodeRef<CPlugSurface>(ref n.shape);
                            }
                            if (Version >= 9)
                            {
                                rw.Id(ref n.joint0Name);
                                rw.Id(ref n.joint1Name);
                                if (Version >= 10)
                                {
                                    rw.Id(ref n.jointFireName);
                                    rw.Vec3(ref n.joint0LocalAxis);
                                    rw.Vec3(ref n.joint1LocalAxis);
                                    rw.Vec3(ref n.jointFireLocalAxis);
                                    rw.Single(ref n.joint0MinAngleDeg);
                                    rw.Single(ref n.joint0MaxAngleDeg);
                                    rw.Single(ref n.joint1MinAngleDeg);
                                    rw.Single(ref n.joint1MaxAngleDeg);
                                    if (Version >= 11)
                                    {
                                        rw.Single(ref n.joint0SpeedDegPerS);
                                        rw.Single(ref n.joint1SpeedDegPerS);
                                        if (Version >= 12)
                                        {
                                            rw.Int32(ref U36);
                                            rw.Single(ref n.aimDetectRadius);
                                            rw.Single(ref n.aimDetectFOVDeg);
                                            rw.Single(ref n.aimMaxTrackDist);
                                            rw.Single(ref n.aimAnticipation);
                                            rw.Int32(ref n.aimKeepAimingDurationMs);
                                            rw.Int32(ref n.aimFireTargetChangeDelayMs);
                                            rw.Single(ref n.aimFireMaxAngleDeg);
                                            rw.EnumInt32<ETurretFixedAngleSignal>(ref n.fixedAngleSignal);
                                            rw.Int32(ref n.fixedAnglePeriodMs);
                                            rw.Single(ref n.fixedAngleMinDeg);
                                            rw.Single(ref n.fixedAngleMaxDeg);
                                            rw.Int32(ref n.firePeriodMs);
                                            if (Version >= 13)
                                            {
                                                rw.String(ref n.rotateSound1Ref);
                                                if (n.RotateSound1Ref==null||n.RotateSound1Ref=="")
                                                {
                                                    rw.NodeRef<CMwNod>(ref n.rotateSound1);
                                                }
                                                if (Version >= 14)
                                                {
                                                    rw.Single(ref n.joint0NextJointUpdateAngleMaxDeg);
                                                    rw.Single(ref n.joint1NextJointUpdateAngleMaxDeg);
                                                    rw.Single(ref n.aimFireMaxDist);
                                                    if (Version >= 15)
                                                    {
                                                        rw.Boolean(ref n.aimEnabled);
                                                        if (Version >= 16)
                                                        {
                                                            rw.Id(ref n.jointRadarName);
                                                            if (Version >= 17)
                                                            {
                                                                rw.Int32(ref n.lifeArmorMax);
                                                                rw.EnumInt32<EOnArmorEmtpy>(ref n.lifeOnArmorEmtpy);
                                                                rw.Int32(ref n.lifeDisabledDuration);
                                                                if (Version >= 18)
                                                                {
                                                                    rw.Boolean(ref n.isControllable);
                                                                    if (Version >= 19)
                                                                    {
                                                                        rw.NodeRef<CPlugParticleEmitterModel>(ref n.onFireParticle);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }



    public enum EOnArmorEmtpy
    {
        Destroy,
        Disable,
    }

    public enum ETurretFixedAngleSignal
    {
        Constant,
        Linear,
        PingPong,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0910F000 => new Chunk0910F000(),
        _ => base.NewChunk(chunkId),
    };
}
