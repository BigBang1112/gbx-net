using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugFxWindOnDecalLayoutTests
{
    [Test]
    public async Task Defaults_MatchNativeConstructor()
    {
        var node = new CPlugFxWindOnDecal();

        await Assert.That(node.Amplitude).IsEqualTo(0.1f);
        await Assert.That(node.OscillationBlend).IsEqualTo(0.5f);
        await Assert.That(node.OscillationFrequency1).IsEqualTo(0.5f);
        await Assert.That(node.OscillationFrequency2).IsEqualTo(1.88f);
        await Assert.That(node.SpatialVariation).IsEqualTo(0f);
        await Assert.That(node.SpatialPeriod).IsEqualTo(10f);
        await Assert.That(node.WindSpeed).IsEqualTo(1f);
        await Assert.That(node.WindDirection).IsEqualTo(0.125f);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadWrite_000_PreservesVersionEightFloatsAndBoundary(bool outsideRendererLimits)
    {
        // The first set comes from TM2020's Stormy.FxWindOnDecal.Gbx.
        float[] values = outsideRendererLimits
            ? [-0.1f, 1.5f, -0.5f, 2.75f, -0.25f, 0f, -7.5f, 1.125f]
            : [0.06f, 0.8f, 0.25f, 0.9f, 0.95f, 50f, 7f, 0.12f];
        using var input = new MemoryStream();
        using (var writer = new GbxWriter(input))
        {
            writer.Write(0);
            foreach (var value in values) writer.Write(value);
            writer.Write(0x11223344);
        }
        var payload = input.ToArray()[..36];
        input.Position = 0;
        var node = new CPlugFxWindOnDecal();
        var chunk = new CPlugFxWindOnDecal.Chunk090D9000();
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
            await Assert.That(input.Position).IsEqualTo(input.Length);
        }

        await Assert.That(chunk.Version).IsEqualTo(0);
        await Assert.That(node.Amplitude).IsEqualTo(values[0]);
        await Assert.That(node.OscillationBlend).IsEqualTo(values[1]);
        await Assert.That(node.OscillationFrequency1).IsEqualTo(values[2]);
        await Assert.That(node.OscillationFrequency2).IsEqualTo(values[3]);
        await Assert.That(node.SpatialVariation).IsEqualTo(values[4]);
        await Assert.That(node.SpatialPeriod).IsEqualTo(values[5]);
        await Assert.That(node.WindSpeed).IsEqualTo(values[6]);
        await Assert.That(node.WindDirection).IsEqualTo(values[7]);

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }
}
