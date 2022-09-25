using System.Diagnostics.CodeAnalysis;

namespace GBX.NET;

public class HeaderChunkSet : SortedSet<Chunk>
{
    public HeaderChunkSet() : base()
    {

    }

    public HeaderChunkSet(IEnumerable<Chunk> collection) : base(collection)
    {

    }

    public bool Remove(uint chunkID)
    {
        return RemoveWhere(x => x.Id == chunkID) > 0;
    }

    public bool Remove<T>() where T : Chunk
    {
        return RemoveWhere(x => x is T) > 0;
    }

    public Chunk Create(uint chunkId)
    {
        if (TryGet(chunkId, out var c))
        {
            return c ?? throw new ThisShouldNotHappenException();
        }

        var chunk = NodeManager.GetNewHeaderChunk(chunkId) ?? throw new Exception("Chunk ID does not exist.");

        Add((Chunk)chunk);

        return (Chunk)chunk;
    }

    public T Create<T>() where T : Chunk
    {
        return (T)Create(NodeManager.HeaderChunkIdsByType[typeof(T)]);
    }

    public Chunk? Get(uint chunkId)
    {
        return this.FirstOrDefault(x => x.Id == chunkId);
    }

#if NET462_OR_GREATER || NETSTANDARD2_0
    public bool TryGet(uint chunkId, out Chunk? chunk)
#else
    public bool TryGet(uint chunkId, [NotNullWhen(true)] out Chunk? chunk)
#endif
    {
        chunk = Get(chunkId);
        return chunk is not null;
    }

    public T? Get<T>() where T : Chunk
    {
        foreach (var chunk in this)
        {
            if (chunk is T t)
            {
                if (chunk is ISkippableChunk s) s.Discover();
                return t;
            }
        }
        return default;
    }

    public bool TryGet<T>(out T? chunk) where T : Chunk
    {
        chunk = Get<T>();
        return chunk is not null;
    }
}
