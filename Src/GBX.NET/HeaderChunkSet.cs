namespace GBX.NET;

public interface IHeaderChunkSet : ISet<IHeaderChunk>
{
    IHeaderChunk Create(uint chunkId);
    T Create<T>() where T : IHeaderChunk, new();
    bool Remove(uint chunkId);
    bool Remove<T>() where T : IHeaderChunk;
    IHeaderChunk? Get(uint chunkId);
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
