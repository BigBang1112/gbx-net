namespace GBX.NET.Engines.Plug;

[Node(0x090F7000)]
public class CPlugVehicleCameraInternalModel : CPlugCamControlModel
{
    private string? name;
    private Vec3 relativePos;
    private float? fov;
    private bool isFirstPerson;
    private Vec3 pitchYawRoll;
    private bool camBlendEnabled;
    private float pilotHeadCoef;
    private float bulletTimeFovSmoothDelta_m_Delta;
    private int bulletTimeFovSmoothDelta_m_TimeDown;
    private int bulletTimeFovSmoothDelta_m_TimeUp;

    internal CPlugVehicleCameraInternalModel()
    {
        
    }

    [NodeMember]
    [AppliedWithChunk<Chunk090F7000>]
    public string? Name { get => name; set => name = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090F7000>(sinceVersion: 2)]
    public Vec3 RelativePos { get => relativePos; set => relativePos = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090F7000>(sinceVersion: 2)]
    public float? Fov { get => fov; set => fov = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090F7000>(sinceVersion: 4)]
    public bool IsFirstPerson { get => isFirstPerson; set => isFirstPerson = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090F7000>(sinceVersion: 4)]
    public Vec3 PitchYawRoll { get => pitchYawRoll; set => pitchYawRoll = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090F7000>(sinceVersion: 5)]
    public bool CamBlendEnabled { get => camBlendEnabled; set => camBlendEnabled = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090F7000>(sinceVersion: 6)]
    public float PilotHeadCoef { get => pilotHeadCoef; set => pilotHeadCoef = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090F7000>(sinceVersion: 7)]
    public float BulletTimeFovSmoothDelta_m_Delta { get => bulletTimeFovSmoothDelta_m_Delta; set => bulletTimeFovSmoothDelta_m_Delta = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090F7000>(sinceVersion: 7)]
    public int BulletTimeFovSmoothDelta_m_TimeDown { get => bulletTimeFovSmoothDelta_m_TimeDown; set => bulletTimeFovSmoothDelta_m_TimeDown = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090F7000>(sinceVersion: 7)]
    public int BulletTimeFovSmoothDelta_m_TimeUp { get => bulletTimeFovSmoothDelta_m_TimeUp; set => bulletTimeFovSmoothDelta_m_TimeUp = value; }

    #region 0x000 chunk

    /// <summary>
    /// CPlugVehicleCameraInternalModel 0x000 chunk
    /// </summary>
    [Chunk(0x090F7000)]
    public class Chunk090F7000 : Chunk<CPlugVehicleCameraInternalModel>, IVersionable
    {
        public Vec3 U01;

        public int Version { get; set; } = 7;

        public override void ReadWrite(CPlugVehicleCameraInternalModel n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            rw.Id(ref n.name);
            
            if (Version >= 2)
            {
                rw.Vec3(ref n.relativePos);
                rw.Single(ref n.fov);
            }
            
            if (Version < 3)
            {
                U01 = n.relativePos;
            }
            else
            {
                rw.Vec3(ref U01);
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
                        }
                    }
                }
            }
        }
    }

    #endregion
}
