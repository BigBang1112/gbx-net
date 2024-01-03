namespace GBX.NET;

/// <summary>
/// A set of header chunks.
/// </summary>
public interface IHeaderChunkSet : ISet<IHeaderChunk>
{
    /// <summary>
    /// Creates a new header chunk using the ID.
    /// </summary>
    /// <param name="chunkId">ID of the header chunk.</param>
    /// <returns>A new header chunk instance.</returns>
    IHeaderChunk Create(uint chunkId);

    /// <summary>
    /// Creates a new header chunk using the header chunk type.
    /// </summary>
    /// <typeparam name="T">Type of the header chunk.</typeparam>
    /// <returns>A new header chunk instance.</returns>
    T Create<T>() where T : IHeaderChunk, new();

    /// <summary>
    /// Removes a header chunk using the ID.
    /// </summary>
    /// <param name="chunkId">ID of the header chunk.</param>
    /// <returns>True if the header chunk was successfully removed, otherwise false.</returns>
    bool Remove(uint chunkId);

    /// <summary>
    /// Removes a header chunk using the header chunk type.
    /// </summary>
    /// <typeparam name="T">Type of the header chunk.</typeparam>
    /// <returns>True if the header chunk was successfully removed, otherwise false.</returns>
    bool Remove<T>() where T : IHeaderChunk;

    /// <summary>
    /// Gets a header chunk using the ID.
    /// </summary>
    /// <param name="chunkId">ID of the header chunk.</param>
    /// <returns>A new header chunk instance if available, otherwise null.</returns>
    IHeaderChunk? Get(uint chunkId);

    /// <summary>
    /// Gets a header chunk using the header chunk type.
    /// </summary>
    /// <typeparam name="T">Type of the header chunk.</typeparam>
    /// <returns>A new header chunk instance if available, otherwise null.</returns>
    T? Get<T>() where T : IHeaderChunk;
}

internal sealed class HeaderChunkSet : HashSet<IHeaderChunk>, IHeaderChunkSet
{
#if NET6_0_OR_GREATER
    internal HeaderChunkSet(int capacity) : base(capacity) { }
#else
#pragma warning disable IDE0060
    internal HeaderChunkSet(int capacity) : base() { }
#pragma warning restore IDE0060
#endif
    internal HeaderChunkSet() : base() { }

    public IHeaderChunk Create(uint chunkId)
    {
        var chunk = ClassManager.NewHeaderChunk(chunkId) ?? throw new Exception($"Header chunk {chunkId:X8} is not supported.");

        Add(chunk);

        return chunk;
    }

    public T Create<T>() where T : IHeaderChunk, new()
    {
        var chunk = new T();

        Add(chunk);

        return chunk;
    }

    public bool Remove(uint chunkId)
    {
        return RemoveWhere(x => x.Id == chunkId) > 0;
    }

    public bool Remove<T>() where T : IHeaderChunk
    {
        return RemoveWhere(x => x is T) > 0;
    }

    public IHeaderChunk? Get(uint chunkId)
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

    public T? Get<T>() where T : IHeaderChunk
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
