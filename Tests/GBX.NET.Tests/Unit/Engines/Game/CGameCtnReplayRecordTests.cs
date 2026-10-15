using GBX.NET.Engines.Function;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.Plug;
using GBX.NET.Inputs;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;
using GBX.NET.Tests.Unit.Engines.GameData;
using TmEssentials;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnReplayRecordTests
{
    [Test]
    [Arguments(false, false, false)]
    [Arguments(false, false, true)]
    [Arguments(false, true, false)]
    [Arguments(false, true, true)]
    [Arguments(true, false, false)]
    [Arguments(true, false, true)]
    [Arguments(true, true, false)]
    [Arguments(true, true, true)]
    public async Task Tm10Inputs_ArePassedToTheValidationGhost(bool legacyGhostList, bool ghostsFirst, bool empty)
    {
        var payload = Payload(w =>
        {
            if (ghostsFirst) WriteGhosts(w, 1, legacyGhostList);
            WriteTm10Inputs(w, empty);
            if (!ghostsFirst) WriteGhosts(w, 1, legacyGhostList);
            w.Write(0xFACADE01u);
        });
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        var replay = reader.ReadNode<CGameCtnReplayRecord>()!;
        var ghost = replay.Ghosts![0]!;
        await Assert.That(ghost.EventsDuration).IsEqualTo(new TimeInt32(empty ? 0 : 1234));
        if (empty)
        {
            await Assert.That(ghost.Inputs).IsEmpty();
        }
        else
        {
            await Assert.That(ghost.Inputs.Length).IsEqualTo(2);
            await Assert.That(ghost.Inputs[0]).IsEqualTo(new Accelerate(new TimeInt32(10), true));
            await Assert.That(ghost.Inputs[1]).IsEqualTo(new Brake(new TimeInt32(20), true));
        }
        await Assert.That(ghost.Validate_ExeVersion).IsEqualTo("Own");
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task Tm10Inputs_HandlesZeroTimestampEntries(bool hasInputs, bool hasZeroTimestamps)
    {
        int[] timestamps = (hasInputs, hasZeroTimestamps) switch
        {
            (false, false) => [],
            (false, true) => [0, 0],
            (true, false) => [10020, 10010],
            (true, true) => [0, 10020, 0, 10010, 0]
        };
        var payload = Payload(w =>
        {
            w.Write(0x03093003u);
            w.Write(1234);
            w.Write(0);
            w.Write(2);
            foreach (var name in new[] { "Accelerate", "Brake" })
            {
                w.Write(0);
                w.Write(0);
                w.Write(name);
            }
            w.Write(timestamps.Length);
            foreach (var timestamp in timestamps)
            {
                w.Write(timestamp);
                if (timestamp == 0) continue;
                w.Write(timestamp == 10020 ? 1 : 0);
                w.Write(1u);
            }
            WriteGhosts(w, 1);
            w.Write(0xFACADE01u);
        });
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        var replay = reader.ReadNode<CGameCtnReplayRecord>()!;
        var ghost = replay.Ghosts![0]!;
        await Assert.That(ghost.EventsDuration).IsEqualTo(new TimeInt32(1234));
        await Assert.That(ghost.Inputs.Length).IsEqualTo(hasInputs ? 2 : 0);
        if (hasInputs)
        {
            await Assert.That(ghost.Inputs[0]).IsEqualTo(new Accelerate(new TimeInt32(10), true));
            await Assert.That(ghost.Inputs[1]).IsEqualTo(new Brake(new TimeInt32(20), true));
        }
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);
    }

    [Test]
    public async Task Tm10Inputs_AreReleasedAfterTheHandoff()
    {
        var payload = Payload(w =>
        {
            WriteTm10Inputs(w);
            WriteGhosts(w, 1, legacy: true);
            WriteGhosts(w, 1);
            w.Write(0xFACADE01u);
        });
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        var replay = reader.ReadNode<CGameCtnReplayRecord>()!;
        var ghost = replay.Ghosts![0]!;
        await Assert.That(ghost.EventsDuration).IsEqualTo(new TimeInt32(1));
        await Assert.That(ghost.Inputs.Single()).IsEqualTo(new Accelerate(new TimeInt32(100), false));
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Tm10Inputs_ResolveTheExternalValidationGhost(bool available)
    {
        using var payload = new ObjectModelPayload();
        WriteTm10Inputs(payload.Writer);
        payload.Writer.Write(0x03093014u);
        payload.Writer.Write(10);
        payload.Writer.Write(1);
        var file = payload.NodeRef("Validation.Ghost.Gbx");
        var ghost = new CGameCtnGhost();
        var loads = 0;
        if (available)
        {
            file.RefTable.ExternalNodes[file.FilePath] = () =>
            {
                loads++;
                return new Gbx<CGameCtnGhost>(ghost);
            };
        }
        payload.Writer.Write(0);
        payload.Writer.WriteArray(Array.Empty<long>());
        payload.Writer.Write(0xFACADE01u);
        payload.Finish();
        var replay = new CGameCtnReplayRecord();
        payload.Read(replay.ReadWrite);

        await Assert.That(replay.Ghosts!.Get(0).File).IsSameReferenceAs(file);
        await Assert.That(replay.Ghosts.Get(0).State).IsEqualTo(available ? NodeRefState.Resolved : NodeRefState.Missing);
        await Assert.That(ghost.Inputs.Length).IsEqualTo(available ? 2 : 0);
        await Assert.That(loads).IsEqualTo(available ? 1 : 0);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ExecutableVersion_IsPassedDirectlyToTheGhost(bool hasGhost)
    {
        var records = Enumerable.Range(0, 144).Select(i => (byte)i).ToArray();
        var payload = Payload(w =>
        {
            if (hasGhost) WriteGhosts(w, 1);
            WriteSkippable(w, 0x03093008, inner =>
            {
                inner.Write("Executable");
                inner.Write(2);
                inner.Write(records);
            });
            w.Write(0xFACADE01u);
        });
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        var replay = reader.ReadNode<CGameCtnReplayRecord>()!;
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);
        await Assert.That(replay.OldCutKeys2!.Length).IsEqualTo(2);
        await Assert.That(replay.OldCutKeys2[0].SequenceEqual(records.Take(72))).IsTrue();
        await Assert.That(replay.OldCutKeys2[1].SequenceEqual(records.Skip(72))).IsTrue();
        if (hasGhost)
        {
            await Assert.That(replay.Ghosts![0]!.Validate_ExeVersion).IsEqualTo("Executable");
            await Assert.That(replay.Ghosts[0]!.EventsDuration).IsEqualTo(new TimeInt32(1));
        }
#pragma warning disable GBXNET10001
        var clone = (CGameCtnReplayRecord)replay.DeepClone();
#pragma warning restore GBXNET10001
        await Assert.That(clone.OldCutKeys2![1].SequenceEqual(records.Skip(72))).IsTrue();
        await Assert.That(clone.OldCutKeys2[0]).IsNotSameReferenceAs(replay.OldCutKeys2[0]);
        if (hasGhost) await Assert.That(clone.Ghosts![0]!.Validate_ExeVersion).IsEqualTo("Executable");
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task LegacyValidation_IsPassedOnlyToASingleGhost(int ghostCount)
    {
        var key = new UInt128(0x1122334455667788, 0x99AABBCCDDEEFF00);
        var payload = Payload(w =>
        {
            WriteGhosts(w, ghostCount);
            w.Write(0x0309300Du);
            w.Write(1234);
            w.Write(7);
            w.WriteArrayId(new[] { "Accelerate" });
            w.Write(1);
            w.Write(23);
            w.Write(100010);
            w.Write((byte)0);
            w.Write(1u);
            WriteSkippable(w, 0x0309300F, inner =>
            {
                inner.Write("Legacy");
                inner.Write(0x12345678u);
                inner.Write(2);
                inner.Write(3);
                inner.Write("Settings");
            });
            w.Write(0x03093012u);
            w.Write(unchecked((int)0x89ABCDEF));
            w.WriteUInt128(key);
            w.Write(0xFACADE01u);
        });
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        var replay = reader.ReadNode<CGameCtnReplayRecord>()!;
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);
        await Assert.That(replay.GetChunk<CGameCtnReplayRecord.Chunk03093012>()!.U01).IsEqualTo(unchecked((int)0x89ABCDEF));
        foreach (var ghost in replay.Ghosts!)
        {
            await Assert.That(ghost!.Validate_ExeVersion).IsEqualTo(ghostCount == 1 ? "Legacy" : "Own");
            if (ghostCount != 1) continue;
            await Assert.That(ghost.EventsDuration).IsEqualTo(new TimeInt32(1234));
            await Assert.That(ghost.InputStoreVersion).IsEqualTo(7);
            await Assert.That(ghost.InputCountLimit).IsEqualTo(23);
            await Assert.That(ghost.Inputs[0]).IsEqualTo(new Accelerate(new TimeInt32(10), true));
            await Assert.That(ghost.Validate_ExeChecksum).IsEqualTo(0x12345678u);
            await Assert.That(ghost.Validate_OsKind).IsEqualTo(2);
            await Assert.That(ghost.Validate_CpuKind).IsEqualTo(3);
            await Assert.That(ghost.Validate_RaceSettings).IsEqualTo("Settings");
            await Assert.That(ghost.SecurityKey128).IsEqualTo(key);
        }
    }

    [Test]
    public async Task GhostValidationBeforeTheGhostList_IsNotRetainedOnTheReplay()
    {
        var payload = Payload(w =>
        {
            WriteSkippable(w, 0x03093008, inner =>
            {
                inner.Write("Executable");
                inner.Write(0);
            });
            WriteGhosts(w, 1);
            w.Write(0xFACADE01u);
        });
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        var replay = reader.ReadNode<CGameCtnReplayRecord>()!;
        await Assert.That(replay.Ghosts![0]!.Validate_ExeVersion).IsEqualTo("Own");
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);
    }

    [Test]
    public async Task LegacyGhostList_AppliesItsVersionToEveryGhost()
    {
        var replay = await ReadChunk(Payload(w =>
        {
            w.Write(17);
            w.WriteListNodeRef_deprec<CGameCtnGhost>([new(), new()]);
            w.Write(0);
            w.WriteArray(Array.Empty<long>());
        }), new CGameCtnReplayRecord.Chunk03093004());
        foreach (var ghost in replay.Ghosts!)
        {
            await Assert.That(ghost!.GhostVersion).IsEqualTo(17);
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task LegacyValidation_ResolvesAnExternalGhostAndPreservesItsDescriptor(bool available)
    {
        using var payload = new ObjectModelPayload();
        payload.Writer.Write(0x03093014u);
        payload.Writer.Write(10);
        payload.Writer.Write(1);
        var file = payload.NodeRef("External.Ghost.Gbx");
        var ghost = new CGameCtnGhost();
        var loads = 0;
        if (available)
        {
            file.RefTable.ExternalNodes[file.FilePath] = () =>
            {
                loads++;
                return new Gbx<CGameCtnGhost>(ghost);
            };
        }
        payload.Writer.Write(0);
        payload.Writer.WriteArray(Array.Empty<long>());
        WriteSkippable(payload.Writer, 0x03093008, w =>
        {
            w.Write("External validation");
            w.Write(0);
        });
        payload.Writer.Write(0xFACADE01u);
        payload.Finish();
        var replay = new CGameCtnReplayRecord();
        payload.Read(replay.ReadWrite);

        await Assert.That(replay.Ghosts!.Get(0).File).IsSameReferenceAs(file);
        await Assert.That(replay.Ghosts.Get(0).State).IsEqualTo(available ? NodeRefState.Resolved : NodeRefState.Missing);
        await Assert.That(ghost.Validate_ExeVersion).IsEqualTo(available ? "External validation" : null);
        await Assert.That(loads).IsEqualTo(available ? 1 : 0);
    }

    [Test]
    public async Task EmbeddedMap_PreservesTheGhostUidsAndKeepsTheMapLazy()
    {
        var map = new CGameCtnChallenge { MapInfo = new Ident("EmbeddedMap", "Stadium", "Author") };
        map.CreateChunk<CGameCtnChallenge.HeaderChunk03043003>();
        using var mapStream = new MemoryStream();
        new Gbx<CGameCtnChallenge>(map).Save(mapStream);
        var payload = Payload(w =>
        {
            WriteGhosts(w, 2);
            w.Write(0x03093002u);
            w.WriteData(mapStream.ToArray());
            w.Write(0xFACADE01u);
        });
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        var replay = reader.ReadNode<CGameCtnReplayRecord>()!;
        foreach (var ghost in replay.Ghosts!)
        {
            await Assert.That(ghost!.Validate_ChallengeUid).IsEqualTo("OwnMap");
        }
        await Assert.That(replay.ChallengeGbxData!.Parsed).IsFalse();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task LegacyValidation_EmptyEventsClearTheInputsAndPreserveOtherGhostState(bool hasDuration)
    {
        var payload = Payload(w =>
        {
            WriteGhosts(w, 1);
            w.Write(0x0309300Du);
            w.Write(hasDuration ? 1234 : 0);
            if (hasDuration)
            {
                w.Write(7);
                w.WriteArrayId(Array.Empty<string>());
                w.Write(0);
                w.Write(23);
            }
            w.Write(0xFACADE01u);
        });
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        var replay = reader.ReadNode<CGameCtnReplayRecord>()!;
        await Assert.That(replay.Ghosts![0]!.EventsDuration).IsEqualTo(new TimeInt32(hasDuration ? 1234 : 0));
        await Assert.That(replay.Ghosts[0]!.InputStoreVersion).IsEqualTo(hasDuration ? 7 : 0);
        await Assert.That(replay.Ghosts[0]!.InputCountLimit).IsEqualTo(hasDuration ? 23 : 0);
        await Assert.That(replay.Ghosts[0]!.Inputs).IsEmpty();
        await Assert.That(replay.Ghosts[0]!.Validate_ExeVersion).IsEqualTo("Own");
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);
    }

    [Test]
    public async Task RecordData_PreservesTheReplayAndGhostReferencesSeparately()
    {
        var ghost = new CGameCtnGhost { RecordData = new CPlugEntRecordData() };
        ghost.CreateChunk<CGameCtnGhost.Chunk03092000>().Version = 5;
        var payload = Payload(w =>
        {
            w.Write(0x03093014u);
            w.WriteListNodeRef_deprec<CGameCtnGhost>([ghost]);
            w.Write(0);
            w.WriteArray(Array.Empty<long>());
            w.Write(0x03093024u);
            w.Write(1);
            w.Write(-1);
            w.WriteNodeRef(new CPlugEntRecordData());
            w.Write(0xFACADE01u);
        });
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        var replay = reader.ReadNode<CGameCtnReplayRecord>()!;
        await Assert.That(replay.RecordData).IsNotNull();
        await Assert.That(replay.Ghosts![0]!.RecordData).IsNotNull();
        await Assert.That(replay.Ghosts[0]!.RecordData).IsNotSameReferenceAs(replay.RecordData);
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);
    }

    [Test]
    [Arguments(-1)]
    [Arguments(4)]
    [Arguments(6)]
    [Arguments(8)]
    [Arguments(9999)]
    [Arguments(10000)]
    [Arguments(10001)]
    public async Task Header_HandlesSpecialVersionsWithoutReadingAnAbsentLogin(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            if (version >= 4 && version != 9999)
            {
                w.Write(new Ident("Map", "Stadium", "Author"));
                w.Write(12345);
                w.Write("Nickname");
                if (version >= 6 && version != 10000) w.Write("Login");
            }
            if (version > 7)
            {
                w.Write((byte)3);
                w.WriteIdAsString("Title");
            }
        });
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        var replay = new CGameCtnReplayRecord();
        new CGameCtnReplayRecord.HeaderChunk03093000().Read(replay, reader);
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);
        await Assert.That(replay.PlayerLogin).IsEqualTo(version >= 6 && version is not (9999 or 10000) ? "Login" : null);
        await Assert.That(replay.TitleId).IsEqualTo(version > 7 ? "Title" : null);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Scenery_ReadsMultipleKeysAndTrailingFields(bool hasTape)
    {
        var payload = Payload(w =>
        {
            w.Write(0);
            w.Write(2);
            for (var i = 0; i < 8; i++) w.Write(i + 0.25f);
            w.Write(10.5f);
            if (hasTape)
            {
                w.Write(-1);
                w.Write(17);
            }
        });
        CGameCtnReplayRecord.Chunk03093019 chunk = hasTape
            ? new CGameCtnReplayRecord.Chunk0309301A()
            : new CGameCtnReplayRecord.Chunk03093019();
        var replay = await ReadChunk(payload, chunk);
        await Assert.That(replay.SceneryVortexKeys!.Count).IsEqualTo(2);
        await Assert.That(replay.SceneryVortexKeys[1].Time.TotalSeconds).IsEqualTo(4.25f);
        await Assert.That(replay.SceneryVortexKeys[1].VortexRadius).IsEqualTo(5.25f);
        await Assert.That(replay.SceneryVortexKeys[1].VortexCenterXZ).IsEqualTo(new Vec2(6.25f, 7.25f));
        await Assert.That(replay.SceneryVortexShrinkSpeed).IsEqualTo(10.5f);
        await Assert.That(replay.SceneryCapturableDataTape).IsNull();
        await Assert.That(replay.SceneryCapturableCount).IsEqualTo(hasTape ? 17 : 0);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task SceneryCapturables_PreservesEmbeddedAndExternalTapes(bool external)
    {
        using var payload = new ObjectModelPayload();
        payload.Writer.Write(0);
        payload.Writer.Write(0);
        payload.Writer.Write(12.5f);
        var tape = CreateTape(33);
        var file = external ? payload.NodeRef("Scenery.Tape.Gbx") : null;
        var loads = 0;
        if (file is not null)
        {
            file.RefTable.ExternalNodes[file.FilePath] = () =>
            {
                loads++;
                return new Gbx<CPlugDataTape>(tape);
            };
        }
        else
        {
            payload.Writer.WriteNodeRef(tape);
        }
        payload.Writer.Write(17);
        payload.Finish();
        var replay = new CGameCtnReplayRecord();
        payload.Read(rw => new CGameCtnReplayRecord.Chunk0309301A().Read(replay, rw.Reader!));
        await Assert.That(loads).IsEqualTo(0);
        await Assert.That(replay.SceneryVortexShrinkSpeed).IsEqualTo(12.5f);
        await Assert.That(replay.SceneryCapturableCount).IsEqualTo(17);
        await Assert.That(replay.References.SceneryCapturableDataTape.Kind)
            .IsEqualTo(external ? NodeRefKind.ExternalFile : NodeRefKind.Embedded);
        if (file is not null)
        {
            await Assert.That(replay.References.SceneryCapturableDataTape.File).IsSameReferenceAs(file);
            await Assert.That(replay.References.SceneryCapturableDataTape.State).IsEqualTo(NodeRefState.Unresolved);
        }
        await Assert.That(replay.SceneryCapturableDataTape!.Header!.U01).IsEqualTo(33);
        await Assert.That(loads).IsEqualTo(external ? 1 : 0);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task PlayersOfInterest_ReadsGhostIndicesAndReplayTimes(bool empty)
    {
        var payload = Payload(w =>
        {
            w.Write(0);
            w.WriteArray(empty ? Array.Empty<Int2>() : [new Int2(2, 1234), new Int2(0, 5678)]);
        });
        var replay = await ReadChunk(payload, new CGameCtnReplayRecord.Chunk0309301B());
        await Assert.That(replay.PlayersOfInterest!.Value.Length).IsEqualTo(empty ? 0 : 2);
        if (!empty)
        {
            await Assert.That(replay.PlayersOfInterest.Value[0].GhostIndex).IsEqualTo(2);
            await Assert.That(replay.PlayersOfInterest.Value[0].Time).IsEqualTo(new TimeInt32(1234));
            await Assert.That(replay.PlayersOfInterest.Value[1].GhostIndex).IsEqualTo(0);
            await Assert.That(replay.PlayersOfInterest.Value[1].Time).IsEqualTo(new TimeInt32(5678));
        }
#pragma warning disable GBXNET10001
        var clone = (CGameCtnReplayRecord)replay.DeepClone();
#pragma warning restore GBXNET10001
        await Assert.That(clone.PlayersOfInterest!.Value.SequenceEqual(replay.PlayersOfInterest.Value)).IsTrue();
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task Actions_ReadsInlineArchivesAndVersionedNames(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(2);
            w.Write(0xFACADE01u);
            w.Write(0xFACADE01u);
            if (version >= 2) w.WriteArray(new[] { "First", "Second" });
            if (version != 0) w.WriteArray(new[] { "Actions\\Shoot.Action.Gbx" });
        });
        var chunk = new CGameCtnReplayRecord.Chunk0309301E();
        var replay = await ReadChunk(payload, chunk);
        await Assert.That(replay.ActionModels!.Count).IsEqualTo(2);
        await Assert.That(replay.ActionNames![1]).IsEqualTo(version >= 2 ? "Second" : "*Replay*\\Action001");
        await Assert.That(replay.ActionFileNames?.Single()).IsEqualTo(version != 0 ? "Actions\\Shoot.Action.Gbx" : null);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task AnchoredObjects_ReadsTheLegacyArrayOnlyInVersionZero(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            if (version == 0)
            {
                w.Write(1);
                w.Write(true);
                w.Write(12);
                w.Write(1.25f);
                w.Write(2.5f);
                w.Write(3.75f);
                w.Write(34);
            }
            else w.Write(-1);
        });
        var chunk = new CGameCtnReplayRecord.Chunk0309301F();
        await ReadChunk(payload, chunk);
        if (version == 0)
        {
            await Assert.That(chunk.U01![0].U01).IsTrue();
            await Assert.That(chunk.U01[0].U06).IsEqualTo(34);
        }
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task ItemResources_VersionZeroHasNoSkinArray(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.WriteArray(new[] { "First", "Second" });
            if (version != 0) w.WriteArray(new[] { new PackDesc("First.Item.Gbx"), PackDesc.Empty });
        });
        var replay = await ReadChunk(payload, new CGameCtnReplayRecord.Chunk03093020());
        await Assert.That(replay.ItemNames![1]).IsEqualTo("Second");
        await Assert.That(replay.ItemSkins!.Count).IsEqualTo(2);
        await Assert.That(replay.ItemSkins[0].FilePath).IsEqualTo(version == 0 ? "" : "First.Item.Gbx");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Minimap_ReadsThePlaygroundFeatureFlag(bool enabled)
    {
        var replay = await ReadChunk(Payload(w =>
        {
            w.Write(0);
            w.Write(enabled);
        }), new CGameCtnReplayRecord.Chunk03093021());
        await Assert.That(replay.EnableMinimap).IsEqualTo(enabled);
    }

    [Test]
    [Arguments(80)]
    [Arguments(84)]
    public async Task TimedCameraRecords_ReadsBothGameWidths(int recordSize)
    {
        var records = Enumerable.Range(0, recordSize * 2).Select(i => (byte)i).ToArray();
        var payload = Payload(w =>
        {
            w.Write(0);
            w.Write(2);
            w.Write(records);
        });
        var replay = await ReadChunk(payload, new CGameCtnReplayRecord.Chunk03093022());
        await Assert.That(replay.CameraRecord!.Length).IsEqualTo(2);
        await Assert.That(replay.CameraRecord[1].SequenceEqual(records.Skip(recordSize))).IsTrue();
#pragma warning disable GBXNET10001
        var clone = (CGameCtnReplayRecord)replay.DeepClone();
#pragma warning restore GBXNET10001
        await Assert.That(clone.CameraRecord).IsNotSameReferenceAs(replay.CameraRecord);
        await Assert.That(clone.CameraRecord![1]).IsNotSameReferenceAs(replay.CameraRecord[1]);
        clone.CameraRecord[1][0] ^= 0xFF;
        await Assert.That(replay.CameraRecord[1][0]).IsEqualTo(records[recordSize]);
    }

    [Test]
    public async Task TimedCameraRecords_ReadsAnEmptyRecording()
    {
        var replay = await ReadChunk(Payload(w =>
        {
            w.Write(0);
            w.Write(0);
        }), new CGameCtnReplayRecord.Chunk03093022());
        await Assert.That(replay.CameraRecord).IsNotNull();
        await Assert.That(replay.CameraRecord!).IsEmpty();
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task SceneTapes_ReadsTheEmptyCarAndDeprecatedTurretRoles(bool recordData, bool external)
    {
        using var payload = new ObjectModelPayload();
        payload.Writer.Write(recordData ? 1 : 0);
        var tape = CreateTape(33);
        var file = external ? payload.NodeRef("Scene.Tape.Gbx") : null;
        var loads = 0;
        if (file is not null)
        {
            file.RefTable.ExternalNodes[file.FilePath] = () =>
            {
                loads++;
                return new Gbx<CPlugDataTape>(tape);
            };
        }
        else
        {
            payload.Writer.WriteNodeRef(tape);
        }
        if (recordData)
        {
            // Index 1 belongs to the tape, including the external reference supplied by the fixture.
            payload.Writer.Write(2);
            payload.Writer.Write(CPlugEntRecordData.Id);
            payload.Writer.WriteNode(new CPlugEntRecordData());
        }
        else
        {
            payload.Writer.Write(1);
            payload.Writer.Write(1.25f);
            payload.Writer.Write(34);
            payload.Writer.Write(true);
        }
        payload.Finish();
        var replay = new CGameCtnReplayRecord();
        Chunk<CGameCtnReplayRecord> chunk = recordData
            ? new CGameCtnReplayRecord.Chunk03093024()
            : new CGameCtnReplayRecord.Chunk03093023();
        payload.Read(rw => chunk.Read(replay, rw.Reader!));
        var reference = recordData ? replay.References.SceneryTurretsDataTape : replay.References.EmptyCarsVisDataTape;
        await Assert.That(loads).IsEqualTo(0);
        await Assert.That(reference.Kind).IsEqualTo(external ? NodeRefKind.ExternalFile : NodeRefKind.Embedded);
        if (file is not null)
        {
            await Assert.That(reference.File).IsSameReferenceAs(file);
            await Assert.That(reference.State).IsEqualTo(NodeRefState.Unresolved);
        }
        var loadedTape = recordData ? replay.SceneryTurretsDataTape : replay.EmptyCarsVisDataTape;
        await Assert.That(loadedTape!.Header!.U01).IsEqualTo(33);
        await Assert.That(loads).IsEqualTo(external ? 1 : 0);
        if (recordData)
        {
            await Assert.That(replay.EmptyCarsVisDataTape).IsNull();
            await Assert.That(replay.RecordData).IsNotNull();
        }
        else
        {
            await Assert.That(replay.SceneryTurretsDataTape).IsNull();
            await Assert.That(replay.BonusBumpKeys!.Single().Time.TotalSeconds).IsEqualTo(1.25f);
        }
    }

    [Test]
    public async Task BonusBumps_ReadsTimeAndBooleanForEveryKey()
    {
        var payload = Payload(w =>
        {
            w.Write(0);
            w.Write(-1);
            w.Write(2);
            w.Write(1.5f);
            w.Write(12);
            w.Write(true);
            w.Write(2.5f);
            w.Write(34);
            w.Write(false);
        });
        var replay = await ReadChunk(payload, new CGameCtnReplayRecord.Chunk03093023());
        await Assert.That(replay.BonusBumpKeys![0].U02).IsTrue();
        await Assert.That(replay.BonusBumpKeys[1].Time.TotalSeconds).IsEqualTo(2.5f);
        await Assert.That(replay.BonusBumpKeys[1].U01).IsEqualTo(34);
        await Assert.That(replay.BonusBumpKeys[1].U02).IsFalse();
        await Assert.That(replay.EmptyCarsVisDataTape).IsNull();
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    [Arguments(1)]
    public async Task RecordData_VersionZeroOmitsTheSecondNode(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(-1);
            if (version != 0) w.WriteNodeRef(new CPlugEntRecordData());
        });
        var replay = await ReadChunk(payload, new CGameCtnReplayRecord.Chunk03093024());
        await Assert.That(replay.SceneryTurretsDataTape).IsNull();
        await Assert.That(replay.RecordData is not null).IsEqualTo(version != 0);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task EntityGhostMappings_ReadsTheFourthFieldInASeparatePass(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(2);
            for (var i = 0; i < 6; i++) w.Write(10 + i);
            if (version >= 1)
            {
                w.Write(100);
                w.Write(200);
            }
        });
        var replay = await ReadChunk(payload, new CGameCtnReplayRecord.Chunk03093026());
        await Assert.That(replay.EntDataSceneUIdsToGhosts![1].U01).IsEqualTo(13);
        await Assert.That(replay.EntDataSceneUIdsToGhosts[0].U04).IsEqualTo(version == 0 ? 0x0FF00000 : 100);
        await Assert.That(replay.EntDataSceneUIdsToGhosts[1].U04).IsEqualTo(version == 0 ? 0x0FF00000 : 200);
    }

    [Test]
    [Arguments(68)]
    [Arguments(72)]
    public async Task LegacyCutKeys_PreservesEveryRecord(int recordSize)
    {
        var records = Enumerable.Range(0, recordSize * 2).Select(i => (byte)i).ToArray();
        var payload = Payload(w =>
        {
            w.Write(2);
            w.Write(records);
        });
        Chunk<CGameCtnReplayRecord> chunk = recordSize == 68
            ? new CGameCtnReplayRecord.Chunk03093005()
            : new CGameCtnReplayRecord.Chunk03093013();
        var replay = await ReadChunk(payload, chunk);
        var restored = recordSize == 68 ? replay.OldCutKeys![1] : replay.OldCutKeys2![1];
        await Assert.That(restored.SequenceEqual(records.Skip(recordSize))).IsTrue();
#pragma warning disable GBXNET10001
        var clone = (CGameCtnReplayRecord)replay.DeepClone();
#pragma warning restore GBXNET10001
        var clonedRecord = recordSize == 68 ? clone.OldCutKeys![1] : clone.OldCutKeys2![1];
        await Assert.That(clonedRecord.SequenceEqual(restored)).IsTrue();
        await Assert.That(clonedRecord).IsNotSameReferenceAs(restored);
    }

    [Test]
    [Arguments(0x005)]
    [Arguments(0x008)]
    [Arguments(0x013)]
    public async Task LegacyCutKeys_ReadsEmptyArrays(int chunkId)
    {
        var payload = Payload(w =>
        {
            if (chunkId == 0x008) w.Write("Executable");
            w.Write(0);
        });
        Chunk<CGameCtnReplayRecord> chunk = chunkId switch
        {
            0x005 => new CGameCtnReplayRecord.Chunk03093005(),
            0x008 => new CGameCtnReplayRecord.Chunk03093008(),
            _ => new CGameCtnReplayRecord.Chunk03093013()
        };
        var replay = await ReadChunk(payload, chunk);
        var keys = chunkId == 0x005 ? replay.OldCutKeys : replay.OldCutKeys2;
        await Assert.That(keys).IsEmpty();
    }

    [Test]
    [Arguments(-1)]
    [Arguments(unchecked((int)0x89ABCDEF))]
    public async Task UnusedLegacyValue_PreservesAllBits(int value)
    {
        var chunk = new CGameCtnReplayRecord.Chunk03093007();
        await Assert.That(chunk.U01).IsEqualTo(-1);
        await ReadChunk(Payload(w => w.Write(value)), chunk);
        await Assert.That(chunk.U01).IsEqualTo(value);
    }

    [Test]
    public async Task Tracks_ReadsEmbeddedTracksAndPreservesSharedReferencesWhenCloning()
    {
        var track = new CGameCtnMediaTrack { Name = "Legacy" };
        track.CreateChunk<CGameCtnMediaTrack.Chunk03078001>();
        var replay = await ReadChunk(Payload(w => w.WriteListNodeRef_deprec<CGameCtnMediaTrack>([track, track])),
            new CGameCtnReplayRecord.Chunk0309300A());
        await Assert.That(replay.Tracks![0]!.Name).IsEqualTo("Legacy");
        await Assert.That(replay.Tracks[0]).IsSameReferenceAs(replay.Tracks[1]);
#pragma warning disable GBXNET10001
        var clone = (CGameCtnReplayRecord)replay.DeepClone();
#pragma warning restore GBXNET10001
        await Assert.That(clone.Tracks![0]).IsNotSameReferenceAs(replay.Tracks[0]);
        await Assert.That(clone.Tracks[0]).IsSameReferenceAs(clone.Tracks[1]);
    }

    [Test]
    public async Task Tracks_PreservesExternalReferencesWithoutLoadingThem()
    {
        using var payload = new ObjectModelPayload();
        payload.Writer.Write(10);
        payload.Writer.Write(1);
        var file = payload.NodeRef("Legacy.Track.Gbx");
        var track = new CGameCtnMediaTrack();
        var loads = 0;
        file.RefTable.ExternalNodes[file.FilePath] = () =>
        {
            loads++;
            return new Gbx<CGameCtnMediaTrack>(track);
        };
        payload.Finish();
        var replay = new CGameCtnReplayRecord();
        payload.Read(rw => new CGameCtnReplayRecord.Chunk0309300A().Read(replay, rw.Reader!));
#pragma warning disable GBXNET10001
        var clone = (CGameCtnReplayRecord)replay.DeepClone();
#pragma warning restore GBXNET10001
        await Assert.That(loads).IsEqualTo(0);
        await Assert.That(replay.Tracks!.Get(0).File).IsSameReferenceAs(file);
        await Assert.That(clone.Tracks!.Get(0).File!.FilePath).IsEqualTo(file.FilePath);
        await Assert.That(clone.Tracks.Get(0).Kind).IsEqualTo(NodeRefKind.ExternalFile);
        await Assert.That(replay.Tracks[0]).IsSameReferenceAs(track);
        await Assert.That(loads).IsEqualTo(1);
    }

    [Test]
    public async Task HumanTimeToGameTimeFunc_ReadsTheConcreteSegmentNode()
    {
        var segment = new CFuncSegment
        {
            Keys = [new() { X = 1, ValueIn = 2, TangentIn = 3, ValueOut = 4, TangentOut = 5 }]
        };
        segment.CreateChunk<CFuncSegment.Chunk05037000>();
        var replay = await ReadChunk(Payload(w => w.WriteNodeRef(segment)), new CGameCtnReplayRecord.Chunk0309300B());
        await Assert.That(replay.HumanTimeToGameTimeFunc!.Keys![0].TangentOut).IsEqualTo(5f);
        await Assert.That(replay.References.HumanTimeToGameTimeFunc.Kind).IsEqualTo(NodeRefKind.Embedded);
    }

    [Test]
    public async Task HumanTimeToGameTimeFunc_PreservesAndResolvesATypedExternalReference()
    {
        using var payload = new ObjectModelPayload();
        var file = payload.NodeRef("TimeFunction.Gbx");
        var segment = new CFuncSegment();
        var loads = 0;
        file.RefTable.ExternalNodes[file.FilePath] = () =>
        {
            loads++;
            return new Gbx<CFuncSegment>(segment);
        };
        payload.Finish();
        var replay = new CGameCtnReplayRecord();
        payload.Read(rw => new CGameCtnReplayRecord.Chunk0309300B().Read(replay, rw.Reader!));
        await Assert.That(loads).IsEqualTo(0);
        await Assert.That(replay.References.HumanTimeToGameTimeFunc.File).IsSameReferenceAs(file);
        await Assert.That(replay.HumanTimeToGameTimeFunc).IsSameReferenceAs(segment);
        await Assert.That(loads).IsEqualTo(1);
        await Assert.That(replay.References.HumanTimeToGameTimeFunc.File).IsSameReferenceAs(file);
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task LegacyTapes_ReadsTheSameRolesWithBothFramings(bool encapsulated, bool hasArena)
    {
        var bullet = CreateTape(11);
        var character = CreateTape(22);
        var arena = hasArena ? CreateTape(33) : null;
        void WriteTapes(GbxWriter w)
        {
            w.WriteNodeRef(bullet);
            w.WriteNodeRef(character);
            w.WriteNodeRef(arena);
        }
        var payload = Payload(w =>
        {
            if (encapsulated) w.WriteEncapsulated(WriteTapes);
            else WriteTapes(w);
        });
        Chunk<CGameCtnReplayRecord> chunk = encapsulated
            ? new CGameCtnReplayRecord.Chunk03093017()
            : new CGameCtnReplayRecord.Chunk03093016();
        var replay = await ReadChunk(payload, chunk);
        await Assert.That(replay.BulletVisDataTape!.Header!.U01).IsEqualTo(11);
        await Assert.That(replay.CharVisDataTape!.Header!.U01).IsEqualTo(22);
        await Assert.That(replay.ArenaVisDataTape?.Header?.U01).IsEqualTo(hasArena ? 33 : (int?)null);
#pragma warning disable GBXNET10001
        var clone = (CGameCtnReplayRecord)replay.DeepClone();
#pragma warning restore GBXNET10001
        await Assert.That(clone.BulletVisDataTape!.Header!.U01).IsEqualTo(11);
        await Assert.That(clone.CharVisDataTape!.Header!.U01).IsEqualTo(22);
        await Assert.That(clone.ArenaVisDataTape?.Header?.U01).IsEqualTo(hasArena ? 33 : (int?)null);
        await Assert.That(clone.BulletVisDataTape).IsNotSameReferenceAs(replay.BulletVisDataTape);
    }

    [Test]
    public async Task LegacyTapes_PreservesExternalReferencesWithoutLoadingThem()
    {
        using var payload = new ObjectModelPayload();
        var bulletFile = payload.NodeRef("Bullet.Tape.Gbx");
        var charFile = payload.NodeRef("Character.Tape.Gbx");
        var arenaFile = payload.NodeRef("Arena.Tape.Gbx");
        var character = CreateTape(22);
        var loads = 0;
        charFile.RefTable.ExternalNodes[charFile.FilePath] = () =>
        {
            loads++;
            return new Gbx<CPlugDataTape>(character);
        };
        payload.Finish();
        var replay = new CGameCtnReplayRecord();
        payload.Read(rw => new CGameCtnReplayRecord.Chunk03093016().Read(replay, rw.Reader!));
#pragma warning disable GBXNET10001
        var clone = (CGameCtnReplayRecord)replay.DeepClone();
#pragma warning restore GBXNET10001
        await Assert.That(loads).IsEqualTo(0);
        await Assert.That(replay.References.BulletVisDataTape.File).IsSameReferenceAs(bulletFile);
        await Assert.That(replay.References.CharVisDataTape.File).IsSameReferenceAs(charFile);
        await Assert.That(replay.References.ArenaVisDataTape.File).IsSameReferenceAs(arenaFile);
        await Assert.That(clone.References.CharVisDataTape.File!.FilePath).IsEqualTo(charFile.FilePath);
        await Assert.That(clone.References.CharVisDataTape.State).IsEqualTo(NodeRefState.Unresolved);
        await Assert.That(replay.CharVisDataTape).IsSameReferenceAs(character);
        await Assert.That(loads).IsEqualTo(1);
    }

    [Test]
    public async Task MissingLegacyChunks_ReadThroughTheNodeDispatcher()
    {
        var payload = Payload(w =>
        {
            w.Write(0x03093009u);
            w.Write(2);
            w.Write(1.5f);
            w.Write(2.5f);
            w.Write(0x0309300Au);
            w.Write(10);
            w.Write(0);
            w.Write(0x0309300Bu);
            w.Write(-1);
            w.Write(0x03093012u);
            w.Write(9);
            w.Write(Enumerable.Repeat((byte)1, 16).ToArray());
            w.Write(0x03093016u);
            for (var i = 0; i < 3; i++) w.Write(-1);
            WriteSkippable(w, 0x03093017, inner => inner.WriteEncapsulated(encapsulated =>
            {
                for (var i = 0; i < 3; i++) encapsulated.Write(-1);
            }));
            WriteSkippable(w, 0x03093019, inner =>
            {
                inner.Write(0);
                inner.Write(0);
                inner.Write(3.5f);
            });
            w.Write(0xFACADE01u);
        });
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        var replay = reader.ReadNode<CGameCtnReplayRecord>()!;
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);
        await Assert.That(replay.GetChunk<CGameCtnReplayRecord.Chunk03093009>()!.U01![1]).IsEqualTo(2.5f);
        await Assert.That(replay.GetChunk<CGameCtnReplayRecord.Chunk03093012>()).IsNotNull();
        await Assert.That(replay.GetChunk<CGameCtnReplayRecord.Chunk03093017>()).IsNotNull();
        await Assert.That(replay.SceneryVortexShrinkSpeed).IsEqualTo(3.5f);
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task GhostLists_ReadsRecordingDurationAndPreservesOldShowTimes(bool legacy, bool hasShowTimes)
    {
        ulong[] records = hasShowTimes ? [0x0123456789ABCDEF, 0xFEDCBA9876543210] : [];
        var payload = Payload(w =>
        {
            w.Write(legacy ? 0x03093004u : 0x03093014u);
            if (legacy) w.Write(17);
            w.WriteListNodeRef_deprec<CGameCtnGhost>([]);
            w.Write(1734);
            w.WriteArray(records);
            w.Write(0xFACADE01u);
        });
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        var replay = reader.ReadNode<CGameCtnReplayRecord>()!;
        await Assert.That(replay.RecordDuration).IsEqualTo(new TimeInt32(1734));
        await Assert.That(replay.OldShowTimes!.Value.Select(x => x.Data).SequenceEqual(records)).IsTrue();
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);
#pragma warning disable GBXNET10001
        var clone = (CGameCtnReplayRecord)replay.DeepClone();
#pragma warning restore GBXNET10001
        await Assert.That(clone.RecordDuration).IsEqualTo(replay.RecordDuration);
        await Assert.That(clone.OldShowTimes!.Value.SequenceEqual(replay.OldShowTimes.Value)).IsTrue();
    }

    private static CPlugDataTape CreateTape(int header)
    {
        var tape = new CPlugDataTape { Header = new() { U01 = header } };
        tape.CreateChunk<CPlugDataTape.Chunk090CE000>();
        return tape;
    }

    private static void WriteTm10Inputs(GbxWriter writer, bool empty = false)
    {
        writer.Write(0x03093003u);
        writer.Write(empty ? 0 : 1234);
        if (empty) return;
        writer.Write(42);
        writer.Write(2);
        foreach (var name in new[] { "Accelerate", "Brake" })
        {
            writer.Write(0);
            writer.Write(0);
            writer.Write(name);
        }
        writer.Write(3); // The count includes the zero-timestamp entry.
        writer.Write(10020);
        writer.Write(1);
        writer.Write(1u);
        writer.Write(10010);
        writer.Write(0);
        writer.Write(1u);
        writer.Write(0);
    }

    private static void WriteGhosts(GbxWriter writer, int count, bool legacy = false)
    {
        writer.Write(legacy ? 0x03093004u : 0x03093014u);
        if (legacy) writer.Write(17);
        var ghosts = Enumerable.Range(0, count).Select<int, CGameCtnGhost?>(_ =>
        {
            var ghost = new CGameCtnGhost
            {
                Validate_ExeVersion = "Own",
                Validate_ChallengeUid = "OwnMap",
                EventsDuration = new TimeInt32(1),
                Inputs = [new Accelerate(new TimeInt32(100), false)]
            };
            ghost.CreateChunk<CGameCtnGhost.Chunk03092011>();
            ghost.CreateChunk<CGameCtnGhost.Chunk03092010>();
            return ghost;
        }).ToList();
        writer.WriteListNodeRef_deprec<CGameCtnGhost>(ghosts);
        writer.Write(0);
        writer.WriteArray(Array.Empty<long>());
    }

    private static void WriteSkippable(GbxWriter writer, uint chunkId, Action<GbxWriter> write)
    {
        var payload = Payload(write);
        writer.Write(chunkId);
        writer.Write(0x534B4950u);
        writer.Write(payload.Length);
        writer.Write(payload);
    }

    private static byte[] Payload(Action<GbxWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        write(writer);
        return stream.ToArray();
    }

    private static async Task<CGameCtnReplayRecord> ReadChunk(byte[] payload, Chunk<CGameCtnReplayRecord> chunk)
    {
        using var stream = new MemoryStream();
        stream.Write(payload);
        using (var writer = new GbxWriter(stream)) writer.Write(0x12345678);
        stream.Position = 0;
        var replay = new CGameCtnReplayRecord();
        using (var bounded = new BoundedStream(stream, payload.Length))
        using (var reader = new GbxReader(bounded))
        {
            chunk.Read(replay, reader);
            await Assert.That(bounded.Remaining).IsEqualTo(0L);
        }
        using (var reader = new GbxReader(stream))
        {
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
        }
        return replay;
    }
}
