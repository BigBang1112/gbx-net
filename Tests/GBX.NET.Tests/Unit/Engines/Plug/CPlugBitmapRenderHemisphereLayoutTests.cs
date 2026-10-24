using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugBitmapRenderHemisphereLayoutTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task ReadWrite_Chunks_PreserveFieldOrderAndLegacyValues(int offset)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            WriteSettings(writer, offset);
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var node = new CPlugBitmapRenderHemisphere();
        var chunk = CreateChunk(offset);
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(node.HemiLayout).IsEqualTo(CPlugBitmapRenderHemisphere.EHemiLayout._2_4_16);
        await Assert.That(node.SpecularPower0).IsEqualTo(12.5f);
        await Assert.That(node.SpecularPower1).IsEqualTo(offset == 0 ? 0f : 6.25f);
        await Assert.That(node.AutoScaleWithPower).IsEqualTo(offset >= 2);
        await Assert.That(node.UseLightDir).IsEqualTo(offset != 3);
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
    public async Task ReadNode_NodeBodies_RecognizeAllChunks(int offset)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(CPlugBitmapRenderHemisphere.Id + (uint)offset);
            WriteSettings(writer, offset);
            writer.Write(0xFACADE01u);
        }
        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var node = reader.ReadNode<CPlugBitmapRenderHemisphere>()!;
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.Chunks.Count).IsEqualTo(1);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) writer.WriteNode(node);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(GameVersion.Unspecified, 44f)]
    [Arguments(GameVersion.TMSX, 20f)]
    [Arguments(GameVersion.TMNESWC, 20f)]
    [Arguments(GameVersion.VSK5, 20f)]
    [Arguments(GameVersion.TMF, 20f)]
    public async Task Constructor_Defaults_MatchNativeConstruction(GameVersion gameVersion, float power)
    {
        var node = new CPlugBitmapRenderHemisphere(gameVersion);
        await Assert.That(node.HemiLayout).IsEqualTo(CPlugBitmapRenderHemisphere.EHemiLayout._1);
        await Assert.That(node.SpecularPower0).IsEqualTo(power);
        await Assert.That(node.SpecularPower1).IsEqualTo(0f);
        await Assert.That(node.AutoScaleWithPower).IsFalse();
        await Assert.That(node.UseLightDir).IsTrue();
    }

    private static Chunk<CPlugBitmapRenderHemisphere> CreateChunk(int offset) => offset switch
    {
        0 => new CPlugBitmapRenderHemisphere.Chunk09058000(),
        1 => new CPlugBitmapRenderHemisphere.Chunk09058001(),
        2 => new CPlugBitmapRenderHemisphere.Chunk09058002(),
        3 => new CPlugBitmapRenderHemisphere.Chunk09058003(),
        _ => throw new ArgumentOutOfRangeException(nameof(offset))
    };

    private static void WriteSettings(GbxWriter writer, int offset)
    {
        writer.Write(1); // HemiLayout._2_4_16.
        writer.Write(12.5f);
        if (offset == 0) return;
        writer.Write(6.25f);
        if (offset == 1)
        {
            for (var i = 0; i < 6; i++) writer.Write(i + 0.5f);
        }
        else
        {
            writer.Write(true);
            if (offset == 3) writer.Write(false);
        }
    }
}
