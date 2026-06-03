using GBX.NET.Managers;

namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfile
{
    private string? description;
    [SupportsFormatting]
    public string? Description { get => description; set => description = value; }

    private string? nickName;
    [SupportsFormatting]
    [AppliedWithChunk<Chunk0308C05B>]
    public string? NickName { get => nickName; set => nickName = value; }

    private Checksum128 cryptedPassword;
    public Checksum128 CryptedPassword { get => cryptedPassword; set => cryptedPassword = value; }

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
                rw.Checksum128(ref n.cryptedPassword);
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
                    var chunkName = r.ReadString();
                    var gameName = r.ReadString();
                    var checksum = r.ReadString();
                    var lastUpdated = r.ReadUnixTime();
                    var archiveVersion = r.ReadInt32();

                    CGamePlayerProfileChunk chunk = chunkId switch
                    {
                        0x0312C000 => new CGamePlayerProfileChunk_AccountSettings(), // CGamePlayerProfileChunk_AccountSettings::ArchiveOldVersion
                        _ => throw new NotImplementedException($"ProfileChunk 0x{chunkId:X8} is not implemented."),
                    };

                    if (chunk is not IReadableWritable readableWritable)
                    {
                        throw new InvalidOperationException($"Profile chunk of type {chunk.GetType()} does not implement IReadableWritable, which is required for old archives.");
                    }

                    readableWritable.ReadWrite(rw, archiveVersion);
                    n.oldProfileChunks[i] = chunk;

                    chunk.ChunkName = chunkName;
                    chunk.GameName = gameName;
                    chunk.Checksum = checksum;
                    chunk.LastUpdatedAt = lastUpdated;
                    chunk.ArchiveVersion = archiveVersion;
                }
            }

            if (rw.Writer is GbxWriter w)
            {
                w.Write(n.oldProfileChunks?.Length ?? 0);

                foreach (var chunk in n.oldProfileChunks ?? [])
                {
                    if (chunk is not IReadableWritable readableWritable)
                    {
                        throw new InvalidOperationException($"Profile chunk of type {chunk.GetType()} does not implement IReadableWritable, which is required for old archives.");
                    }

                    w.Write(ClassManager.GetId(chunk.GetType()) ?? throw new InvalidOperationException($"Type {chunk.GetType()} is not registered in ClassManager."));
                    w.Write(chunk.ChunkName);
                    w.Write(chunk.GameName);
                    w.Write(chunk.Checksum);
                    w.WriteUnixTime(chunk.LastUpdatedAt);
                    w.Write(chunk.ArchiveVersion);

                    readableWritable.ReadWrite(rw, chunk.ArchiveVersion);
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
                    var chunkClassId = r.ReadUInt32();
                    var chunkName = r.ReadString();
                    var gameName = r.ReadString();
                    var checksum = r.ReadString();
                    var lastUpdated = r.ReadUnixTime();
                    var createdAt = Version >= 2 ? r.ReadUnixTime() : default(DateTimeOffset?);
                    if (createdAt == DateTimeOffset.FromUnixTimeSeconds(0)) createdAt = null;

                    var chunkDataSize = r.ReadInt32();
                    using var boundedStream = new BoundedStream(r.BaseStream, chunkDataSize);
                    using var chunkReader = new GbxReader(boundedStream, r.Settings);
                    using var chunkRw = new GbxReaderWriter(chunkReader);

                    using var _ = r.Logger?.BeginScope(ClassManager.GetName(chunkClassId) ?? $"0x{chunkClassId:X8}");

                    var skipArchiveVersion = chunkReader.ReadInt32();
                    var archiveVersion = chunkReader.ReadInt32();

                    var chunk = (CGamePlayerProfileChunk)(ClassManager.New(chunkClassId)
                        ?? throw new NotImplementedException($"Profile chunk 0x{chunkClassId:X8} ({ClassManager.GetName(chunkClassId)}) is not implemented."));

                    n.profileChunks[i] = chunk;

                    chunk.ChunkName = chunkName;
                    chunk.GameName = gameName;
                    chunk.Checksum = checksum;
                    chunk.LastUpdatedAt = lastUpdated;
                    chunk.CreatedAt = createdAt;
                    chunk.SkipArchiveVersion = skipArchiveVersion;
                    chunk.ArchiveVersion = archiveVersion;

                    chunk.ReadWrite(chunkRw);

                    if (boundedStream.Remaining > 0)
                    {
                        throw new Exception($"Not all data was read from profile chunk 0x{chunkClassId:X8} ({ClassManager.GetName(chunkClassId)}). {boundedStream.Remaining} bytes remaining.");
                    }
                }
            }

            if (rw.Writer is GbxWriter w)
            {
                w.Write(n.profileChunks?.Length ?? 0);

                foreach (var chunk in n.profileChunks ?? [])
                {
                    w.Write(ClassManager.GetId(chunk.GetType()) ?? throw new InvalidOperationException($"Type {chunk.GetType()} is not registered in ClassManager."));
                    w.Write(chunk.ChunkName);
                    w.Write(chunk.GameName);
                    w.Write(chunk.Checksum);
                    w.WriteUnixTime(chunk.LastUpdatedAt);

                    if (Version >= 2)
                    {
                        w.WriteUnixTime(chunk.CreatedAt ?? DateTimeOffset.FromUnixTimeSeconds(0));
                    }

                    using var ms = new MemoryStream();
                    using var chunkWriter = new GbxWriter(ms, w.Settings);
                    using var chunkRw = new GbxReaderWriter(chunkWriter);
                    chunkWriter.Write(chunk.SkipArchiveVersion.GetValueOrDefault());
                    chunkWriter.Write(chunk.ArchiveVersion);
                    chunk.ReadWrite(chunkRw);

                    var chunkData = ms.ToArray();
                    w.WriteData(chunkData);
                }
            }
        }
    }
}
