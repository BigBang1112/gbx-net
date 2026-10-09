using GBX.NET.Engines.Graphic;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit.Engines.Graphic;

[Category("Unit")]
public class GxLightSpotLayoutTests
{
    [Test]
    public async Task OldestChunkDerivesFlareAngleAndKeepsArchivedFalloff()
    {
        var payload = Payload(w => WriteFloats(w, 12, 23, 2.5f));
        var node = new GxLightSpot { Flags = 0x40 };
        var chunk = new GxLightSpot.Chunk0400B000();

        await ReadPayload(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.AngleInner).IsEqualTo(12f);
        await Assert.That(node.AngleOuter).IsEqualTo(23f);
        await Assert.That(node.AngleFlare).IsEqualTo(46f);
        await Assert.That(node.FalloffExponent).IsEqualTo(2.5f);
        await Assert.That(node.Flags).IsEqualTo(0x40u);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.TM10 | GameVersion.TMPU);

        node.AngleFlare = 123;
        await Assert.That(WritePayload(rw => chunk.ReadWrite(node, rw))).IsEquivalentTo(payload, CollectionOrdering.Matching);
        await Assert.That(node.AngleFlare).IsEqualTo(123f);
    }

    [Test]
    [Arguments(30f, 30f, false)]
    [Arguments(30f, 30.00015f, false)]
    [Arguments(30f, 30.001f, true)]
    [Arguments(0f, 0.00002f, true)]
    [Arguments(-10f, -10.00005f, false)]
    [Arguments(-10f, -10.001f, true)]
    [Arguments(1000000f, 1000005f, false)]
    [Arguments(1000000f, 1000020f, true)]
    public async Task LegacyFlareUsesNativeRelativeTolerance(float outer, float flare, bool custom)
    {
        var payload = Payload(w => WriteFloats(w, 12, outer, flare, 2.5f));
        var node = new GxLightSpot { Flags = 0x40 };
        var chunk = new GxLightSpot.Chunk0400B001();

        await ReadPayload(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Flags).IsEqualTo(custom ? 0x41u : 0x40u);
        await Assert.That(node.CustomAngleFlare).IsEqualTo(custom);
        await Assert.That(node.AngleFlare).IsEqualTo(flare);
        await Assert.That(node.FalloffExponent).IsEqualTo(2.5f);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT);

        node.Flags = 0x40;
        await Assert.That(WritePayload(rw => chunk.ReadWrite(node, rw))).IsEquivalentTo(payload, CollectionOrdering.Matching);
        await Assert.That(node.Flags).IsEqualTo(0x40u);
    }

    [Test]
    public async Task UnversionedModernChunkPreservesFlagsAndShadowAngles()
    {
        var payload = Payload(w =>
        {
            w.Write(0xDEADBEEFu);
            WriteFloats(w, 12, 23, 34, 45, 56, 2.5f);
        });
        var node = new GxLightSpot();
        var chunk = new GxLightSpot.Chunk0400B002();

        await ReadPayload(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Flags).IsEqualTo(0xDEADBEEFu);
        await Assert.That(Angles(node)).IsEquivalentTo(new[] { 12f, 23f, 34f, 45f, 56f, 2.5f }, CollectionOrdering.Matching);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4);
        await Assert.That(WritePayload(rw => chunk.ReadWrite(node, rw))).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task VersionedChunkDistinguishesLegacyWordFromSubLightGrid(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(0xDEADBEEFu);
            WriteFloats(w, 12, 23, 34, 45, 56, 2.5f);
            if (version == 0)
            {
                w.Write(-1234);
            }
            else
            {
                w.Write((byte)3);
                w.Write((byte)7);
            }
        });
        var node = new GxLightSpot();
        var chunk = new GxLightSpot.Chunk0400B003();
        await Assert.That(chunk.Version).IsEqualTo(1);

        await ReadPayload(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.TM2020);
        await Assert.That(node.Flags).IsEqualTo(0xDEADBEEFu);
        await Assert.That(Angles(node)).IsEquivalentTo(new[] { 12f, 23f, 34f, 45f, 56f, 2.5f }, CollectionOrdering.Matching);
        await Assert.That(node.SubLightCountX).IsEqualTo(version == 0 ? (byte)1 : (byte)3);
        await Assert.That(node.SubLightCountY).IsEqualTo(version == 0 ? (byte)1 : (byte)7);
        await Assert.That(node.LegacyValue).IsEqualTo(version == 0 ? -1234 : 0);
        await Assert.That(WritePayload(rw => chunk.ReadWrite(node, rw))).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }

    [Test]
    public async Task NativeDefaultsAndCustomAnglePropertiesKeepFlagGroupsSeparate()
    {
        var node = new GxLightSpot();
        await Assert.That(Angles(node)).IsEquivalentTo(new[] { 30f, 45f, 45f, 30f, 45f, 1f }, CollectionOrdering.Matching);
        await Assert.That(node.SubLightCountX).IsEqualTo((byte)1);
        await Assert.That(node.SubLightCountY).IsEqualTo((byte)1);
        await Assert.That(node.Flags).IsEqualTo(0u);
        await Assert.That(node.LightMapOnly).IsTrue();
        await Assert.That(((GxLight)node).Flags).IsEqualTo(GxLight.EFlags.LightMapOnly);

        ((GxLightBall)node).Flags = 0xDEADBEEF;
        node.Flags = 0x40;
        node.CustomAngleFlare = true;
        node.CustomAngleShadow = true;
        await Assert.That(node.Flags).IsEqualTo(0x43u);
        node.CustomAngleFlare = false;
        await Assert.That(node.Flags).IsEqualTo(0x42u);
        await Assert.That(node.CustomAngleShadow).IsTrue();
        node.CustomAngleShadow = false;
        await Assert.That(node.Flags).IsEqualTo(0x40u);
        await Assert.That(((GxLightBall)node).Flags).IsEqualTo(0xDEADBEEFu);
        await Assert.That(((GxLight)node).Flags).IsEqualTo(GxLight.EFlags.LightMapOnly);
    }

    private static float[] Angles(GxLightSpot node) =>
        [node.AngleInner, node.AngleOuter, node.AngleFlare, node.AngleInnerShadow, node.AngleOuterShadow, node.FalloffExponent];

    private static void WriteFloats(BinaryWriter writer, params float[] values)
    {
        foreach (var value in values)
        {
            writer.Write(value);
        }
    }

    private static byte[] Payload(Action<BinaryWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        write(writer);
        return stream.ToArray();
    }

    private static async Task ReadPayload(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        using var stream = new MemoryStream();
        stream.Write(payload);
        using var suffixWriter = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        suffixWriter.Write(0xDEADBEEFu);
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        serialize(rw);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(0xDEADBEEFu);
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
    }

    private static byte[] WritePayload(Action<GbxReaderWriter> serialize)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        using var rw = new GbxReaderWriter(writer);
        serialize(rw);
        return stream.ToArray();
    }
}
