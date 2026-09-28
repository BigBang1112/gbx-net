namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0308C000</remarks>
[Class(0x0308C000)]
public partial class CGamePlayerProfile : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0308C000;




    private string? onlineLogin;
    [AppliedWithChunk<HeaderChunk0308C000>]
    public string? OnlineLogin { get => onlineLogin; set => onlineLogin = value; }

    private string? onlineSupportKey;
    /// <summary>
    /// Has AllowUnprintableStrings around it, so it can contain any odd character.
    /// </summary>
    [AppliedWithChunk<HeaderChunk0308C000>]
    public string? OnlineSupportKey { get => onlineSupportKey; set => onlineSupportKey = value; }

    private string? currentSoloPlaylistName;
    [AppliedWithChunk<Chunk0308C038>]
    public string? CurrentSoloPlaylistName { get => currentSoloPlaylistName; set => currentSoloPlaylistName = value; }

    private string? currentSoloPlaylistPath;
    [AppliedWithChunk<Chunk0308C038>]
    public string? CurrentSoloPlaylistPath { get => currentSoloPlaylistPath; set => currentSoloPlaylistPath = value; }

    private bool autoConnect;
    [AppliedWithChunk<Chunk0308C041>]
    public bool AutoConnect { get => autoConnect; set => autoConnect = value; }

    private VehicleProfile[]? vehicleProfiles;
    [AppliedWithChunk<Chunk0308C043>]
    public VehicleProfile[]? VehicleProfiles { get => vehicleProfiles; set => vehicleProfiles = value; }

    private bool isShowPlayerGhost;
    [AppliedWithChunk<Chunk0308C044>]
    public bool IsShowPlayerGhost { get => isShowPlayerGhost; set => isShowPlayerGhost = value; }

    private CInputBindingsConfig? bindingsConfig;
    [AppliedWithChunk<Chunk0308C04B>]
    public CInputBindingsConfig? BindingsConfig { get => bindingsConfig; set => bindingsConfig = value; }

    private CGameLeague[]? leagues;
    [AppliedWithChunk<Chunk0308C04C>]
    public CGameLeague[]? Leagues { get => leagues; set => leagues = value; }

    private CGameBuddy[]? buddies;
    [AppliedWithChunk<Chunk0308C056>]
    public CGameBuddy[]? Buddies { get => buddies; set => buddies = value; }

    private bool askOpponents;
    [AppliedWithChunk<Chunk0308C058>]
    public bool AskOpponents { get => askOpponents; set => askOpponents = value; }

    private bool lockHigherDifficulties;
    [AppliedWithChunk<Chunk0308C059>]
    public bool LockHigherDifficulties { get => lockHigherDifficulties; set => lockHigherDifficulties = value; }

    private string? profileName;
    [AppliedWithChunk<Chunk0308C05B>]
    public string? ProfileName { get => profileName; set => profileName = value; }

    private CGameCtnMediaShootParams? shootParams;
    [AppliedWithChunk<Chunk0308C05E>]
    public CGameCtnMediaShootParams? ShootParams { get => shootParams; set => shootParams = value; }

    private CInputBindingsConfig? bindingsForCompatConfig;
    [AppliedWithChunk<Chunk0308C062>]
    public CInputBindingsConfig? BindingsForCompatConfig { get => bindingsForCompatConfig; set => bindingsForCompatConfig = value; }

    private bool enableChat;
    [AppliedWithChunk<Chunk0308C063>]
    public bool EnableChat { get => enableChat; set => enableChat = value; }

    private bool enableAvatars;
    [AppliedWithChunk<Chunk0308C063>]
    public bool EnableAvatars { get => enableAvatars; set => enableAvatars = value; }

    private bool enableCarSkinGeom;
    [AppliedWithChunk<Chunk0308C063>]
    public bool EnableCarSkinGeom { get => enableCarSkinGeom; set => enableCarSkinGeom = value; }

    private bool enableUnlimitedHorns;
    [AppliedWithChunk<Chunk0308C063>]
    public bool EnableUnlimitedHorns { get => enableUnlimitedHorns; set => enableUnlimitedHorns = value; }

    private bool unlockAllCheat;
    [AppliedWithChunk<Chunk0308C063>]
    public bool UnlockAllCheat { get => unlockAllCheat; set => unlockAllCheat = value; }

    private PlayerTagConfig[]? playerTags;
    [AppliedWithChunk<Chunk0308C064>]
    public PlayerTagConfig[]? PlayerTags { get => playerTags; set => playerTags = value; }

    private ChallengeOpponent[]? challengeOpponents;
    [AppliedWithChunk<Chunk240B5006>]
    public ChallengeOpponent[]? ChallengeOpponents { get => challengeOpponents; set => challengeOpponents = value; }

    private CampaignUnlock[]? campaignUnlocks;
    [AppliedWithChunk<Chunk240B5007>]
    public CampaignUnlock[]? CampaignUnlocks { get => campaignUnlocks; set => campaignUnlocks = value; }

    /// <summary>
    /// [SNetPlayerProfileHeaderOnlineSupportKey] CGamePlayerProfile 0x000 header chunk
    /// </summary>
    [Chunk(0x0308C000)]
    public partial class HeaderChunk0308C000 : HeaderChunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C000;


        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.String(ref n.onlineLogin);
            rw.String(ref n.onlineSupportKey); // Has AllowUnprintableStrings around it, so it can contain any odd character.
        }
    }


    /// <summary>
    /// CGamePlayerProfile 0x038 chunk
    /// </summary>
    [Chunk(0x0308C038)]
    public partial class Chunk0308C038 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C038;

        public bool U01;
        public Vec3 U02;
        public Vec3 U03;
        public Vec3 U04;
        public Vec3 U05;
        public Vec3 U06;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.String(ref n.currentSoloPlaylistName);
            rw.String(ref n.currentSoloPlaylistPath);
            rw.Boolean(ref U01);
            rw.Vec3(ref U02);
            rw.Vec3(ref U03);
            rw.Vec3(ref U04);
            rw.Vec3(ref U05);
            rw.Vec3(ref U06);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x03B chunk (crypted)
    /// </summary>
    [Chunk(0x0308C03B, "crypted")]
    public partial class Chunk0308C03B : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C03B;

        public byte U01;
        public byte U02;
        public int U03;
        public int U04;
        public bool U05;
        public int U06;
        public bool U07;
        public int U08;
        public bool U09;
        public int U10;
        public bool U11;
        public bool U12;
        public bool U13;
        public bool U14;
        public bool U15;
        public bool U16;
        public int U17;
        public bool U18;
        public string? U19;
        public bool U20;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Byte(ref U01);
            rw.Byte(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Boolean(ref U05);
            rw.Int32(ref U06);
            rw.Boolean(ref U07);
            rw.Int32(ref U08);
            rw.Boolean(ref U09);
            rw.Int32(ref U10);
            rw.Boolean(ref U11);
            rw.Boolean(ref U12);
            rw.Boolean(ref U13);
            rw.Boolean(ref U14);
            rw.Boolean(ref U15);
            rw.Boolean(ref U16);
            rw.Int32(ref U17);
            rw.Boolean(ref U18);
            rw.String(ref U19);
            rw.Boolean(ref U20);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x040 chunk
    /// </summary>
    [Chunk(0x0308C040)]
    public partial class Chunk0308C040 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C040;

        public DateTime? U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.SystemTime(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x041 chunk
    /// </summary>
    [Chunk(0x0308C041)]
    public partial class Chunk0308C041 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C041;


        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.autoConnect);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x043 chunk
    /// </summary>
    [Chunk(0x0308C043)]
    public partial class Chunk0308C043 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C043;


        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.ArrayReadableWritable<VehicleProfile>(ref n.vehicleProfiles!);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x044 chunk
    /// </summary>
    [Chunk(0x0308C044)]
    public partial class Chunk0308C044 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C044;


        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isShowPlayerGhost);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x047 chunk
    /// </summary>
    [Chunk(0x0308C047)]
    public partial class Chunk0308C047 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C047;


        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CGameNetOnlineMessage>(ref n.inboxMessages!);
            rw.ArrayNodeRef_deprec<CGameNetOnlineMessage>(ref n.readMessages!);
            rw.ArrayNodeRef_deprec<CGameNetOnlineMessage>(ref n.outboxMessages!);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x04A chunk
    /// </summary>
    [Chunk(0x0308C04A)]
    public partial class Chunk0308C04A : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C04A;

        /// <summary>
        /// might be false
        /// </summary>
        public CSceneMobil[]? U01;
        public int U02;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef<CSceneMobil>(ref U01!); // might be false
            rw.Int32(ref U02);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x04B chunk
    /// </summary>
    [Chunk(0x0308C04B)]
    public partial class Chunk0308C04B : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C04B;


        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.NodeRef<CInputBindingsConfig>(ref n.bindingsConfig);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x04C chunk (LeagueManager)
    /// </summary>
    [Chunk(0x0308C04C, "LeagueManager")]
    public partial class Chunk0308C04C : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C04C;

        public byte U01;
        public int U02;
        public bool U03;
        public DateTime? U04;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Byte(ref U01);
            rw.Int32(ref U02);
            rw.Boolean(ref U03);
            if (U03)
            {
                rw.SystemTime(ref U04);
                rw.ArrayNodeRef_deprec<CGameLeague>(ref n.leagues!);
            }
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x04D chunk
    /// </summary>
    [Chunk(0x0308C04D)]
    public partial class Chunk0308C04D : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C04D;

        public DateTime? U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.SystemTime(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x050 chunk
    /// </summary>
    [Chunk(0x0308C050)]
    public partial class Chunk0308C050 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C050;

        public int U01;
        public int U02;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x052 skippable chunk
    /// </summary>
    [Chunk(0x0308C052)]
    public partial class Chunk0308C052 : SkippableChunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C052;

        public string? U01;
        public string? U02;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
            rw.String(ref U02);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x053 chunk
    /// </summary>
    [Chunk(0x0308C053)]
    public partial class Chunk0308C053 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C053;

        public string? U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x054 chunk
    /// </summary>
    [Chunk(0x0308C054)]
    public partial class Chunk0308C054 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C054;

        public bool U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x056 chunk
    /// </summary>
    [Chunk(0x0308C056)]
    public partial class Chunk0308C056 : Chunk<CGamePlayerProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C056;

        public int Version { get; set; }


        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<CGameBuddy>(ref n.buddies!);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x058 chunk
    /// </summary>
    [Chunk(0x0308C058)]
    public partial class Chunk0308C058 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C058;


        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.askOpponents);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x059 chunk
    /// </summary>
    [Chunk(0x0308C059)]
    public partial class Chunk0308C059 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C059;


        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.lockHigherDifficulties);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x05A skippable chunk
    /// </summary>
    [Chunk(0x0308C05A)]
    public partial class Chunk0308C05A : SkippableChunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C05A;

        public float U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x05B chunk
    /// </summary>
    [Chunk(0x0308C05B)]
    public partial class Chunk0308C05B : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C05B;

        public string? U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.String(ref n.profileName);
            rw.String(ref n.nickName);
            rw.Id(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x05E chunk
    /// </summary>
    [Chunk(0x0308C05E)]
    public partial class Chunk0308C05E : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C05E;


        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnMediaShootParams>(ref n.shootParams);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x05F chunk
    /// </summary>
    [Chunk(0x0308C05F)]
    public partial class Chunk0308C05F : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C05F;

        public int U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x060 skippable chunk
    /// </summary>
    [Chunk(0x0308C060)]
    public partial class Chunk0308C060 : SkippableChunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C060;

        public float U01;
        public float U02 = 1;
        public float U03;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x061 chunk
    /// </summary>
    [Chunk(0x0308C061)]
    public partial class Chunk0308C061 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C061;

        public string? U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x062 chunk (ArchiveBindingsForCompat)
    /// </summary>
    [Chunk(0x0308C062, "ArchiveBindingsForCompat")]
    public partial class Chunk0308C062 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C062;

        public int U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.NodeRef<CInputBindingsConfig>(ref n.bindingsForCompatConfig);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x063 skippable chunk
    /// </summary>
    [Chunk(0x0308C063)]
    public partial class Chunk0308C063 : SkippableChunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C063;

        public float U01;
        public float U02;
        public float U03;
        public bool U04;
        public bool U05;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.enableChat);
            rw.Boolean(ref n.enableAvatars);
            rw.Boolean(ref n.enableCarSkinGeom);
            rw.Boolean(ref n.enableUnlimitedHorns);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Boolean(ref U04);
            rw.Boolean(ref n.unlockAllCheat);
            rw.Boolean(ref U05);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x064 chunk (PlayerTagsConfig)
    /// </summary>
    [Chunk(0x0308C064, "PlayerTagsConfig")]
    public partial class Chunk0308C064 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C064;

        public int U01;
        public int[]? U02;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.ArrayReadableWritable<PlayerTagConfig>(ref n.playerTags!);
            rw.Array<int>(ref U02!);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x065 chunk
    /// </summary>
    [Chunk(0x0308C065)]
    public partial class Chunk0308C065 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C065;

        public int U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x066 chunk
    /// </summary>
    [Chunk(0x0308C066)]
    public partial class Chunk0308C066 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C066;

        public bool U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x068 chunk
    /// </summary>
    [Chunk(0x0308C068)]
    public partial class Chunk0308C068 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C068;

    }

    /// <summary>
    /// CGamePlayerProfile 0x069 chunk
    /// </summary>
    [Chunk(0x0308C069)]
    public partial class Chunk0308C069 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C069;

    }

    /// <summary>
    /// CGamePlayerProfile 0x06A chunk
    /// </summary>
    [Chunk(0x0308C06A)]
    public partial class Chunk0308C06A : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C06A;

        public bool U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x06B chunk
    /// </summary>
    [Chunk(0x0308C06B)]
    public partial class Chunk0308C06B : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C06B;

        public bool U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x06C chunk
    /// </summary>
    [Chunk(0x0308C06C)]
    public partial class Chunk0308C06C : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C06C;

        public int U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x07C skippable chunk
    /// </summary>
    [Chunk(0x0308C07C)]
    public partial class Chunk0308C07C : SkippableChunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C07C;

    }

    /// <summary>
    /// CGamePlayerProfile 0x07E skippable chunk
    /// </summary>
    [Chunk(0x0308C07E)]
    public partial class Chunk0308C07E : SkippableChunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308C07E;

    }

    /// <summary>
    /// CGamePlayerProfile 0x003 chunk
    /// </summary>
    [Chunk(0x240B5003)]
    public partial class Chunk240B5003 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x240B5003;

        public string? U01;
        public string? U02;
        public string? U03;
        public string? U04;
        public string? U05;
        public string? U06;
        public string? U07;
        public string? U08;
        public string? U09;
        public string? U10;
        public string? U11;
        public string? U12;
        public string? U13;
        public string? U14;
        public string? U15;
        public string? U16;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
            rw.String(ref U02);
            rw.String(ref U03);
            rw.String(ref U04);
            rw.String(ref U05);
            rw.String(ref U06);
            rw.String(ref U07);
            rw.String(ref U08);
            rw.Id(ref U09);
            rw.Id(ref U10);
            rw.Id(ref U11);
            rw.Id(ref U12);
            rw.Id(ref U13);
            rw.Id(ref U14);
            rw.Id(ref U15);
            rw.Id(ref U16);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x004 chunk
    /// </summary>
    [Chunk(0x240B5004)]
    public partial class Chunk240B5004 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x240B5004;

        public CGameCtnChallenge[]? U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef<CGameCtnChallenge>(ref U01!);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x006 chunk
    /// </summary>
    [Chunk(0x240B5006)]
    public partial class Chunk240B5006 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x240B5006;


        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.ArrayReadableWritable<ChallengeOpponent>(ref n.challengeOpponents!);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x007 chunk
    /// </summary>
    [Chunk(0x240B5007)]
    public partial class Chunk240B5007 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x240B5007;


        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.ArrayReadableWritable<CampaignUnlock>(ref n.campaignUnlocks!);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x009 chunk
    /// </summary>
    [Chunk(0x240B5009)]
    public partial class Chunk240B5009 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x240B5009;

        public bool U01;
        public bool U02;
        public int U03;
        public byte[]? U04;
        public byte[]? U05;
        public byte U06;
        public byte U07;
        public byte U08;
        public byte U09;
        public byte U10;
        public int U11;
        public int U12;
        public bool U13;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.Int32(ref U03);
            if (U03<5)
            {
                rw.Data(ref U04!, 44);
                if (U03==1)
                {
                    rw.Data(ref U05!, 42);
                }
            }
            rw.Byte(ref U06);
            rw.Byte(ref U07);
            rw.Byte(ref U08);
            rw.Byte(ref U09);
            rw.Byte(ref U10);
            rw.Int32(ref U11);
            rw.Int32(ref U12);
            rw.Boolean(ref U13);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x00B chunk
    /// </summary>
    [Chunk(0x240B500B)]
    public partial class Chunk240B500B : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x240B500B;

        public CGameCtnCampaign[]? U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CGameCtnCampaign>(ref U01!);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x00C chunk
    /// </summary>
    [Chunk(0x240B500C)]
    public partial class Chunk240B500C : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x240B500C;

        public Ident[]? U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.ArrayIdent(ref U01!);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x00D chunk
    /// </summary>
    [Chunk(0x240B500D)]
    public partial class Chunk240B500D : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x240B500D;

        public bool U01;
        public bool U02;
        public bool U03;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.Boolean(ref U03);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x00E chunk
    /// </summary>
    [Chunk(0x240B500E)]
    public partial class Chunk240B500E : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x240B500E;

        public int U01;
        public int U02;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x00F chunk
    /// </summary>
    [Chunk(0x240B500F)]
    public partial class Chunk240B500F : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x240B500F;

        public int[]? U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Array<int>(ref U01!);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x010 chunk
    /// </summary>
    [Chunk(0x240B5010)]
    public partial class Chunk240B5010 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x240B5010;

        public bool U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x011 chunk
    /// </summary>
    [Chunk(0x240B5011)]
    public partial class Chunk240B5011 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x240B5011;

        public int[]? U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Array<int>(ref U01!);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x012 chunk
    /// </summary>
    [Chunk(0x240B5012)]
    public partial class Chunk240B5012 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x240B5012;

        public bool U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x013 chunk
    /// </summary>
    [Chunk(0x240B5013)]
    public partial class Chunk240B5013 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x240B5013;

        public bool U01;
        public bool U02;
        public bool U03;
        public bool U04;
        public bool U05;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.Boolean(ref U03);
            rw.Boolean(ref U04);
            rw.Boolean(ref U05);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x014 chunk
    /// </summary>
    [Chunk(0x240B5014)]
    public partial class Chunk240B5014 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x240B5014;

        public int U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfile 0x015 chunk
    /// </summary>
    [Chunk(0x240B5015)]
    public partial class Chunk240B5015 : Chunk<CGamePlayerProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x240B5015;

        public SoloChallengesCurrentPage[]? U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.ArrayReadableWritable<SoloChallengesCurrentPage>(ref U01!);
        }
    }


    public sealed partial class ChallengeOpponent : IReadableWritable
    {

        private byte u01;
        public byte U01 { get => u01; set => u01 = value; }

        private Ident? u02;
        public Ident? U02 { get => u02; set => u02 = value; }

        private int[]? u03;
        public int[]? U03 { get => u03; set => u03 = value; }

        private string[]? u04;
        public string[]? U04 { get => u04; set => u04 = value; }

        private int u05;
        public int U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Byte(ref u01);
            if (U01<2)
            {
                rw.Ident(ref u02);
                rw.Array<int>(ref u03!);
                return;
            }
            rw.Ident(ref u02);
            rw.ArrayId(ref u04!);
            if (U01>=3)
            {
                rw.Int32(ref u05);
            }
        }
    }

    public sealed partial class SoloChallengesCurrentPage : IReadableWritable
    {

        private Int3 u01;
        public Int3 U01 { get => u01; set => u01 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int3(ref u01);
        }
    }

    public sealed partial class PlayerTagConfig : IReadableWritable
    {

        private string? tagId;
        public string? TagId { get => tagId; set => tagId = value; }

        private bool isVisibleByOtherPlayers;
        public bool IsVisibleByOtherPlayers { get => isVisibleByOtherPlayers; set => isVisibleByOtherPlayers = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.String(ref tagId);
            rw.Boolean(ref isVisibleByOtherPlayers);
        }
    }

    public sealed partial class VehicleProfile : IReadableWritable
    {

        private Ident? u01;
        public Ident? U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        private UInt128 u03;
        public UInt128 U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        private float u05;
        public float U05 { get => u05; set => u05 = value; }

        private float u06;
        public float U06 { get => u06; set => u06 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Ident(ref u01);
            rw.String(ref u02);
            rw.UInt128(ref u03);
            rw.Int32(ref u04);
            rw.Single(ref u05);
            rw.Single(ref u06);
        }
    }

    public sealed partial class CampaignUnlock : IReadableWritable
    {

        private byte u01;
        public byte U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        private string? u03;
        public string? U03 { get => u03; set => u03 = value; }

        private bool u04;
        public bool U04 { get => u04; set => u04 = value; }

        private bool u05;
        public bool U05 { get => u05; set => u05 = value; }

        private bool u06;
        public bool U06 { get => u06; set => u06 = value; }

        private bool u07;
        public bool U07 { get => u07; set => u07 = value; }

        private bool u08;
        public bool U08 { get => u08; set => u08 = value; }

        private bool u09;
        public bool U09 { get => u09; set => u09 = value; }

        private int u10;
        public int U10 { get => u10; set => u10 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Byte(ref u01);
            rw.Id(ref u02);
            rw.String(ref u03);
            if (U01==0)
            {
                rw.Boolean(ref u04);
                rw.Boolean(ref u05);
                rw.Boolean(ref u06);
                rw.Boolean(ref u07);
                rw.Boolean(ref u08);
                rw.Boolean(ref u09);
            }
            if (U01!=0)
            {
                rw.Int32(ref u10);
            }
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0308C038 => new Chunk0308C038(),
        0x0308C03B => new Chunk0308C03B(),
        0x0308C040 => new Chunk0308C040(),
        0x0308C041 => new Chunk0308C041(),
        0x0308C043 => new Chunk0308C043(),
        0x0308C044 => new Chunk0308C044(),
        0x0308C047 => new Chunk0308C047(),
        0x0308C04A => new Chunk0308C04A(),
        0x0308C04B => new Chunk0308C04B(),
        0x0308C04C => new Chunk0308C04C(),
        0x0308C04D => new Chunk0308C04D(),
        0x0308C050 => new Chunk0308C050(),
        0x0308C052 => new Chunk0308C052(),
        0x0308C053 => new Chunk0308C053(),
        0x0308C054 => new Chunk0308C054(),
        0x0308C056 => new Chunk0308C056(),
        0x0308C058 => new Chunk0308C058(),
        0x0308C059 => new Chunk0308C059(),
        0x0308C05A => new Chunk0308C05A(),
        0x0308C05B => new Chunk0308C05B(),
        0x0308C05E => new Chunk0308C05E(),
        0x0308C05F => new Chunk0308C05F(),
        0x0308C060 => new Chunk0308C060(),
        0x0308C061 => new Chunk0308C061(),
        0x0308C062 => new Chunk0308C062(),
        0x0308C063 => new Chunk0308C063(),
        0x0308C064 => new Chunk0308C064(),
        0x0308C065 => new Chunk0308C065(),
        0x0308C066 => new Chunk0308C066(),
        0x0308C068 => new Chunk0308C068(),
        0x0308C069 => new Chunk0308C069(),
        0x0308C06A => new Chunk0308C06A(),
        0x0308C06B => new Chunk0308C06B(),
        0x0308C06C => new Chunk0308C06C(),
        0x0308C07C => new Chunk0308C07C(),
        0x0308C07E => new Chunk0308C07E(),
        0x240B5003 => new Chunk240B5003(),
        0x240B5004 => new Chunk240B5004(),
        0x240B5006 => new Chunk240B5006(),
        0x240B5007 => new Chunk240B5007(),
        0x240B5009 => new Chunk240B5009(),
        0x240B500B => new Chunk240B500B(),
        0x240B500C => new Chunk240B500C(),
        0x240B500D => new Chunk240B500D(),
        0x240B500E => new Chunk240B500E(),
        0x240B500F => new Chunk240B500F(),
        0x240B5010 => new Chunk240B5010(),
        0x240B5011 => new Chunk240B5011(),
        0x240B5012 => new Chunk240B5012(),
        0x240B5013 => new Chunk240B5013(),
        0x240B5014 => new Chunk240B5014(),
        0x240B5015 => new Chunk240B5015(),
        _ => base.NewChunk(chunkId),
    };
}
