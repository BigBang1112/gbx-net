using GBX.NET.Managers;

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

    private CGamePlayerProfileChunk[]? oldProfileChunks;
    public CGamePlayerProfileChunk[]? OldProfileChunks { get => oldProfileChunks; set => oldProfileChunks = value; }

    private CGamePlayerProfileChunk[]? profileChunks;
    public CGamePlayerProfileChunk[]? ProfileChunks { get => profileChunks; set => profileChunks = value; }

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
                n.oldProfileChunks = new CGamePlayerProfileChunk[r.ReadInt32()];

                for (var i = 0; i < n.oldProfileChunks.Length; i++)
                {
                    var chunkId = r.ReadUInt32();
                    var u01 = r.ReadString();
                    var u02 = r.ReadString();
                    var u03 = r.ReadString();
                    var u04 = r.ReadInt32();
                    var archiveVersion = r.ReadInt32();

                    switch (chunkId)
                    {
                        case 0x0312C000:
                            // CGamePlayerProfileChunk_AccountSettings::ArchiveOldVersion
                            var chunk = new CGamePlayerProfileChunk_AccountSettings();
                            chunk.ReadWrite(rw, archiveVersion);
                            n.oldProfileChunks[i] = chunk;
                            break;
                        default:
                            throw new NotImplementedException($"ProfileChunk 0x{chunkId:X8} is not implemented.");
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
                n.profileChunks = new CGamePlayerProfileChunk[r.ReadInt32()];

                for (var i = 0; i < n.profileChunks.Length; i++)
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
                    using var ms = new MemoryStream(chunkData);
                    using var chunkReader = new GbxReader(ms, r.Settings);
                    using var chunkRw = new GbxReaderWriter(chunkReader);

                    var u06 = chunkReader.ReadInt32();
                    var u07 = chunkReader.ReadInt32();

                    var chunk = (CGamePlayerProfileChunk?)ClassManager.New(chunkId)
                        ?? throw new NotImplementedException($"Profile chunk 0x{chunkId:X8} ({ClassManager.GetName(chunkId)}) is not implemented.");

                    if (chunk is not null)
                    {
                        chunk.ReadWrite(chunkRw);
                        n.profileChunks[i] = chunk;
                    }
                }
            }
        }
    }
}
