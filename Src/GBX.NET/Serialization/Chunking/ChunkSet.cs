using GBX.NET.Managers;

namespace GBX.NET.Serialization.Chunking;

/// <summary>
/// A set of body chunks.
/// </summary>
public interface IChunkSet : ISet<IChunk>
{
    /// <summary>
    /// Creates a new chunk using the ID.
    /// </summary>
    /// <param name="chunkId">ID of the chunk.</param>
    /// <returns>A new chunk instance.</returns>
    IChunk Create(uint chunkId);

    /// <summary>
    /// Creates a new chunk using the chunk type.
    /// </summary>
    /// <typeparam name="T">Type of the chunk.</typeparam>
    /// <returns>A new chunk instance.</returns>
    T Create<T>() where T : IChunk, new();

    /// <summary>
    /// Removes a chunk using the ID.
    /// </summary>
    /// <param name="chunkId">ID of the chunk.</param>
    /// <returns>True if the chunk was successfully removed, otherwise false.</returns>
    bool Remove(uint chunkId);

    /// <summary>
    /// Removes a chunk using the chunk type.
    /// </summary>
    /// <typeparam name="T">Type of the chunk.</typeparam>
    /// <returns>True if the chunk was successfully removed, otherwise false.</returns>
    bool Remove<T>() where T : IChunk;

    /// <summary>
    /// Gets a chunk using the ID.
    /// </summary>
    /// <param name="chunkId">ID of the chunk.</param>
    /// <returns>A new chunk instance if available, otherwise null.</returns>
    IChunk? Get(uint chunkId);

    /// <summary>
    /// Gets a chunk using the chunk type.
    /// </summary>
    /// <typeparam name="T">Type of the chunk.</typeparam>
    /// <returns>A new chunk instance if available, otherwise null.</returns>
    T? Get<T>() where T : IChunk;
}

internal sealed class ChunkSet : SortedSet<IChunk>, IChunkSet
{
    internal ChunkSet(int capacity) : base(ChunkIdComparer.Default) { }
    internal ChunkSet() : base(ChunkIdComparer.Default) { }

    public IChunk Create(uint chunkId)
    {
        var chunk = ClassManager.NewChunk(chunkId) ?? ClassManager.NewHeaderChunk(chunkId) ?? throw new Exception($"Chunk 0x{chunkId:X8} is not supported.");

        Add(chunk);

        return chunk;
    }

    public T Create<T>() where T : IChunk, new()
    {
        var chunk = new T();

        Add(chunk);

        return chunk;
    }

    public bool Remove(uint chunkId)
    {
        return RemoveWhere(x => x.Id == chunkId) > 0;
    }

    public bool Remove<T>() where T : IChunk
    {
        return RemoveWhere(x => x is T) > 0;
    }

    public IChunk? Get(uint chunkId)
    {
        foreach (var chunk in this)
        {
            if (chunk.Id == chunkId)
            {
                return chunk;
            }
        }

        return null;
    }

    public T? Get<T>() where T : IChunk
    {
        foreach (var chunk in this)
        {
            if (chunk is T c)
            {
                return c;
            }
        }

        return default;
    }
}
