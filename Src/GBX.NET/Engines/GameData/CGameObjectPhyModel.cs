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

    private CPlugSurface? moveShapeFid;
    private GameBoxRefTable.File? moveShapeFidFile;
    private CPlugSurface? hitShapeFid;
    private GameBoxRefTable.File? hitShapeFidFile;
    private CPlugSurface? triggerShapeFid;
    private GameBoxRefTable.File? triggerShapeFidFile;
    private int triggerActionVersion = 5;
    private CPlugTriggerAction[]? triggers;
    private string? moveShape;
    private string? hitShape;
    private CPlugDynaPointModel? dynaPointModel;
    private EProgram? program;
    private string? triggerShape;
    private string? actionModel;
    private CGameActionModel?[]? actions;
    private CMwNod? specialProperties;
    private GameBoxRefTable.File? specialPropertiesFile;
    private EPersistence? persistence = EPersistence.NeverRemove;
    private bool canStopEnemy;
    private bool canStopEnemyBullet;
    private float throwSpeed = 30;
    private float throwAngularSpeed = 10;
    private int armor = 100;
    private bool hasALifeTime;
    private int lifeTimeDuration = 7000;
    private float scaleCoefMax = 1;
    private float staminaSpawnCoef = 1;
    private int timeBeforeDome = 500;
    private bool healEnabled;
    private int healArmorGainPerSecond = 10;
    private bool shieldEnabled;
    private int shieldDomeArmor = 200;
    private bool bumperEnabled;
    private bool magnetEnabled;

    #endregion

    #region Properties

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>]
    public CPlugSurface? MoveShapeFid
    {
        get => moveShapeFid = GetNodeFromRefTable(moveShapeFid, moveShapeFidFile) as CPlugSurface;
        set => moveShapeFid = value;
    }

    [NodeMember]
    [AppliedWithChunk<Chunk2E006001>]
    public CPlugSurface? HitShapeFid
    {
        get => hitShapeFid = GetNodeFromRefTable(hitShapeFid, hitShapeFidFile) as CPlugSurface;
        set => hitShapeFid = value;
    }

    [NodeMember]
    [AppliedWithChunk<Chunk2E006001>]
    public CPlugSurface? TriggerShapeFid
    {
        get => triggerShapeFid = GetNodeFromRefTable(triggerShapeFid, triggerShapeFidFile) as CPlugSurface;
        set => triggerShapeFid = value;
    }

    [NodeMember]
    [AppliedWithChunk<Chunk2E006001>]
    public int TriggerActionVersion { get => triggerActionVersion; set => triggerActionVersion = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>]
    public CPlugTriggerAction[]? Triggers { get => triggers; set => triggers = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 2)]
    public string? MoveShape { get => moveShape; set => moveShape = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 4)]
    public string? HitShape { get => hitShape; set => hitShape = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 3)]
    public CPlugDynaPointModel? DynaPointModel { get => dynaPointModel; set => dynaPointModel = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 5)]
    public EProgram? Program { get => program; set => program = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 7)]
    public string? TriggerShape { get => triggerShape; set => triggerShape = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 13)]
    public string? ActionModel { get => actionModel; set => actionModel = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 9)]
    public CGameActionModel?[]? Actions { get => actions; set => actions = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 16)]
    public CMwNod? SpecialProperties // CPlugCharPhySpecialProperty?
    {
        get => specialProperties = GetNodeFromRefTable(specialProperties, specialPropertiesFile) as CMwNod;
        set => specialProperties = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 17)]
    public EPersistence? Persistence { get => persistence; set => persistence = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 18)]
    public bool CanStopEnemy { get => canStopEnemy; set => canStopEnemy = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 18)]
    public bool CanStopEnemyBullet { get => canStopEnemyBullet; set => canStopEnemyBullet = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 19)]
    public float ThrowSpeed { get => throwSpeed; set => throwSpeed = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 19)]
    public float ThrowAngularSpeed { get => throwAngularSpeed; set => throwAngularSpeed = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 19)]
    public int Armor { get => armor; set => armor = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 19)]
    public bool HasALifeTime { get => hasALifeTime; set => hasALifeTime = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 19)]
    public int LifeTimeDuration { get => lifeTimeDuration; set => lifeTimeDuration = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 19)]
    public float ScaleCoefMax { get => scaleCoefMax; set => scaleCoefMax = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 19)]
    public float StaminaSpawnCoef { get => staminaSpawnCoef; set => staminaSpawnCoef = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 19)]
    public int TimeBeforeDome { get => timeBeforeDome; set => timeBeforeDome = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 19)]
    public bool HealEnabled { get => healEnabled; set => healEnabled = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 19)]
    public int HealArmorGainPerSecond { get => healArmorGainPerSecond; set => healArmorGainPerSecond = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 19)]
    public bool ShieldEnabled { get => shieldEnabled; set => shieldEnabled = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 19)]
    public int ShieldDomeArmor { get => shieldDomeArmor; set => shieldDomeArmor = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 23)]
    public bool BumperEnabled { get => bumperEnabled; set => bumperEnabled = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E006001>(sinceVersion: 24)]
    public bool MagnetEnabled { get => magnetEnabled; set => magnetEnabled = value; }

    #endregion

    #region Constructors

    internal CGameObjectPhyModel()
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

        public CMwNod? U01;
        public CMwNod? U03;
        public Iso4? U09;
        public string? U14;
        public CMwNod? U15;
        public bool U17;
        public int? U19;
        public bool? U20;
        public bool? U21;
        public bool? U22;
        public bool? U23;
        public bool? U24;
        public bool U25 = true;
        public CMwNod? U26;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameObjectPhyModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version < 11)
            {
                rw.NodeRef<CPlugSurface>(ref n.moveShapeFid, ref n.moveShapeFidFile); // EGameObjectPhyModelShapeType == 0
                rw.NodeRef<CPlugSurface>(ref n.hitShapeFid, ref n.hitShapeFidFile); // EGameObjectPhyModelShapeType == 1
                rw.NodeRef<CPlugSurface>(ref n.triggerShapeFid, ref n.triggerShapeFidFile); // EGameObjectPhyModelShapeType == 2
            }

            if (version >= 8)
            {
                rw.Int32(ref n.triggerActionVersion); // EPlugTriggerActionVersion
            }
            else
            {
                n.triggerActionVersion = 5;
            }

            rw.ArrayArchive<CPlugTriggerAction>(ref n.triggers, n.triggerActionVersion); // CPlugTriggerAction::ArchiveTrigger

            if (version < 14)
            {
                rw.NodeRef(ref U03); // CPlugTriggerAction
            }

            if (version >= 2)
            {
                rw.String(ref n.moveShape);
                
                if (version >= 4)
                {
                    rw.String(ref n.hitShape);
                }

                if (version >= 3)
                {
                    if (rw.Boolean(n.dynaPointModel is not null))
                    {
                        rw.Archive<CPlugDynaPointModel>(ref n.dynaPointModel);
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
                                        rw.String(ref n.actionModel);
                                    }

                                    if (string.IsNullOrEmpty(n.actionModel))
                                    {
                                        rw.ArrayNode(ref n.actions); // sometimes maybe not available at all?
                                    }

                                    if (version < 11)
                                    {
                                        //HackRefCompat(&this->MoveShape);
                                        //HackRefCompat(&this->HitShape);
                                        //HackRefCompat(&this->TriggerShape);
                                    }

                                    if (version >= 21)
                                    {
                                        rw.NodeRef(ref U01); // ???
                                    }

                                    if (version >= 11)
                                    {
                                        if (string.IsNullOrEmpty(n.moveShape))
                                        {
                                            rw.NodeRef<CPlugSurface>(ref n.moveShapeFid, ref n.moveShapeFidFile);
                                        }
                                        else
                                        {
                                            // Something is there but without it it works? xd
                                        }

                                        if (string.IsNullOrEmpty(n.hitShape))
                                        {
                                            rw.NodeRef<CPlugSurface>(ref n.hitShapeFid, ref n.hitShapeFidFile);
                                        }

                                        if (string.IsNullOrEmpty(n.triggerShape))
                                        {
                                            rw.NodeRef<CPlugSurface>(ref n.triggerShapeFid, ref n.triggerShapeFidFile);
                                        }

                                        if (version >= 12)
                                        {
                                            rw.String(ref U14);

                                            if (string.IsNullOrEmpty(U14))
                                            {
                                                rw.NodeRef(ref U15); // Solid?
                                            }

                                            if (version >= 15)
                                            {
                                                rw.Int32(ref U19);

                                                if (version >= 16)
                                                {
                                                    rw.NodeRef(ref n.specialProperties, ref n.specialPropertiesFile); // CPlugFileImg? whatever, ill leave it SpecialProperties

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
