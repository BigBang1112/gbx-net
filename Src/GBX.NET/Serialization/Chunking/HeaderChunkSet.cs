namespace GBX.NET.Serialization.Chunking;

/// <summary>
/// A set of header chunks.
/// </summary>
public interface IHeaderChunkSet : IChunkSet<IHeaderChunk>;

internal sealed class HeaderChunkSet : ChunkSet<IHeaderChunk>, IHeaderChunkSet
{
    public HeaderChunkSet() : base() { }
}