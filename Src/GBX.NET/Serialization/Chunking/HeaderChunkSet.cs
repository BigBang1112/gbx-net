using GBX.NET.Managers;

namespace GBX.NET.Serialization.Chunking;

/// <summary>
/// A set of header chunks.
/// </summary>
public interface IHeaderChunkSet : IChunkSet<IHeaderChunk>;

internal sealed class HeaderChunkSet : ChunkSet<IHeaderChunk>, IHeaderChunkSet
{
    public HeaderChunkSet() : base() { }

    protected override IHeaderChunk New(uint chunkId)
    {
        return ClassManager.NewHeaderChunk(chunkId) ?? throw new Exception($"Chunk 0x{chunkId:X8} is not supported.");
    }
}