using GBX.NET.Managers;
using System.Collections;

namespace GBX.NET.Serialization.Chunking;

/// <summary>
/// A set of chunks.
/// </summary>
public interface IChunkSet<TKind> : ICollection<TKind>, IEnumerable<TKind>, IEnumerable where TKind : IChunk
{
    /// <summary>
    /// Creates a new chunk using the ID.
    /// </summary>
    /// <param name="chunkId">ID of the chunk.</param>
    /// <returns>A new chunk instance.</returns>
    TKind Create(uint chunkId);

    /// <summary>
    /// Creates a new chunk using the chunk type.
    /// </summary>
    /// <typeparam name="T">Type of the chunk.</typeparam>
    /// <returns>A new chunk instance.</returns>
    T Create<T>() where T : TKind, new();

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
    bool Remove<T>() where T : TKind;

    /// <summary>
    /// Gets a chunk using the ID.
    /// </summary>
    /// <param name="chunkId">ID of the chunk.</param>
    /// <returns>A new chunk instance if available, otherwise null.</returns>
    TKind? Get(uint chunkId);

    /// <summary>
    /// Gets a chunk using the chunk type.
    /// </summary>
    /// <typeparam name="T">Type of the chunk.</typeparam>
    /// <returns>A new chunk instance if available, otherwise null.</returns>
    T? Get<T>() where T : TKind;
}

internal class ChunkSet<TKind> : IChunkSet<TKind> where TKind : IChunk
{
    private readonly SortedDictionary<uint, TKind> chunksById;
    private readonly Dictionary<Type, uint> idsByType;

    public int Count => chunksById.Count;
    public bool IsReadOnly => false;

    public ChunkSet()
    {
        chunksById = new(new ChunkIdComparer());
        idsByType = [];
    }

    public virtual TKind Create(uint chunkId)
    {
        if (chunksById.TryGetValue(chunkId, out var chunk))
        {
            return chunk;
        }

        chunk = (TKind)(ClassManager.NewChunk(chunkId) ?? ClassManager.NewHeaderChunk(chunkId) ?? throw new Exception($"Chunk 0x{chunkId:X8} is not supported."));

        Add(chunk);

        return chunk;
    }

    public T Create<T>() where T : TKind, new()
    {
        var chunk = new T();

        if (chunksById.TryGetValue(chunk.Id, out var c))
        {
            return (T)c;
        }

        Add(chunk);

        return chunk;
    }

    public TKind? Get(uint chunkId)
    {
        return chunksById.TryGetValue(chunkId, out var chunk) ? chunk : default;
    }

    private IChunk? Get(Type type)
    {
        return idsByType.TryGetValue(type, out var id) ? Get(id) : null;
    }

    public T? Get<T>() where T : TKind
    {
        return (T?)Get(typeof(T));
    }

    public bool Remove(uint chunkId)
    {
        return chunksById.Remove(chunkId);
    }

    public bool Remove<T>() where T : TKind
    {
        return idsByType.TryGetValue(typeof(T), out var id) && Remove(id);
    }

    public bool Remove(TKind item)
    {
        return chunksById.Remove(item.Id);
    }

    public bool Add(TKind item)
    {
#if NET6_0_OR_GREATER
        return chunksById.TryAdd(item.Id, item);
#else
        try
        {
            chunksById.Add(item.Id, item);
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
#endif
    }

    public void Clear()
    {
        chunksById.Clear();
    }

    public bool Contains(TKind item)
    {
        return chunksById.ContainsKey(item.Id);
    }

    public void CopyTo(TKind[] array, int arrayIndex)
    {
        chunksById.Values.CopyTo(array, arrayIndex);
    }

    public void ExceptWith(IEnumerable<TKind> other)
    {
        foreach (var chunk in other)
        {
            Remove(chunk.Id);
        }
    }

    public IEnumerator<TKind> GetEnumerator()
    {
        return chunksById.Values.GetEnumerator();
    }

    public bool Overlaps(IEnumerable<TKind> other)
    {
        foreach (var chunk in other)
        {
            if (Contains(chunk))
            {
                return true;
            }
        }

        return false;
    }

    void ICollection<TKind>.Add(TKind item)
    {
        Add(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}