using GBX.NET.Components;

namespace GBX.NET.Engines.GameData;

public partial class CGameObjectPhyModel
{
    private CPlugSurface? moveShapeFid;
    public CPlugSurface? MoveShapeFid
    {
        get => moveShapeFidFile?.GetNode(ref moveShapeFid) ?? moveShapeFid;
        set => moveShapeFid = value;
    }
    private GbxRefTableFile? moveShapeFidFile;
    public GbxRefTableFile? MoveShapeFidFile { get => moveShapeFidFile; set => moveShapeFidFile = value; }
    public CPlugSurface? GetMoveShapeFid(GbxReadSettings settings = default) => moveShapeFidFile?.GetNode(ref moveShapeFid, settings);

    private CPlugSurface? hitShapeFid;
    public CPlugSurface? HitShapeFid
    {
        get => hitShapeFidFile?.GetNode(ref hitShapeFid) ?? hitShapeFid;
        set => hitShapeFid = value;
    }
    private GbxRefTableFile? hitShapeFidFile;
    public GbxRefTableFile? HitShapeFidFile { get => hitShapeFidFile; set => hitShapeFidFile = value; }
    public CPlugSurface? GetHitShapeFid(GbxReadSettings settings = default) => hitShapeFidFile?.GetNode(ref hitShapeFid, settings);

    private CPlugSurface? triggerShapeFid;
    public CPlugSurface? TriggerShapeFid
    {
        get => triggerShapeFidFile?.GetNode(ref triggerShapeFid) ?? triggerShapeFid;
        set => triggerShapeFid = value;
    }
    private GbxRefTableFile? triggerShapeFidFile;
    public GbxRefTableFile? TriggerShapeFidFile { get => triggerShapeFidFile; set => triggerShapeFidFile = value; }
    public CPlugSurface? GetTriggerShapeFid(GbxReadSettings settings = default) => triggerShapeFidFile?.GetNode(ref triggerShapeFid, settings);

    private int triggerActionVersion = 5;
    public int TriggerActionVersion { get => triggerActionVersion; set => triggerActionVersion = value; }

    private CPlugTriggerAction[]? triggers;
    public CPlugTriggerAction[]? Triggers { get => triggers; set => triggers = value; }

    private string? moveShape;
    public string? MoveShape { get => moveShape; set => moveShape = value; }

    private string? hitShape;
    public string? HitShape { get => hitShape; set => hitShape = value; }

    private CPlugDynaPointModel? dynaPointModel;
    public CPlugDynaPointModel? DynaPointModel { get => dynaPointModel; set => dynaPointModel = value; }

    private EProgram? program;
    public EProgram? Program { get => program; set => program = value; }

    private string? triggerShape;
    public string? TriggerShape { get => triggerShape; set => triggerShape = value; }

    private string? actionModel;
    public string? ActionModel { get => actionModel; set => actionModel = value; }

    private CGameActionModel?[]? actions;
    public CGameActionModel?[]? Actions { get => actions; set => actions = value; }

    private CMwNod? specialProperties;
    public CMwNod? SpecialProperties
    {
        get => specialPropertiesFile?.GetNode(ref specialProperties) ?? specialProperties;
        set => specialProperties = value;
    }
    private GbxRefTableFile? specialPropertiesFile;
    public GbxRefTableFile? SpecialPropertiesFile { get => specialPropertiesFile; set => specialPropertiesFile = value; }
    public CMwNod? GetSpecialProperties(GbxReadSettings settings = default) => specialPropertiesFile?.GetNode(ref specialProperties, settings);

    private EPersistence? persistence = EPersistence.NeverRemove;
    public EPersistence? Persistence { get => persistence; set => persistence = value; }

    private bool canStopEnemy;
    public bool CanStopEnemy { get => canStopEnemy; set => canStopEnemy = value; }

    private bool canStopEnemyBullet;
    public bool CanStopEnemyBullet { get => canStopEnemyBullet; set => canStopEnemyBullet = value; }

    private float throwSpeed = 30;
    public float ThrowSpeed { get => throwSpeed; set => throwSpeed = value; }

    private float throwAngularSpeed = 10;
    public float ThrowAngularSpeed { get => throwAngularSpeed; set => throwAngularSpeed = value; }

    private int armor = 100;
    public int Armor { get => armor; set => armor = value; }

    private bool hasALifeTime;
    public bool HasALifeTime { get => hasALifeTime; set => hasALifeTime = value; }

    private int lifeTimeDuration = 7000;
    public int LifeTimeDuration { get => lifeTimeDuration; set => lifeTimeDuration = value; }

    private float scaleCoefMax = 1;
    public float ScaleCoefMax { get => scaleCoefMax; set => scaleCoefMax = value; }

    private float staminaSpawnCoef = 1;
    public float StaminaSpawnCoef { get => staminaSpawnCoef; set => staminaSpawnCoef = value; }

    private int timeBeforeDome = 500;
    public int TimeBeforeDome { get => timeBeforeDome; set => timeBeforeDome = value; }

    private bool healEnabled;
    public bool HealEnabled { get => healEnabled; set => healEnabled = value; }

    private int healArmorGainPerSecond = 10;
    public int HealArmorGainPerSecond { get => healArmorGainPerSecond; set => healArmorGainPerSecond = value; }

    private bool shieldEnabled;
    public bool ShieldEnabled { get => shieldEnabled; set => shieldEnabled = value; }

    private int shieldDomeArmor = 200;
    public int ShieldDomeArmor { get => shieldDomeArmor; set => shieldDomeArmor = value; }

    private bool bumperEnabled;
    public bool BumperEnabled { get => bumperEnabled; set => bumperEnabled = value; }

    private bool magnetEnabled;
    public bool MagnetEnabled { get => magnetEnabled; set => magnetEnabled = value; }

    public partial class Chunk2E006001 : IVersionable
    {
        public int Version { get; set; }

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
    }
}
