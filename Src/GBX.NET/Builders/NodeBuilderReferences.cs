namespace GBX.NET.Builders;

internal static class NodeBuilderReferences
{
    public static TNode[] ToNodes<TNode, TView>(IEnumerable<TView> values, bool allowNull) where TNode : CMwNod?
    {
        if (values is null) throw new ArgumentNullException(nameof(values));
        var nodes = new List<TNode>();
        foreach (var value in values)
        {
            if (value is TNode node) nodes.Add(node);
            else if (value is null && allowNull) nodes.Add(default!);
            else throw new ArgumentException("A GBX.NET node is required.", nameof(values));
        }

        return nodes.ToArray();
    }

    public static NodeRefList<T> CopyList<T>(NodeRefList<T> source) where T : CMwNod?
    {
        var copy = new NodeRefList<T>(source.Count);
        foreach (var reference in source.EnumerateReferences()) copy.Add(reference);
        return copy;
    }
}
