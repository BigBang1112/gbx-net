namespace GBX.NET;

public sealed class ChunkIdComparer : IComparer<IChunk>
{
    public int Compare(IChunk? x, IChunk? y)
    {
        if (x is null || y is null)
        {
            return 0;
        }

        return x.Id.CompareTo(y.Id);
    }
}