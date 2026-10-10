using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Engines.Game;

#pragma warning disable CS0618 // Legacy chunk behavior is covered intentionally.

[Category("Unit")]
public class CGameCtnBlockInfoClipTests
{
    private const int Sentinel = 0x12345678;

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    public async Task ReadWrite_FreeClipVersions_PreservesFlagsAndChunkBoundary(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(0); // Four-byte CanBeDeletedByFullFreeClip.
            if (version >= 1) w.Write(6); // PreviousDirOnly.
            if (version >= 2) w.Write((byte)1); // IsAlwaysVisibleFreeClip.
            if (version >= 3) w.Write((byte)0); // IsFCTOrFCBIgnoredByVFC.
            if (version >= 4) w.Write((byte)1); // IsAntiClip.
        });
        var node = new CGameCtnBlockInfoClip();
        var chunk = new CGameCtnBlockInfoClip.Chunk03053006();

        await Verify(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.CanBeDeletedByFullFreeClip).IsFalse();
        await Assert.That(node.TopBottomMultiDir).IsEqualTo(version >= 1
            ? CGameCtnBlockInfo.EMultiDir.PreviousDirOnly : CGameCtnBlockInfo.EMultiDir.SameDir);
        await Assert.That(node.IsAlwaysVisibleFreeClip).IsEqualTo(version >= 2);
        await Assert.That(node.IsFCTOrFCBIgnoredByVFC).IsFalse();
        await Assert.That(node.IsAntiClip).IsEqualTo(version >= 4);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task ReadWrite_ClipGroupVersions_PreservesEveryGroupId(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.WriteIdAsString("Group");
            w.WriteIdAsString("Symmetrical");
            if (version >= 1)
            {
                w.WriteIdAsString("Group2");
                w.WriteIdAsString("Symmetrical2");
            }
        });
        var node = new CGameCtnBlockInfoClip();
        var chunk = new CGameCtnBlockInfoClip.Chunk03053008();

        await Verify(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.ClipGroupId).IsEqualTo("Group");
        await Assert.That(node.SymmetricalClipGroupId).IsEqualTo("Symmetrical");
        await Assert.That(node.ClipGroupId2).IsEqualTo(version >= 1 ? "Group2" : null);
        await Assert.That(node.SymmetricalClipGroupId2).IsEqualTo(version >= 1 ? "Symmetrical2" : null);
        await Assert.That(chunk is ISkippableChunk).IsTrue();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadWrite_LegacyFreeClipFlag_ConvertsToClipType(bool isFreeClip)
    {
        var payload = Payload(w => w.Write(isFreeClip ? 1 : 0));
        var node = new CGameCtnBlockInfoClip { ClipType = CGameCtnBlockInfoClip.EClipType.FreeClipBottom };
        var chunk = new CGameCtnBlockInfoClip.Chunk03053003();

        await Verify(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.ClipType).IsEqualTo(isFreeClip
            ? CGameCtnBlockInfoClip.EClipType.FreeClipSide : CGameCtnBlockInfoClip.EClipType.ClassicClip);
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(0, true)]
    [Arguments(1, false)]
    [Arguments(1, true)]
    public async Task ReadWrite_LegacyClipReference_RestoresIdOnlyForNonNullReference(int offset, bool hasReference)
    {
        var referenced = new CGameCtnCollector { Ident = new Ident("SymmetricalClip", "Stadium", "Nadeo") };
        referenced.Chunks.Create<CGameCtnCollector.Chunk2E001002>();
        var payload = Payload(w => w.WriteNodeRef<CGameCtnCollector>(hasReference ? referenced : null));
        var node = new CGameCtnBlockInfoClip { SymmetricalClipId = "PreviousClip" };
        Chunk<CGameCtnBlockInfoClip> chunk = offset == 0
            ? new CGameCtnBlockInfoClip.Chunk03053000() : new CGameCtnBlockInfoClip.Chunk03053001();

        await Verify(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.SymmetricalClipId).IsEqualTo(hasReference ? "SymmetricalClip" : "PreviousClip");
        await Assert.That(node.LegacySymmetricalClip?.Ident.Id).IsEqualTo(hasReference ? "SymmetricalClip" : null);
        await Assert.That(chunk is ISkippableChunk).IsEqualTo(offset == 0);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadWrite_LegacyPassingPoint_PreservesConditionalCoordinates(bool hasPassingPoint)
    {
        var payload = Payload(w =>
        {
            w.Write(0);
            w.Write(hasPassingPoint ? 1 : 0);
            if (hasPassingPoint)
            {
                w.Write(1.25f);
                w.Write(-2.5f);
                w.Write(3.75f);
                w.Write(-4.5f);
            }
        });
        var node = new CGameCtnBlockInfoClip();
        var chunk = new CGameCtnBlockInfoClip.Chunk03053007();

        await Verify(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.HasPassingPoint).IsEqualTo(hasPassingPoint);
        await Assert.That(node.PassingPointPos).IsEqualTo(hasPassingPoint ? new Vec2(1.25f, -2.5f) : default);
        await Assert.That(node.PassingPointRoll).IsEqualTo(hasPassingPoint ? 3.75f : 0);
        await Assert.That(node.PassingPointPitch).IsEqualTo(hasPassingPoint ? -4.5f : 0);
    }

    [Test]
    public async Task Constructor_ClipDefaultsAndGameVersions_MatchNativeWriters()
    {
        var node = new CGameCtnBlockInfoClip();
        await Assert.That(node.CatalogPosition).IsEqualTo(-1);
        await Assert.That(node.CanBeDeletedByFullFreeClip).IsTrue();
        await Assert.That(new CGameCtnBlockInfoClip.Chunk03053006(GameVersion.TMT).Version).IsEqualTo(1);
        await Assert.That(new CGameCtnBlockInfoClip.Chunk03053006(GameVersion.MP4).Version).IsEqualTo(1);
        await Assert.That(new CGameCtnBlockInfoClip.Chunk03053006(GameVersion.TM2020).Version).IsEqualTo(4);
        await Assert.That(new CGameCtnBlockInfoClip.Chunk03053008(GameVersion.TM2020).Version).IsEqualTo(1);
        await Assert.That(new CGameCtnBlockInfoClip.Chunk03053007().GameVersion.HasFlag(GameVersion.TM2020)).IsFalse();

        node.ASymmetricalClipId = "LegacyName";
        await Assert.That(node.SymmetricalClipId).IsEqualTo("LegacyName");
    }

    private static byte[] Payload(Action<GbxWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        write(writer);
        writer.Write(Sentinel);
        return stream.ToArray();
    }

    private static async Task Verify(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        using var input = new MemoryStream(payload);
        using var reader = new GbxReader(input);
        using (var rw = new GbxReaderWriter(reader)) serialize(rw);
        await Assert.That(reader.ReadInt32()).IsEqualTo(Sentinel);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using (var rw = new GbxReaderWriter(writer)) serialize(rw);
        writer.Write(Sentinel);
        await Assert.That(output.ToArray().SequenceEqual(payload)).IsTrue();
    }
}
