using GBX.NET.Engines.Game;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit;

public class GbxOfTTests
{
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
}
