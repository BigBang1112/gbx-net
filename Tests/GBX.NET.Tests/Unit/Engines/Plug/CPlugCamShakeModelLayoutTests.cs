using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugCamShakeModelLayoutTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task ReadWrite_Versions_PreserveFieldOrderAndParameterSets(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            WriteSettings(writer, version);
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var node = new CPlugCamShakeModel();
        var chunk = new CPlugCamShakeModel.Chunk0910B000();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(node.Params.Length).IsEqualTo(2);
        await Assert.That(node.Params[0].Duration.TotalMilliseconds).IsEqualTo(321);
        await Assert.That(node.Params[0].FadeDuration.TotalMilliseconds).IsEqualTo(654);
        await Assert.That(node.Params[0].PosFreq).IsEqualTo(1.25f);
        await Assert.That(node.Params[0].PosOffset).IsEqualTo(2.5f);
        await Assert.That(node.Params[0].RotFreq).IsEqualTo(3.75f);
        await Assert.That(node.Params[0].RotOffsetDeg).IsEqualTo(4.25f);
        await Assert.That(node.Params[0].ImpactIntensityMin).IsEqualTo(version >= 2 ? 0.5f : 0.75f);
        if (version >= 3)
        {
            await Assert.That(node.Params[1].Duration.TotalMilliseconds).IsEqualTo(432);
            await Assert.That(node.Params[1].FadeDuration.TotalMilliseconds).IsEqualTo(765);
            await Assert.That(node.Params[1].PosFreq).IsEqualTo(5.25f);
            await Assert.That(node.Params[1].PosOffset).IsEqualTo(6.5f);
            await Assert.That(node.Params[1].RotFreq).IsEqualTo(7.75f);
            await Assert.That(node.Params[1].RotOffsetDeg).IsEqualTo(8.25f);
            await Assert.That(node.Params[1].ImpactIntensityMin).IsEqualTo(0.625f);
        }
        else
        {
            await Assert.That(node.Params[1].Duration.TotalMilliseconds).IsEqualTo(150);
            await Assert.That(node.Params[1].PosOffset).IsEqualTo(0.05f);
            await Assert.That(node.Params[1].ImpactIntensityMin).IsEqualTo(0.75f);
        }
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task ReadNode_NodeBodies_RoundTripWithoutExtraVersionOrCountFields(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(CPlugCamShakeModel.Id);
            WriteSettings(writer, version);
            writer.Write(0xFACADE01u);
        }
        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var node = reader.ReadNode<CPlugCamShakeModel>()!;
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.Chunks.Count).IsEqualTo(1);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) writer.WriteNode(node);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task Write_NewNodes_SerializesNativeDefaultsForEachVersion(int version)
    {
        var node = new CPlugCamShakeModel();
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer))
        {
            new CPlugCamShakeModel.Chunk0910B000 { Version = version }.ReadWrite(node, rw);
        }
        using var expected = new MemoryStream();
        using (var writer = new GbxWriter(expected))
        {
            writer.Write(version);
            WriteDefaults(writer, version);
            if (version >= 3) WriteDefaults(writer, version);
        }
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task Constructor_ParameterSets_HaveIndependentNativeDefaults()
    {
        var node = new CPlugCamShakeModel();
        await Assert.That(node.Params.Length).IsEqualTo(2);
        foreach (var parameters in node.Params)
        {
            await Assert.That(parameters.Duration.TotalMilliseconds).IsEqualTo(150);
            await Assert.That(parameters.FadeDuration.TotalMilliseconds).IsEqualTo(150);
            await Assert.That(parameters.PosFreq).IsEqualTo(12f);
            await Assert.That(parameters.PosOffset).IsEqualTo(0.05f);
            await Assert.That(parameters.RotFreq).IsEqualTo(10f);
            await Assert.That(parameters.RotOffsetDeg).IsEqualTo(1f);
            await Assert.That(parameters.ImpactIntensityMin).IsEqualTo(0.75f);
        }
        node.Params[0].PosOffset = 2f;
        await Assert.That(node.Params[1].PosOffset).IsEqualTo(0.05f);
    }

    private static void WriteSettings(GbxWriter writer, int version)
    {
        writer.Write(version);
        writer.Write(321);
        writer.Write(654);
        writer.Write(1.25f);
        writer.Write(2.5f);
        writer.Write(3.75f);
        writer.Write(4.25f);
        if (version >= 2) writer.Write(0.5f);
        if (version >= 3)
        {
            writer.Write(432);
            writer.Write(765);
            writer.Write(5.25f);
            writer.Write(6.5f);
            writer.Write(7.75f);
            writer.Write(8.25f);
            writer.Write(0.625f);
        }
    }

    private static void WriteDefaults(GbxWriter writer, int version)
    {
        writer.Write(150);
        writer.Write(150);
        writer.Write(12f);
        writer.Write(0.05f);
        writer.Write(10f);
        writer.Write(1f);
        if (version >= 2) writer.Write(0.75f);
    }
}
