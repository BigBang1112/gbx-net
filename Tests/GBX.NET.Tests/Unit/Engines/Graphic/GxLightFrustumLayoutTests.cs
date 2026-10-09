using GBX.NET.Engines.Graphic;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;
using System.Text;

namespace GBX.NET.Tests.Unit.Engines.Graphic;

[Category("Unit")]
public class GxLightFrustumLayoutTests
{
    [Test]
    [Arguments(0, false)]
    [Arguments(0, true)]
    [Arguments(1, false)]
    [Arguments(1, true)]
    public async Task OldFrustumChunksConvertBoundsAndLegacyBooleans(int offset, bool orthographic)
    {
        var payload = Payload(orthographic, offset == 0 ? 0x80050u : uint.MaxValue, legacyBooleans: offset == 0);
        var node = new GxLightFrustum();
        ((GxLightBall)node).Flags = 0xDEADBEEF;
        var chunk = CreateChunk(offset);

        await ReadPayload(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Frustum!.IsOrthographic).IsEqualTo(orthographic);
        await Assert.That(Geometry(node)).IsEquivalentTo(orthographic
            ? new[] { 2f, 2f, 5.5f, 4f, 6f, 5f }
            : new[] { -2f, -4f, 0.5f, 6f, 8f, 10.5f }, CollectionOrdering.Matching);
        await Assert.That(node.Flags).IsEqualTo(offset == 0 ? 0x80050u : uint.MaxValue);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.Unspecified);
        await Assert.That(WritePayload(rw => chunk.ReadWrite(node, rw))).IsEquivalentTo(payload, CollectionOrdering.Matching);
        await Assert.That(((GxLightBall)node).Flags).IsEqualTo(0xDEADBEEFu);
    }

    [Test]
    [Arguments(2, false)]
    [Arguments(2, true)]
    [Arguments(4, false)]
    [Arguments(4, true)]
    [Arguments(5, false)]
    [Arguments(5, true)]
    [Arguments(6, false)]
    [Arguments(6, true)]
    public async Task ModernGeometryKeepsAllSixValuesAndAppliesLegacyFlagMasks(int offset, bool orthographic)
    {
        var node = new GxLightFrustum();
        ((GxLightBall)node).Flags = 0xDEADBEEF;
        var chunk = CreateChunk(offset);
        var payload = Payload(orthographic, uint.MaxValue);
        var expectedFlags = offset switch
        {
            2 or 4 => 0xFFFFC7FFu,
            5 => 0xFFFC3FFFu,
            _ => uint.MaxValue
        };

        await ReadPayload(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Frustum!.IsOrthographic).IsEqualTo(orthographic);
        await Assert.That(Geometry(node)).IsEquivalentTo(new[] { -2f, -4f, 0.5f, 6f, 8f, 10.5f }, CollectionOrdering.Matching);
        await Assert.That(node.Flags).IsEqualTo(expectedFlags);
        await Assert.That(chunk.GameVersion).IsEqualTo(offset switch
        {
            4 => GameVersion.TM10 | GameVersion.TMPU,
            6 => GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020,
            _ => GameVersion.Unspecified
        });
        await Assert.That(WritePayload(rw => chunk.ReadWrite(node, rw)))
            .IsEquivalentTo(Payload(orthographic, expectedFlags), CollectionOrdering.Matching);
        await Assert.That(((GxLightBall)node).Flags).IsEqualTo(0xDEADBEEFu);
    }

    [Test]
    [Arguments(6u, 1u, 0u)]
    [Arguments(6u, 0u, 1u)]
    [Arguments(1u, 1u, 2u)]
    [Arguments(6u, 2u, 3u)]
    [Arguments(123u, 456u, 0u)]
    public async Task LegacyBlendPairsMapToNativeApplyModes(uint source, uint destination, uint mode)
    {
        var node = new GxLightFrustum();
        ((GxLightBall)node).Flags = 0xDEADBEEF;
        var chunk = new GxLightFrustum.Chunk0400A003();
        var payload = Payload(false, uint.MaxValue, source: source, destination: destination);
        var expectedFlags = 0xFFFC007Fu | (mode << 7);

        await ReadPayload(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Flags).IsEqualTo(expectedFlags);
        await Assert.That(node.LegacyBlendSource).IsEqualTo(source);
        await Assert.That(node.LegacyBlendDestination).IsEqualTo(destination);
        await Assert.That(Geometry(node)).IsEquivalentTo(new[] { -2f, -4f, 0.5f, 6f, 8f, 10.5f }, CollectionOrdering.Matching);
        await Assert.That(WritePayload(rw => chunk.ReadWrite(node, rw)))
            .IsEquivalentTo(Payload(false, expectedFlags, source: source, destination: destination), CollectionOrdering.Matching);
        await Assert.That(((GxLightBall)node).Flags).IsEqualTo(0xDEADBEEFu);
    }

    [Test]
    public async Task ConstructorUsesNativePerspectiveAndAttenuationDefaults()
    {
        var node = new GxLightFrustum();
        await Assert.That(node.Frustum).IsNotNull();
        await Assert.That(node.Frustum!.IsOrthographic).IsFalse();
        await Assert.That(Geometry(node)).IsEquivalentTo(new[] { -0.2679492f, -0.2679492f, 0.5f, 0.2679492f, 0.2679492f, 10f }, CollectionOrdering.Matching);
        await Assert.That(node.AttHTnLR).IsEqualTo(0f);
        await Assert.That(node.AttHTnLR2).IsEqualTo(0f);
        await Assert.That(node.Flags).IsEqualTo(0x80010u);
        ((GxLightBall)node).Flags = 0xDEADBEEF;
        ((GxLight)node).Flags = GxLight.EFlags.DoLighting;
        await Assert.That(node.Flags).IsEqualTo(0x80010u);
        var other = new GxLightFrustum();
        node.Frustum.X = 42;
        await Assert.That(other.Frustum!.X).IsEqualTo(-0.2679492f);
    }

    private static float[] Geometry(GxLightFrustum node)
    {
        var frustum = node.Frustum!;
        return [frustum.X, frustum.Y, frustum.Z, frustum.X2, frustum.Y2, frustum.Z2];
    }

    private static Chunk<GxLightFrustum> CreateChunk(int offset) => offset switch
    {
        0 => new GxLightFrustum.Chunk0400A000(),
        1 => new GxLightFrustum.Chunk0400A001(),
        2 => new GxLightFrustum.Chunk0400A002(),
        4 => new GxLightFrustum.Chunk0400A004(),
        5 => new GxLightFrustum.Chunk0400A005(),
        6 => new GxLightFrustum.Chunk0400A006(),
        _ => throw new ArgumentOutOfRangeException(nameof(offset))
    };

    private static byte[] Payload(bool orthographic, uint flags, bool legacyBooleans = false, uint? source = null, uint? destination = null)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        writer.Write(orthographic ? 1 : 0);
        foreach (var value in new[] { -2f, -4f, 0.5f, 6f, 8f, 10.5f })
        {
            writer.Write(value);
        }
        if (legacyBooleans)
        {
            writer.Write((flags & 0x40) != 0 ? 1 : 0);
            writer.Write((flags & 0x20) != 0 ? 1 : 0);
        }
        else
        {
            writer.Write(flags);
        }
        if (source.HasValue)
        {
            writer.Write(source.Value);
            writer.Write(destination!.Value);
        }
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
