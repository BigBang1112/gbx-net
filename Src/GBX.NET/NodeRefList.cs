using System.Collections;
using System.Runtime.CompilerServices;

namespace GBX.NET;

/// <summary>
/// A collection that retains external descriptors and resolves nodes on element access.
/// </summary>
public sealed class NodeRefList<T> : IList<T?>, IReadOnlyList<T?>, IDeepCloneable where T : CMwNod
{
    private readonly List<NodeRef<T>> references = [];

    public bool IsReadOnly => false;
    public int Count => references.Count;

    public NodeRefList()
    {
    }

    public NodeRefList(IEnumerable<T?> nodes)
    {
        if (nodes is null)
        {
            throw new ArgumentNullException(nameof(nodes));
        }

        foreach (var node in nodes)
        {
            Add(node);
        }
    }

    public static implicit operator NodeRefList<T>?(T?[]? nodes) => nodes is null ? null : new(nodes);

    public static implicit operator NodeRefList<T>?(List<T?>? nodes) => nodes is null ? null : new(nodes);

    [Obsolete("Create NodeRefList<T> and use Get(index).Descriptor = file instead.")]
    public static implicit operator NodeRefList<T>?(External<T>[]? nodes)
    {
        if (nodes is null)
        {
            return null;
        }

        var result = new NodeRefList<T>();

        foreach (var node in nodes)
        {
            if (node is null)
            {
                result.Add(null);
            }
            else
            {
                result.Add(node.ToNodeRef());
            }
        }

        return result;
    }

    public T? this[int index]
    {
        get => references[index].Resolve();
        set => references[index].Node = value;
    }

    /// <summary>
    /// Gets the stored handle without resolving its node.
    /// </summary>
    public NodeRef<T> Get(int index) => references[index];

    /// <summary>
    /// Enumerates the stored handles without resolving their nodes.
    /// </summary>
    public IEnumerable<NodeRef<T>> EnumerateReferences()
    {
        foreach (var reference in references)
        {
            yield return reference;
        }
    }

    /// <summary>
    /// Appends a copy of the stored reference without resolving it. The new element owns its own handle.
    /// </summary>
    public void Add(NodeRef<T> reference) => references.Add(Copy(reference));

    [OverloadResolutionPriority(1)]
    public void Add(T? item) => references.Add(new() { Node = item });

    /// <summary>
    /// Inserts a copy of the stored reference without resolving it. The new element owns its own handle.
    /// </summary>
    public void Insert(int index, NodeRef<T> reference) => references.Insert(index, Copy(reference));

    [OverloadResolutionPriority(1)]
    public void Insert(int index, T? item) => references.Insert(index, new() { Node = item });

    private static NodeRef<T> Copy(NodeRef<T> reference)
    {
        if (reference is null)
        {
            throw new ArgumentNullException(nameof(reference));
        }

        // Each element owns its handle even when descriptors or nodes are shared.
        var entry = new NodeRef<T>();
        entry.CopyFrom(reference);
        return entry;
    }

    public void Clear() => references.Clear();

    public void RemoveAt(int index) => references.RemoveAt(index);

    public bool Remove(T? item)
    {
        var index = IndexOf(item);
        if (index < 0)
        {
            return false;
        }

        RemoveAt(index);
        return true;
    }

    public int IndexOf(T? item)
    {
        var comparer = EqualityComparer<T?>.Default;

        for (var i = 0; i < Count; i++)
        {
            if (comparer.Equals(this[i], item))
            {
                return i;
            }
        }

        return -1;
    }

    public bool Contains(T? item) => IndexOf(item) >= 0;

    public int RemoveAll(Predicate<T?> match)
    {
        if (match is null)
        {
            throw new ArgumentNullException(nameof(match));
        }

        return references.RemoveAll(reference => match(reference.Resolve()));
    }

    internal IEnumerable<T?> GetEmbeddedNodes()
    {
        foreach (var reference in references)
        {
            if (reference.Descriptor is not null)
            {
                throw new NotSupportedException("This serialization context cannot encode external references.");
            }

            yield return reference.CachedNode;
        }
    }

    public void CopyTo(T?[] array, int arrayIndex)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        if (arrayIndex < 0 || arrayIndex > array.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        }

        if (array.Length - arrayIndex < Count)
        {
            throw new ArgumentException("The destination array is too small.", nameof(array));
        }

        foreach (var reference in references)
        {
            array[arrayIndex++] = reference.Resolve();
        }
    }

    public T?[] ToArray()
    {
        var result = new T?[Count];
        CopyTo(result, 0);
        return result;
    }

    public IEnumerator<T?> GetEnumerator()
    {
        foreach (var reference in references)
        {
            yield return reference.Resolve();
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    object IDeepCloneable.DeepClone(DeepCloneContext context)
    {
        var clone = new NodeRefList<T>();
        context.Register(this, clone);

        foreach (var reference in references)
        {
            clone.references.Add(context.Clone(reference)!);
        }

        return clone;
    }
}
