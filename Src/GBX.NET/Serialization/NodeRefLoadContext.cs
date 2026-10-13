namespace GBX.NET.Serialization;

internal sealed class NodeRefLoadContext
{
    private static readonly AsyncLocal<NodeRefLoadContext?> current = new();
    private static readonly StringComparer pathComparer = Path.DirectorySeparatorChar == '\\' ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

    private readonly Dictionary<string, Entry> ancestors;
    private readonly Entry owner;

    private NodeRefLoadContext(Dictionary<string, Entry> ancestors, Entry owner)
    {
        this.ancestors = ancestors;
        this.owner = owner;
    }

    public static NodeRefLoadContext Create(string path, CMwNod node)
    {
        var ancestors = current.Value is not null
            ? new Dictionary<string, Entry>(current.Value.ancestors, pathComparer)
            : new Dictionary<string, Entry>(pathComparer);

        var owner = new Entry(node);
        ancestors[Path.GetFullPath(path)] = owner;

        return new(ancestors, owner);
    }

    public void MarkParsed() => owner.Parsed = true;

    public CMwNod? FindAncestor(string path)
    {
        if (!ancestors.TryGetValue(Path.GetFullPath(path), out var entry))
        {
            return null;
        }

        if (!entry.Parsed)
        {
            throw new InvalidOperationException("An external dependency cycle re-entered a node that is still being parsed.");
        }

        return entry.Node;
    }

    public IDisposable Enter()
    {
        var previous = current.Value;
        current.Value = this;
        return new Scope(previous);
    }

    private sealed class Entry(CMwNod node)
    {
        public CMwNod Node { get; } = node;
        public bool Parsed { get; set; }
    }

    private sealed class Scope(NodeRefLoadContext? previous) : IDisposable
    {
        public void Dispose() => current.Value = previous;
    }
}
