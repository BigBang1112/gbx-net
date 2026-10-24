using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugBitmapRenderCubeMapLayoutTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    public async Task ReadWrite_Chunks_PreserveFieldOrderAndPackedFlags(int offset)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            WriteSettings(writer, offset);
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var node = new CPlugBitmapRenderCubeMap();
        var chunk = CreateChunk(offset);
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(node.CubeFaceCount).IsEqualTo(offset is 0 or 2 ? 2 : 6);
        await Assert.That(node.NearZ).IsEqualTo(offset is 0 or 2 ? 1.25f : 0.5f);
        await Assert.That(node.FarZ).IsEqualTo(offset is 0 or 2 ? 750f : 1000f);
        await Assert.That(node.MinDistToUpdate).IsEqualTo(offset == 2 ? 2.5f : 1f);
        await Assert.That(node.Flags).IsEqualTo(offset == 1 ? 0x8000005Fu : 0u);
        await Assert.That(node.BitmapImageSpace).IsNull();
        await Assert.That(node.References.BitmapImageSpace.File).IsNull();
        await Assert.That(node.BitmapImageSpaceDistToCenter).IsEqualTo(offset >= 3 ? -3.5f : 0f);
        await Assert.That(node.BitmapImageSpaceScaleHeight).IsEqualTo(offset == 4 ? 4.5f : 1f);
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
    [Arguments(4)]
    public async Task ReadNode_NodeBodies_RecognizeAllChunks(int offset)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(CPlugBitmapRenderCubeMap.Id + (uint)offset);
            WriteSettings(writer, offset);
            writer.Write(0xFACADE01u);
        }
        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var node = reader.ReadNode<CPlugBitmapRenderCubeMap>()!;
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.Chunks.Count).IsEqualTo(1);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) writer.WriteNode(node);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(3)]
    [Arguments(4)]
    public async Task ReadWrite_ImageSpaceBitmap_PreservesEmbeddedNodeReferences(int offset)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.WriteNodeRef(new CPlugBitmap());
            writer.Write(2.5f);
            if (offset == 4) writer.Write(3.5f);
        }
        payload.Position = 0;
        var node = new CPlugBitmapRenderCubeMap();
        var chunk = CreateChunk(offset);
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.BitmapImageSpace).IsNotNull();
        await Assert.That(node.References.BitmapImageSpace.File).IsNull();
        await Assert.That(node.BitmapImageSpaceDistToCenter).IsEqualTo(2.5f);
        await Assert.That(node.BitmapImageSpaceScaleHeight).IsEqualTo(offset == 4 ? 3.5f : 1f);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task Constructor_Defaults_MatchNativeConstruction()
    {
        var node = new CPlugBitmapRenderCubeMap();
        await Assert.That(node.CubeFaceCount).IsEqualTo(6);
        await Assert.That(node.NearZ).IsEqualTo(0.5f);
        await Assert.That(node.FarZ).IsEqualTo(1000f);
        await Assert.That(node.MinDistToUpdate).IsEqualTo(1f);
        await Assert.That(node.Flags).IsEqualTo(0u);
        await Assert.That(node.BitmapImageSpace).IsNull();
        await Assert.That(node.BitmapImageSpaceDistToCenter).IsEqualTo(0f);
        await Assert.That(node.BitmapImageSpaceScaleHeight).IsEqualTo(1f);
    }

    private static Chunk<CPlugBitmapRenderCubeMap> CreateChunk(int offset) => offset switch
    {
        0 => new CPlugBitmapRenderCubeMap.Chunk09088000(),
        1 => new CPlugBitmapRenderCubeMap.Chunk09088001(),
        2 => new CPlugBitmapRenderCubeMap.Chunk09088002(),
        3 => new CPlugBitmapRenderCubeMap.Chunk09088003(),
        4 => new CPlugBitmapRenderCubeMap.Chunk09088004(),
        _ => throw new ArgumentOutOfRangeException(nameof(offset))
    };

    private static void WriteSettings(GbxWriter writer, int offset)
    {
        switch (offset)
        {
            case 0:
            case 2:
                writer.Write(2);
                writer.Write(1.25f);
                writer.Write(750f);
                if (offset == 2) writer.Write(2.5f);
                break;
            case 1:
                writer.Write(0x8000005Fu);
                break;
            case 3:
            case 4:
                writer.Write(-1); // No image-space bitmap reference.
                writer.Write(-3.5f);
                if (offset == 4) writer.Write(4.5f);
                break;
        }
    }
}
