namespace GBX.NET.Serialization.Chunking;

public sealed class ChunkComparer : IComparer<IChunk>
{
    public static readonly ChunkComparer Default = new();

    public int Compare(IChunk? x, IChunk? y)
    {
        // Both null or same instance
        if (ReferenceEquals(x, y)) return 0;

        // null is considered less than any non-null
        if (x is null) return -1;
        if (y is null) return 1;

        // Prioritize IHeaderChunk instances
        if (x is IHeaderChunk && y is not IHeaderChunk) return -1;
        if (x is not IHeaderChunk && y is IHeaderChunk) return 1;

        // Compare by Id if none of the above conditions are met
        return x.Id.CompareTo(y.Id);
    }
}