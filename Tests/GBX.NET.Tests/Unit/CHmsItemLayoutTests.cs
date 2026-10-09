using GBX.NET.Engines.Hms;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;
using System.Text;

namespace GBX.NET.Tests.Unit;

public class CHmsItemLayoutTests
{
    [Test]
    public async Task PackedPropertiesPreserveOtherBitsAndNativeStaticConstraints()
    {
        var node = new CHmsItem();
        await Assert.That(node.FlagsItem).IsEqualTo(0xFFF1C00018800000UL);
        await Assert.That(node.VisibleId).IsEqualTo((ushort)1);
        await Assert.That(node.IsVisible).IsTrue();
        await Assert.That(node.CanSelfShadow).IsTrue();
        await Assert.That(node.LightLensFlareEnable).IsTrue();

        node.FlagsItem = 0;
        node.ShadowReceiverGroupMask = 0xABC;
        node.LightEmitterGroupMask = 5;
        node.CountShadowTexCasted = 2;
        await Assert.That(node.ShadowCasterGroupMask).IsEqualTo(1u);
        node.ShadowCasterGroupMask = 0;
        await Assert.That(node.ShadowCasterGroupMask).IsEqualTo(1u);
        node.ShadowCasterGroupMask = 0x246;
        node.IsVisible = true;
        await Assert.That(node.FlagsItem).IsEqualTo(0xABC5824600000002UL);
        node.IsVisionStatic = true;
        node.IsStatic = false;
        await Assert.That(node.IsStatic).IsTrue();
        node.IsVisionStatic = false;
        await Assert.That(node.IsStatic).IsFalse();
        await Assert.That(node.ShadowReceiverGroupMask).IsEqualTo(0xABCu);

        node.VisibleId = 0xFFFF;
        node.VIdReflected = false;
        node.VIdInvisibleStopBounce = false;
        await Assert.That(node.VisibleId).IsEqualTo((ushort)0x7FFE);
        node.VIdInvisibleStopBounce = true;
        await Assert.That(node.VisibleId).IsEqualTo((ushort)0xFFFE);
    }

    [Test]
    [Arguments(0)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(7)]
    [Arguments(8)]
    [Arguments(9)]
    public async Task LegacyScalarPayloadsKeepWidthsAndRawFields(int offset)
    {
        var payload = Payload(w =>
        {
            if (offset != 9) w.Write(uint.MaxValue);
            if (offset >= 8)
            {
                w.Write(0x00000302u);
                if (offset == 8) w.Write(1);
                return;
            }
            if (offset >= 6) w.Write(1); // Obsolete light-emitter switch.
            if (offset >= 5) w.Write(1); // Background switch.
            w.Write(1); // Collision group.
            w.Write(0); // Contact interest.
            w.Write(1); // Dynamic type.
            if (offset == 3) w.Write(1);
            if (offset >= 4) w.Write((byte)2);
        });
        var node = new CHmsItem();
        var chunk = Chunk(offset);
        await ReadPayload(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(WritePayload(rw => chunk.ReadWrite(node, rw)))
            .IsEquivalentTo(payload, CollectionOrdering.Matching);
        if (offset is >= 3 and <= 7)
        {
            await Assert.That(node.CountShadowTexCasted).IsEqualTo((byte)2);
            await Assert.That(node.CanSelfShadow).IsTrue();
        }
        if (offset is >= 5 and <= 8) await Assert.That(node.IsBackground).IsTrue();
        if (offset == 8) await Assert.That(node.VisibleId).IsEqualTo((ushort)0);
        if (offset == 9) await Assert.That(node.ShadowCasterGroupMask).IsEqualTo(1u);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.Unspecified);
    }

