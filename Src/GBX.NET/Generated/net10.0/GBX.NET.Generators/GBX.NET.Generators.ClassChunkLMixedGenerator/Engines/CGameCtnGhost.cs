namespace GBX.NET.Engines.Game;

/// <summary>
/// A ghost.
/// </summary>
/// <remarks>ID: 0x03092000</remarks>
[Class(0x03092000)]
public partial class CGameCtnGhost : CGameGhost, IClass
{
    [Hexadecimal] public static new uint Id => 0x03092000;




    private int appearanceVersion;
    [AppliedWithChunk<Chunk03092000>]
    public int AppearanceVersion { get => appearanceVersion; set => appearanceVersion = value; }

    private Ident? playerModel;
    [AppliedWithChunk<Chunk03092000>]
    [AppliedWithChunk<Chunk03092003>]
    [AppliedWithChunk<Chunk03092006>]
    [AppliedWithChunk<Chunk0309200D>]
    [AppliedWithChunk<Chunk03092018>]
    public Ident? PlayerModel { get => playerModel; set => playerModel = value; }

    private Vec3 lightTrailColor;
    [AppliedWithChunk<Chunk03092000>]
    [AppliedWithChunk<Chunk03092007>]
    [AppliedWithChunk<Chunk03092009>]
    public Vec3 LightTrailColor { get => lightTrailColor; set => lightTrailColor = value; }

    private List<PackDesc>? skinPackDescs;
    [AppliedWithChunk<Chunk03092000>]
    [AppliedWithChunk<Chunk03092017>]
    public List<PackDesc>? SkinPackDescs { get => skinPackDescs; set => skinPackDescs = value; }

    private bool hasBadges;
    /// <summary>
    /// boolnode?
    /// </summary>
    [AppliedWithChunk<Chunk03092000>]
    public bool HasBadges { get => hasBadges; set => hasBadges = value; }

    private SBadge? badge;
    [AppliedWithChunk<Chunk03092000>]
    public SBadge? Badge { get => badge; set => badge = value; }

    private string? ghostAvatarName;
    [AppliedWithChunk<Chunk03092000>]
    [AppliedWithChunk<Chunk03092017>]
    public string? GhostAvatarName { get => ghostAvatarName; set => ghostAvatarName = value; }

    private string? recordingContext;
    [AppliedWithChunk<Chunk03092000>]
    public string? RecordingContext { get => recordingContext; set => recordingContext = value; }

    private CPlugEntRecordData? recordData;
    [AppliedWithChunk<Chunk03092000>]
    public CPlugEntRecordData? RecordData { get => recordData; set => recordData = value; }

    private string? ghostTrigram;
    [AppliedWithChunk<Chunk03092000>]
    public string? GhostTrigram { get => ghostTrigram; set => ghostTrigram = value; }

    private string? ghostZone;
    [AppliedWithChunk<Chunk03092000>]
    public string? GhostZone { get => ghostZone; set => ghostZone = value; }

    private string? skinFile;
    [AppliedWithChunk<Chunk03092003>]
    [AppliedWithChunk<Chunk03092006>]
    [AppliedWithChunk<Chunk0309200D>]
    public string? SkinFile { get => skinFile; set => skinFile = value; }

    private Checkpoint[]? checkpoints;
    [AppliedWithChunk<Chunk03092004>]
    [AppliedWithChunk<Chunk0309200B>]
    public Checkpoint[]? Checkpoints { get => checkpoints; set => checkpoints = value; }

    private TimeInt32? raceTime;
    [AppliedWithChunk<Chunk03092005>]
    public TimeInt32? RaceTime { get => raceTime; set => raceTime = value; }

    private int? respawns;
    [AppliedWithChunk<Chunk03092008>]
    public int? Respawns { get => respawns; set => respawns = value; }

    private int? stuntScore;
    [AppliedWithChunk<Chunk0309200A>]
    public int? StuntScore { get => stuntScore; set => stuntScore = value; }

    private string? ghostLogin;
    [AppliedWithChunk<Chunk0309200F>]
    public string? GhostLogin { get => ghostLogin; set => ghostLogin = value; }

    private string? validate_ChallengeUid;
    [AppliedWithChunk<Chunk03092010>]
    public string? Validate_ChallengeUid { get => validate_ChallengeUid; set => validate_ChallengeUid = value; }

