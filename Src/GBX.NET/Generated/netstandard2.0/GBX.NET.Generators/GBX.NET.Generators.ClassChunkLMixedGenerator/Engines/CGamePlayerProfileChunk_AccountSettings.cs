namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0312C000</remarks>
[Class(0x0312C000)]
public partial class CGamePlayerProfileChunk_AccountSettings : CGamePlayerProfileChunk, IClass, IReadableWritable
{
    [Hexadecimal] public static new uint Id => 0x0312C000;




    private int eulaVersion;
    [AppliedWithChunk<Chunk0312C000>]
    public int EulaVersion { get => eulaVersion; set => eulaVersion = value; }

    private byte flags;
    [AppliedWithChunk<Chunk0312C000>]
    public byte Flags { get => flags; set => flags = value; }

    private string? onlineLogin;
    [AppliedWithChunk<Chunk0312C000>]
    public string? OnlineLogin { get => onlineLogin; set => onlineLogin = value; }

    private string? onlinePassword;
    [AppliedWithChunk<Chunk0312C000>]
    public string? OnlinePassword { get => onlinePassword; set => onlinePassword = value; }

    private string? onlineValidationKey;
    [AppliedWithChunk<Chunk0312C000>]
    public string? OnlineValidationKey { get => onlineValidationKey; set => onlineValidationKey = value; }

    private string? onlineSupportKey;
    [AppliedWithChunk<Chunk0312C000>]
    public string? OnlineSupportKey { get => onlineSupportKey; set => onlineSupportKey = value; }

    private string? lastUsedMSAddress;
    [AppliedWithChunk<Chunk0312C000>]
    public string? LastUsedMSAddress { get => lastUsedMSAddress; set => lastUsedMSAddress = value; }

    private string? lastUsedMSPath;
    [AppliedWithChunk<Chunk0312C000>]
    public string? LastUsedMSPath { get => lastUsedMSPath; set => lastUsedMSPath = value; }

    private string? lastSessionId;
    [AppliedWithChunk<Chunk0312C000>]
    public string? LastSessionId { get => lastSessionId; set => lastSessionId = value; }

    private string? league;
    [AppliedWithChunk<Chunk0312C000>]
    public string? League { get => league; set => league = value; }

    private int onlineRemainingNickNamesChangesCount;
    [AppliedWithChunk<Chunk0312C000>]
    public int OnlineRemainingNickNamesChangesCount { get => onlineRemainingNickNamesChangesCount; set => onlineRemainingNickNamesChangesCount = value; }

    private int onlinePlanets;
    [AppliedWithChunk<Chunk0312C000>]
    public int OnlinePlanets { get => onlinePlanets; set => onlinePlanets = value; }

    private string? rSAPublicKey;
    [AppliedWithChunk<Chunk0312C000>]
    public string? RSAPublicKey { get => rSAPublicKey; set => rSAPublicKey = value; }

    private string? rSAPrivateKey;
    [AppliedWithChunk<Chunk0312C000>]
    public string? RSAPrivateKey { get => rSAPrivateKey; set => rSAPrivateKey = value; }

    private int privacyPolicyVersion;
    [AppliedWithChunk<Chunk0312C000>]
    public int PrivacyPolicyVersion { get => privacyPolicyVersion; set => privacyPolicyVersion = value; }

    private int age;
    [AppliedWithChunk<Chunk0312C000>]
    public int Age { get => age; set => age = value; }

    private string? avatarName;
    [AppliedWithChunk<Chunk0312C001>]
    public string? AvatarName { get => avatarName; set => avatarName = value; }

    private SPlayerTagsConfig? playerTagsConfig;
    [AppliedWithChunk<Chunk0312C001>]
    public SPlayerTagsConfig? PlayerTagsConfig { get => playerTagsConfig; set => playerTagsConfig = value; }

    private string? trigram;
    [AppliedWithChunk<Chunk0312C001>]
    public string? Trigram { get => trigram; set => trigram = value; }

    private bool receiveNews;
    [AppliedWithChunk<Chunk0312C002>]
    public bool ReceiveNews { get => receiveNews; set => receiveNews = value; }

    private CGameBuddy[]? buddies;
    [AppliedWithChunk<Chunk0312C003>]
    public CGameBuddy[]? Buddies { get => buddies; set => buddies = value; }

    private byte flags2;
    [AppliedWithChunk<Chunk0312C006>]
    public byte Flags2 { get => flags2; set => flags2 = value; }

    private string? clubLinkUrl;
    [AppliedWithChunk<Chunk0312C008>]
    public string? ClubLinkUrl { get => clubLinkUrl; set => clubLinkUrl = value; }

