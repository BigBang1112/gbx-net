using System.Collections;

namespace GBX.NET;

/// <summary>A collection that retains external descriptors and resolves targets on element access.</summary>
public sealed class NodeRefList<T> : IList<T?>, IReadOnlyList<T?>, IDeepCloneable where T : CMwNod
{
    private readonly List<NodeRef<T>> refs = [];
    public bool IsFixedSize { get; internal set; }
    public bool IsReadOnly => false;
    public int Count => refs.Count;
    [Obsolete("Use Count instead.")]
    public int Length => Count;

    public NodeRefList() { }

    public NodeRefList(IEnumerable<T?> nodes)
    {
        foreach (var node in nodes) Add(node);
    }

    public static implicit operator NodeRefList<T>?(T?[]? nodes) => nodes is null ? null : new(nodes);
    public static implicit operator NodeRefList<T>?(List<T?>? nodes) => nodes is null ? null : new(nodes);

    [Obsolete("Create NodeRefList<T> and use GetReference(index).SetExternal(file) instead.")]
    public static implicit operator NodeRefList<T>?(External<T>[]? nodes)
    {
        if (nodes is null) return null;
        var result = new NodeRefList<T>();
        foreach (var node in nodes)
        {
            result.Add(node?.CachedTarget);
            if (node?.File is { } file) result.refs[result.Count - 1].SetExternal(file);
        }
        return result;
    }

    public static NodeRefList<T> CreateFixed(int count)
    {
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
        var result = new NodeRefList<T>();
        for (var i = 0; i < count; i++) result.Add(null);
        result.IsFixedSize = true;
        return result;
    }

    public T? this[int index]
    {
        get => refs[index].Resolve();
        set => refs[index].SetEmbedded(value);
    }

    public NodeRef<T> GetReference(int index) => refs[index];
    public ReferenceView References => new(this);

    public readonly struct ReferenceView
    {
        private readonly NodeRefList<T> owner;
        internal ReferenceView(NodeRefList<T> owner) => this.owner = owner;
        public int Count => owner.Count;
        public NodeRef<T> this[int index] => owner.GetReference(index);
    }

    public void AddReference(NodeRef<T> reference)
    {
        EnsureVariableSize();
        if (reference is null) throw new ArgumentNullException(nameof(reference));
        // Each element owns its handle even when descriptors or targets are shared.
        var entry = new NodeRef<T>();
        if (reference.Descriptor is { } descriptor) entry.SetDescriptor(descriptor);
        else entry.SetEmbedded(reference.CachedTarget);
        refs.Add(entry);
    }

    public void Add(T? item) => Insert(Count, item);
    public void Insert(int index, T? item)
    {
        EnsureVariableSize();
        var reference = new NodeRef<T>();
        reference.SetEmbedded(item);
        refs.Insert(index, reference);
    }

    public void Clear() { EnsureVariableSize(); refs.Clear(); }
    public void RemoveAt(int index) { EnsureVariableSize(); refs.RemoveAt(index); }
    public bool Remove(T? item)
    {
        EnsureVariableSize();
        var index = IndexOf(item);
        if (index < 0) return false;
        RemoveAt(index);
        return true;
    }

    public int IndexOf(T? item)
    {
        for (var i = 0; i < Count; i++) if (EqualityComparer<T?>.Default.Equals(this[i], item)) return i;
        return -1;
    }

    public bool Contains(T? item) => IndexOf(item) >= 0;
    public int RemoveAll(Predicate<T?> match)
    {
        EnsureVariableSize();
        if (match is null) throw new ArgumentNullException(nameof(match));
        var count = 0;
        for (var i = Count - 1; i >= 0; i--)
        {
            if (!match(this[i])) continue;
            refs.RemoveAt(i);
            count++;
        }
        return count;
    }

    internal IEnumerable<T?> GetEmbeddedTargets()
    {
        foreach (var reference in refs)
        {
            if (reference.Descriptor is not null) throw new NotSupportedException("This serialization context cannot encode external references.");
            yield return reference.CachedTarget;
        }
    }
    public void CopyTo(T?[] array, int arrayIndex)
    {
        if (array is null) throw new ArgumentNullException(nameof(array));
        if (arrayIndex < 0 || arrayIndex > array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        if (array.Length - arrayIndex < Count) throw new ArgumentException("The destination array is too small.", nameof(array));
        foreach (var reference in refs) array[arrayIndex++] = reference.Resolve();
    }

    public T?[] ToArray()
    {
        var result = new T?[Count];
        CopyTo(result, 0);
        return result;
    }

    public IEnumerator<T?> GetEnumerator()
    {
        foreach (var reference in refs) yield return reference.Resolve();
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private void EnsureVariableSize()
    {
        if (IsFixedSize) throw new NotSupportedException("This reference collection has a fixed size.");
    }

    object IDeepCloneable.DeepClone(DeepCloneContext context)
    {
        var clone = new NodeRefList<T>();
        context.Register(this, clone);
        foreach (var reference in refs) clone.refs.Add(context.Clone(reference)!);
        clone.IsFixedSize = IsFixedSize;
        return clone;
    }
}
