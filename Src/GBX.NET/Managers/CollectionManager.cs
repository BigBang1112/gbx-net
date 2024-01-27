namespace GBX.NET.Managers;

public static partial class CollectionManager
{
    public static IDictionary<int, string> CustomCollections { get; } = new Dictionary<int, string>();

    public static partial string? GetName(int id);
}