    private byte fameStars;
    [AppliedWithChunk<Chunk0312C009>]
    public byte FameStars { get => fameStars; set => fameStars = value; }

    private YoutubeUpload[]? youtubeUploads;
    [AppliedWithChunk<Chunk0312C00C>]
    public YoutubeUpload[]? YoutubeUploads { get => youtubeUploads; set => youtubeUploads = value; }

    private int u01;
    public int U01 { get => u01; set => u01 = value; }

    private ulong u02;
    public ulong U02 { get => u02; set => u02 = value; }

    private int u03;
    public int U03 { get => u03; set => u03 = value; }

    private ulong u04;
    public ulong U04 { get => u04; set => u04 = value; }

    private CGameNetOnlineMessage[]? inboxMessages;
    public CGameNetOnlineMessage[]? InboxMessages { get => inboxMessages; set => inboxMessages = value; }

    private CGameNetOnlineMessage[]? readMessages;
    public CGameNetOnlineMessage[]? ReadMessages { get => readMessages; set => readMessages = value; }

    private CGameNetOnlineMessage[]? outboxMessages;
    public CGameNetOnlineMessage[]? OutboxMessages { get => outboxMessages; set => outboxMessages = value; }

    private string? u05;
    public string? U05 { get => u05; set => u05 = value; }

    private int u06;
    public int U06 { get => u06; set => u06 = value; }

