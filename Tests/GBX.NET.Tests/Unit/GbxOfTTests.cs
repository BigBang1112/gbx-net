using GBX.NET.Components;
using GBX.NET.Engines.Game;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit;

public class GbxOfTTests
{
    [Test]
    public async Task ConstructorPreservesTypedNodeAndBasicHeader()
    {
        var node = new CGameCtnMediaClip();
        var basic = GbxHeaderBasic.Create(mode: GbxMode.Editor);
        var gbx = new Gbx<CGameCtnMediaClip>(node, basic);
        CGameCtnMediaClip convertedNode = gbx;

        await Assert.That(gbx.Node).IsSameReferenceAs(node);
        await Assert.That(convertedNode).IsSameReferenceAs(node);
        await Assert.That(gbx.Header.Basic).IsEqualTo(basic);
        await Assert.That(gbx.Header.ClassId).IsEqualTo(CGameCtnMediaClip.Id);
    }

    [Test]
    [Arguments(GbxCompression.Compressed)]
    [Arguments(GbxCompression.Uncompressed)]
    public async Task SaveAndParsePreserveUnknownChunkPayload(GbxCompression compression)
    {
        const uint chunkId = 0x030790FE;
        byte[] payload = [0, 1, 255, 83, 75, 73, 80];
        var node = new CGameCtnMediaClip();
        node.Chunks.Add(new SkippableChunk(chunkId) { Data = payload });
        var gbx = new Gbx<CGameCtnMediaClip>(node) { BodyCompression = compression };
        using var stream = new MemoryStream();

        gbx.Save(stream);
        stream.Position = 0;
        var parsed = Gbx.Parse<CGameCtnMediaClip>(stream);
        var chunk = (await Assert.That(parsed.Node.Chunks.Get(chunkId)).IsTypeOf<SkippableChunk>())!;

        await Assert.That(parsed.BodyCompression).IsEqualTo(compression);
        await Assert.That(chunk.Data!).IsEquivalentTo(payload, CollectionOrdering.Matching);
        await Assert.That(stream.CanRead).IsTrue();

        using var saved = new MemoryStream();
        parsed.Save(saved);
        await Assert.That(saved.ToArray()).IsEquivalentTo(stream.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(GbxCompression.Compressed)]
    [Arguments(GbxCompression.Uncompressed)]
    public async Task SaveAndParsePreserveMultipleUnknownChunksIncludingEmptyPayload(GbxCompression compression)
    {
        const uint emptyChunkId = 0x030790FD;
        const uint largeChunkId = 0x030790FE;
        byte[] payload = Enumerable.Range(0, 4096).Select(i => (byte)i).ToArray();
        var node = new CGameCtnMediaClip();
        node.Chunks.Add(new SkippableChunk(emptyChunkId) { Data = [] });
        node.Chunks.Add(new SkippableChunk(largeChunkId) { Data = payload });
        var gbx = new Gbx<CGameCtnMediaClip>(node) { BodyCompression = compression };
        using var stream = new MemoryStream();

        gbx.Save(stream);
        stream.Position = 0;
        var parsed = Gbx.Parse<CGameCtnMediaClip>(stream);
        var chunks = parsed.Node.Chunks.ToArray();

        await Assert.That(chunks.Select(x => x.Id).ToArray()).IsEquivalentTo(new[] { emptyChunkId, largeChunkId }, CollectionOrdering.Matching);
        await Assert.That(((SkippableChunk)chunks[0]).Data!).IsEmpty();
        await Assert.That(((SkippableChunk)chunks[1]).Data!).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }

    [Test]
    public async Task DeepCloneCopiesTypedNodeHeaderAndChunkPayload()
    {
        const uint chunkId = 0x030790FE;
        var node = new CGameCtnMediaClip();
        node.Chunks.Add(new SkippableChunk(chunkId) { Data = [1, 2, 3] });
        var original = new Gbx<CGameCtnMediaClip>(node)
        {
            FilePath = "original.Clip.Gbx",
            BodyCompression = GbxCompression.Uncompressed
        };

#pragma warning disable GBXNET10001
        var clone = original.DeepClone();
#pragma warning restore GBXNET10001
        var clonedChunk = (SkippableChunk)clone.Node.Chunks.Get(chunkId)!;
        clonedChunk.Data![0] = 99;
        clone.BodyCompression = GbxCompression.Compressed;

        await Assert.That(clone).IsNotSameReferenceAs(original);
        await Assert.That(clone.Node).IsNotSameReferenceAs(original.Node);
        await Assert.That(clone.Header).IsNotSameReferenceAs(original.Header);
        await Assert.That(clone.FilePath).IsEqualTo(original.FilePath);
        await Assert.That(((SkippableChunk)original.Node.Chunks.Get(chunkId)!).Data!).IsEquivalentTo(new byte[] { 1, 2, 3 }, CollectionOrdering.Matching);
        await Assert.That(original.BodyCompression).IsEqualTo(GbxCompression.Uncompressed);
    }
}
