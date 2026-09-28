namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090E5000</remarks>
[Class(0x090E5000)]
public partial class CPlugFlockModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090E5000;




    private float range;
    [AppliedWithChunk<Chunk090E5000>]
    public float Range { get => range; set => range = value; }

    private float cosViewAngle;
    [AppliedWithChunk<Chunk090E5000>]
    public float CosViewAngle { get => cosViewAngle; set => cosViewAngle = value; }

    private float minSpeed;
    [AppliedWithChunk<Chunk090E5000>]
    public float MinSpeed { get => minSpeed; set => minSpeed = value; }

    private float maxSpeed;
    [AppliedWithChunk<Chunk090E5000>]
    public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }

    private int updateFrequency;
    [AppliedWithChunk<Chunk090E5000>]
    public int UpdateFrequency { get => updateFrequency; set => updateFrequency = value; }

    private float variance;
    [AppliedWithChunk<Chunk090E5000>]
    public float Variance { get => variance; set => variance = value; }

    private float vAvoidance;
    [AppliedWithChunk<Chunk090E5000>]
    public float VAvoidance { get => vAvoidance; set => vAvoidance = value; }

    private float kAvoidance;
    [AppliedWithChunk<Chunk090E5000>]
    public float KAvoidance { get => kAvoidance; set => kAvoidance = value; }

    private float vGrouping;
    [AppliedWithChunk<Chunk090E5000>]
    public float VGrouping { get => vGrouping; set => vGrouping = value; }

    private float kGrouping;
    [AppliedWithChunk<Chunk090E5000>]
    public float KGrouping { get => kGrouping; set => kGrouping = value; }

    private float vMatching;
    [AppliedWithChunk<Chunk090E5000>]
    public float VMatching { get => vMatching; set => vMatching = value; }

    private float kMatching;
    [AppliedWithChunk<Chunk090E5000>]
    public float KMatching { get => kMatching; set => kMatching = value; }

    private float volatility;
    [AppliedWithChunk<Chunk090E5000>]
    public float Volatility { get => volatility; set => volatility = value; }

    private float vGroundAvoid;
    [AppliedWithChunk<Chunk090E5000>]
    public float VGroundAvoid { get => vGroundAvoid; set => vGroundAvoid = value; }

    private float kGroundAvoid;
    [AppliedWithChunk<Chunk090E5000>]
    public float KGroundAvoid { get => kGroundAvoid; set => kGroundAvoid = value; }

    private int standingDuration;
    [AppliedWithChunk<Chunk090E5000>]
    public int StandingDuration { get => standingDuration; set => standingDuration = value; }

    private EFlockType flockType;
    [AppliedWithChunk<Chunk090E5000>]
    public EFlockType FlockType { get => flockType; set => flockType = value; }

    private CMwNod? animFileFid;
    [AppliedWithChunk<Chunk090E5000>]
    public CMwNod? AnimFileFid { get => animFileFidFile?.GetNode(ref animFileFid) ?? animFileFid; set => animFileFid = value; }
    private Components.GbxRefTableFile? animFileFidFile;
    public Components.GbxRefTableFile? AnimFileFidFile { get => animFileFidFile; set => animFileFidFile = value; }
    public CMwNod? GetAnimFileFid(GbxReadSettings settings = default, bool exceptions = false) => animFileFidFile?.GetNode(ref animFileFid, settings, exceptions) ?? animFileFid;

    private int defSpawnCount;
    [AppliedWithChunk<Chunk090E5000>]
    public int DefSpawnCount { get => defSpawnCount; set => defSpawnCount = value; }

    private CMwNod? birdModel;
    [AppliedWithChunk<Chunk090E5000>]
    public CMwNod? BirdModel { get => birdModelFile?.GetNode(ref birdModel) ?? birdModel; set => birdModel = value; }
    private Components.GbxRefTableFile? birdModelFile;
    public Components.GbxRefTableFile? BirdModelFile { get => birdModelFile; set => birdModelFile = value; }
    public CMwNod? GetBirdModel(GbxReadSettings settings = default, bool exceptions = false) => birdModelFile?.GetNode(ref birdModel, settings, exceptions) ?? birdModel;

    private CMwNod? birdModelFid;
    [AppliedWithChunk<Chunk090E5001>]
    public CMwNod? BirdModelFid { get => birdModelFidFile?.GetNode(ref birdModelFid) ?? birdModelFid; set => birdModelFid = value; }
    private Components.GbxRefTableFile? birdModelFidFile;
    public Components.GbxRefTableFile? BirdModelFidFile { get => birdModelFidFile; set => birdModelFidFile = value; }
    public CMwNod? GetBirdModelFid(GbxReadSettings settings = default, bool exceptions = false) => birdModelFidFile?.GetNode(ref birdModelFid, settings, exceptions) ?? birdModelFid;

    private int animPeriod;
    [AppliedWithChunk<Chunk090E5001>]
    public int AnimPeriod { get => animPeriod; set => animPeriod = value; }

    private int animStandingStart;
    [AppliedWithChunk<Chunk090E5001>]
    public int AnimStandingStart { get => animStandingStart; set => animStandingStart = value; }

    private int animStandingEnd;
    [AppliedWithChunk<Chunk090E5001>]
    public int AnimStandingEnd { get => animStandingEnd; set => animStandingEnd = value; }

    private int animGlidingStart;
    [AppliedWithChunk<Chunk090E5001>]
    public int AnimGlidingStart { get => animGlidingStart; set => animGlidingStart = value; }

    private int animGlidingEnd;
    [AppliedWithChunk<Chunk090E5001>]
    public int AnimGlidingEnd { get => animGlidingEnd; set => animGlidingEnd = value; }

    private int animFlappingStart;
    [AppliedWithChunk<Chunk090E5001>]
    public int AnimFlappingStart { get => animFlappingStart; set => animFlappingStart = value; }

    private int animFlappingEnd;
    [AppliedWithChunk<Chunk090E5001>]
    public int AnimFlappingEnd { get => animFlappingEnd; set => animFlappingEnd = value; }

    private CPlugSound? soundLoop;
    [AppliedWithChunk<Chunk090E5002>]
    public CPlugSound? SoundLoop { get => soundLoopFile?.GetNode(ref soundLoop) ?? soundLoop; set => soundLoop = value; }
    private Components.GbxRefTableFile? soundLoopFile;
    public Components.GbxRefTableFile? SoundLoopFile { get => soundLoopFile; set => soundLoopFile = value; }
    public CPlugSound? GetSoundLoop(GbxReadSettings settings = default, bool exceptions = false) => soundLoopFile?.GetNode(ref soundLoop, settings, exceptions) ?? soundLoop;

    private CPlugSound? soundEventTakeOff;
    [AppliedWithChunk<Chunk090E5002>]
    public CPlugSound? SoundEventTakeOff { get => soundEventTakeOff; set => soundEventTakeOff = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugFlockModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugFlockModel() { }


    /// <summary>
    /// CPlugFlockModel 0x000 chunk
    /// </summary>
    [Chunk(0x090E5000)]
    public partial class Chunk090E5000 : Chunk<CPlugFlockModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090E5000;

        public int Version { get; set; }

        public float U01;

        public override void ReadWrite(CPlugFlockModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version <= 5)
            {
                rw.Single(ref n.range);
                rw.Single(ref n.cosViewAngle);
                rw.Single(ref n.minSpeed);
                rw.Single(ref n.maxSpeed);
                rw.Int32(ref n.updateFrequency);
                rw.Single(ref n.variance);
                rw.Single(ref n.vAvoidance);
                rw.Single(ref n.kAvoidance);
                rw.Single(ref n.vGrouping);
                rw.Single(ref n.kGrouping);
                rw.Single(ref n.vMatching);
                rw.Single(ref n.kMatching);
                rw.Single(ref n.volatility);
                rw.Single(ref n.vGroundAvoid);
                rw.Single(ref n.kGroundAvoid);
                rw.Single(ref U01);
                rw.Int32(ref n.standingDuration);
                if (Version >= 2)
                {
                    rw.EnumInt32<EFlockType>(ref n.flockType);
                    if (Version >= 3)
                    {
                        rw.NodeRef<CMwNod>(ref n.animFileFid, ref n.animFileFidFile);
                        if (Version >= 4)
                        {
                            rw.Int32(ref n.defSpawnCount);
                            if (Version >= 5)
                            {
                                rw.NodeRef<CMwNod>(ref n.birdModel, ref n.birdModelFile);
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugFlockModel 0x001 chunk
    /// </summary>
    [Chunk(0x090E5001)]
    public partial class Chunk090E5001 : Chunk<CPlugFlockModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090E5001;

        public int Version { get; set; }


        public override void ReadWrite(CPlugFlockModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CMwNod>(ref n.birdModelFid, ref n.birdModelFidFile);
            rw.Int32(ref n.animPeriod);
            if (Version >= 2)
            {
                rw.Int32(ref n.animStandingStart);
                rw.Int32(ref n.animStandingEnd);
                rw.Int32(ref n.animGlidingStart);
                rw.Int32(ref n.animGlidingEnd);
                rw.Int32(ref n.animFlappingStart);
                rw.Int32(ref n.animFlappingEnd);
            }
        }
    }

    /// <summary>
    /// CPlugFlockModel 0x002 chunk
    /// </summary>
    [Chunk(0x090E5002)]
    public partial class Chunk090E5002 : Chunk<CPlugFlockModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090E5002;

        public int Version { get; set; }


        public override void ReadWrite(CPlugFlockModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugSound>(ref n.soundLoop, ref n.soundLoopFile);
            if (Version >= 2)
            {
                rw.NodeRef<CPlugSound>(ref n.soundEventTakeOff);
            }
        }
    }



    public enum EFlockType
    {
        Bird,
        Pig,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090E5000 => new Chunk090E5000(),
        0x090E5001 => new Chunk090E5001(),
        0x090E5002 => new Chunk090E5002(),
        _ => base.NewChunk(chunkId),
    };
}
