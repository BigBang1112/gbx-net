using System.Collections;

namespace GBX.NET.Builders;

// Resolve each element only when requested, and follow replacement of the
// owner's collection rather than capturing an obsolete list instance.
internal sealed class ScopedNodeList<TNode, TView>(Func<NodeRefList<TNode>?> getNodes, Func<TNode, TView> project)
    : IReadOnlyList<TView> where TNode : CMwNod?
{
    public int Count => getNodes()?.Count ?? 0;

    public TView this[int index] => project((getNodes() ?? throw new ArgumentOutOfRangeException(nameof(index)))[index]);

    public IEnumerator<TView> GetEnumerator()
    {
        for (var i = 0; i < Count; i++) yield return this[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
