using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Serialization.Chunking;

[Category("Unit")]
public class ChunkSetTests
{
    [Test]
    public async Task CreateAndTryCreateShareTheSameInstanceAcrossLookups()
    {
        var chunks = new ChunkSet(null);
        var chunk = chunks.Create<CGameCtnCollector.Chunk2E00100E>();

        await Assert.That(chunks.Create(chunk.Id)).IsSameReferenceAs(chunk);
        await Assert.That(chunks.Create<CGameCtnCollector.Chunk2E00100E>()).IsSameReferenceAs(chunk);
        await Assert.That(chunks.TryCreate<CGameCtnCollector.Chunk2E00100E>(out var byType)).IsFalse();
        await Assert.That(chunks.TryCreate(chunk.Id, false, out var byId)).IsFalse();
        await Assert.That(byType).IsSameReferenceAs(chunk);
        await Assert.That(byId).IsSameReferenceAs(chunk);
        await Assert.That(chunks.Count).IsEqualTo(1);
    }

    [Test]
    public async Task EnumerationOrdersBaseClassChunksBeforeDerivedClassChunks()
    {
        var chunks = new ChunkSet(null);
        var derivedChunk = chunks.Create<CGameCtnMacroBlockInfo.Chunk0310D000>();
        var baseChunk = chunks.Create<CGameCtnCollector.Chunk2E00100E>();

        await Assert.That(chunks.ToArray()).IsEquivalentTo(new IChunk[] { baseChunk, derivedChunk }, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments("id")]
    [Arguments("type")]
    [Arguments("instance")]
    public async Task RemoveClearsAllLookupsAndAllowsRecreation(string lookup)
    {
        var chunks = new ChunkSet(null);
        var chunk = chunks.Create<CGameCtnCollector.Chunk2E00100E>();
        var removed = lookup switch
        {
            "id" => chunks.Remove(chunk.Id),
            "type" => chunks.Remove<CGameCtnCollector.Chunk2E00100E>(),
            _ => chunks.Remove(chunk)
        };

        await Assert.That(removed).IsTrue();
        await Assert.That(chunks.Count).IsEqualTo(0);
        await Assert.That(chunks.Get(chunk.Id)).IsNull();
        await Assert.That(chunks.Get<CGameCtnCollector.Chunk2E00100E>()).IsNull();
        await Assert.That(chunks.TryCreate<CGameCtnCollector.Chunk2E00100E>(out var replacement)).IsTrue();
        await Assert.That(replacement).IsNotSameReferenceAs(chunk);
    }

    [Test]
    public async Task ClearRemovesKnownAndUnknownChunksFromAllLookups()
    {
        var chunks = new ChunkSet(null);
        var known = chunks.Create<CGameCtnCollector.Chunk2E00100E>();
        var unknown = new SkippableChunk(0x2E001FFF) { Data = [1, 2, 3] };
        chunks.Add(unknown);

        chunks.Clear();

        await Assert.That(chunks.Count).IsEqualTo(0);
        await Assert.That(chunks.Get(known.Id)).IsNull();
        await Assert.That(chunks.Get(unknown.Id)).IsNull();
        await Assert.That(chunks.Get<SkippableChunk>()).IsNull();
        await Assert.That(chunks.Get<CGameCtnCollector.Chunk2E00100E>()).IsNull();
    }

    [Test]
    public async Task CloningUnknownChunkCopiesItsPayload()
    {
        var original = new SkippableChunk(0x2E001FFF) { Data = [1, 2, 3] };
        var clone = original.DeepClone();
        clone.Data![0] = 99;

        await Assert.That(clone.Id).IsEqualTo(original.Id);
        await Assert.That(original.Data!).IsEquivalentTo(new byte[] { 1, 2, 3 }, CollectionOrdering.Matching);
        await Assert.That(clone.Data).IsEquivalentTo(new byte[] { 99, 2, 3 }, CollectionOrdering.Matching);
    }
}
