using GBX.NET.Engines.Game;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit;

public class CGameCtnMediaBlockEntityTests
{
    [Test]
    public async Task TimeScaleDefaultsToNormalPlayback()
    {
        await Assert.That(new CGameCtnMediaBlockEntity().TimeScale).IsEqualTo(1f);
    }

    [Test]
    [Arguments(0f)]
    [Arguments(0.5f)]
    [Arguments(2.5f)]
    public async Task TimeScaleChunkReadsNativePayloadAndPreservesFollowingChunk(float timeScale)
    {
        const uint chunkId = 0x0329F003;
        const uint followingChunkId = 0x0329F0FE;
        byte[] followingPayload = [0x12, 0x34, 0x56, 0x78];
        var node = new CGameCtnMediaBlockEntity();
        node.Chunks.Add(new SkippableChunk(chunkId) { Data = BitConverter.GetBytes(timeScale) });
        node.Chunks.Add(new SkippableChunk(followingChunkId) { Data = followingPayload });
        var gbx = new Gbx<CGameCtnMediaBlockEntity>(node) { BodyCompression = GbxCompression.Uncompressed };
        using var input = new MemoryStream();
        gbx.Save(input);
        input.Position = 0;

        var parsed = Gbx.Parse<CGameCtnMediaBlockEntity>(input);

        await Assert.That(parsed.Node.Chunks.Get(chunkId)).IsTypeOf<CGameCtnMediaBlockEntity.Chunk0329F003>();
        await Assert.That(parsed.Node.TimeScale).IsEqualTo(timeScale);
        var followingChunk = (SkippableChunk)parsed.Node.Chunks.Get(followingChunkId)!;
        await Assert.That(followingChunk.Data!).IsEquivalentTo(followingPayload, CollectionOrdering.Matching);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        parsed.Save(output);
        await Assert.That(output.ToArray()).IsEquivalentTo(input.ToArray(), CollectionOrdering.Matching);
    }
}
