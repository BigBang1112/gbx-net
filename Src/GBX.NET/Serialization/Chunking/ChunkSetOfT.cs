using GBX.NET.Managers;
using System.Collections;

namespace GBX.NET.Serialization.Chunking;

/// <summary>
/// A set of chunks.
/// </summary>
public interface IChunkSet<TKind> : ICollection<TKind>, IEnumerable<TKind>, IEnumerable where TKind : IChunk
{
    IComparer<TKind> Comparer { get; }

    /// <summary>
    /// Creates a new chunk using the ID.
    /// </summary>
    /// <param name="chunkId">ID of the chunk.</param>
    /// <param name="preferHeaderChunks">Whether to prefer creating header chunks if available.</param>
    /// <returns>A new chunk instance.</returns>
    TKind Create(uint chunkId, bool preferHeaderChunks = false);

    /// <summary>
    /// Creates a new chunk using the chunk type.
    /// </summary>
    /// <typeparam name="T">Type of the chunk.</typeparam>
    /// <returns>A new chunk instance.</returns>
    T Create<T>() where T : TKind, new();

    /// <summary>
    /// Tries to create a new chunk using the ID. Returns false if the same chunk ID already exists.
    /// </summary>
    /// <param name="chunkId">ID of the chunk.</param>
    /// <param name="preferHeaderChunks">Whether to prefer creating header chunks if available.</param>
    /// <param name="chunk">The created chunk if successful.</param>
    /// <returns>True if a new chunk was successfully created, otherwise false.</returns>
    bool TryCreate(uint chunkId, bool preferHeaderChunks, out TKind chunk);

    /// <summary>
    /// Tries to create a new chunk using the chunk type. Returns false if the same chunk already exists.
    /// </summary>
    /// <typeparam name="T">Type of the chunk.</typeparam>
    /// <param name="chunk">The created chunk if successful.</param>
    /// <returns>True if a new chunk was successfully created, otherwise false.</returns>
    bool TryCreate<T>(out T chunk) where T : TKind, new();

    /// <summary>
    /// Gets a chunk using the ID.
    /// </summary>
    /// <param name="chunkId">ID of the chunk.</param>
    /// <returns>A new chunk instance if available, otherwise null.</returns>
    TKind? Get(uint chunkId);

    /// <summary>
    /// Gets a chunk using the chunk type.
    /// </summary>
    /// <param name="chunkType">Type of the chunk.</param>
    /// <returns>A new chunk instance if available, otherwise null.</returns>
    TKind? Get(Type chunkType);

    /// <summary>
    /// Gets a chunk using the chunk type.
    /// </summary>
    /// <typeparam name="T">Type of the chunk.</typeparam>
    /// <returns>A new chunk instance if available, otherwise null.</returns>
    T? Get<T>() where T : TKind;

    /// <summary>
    /// Removes a chunk using the chunk ID.
    /// </summary>
    /// <param name="chunkId">ID of the chunk.</param>
    /// <returns>True if the chunk was successfully removed, otherwise false.</returns>
    bool Remove(uint chunkId);

    /// <summary>
    /// Removes a chunk using the chunk type.
    /// </summary>
    /// <param name="chunkType">Type of the chunk.</param>
    /// <returns>True if the chunk was successfully removed, otherwise false.</returns>
    bool Remove(Type chunkType);

    /// <summary>
    /// Removes a chunk using the chunk type.
    /// </summary>
    /// <typeparam name="T">Type of the chunk.</typeparam>
    /// <returns>True if the chunk was successfully removed, otherwise false.</returns>
    bool Remove<T>() where T : TKind;

    bool Contains(uint chunkId);
    bool Contains(Type chunkType);
    bool Contains<T>() where T : TKind;
}

internal class ChunkSet<TKind>(CMwNod? node) : IChunkSet<TKind>, ICollection where TKind : IChunk
{
    private readonly CMwNod? node = node;
    private readonly List<TKind> chunks = [];
    private readonly Dictionary<uint, TKind> chunksById = [];
    private readonly Dictionary<Type, TKind> chunksByType = [];

    public virtual IComparer<TKind> Comparer => ChunkComparer<TKind>.Default;

#if NET9_0_OR_GREATER
    private readonly Lock _lock = new();
#else
    private readonly object _lock = new();
#endif
    private object? _syncRoot;

    public int Count => chunks.Count;
    public bool IsReadOnly => false;

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot => _syncRoot ??= new object();

    protected virtual TKind New(uint chunkId, bool preferHeaderChunks)
    {
        if (preferHeaderChunks)
        {
            return (TKind)(node?.NewHeaderChunk(chunkId) ?? ClassManager.NewHeaderChunk(chunkId) ?? ClassManager.NewChunk(chunkId) ?? throw new ChunkNotSupportedException(chunkId));
        }
        else
        {
            return (TKind)(node?.NewChunk(chunkId) ?? ClassManager.NewChunk(chunkId) ?? ClassManager.NewHeaderChunk(chunkId) ?? throw new ChunkNotSupportedException(chunkId));
        }
    }

