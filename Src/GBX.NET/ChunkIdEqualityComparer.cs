namespace GBX.NET;

public sealed class ChunkIdEqualityComparer : IEqualityComparer<IChunk>
{
    public bool Equals(IChunk? x, IChunk? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        return x.Id == y.Id;
    }

    public int GetHashCode(IChunk? obj)
    {
        return obj is null ? 0 : obj.Id.GetHashCode();
    }
}