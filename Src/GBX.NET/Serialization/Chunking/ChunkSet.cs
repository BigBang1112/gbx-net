namespace GBX.NET.Serialization.Chunking;

/// <summary>
/// A set of body chunks.
/// </summary>
public interface IChunkSet : IChunkSet<IChunk>;

internal sealed class ChunkSet : ChunkSet<IChunk>, IChunkSet
{
    public ChunkSet() : base() { }
}
