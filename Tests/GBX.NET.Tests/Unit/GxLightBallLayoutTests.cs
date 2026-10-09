using GBX.NET.Engines.Graphic;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;
using System.Text;

namespace GBX.NET.Tests.Unit;

public class GxLightBallLayoutTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    public async Task LegacyChunksConvertCoefficientsAndKeepNativeFieldOrder(int offset)
    {
        var node = new GxLightBall();
        var chunk = LegacyChunk(offset);
        var payload = LegacyPayload(offset, uint.MaxValue, 0.25f, 0.5f);

        await ReadPayload(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Radius).IsEqualTo(8f);
        await Assert.That(node.Attenuation1).IsEqualTo(0.25f);
        await Assert.That(node.Attenuation2).IsEqualTo(0.5f);
        await Assert.That(node.AttHTnLR).IsEqualTo(2f);
        await Assert.That(node.AttHTnLR2).IsEqualTo(32f);
        await Assert.That(node.RadiusSpecular).IsEqualTo(offset == 2 ? 8f : offset >= 3 ? 6f : 10f);
        await Assert.That(node.EmittingRadius).IsEqualTo(offset >= 1 ? 0.5f : 0f);
        await Assert.That(node.AmbientRGB).IsEqualTo(offset >= 2 ? new Vec3(1, 2, 3) : default);
        await Assert.That(node.RadiusShadow).IsEqualTo(offset >= 4 ? 9f : 10f);
        await Assert.That(node.RadiusFlare).IsEqualTo(offset >= 5 ? 32f : 40f);

        var expectedFlags = offset switch
        {
            3 => 0x11u,
            4 => 0xFFFFFFC3u,
            5 => 0xFFFFFFC7u,
            6 => uint.MaxValue,
            _ => 0x10u
        };
        await Assert.That(node.Flags).IsEqualTo(expectedFlags);
        await Assert.That(chunk.GameVersion).IsEqualTo(offset switch
        {
            2 => GameVersion.TM10 | GameVersion.TMPU,
            6 => GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.VSK5 | GameVersion.TMF,
            _ => GameVersion.Unspecified
        });

        // Writing retains the original coefficients rather than converting the normalized values again.
        node.AttHTnLR = 17;
        node.AttHTnLR2 = 43;
        var rewritten = WritePayload(rw => chunk.ReadWrite(node, rw));
        await Assert.That(rewritten).IsEquivalentTo(LegacyPayload(offset, expectedFlags, 0.25f, 0.5f), CollectionOrdering.Matching);
        await Assert.That(node.AttHTnLR).IsEqualTo(17f);
        await Assert.That(node.AttHTnLR2).IsEqualTo(43f);
    }

    [Test]
    [Arguments(-1f, false)]
    [Arguments(-0.5f, false)]
    [Arguments(-0.25f, true)]
    public async Task LegacyCoefficientSentinelControlsConversion(float linear, bool converts)
    {
        var node = new GxLightBall { AttHTnLR = 17, AttHTnLR2 = 43 };
        var chunk = new GxLightBall.Chunk04002006();
        var payload = LegacyPayload(6, 0x10, linear, 0.5f);

        await ReadPayload(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.AttHTnLR).IsEqualTo(converts ? -2f : 17f);
        await Assert.That(node.AttHTnLR2).IsEqualTo(converts ? 32f : 43f);
        await Assert.That(WritePayload(rw => chunk.ReadWrite(node, rw))).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(10f, 10f, false)]
    [Arguments(10f, 10.00005f, false)]
    [Arguments(10f, 10.001f, true)]
    [Arguments(0f, 0.00002f, true)]
    [Arguments(-10f, -10.00005f, false)]
    [Arguments(-10f, -10.001f, true)]
    [Arguments(1000000f, 1000005f, false)]
    [Arguments(1000000f, 1000020f, true)]
    public async Task LegacySpecularRadiusUsesRelativeTolerance(float radius, float specularRadius, bool custom)
    {
        var payload = Payload(w =>
        {
            foreach (var value in new[] { radius, specularRadius, 0f, -1f, -1f, 0f, 0f, 0f })
            {
                w.Write(value);
            }
        });
        var node = new GxLightBall();
        var chunk = new GxLightBall.Chunk04002003();

        await ReadPayload(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Flags).IsEqualTo(custom ? 0x11u : 0x10u);
        await Assert.That(WritePayload(rw => chunk.ReadWrite(node, rw))).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(7)]
    [Arguments(8)]
    public async Task ModernChunksKeepNormalizedAttenuationAndPackedFlags(int offset)
    {
        var payload = Payload(w =>
        {
            w.Write(0xDEADBEEFu);
            foreach (var value in new[] { 8f, 6f, 9f, 32f, 0.5f })
            {
                w.Write(value);
            }
            if (offset == 8)
            {
                w.Write(12f);
            }
            foreach (var value in new[] { 2f, 32f, 1f, 2f, 3f, -2f, 0.75f })
            {
                w.Write(value);
            }
        });
        var node = new GxLightBall();
        Chunk<GxLightBall> chunk = offset == 7 ? new GxLightBall.Chunk04002007() : new GxLightBall.Chunk04002008();

        await ReadPayload(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Flags).IsEqualTo(0xDEADBEEFu);
        await Assert.That(node.Radius).IsEqualTo(8f);
        await Assert.That(node.RadiusSpecular).IsEqualTo(6f);
        await Assert.That(node.RadiusShadow).IsEqualTo(9f);
        await Assert.That(node.RadiusFlare).IsEqualTo(32f);
        await Assert.That(node.EmittingRadius).IsEqualTo(0.5f);
        await Assert.That(node.EmittingCylinderLenZ).IsEqualTo(offset == 8 ? 12f : 0f);
        await Assert.That(node.AttHTnLR).IsEqualTo(2f);
        await Assert.That(node.AttHTnLR2).IsEqualTo(32f);
        await Assert.That(node.AmbientRGB).IsEqualTo(new Vec3(1, 2, 3));
        await Assert.That(node.AttHyper2DerivAt0).IsEqualTo(-2f);
        await Assert.That(node.AttHyper2Tension).IsEqualTo(0.75f);
        await Assert.That(WritePayload(rw => chunk.ReadWrite(node, rw))).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }

    [Test]
    public async Task SeparateRadiusIndexAndHdrCoefficientChunksKeepTheirValues()
    {
        var node = new GxLightBall();
        var indexChunk = new GxLightBall.Chunk04002009();
        var hdrChunk = new GxLightBall.Chunk0400200A();
        var indexPayload = Payload(w => w.Write(123f));
        var hdrPayload = Payload(w => w.Write(0.125f));

        await ReadPayload(indexPayload, rw => indexChunk.ReadWrite(node, rw));
        await ReadPayload(hdrPayload, rw => hdrChunk.ReadWrite(node, rw));
        await Assert.That(node.RadiusIndex).IsEqualTo(123f);
        await Assert.That(node.AttHdrRadiusCoef).IsEqualTo(0.125f);
        await Assert.That(WritePayload(rw => indexChunk.ReadWrite(node, rw))).IsEquivalentTo(indexPayload, CollectionOrdering.Matching);
        await Assert.That(WritePayload(rw => hdrChunk.ReadWrite(node, rw))).IsEquivalentTo(hdrPayload, CollectionOrdering.Matching);
    }

    [Test]
    public async Task NativeDefaultsKeepBallFlagsSeparateFromBaseFlags()
    {
        var node = new GxLightBall();

        await Assert.That(node.Flags).IsEqualTo(0x10u);
        await Assert.That(node.Radius).IsEqualTo(10f);
        await Assert.That(node.RadiusSpecular).IsEqualTo(10f);
        await Assert.That(node.RadiusShadow).IsEqualTo(10f);
        await Assert.That(node.RadiusIndex).IsEqualTo(10f);
        await Assert.That(node.RadiusFlare).IsEqualTo(40f);
        await Assert.That(node.EmittingRadius).IsEqualTo(0f);
        await Assert.That(node.EmittingCylinderLenZ).IsEqualTo(0f);
        await Assert.That(node.AmbientRGB).IsEqualTo(default(Vec3));
        await Assert.That(node.Attenuation1).IsEqualTo(-1f);
        await Assert.That(node.Attenuation2).IsEqualTo(-1f);
        await Assert.That(node.AttHTnLR).IsEqualTo(10f);
        await Assert.That(node.AttHTnLR2).IsEqualTo(0f);
        await Assert.That(node.AttHyper2DerivAt0).IsEqualTo(-1.5f);
        await Assert.That(node.AttHyper2Tension).IsEqualTo(0.3f);
        await Assert.That(node.AttHdrRadiusCoef).IsEqualTo(0.015625f);

        ((GxLight)node).Flags = GxLight.EFlags.DoLighting;
        node.Flags = 0xDEADBEEF;
        await Assert.That(node.Flags).IsEqualTo(0xDEADBEEFu);
        node.Flags = 0x10;
        await Assert.That(((GxLight)node).Flags).IsEqualTo(GxLight.EFlags.DoLighting);
    }

    private static Chunk<GxLightBall> LegacyChunk(int offset) => offset switch
    {
        0 => new GxLightBall.Chunk04002000(),
        1 => new GxLightBall.Chunk04002001(),
        2 => new GxLightBall.Chunk04002002(),
        3 => new GxLightBall.Chunk04002003(),
        4 => new GxLightBall.Chunk04002004(),
        5 => new GxLightBall.Chunk04002005(),
        6 => new GxLightBall.Chunk04002006(),
        _ => throw new ArgumentOutOfRangeException(nameof(offset))
    };

    private static byte[] LegacyPayload(int offset, uint flags, float linear, float quadratic) => Payload(w =>
    {
        if (offset >= 4)
        {
            w.Write(flags);
        }
        w.Write(8f);
        if (offset >= 3)
        {
            w.Write(6f);
        }
        if (offset >= 4)
        {
            w.Write(9f);
        }
        if (offset >= 5)
        {
            w.Write(32f);
        }
        if (offset >= 3)
        {
            w.Write(0.5f);
        }
        w.Write(linear);
        w.Write(quadratic);
        if (offset is 1 or 2)
        {
            w.Write(0.5f);
        }
        if (offset >= 2)
        {
            w.Write(1f);
            w.Write(2f);
            w.Write(3f);
        }
    });

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
