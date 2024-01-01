namespace GBX.NET.Components;

public sealed class GbxRefTable(IDirectory root, int ancestorLevel)
{
    public IDirectory Root { get; } = root;
    public int AncestorLevel { get; } = ancestorLevel;
    public IDictionary<int, GbxRefTableResource> Resources { get; } = new Dictionary<int, GbxRefTableResource>();

    internal static GbxRefTable? Parse(GbxReader reader, GbxHeader header, GbxReadSettings settings)
    {
        return new GbxRefTableReader(reader, header, settings).Parse();
    }
}