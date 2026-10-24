using System.Collections;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace GBX.NET;

/// <summary>
/// A collection that retains external descriptors and resolves nodes on element access.
/// </summary>
/// <typeparam name="T">The node type. Use a nullable type argument when elements can be null.</typeparam>
public sealed class NodeRefList<T> : IList<T>, IReadOnlyList<T>, IList, IDeepCloneable where T : CMwNod?
{
    private readonly List<NodeRef<T>> references;
    private ReadOnlyCollection<NodeRef<T>>? referenceView;

    internal IReadOnlyList<NodeRef<T>> References => referenceView ??= references.AsReadOnly();

    public bool IsReadOnly => false;
    public int Count => references.Count;

    bool IList.IsFixedSize => false;
    bool ICollection.IsSynchronized => false;
    object ICollection.SyncRoot => this;

    public NodeRefList() : this(0)
    {
    }

    public NodeRefList(int capacity)
    {
        references = new(capacity);
    }

    public NodeRefList(IEnumerable<T> nodes) : this(nodes is ICollection<T> collection ? collection.Count : 0)
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

    public static implicit operator NodeRefList<T>?(T[]? nodes) => nodes is null ? null : new(nodes);

    public static implicit operator NodeRefList<T>?(List<T>? nodes) => nodes is null ? null : new(nodes);

#pragma warning disable CS0618 // This obsolete conversion preserves compatibility with External<T> arrays.
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
                result.Add(default(T)!);
            }
            else
            {
                result.Add(node.ToNodeRef());
            }
        }

        return result;
    }

#pragma warning restore CS0618

    public T this[int index]
    {
        get => references[index].Resolve()!;
        set => references[index].Node = value;
    }

    object? IList.this[int index]
    {
        get => this[index];
        set => this[index] = ValidateNode(value);
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

    // Readers create a fresh handle for each entry, so ownership can transfer without copying.
    internal void AddOwned(NodeRef<T> reference) => references.Add(reference);

    [OverloadResolutionPriority(1)]
    public void Add(T item) => references.Add(new(item));

    int IList.Add(object? value)
    {
        Add(ValidateNode(value));
        return Count - 1;
    }

    /// <summary>
    /// Inserts a copy of the stored reference without resolving it. The new element owns its own handle.
    /// </summary>
    public void Insert(int index, NodeRef<T> reference) => references.Insert(index, Copy(reference));

    [OverloadResolutionPriority(1)]
    public void Insert(int index, T item) => references.Insert(index, new(item));

    void IList.Insert(int index, object? value) => Insert(index, ValidateNode(value));

    private static T ValidateNode(object? value)
    {
        if (value is null or T)
        {
            return (T)value!;
        }

        throw new ArgumentException($"Expected a node of type {typeof(T).Name}.", nameof(value));
    }

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

    public bool Remove(T item)
    {
        var index = IndexOf(item);
        if (index < 0)
        {
            return false;
        }

        RemoveAt(index);
        return true;
    }

    void IList.Remove(object? value)
    {
        if (value is null or T)
        {
            Remove((T)value!);
        }
    }

    public int IndexOf(T item)
    {
        var comparer = EqualityComparer<T>.Default;

        for (var i = 0; i < Count; i++)
        {
            if (comparer.Equals(this[i], item))
            {
                return i;
            }
        }

        return -1;
    }

    int IList.IndexOf(object? value) => value is null or T ? IndexOf((T)value!) : -1;

    public bool Contains(T item) => IndexOf(item) >= 0;

    bool IList.Contains(object? value) => (value is null or T) && Contains((T)value!);

    public int RemoveAll(Predicate<T> match)
    {
        if (match is null)
        {
            throw new ArgumentNullException(nameof(match));
        }

        return references.RemoveAll(reference => match(reference.Resolve()!));
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

    public void CopyTo(T[] array, int arrayIndex)
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
            array[arrayIndex++] = reference.Resolve()!;
        }
    }

    void ICollection.CopyTo(Array array, int index)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        if (array.Rank != 1 || array.GetLowerBound(0) != 0)
        {
            throw new ArgumentException("The destination must be a one-dimensional, zero-based array.", nameof(array));
        }

        if (index < 0 || index > array.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        if (array.Length - index < Count)
        {
            throw new ArgumentException("The destination array is too small.", nameof(array));
        }

        var elementType = array.GetType().GetElementType()!;

        if (!elementType.IsAssignableFrom(typeof(T)) && !typeof(T).IsAssignableFrom(elementType))
        {
            throw new ArgumentException("The destination array has an incompatible element type.", nameof(array));
        }

        foreach (var reference in references)
        {
            var node = reference.Resolve();

            try
            {
                array.SetValue(node, index++);
            }
            catch (InvalidCastException ex)
            {
                throw new ArgumentException("The destination array has an incompatible element type.", nameof(array), ex);
            }
        }
    }

    public T[] ToArray()
    {
        var result = new T[Count];
        CopyTo(result, 0);
        return result;
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var reference in references)
        {
            yield return reference.Resolve()!;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    object IDeepCloneable.DeepClone(DeepCloneContext context)
    {
        var clone = new NodeRefList<T>(Count);
        context.Register(this, clone);

        foreach (var reference in references)
        {
            clone.references.Add(context.Clone(reference)!);
        }

        return clone;
    }
}
