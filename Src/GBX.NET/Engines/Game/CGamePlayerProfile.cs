namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfile
{
    private string? description;
    public string? Description { get => description; set => description = value; }

    private UInt128 cryptedPassword;
    public UInt128 CryptedPassword { get => cryptedPassword; set => cryptedPassword = value; }

    private bool loginValidated;
    public bool LoginValidated { get => loginValidated; set => loginValidated = value; }

    private bool rememberOnlinePassword;
    public bool RememberOnlinePassword { get => rememberOnlinePassword; set => rememberOnlinePassword = value; }

    public bool AskForAccountConversion { get; set; }

    private string? onlinePassword;
    public string? OnlinePassword { get => onlinePassword; set => onlinePassword = value; }

    private string? onlineValidationCode;
    public string? OnlineValidationCode { get => onlineValidationCode; set => onlineValidationCode = value; }

    private string? lastUsedMSAddress;
    public string? LastUsedMSAddress { get => lastUsedMSAddress; set => lastUsedMSAddress = value; }
    private string? lastUsedMSPath;
    public string? LastUsedMSPath { get => lastUsedMSPath; set => lastUsedMSPath = value; }
    private string? lastSessionId;
    public string? LastSessionId { get => lastSessionId; set => lastSessionId = value; }

    private int? onlineRemainingNickNamesChangesCount;
    public int? OnlineRemainingNickNamesChangesCount { get => onlineRemainingNickNamesChangesCount; set => onlineRemainingNickNamesChangesCount = value; }

    private int? onlinePlanets;
    public int? OnlinePlanets { get => onlinePlanets; set => onlinePlanets = value; }

    private string? rsaPublicKey;
    public string? RSAPublicKey { get => rsaPublicKey; set => rsaPublicKey = value; }

    private string? rsaPrivateKey;
    public string? RSAPrivateKey { get => rsaPrivateKey; set => rsaPrivateKey = value; }

    private CGameNetOnlineMessage[]? inboxMessages;
    public CGameNetOnlineMessage[]? InboxMessages { get => inboxMessages; set => inboxMessages = value; }

    private CGameNetOnlineMessage[]? readMessages;
    public CGameNetOnlineMessage[]? ReadMessages { get => readMessages; set => readMessages = value; }

    private CGameNetOnlineMessage[]? outboxMessages;
    public CGameNetOnlineMessage[]? OutboxMessages { get => outboxMessages; set => outboxMessages = value; }

    public bool UnlockAllCheats { get; set; }
    public bool FriendsCheat { get; set; }

    public string? avatarName;
    public string? AvatarName { get => avatarName; set => avatarName = value; }

    private int? eulaVersion;
    public int? EulaVersion { get => eulaVersion; set => eulaVersion = value; }

    private PlayerTagsConfig? playerTagsConfig;
    public PlayerTagsConfig? PlayerTagsConfiguration { get => playerTagsConfig; set => playerTagsConfig = value; }

    private bool receiveNews;
    public bool ReceiveNews { get => receiveNews; set => receiveNews = value; }

    private ProfileChunk[]? profileChunks;
    public ProfileChunk[]? ProfileChunks { get => profileChunks; set => profileChunks = value; }

    private ProfileChunk[]? profileChunks2;
    public ProfileChunk[]? ProfileChunks2 { get => profileChunks2; set => profileChunks2 = value; }

    public partial class Chunk0308C068
    {
        public Dictionary<string, int>? U01;

        public override void Read(CGamePlayerProfile n, GbxReader r)
        {
            var count = r.ReadInt32();
            U01 = new(count);

            for (var i = 0; i < count; i++)
            {
                U01.Add(r.ReadIdAsString(), r.ReadInt32());
            }
        }

        public override void Write(CGamePlayerProfile n, GbxWriter w)
        {
            if (U01 is null)
            {
                w.Write(0);
                return;
            }

            w.Write(U01.Count);

            foreach (var pair in U01)
            {
                w.WriteIdAsString(pair.Key);
                w.Write(pair.Value);
            }
        }
    }

    public partial class Chunk0308C069
    {
        public string? U01;
        public int U02;
        public int U03;
        public string? U04;
        public int U05;
        public byte[]? U06;
        public string? U07;
        public string? U08;
        public bool U09;
        public bool U10;
        public string? U11;
        public string? U12;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
            rw.String(ref n.description);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.String(ref n.onlineLogin);
            rw.Int32(ref U05);

            if (U05 != 0)
            {
                rw.UInt128(ref n.cryptedPassword);
                rw.Data(ref U06, U05);
            }

            rw.String(ref U07);
            rw.String(ref U08);
            rw.Boolean(ref n.loginValidated);
            rw.Boolean(ref n.rememberOnlinePassword);
            rw.String(ref U11);
            rw.String(ref U12);
        }
    }

    public partial class Chunk0308C07C
    {
        public string? U01;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Id(ref U01);
            rw.String(ref n.profileName);

            if (rw.Reader is GbxReader r)
            {
                n.profileChunks = new ProfileChunk[r.ReadInt32()];

                for (var i = 0; i < n.profileChunks.Length; i++)
                {
                    var chunkId = r.ReadUInt32();
                    var u01 = r.ReadString();
                    var u02 = r.ReadString();
                    var u03 = r.ReadString();
                    var u04 = r.ReadInt32();
                    var archiveVersion = r.ReadInt32();

                    ProfileChunk? chunk = chunkId switch
                    {
                        0x0312C000 => new AccountSettings(), // CGamePlayerProfileChunk_AccountSettings::ArchiveOldVersion
                        _ => throw new NotImplementedException($"ProfileChunk 0x{chunkId:X8} is not implemented."),
                    };

                    if (chunk is not null)
                    {
                        chunk.ReadWrite(n, rw, archiveVersion);
                        n.profileChunks[i] = chunk;
                    }
                }
            }
        }
    }

    public partial class Chunk0308C07E : IVersionable
    {
        public string? U01;

        public int Version { get; set; }

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref U01);
            rw.String(ref n.profileName);

            if (rw.Reader is GbxReader r)
            {
                n.profileChunks2 = new ProfileChunk[r.ReadInt32()];

                for (var i = 0; i < n.profileChunks2.Length; i++)
                {
                    var chunkId = r.ReadUInt32();
                    var u01 = r.ReadString();
                    var u02 = r.ReadString();
                    var u03 = r.ReadString();
                    var u04 = r.ReadInt32();

                    if (Version >= 2)
                    {
                        var u05 = r.ReadInt32();
                    }

                    var chunkData = r.ReadData();
                }
            }
        }
    }

    public abstract class ProfileChunk
    {
        public abstract void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw, int version);
    }

    public partial class AccountSettings : ProfileChunk
    {
        public int U01;
        public string League = "";
        public ulong U02;
        public int U03;
        public ulong U04;
        public string? U05;
        public int U06;
        public int U07;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw, int version)
        {
            rw.String(ref n.description);
            rw.String(ref n.nickName);

            if (rw.Reader is not null)
            {
                var flags = rw.Reader.ReadByte();
                n.loginValidated = (flags & 1) != 0;
                n.rememberOnlinePassword = (flags & 2) != 0;
                n.autoConnect = (flags & 4) != 0;
                n.AskForAccountConversion = (flags & 8) != 0;
            }

            if (rw.Writer is not null)
            {
                byte flags = 0;
                if (n.loginValidated) flags |= 1;
                if (n.rememberOnlinePassword) flags |= 2;
                if (n.autoConnect) flags |= 4;
                if (n.AskForAccountConversion) flags |= 8;
                rw.Writer.Write(flags);
            }

            rw.String(ref n.onlineLogin);
            rw.String(ref n.onlinePassword);
            rw.String(ref n.onlineValidationCode);
            rw.String(ref n.onlineSupportKey);
            rw.String(ref n.lastUsedMSAddress);
            rw.String(ref n.lastUsedMSPath);
            rw.String(ref n.lastSessionId);
            rw.String(ref League);

            if (version < 4)
            {
                rw.Int32(ref U01);
            }

            rw.Int32(ref n.onlineRemainingNickNamesChangesCount);
            rw.Int32(ref n.onlinePlanets);

            if (version >= 1)
            {
                rw.String(ref n.rsaPublicKey);
            }

            rw.String(ref n.rsaPrivateKey);
            rw.UInt64(ref U02); // SSystemTime
            rw.Int32(ref U03);
            rw.ArrayReadableWritable<CGameBuddy>(ref n.buddies);
            rw.UInt64(ref U04); // SSystemTime
            rw.ArrayNodeRef_deprec<CGameNetOnlineMessage>(ref n.inboxMessages);
            rw.ArrayNodeRef_deprec<CGameNetOnlineMessage>(ref n.readMessages);
            rw.ArrayNodeRef_deprec<CGameNetOnlineMessage>(ref n.outboxMessages);

            if (rw.Reader is not null)
            {
                var flags = rw.Reader.ReadByte();
                n.UnlockAllCheats = (flags & 1) != 0;
                n.FriendsCheat = (flags & 2) != 0;
            }

            if (rw.Writer is not null)
            {
                byte flags = 0;
                if (n.UnlockAllCheats) flags |= 1;
                if (n.FriendsCheat) flags |= 2;
                rw.Writer.Write(flags);
            }

            if (version >= 3)
            {
                rw.String(ref U05);
            }

            rw.String(ref n.avatarName);
            rw.ReadableWritable<PlayerTagsConfig>(ref n.playerTagsConfig);
            rw.Boolean(ref n.receiveNews, asByte: true);

            if (version < 2)
            {
                rw.Int32(ref U06);
            }

            if (version >= 5)
            {
                rw.Int32(ref n.eulaVersion);

                if (version >= 8)
                {
                    rw.Int32(ref U07);
                    rw.ArrayReadableWritable<CGameBuddy>(ref n.buddies);
                }
            }
        }
    }
}
