using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugCloudsParamLayoutTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadWrite_Settings_PreserveNativeFieldOrder(bool versioned)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            WriteSettings(writer, versioned);
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var node = new CPlugCloudsParam();
        CPlugCloudsParam.Chunk09182000 chunk = versioned
            ? new CPlugCloudsParam.Chunk09182002()
            : new CPlugCloudsParam.Chunk09182000();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(node.HeightCenter).IsEqualTo(CPlugCloudsParam.EHeightCenter.WorldXZ);
        await Assert.That(node.HeightCenterXZ).IsEqualTo(new Vec2(12.5f, -3.5f));
        await Assert.That(node.BottomNearZ).IsEqualTo(234f);
        await Assert.That(node.BottomFarZ).IsEqualTo(1234f);
        await Assert.That(node.SpeedScale).IsEqualTo(75.5f);
        await Assert.That(node.Lighting).IsEqualTo(CPlugCloudsParam.ELighting.PerSprite);
        await Assert.That(node.MaterialUseT3b).IsEqualTo(versioned);
        if (chunk is CPlugCloudsParam.Chunk09182002 settings)
        {
            await Assert.That(settings.Version).IsEqualTo(0);
        }
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0)]
    [Arguments(2)]
    [Arguments(4)]
    public async Task ReadWrite_HeightPoints_PreserveEverySerializedPair(int count)
    {
        var points = Enumerable.Range(0, count).Select(i => new Vec2(i * 100f, i + 0.5f)).ToArray();
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(count);
            foreach (var point in points) writer.Write(point);
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var node = new CPlugCloudsParam();
        var chunk = new CPlugCloudsParam.Chunk09182001();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(node.Points!).IsEquivalentTo(points, CollectionOrdering.Matching);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task ReadNode_NodeBodies_SupportCurrentAndLegacyChunkIds(bool legacyIds, bool versioned)
    {
        var classId = legacyIds ? 0x05046000u : CPlugCloudsParam.Id;
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            if (!versioned)
            {
                writer.Write(classId);
                WriteSettings(writer, false);
            }
            writer.Write(classId + 1);
            writer.Write(2);
            writer.Write(new Vec2(100, 200));
            writer.Write(new Vec2(500, 400));
            if (versioned)
            {
                writer.Write(classId + 2);
                WriteSettings(writer, true);
            }
            writer.Write(0xFACADE01u);
        }
        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var node = reader.ReadNode<CPlugCloudsParam>()!;
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.Chunks.Count).IsEqualTo(2);
        await Assert.That(node.HeightCenter).IsEqualTo(CPlugCloudsParam.EHeightCenter.WorldXZ);
        await Assert.That(node.Points!).IsEquivalentTo(new Vec2[] { new(100, 200), new(500, 400) }, CollectionOrdering.Matching);
        await Assert.That(node.MaterialUseT3b).IsEqualTo(versioned);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)
        {
            ClassIdRemapMode = legacyIds ? ClassIdRemapMode.Id2006 : ClassIdRemapMode.Latest
        }) writer.WriteNode(node);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task Constructor_Defaults_MatchNativeConstruction()
    {
        var node = new CPlugCloudsParam();
        await Assert.That(node.HeightCenter).IsEqualTo(CPlugCloudsParam.EHeightCenter.Camera);
        await Assert.That(node.HeightCenterXZ).IsEqualTo(Vec2.Zero);
        await Assert.That(node.BottomNearZ).IsEqualTo(1000f);
        await Assert.That(node.BottomFarZ).IsEqualTo(1000f);
        await Assert.That(BitConverter.SingleToInt32Bits(node.SpeedScale)).IsEqualTo(0x42C2624E);
        await Assert.That(node.Lighting).IsEqualTo(CPlugCloudsParam.ELighting.PerObject);
        await Assert.That(node.MaterialUseT3b).IsFalse();
    }

    private static void WriteSettings(GbxWriter writer, bool versioned)
    {
        if (versioned) writer.Write(0);
        writer.Write(1); // HeightCenter: WorldXZ.
        writer.Write(new Vec2(12.5f, -3.5f));
        writer.Write(234f);
        writer.Write(1234f);
        writer.Write(75.5f);
        writer.Write(1); // Lighting: per sprite.
        if (versioned) writer.Write(true);
    }
}