    [Test]
    [Arguments(10, 2)]
    [Arguments(10, 15)]
    [Arguments(11, 11)]
    [Arguments(11, 4095)]
    public async Task ObsoleteCasterIdsBecomeMasksAndKeepRawArchives(int offset, int groupId)
    {
        var flags = ((ulong)groupId << 32) | 0xDA001234u;
        var payload = Payload(w =>
        {
            w.Write(flags);
            if (offset == 11) w.Write((ushort)0xFEDC);
        });
        var node = new CHmsItem();
        var chunk = Chunk(offset);
        await ReadPayload(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.ShadowCasterGroupMask).IsEqualTo(groupId < 12 ? 1u << groupId : 1u);
        await Assert.That(node.VisibleId).IsEqualTo(offset == 10 ? (ushort)0xDA : (ushort)0xFEDC);
        await Assert.That(node.FlagsLegacy64).IsEqualTo(flags);
        await Assert.That(WritePayload(rw => chunk.ReadWrite(node, rw)))
            .IsEquivalentTo(payload, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(12, 0xFFF1DEFA10001BFFUL)]
    [Arguments(13, 0xFFF1DEFA10001BFFUL)]
    [Arguments(14, 0xFFFADEFA10001BFFUL)]
    [Arguments(15, 0xFFFADEFA10001BFFUL)]
    [Arguments(16, 0xABCADEFA10001FFFUL)]
    [Arguments(17, 0xABCADEFA0001FFFFUL)]
    public async Task PackedFlagsApplyChunkSpecificLegacyConversions(int offset, ulong expectedFlags)
    {
        var payload = Payload(w =>
        {
            w.Write(0xABCADEFA0001FFFFUL);
            w.Write((ushort)0xFEDC);
        });
        var node = new CHmsItem();
        var chunk = Chunk(offset);
        await ReadPayload(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.FlagsItem).IsEqualTo(expectedFlags);
        await Assert.That(node.VisibleId).IsEqualTo((ushort)0xFEDC);
        if (offset < 14)
        {
            await Assert.That(WritePayload(rw => chunk.ReadWrite(node, rw)))
                .IsEquivalentTo(payload, CollectionOrdering.Matching);
        }
    }

    [Test]
    public async Task LatestWriterClearsOnlyTheTwoCurrentRuntimeBits()
    {
        var node = new CHmsItem { FlagsItem = ulong.MaxValue, VisibleId = ushort.MaxValue };
        var chunk = new CHmsItem.Chunk06003011();
        var expected = Payload(w =>
        {
            w.Write(0xFFFFFFFF3FFFFFFFUL);
            w.Write(ushort.MaxValue);
        });
        await Assert.That(WritePayload(rw => chunk.ReadWrite(node, rw)))
            .IsEquivalentTo(expected, CollectionOrdering.Matching);
        await Assert.That(node.IsStatic).IsTrue(); // Bit 29 remains serializable in Maniaplanet/TM2020.
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.TMNESWC | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020);
    }

    [Test]
    public async Task LegacyPortalArrayUsesAPlainCountAndPreservesSharedReferences()
    {
        var portal = new CHmsPortal { CanSeeThrough = true, SeeThroughOpacity = 0.75f };
        portal.CreateChunk<CHmsPortal.Chunk06006002>();
        using var input = new MemoryStream();
        using (var writer = new GbxWriter(input))
        {
            writer.Write(3); // No deprecated-array version prefix.
            writer.WriteNodeRef(portal);
            writer.WriteNodeRef<CHmsPortal>(null);
            writer.WriteNodeRef(portal);
            writer.Write(0xDEADBEEFu);
        }
        input.Position = 0;
        var node = new CHmsItem();
        var chunk = new CHmsItem.Chunk06003002();
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadUInt32()).IsEqualTo(0xDEADBEEFu);
            await Assert.That(input.Position).IsEqualTo(input.Length);
        }
        await Assert.That(node.Portals!.Length).IsEqualTo(3);
        await Assert.That(node.Portals[1]).IsNull();
        await Assert.That(node.Portals[2]).IsSameReferenceAs(node.Portals[0]);
        await Assert.That(node.Portals[0]!.SeeThroughOpacity).IsEqualTo(0.75f);
        var output = WritePayload(rw => chunk.ReadWrite(node, rw));
        await Assert.That(output).IsEquivalentTo(input.ToArray()[..^4], CollectionOrdering.Matching);
    }

    private static Chunk<CHmsItem> Chunk(int offset)
        => (Chunk<CHmsItem>)Activator.CreateInstance(typeof(CHmsItem).GetNestedType($"Chunk{0x06003000 + offset:X8}")!)!;

    private static byte[] Payload(Action<BinaryWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        write(writer);
        return stream.ToArray();
    }

    private static async Task ReadPayload(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        using var input = new MemoryStream();
        input.Write(payload);
        using var suffix = new BinaryWriter(input, Encoding.UTF8, leaveOpen: true);
        suffix.Write(0xDEADBEEFu);
        input.Position = 0;
        using var reader = new GbxReader(input);
        using var rw = new GbxReaderWriter(reader);
        serialize(rw);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(0xDEADBEEFu);
        await Assert.That(input.Position).IsEqualTo(input.Length);
    }

    private static byte[] WritePayload(Action<GbxReaderWriter> serialize)
    {
        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using var rw = new GbxReaderWriter(writer);
        serialize(rw);
        return output.ToArray();
    }
}
