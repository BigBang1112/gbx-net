using GBX.NET.Managers;

namespace GBX.NET.Serialization.Chunking;

/// <summary>
/// A set of header chunks.
/// </summary>
public interface IHeaderChunkSet : IChunkSet<IHeaderChunk>;

internal sealed class HeaderChunkSet : ChunkSet<IHeaderChunk>, IHeaderChunkSet
{
    public override IComparer<IHeaderChunk> Comparer => HeaderChunkComparer.Default;

    public HeaderChunkSet() : base(node: null) { }

    protected override IHeaderChunk New(uint chunkId, bool preferHeaderChunks)
    {
        return ClassManager.NewHeaderChunk(chunkId) ?? throw new ChunkNotSupportedException(chunkId);
    }
}