namespace GBX.NET.Interfaces;

internal static class NodeInterfaceReferences
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
}