    private int u07;
    public int U07 { get => u07; set => u07 = value; }

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        rw.String(ref description);
        rw.String(ref nickName);
        rw.Byte(ref flags);
        if ((Flags&1)!=0)
        {
            rw.String(ref onlineLogin);
            rw.String(ref onlinePassword);
            rw.String(ref onlineValidationKey);
            rw.String(ref onlineSupportKey);
            rw.String(ref lastUsedMSAddress);
            rw.String(ref lastUsedMSPath);
            rw.String(ref lastSessionId);
            rw.String(ref league);
            if (v <= 3)
            {
                rw.Int32(ref u01);
            }
            rw.Int32(ref onlineRemainingNickNamesChangesCount);
            rw.Int32(ref onlinePlanets);
            if (v >= 1)
            {
                rw.String(ref rSAPublicKey);
            }
        }
        rw.String(ref rSAPrivateKey);
        rw.UInt64(ref u02);
        rw.Int32(ref u03);
        rw.ArrayReadableWritable<CGameBuddy>(ref buddies!);
        rw.UInt64(ref u04);
        rw.ArrayNodeRef_deprec<CGameNetOnlineMessage>(ref inboxMessages!);
        rw.ArrayNodeRef_deprec<CGameNetOnlineMessage>(ref readMessages!);
        rw.ArrayNodeRef_deprec<CGameNetOnlineMessage>(ref outboxMessages!);
        rw.Byte(ref flags2);
        if (v >= 3)
        {
            rw.String(ref u05);
        }
        rw.String(ref avatarName);
        rw.ReadableWritable<SPlayerTagsConfig>(ref playerTagsConfig);
        rw.Boolean(ref receiveNews, asByte: true);
        if (v <= 1)
        {
            rw.Int32(ref u06);
        }
        if (v >= 5)
        {
            rw.Int32(ref eulaVersion);
            if (v >= 8)
            {
                rw.Int32(ref u07);
                rw.ArrayReadableWritable<CGameBuddy>(ref buddies!);
            }
        }
    }


    /// <summary>
    /// CGamePlayerProfileChunk_AccountSettings 0x000 skippable chunk
    /// </summary>
    [Chunk(0x0312C000)]
    public partial class Chunk0312C000 : SkippableChunk<CGamePlayerProfileChunk_AccountSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312C000;

        public int Version { get; set; }

        public ulong U01;
        public bool U02;

        public override void ReadWrite(CGamePlayerProfileChunk_AccountSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref n.eulaVersion);
            rw.String(ref n.nickName);
            rw.Byte(ref n.flags);
            if (n.LoginValidated)
            {
                rw.String(ref n.onlineLogin);
                rw.String(ref n.onlinePassword);
                rw.String(ref n.onlineValidationKey);
                rw.String(ref n.onlineSupportKey);
                rw.String(ref n.lastUsedMSAddress);
                rw.String(ref n.lastUsedMSPath);
                rw.String(ref n.lastSessionId);
                rw.String(ref n.league);
                rw.Int32(ref n.onlineRemainingNickNamesChangesCount);
                rw.Int32(ref n.onlinePlanets);
                rw.String(ref n.rSAPublicKey);
            }
            rw.String(ref n.rSAPrivateKey);
            rw.UInt64(ref U01);
            if (Version >= 2)
            {
                rw.Boolean(ref U02);
                if (Version >= 3)
                {
                    rw.Int32(ref n.privacyPolicyVersion);
                    rw.Int32(ref n.age);
                }
            }
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_AccountSettings 0x001 skippable chunk
    /// </summary>
    [Chunk(0x0312C001)]
    public partial class Chunk0312C001 : SkippableChunk<CGamePlayerProfileChunk_AccountSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312C001;

        public int Version { get; set; }


        public override void ReadWrite(CGamePlayerProfileChunk_AccountSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref n.description);
            rw.String(ref n.avatarName);
            rw.ReadableWritable<SPlayerTagsConfig>(ref n.playerTagsConfig);
            if (Version >= 2)
            {
                rw.String(ref n.trigram);
            }
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_AccountSettings 0x002 skippable chunk
    /// </summary>
    [Chunk(0x0312C002)]
    public partial class Chunk0312C002 : SkippableChunk<CGamePlayerProfileChunk_AccountSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312C002;

        public int Version { get; set; }


        public override void ReadWrite(CGamePlayerProfileChunk_AccountSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Boolean(ref n.receiveNews, asByte: true);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_AccountSettings 0x003 skippable chunk
    /// </summary>
    [Chunk(0x0312C003)]
    public partial class Chunk0312C003 : SkippableChunk<CGamePlayerProfileChunk_AccountSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312C003;

        public int Version { get; set; }

        public int U01;
        public string? U02;
        public string[]? U03;
        public byte[]? U04;
        public byte[]? U05;

        public override void ReadWrite(CGamePlayerProfileChunk_AccountSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.ArrayReadableWritable<CGameBuddy>(ref n.buddies!);
            if (Version >= 3)
            {
                rw.String(ref U02);
                if (Version == 4)
                {
                    rw.ArrayString(ref U03!);
                }
                if (Version >= 6)
                {
                    rw.Data(ref U04);
                    if (Version >= 7)
                    {
                        rw.Data(ref U05);
                    }
                }
            }
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_AccountSettings 0x004 skippable chunk
    /// </summary>
    [Chunk(0x0312C004)]
    public partial class Chunk0312C004 : SkippableChunk<CGamePlayerProfileChunk_AccountSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312C004;

        public int Version { get; set; }

        public string? U01;

        public override void ReadWrite(CGamePlayerProfileChunk_AccountSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_AccountSettings 0x005 skippable chunk
    /// </summary>
    [Chunk(0x0312C005)]
    public partial class Chunk0312C005 : SkippableChunk<CGamePlayerProfileChunk_AccountSettings>
    {
        /// <inheritdoc />
        public override uint Id => 0x0312C005;

    }

    /// <summary>
    /// CGamePlayerProfileChunk_AccountSettings 0x006 skippable chunk
    /// </summary>
    [Chunk(0x0312C006)]
    public partial class Chunk0312C006 : SkippableChunk<CGamePlayerProfileChunk_AccountSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312C006;

        public int Version { get; set; }


        public override void ReadWrite(CGamePlayerProfileChunk_AccountSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Byte(ref n.flags2);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_AccountSettings 0x007 skippable chunk
    /// </summary>
    [Chunk(0x0312C007)]
    public partial class Chunk0312C007 : SkippableChunk<CGamePlayerProfileChunk_AccountSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312C007;

        public int Version { get; set; }

        public string? U01;
        public int U02;
        public int U03;
        public int U04;
        public int U05;
        public int U06;
        public int U07;
        public bool U08;
        public float U09;
        public float U10;
        public string? U11;

        public override void ReadWrite(CGamePlayerProfileChunk_AccountSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
            rw.Int32(ref U07);
            rw.Boolean(ref U08);
            rw.Single(ref U09);
            rw.Single(ref U10);
            rw.String(ref U11);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_AccountSettings 0x008 skippable chunk
    /// </summary>
    [Chunk(0x0312C008)]
    public partial class Chunk0312C008 : SkippableChunk<CGamePlayerProfileChunk_AccountSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312C008;

        public int Version { get; set; }


        public override void ReadWrite(CGamePlayerProfileChunk_AccountSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref n.clubLinkUrl);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_AccountSettings 0x009 skippable chunk
    /// </summary>
    [Chunk(0x0312C009)]
    public partial class Chunk0312C009 : SkippableChunk<CGamePlayerProfileChunk_AccountSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312C009;

        public int Version { get; set; }


        public override void ReadWrite(CGamePlayerProfileChunk_AccountSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Byte(ref n.fameStars);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_AccountSettings 0x00A skippable chunk
    /// </summary>
    [Chunk(0x0312C00A)]
    public partial class Chunk0312C00A : SkippableChunk<CGamePlayerProfileChunk_AccountSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312C00A;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public bool U03;
        public float U04;
        public bool U05;
        public bool U06;
        public float U07;
        public float U08;
        public bool U09;
        public float U10;
        public float U11;
        public float U12;

        public override void ReadWrite(CGamePlayerProfileChunk_AccountSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            rw.Single(ref U02);
            if (Version >= 2)
            {
                rw.Boolean(ref U03);
            }
            rw.Single(ref U04);
            rw.Boolean(ref U05);
            rw.Boolean(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Boolean(ref U09);
            if (Version >= 3)
            {
                rw.Single(ref U10);
            }
            rw.Single(ref U11);
            rw.Single(ref U12);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_AccountSettings 0x00B skippable chunk
    /// </summary>
    [Chunk(0x0312C00B)]
    public partial class Chunk0312C00B : SkippableChunk<CGamePlayerProfileChunk_AccountSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312C00B;

        public int Version { get; set; }

        public int U01;
        public float U02;

        public override void ReadWrite(CGamePlayerProfileChunk_AccountSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            if (Version >= 1)
            {
                rw.Single(ref U02);
            }
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_AccountSettings 0x00C skippable chunk
    /// </summary>
    [Chunk(0x0312C00C)]
    public partial class Chunk0312C00C : SkippableChunk<CGamePlayerProfileChunk_AccountSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312C00C;

        public int Version { get; set; }

        public string? U01;

        public override void ReadWrite(CGamePlayerProfileChunk_AccountSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref U01);
            rw.ArrayReadableWritable<YoutubeUpload>(ref n.youtubeUploads!);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_AccountSettings 0x00D skippable chunk
    /// </summary>
    [Chunk(0x0312C00D)]
    public partial class Chunk0312C00D : SkippableChunk<CGamePlayerProfileChunk_AccountSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312C00D;

        public int Version { get; set; }

        public int U01;
        public int U02;

        public override void ReadWrite(CGamePlayerProfileChunk_AccountSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_AccountSettings 0x00E skippable chunk
    /// </summary>
    [Chunk(0x0312C00E)]
    public partial class Chunk0312C00E : SkippableChunk<CGamePlayerProfileChunk_AccountSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312C00E;

        public int Version { get; set; }

        public ulong U01;
        public bool U02;

        public override void ReadWrite(CGamePlayerProfileChunk_AccountSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.UInt64(ref U01);
            rw.Boolean(ref U02);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_AccountSettings 0x00F skippable chunk
    /// </summary>
    [Chunk(0x0312C00F)]
    public partial class Chunk0312C00F : SkippableChunk<CGamePlayerProfileChunk_AccountSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312C00F;

        public int Version { get; set; }

        public string? U01;

        public override void ReadWrite(CGamePlayerProfileChunk_AccountSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref U01);
        }
    }


    public sealed partial class SPlayerTagsConfig : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private PlayerTagConfig[]? playerTags;
        public PlayerTagConfig[]? PlayerTags { get => playerTags; set => playerTags = value; }

        private int[]? u02;
        public int[]? U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.ArrayReadableWritable<PlayerTagConfig>(ref playerTags!);
            rw.Array<int>(ref u02!);
        }
    }

    public sealed partial class YoutubeUpload : IReadableWritable
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



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0312C000 => new Chunk0312C000(),
        0x0312C001 => new Chunk0312C001(),
        0x0312C002 => new Chunk0312C002(),
        0x0312C003 => new Chunk0312C003(),
        0x0312C004 => new Chunk0312C004(),
        0x0312C005 => new Chunk0312C005(),
        0x0312C006 => new Chunk0312C006(),
        0x0312C007 => new Chunk0312C007(),
        0x0312C008 => new Chunk0312C008(),
        0x0312C009 => new Chunk0312C009(),
        0x0312C00A => new Chunk0312C00A(),
        0x0312C00B => new Chunk0312C00B(),
        0x0312C00C => new Chunk0312C00C(),
        0x0312C00D => new Chunk0312C00D(),
        0x0312C00E => new Chunk0312C00E(),
        0x0312C00F => new Chunk0312C00F(),
        _ => base.NewChunk(chunkId),
    };
}
