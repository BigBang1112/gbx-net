using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugVisualIndexedLinesLayoutTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(-1)]
    public async Task RenderMode_EmbeddedChunk_PreservesAll32Bits(int mode)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0x09009001u);
            writer.Write(mode);
            writer.Write(0xFACADE01u);
        }

        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var node = reader.ReadNode<CPlugVisualIndexedLines>()!;
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That((int)node.RenderMode).IsEqualTo(mode);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) writer.WriteNode(node);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task LegacyIndices_EmbeddedChunk_UsesSharedIndexBuffer(bool empty)
    {
        ushort[] indices = empty ? [] : [0, 32768, 65535, 42];
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0x09009000u);
            writer.Write(indices.Length);
            foreach (var index in indices) writer.Write(index);
            writer.Write(0x09009001u);
            writer.Write(2);
            writer.Write(0xFACADE01u);
        }

        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var node = reader.ReadNode<CPlugVisualIndexedLines>()!;
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.IndexBuffer!.Indices).IsEquivalentTo(indices.Select(x => (int)x).ToArray(), CollectionOrdering.Matching);
        await Assert.That(node.RenderMode).IsEqualTo(CPlugVisualIndexedLines.ERenderMode.WideScreen);
        await Assert.That(node.GetChunk<CPlugVisualIndexedLines.Chunk09009000>()!.GameVersion).IsEqualTo(GameVersion.Unspecified);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) writer.WriteNode(node);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }
}