    public bool Add(TKind chunk)
    {
        if (chunk is null) throw new ArgumentNullException(nameof(chunk));

        var chunkType = chunk.GetType();

        if (chunkType != typeof(SkippableChunk) && chunksByType.ContainsKey(chunkType))
        {
            return false;
        }

        chunksById[chunk.Id] = chunk; // overwriting IDs is intentional as some chunks share the same ID with header chunks
        chunksByType[chunkType] = chunk;

        var index = chunks.BinarySearch(chunk, Comparer);
        if (index < 0) index = ~index; // Bitwise complement gets the exact insertion point

        chunks.Insert(index, chunk);
        return true;
    }

    /// <summary>
    /// Add the chunk without checking for duplicates or maintaining order. This is used internally when creating chunks to avoid unnecessary overhead.
    /// </summary>
    /// <param name="chunk"></param>
    /// <exception cref="ArgumentNullException"></exception>
    internal void AddInternal(TKind? chunk)
    {
        if (chunk is null) return;

        chunksById[chunk.Id] = chunk;
        chunksByType[chunk.GetType()] = chunk;

        chunks.Add(chunk);
    }

    public TKind Create(uint chunkId, bool preferHeaderChunks = false)
    {
        if (chunksById.TryGetValue(chunkId, out var chunk))
        {
            return chunk;
        }

        chunk = New(chunkId, preferHeaderChunks);

        Add(chunk);

        return chunk;
    }

    public T Create<T>() where T : TKind, new()
    {
        if (chunksByType.TryGetValue(typeof(T), out var chunk))
        {
            return (T)chunk;
        }

        var newChunk = new T();
        Add(newChunk);

        return newChunk;
    }

    public bool TryCreate(uint chunkId, bool preferHeaderChunks, out TKind chunk)
    {
        if (chunksById.TryGetValue(chunkId, out var chunkById) && chunkById is not null) 
        {
            chunk = chunkById;
            return false;
        }

        chunk = New(chunkId, preferHeaderChunks);

        Add(chunk);

        return true;
    }

    public bool TryCreate<T>(out T chunk) where T : TKind, new()
    {
        if (chunksByType.TryGetValue(typeof(T), out var chunkBase))
        {
            chunk = (T)chunkBase;
            return false;
        }

        chunk = new T();
        Add(chunk);

        return true;
    }

    public TKind? Get(uint chunkId)
    {
        return chunksById.TryGetValue(chunkId, out var chunk) ? chunk : default;
    }

    public TKind? Get(Type chunkType)
    {
        return chunksByType.TryGetValue(chunkType, out var chunk) ? chunk : default;
    }

    public T? Get<T>() where T : TKind
    {
        return (T?)Get(typeof(T));
    }

    public bool Remove(uint chunkId)
    {
        if (!chunksById.TryGetValue(chunkId, out TKind? chunk))
        {
            return false;
        }

        chunksById.Remove(chunkId);
        chunksByType.Remove(chunk.GetType());

        chunks.Remove(chunk);
        return true;
    }

    public bool Remove(Type chunkType)
    {
        if (!chunksByType.TryGetValue(chunkType, out var chunk))
        {
            return false;
        }

        chunksById.Remove(chunk.Id);
        chunksByType.Remove(chunkType);

        chunks.Remove(chunk);
        return true;
    }

    public bool Remove<T>() where T : TKind
    {
        return Remove(typeof(T));
    }

    public bool Remove(TKind chunk)
    {
        if (chunk is null || !chunksById.ContainsKey(chunk.Id))
        {
            return false;
        }

        chunksById.Remove(chunk.Id);
        chunksByType.Remove(chunk.GetType());

        chunks.Remove(chunk);
        return true;
    }

    public void Clear()
    {
        chunks.Clear();
        chunksById.Clear();
        chunksByType.Clear();
    }

    public bool Contains(TKind item)
    {
        return chunks.Contains(item);
    }

    public bool Contains(uint chunkId)
    {
        return chunksById.ContainsKey(chunkId);
    }

    public bool Contains(Type chunkType)
    {
        return chunksByType.ContainsKey(chunkType);
    }

    public bool Contains<T>() where T : TKind
    {
        return Contains(typeof(T));
    }

    public void CopyTo(TKind[] array, int arrayIndex)
    {
        chunks.CopyTo(array, arrayIndex);
    }

    public IEnumerator<TKind> GetEnumerator()
    {
        return chunks.GetEnumerator();
    }

    void ICollection<TKind>.Add(TKind item)
    {
        _ = Add(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    void ICollection.CopyTo(Array array, int index)
    {
        chunks.CopyTo((TKind[])array, index);
    }
}