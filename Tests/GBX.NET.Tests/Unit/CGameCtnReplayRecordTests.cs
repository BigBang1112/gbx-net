using GBX.NET.Engines.Game;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit;

public class CGameCtnReplayRecordTests
{
    [Test]
    [Arguments("TM10")]
    [Arguments("TMPU")]
    [Arguments("TMSX")]
    [Arguments("TMNESWC")]
    [Arguments("TMU")]
    [Arguments("TMF")]
    [Arguments("MP3")]
    [Arguments("TMT")]
    [Arguments("MP4")]
    public async Task ReplayFixtures_ParseWithoutIgnoringChunkFailures(string game)
    {
        var path = TestFiles.Gbx("CGameCtnReplayRecord", $"GBX-NET 2 CGameCtnReplayRecord {game} 001.Replay.Gbx");
        var replay = Gbx.ParseNode<CGameCtnReplayRecord>(path, new GbxReadSettings { SafeSkippableChunks = false });
        await Assert.That(replay.GetGhosts().Any()).IsTrue();
        if (game == "MP4")
        {
            await Assert.That(replay.ActionModels).IsNotNull();
            await Assert.That(replay.ItemNames).IsNotNull();
        }
    }

    [Test]
    public async Task TM2020ReplayHeader_Parses()
    {
        // Full parsing of this fixture currently fails inside CGameCtnGhost.
        var path = TestFiles.Gbx("CGameCtnReplayRecord", "GBX-NET 2 CGameCtnReplayRecord TM2020 001.Replay.Gbx");
        var replay = Gbx.ParseHeader<CGameCtnReplayRecord>(path).Node;
        await Assert.That(replay.MapInfo).IsNotNull();
        await Assert.That(replay.PlayerNickname).IsNotNull();
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
        await Assert.That(chunk.U01).IsEqualTo(10.5f);
        await Assert.That(replay.SceneryCapturableCount).IsEqualTo(hasTape ? 17 : 0);
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
            if (version != 0) w.WriteArray(new[] { "Resource" });
        });
        var chunk = new CGameCtnReplayRecord.Chunk0309301E();
        var replay = await ReadChunk(payload, chunk);
        await Assert.That(replay.ActionModels!.Count).IsEqualTo(2);
        await Assert.That(replay.ActionNames![1]).IsEqualTo(version >= 2 ? "Second" : "*Replay*\\Action001");
        await Assert.That(chunk.U01?.Single()).IsEqualTo(version != 0 ? "Resource" : null);
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
        var chunk = new CGameCtnReplayRecord.Chunk03093022();
        await ReadChunk(payload, chunk);
        await Assert.That(chunk.U01![1].SequenceEqual(records.Skip(recordSize))).IsTrue();
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
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task RecordData_VersionZeroOmitsTheSecondNode(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(-1);
            if (version != 0) w.Write(-1);
        });
        await ReadChunk(payload, new CGameCtnReplayRecord.Chunk03093024());
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
        await ReadChunk(payload, chunk);
        var restored = chunk is CGameCtnReplayRecord.Chunk03093005 old ? old.U02 : ((CGameCtnReplayRecord.Chunk03093013)chunk).U01;
        await Assert.That(restored![1].SequenceEqual(records.Skip(recordSize))).IsTrue();
    }

    [Test]
    public async Task LegacyTapes_ReadsEncapsulatedNodes()
    {
        var payload = Payload(w => w.WriteEncapsulated(inner =>
        {
            var tape = new CPlugDataTape();
            inner.WriteNodeRef(tape);
            inner.WriteNodeRef(tape);
            inner.Write(-1);
        }));
        var chunk = new CGameCtnReplayRecord.Chunk03093017();
        await ReadChunk(payload, chunk);
        await Assert.That(chunk.U01).IsNotNull();
        await Assert.That(chunk.U02).IsNotNull();
        await Assert.That(chunk.U03).IsNull();
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
        await Assert.That(replay.GetChunk<CGameCtnReplayRecord.Chunk03093012>()!.U02!.Length).IsEqualTo(16);
        await Assert.That(replay.GetChunk<CGameCtnReplayRecord.Chunk03093017>()).IsNotNull();
        await Assert.That(replay.GetChunk<CGameCtnReplayRecord.Chunk03093019>()!.U01).IsEqualTo(3.5f);
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
