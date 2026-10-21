using System.Collections;
using GBX.NET.Exceptions;

namespace GBX.NET.Interfaces;

// Resolve each element only when requested, and follow replacement of the
// owner's collection rather than capturing an obsolete list instance.
internal sealed class ScopedNodeList<TNode, TView>(Func<NodeRefList<TNode>?> getNodes, bool required, string member)
    : IReadOnlyList<TView> where TNode : CMwNod?
{
    public int Count => getNodes()?.Count ?? 0;

    public TView this[int index]
    {
        get
        {
            var node = (getNodes() ?? throw new ArgumentOutOfRangeException(nameof(index)))[index];
            if (node is null)
            {
                if (required) throw new MemberNullException(member);
                return default!;
            }

            return (TView)(object)node;
        }
    }

    public IEnumerator<TView> GetEnumerator()
    {
        for (var i = 0; i < Count; i++) yield return this[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