    private UInt128? securityKey128;
    [AppliedWithChunk<Chunk03092012>]
    public UInt128? SecurityKey128 { get => securityKey128; set => securityKey128 = value; }

    private int? ghostVersion;
    [AppliedWithChunk<Chunk03092014>]
    public int? GhostVersion { get => ghostVersion; set => ghostVersion = value; }

    private UInt256? securityKey256;
    [AppliedWithChunk<Chunk0309201C>]
    public UInt256? SecurityKey256 { get => securityKey256; set => securityKey256 = value; }

    private PlayerInputData[]? playerInputs;
    [AppliedWithChunk<Chunk0309201D>]
    public PlayerInputData[]? PlayerInputs { get => playerInputs; set => playerInputs = value; }

    private OldSettingsInfos[]? oldSettings;
    [AppliedWithChunk<Chunk03092022>]
    public OldSettingsInfos[]? OldSettings { get => oldSettings; set => oldSettings = value; }

    private SettingsInfos[]? settings;
    [AppliedWithChunk<Chunk03092022>]
    public SettingsInfos[]? Settings { get => settings; set => settings = value; }

    private MatchReplaySeparator[]? matchReplaySeparators;
    [AppliedWithChunk<Chunk03092024>]
    public MatchReplaySeparator[]? MatchReplaySeparators { get => matchReplaySeparators; set => matchReplaySeparators = value; }

    private Checksum128? ghostUid128;
    [AppliedWithChunk<Chunk03092026>]
    public Checksum128? GhostUid128 { get => ghostUid128; set => ghostUid128 = value; }

    private DateTimeOffset? walltimeStartTimestamp;
    [AppliedWithChunk<Chunk0309202C>]
    public DateTimeOffset? WalltimeStartTimestamp { get => walltimeStartTimestamp; set => walltimeStartTimestamp = value; }

    private DateTimeOffset? walltimeEndTimestamp;
    [AppliedWithChunk<Chunk0309202C>]
    public DateTimeOffset? WalltimeEndTimestamp { get => walltimeEndTimestamp; set => walltimeEndTimestamp = value; }


    /// <summary>
    /// CGameCtnGhost 0x001 skippable chunk (TMCP)
    /// </summary>
    [Chunk(0x0304F001, "TMCP")]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk0304F001 : SkippableChunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304F001;

