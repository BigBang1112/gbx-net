namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x2E006000</remarks>
[Node(0x2E006000)]
public class CGameObjectPhyModel : CMwNod
{
    #region Enums

    public enum EPersistence
    {
        OnPlayerUnspawn,
        OnPlayerRemoved,
        NeverRemove
    }

    public enum EProgram
    {
        None,
        Target,
        Turret
    }

    #endregion

    #region Fields

    private CMwNod? specialProperties;
    private EPersistence? persistence;
    private EProgram? program;
    private CPlugSurface? moveShapeFid;
    private bool? canStopEnemy;
    private bool? canStopEnemyBullet;
    private float? throwSpeed;
    private float? throwAngularSpeed;
    private int? armor;
    private bool? hasALifeTime;
    private int? lifeTimeDuration;
    private float? scaleCoefMax;
    private float? staminaSpawnCoef;
    private int? timeBeforeDome;
    private bool? healEnabled;
    private int? healArmorGainPerSecond;
    private bool? shieldEnabled;
    private int? shieldDomeArmor;
    private bool? bumperEnabled;
    private bool? magnetEnabled;
    private string? hitShape;
    private string? triggerShape;

    #endregion

    #region Properties

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 16)]
    public CMwNod? SpecialProperties { get => specialProperties; set => specialProperties = value; } // CPlugCharPhySpecialProperty

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 17)]
    public EPersistence? Persistence { get => persistence; set => persistence = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 5)]
    public EProgram? Program { get => program; set => program = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 21)]
    public CPlugSurface? MoveShapeFid { get => moveShapeFid; set => moveShapeFid = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 18)]
    public bool? CanStopEnemy { get => canStopEnemy; set => canStopEnemy = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 18)]
    public bool? CanStopEnemyBullet { get => canStopEnemyBullet; set => canStopEnemyBullet = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 19)]
    public float? ThrowSpeed { get => throwSpeed; set => throwSpeed = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 19)]
    public float? ThrowAngularSpeed { get => throwAngularSpeed; set => throwAngularSpeed = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 19)]
    public int? Armor { get => armor; set => armor = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 19)]
    public bool? HasALifeTime { get => hasALifeTime; set => hasALifeTime = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 19)]
    public int? LifeTimeDuration { get => lifeTimeDuration; set => lifeTimeDuration = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 19)]
    public float? ScaleCoefMax { get => scaleCoefMax; set => scaleCoefMax = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 19)]
    public float? StaminaSpawnCoef { get => staminaSpawnCoef; set => staminaSpawnCoef = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 19)]
    public int? TimeBeforeDome { get => timeBeforeDome; set => timeBeforeDome = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 19)]
    public bool? HealEnabled { get => healEnabled; set => healEnabled = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 19)]
    public int? HealArmorGainPerSecond { get => healArmorGainPerSecond; set => healArmorGainPerSecond = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 19)]
    public bool? ShieldEnabled { get => shieldEnabled; set => shieldEnabled = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 19)]
    public int? ShieldDomeArmor { get => shieldDomeArmor; set => shieldDomeArmor = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 23)]
    public bool? BumperEnabled { get => bumperEnabled; set => bumperEnabled = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 24)]
    public bool? MagnetEnabled { get => magnetEnabled; set => magnetEnabled = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 2)]
    public string? HitShape { get => hitShape; set => hitShape = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E006001), sinceVersion: 7)]
    public string? TriggerShape { get => triggerShape; set => triggerShape = value; }

    #endregion

    #region Constructors

    protected CGameObjectPhyModel()
    {

    }

    #endregion

    #region Chunks

    #region 0x001 chunk

    /// <summary>
    /// CGameObjectPhyModel 0x001 chunk
    /// </summary>
    [Chunk(0x2E006001)]
    public class Chunk2E006001 : Chunk<CGameObjectPhyModel>, IVersionable
    {
        private int version;

        public int U01 = 5;
        public int U02;
        public CMwNod? U03;
        public string? U05;
        public bool? U06;
        public CMwNod? U07;
        public int? U08;
        public Iso4? U09;
        public string? U11;
        public CMwNod? U12;
        public CMwNod? U13;
        public string? U14;
        public CMwNod? U15;
        public int? U16;
        public bool U17;
        public CMwNod? U18;
        public int? U19;
        public bool? U20;
        public bool? U21;
        public bool? U22;
        public bool? U23;
        public bool? U24;
        public bool? U25;
        public CMwNod? U26;
        public CMwNod? U27;
        public CMwNod? U28;
        public CMwNod? U29;
        public CMwNod? U30;
        public CMwNod? U31;
        public CMwNod? U32;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameObjectPhyModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version < 11)
            {
                rw.NodeRef(ref U30); // MoveShape
                rw.NodeRef(ref U31); // HitShape
                rw.NodeRef(ref U32); // TriggerShape
            }

            if (version >= 8)
            {
                rw.Int32(ref U01);
            }

            rw.Int32(ref U02); // CPlugTriggerAction array

            if (version < 14)
            {
                rw.NodeRef(ref U03);
            }

            if (version >= 2)
            {
                rw.String(ref n.hitShape);
                
                if (version >= 4)
                {
                    rw.String(ref U05);
                }

                if (version >= 3)
                {
                    rw.Boolean(ref U06);

                    if (U06.GetValueOrDefault())
                    {
                        rw.NodeRef(ref U07);
                    }

                    if (version >= 5)
                    {
                        rw.EnumInt32<EProgram>(ref n.program);
                        
                        if (version >= 6)
                        {
                            if (version >= 22)
                            {
                                rw.Boolean(ref U17, asByte: true);

                                if (U17)
                                {
                                    rw.Iso4(ref U09);
                                }
                            }
                            else
                            {
                                rw.Iso4(ref U09);
                            }

                            if (version >= 7)
                            {
                                rw.String(ref n.triggerShape);

                                if (version >= 9)
                                {
                                    if (version >= 13)
                                    {
                                        rw.String(ref U11);
                                    }

                                    rw.Int32(ref U16); // CGameActionModel array, Actions possibly

                                    if (U16 > 0)
                                    {
                                        throw new Exception("U16 > 0");
                                    }

                                    if (version >= 21)
                                    {
                                        rw.NodeRef(ref n.moveShapeFid);
                                    }

                                    if (version >= 11)
                                    {
                                        if (n.moveShapeFid is null)
                                        {
                                            rw.NodeRef(ref U27); // MoveShape
                                        }
                                        else
                                        {
                                            // some fid stuff probably, prefer to throw before tested
                                            throw new Exception("MoveShapeFid not null");
                                        }

                                        if (string.IsNullOrEmpty(n.hitShape))
                                        {
                                            rw.NodeRef(ref U28);
                                        }

                                        if (string.IsNullOrEmpty(n.triggerShape))
                                        {
                                            rw.NodeRef(ref U29);
                                        }

                                        if (version >= 12)
                                        {
                                            rw.String(ref U14);
                                            rw.NodeRef(ref U15);

                                            if (version >= 15)
                                            {
                                                rw.Int32(ref U19);

                                                if (version >= 16)
                                                {
                                                    rw.NodeRef(ref n.specialProperties);

                                                    if (version >= 17)
                                                    {
                                                        rw.EnumInt32<EPersistence>(ref n.persistence);

                                                        if (version >= 18)
                                                        {
                                                            rw.Boolean(ref n.canStopEnemy);
                                                            rw.Boolean(ref n.canStopEnemyBullet);

                                                            if (version >= 19)
                                                            {
                                                                rw.Single(ref n.throwSpeed);
                                                                rw.Single(ref n.throwAngularSpeed);
                                                                rw.Int32(ref n.armor);
                                                                rw.Boolean(ref n.hasALifeTime);
                                                                rw.Int32(ref n.lifeTimeDuration);
                                                                rw.Single(ref n.scaleCoefMax);
                                                                rw.Single(ref n.staminaSpawnCoef);
                                                                rw.Int32(ref n.timeBeforeDome);
                                                                rw.Boolean(ref U20);
                                                                rw.Boolean(ref U21);
                                                                rw.Boolean(ref U22);
                                                                rw.Boolean(ref U23);
                                                                rw.Boolean(ref U24);
                                                                rw.Boolean(ref n.healEnabled);
                                                                rw.Int32(ref n.healArmorGainPerSecond);
                                                                rw.Boolean(ref n.shieldEnabled);
                                                                rw.Int32(ref n.shieldDomeArmor);

                                                                if (version >= 20)
                                                                {
                                                                    rw.Boolean(ref U25);

                                                                    if (version >= 23)
                                                                    {
                                                                        rw.Boolean(ref n.bumperEnabled);

                                                                        if (version >= 24)
                                                                        {
                                                                            rw.Boolean(ref n.magnetEnabled);

                                                                            if (version >= 25)
                                                                            {
                                                                                rw.NodeRef(ref U26);

                                                                                if (version >= 26)
                                                                                {
                                                                                    throw new ChunkVersionNotSupportedException(version);
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
            }
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CGameObjectPhyModel 0x003 chunk
    /// </summary>
    [Chunk(0x2E006003)]
    public class Chunk2E006003 : Chunk<CGameObjectPhyModel>
    {
        public bool U01;

        public override void ReadWrite(CGameObjectPhyModel n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);

            if (U01)
            {
                throw new Exception("U01 == true");
            }
        }
    }

    #endregion

    #endregion
}
