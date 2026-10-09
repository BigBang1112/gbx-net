using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Serialization.Chunking;

[Category("Unit")]
public class ChunkComparerTests
{
    private readonly ChunkComparer<IChunk> chunkComparer = new();

    [Test]
    public async Task CollectorChunkAndMacroBlockChunk_CollectorChunkIsFirst()
    {
        var collectorChunk = new CGameCtnCollector.Chunk2E00100E();
        var macroBlockChunk = new CGameCtnMacroBlockInfo.Chunk0310D000();

        var result = chunkComparer.Compare(collectorChunk, macroBlockChunk);

        await Assert.That(result < 0).IsTrue().Because("Expected collector chunk to be ordered before macroblock chunk.");
    }
}
