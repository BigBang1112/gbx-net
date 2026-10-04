using GBX.NET.Engines.Game;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit;

public class CGameCtnChallengeIgnoredChunkTests
{
    [Test]
    public async Task Chunk0304303A_PreservesDataAndIdentifiers()
    {
        var payload = Payload(w =>
        {
            w.Write(7);
            w.WriteData([1, 2, 3]);
            w.Write(1);
            w.Write(new Ident("Model", "Stadium", "Author"));
        });
        var chunk = new CGameCtnChallenge.Chunk0304303A();
        await RoundTrip(payload, chunk);
        await Assert.That(chunk.U02!.SequenceEqual<byte>([1, 2, 3])).IsTrue();
        await Assert.That(chunk.U03![0].Id).IsEqualTo("Model");
    }

    [Test]
    public async Task Chunk03043041_ReadsEncapsulatedNode()
    {
        var payload = Payload(w => w.WriteEncapsulated(inner => WriteEmptyNode(inner, 0x01001000)));
        var chunk = new CGameCtnChallenge.Chunk03043041();
        await RoundTrip(payload, chunk);
        await Assert.That(chunk.U01).IsNotNull();
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task Chunk0304304D_ReadsDeprecatedArrays(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.WriteEncapsulated(inner =>
            {
                for (var i = 0; i < 2 + Math.Min(version, 2); i++)
                {
                    inner.Write(10);
                    inner.Write(1);
                    WriteEmptyNode(inner, 0x01001000);
                }
            });
        });
        await RoundTrip(payload, new CGameCtnChallenge.Chunk0304304D());
    }

    [Test]
    [Arguments(0)]
    [Arguments(5)]
    public async Task Chunk0304304E_PassesTriggerArchiveVersion(int triggerVersion)
    {
        var payload = Payload(w =>
        {
            w.Write(0);
            w.WriteEncapsulated(inner =>
            {
                inner.Write(-1);
                inner.Write(1);
                inner.Write(triggerVersion);
                if (triggerVersion >= 5) inner.Write(11);
                if (triggerVersion >= 4)
                {
                    for (var i = 0; i < 4; i++) inner.Write(1.5f + i);
                }
                if (triggerVersion >= 3) inner.Write(12);
                if (triggerVersion >= 2) inner.Write(13);
                if (triggerVersion >= 1)
                {
                    inner.Write(14);
                    inner.Write(15);
                }
                for (var i = 0; i < 6; i++) inner.Write(2.5f + i);
                inner.WriteIdAsString("Trigger");
            });
        });
        var node = await RoundTrip(payload, new CGameCtnChallenge.Chunk0304304E());
        await Assert.That(node.AnimationTriggers).Count().IsEqualTo(1);
    }

    [Test]
    public async Task Chunk0304304E_DerivesTriggerCountWhenWriting()
    {
        var original = new CGameCtnChallenge { AnimationTriggers = [new CPlugTriggerAction()] };
        var payload = Payload(w =>
        {
            using var rw = new GbxReaderWriter(w);
            new CGameCtnChallenge.Chunk0304304E().ReadWrite(original, rw);
        });
        var restored = await RoundTrip(payload, new CGameCtnChallenge.Chunk0304304E());
        await Assert.That(restored.AnimationTriggers).Count().IsEqualTo(1);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    public async Task Chunk03043057_ReadsVersionedRecordedPaths(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            if (version <= 1)
            {
                w.Write(1);
                WritePath(w);
                WriteTimes(w);
            }
            if (version >= 1)
            {
                w.Write(1);
                w.Write(3);
                WritePath(w);
                if (version <= 4) WriteTimes(w);
                if (version >= 3)
                {
                    w.Write(1);
                    w.Write(new Quat(0, 0, 0, 1));
                }
                if (version >= 4) w.Write(new Ident("Car", "Stadium", "Author"));
            }
        });
        var node = await RoundTrip(payload, new CGameCtnChallenge.Chunk03043057());
        if (version <= 1) await Assert.That(node.LegacyRecordedBotPaths![0].Times![0]).IsEqualTo(123u);
        if (version >= 1) await Assert.That(node.RecordedBotPaths![0].BotPathIndex).IsEqualTo(3);
        if (version == 5) await Assert.That(node.RecordedBotPaths![0].Times).IsNull();
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task Chunk0304305C_ReadsLegacyArraysOnlyInVersionZero(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            if (version == 0)
            {
                WriteTimes(w);
                WriteTimes(w);
            }
        });
        await RoundTrip(payload, new CGameCtnChallenge.Chunk0304305C());
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task Chunk0304305D_DerivesLeafSizeFromParent(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(true);
            w.Write(4);
            w.Write(new Int3(4, 4, 4));
            w.Write(3);
            w.Write(-1);
            WriteChildren(w, 1);
            w.Write(0);
            WriteChildren(w, 2);
            w.Write(1);
            w.Write((byte)73);
            if (version == 0) w.Write(1234);
        });
        var node = await RoundTrip(payload, new CGameCtnChallenge.Chunk0304305D());
        await Assert.That(node.BlockOctree!.Nodes![2].Value).IsEqualTo((byte)73);
        await Assert.That(node.BlockOctree.Nodes[2].LegacyValue).IsEqualTo(version == 0 ? 1234 : 0);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(1, 1)]
    [Arguments(2, 3)]
    [Arguments(4, 6)]
    [Arguments(5, 7)]
    [Arguments(6, 9)]
    [Arguments(7, 12)]
    [Arguments(8, 14)]
    public async Task Chunk0304305E_ReadsMacroblockArchiveVersions(int blockVersion, int itemVersion)
    {
        var payload = Payload(w =>
        {
            w.Write(1);
            w.WriteEncapsulated(inner =>
            {
                inner.Write(1); // macroblock count
                inner.Write(0); // macroblock archive version, with legacy tags
                inner.Write(1); // block count
                WriteMacroblockBlock(inner, blockVersion);
                inner.Write(1); // skin count
                inner.Write(1); // skin archive version
                inner.Write(-1); // null skin
                inner.Write(0); // block index
                inner.Write(1); // item count
                WriteMacroblockItem(inner, itemVersion);
                inner.Write(1); // legacy tag count
                inner.WriteData(Enumerable.Range(0, 16).Select(x => (byte)x).ToArray(), 16);
                inner.Write(0); // references version
                inner.Write(42);
                inner.Write(1); // reference count
                WriteEmptyNode(inner, 0x01001000);
                inner.Write(2); // reserved count without entries
            });
        });
        var node = await RoundTrip(payload, new CGameCtnChallenge.Chunk0304305E());
        await Assert.That(node.LegacyMacroblocks![0].Blocks![0].Version).IsEqualTo(blockVersion);
        await Assert.That(node.LegacyMacroblocks[0].Items![0].Version).IsEqualTo(itemVersion);
        await Assert.That(node.LegacyMacroblocks[0].Blocks![0]).IsTypeOf<CGameCtnMacroBlockInfo.BlockSpawn>();
        await Assert.That(node.LegacyMacroblocks[0].Skins![0]).IsTypeOf<CGameCtnMacroBlockInfo.BlockSkinSpawn>();
        await Assert.That(node.LegacyMacroblocks[0].Items![0]).IsTypeOf<CGameCtnMacroBlockInfo.ObjectSpawn>();
        await Assert.That(node.LegacyMacroblocks[0].Skins![0].BlockSpawnIndex).IsEqualTo(0);
        if (blockVersion < 2)
        {
            await Assert.That(node.LegacyMacroblocks[0].Blocks![0].MobilIndex).IsEqualTo(2);
            await Assert.That(node.LegacyMacroblocks[0].Blocks![0].MobilVariantIndex).IsEqualTo(3);
            await Assert.That(node.LegacyMacroblocks[0].Blocks![0].IsGround).IsTrue();
        }
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task Chunk03043061_PreservesTemporaryEditorData(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(9);
            WriteTimes(w);
            w.WriteData([5, 4, 3]);
            if (version >= 1) w.Write(17);
        });
        await RoundTrip(payload, new CGameCtnChallenge.Chunk03043061());
    }

    [Test]
    public async Task Chunk03043064_ReadsEncapsulatedClipGroups()
    {
        var payload = Payload(w =>
        {
            w.Write(0);
            w.WriteEncapsulated(inner =>
            {
                inner.Write(1);
                WriteEmptyNode(inner, 0x0307A000);
            });
        });
        var node = await RoundTrip(payload, new CGameCtnChallenge.Chunk03043064());
        await Assert.That(node.ClipGroups).Count().IsEqualTo(1);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(7)]
    public async Task Chunk03043067_ReadsPopulatedCheckpointNode(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(0);
            w.WriteEncapsulated(inner =>
            {
                inner.Write(0x03262000u);
                inner.Write(0x03262000u);
                inner.Write(version);
                if (version >= 3) inner.Write(33);
                inner.Write(1);
                if (version == 0)
                {
                    inner.Write(12);
                    inner.Write(13);
                }
                inner.Write(123u);
                if (version >= 2) inner.Write(456);
                if (version >= 1) inner.Write(789);
                inner.WriteData(new byte[12], 12);
                inner.Write(0x0A02B000u);
                var vehicleSize = version == 0 ? 422 : version <= 3 ? 574 : 578;
                inner.WriteData(new byte[vehicleSize], vehicleSize);
                var auxiliarySize = version < 5 ? 18 : 17;
                inner.WriteData(new byte[auxiliarySize], auxiliarySize);
                inner.WriteData(new byte[40], 40); // two events
                if (version >= 1)
                {
                    inner.Write(1);
                    var snapshotSize = version >= 3 ? 116 : 103;
                    inner.WriteData(new byte[snapshotSize], snapshotSize);
                    inner.Write(789u);
                    inner.Write(1); // snapshot count for the checkpoint
                }
                if (version >= 6)
                {
                    if (version == 6)
                    {
                        inner.Write(12);
                        inner.Write(13);
                    }
                    inner.Write(0);
                    inner.Write(new Ident("Car", "Stadium", "Author"));
                }
                inner.Write(0xFACADE01u);
            });
        });
        var node = await RoundTrip(payload, new CGameCtnChallenge.Chunk03043067());
        await Assert.That(node.LaunchedCheckpoints!.Checkpoints![0].State!.Time).IsEqualTo(123);
        if (version >= 1)
        {
            await Assert.That(node.LaunchedCheckpoints.Snapshots![0].Time.TotalMilliseconds).IsEqualTo(789);
            await Assert.That(node.LaunchedCheckpoints.Checkpoints[0].SnapshotCount).IsEqualTo(1);
        }
    }

    [Test]
    [Arguments(17, 78)]
    [Arguments(18, 79)]
    [Arguments(19, 80)]
    [Arguments(20, 81)]
    [Arguments(21, 85)]
    [Arguments(22, 89)]
    [Arguments(23, 93)]
    [Arguments(24, 126)]
    [Arguments(25, 142)]
    [Arguments(26, 146)]
    [Arguments(27, 150)]
    [Arguments(28, 166)]
    [Arguments(29, 170)]
    [Arguments(30, 107)]
    [Arguments(31, 111)]
    [Arguments(32, 116)]
    [Arguments(33, 120)]
    public async Task CheckpointSnapshots_PreservePackedRecordVersions(int version, int size)
    {
        var payload = Enumerable.Range(0, size).Select(x => (byte)x).ToArray();
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        var snapshot = new CGameSaveLaunchedCheckpoints.Snapshot();
        snapshot.ReadWrite(rw, version);
        await Assert.That(stream.Position).IsEqualTo((long)size);
        var restoredPayload = Payload(w =>
        {
            using var writerWriter = new GbxReaderWriter(w);
            snapshot.ReadWrite(writerWriter, version);
        });
        await Assert.That(restoredPayload.SequenceEqual(payload)).IsTrue();
    }

    private static void WriteMacroblockBlock(GbxWriter w, int version)
    {
        w.Write(version);
        w.Write(new Ident("Block", "Stadium", "Author"));
        if (version < 2)
        {
            w.Write(new Int3(1, 2, 3));
            w.Write(1);
            w.Write(2);
            w.Write(3);
            w.Write(true);
            if (version >= 1) w.Write(4);
        }
        else
        {
            if (version < 5) w.WriteData([1, 2, 3, 1], 4);
            w.Write(1 << 26);
            if (version >= 5)
            {
                w.Write(new Vec3(1, 2, 3));
                w.Write(new Vec3(4, 5, 6));
            }
        }
        if (version >= 3) w.Write(-1);
        if (version >= 4 && version <= 5) w.Write(-1);
        if (version >= 7) w.Write((byte)2);
        if (version >= 8) w.Write((byte)3);
    }

    private static void WriteMacroblockItem(GbxWriter w, int version)
    {
        w.Write(version);
        w.Write(new Ident("Item", "Stadium", "Author"));
        if (version < 3)
        {
            w.Write((byte)1);
            if (version >= 1) w.Write((byte)2);
        }
        else w.Write(new Vec3(1, 2, 3));
        w.Write(new Int3(4, 5, 6));
        w.WriteIdAsString("Anchor");
        w.Write(new Vec3(7, 8, 9));
        if (version >= 2 && version <= 4) w.Write(10);
        if (version >= 4 && version <= 6) w.Write(11);
        if (version >= 6) w.Write((short)12);
        if (version >= 7) w.Write(new Vec3(13, 14, 15));
        if (version >= 8) w.Write(-1);
        if (version >= 9) w.Write(1.5f);
        if (version >= 10) w.Write(new Int3(16, 17, 18));
        if (version >= 11) w.WriteData([1, 2], 2);
        if (version >= 12) w.Write((byte)3);
        if (version >= 13) w.WriteData([0, 0], 2);
        if (version >= 14) w.Write(19);
    }

    private static void WritePath(GbxWriter w)
    {
        w.Write(1);
        w.Write(new Vec3(1, 2, 3));
    }

    private static void WriteTimes(GbxWriter w)
    {
        w.Write(1);
        w.Write(123u);
    }

    private static void WriteChildren(GbxWriter w, uint first)
    {
        w.Write(first);
        for (var i = 1; i < 8; i++) w.Write(uint.MaxValue);
    }

    private static void WriteEmptyNode(GbxWriter w, uint classId)
    {
        w.Write(classId);
        w.Write(0xFACADE01u);
    }

    private static byte[] Payload(Action<GbxWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        write(writer);
        return stream.ToArray();
    }

    private static async Task<CGameCtnChallenge> RoundTrip(byte[] payload, Chunk<CGameCtnChallenge> chunk)
    {
        using var stream = new MemoryStream();
        stream.Write(payload);
        using (var writer = new GbxWriter(stream)) writer.Write(0x12345678);
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        var node = new CGameCtnChallenge();
        chunk.ReadWrite(node, rw);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
        var restoredPayload = Payload(w =>
        {
            using var writerWriter = new GbxReaderWriter(w);
            chunk.ReadWrite(node, writerWriter);
        });
        await Assert.That(restoredPayload.SequenceEqual(payload)).IsTrue();
        return node;
    }
}
