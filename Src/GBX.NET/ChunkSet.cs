namespace GBX.NET;

public interface IChunkSet : ISet<IChunk>
{
    IChunk Create(uint chunkId);
    T Create<T>() where T : IChunk, new();
    bool Remove(uint chunkId);
    bool Remove<T>() where T : IChunk;
    IChunk? Get(uint chunkId);
    T? Get<T>() where T : IChunk;
}

internal sealed class ChunkSet : HashSet<IChunk>, IChunkSet
{
#if NET6_0_OR_GREATER
    internal ChunkSet(int capacity) : base(capacity) { }
#else
#pragma warning disable IDE0060
    internal ChunkSet(int capacity) : base() { }
#pragma warning restore IDE0060
#endif
    internal ChunkSet() : base() { }

    public IChunk Create(uint chunkId)
    {
        var chunk = ClassManager.NewChunk(chunkId) ?? throw new Exception($"Chunk {chunkId:X8} is not supported.");

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
