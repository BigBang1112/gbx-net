using GBX.NET.Engines.Game;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;
using System.Net;
using TmEssentials;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnGhostTests
{
    [Test]
    [Arguments(0x11, 0, 2, 0)]
    [Arguments(0x19, 0, 3, 2)]
    [Arguments(0x25, 0, 5, 14)]
    [Arguments(0x25, 1, 6, 17)]
    [Arguments(0x11, 0, -1, -1)]
    [Arguments(0x19, 0, int.MaxValue, int.MaxValue)]
    [Arguments(0x25, 0, -1, 9)]
    [Arguments(0x25, 1, 2, 13)]
    [Arguments(0x25, 1, 2, 15)]
    [Arguments(0x25, 1, int.MaxValue, 20)]
    public async Task ReadWrite_ValidationInputStore_ReadsNodeMembersAndWritesWithFreshChunks(int offset, int version, int cpuKind, int osKind)
    {
        var payload = Payload(w =>
        {
            if (offset == 0x25) w.Write(version);
            w.Write(1200); // Events duration.
            w.Write(7); // Input store version, preserved without interpretation.
            w.Write(0); // Input name count.
            w.Write(0); // Event count.
            w.Write(456); // Input count limit.
            w.Write("Executable");
            w.Write(123u);
            w.Write(osKind);
            w.Write(cpuKind);
            w.Write("Settings");
            if (offset != 0x11) w.Write(9); // Validation seed.
            if (offset == 0x25) w.Write(true);
        });
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        var ghost = new CGameCtnGhost();
        ValidationInputChunk(offset, version).ReadWrite(ghost, rw);

        await Assert.That(ghost.InputStoreVersion).IsEqualTo(7);
        await Assert.That(ghost.InputCountLimit).IsEqualTo(456);
        await Assert.That(ghost.Validate_OsKind).IsEqualTo((CGameCtnGhost.EKindOs)osKind);
        await Assert.That(ghost.Validate_CpuKind).IsEqualTo((CGameCtnGhost.EKindCpu)cpuKind);
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);
#pragma warning disable GBXNET10001
        var clone = (CGameCtnGhost)ghost.DeepClone();
#pragma warning restore GBXNET10001
        await Assert.That(clone.InputStoreVersion).IsEqualTo(7);
        await Assert.That(clone.InputCountLimit).IsEqualTo(456);
        await Assert.That(clone.Validate_OsKind).IsEqualTo(ghost.Validate_OsKind);
        await Assert.That(clone.Validate_CpuKind).IsEqualTo(ghost.Validate_CpuKind);
        var rewritten = Payload(w =>
        {
            using var writeRw = new GbxReaderWriter(w);
            ValidationInputChunk(offset, version).ReadWrite(clone, writeRw);
        });
        await Assert.That(rewritten.SequenceEqual(payload)).IsTrue();
    }

    [Test]
    public async Task ReadWrite_ValidationInputFlag_RemainsOnTheNodeWhenSeparateInputsAreUnsupported()
    {
        using var stream = new MemoryStream(Payload(w => w.Write(1)));
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        var ghost = new CGameCtnGhost();

        await Assert.That(() => new CGameCtnGhost.Chunk0309202D().ReadWrite(ghost, rw)).Throws<Exception>();
        await Assert.That(ghost.HasInputs).IsEqualTo(1);
    }

    [Test]
    [Arguments(-1, 0)]
    [Arguments(0, 1)]
    [Arguments(1, 2)]
    [Arguments(1, int.MinValue)]
    [Arguments(1, int.MaxValue)]
    public async Task Appearance_ReadWrite_PreservesCustomizationAndIntegerField(int appearanceVersion, int unknown)
    {
        const string customization = "Skin&Team=Team01&Medal=Gold&Level=3";
        var payload = Payload(w =>
        {
            w.Write(9);
            w.Write(appearanceVersion);
            w.Write((Ident?)null);
            w.Write(default(Vec3));
            w.Write(0); // skin pack count
            w.Write(false); // no badge
            if (appearanceVersion >= 1) w.Write(customization);
            w.Write("Nickname");
            w.Write("Avatar");
            w.Write("Context");
            w.Write(unknown);
            w.Write(-1); // no record-data node
            w.WriteArray(new[] { 3, 5 });
            w.Write("ABC");
            w.Write("World|Europe");
            w.Write("Club");
        });
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        var ghost = new CGameCtnGhost();
        var appearanceChunk = new CGameCtnGhost.Chunk03092000();
        appearanceChunk.ReadWrite(ghost, rw);

        await Assert.That(ghost.SkinCustomization).IsEqualTo(appearanceVersion >= 1 ? customization : null);
        await Assert.That(appearanceChunk.U01).IsEqualTo(unknown);
        await Assert.That(appearanceChunk.U02!.SequenceEqual(new[] { 3, 5 })).IsTrue();
        await Assert.That(ghost.GhostNickname).IsEqualTo("Nickname");
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);

        var rewritten = Payload(w =>
        {
            using var writerWriter = new GbxReaderWriter(w);
            appearanceChunk.ReadWrite(ghost, writerWriter);
        });
        await Assert.That(rewritten.SequenceEqual(payload)).IsTrue();

        var customizationPayload = Payload(w => w.Write(customization));
        var fromAppearance = Payload(w =>
        {
            using var writerWriter = new GbxReaderWriter(w);
            new CGameCtnGhost.Chunk0309202E().ReadWrite(ghost, writerWriter);
        });
        var expectedCustomizationPayload = appearanceVersion >= 1 ? customizationPayload : Payload(w => w.Write(""));
        await Assert.That(fromAppearance.SequenceEqual(expectedCustomizationPayload)).IsTrue();

        using var customizationStream = new MemoryStream(customizationPayload);
        using var customizationReader = new GbxReader(customizationStream);
        using var customizationRw = new GbxReaderWriter(customizationReader);
        new CGameCtnGhost.Chunk0309202E().ReadWrite(ghost, customizationRw);
        await Assert.That(ghost.SkinCustomization).IsEqualTo(customization);
        await Assert.That(customizationStream.Position).IsEqualTo((long)customizationPayload.Length);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadWrite_GhostUid_PreservesStringAndNumericIds(bool numeric)
    {
        var uid = numeric ? new Id(26) : new Id("GhostUid");
        var payload = Payload(w => w.Write(uid));
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        var ghost = new CGameCtnGhost();
        var chunk = new CGameCtnGhost.Chunk0309200E();

        chunk.ReadWrite(ghost, rw);

        await Assert.That(ghost.GhostUid).IsEqualTo(uid);
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);
        var rewritten = Payload(w =>
        {
            using var writerWriter = new GbxReaderWriter(w);
            chunk.ReadWrite(ghost, writerWriter);
        });
        await Assert.That(rewritten.SequenceEqual(payload)).IsTrue();
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task ReadWrite_RecordContext_ReadsScopeAndGameMode(int scope)
    {
        var payload = Payload(w =>
        {
            w.Write(1);
            w.Write(scope);
            w.Write("SeasonId");
            w.Write("TrackMania/TM_Race");
            w.Write("CustomData");
        });
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        var ghost = new CGameCtnGhost();
        new CGameCtnGhost.Chunk03092029().ReadWrite(ghost, rw);

        await Assert.That((int)ghost.Validate_ScopeType).IsEqualTo(scope);
        await Assert.That(ghost.Validate_ScopeId).IsEqualTo("SeasonId");
        await Assert.That(ghost.Validate_GameMode).IsEqualTo("TrackMania/TM_Race");
        await Assert.That(ghost.Validate_GameModeCustomData).IsEqualTo("CustomData");
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);
    }

    [Test]
    [Arguments(-1, 5, 14)]
    [Arguments(120100, 6, 17)]
    [Arguments(-1, -1, -1)]
    [Arguments(120100, int.MaxValue, int.MaxValue)]
    public async Task ReadWrite_ValidationSettings_ReadsPackedRulesAndSimulationTime(int startTime, int cpuKind, int osKind)
    {
        const int gameRules = unchecked((int)0xFEDCBA98);
        var payload = Payload(w =>
        {
            w.Write(gameRules);
            w.Write(startTime);
        });
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        var ghost = new CGameCtnGhost();
        new CGameCtnGhost.Chunk0309202A().ReadWrite(ghost, rw);

        await Assert.That(ghost.Validate_GameRules).IsEqualTo(gameRules);
        await Assert.That(ghost.Validate_RaceStartTime?.TotalMilliseconds).IsEqualTo(startTime == -1 ? null : (int?)startTime);
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);

        var rewritten = Payload(w =>
        {
            using var writerWriter = new GbxReaderWriter(w);
            new CGameCtnGhost.Chunk0309202A().ReadWrite(ghost, writerWriter);
        });
        await Assert.That(rewritten.SequenceEqual(payload)).IsTrue();

        var validationPayload = Payload(w =>
        {
            w.Write(0); // no input store
            w.Write(""); // executable version
            w.Write(0u); // executable checksum
            w.Write(osKind);
            w.Write(cpuKind);
            w.Write(-1); // walltime start
            w.Write(-1); // walltime end
            w.Write(""); // title ID
            w.Write(new byte[32]); // title checksum
            w.Write(gameRules);
            w.Write(startTime);
            w.Write(-1); // validation seed
            w.Write(0x12345679); // simulation flags, including bits beyond steering sensitivity
            w.Write(""); // race settings
        });
        using var validationStream = new MemoryStream(validationPayload);
        using var validationReader = new GbxReader(validationStream);
        using var validationRw = new GbxReaderWriter(validationReader);
        var validationGhost = new CGameCtnGhost();
        var validationChunk = new CGameCtnGhost.Chunk0309202D();
        validationChunk.ReadWrite(validationGhost, validationRw);
        await Assert.That(validationGhost.Validate_GameRules).IsEqualTo(ghost.Validate_GameRules);
        await Assert.That(validationGhost.Validate_RaceStartTime).IsEqualTo(ghost.Validate_RaceStartTime);
        await Assert.That(validationGhost.HasInputs).IsEqualTo(0);
        await Assert.That(validationGhost.Validate_OsKind).IsEqualTo((CGameCtnGhost.EKindOs)osKind);
        await Assert.That(validationGhost.Validate_CpuKind).IsEqualTo((CGameCtnGhost.EKindCpu)cpuKind);
        await Assert.That(validationGhost.SimulationFlags).IsEqualTo(0x12345679);
        await Assert.That(validationStream.Position).IsEqualTo((long)validationPayload.Length);
        var validationRewritten = Payload(w =>
        {
            using var writerWriter = new GbxReaderWriter(w);
            new CGameCtnGhost.Chunk0309202D().ReadWrite(validationGhost, writerWriter);
        });
        await Assert.That(validationRewritten.SequenceEqual(validationPayload)).IsTrue();
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task ReadWrite_RaceResult_ReadsNativeFormatAndPreservesUnknowns(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(12345); // time
            w.Write(0); // stunts score
            w.Write(2); // respawns
            w.Write(0x12345678); // extra result field
            w.Write(2); // checkpoint count
            w.Write(4567);
            w.Write(37); // checkpoint identifier
            w.Write(12345);
            w.Write(-1); // missing identifier
            w.Write(0x76543210); // trailing ghost field
        });
        using var stream = new MemoryStream();
        stream.Write(payload);
        using (var writer = new GbxWriter(stream)) writer.Write(0x11223344);
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        var ghost = new CGameCtnGhost();
        var chunk = new CGameCtnGhost.Chunk0309202B();
        chunk.ReadWrite(ghost, rw);

        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(ghost.RaceTime?.TotalMilliseconds).IsEqualTo(12345);
        await Assert.That(ghost.StuntScore).IsEqualTo(0);
        await Assert.That(ghost.Respawns).IsEqualTo(2);
        await Assert.That(ghost.SpawnLandmarkId).IsEqualTo(0x12345678);
        await Assert.That(ghost.Checkpoints!.Length).IsEqualTo(2);
        await Assert.That(ghost.Checkpoints[0].Time?.TotalMilliseconds).IsEqualTo(4567);
        await Assert.That(ghost.Checkpoints[0].CheckpointId).IsEqualTo(37);
        await Assert.That(ghost.Checkpoints[1].Time?.TotalMilliseconds).IsEqualTo(12345);
        await Assert.That(ghost.Checkpoints[1].CheckpointId).IsEqualTo(-1);
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);

        var rewritten = Payload(w =>
        {
            using var writerWriter = new GbxReaderWriter(w);
            chunk.ReadWrite(ghost, writerWriter);
        });
        await Assert.That(rewritten.SequenceEqual(payload)).IsTrue();
    }

    [Test]
    [Arguments(0, 10000, false)]
    [Arguments(1, 10000, false)]
    [Arguments(1, 0, false)]
    [Arguments(1, -1, false)]
    [Arguments(2, 10000, false)]
    [Arguments(2, -1, false)]
    [Arguments(0, -1, true)]
    [Arguments(1, -1, true)]
    [Arguments(2, -1, true)]
    public async Task ReadWrite_LegacyRaceResult_DecodesRawAndDeltaTimes(int version, int raceTime, bool empty)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            if (version == 2) w.Write((ushort)(empty ? 0 : 3));
            if (version != 2) w.Write(raceTime);
            if (version == 0)
            {
                w.Write(50000);
                w.Write(-1); // missing respawn count
                w.Write(empty ? 0 : 3);
            }
            else
            {
                w.Write((ushort)50000); // unsigned score
                w.Write(ushort.MaxValue); // missing respawn count
                if (version == 1) w.Write((ushort)(empty ? 0 : 3));
                else w.Write(raceTime);
            }

            if (!empty)
            {
                if (version == 0)
                {
                    w.Write(1000);
                    w.Write(4000);
                    w.Write(9000);
                }
                else if (version == 1)
                {
                    w.Write(unchecked((raceTime == 0 ? -1 : raceTime) - 9000));
                    w.Write(5000);
                    w.Write(3000);
                }
                else
                {
                    w.Write(1000);
                    w.Write(3000);
                    w.Write(5000);
                }
            }

            if (version == 2) w.Write(120000); // replay start, separate from validation start
        });

        using var stream = new MemoryStream(Payload(w =>
        {
            w.Write(payload);
            w.Write(0x11223344);
        }));
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        var ghost = new CGameCtnGhost
        {
            StuntScore = -1,
            Respawns = -1,
            Validate_RaceStartTime = TimeInt32.FromMilliseconds(99000)
        };
        var chunk = new CGameCtnGhost.Chunk0309201B();
        chunk.ReadWrite(ghost, rw);

        await Assert.That(chunk.Ignore).IsFalse();
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(ghost.RaceTime?.TotalMilliseconds).IsEqualTo(raceTime == -1 ? null : (int?)raceTime);
        await Assert.That(ghost.StuntScore).IsEqualTo(50000);
        await Assert.That(ghost.Respawns).IsEqualTo(-1);
        await Assert.That(ghost.Checkpoints!.Length).IsEqualTo(empty ? 0 : 3);
        if (!empty)
        {
            await Assert.That(ghost.Checkpoints.Select(x => x.Time?.TotalMilliseconds).SequenceEqual(new int?[] { 1000, 4000, 9000 })).IsTrue();
            await Assert.That(ghost.Checkpoints.All(x => x.Speed is null && x.StuntsScore is null && x.CheckpointId is null)).IsTrue();
        }
        await Assert.That(ghost.RaceStartTime?.TotalMilliseconds).IsEqualTo(version == 2 ? 120000 : (int?)null);
        await Assert.That(ghost.Validate_RaceStartTime?.TotalMilliseconds).IsEqualTo(99000);
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);

        var rewritten = Payload(w =>
        {
            using var writerWriter = new GbxReaderWriter(w);
            chunk.ReadWrite(ghost, writerWriter);
        });
        await Assert.That(rewritten.SequenceEqual(payload)).IsTrue();
    }

    [Test]
    [Arguments(CGameCtnGhost.EKindCpu.Unknown, 0)]
    [Arguments(CGameCtnGhost.EKindCpu.X86, 1)]
    [Arguments(CGameCtnGhost.EKindCpu.IntelX86, 2)]
    [Arguments(CGameCtnGhost.EKindCpu.AmdX86, 3)]
    [Arguments(CGameCtnGhost.EKindCpu.X64, 4)]
    [Arguments(CGameCtnGhost.EKindCpu.IntelX64, 5)]
    [Arguments(CGameCtnGhost.EKindCpu.AmdX64, 6)]
    public async Task CpuKind_UsesNativeValues(CGameCtnGhost.EKindCpu kind, int value)
    {
        await Assert.That((int)kind).IsEqualTo(value);
    }

    [Test]
    [Arguments(1, 0)]
    [Arguments(1, 1)]
    [Arguments(1, 2)]
    [Arguments(1, 3)]
    [Arguments(2, 4)]
    [Arguments(2, 5)]
    [Arguments(3, 6)]
    [Arguments(3, -1)]
    [Arguments(3, int.MaxValue)]
    public async Task ReadWrite_SystemInfo_PreservesCpuKindIndependentlyOfValidation(int version, int cpuKind)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write("CPU");
            w.Write(cpuKind);
            w.Write("GPU");
            w.Write(123);
            w.Write(456);
            w.Write("Hardware");
            w.WriteIPv4(IPAddress.Loopback);
            w.Write("User");
            if (version >= 2)
            {
                w.Write((byte)1);
                w.Write(0);
                w.Write(0);
            }
            if (version >= 3)
            {
                w.Write((byte)0);
                w.Write((byte)0);
            }
        });
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        var ghost = new CGameCtnGhost { Validate_CpuKind = CGameCtnGhost.EKindCpu.IntelX64 };
        var chunk = new CGameCtnGhost.Chunk03092023();
        chunk.ReadWrite(ghost, rw);

        await Assert.That(ghost.CpuKind).IsEqualTo((CGameCtnGhost.EKindCpu)cpuKind);
        await Assert.That(ghost.Validate_CpuKind).IsEqualTo(CGameCtnGhost.EKindCpu.IntelX64);
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);
        ghost.Validate_CpuKind = CGameCtnGhost.EKindCpu.AmdX64;
        await Assert.That(ghost.CpuKind).IsEqualTo((CGameCtnGhost.EKindCpu)cpuKind);
        var rewritten = Payload(w =>
        {
            using var writeRw = new GbxReaderWriter(w);
            new CGameCtnGhost.Chunk03092023 { Version = version }.ReadWrite(ghost, writeRw);
        });
        await Assert.That(rewritten.SequenceEqual(payload)).IsTrue();
    }

    private static Chunk<CGameCtnGhost> ValidationInputChunk(int offset, int version) => offset switch
    {
        0x11 => new CGameCtnGhost.Chunk03092011(),
        0x19 => new CGameCtnGhost.Chunk03092019(),
        0x25 => new CGameCtnGhost.Chunk03092025 { Version = version },
        _ => throw new ArgumentOutOfRangeException(nameof(offset))
    };

    private static byte[] Payload(Action<GbxWriter> write)
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream)) write(writer);
        return stream.ToArray();
    }
}
