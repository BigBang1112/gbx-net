using GBX.NET.Managers;

namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfile
{
    private string? lastUsedMSAddress;
    public partial string? LastUsedMSAddress { get => lastUsedMSAddress; set => lastUsedMSAddress = value; }
    private string? lastUsedMSPath;
    public partial string? LastUsedMSPath { get => lastUsedMSPath; set => lastUsedMSPath = value; }
    private int? onlineRemainingNickNamesChangesCount = -1;
    public partial int? OnlineRemainingNickNamesChangesCount { get => onlineRemainingNickNamesChangesCount; set => onlineRemainingNickNamesChangesCount = value; }

    private CGamePlayerProfileChunk[]? oldProfileChunks;
    [AppliedWithChunk<Chunk0308C07C>]
    public CGamePlayerProfileChunk[]? OldProfileChunks { get => oldProfileChunks; set => oldProfileChunks = value; }

    private CGamePlayerProfileChunk[]? profileChunks;
    [AppliedWithChunk<Chunk0308C07D>]
    [AppliedWithChunk<Chunk0308C07E>]
    public CGamePlayerProfileChunk[]? ProfileChunks { get => profileChunks; set => profileChunks = value; }

    public partial class VehicleProfile
    {
        [Obsolete("Use VehicleIdent instead.")]
        public Ident? U01 { get => VehicleIdent; set => VehicleIdent = value; }

        [Obsolete("Use SkinName instead.")]
        public string? U02 { get => SkinName; set => SkinName = value; }

        [Obsolete("Use SkinChecksum instead.")]
        public UInt128 U03 { get => SkinChecksum; set => SkinChecksum = value; }

        [Obsolete("Use GameCamera instead.")]
        public int U04 { get => GameCamera; set => GameCamera = value; }

        [Obsolete("Use AnalogSensitivity instead.")]
        public float U05 { get => AnalogSensitivity; set => AnalogSensitivity = value; }

        [Obsolete("Use AnalogDeadZone instead.")]
        public float U06 { get => AnalogDeadZone; set => AnalogDeadZone = value; }
    }

    public partial class Chunk0308C000
    {
        public CGameCtnCollectorList? U02;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.String(ref n.profileName);
            rw.String(ref n.nickName);
            rw.Id(ref n.profileId);
            U02 ??= new CGameCtnCollectorList();
            U02.ReadWrite(rw);
        }
    }

    public partial class Chunk0308C064
    {
        [Obsolete("Use Version instead.")]
        public int U01 { get => Version; set => Version = value; }
    }

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

    public partial class Chunk0308C07C
    {
        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.Id(ref n.profileId);
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

                    var versions = GetProfileArchiveVersions(chunkId)!.Value;
                    var payloadVersion = archiveVersion;
                    if (payloadVersion > versions.Beta)
                    {
                        chunk.SkipArchiveVersion = payloadVersion;
                        archiveVersion = r.ReadInt32();
                    }

                    if (payloadVersion < versions.Nod)
                    {
                        ((IReadableWritable)chunk).ReadWrite(rw, payloadVersion);
                    }
                    else
                    {
                        chunk.ReadWrite(rw);
                    }
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
                    var classId = ClassManager.GetId(chunk.GetType()) ?? throw new InvalidOperationException($"Type {chunk.GetType()} is not registered in ClassManager.");
                    var versions = GetProfileArchiveVersions(classId)
                        ?? throw new NotSupportedException($"Archive versions for profile chunk 0x{classId:X8} are unknown.");
                    w.Write(classId);
                    w.Write(chunk.ChunkName);
                    w.Write(chunk.GameName);
                    w.Write(chunk.Checksum);
                    w.WriteUnixTime(chunk.LastUpdatedAt);
                    var payloadVersion = chunk.SkipArchiveVersion ?? chunk.ArchiveVersion;
                    w.Write(payloadVersion);
                    if (payloadVersion > versions.Beta)
                    {
                        w.Write(chunk.ArchiveVersion);
                    }

                    if (payloadVersion < versions.Nod)
                    {
                        if (chunk is not IReadableWritable readableWritable)
                        {
                            throw new NotSupportedException($"Old archive version {payloadVersion} of profile chunk 0x{classId:X8} is not implemented.");
                        }

                        readableWritable.ReadWrite(rw, payloadVersion);
                    }
                    else
                    {
                        chunk.ReadWrite(rw);
                    }
                }
            }
        }
    }

    public partial class Chunk0308C07D : IVersionable
    {
        public int Version { get; set; } = 1;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref n.profileId);
            rw.String(ref n.profileName);
            n.ReadWriteProfileChunks(rw, hasCreationTime: false);
        }
    }

    public partial class Chunk0308C07E : IVersionable
    {
        public int Version { get; set; } = 2;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref n.profileId);
            rw.String(ref n.profileName);
            n.ReadWriteProfileChunks(rw, hasCreationTime: Version >= 2);
        }
    }

    private static (int Beta, int Nod)? GetProfileArchiveVersions(uint classId) => classId switch
    {
        0x0312C000 => (5, 9), // AccountSettings
        0x0312D000 => (5, 10), // GameSettings
        0x0312E000 => (2, 4), // InterfaceSettings
        0x0312F000 => (0, 2), // InputBindingsConfig
        0x03130000 => (4, 9), // VehiclesSettings
        0x03140000 => (3, 5), // PackagesInfos
        0x03146000 => (1, 4), // GameScores
        0x03147000 => (0, 2), // GameStats
        0x03148000 => (0, 3), // ChallengesStats
        0x03149000 => (1, 4), // ChallengesScores
        0x03163000 => (0, 2), // EditorSettings
        0x03170000 => (0, 2), // ScriptPersistentTraits
        0x03179000 => (0, 2), // GlobalInterfaceSettings
        0x03180000 => (0, 2), // ManiaPlanetStations
        _ => null,
    };

    private void ReadWriteProfileChunks(GbxReaderWriter rw, bool hasCreationTime)
    {
        if (rw.Reader is GbxReader r)
        {
            profileChunks = new CGamePlayerProfileChunk[r.ReadInt32()];

            for (var i = 0; i < profileChunks.Length; i++)
            {
                var chunkClassId = r.ReadUInt32();
                var chunkName = r.ReadString();
                var gameName = r.ReadString();
                var checksum = r.ReadString();
                var lastUpdated = r.ReadUnixTime();
                var createdAt = hasCreationTime ? r.ReadUnixTime() : default(DateTimeOffset?);
                if (createdAt == DateTimeOffset.FromUnixTimeSeconds(0)) createdAt = null;

                var chunkData = r.ReadData();
                var versions = GetProfileArchiveVersions(chunkClassId);
                var chunk = chunkData.Length == 0 || versions is null ? null : ClassManager.New(chunkClassId) as CGamePlayerProfileChunk;

                if (chunk is null)
                {
                    chunk = new CGamePlayerProfileChunk_Unknown(chunkClassId, chunkData);
                }
                else
                {
                    using var ms = new MemoryStream(chunkData, writable: false);
                    using var chunkReader = new GbxReader(ms, r.Settings);
                    using var chunkRw = new GbxReaderWriter(chunkReader);
                    using var _ = r.Logger?.BeginScope(ClassManager.GetName(chunkClassId) ?? $"0x{chunkClassId:X8}");

                    var archiveVersion = chunkReader.ReadInt32();
                    chunk.ArchiveVersion = archiveVersion;

                    // Beta archives have a single version. Later archives add a
                    // compatibility version before either an old archive or a node.
                    if (archiveVersion > versions!.Value.Beta)
                    {
                        chunk.SkipArchiveVersion = archiveVersion;
                        chunk.ArchiveVersion = chunkReader.ReadInt32();
                    }

                    if (archiveVersion < versions.Value.Nod)
                    {
                        if (chunk is IReadableWritable readableWritable)
                        {
                            readableWritable.ReadWrite(chunkRw, archiveVersion);
                        }
                        else
                        {
                            chunk = new CGamePlayerProfileChunk_Unknown(chunkClassId, chunkData);
                        }
                    }
                    else
                    {
                        chunk.ReadWrite(chunkRw);
                    }

                    if (chunk is not CGamePlayerProfileChunk_Unknown && ms.Position != ms.Length)
                    {
                        throw new InvalidDataException($"Not all data was read from profile chunk 0x{chunkClassId:X8} ({ClassManager.GetName(chunkClassId)}). {ms.Length - ms.Position} bytes remaining.");
                    }
                }

                profileChunks[i] = chunk;
                chunk.ChunkName = chunkName;
                chunk.GameName = gameName;
                chunk.Checksum = checksum;
                chunk.LastUpdatedAt = lastUpdated;
                chunk.CreatedAt = createdAt;
            }
        }

        if (rw.Writer is GbxWriter w)
        {
            w.Write(profileChunks?.Length ?? 0);

            foreach (var chunk in profileChunks ?? [])
            {
                var chunkClassId = chunk is CGamePlayerProfileChunk_Unknown unknown ? unknown.ClassId
                    : ClassManager.GetId(chunk.GetType()) ?? throw new InvalidOperationException($"Type {chunk.GetType()} is not registered in ClassManager.");
                w.Write(chunkClassId);
                w.Write(chunk.ChunkName);
                w.Write(chunk.GameName);
                w.Write(chunk.Checksum);
                w.WriteUnixTime(chunk.LastUpdatedAt);

                if (hasCreationTime)
                {
                    w.WriteUnixTime(chunk.CreatedAt ?? DateTimeOffset.FromUnixTimeSeconds(0));
                }

                if (chunk is CGamePlayerProfileChunk_Unknown raw)
                {
                    w.WriteData(raw.Data);
                    continue;
                }

                using var ms = new MemoryStream();
                using var chunkWriter = new GbxWriter(ms, w.Settings);
                using var chunkRw = new GbxReaderWriter(chunkWriter);
                var versions = GetProfileArchiveVersions(chunkClassId)
                    ?? throw new NotSupportedException($"Archive versions for profile chunk 0x{chunkClassId:X8} are unknown.");
                var archiveVersion = chunk.SkipArchiveVersion ?? chunk.ArchiveVersion;

                if (chunk.SkipArchiveVersion is null && archiveVersion == 0
                    && (chunk is not IReadableWritable || chunk.Chunks.Count > 0))
                {
                    archiveVersion = versions.Nod;
                }

                chunkWriter.Write(archiveVersion);
                if (archiveVersion > versions.Beta)
                {
                    chunkWriter.Write(chunk.SkipArchiveVersion is null ? archiveVersion : chunk.ArchiveVersion);
                }

                if (archiveVersion < versions.Nod)
                {
                    if (chunk is not IReadableWritable readableWritable)
                    {
                        throw new NotSupportedException($"Old archive version {archiveVersion} of profile chunk 0x{chunkClassId:X8} is not implemented.");
                    }

                    readableWritable.ReadWrite(chunkRw, archiveVersion);
                }
                else
                {
                    chunk.ReadWrite(chunkRw);
                }
                w.WriteData(ms.ToArray());
            }
        }
    }
}
