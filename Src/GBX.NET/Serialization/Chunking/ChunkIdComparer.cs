namespace GBX.NET.Serialization.Chunking;

public sealed class ChunkIdComparer : IComparer<uint>
{
    public static readonly ChunkIdComparer Default = new();

    public int Compare(uint x, uint y)
    {
        // TODO order by chunk type and inheritance
        return x.CompareTo(y);
    }
}