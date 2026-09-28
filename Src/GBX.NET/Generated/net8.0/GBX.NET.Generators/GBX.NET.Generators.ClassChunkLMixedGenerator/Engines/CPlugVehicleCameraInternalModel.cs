namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090F7000</remarks>
[Class(0x090F7000)]
public partial class CPlugVehicleCameraInternalModel : CPlugCamControlModel, IClass
{
    [Hexadecimal] public static new uint Id => 0x090F7000;




    private string? name;
    [AppliedWithChunk<Chunk090F7000>]
    public string? Name { get => name; set => name = value; }

    private Vec3 relativePos;
    [AppliedWithChunk<Chunk090F7000>]
    public Vec3 RelativePos { get => relativePos; set => relativePos = value; }

    private float? fov;
    [AppliedWithChunk<Chunk090F7000>]
    public float? Fov { get => fov; set => fov = value; }

    private bool isFirstPerson;
    [AppliedWithChunk<Chunk090F7000>]
    public bool IsFirstPerson { get => isFirstPerson; set => isFirstPerson = value; }

    private Vec3 pitchYawRoll;
    [AppliedWithChunk<Chunk090F7000>]
    public Vec3 PitchYawRoll { get => pitchYawRoll; set => pitchYawRoll = value; }

    private bool camBlendEnabled;
    [AppliedWithChunk<Chunk090F7000>]
    public bool CamBlendEnabled { get => camBlendEnabled; set => camBlendEnabled = value; }

    private float pilotHeadCoef;
    [AppliedWithChunk<Chunk090F7000>]
    public float PilotHeadCoef { get => pilotHeadCoef; set => pilotHeadCoef = value; }

    private float bulletTimeFovSmoothDelta_m_Delta;
    [AppliedWithChunk<Chunk090F7000>]
    public float BulletTimeFovSmoothDelta_m_Delta { get => bulletTimeFovSmoothDelta_m_Delta; set => bulletTimeFovSmoothDelta_m_Delta = value; }

    private int bulletTimeFovSmoothDelta_m_TimeDown;
    [AppliedWithChunk<Chunk090F7000>]
    public int BulletTimeFovSmoothDelta_m_TimeDown { get => bulletTimeFovSmoothDelta_m_TimeDown; set => bulletTimeFovSmoothDelta_m_TimeDown = value; }

    private int bulletTimeFovSmoothDelta_m_TimeUp;
    [AppliedWithChunk<Chunk090F7000>]
    public int BulletTimeFovSmoothDelta_m_TimeUp { get => bulletTimeFovSmoothDelta_m_TimeUp; set => bulletTimeFovSmoothDelta_m_TimeUp = value; }

    private float superBulletTimeFovSmoothMultiplier;
    [AppliedWithChunk<Chunk090F7000>]
    public float SuperBulletTimeFovSmoothMultiplier { get => superBulletTimeFovSmoothMultiplier; set => superBulletTimeFovSmoothMultiplier = value; }

    private int superBulletTimeFovSmoothMultiplier_m_TimeUp;
    [AppliedWithChunk<Chunk090F7000>]
    public int SuperBulletTimeFovSmoothMultiplier_m_TimeUp { get => superBulletTimeFovSmoothMultiplier_m_TimeUp; set => superBulletTimeFovSmoothMultiplier_m_TimeUp = value; }

    private int superBulletTimeFovSmoothMultiplier_m_TimeDown;
    [AppliedWithChunk<Chunk090F7000>]
    public int SuperBulletTimeFovSmoothMultiplier_m_TimeDown { get => superBulletTimeFovSmoothMultiplier_m_TimeDown; set => superBulletTimeFovSmoothMultiplier_m_TimeDown = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugVehicleCameraInternalModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugVehicleCameraInternalModel() { }


    /// <summary>
    /// CPlugVehicleCameraInternalModel 0x000 chunk
    /// </summary>
    [Chunk(0x090F7000)]
    public partial class Chunk090F7000 : Chunk<CPlugVehicleCameraInternalModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090F7000;

        public int Version { get; set; }

        /// <summary>
        /// U01 = RelativePos in v2-
        /// </summary>
        public Vec3 U01;

        public override void ReadWrite(CPlugVehicleCameraInternalModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref n.name);
            if (Version >= 2)
            {
                rw.Vec3(ref n.relativePos);
                rw.Single(ref n.fov);
                if (Version >= 3)
                {
                    rw.Vec3(ref U01); // U01 = RelativePos in v2-
                }
            }
            if (Version >= 4)
            {
                rw.Boolean(ref n.isFirstPerson);
                rw.Vec3(ref n.pitchYawRoll);
                if (Version >= 5)
                {
                    rw.Boolean(ref n.camBlendEnabled);
                    if (Version >= 6)
                    {
                        rw.Single(ref n.pilotHeadCoef);
                        if (Version >= 7)
                        {
                            rw.Single(ref n.bulletTimeFovSmoothDelta_m_Delta);
                            rw.Int32(ref n.bulletTimeFovSmoothDelta_m_TimeDown);
                            rw.Int32(ref n.bulletTimeFovSmoothDelta_m_TimeUp);
                            if (Version >= 8)
                            {
                                rw.Single(ref n.superBulletTimeFovSmoothMultiplier);
                                rw.Int32(ref n.superBulletTimeFovSmoothMultiplier_m_TimeUp);
                                rw.Int32(ref n.superBulletTimeFovSmoothMultiplier_m_TimeDown);
                            }
                        }
                    }
                }
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090F7000 => new Chunk090F7000(),
        _ => base.NewChunk(chunkId),
    };
}