        /// <inheritdoc />
        public override bool Ignore => true;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;

    }

    /// <summary>
    /// CGameCtnGhost 0x000 skippable chunk
    /// </summary>
    [Chunk(0x03092000)]
    [ChunkGameVersion(GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020, 2, 2, 9)]
    public partial class Chunk03092000 : SkippableChunk<CGameCtnGhost>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03092000;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }

        public string? U01;
        public bool U02;
        public int[]? U03;

        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 9)
            {
                rw.Int32(ref n.appearanceVersion);
            }
            rw.Ident(ref n.playerModel);
            rw.Vec3(ref n.lightTrailColor);
            rw.ListPackDesc(ref n.skinPackDescs!);
            rw.Boolean(ref n.hasBadges); // boolnode?
            if (n.HasBadges)
            {
                rw.ReadableWritable<SBadge>(ref n.badge);
            }
            if (n.AppearanceVersion>=1)
            {
                rw.String(ref U01);
            }
            n.GhostNickname = rw.String(n.GhostNickname);
            rw.String(ref n.ghostAvatarName);
            if (Version >= 2)
            {
                rw.String(ref n.recordingContext);
                if (Version >= 4)
                {
                    rw.Boolean(ref U02);
                    if (Version >= 5)
                    {
                        rw.NodeRef<CPlugEntRecordData>(ref n.recordData);
                        rw.Array<int>(ref U03!);
                        if (Version >= 6)
                        {
                            rw.String(ref n.ghostTrigram);
                            if (Version >= 7)
                            {
                                rw.String(ref n.ghostZone);
                                if (Version >= 8)
                                {
                                    n.GhostClubTag = rw.String(n.GhostClubTag);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x003 chunk
    /// </summary>
    [Chunk(0x03092003)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMPU)]
    public partial class Chunk03092003 : Chunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x03092003;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMPU;


        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.Ident(ref n.playerModel);
            rw.String(ref n.skinFile);
            n.GhostNickname = rw.String(n.GhostNickname);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x004 skippable chunk (checkpoints TMPU)
    /// </summary>
    [Chunk(0x03092004, "checkpoints TMPU")]
    [ChunkGameVersion(GameVersion.TMPU)]
    public partial class Chunk03092004 : SkippableChunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x03092004;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMPU;


        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.ArrayReadableWritable<Checkpoint>(ref n.checkpoints!);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x005 skippable chunk (race time)
    /// </summary>
    [Chunk(0x03092005, "race time")]
    [ChunkGameVersion(GameVersion.TMPU | GameVersion.TMS | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03092005 : SkippableChunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x03092005;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMPU | GameVersion.TMS | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.TimeInt32Nullable(ref n.raceTime);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x006 chunk
    /// </summary>
    [Chunk(0x03092006)]
    [ChunkGameVersion(GameVersion.TMS | GameVersion.TMSX | GameVersion.TMNESWC)]
    public partial class Chunk03092006 : Chunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x03092006;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMS | GameVersion.TMSX | GameVersion.TMNESWC;

        public int U01;

        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.Ident(ref n.playerModel);
            rw.String(ref n.skinFile);
            rw.Int32(ref U01);
            n.GhostNickname = rw.String(n.GhostNickname);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x007 chunk (old light trail color)
    /// </summary>
    [Chunk(0x03092007, "old light trail color")]
    public partial class Chunk03092007 : Chunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x03092007;


        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.Vec3(ref n.lightTrailColor);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x008 skippable chunk (respawns)
    /// </summary>
    [Chunk(0x03092008, "respawns")]
    [ChunkGameVersion(GameVersion.TMS | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03092008 : SkippableChunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x03092008;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMS | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.respawns);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x009 skippable chunk (light trail color)
    /// </summary>
    [Chunk(0x03092009, "light trail color")]
    [ChunkGameVersion(GameVersion.TMS | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3)]
    public partial class Chunk03092009 : SkippableChunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x03092009;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMS | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3;


        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.Vec3(ref n.lightTrailColor);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x00A skippable chunk (stunt score)
    /// </summary>
    [Chunk(0x0309200A, "stunt score")]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0309200A : SkippableChunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309200A;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.stuntScore);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x00B skippable chunk (checkpoint times)
    /// </summary>
    [Chunk(0x0309200B, "checkpoint times")]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0309200B : SkippableChunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309200B;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.ArrayReadableWritable<Checkpoint>(ref n.checkpoints!, version: 1);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x00C chunk
    /// </summary>
    [Chunk(0x0309200C)]
    [ChunkGameVersion(GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0309200C : Chunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309200C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int U01;

        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x00D chunk
    /// </summary>
    [Chunk(0x0309200D)]
    [ChunkGameVersion(GameVersion.TMU)]
    public partial class Chunk0309200D : Chunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309200D;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU;

        public int U01;
        public int U02;
        public int U03;
        public int U04;

        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.Ident(ref n.playerModel);
            rw.String(ref n.skinFile);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            n.GhostNickname = rw.String(n.GhostNickname);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x00E chunk (GhostUid)
    /// </summary>
    [Chunk(0x0309200E, "GhostUid")]
    [ChunkGameVersion(GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0309200E : Chunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309200E;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnGhost 0x00F chunk (ghost login)
    /// </summary>
    [Chunk(0x0309200F, "ghost login")]
    [ChunkGameVersion(GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0309200F : Chunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309200F;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.String(ref n.ghostLogin);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x010 chunk (validation map UID)
    /// </summary>
    [Chunk(0x03092010, "validation map UID")]
    [ChunkGameVersion(GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03092010 : Chunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x03092010;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.Id(ref n.validate_ChallengeUid);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x011 chunk (validation TMU)
    /// </summary>
    [Chunk(0x03092011, "validation TMU")]
    [ChunkGameVersion(GameVersion.TMU)]
    public partial class Chunk03092011 : Chunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x03092011;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU;

    }

    /// <summary>
    /// CGameCtnGhost 0x012 chunk (old security key)
    /// </summary>
    [Chunk(0x03092012, "old security key")]
    [ChunkGameVersion(GameVersion.TMU | GameVersion.TMF)]
    public partial class Chunk03092012 : Chunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x03092012;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU | GameVersion.TMF;

        public uint U01;

        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.UInt32(ref U01);
            rw.UInt128(ref n.securityKey128);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x013 skippable chunk
    /// </summary>
    [Chunk(0x03092013)]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03092013 : SkippableChunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x03092013;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int U01;
        public int U02;

        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x014 skippable chunk (ghost version)
    /// </summary>
    [Chunk(0x03092014, "ghost version")]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03092014 : SkippableChunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x03092014;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.ghostVersion);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x015 chunk (ghost nickname)
    /// </summary>
    [Chunk(0x03092015, "ghost nickname")]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk03092015 : Chunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x03092015;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;


        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            n.GhostNickname = rw.Id(n.GhostNickname);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x017 skippable chunk (ghost metadata)
    /// </summary>
    [Chunk(0x03092017, "ghost metadata")]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3)]
    public partial class Chunk03092017 : SkippableChunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x03092017;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3;


        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.ListPackDesc(ref n.skinPackDescs!);
            n.GhostNickname = rw.String(n.GhostNickname);
            rw.String(ref n.ghostAvatarName);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x018 chunk (PlayerModel TMF-MP3)
    /// </summary>
    [Chunk(0x03092018, "PlayerModel TMF-MP3")]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3)]
    public partial class Chunk03092018 : Chunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x03092018;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3;


        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.Ident(ref n.playerModel);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x019 chunk (validation TMUF)
    /// </summary>
    [Chunk(0x03092019, "validation TMUF")]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk03092019 : Chunk03092011
    {
        /// <inheritdoc />
        public override uint Id => 0x03092019;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;

    }

    /// <summary>
    /// CGameCtnGhost 0x01A skippable chunk (checkpoint count)
    /// </summary>
    [Chunk(0x0309201A, "checkpoint count")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0309201A : SkippableChunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309201A;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnGhost 0x01B skippable chunk (race result)
    /// </summary>
    [Chunk(0x0309201B, "race result")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0309201B : SkippableChunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309201B;

        /// <inheritdoc />
        public override bool Ignore => true;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnGhost 0x01C chunk (security key)
    /// </summary>
    [Chunk(0x0309201C, "security key")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0309201C : Chunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309201C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.UInt256(ref n.securityKey256);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x01D skippable chunk (player input data)
    /// </summary>
    [Chunk(0x0309201D, "player input data")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020, 3, 3, 3, 4)]
    public partial class Chunk0309201D : SkippableChunk<CGameCtnGhost>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0309201D;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<PlayerInputData>(ref n.playerInputs!, version: Version);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x01F skippable chunk (OldColorHistory)
    /// </summary>
    [Chunk(0x0309201F, "OldColorHistory")]
    [ChunkGameVersion(GameVersion.MP4)]
    public partial class Chunk0309201F : SkippableChunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309201F;

        /// <inheritdoc />
        public override bool Ignore => true;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4;

    }

    /// <summary>
    /// CGameCtnGhost 0x021 skippable chunk (OldKeyStrokes)
    /// </summary>
    [Chunk(0x03092021, "OldKeyStrokes")]
    public partial class Chunk03092021 : SkippableChunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x03092021;

        /// <inheritdoc />
        public override bool Ignore => true;

    }

    /// <summary>
    /// CGameCtnGhost 0x022 skippable chunk (settings info)
    /// </summary>
    [Chunk(0x03092022, "settings info")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03092022 : SkippableChunk<CGameCtnGhost>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03092022;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version <= 1)
            {
                rw.ArrayReadableWritable<OldSettingsInfos>(ref n.oldSettings!);
            }
            if (Version >= 2)
            {
                rw.ArrayReadableWritable<SettingsInfos>(ref n.settings!);
            }
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x023 skippable chunk (anticheat data)
    /// </summary>
    [Chunk(0x03092023, "anticheat data")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03092023 : SkippableChunk<CGameCtnGhost>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03092023;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }

        public string? U01;
        public int U02;
        public string? U03;
        public int U04;
        public int U05;
        public string? U06;
        public int U07;
        public string? U08;
        public byte U09;
        public int U10;
        public int U11;
        public byte U12;
        public byte U13;

        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref U01);
            rw.Int32(ref U02);
            rw.String(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.String(ref U06);
            rw.Int32(ref U07);
            rw.String(ref U08);
            if (Version >= 2)
            {
                rw.Byte(ref U09);
                rw.Int32(ref U10);
                rw.Int32(ref U11);
                if (Version >= 3)
                {
                    rw.Byte(ref U12);
                    rw.Byte(ref U13);
                }
            }
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x024 skippable chunk (match replay separators)
    /// </summary>
    [Chunk(0x03092024, "match replay separators")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03092024 : SkippableChunk<CGameCtnGhost>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03092024;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<MatchReplaySeparator>(ref n.matchReplaySeparators!);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x025 skippable chunk (validation TM2)
    /// </summary>
    [Chunk(0x03092025, "validation TM2")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020, 0, 0, 0, 1)]
    public partial class Chunk03092025 : SkippableChunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x03092025;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnGhost 0x026 skippable chunk (GhostUid128)
    /// </summary>
    [Chunk(0x03092026, "GhostUid128")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03092026 : SkippableChunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x03092026;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.Checksum128(ref n.ghostUid128);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x027 skippable chunk (timed pixel array)
    /// </summary>
    [Chunk(0x03092027, "timed pixel array")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03092027 : SkippableChunk<CGameCtnGhost>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03092027;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }

        public CPlugTimedPixelArray[]? U01;
        public int[]? U02;
        public CPlugTimedPixelArray[]? U03;

        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayNodeRef<CPlugTimedPixelArray>(ref U01!);
            if (Version >= 1)
            {
                rw.Array<int>(ref U02!);
                if (Version >= 3)
                {
                    rw.ArrayNodeRef<CPlugTimedPixelArray>(ref U03!);
                }
            }
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x028 skippable chunk (title id)
    /// </summary>
    [Chunk(0x03092028, "title id")]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03092028 : SkippableChunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x03092028;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnGhost 0x029 skippable chunk
    /// </summary>
    [Chunk(0x03092029)]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk03092029 : SkippableChunk<CGameCtnGhost>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03092029;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

        public int Version { get; set; }

        public int U01;
        public string? U02;
        public string? U03;
        public string? U04;

        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.String(ref U02);
            rw.String(ref U03);
            rw.String(ref U04);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x02A skippable chunk
    /// </summary>
    [Chunk(0x0309202A)]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk0309202A : SkippableChunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309202A;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

        public int U01;
        public int U02;

        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x02B skippable chunk
    /// </summary>
    [Chunk(0x0309202B)]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk0309202B : SkippableChunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309202B;

        /// <inheritdoc />
        public override bool Ignore => true;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnGhost 0x02C skippable chunk
    /// </summary>
    [Chunk(0x0309202C)]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk0309202C : SkippableChunk<CGameCtnGhost>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0309202C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.UnixTime(ref n.walltimeStartTimestamp);
            rw.UnixTime(ref n.walltimeEndTimestamp);
        }
    }

    /// <summary>
    /// CGameCtnGhost 0x02D skippable chunk
    /// </summary>
    [Chunk(0x0309202D)]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk0309202D : SkippableChunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309202D;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnGhost 0x02E skippable chunk
    /// </summary>
    [Chunk(0x0309202E)]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk0309202E : SkippableChunk<CGameCtnGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309202E;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

        public string? U01;

        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
        }
    }


    public sealed partial class Checkpoint : IReadableWritable
    {

        private TimeInt32? time;
        public TimeInt32? Time { get => time; set => time = value; }

        private float? speed;
        public float? Speed { get => speed; set => speed = value; }

        private int? stuntsScore;
        public int? StuntsScore { get => stuntsScore; set => stuntsScore = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeInt32Nullable(ref time);
            if (v == 0)
            {
                rw.Single(ref speed);
            }
            if (v == 1)
            {
                rw.Int32(ref stuntsScore);
            }
        }
    }

    public sealed partial class SSticker : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.String(ref u01);
            rw.String(ref u02);
        }
    }

    public sealed partial class SettingsInfos : IReadableWritable
    {

        private bool u01;
        public bool U01 { get => u01; set => u01 = value; }

        private bool u02;
        public bool U02 { get => u02; set => u02 = value; }

        private byte u03;
        public byte U03 { get => u03; set => u03 = value; }

        private bool u04;
        public bool U04 { get => u04; set => u04 = value; }

        private float u05;
        public float U05 { get => u05; set => u05 = value; }

        private bool u06;
        public bool U06 { get => u06; set => u06 = value; }

        private float u07;
        public float U07 { get => u07; set => u07 = value; }

        private float[]? u08;
        public float[]? U08 { get => u08; set => u08 = value; }

        private string? u09;
        public string? U09 { get => u09; set => u09 = value; }

        private Int3 u10;
        public Int3 U10 { get => u10; set => u10 = value; }

        private float u11;
        public float U11 { get => u11; set => u11 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Boolean(ref u01);
            rw.Boolean(ref u02);
            rw.Byte(ref u03);
            rw.Boolean(ref u04);
            rw.Single(ref u05);
            rw.Boolean(ref u06);
            rw.Single(ref u07);
            rw.Array<float>(ref u08!);
            rw.String(ref u09);
            if (v >= 3)
            {
                rw.Int3(ref u10);
                if (v >= 4)
                {
                    rw.Single(ref u11);
                }
            }
        }
    }

    public sealed partial class MatchReplaySeparator : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
        }
    }

    public sealed partial class SBadge : IReadableWritable
    {

        private int version;
        public int Version { get => version; set => version = value; }

        private Vec3 color;
        public Vec3 Color { get => color; set => color = value; }

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        private List<SSticker>? stickers;
        public List<SSticker>? Stickers { get => stickers; set => stickers = value; }

        private List<string>? layers;
        public List<string>? Layers { get => layers; set => layers = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref this.version);
            rw.Vec3(ref color);
            if (Version==0)
            {
                rw.Int32(ref u01);
                rw.String(ref u02);
            }
            rw.ListReadableWritable<SSticker>(ref stickers!);
            rw.ListString(ref layers!);
        }
    }

    public sealed partial class PlayerInputData : IReadableWritable
    {

        private EVersion version;
        /// <summary>
        /// 8 in ShootMania, 12 in TM2020
        /// </summary>
        public EVersion Version { get => version; set => version = value; }

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private TimeInt32? startOffset;
        public TimeInt32? StartOffset { get => startOffset; set => startOffset = value; }

        private int ticks;
        public int Ticks { get => ticks; set => ticks = value; }

        private byte[]? data;
        public byte[]? Data { get => data; set => data = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.EnumInt32<EVersion>(ref this.version); // 8 in ShootMania, 12 in TM2020
            rw.Int32(ref u01);
            if (v >= 4)
            {
                rw.TimeInt32Nullable(ref startOffset);
            }
            rw.Int32(ref ticks);
            rw.Data(ref data);
        }
    }

    public sealed partial class OldSettingsInfos : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0304F001 => new Chunk0304F001(),
        0x03092000 => new Chunk03092000(),
        0x03092003 => new Chunk03092003(),
        0x03092004 => new Chunk03092004(),
        0x03092005 => new Chunk03092005(),
        0x03092006 => new Chunk03092006(),
        0x03092007 => new Chunk03092007(),
        0x03092008 => new Chunk03092008(),
        0x03092009 => new Chunk03092009(),
        0x0309200A => new Chunk0309200A(),
        0x0309200B => new Chunk0309200B(),
        0x0309200C => new Chunk0309200C(),
        0x0309200D => new Chunk0309200D(),
        0x0309200E => new Chunk0309200E(),
        0x0309200F => new Chunk0309200F(),
        0x03092010 => new Chunk03092010(),
        0x03092011 => new Chunk03092011(),
        0x03092012 => new Chunk03092012(),
        0x03092013 => new Chunk03092013(),
        0x03092014 => new Chunk03092014(),
        0x03092015 => new Chunk03092015(),
        0x03092017 => new Chunk03092017(),
        0x03092018 => new Chunk03092018(),
        0x03092019 => new Chunk03092019(),
        0x0309201A => new Chunk0309201A(),
        0x0309201B => new Chunk0309201B(),
        0x0309201C => new Chunk0309201C(),
        0x0309201D => new Chunk0309201D(),
        0x0309201F => new Chunk0309201F(),
        0x03092021 => new Chunk03092021(),
        0x03092022 => new Chunk03092022(),
        0x03092023 => new Chunk03092023(),
        0x03092024 => new Chunk03092024(),
        0x03092025 => new Chunk03092025(),
        0x03092026 => new Chunk03092026(),
        0x03092027 => new Chunk03092027(),
        0x03092028 => new Chunk03092028(),
        0x03092029 => new Chunk03092029(),
        0x0309202A => new Chunk0309202A(),
        0x0309202B => new Chunk0309202B(),
        0x0309202C => new Chunk0309202C(),
        0x0309202D => new Chunk0309202D(),
        0x0309202E => new Chunk0309202E(),
        _ => base.NewChunk(chunkId),
    };
}
