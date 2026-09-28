namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x2E008000</remarks>
[Class(0x2E008000)]
public partial class CGameActionModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x2E008000;




    private bool useVehicleGuns;
    [AppliedWithChunk<Chunk2E008000>]
    public bool UseVehicleGuns { get => useVehicleGuns; set => useVehicleGuns = value; }

    private CPlugBulletModel[]? bulletModels_Nadeo;
    [AppliedWithChunk<Chunk2E008000>]
    public CPlugBulletModel[]? BulletModels_Nadeo { get => bulletModels_Nadeo; set => bulletModels_Nadeo = value; }

    private string? actionName;
    [AppliedWithChunk<Chunk2E008000>]
    [AppliedWithChunk<Chunk2E008000>]
    [AppliedWithChunk<Chunk2E008000>]
    public string? ActionName { get => actionName; set => actionName = value; }

    private int inventoryItemClass;
    [AppliedWithChunk<Chunk2E008000>]
    public int InventoryItemClass { get => inventoryItemClass; set => inventoryItemClass = value; }

    private int spriteBlockVersion;
    [AppliedWithChunk<Chunk2E008000>]
    public int SpriteBlockVersion { get => spriteBlockVersion; set => spriteBlockVersion = value; }

    private SpriteBlock[]? spriteBlocks;
    [AppliedWithChunk<Chunk2E008000>]
    public SpriteBlock[]? SpriteBlocks { get => spriteBlocks; set => spriteBlocks = value; }

    private string? description;
    [AppliedWithChunk<Chunk2E008000>]
    public string? Description { get => description; set => description = value; }

    private int beamVersion = 1;
    [AppliedWithChunk<Chunk2E008000>]
    public int BeamVersion { get => beamVersion; set => beamVersion = value; }

    private CPlugCustomBeamModel[]? beams;
    [AppliedWithChunk<Chunk2E008000>]
    public CPlugCustomBeamModel[]? Beams { get => beams; set => beams = value; }

    private ScriptParams? cooldown;
    [AppliedWithChunk<Chunk2E008000>]
    [AppliedWithChunk<Chunk2E008000>]
    public ScriptParams? Cooldown { get => cooldown; set => cooldown = value; }

    private int scriptParamsVersion = 2;
    [AppliedWithChunk<Chunk2E008000>]
    public int ScriptParamsVersion { get => scriptParamsVersion; set => scriptParamsVersion = value; }

    private int actionScriptEffectVersion;
    [AppliedWithChunk<Chunk2E008000>]
    public int ActionScriptEffectVersion { get => actionScriptEffectVersion; set => actionScriptEffectVersion = value; }

    private ActionScriptEffect[]? actionScriptEffects;
    [AppliedWithChunk<Chunk2E008000>]
    public ActionScriptEffect[]? ActionScriptEffects { get => actionScriptEffects; set => actionScriptEffects = value; }

    private int customBulletVersion;
    [AppliedWithChunk<Chunk2E008000>]
    public int CustomBulletVersion { get => customBulletVersion; set => customBulletVersion = value; }

    private CPlugCustomBulletModel[]? projectiles;
    [AppliedWithChunk<Chunk2E008000>]
    public CPlugCustomBulletModel[]? Projectiles { get => projectiles; set => projectiles = value; }

    private CPlugAnimFile? anim;
    [AppliedWithChunk<Chunk2E008000>]
    public CPlugAnimFile? Anim { get => anim; set => anim = value; }

    private CPlugScriptWithSettings? script;
    [AppliedWithChunk<Chunk2E008000>]
    [AppliedWithChunk<Chunk2E008000>]
    public CPlugScriptWithSettings? Script { get => script; set => script = value; }

    private string? icon;
    [AppliedWithChunk<Chunk2E008000>]
    public string? Icon { get => icon; set => icon = value; }

    private string? crosshair;
    [AppliedWithChunk<Chunk2E008000>]
    public string? Crosshair { get => crosshair; set => crosshair = value; }

    private ParticleBlock[]? particleBlocks;
    [AppliedWithChunk<Chunk2E008000>]
    public ParticleBlock[]? ParticleBlocks { get => particleBlocks; set => particleBlocks = value; }

    private SoundBlock[]? soundBlocks;
    [AppliedWithChunk<Chunk2E008000>]
    public SoundBlock[]? SoundBlocks { get => soundBlocks; set => soundBlocks = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameActionModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameActionModel() { }


    /// <summary>
    /// CGameActionModel 0x000 skippable chunk
    /// </summary>
    [Chunk(0x2E008000)]
    public partial class Chunk2E008000 : SkippableChunk<CGameActionModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E008000;

        public int Version { get; set; }

        public CPlugShieldModel? U01;
        public int U02;
        public int U03;
        public bool U04;
        public int U05;
        public string? U06;
        public int U07 = 2;
        public int U08 = 1;
        public string? U09;
        public CPlugBulletModel[]? U10;
        public CPlugFileTextScript? U11;
        public CPlugFileTextScript? U12;
        public CPlugBitmap? U13;
        public bool U14;
        public bool U99;
        public int U16;
        public float U17;
        public int U18;
        public bool U19;
        public int U20;
        public int U21;
        public bool U22;

        public override void ReadWrite(CGameActionModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 31)
            {
                rw.Boolean(ref n.useVehicleGuns);
            }
            if (Version >= 30)
            {
                rw.ArrayNodeRef_deprec<CPlugBulletModel>(ref n.bulletModels_Nadeo!);
            }
            if (Version >= 25)
            {
                rw.NodeRef<CPlugShieldModel>(ref U01);
            }
            if (Version == 24)
            {
                throw new (""); // shield model but directly
            }
            if (Version >= 23)
            {
                rw.String(ref n.actionName);
            }
            if (Version >= 22)
            {
                rw.Int32(ref n.inventoryItemClass);
            }
            if (Version >= 21)
            {
                if (Version <= 23)
                {
                    rw.Int32(ref U02);
                    rw.Int32(ref U03);
                }
            }
            if (Version >= 19)
            {
                rw.Int32(ref n.spriteBlockVersion);
                rw.ArrayReadableWritable<SpriteBlock>(ref n.spriteBlocks!);
            }
            if (Version >= 17)
            {
                rw.String(ref n.description);
            }
            if (Version >= 16)
            {
                rw.Int32(ref n.beamVersion);
                rw.ArrayReadableWritable<CPlugCustomBeamModel>(ref n.beams!, version: n.BeamVersion);
            }
            if (Version >= 15)
            {
                rw.Boolean(ref U04);
            }
            if (Version >= 26)
            {
                rw.String(ref n.actionName);
            }
            if (Version <= 25)
            {
                if (Version >= 14)
                {
                    rw.Id(ref n.actionName);
                }
            }
            if (Version >= 13)
            {
                rw.Int32(ref U05);
            }
            if (Version >= 9)
            {
                rw.Id(ref U06);
            }
            if (Version <= 9)
            {
                rw.ReadableWritable<ScriptParams>(ref n.cooldown, version: 2);
            }
            if (Version >= 10)
            {
                rw.Int32(ref n.scriptParamsVersion);
                rw.Int32(ref n.actionScriptEffectVersion);
                rw.ReadableWritable<ScriptParams>(ref n.cooldown, version: n.ScriptParamsVersion);
                rw.ArrayReadableWritable<ActionScriptEffect>(ref n.actionScriptEffects!, version: n.ActionScriptEffectVersion);
            }
            if (Version >= 7)
            {
                rw.Int32(ref n.customBulletVersion);
            }
            if (Version >= 6)
            {
                rw.ArrayReadableWritable<CPlugCustomBulletModel>(ref n.projectiles!, version: n.CustomBulletVersion);
            }
            if (Version >= 5)
            {
                rw.Int32(ref U07);
                rw.Int32(ref U08);
            }
            rw.String(ref U09);
            if (Version <= 5)
            {
                rw.ArrayNodeRef<CPlugBulletModel>(ref U10!);
            }
            rw.NodeRef<CPlugAnimFile>(ref n.anim);
            if (Version <= 10)
            {
                rw.NodeRef<CPlugFileTextScript>(ref U11);
                rw.NodeRef<CPlugScriptWithSettings>(ref n.script);
            }
            if (Version >= 11)
            {
                rw.NodeRef<CPlugScriptWithSettings>(ref n.script);
                rw.NodeRef<CPlugFileTextScript>(ref U12);
            }
            if (Version <= 11)
            {
                rw.NodeRef<CPlugBitmap>(ref U13);
            }
            if (Version >= 12)
            {
                rw.String(ref n.icon);
                rw.String(ref n.crosshair);
            }
            if (Version >= 1)
            {
                rw.ArrayReadableWritable<ParticleBlock>(ref n.particleBlocks!, version: Version);
                rw.ArrayReadableWritable<SoundBlock>(ref n.soundBlocks!, version: Version);
            }
            if (Version >= 20)
            {
                rw.Boolean(ref U14);
            }
            if (Version == 28)
            {
                rw.Boolean(ref U99);
                if (U99)
                {
                    rw.Int32(ref U16);
                    rw.Single(ref U17);
                    rw.Int32(ref U18);
                }
            }
            if (Version == 27)
            {
                rw.Boolean(ref U19);
                rw.Int32(ref U20);
                rw.Int32(ref U21);
            }
            if (Version >= 29)
            {
                rw.Boolean(ref U22);
            }
        }
    }


    public sealed partial class AnimScriptEffect : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private bool u02;
        public bool U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        private float u05;
        public float U05 { get => u05; set => u05 = value; }

        private float u06;
        public float U06 { get => u06; set => u06 = value; }

        private float u07;
        public float U07 { get => u07; set => u07 = value; }

        private float u08;
        public float U08 { get => u08; set => u08 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Boolean(ref u02);
            rw.Single(ref u03);
            rw.Single(ref u04);
            rw.Single(ref u05);
            rw.Single(ref u06);
            rw.Single(ref u07);
            rw.Single(ref u08);
        }
    }

    public sealed partial class SoundKey : IReadableWritable
    {

        private float u01;
        public float U01 { get => u01; set => u01 = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private bool u04 = true;
        public bool U04 { get => u04; set => u04 = value; }

        private bool u05 = true;
        public bool U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Single(ref u01);
            rw.Single(ref u02);
            rw.Single(ref u03);
            if (v >= 4)
            {
                rw.Boolean(ref u04);
                rw.Boolean(ref u05);
            }
        }
    }

    public sealed partial class SpriteBlock : IReadableWritable
    {

        private SpriteKey[]? keys;
        public SpriteKey[]? Keys { get => keys; set => keys = value; }

        private bool u01;
        public bool U01 { get => u01; set => u01 = value; }

        private CPlugFileImg? u02;
        public CPlugFileImg? U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.ArrayReadableWritable<SpriteKey>(ref keys!);
            rw.Boolean(ref u01);
            if (v >= 1)
            {
                rw.NodeRef<CPlugFileImg>(ref u02);
            }
        }
    }

    public sealed partial class EffectScriptEffect : IReadableWritable
    {

        private bool u01;
        public bool U01 { get => u01; set => u01 = value; }

        private bool u02;
        public bool U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private bool u04;
        public bool U04 { get => u04; set => u04 = value; }

        private bool u05;
        public bool U05 { get => u05; set => u05 = value; }

        private int u06;
        public int U06 { get => u06; set => u06 = value; }

        private int u07;
        public int U07 { get => u07; set => u07 = value; }

        private int u08;
        public int U08 { get => u08; set => u08 = value; }

        private int u09;
        public int U09 { get => u09; set => u09 = value; }

        private string? u10;
        public string? U10 { get => u10; set => u10 = value; }

        private bool u11;
        public bool U11 { get => u11; set => u11 = value; }

        private float u12;
        public float U12 { get => u12; set => u12 = value; }

        private float u13;
        public float U13 { get => u13; set => u13 = value; }

        private float u14;
        public float U14 { get => u14; set => u14 = value; }

        private float u15;
        public float U15 { get => u15; set => u15 = value; }

        private float u16;
        public float U16 { get => u16; set => u16 = value; }

        private float u17;
        public float U17 { get => u17; set => u17 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            if (v >= 2)
            {
                rw.Boolean(ref u01);
                rw.Boolean(ref u02);
            }
            if (v >= 1)
            {
                rw.Single(ref u03);
                rw.Boolean(ref u04);
                rw.Boolean(ref u05);
            }
            rw.Int32(ref u06);
            rw.Int32(ref u07);
            rw.Int32(ref u08);
            rw.Int32(ref u09);
            rw.String(ref u10);
            rw.Boolean(ref u11);
            rw.Single(ref u12);
            rw.Single(ref u13);
            rw.Single(ref u14);
            if (v == 0)
            {
                rw.Single(ref u15);
                rw.Single(ref u16);
                rw.Single(ref u17);
            }
        }
    }

    public sealed partial class ParticleBlock : IReadableWritable
    {

        private ParticleKey[]? keys;
        public ParticleKey[]? Keys { get => keys; set => keys = value; }

        private string[]? u01;
        public string[]? U01 { get => u01; set => u01 = value; }

        private bool u02;
        public bool U02 { get => u02; set => u02 = value; }

        private bool u03;
        public bool U03 { get => u03; set => u03 = value; }

        private bool u04;
        public bool U04 { get => u04; set => u04 = value; }

        private bool u05;
        public bool U05 { get => u05; set => u05 = value; }

        private bool u06;
        public bool U06 { get => u06; set => u06 = value; }

        private float u07;
        public float U07 { get => u07; set => u07 = value; }

        private CPlugParticleEmitterModel? particleEmitter;
        public CPlugParticleEmitterModel? ParticleEmitter { get => particleEmitter; set => particleEmitter = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.ArrayReadableWritable<ParticleKey>(ref keys!, version: v);
            rw.ArrayId(ref u01!);
            if (v <= 1)
            {
                rw.Boolean(ref u02);
                rw.Boolean(ref u03);
            }
            if (v >= 3)
            {
                rw.Boolean(ref u04);
            }
            if (v >= 4)
            {
                rw.Boolean(ref u05);
            }
            if (v >= 7)
            {
                rw.Boolean(ref u06);
            }
            if (v >= 8)
            {
                rw.Single(ref u07);
            }
            if (v >= 9)
            {
                rw.NodeRef<CPlugParticleEmitterModel>(ref particleEmitter);
            }
        }
    }

    public sealed partial class ScriptParams : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private bool u04;
        public bool U04 { get => u04; set => u04 = value; }

        private bool u05;
        public bool U05 { get => u05; set => u05 = value; }

        private int u06;
        public int U06 { get => u06; set => u06 = value; }

        private bool u07;
        public bool U07 { get => u07; set => u07 = value; }

        private int u08;
        public int U08 { get => u08; set => u08 = value; }

        private int u09;
        public int U09 { get => u09; set => u09 = value; }

        private bool u10;
        public bool U10 { get => u10; set => u10 = value; }

        private float u11;
        public float U11 { get => u11; set => u11 = value; }

        private float u12;
        public float U12 { get => u12; set => u12 = value; }

        private float u13;
        public float U13 { get => u13; set => u13 = value; }

        private float u14;
        public float U14 { get => u14; set => u14 = value; }

        private float u15;
        public float U15 { get => u15; set => u15 = value; }

        private float u16;
        public float U16 { get => u16; set => u16 = value; }

        private bool u17;
        public bool U17 { get => u17; set => u17 = value; }

        private float u18;
        public float U18 { get => u18; set => u18 = value; }

        private float u19;
        public float U19 { get => u19; set => u19 = value; }

        private float u20;
        public float U20 { get => u20; set => u20 = value; }

        private float u21;
        public float U21 { get => u21; set => u21 = value; }

        private float u22;
        public float U22 { get => u22; set => u22 = value; }

        private float u23;
        public float U23 { get => u23; set => u23 = value; }

        private int u24;
        public int U24 { get => u24; set => u24 = value; }

        private int u25;
        public int U25 { get => u25; set => u25 = value; }

        private int u26;
        public int U26 { get => u26; set => u26 = value; }

        private bool u27;
        public bool U27 { get => u27; set => u27 = value; }

        private bool u28;
        public bool U28 { get => u28; set => u28 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            if (v <= 2)
            {
                rw.Int32(ref u01);
                rw.Int32(ref u02);
                rw.Int32(ref u03);
                rw.Boolean(ref u04);
                rw.Boolean(ref u05);
                rw.Int32(ref u06);
                rw.Boolean(ref u07);
            }
            if (v == 2)
            {
                rw.Int32(ref u08);
                rw.Int32(ref u09);
                rw.Boolean(ref u10);
                rw.Single(ref u11);
                rw.Single(ref u12);
                rw.Single(ref u13);
                rw.Single(ref u14);
                rw.Single(ref u15);
                rw.Single(ref u16);
                rw.Boolean(ref u17);
                rw.Single(ref u18);
                rw.Single(ref u19);
                rw.Single(ref u20);
                rw.Single(ref u21);
                rw.Single(ref u22);
                rw.Single(ref u23);
            }
            if (v >= 2)
            {
                if (v >= 3)
                {
                    rw.Int32(ref u24);
                    rw.Int32(ref u25);
                    rw.Int32(ref u26);
                    rw.Boolean(ref u27);
                    if (v >= 4)
                    {
                        rw.Boolean(ref u28);
                    }
                }
            }
        }
    }

    public sealed partial class SpriteKey : IReadableWritable
    {

        private float u01;
        public float U01 { get => u01; set => u01 = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        private float u05;
        public float U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Single(ref u01);
            rw.Single(ref u02);
            rw.Single(ref u03);
            rw.Single(ref u04);
            rw.Single(ref u05);
        }
    }

    public sealed partial class ActionScriptEffect : IReadableWritable
    {

        private int type;
        public int Type { get => type; set => type = value; }

        private AnimScriptEffect? animEffect;
        public AnimScriptEffect? AnimEffect { get => animEffect; set => animEffect = value; }

        private EffectScriptEffect? effectEffect;
        public EffectScriptEffect? EffectEffect { get => effectEffect; set => effectEffect = value; }

        private BuffScriptEffect? buffEffect;
        public BuffScriptEffect? BuffEffect { get => buffEffect; set => buffEffect = value; }

        private ProjScriptEffect? projEffect;
        public ProjScriptEffect? ProjEffect { get => projEffect; set => projEffect = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref type);
            if (Type==0)
            {
                rw.ReadableWritable<AnimScriptEffect>(ref animEffect);
            }
            if (Type==5)
            {
                rw.ReadableWritable<EffectScriptEffect>(ref effectEffect);
            }
            if (Type==3)
            {
                rw.ReadableWritable<BuffScriptEffect>(ref buffEffect);
            }
            if (Type!=0&&Type!=5&&Type!=3)
            {
                rw.ReadableWritable<ProjScriptEffect>(ref projEffect);
            }
        }
    }

    public sealed partial class ParticleKey : IReadableWritable
    {

        private float u01;
        public float U01 { get => u01; set => u01 = value; }

        private Iso4 u02;
        public Iso4 U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        private float u05;
        public float U05 { get => u05; set => u05 = value; }

        private float u06;
        public float U06 { get => u06; set => u06 = value; }

        private bool u07;
        public bool U07 { get => u07; set => u07 = value; }

        private bool u08 = true;
        public bool U08 { get => u08; set => u08 = value; }

        private bool u09 = true;
        public bool U09 { get => u09; set => u09 = value; }

        private bool u10 = true;
        public bool U10 { get => u10; set => u10 = value; }

        private bool u11 = true;
        public bool U11 { get => u11; set => u11 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Single(ref u01);
            rw.Iso4(ref u02);
            rw.Single(ref u03);
            rw.Single(ref u04);
            rw.Single(ref u05);
            rw.Single(ref u06);
            if (v >= 5)
            {
                rw.Boolean(ref u07);
                rw.Boolean(ref u08);
                rw.Boolean(ref u09);
            }
            if (v >= 6)
            {
                rw.Boolean(ref u10);
                rw.Boolean(ref u11);
            }
        }
    }

    public sealed partial class ProjScriptEffect : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private bool u03;
        public bool U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        private float u05;
        public float U05 { get => u05; set => u05 = value; }

        private float u06;
        public float U06 { get => u06; set => u06 = value; }

        private float u07;
        public float U07 { get => u07; set => u07 = value; }

        private float u08;
        public float U08 { get => u08; set => u08 = value; }

        private float u09;
        public float U09 { get => u09; set => u09 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            rw.Boolean(ref u03);
            rw.Single(ref u04);
            rw.Single(ref u05);
            rw.Single(ref u06);
            rw.Single(ref u07);
            rw.Single(ref u08);
            rw.Single(ref u09);
        }
    }

    public sealed partial class SoundBlock : IReadableWritable
    {

        private SoundKey[]? keys;
        public SoundKey[]? Keys { get => keys; set => keys = value; }

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        private float u05;
        public float U05 { get => u05; set => u05 = value; }

        private float u06;
        public float U06 { get => u06; set => u06 = value; }

        private float u07;
        public float U07 { get => u07; set => u07 = value; }

        private float u08;
        public float U08 { get => u08; set => u08 = value; }

        private float u09;
        public float U09 { get => u09; set => u09 = value; }

        private float u10;
        public float U10 { get => u10; set => u10 = value; }

        private float u11;
        public float U11 { get => u11; set => u11 = value; }

        private float u12;
        public float U12 { get => u12; set => u12 = value; }

        private float u13;
        public float U13 { get => u13; set => u13 = value; }

        private bool u14;
        public bool U14 { get => u14; set => u14 = value; }

        private int u15;
        public int U15 { get => u15; set => u15 = value; }

        private bool u16 = true;
        public bool U16 { get => u16; set => u16 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.ArrayReadableWritable<SoundKey>(ref keys!);
            rw.Id(ref u01);
            rw.Single(ref u02);
            rw.Single(ref u03);
            rw.Single(ref u04);
            rw.Single(ref u05);
            rw.Single(ref u06);
            rw.Single(ref u07);
            rw.Single(ref u08);
            rw.Single(ref u09);
            rw.Single(ref u10);
            rw.Single(ref u11);
            rw.Single(ref u12);
            rw.Single(ref u13);
            if (v <= 2)
            {
                rw.Boolean(ref u14);
            }
            if (v >= 1)
            {
                rw.Int32(ref u15);
            }
            if (v >= 2)
            {
                rw.Boolean(ref u16);
            }
        }
    }

    public sealed partial class BuffScriptEffect : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private string? u03;
        public string? U03 { get => u03; set => u03 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            rw.String(ref u03);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x2E008000 => new Chunk2E008000(),
        _ => base.NewChunk(chunkId),
    };
}
