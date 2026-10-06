using GBX.NET.Engines.Graphic;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit;

public class GxFogLayoutTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(43)]
    public async Task DepthAndHeightSettingsKeepNativeOrder(int legacyMode)
    {
        var node = new GxFog();
        var chunk = new GxFog.Chunk04004000();
        var payload = Payload(w =>
        {
            w.Write(1);
            w.Write(legacyMode);
            foreach (var value in new[] { 10f, 500f, 2f, -20f, 100f, 0.25f, 0.75f, 0.1f, 0.9f })
            {
                w.Write(value);
            }
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Enabled).IsTrue();
        await Assert.That(node.LegacyMode).IsEqualTo(legacyMode);
        await Assert.That(node.DepthMin).IsEqualTo(10f);
        await Assert.That(node.DepthMax).IsEqualTo(500f);
        await Assert.That(node.Exponent).IsEqualTo(2f);
        await Assert.That(node.HeightYBottom).IsEqualTo(-20f);
        await Assert.That(node.HeightYTop).IsEqualTo(100f);
        await Assert.That(node.HeightMulBottom).IsEqualTo(0.25f);
        await Assert.That(node.HeightMulTop).IsEqualTo(0.75f);
        await Assert.That(node.IntensityMin).IsEqualTo(0.1f);
        await Assert.That(node.IntensityMax).IsEqualTo(0.9f);
    }

    [Test]
    public async Task ColorAndFogVersionUseIndependentVersionedChunks()
    {
        var node = new GxFog();
        var colorChunk = new GxFog.Chunk04004001();
        var fogVersionChunk = new GxFog.Chunk04004002();

        await RoundTrip(Payload(w =>
        {
            w.Write(0);
            w.Write(0.25f);
            w.Write(0.5f);
            w.Write(0.75f);
        }), rw => colorChunk.ReadWrite(node, rw));
        await RoundTrip(Payload(w =>
        {
            w.Write(0);
            w.Write(uint.MaxValue);
        }), rw => fogVersionChunk.ReadWrite(node, rw));

        await Assert.That(node.Color).IsEqualTo(new Vec3(0.25f, 0.5f, 0.75f));
        await Assert.That(node.FogVersion).IsEqualTo(-1);
        await Assert.That(colorChunk.Version).IsEqualTo(0);
        await Assert.That(fogVersionChunk.Version).IsEqualTo(0);
        await Assert.That(fogVersionChunk.GameVersion).IsEqualTo(GameVersion.TM2020);
    }

    [Test]
    public async Task DefaultsMatchNativeConstructor()
    {
        var node = new GxFog();

        await Assert.That(node.Enabled).IsTrue();
        await Assert.That(node.LegacyMode).IsEqualTo(1);
        await Assert.That(node.DepthMin).IsEqualTo(0f);
        await Assert.That(node.DepthMax).IsEqualTo(50000f);
        await Assert.That(node.Exponent).IsEqualTo(1f);
        await Assert.That(node.HeightYBottom).IsEqualTo(0f);
        await Assert.That(node.HeightYTop).IsEqualTo(1000f);
        await Assert.That(node.HeightMulBottom).IsEqualTo(1f);
        await Assert.That(node.HeightMulTop).IsEqualTo(1f);
        await Assert.That(node.IntensityMin).IsEqualTo(0f);
        await Assert.That(node.IntensityMax).IsEqualTo(1f);
        await Assert.That(node.Color).IsEqualTo(new Vec3(1, 0, 0));
        await Assert.That(node.FogVersion).IsEqualTo(0);
    }

    private static byte[] Payload(Action<BinaryWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        write(writer);
        return stream.ToArray();
    }

    private static async Task RoundTrip(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        using var input = new MemoryStream();
        input.Write(payload);
        using var suffixWriter = new BinaryWriter(input, Encoding.UTF8, leaveOpen: true);
        suffixWriter.Write(0xDEADBEEFu);
        input.Position = 0;
        using var reader = new GbxReader(input);
        using var readWrite = new GbxReaderWriter(reader);
        serialize(readWrite);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(0xDEADBEEFu);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using var writeRead = new GbxReaderWriter(writer);
        serialize(writeRead);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }
}
