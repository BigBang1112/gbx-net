using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Serialization;

internal interface IDeepCloneable
{
    object DeepClone(DeepCloneContext context);
}

internal sealed class DeepCloneContext
{
    private readonly Dictionary<object, object> clones = new(ReferenceComparer.Instance);

    public void Register(object source, object clone) => clones.Add(source, clone);

    public T? Clone<T>(T? source)
    {
        if (source is null)
        {
            return default;
        }

        if (source is string || source is Ident || source is PackDesc || source is ImmutableList<Ident> ||
            source is Delegate || source is Stream || source is Exception)
        {
            return source;
        }

        if (clones.TryGetValue(source, out var existing))
        {
            return (T)existing;
        }

        if (source is CMwNod node)
        {
            return (T)(object)CloneNode(node);
        }

        if (source is IChunk chunk)
        {
            return (T)CloneChunk(chunk);
        }

        if (source is IDeepCloneable cloneable)
        {
            return (T)cloneable.DeepClone(this);
        }

        if (source is Array array)
        {
            return (T)(object)CloneArrayCore(array);
        }

        if (source is CScriptTraitsMetadata.ScriptArrayType arrayType)
        {
            return (T)(object)new CScriptTraitsMetadata.ScriptArrayType(
                Clone(arrayType.KeyType)!, Clone(arrayType.ValueType)!);
        }

        if (source.GetType().IsValueType || source is Uri || source is Type || source is System.Threading.Tasks.Task)
        {
            return source;
        }

        throw new NotSupportedException($"Deep cloning {source.GetType()} is not supported.");
    }

    public CMwNod CloneNode(CMwNod source)
    {
        if (clones.TryGetValue(source, out var existing))
        {
            return (CMwNod)existing;
        }

        var clone = source.CreateCloneShell();
        Register(source, clone);
        source.DeepCloneFields(clone, this);
        return clone;
    }

    public IChunk CloneChunk(IChunk source)
    {
        if (clones.TryGetValue(source, out var existing))
        {
            return (IChunk)existing;
        }

#pragma warning disable GBXNET10001
        var clone = source.DeepClone();
#pragma warning restore GBXNET10001
        Register(source, clone);

        if (source is Chunk chunk)
        {
            chunk.DeepCloneFields((Chunk)clone, this);
        }

        return clone;
    }

    public T[]? CloneArray<T>(T[]? source)
    {
        if (source is null)
        {
            return null;
        }

        return (T[])CloneArrayCore(source);
    }

    private Array CloneArrayCore(Array source)
    {
        if (clones.TryGetValue(source, out var existing))
        {
            return (Array)existing;
        }

        var clone = (Array)source.Clone();
        Register(source, clone);

        var elementType = source.GetType().GetElementType();
        if (elementType?.IsPrimitive != true && elementType?.IsEnum != true && elementType != typeof(string))
        {
            CloneArrayElements(source, clone, 0, new int[source.Rank]);
        }

        return clone;
    }

    private void CloneArrayElements(Array source, Array clone, int dimension, int[] indices)
    {
        var end = source.GetLowerBound(dimension) + source.GetLength(dimension);

        for (var i = source.GetLowerBound(dimension); i < end; i++)
        {
            indices[dimension] = i;

            if (dimension + 1 < source.Rank)
            {
                CloneArrayElements(source, clone, dimension + 1, indices);
            }
            else
            {
                clone.SetValue(Clone(source.GetValue(indices)), indices);
            }
        }
    }

    public List<T>? CloneList<T>(IEnumerable<T>? source)
    {
        if (source is null)
        {
            return null;
        }

        if (clones.TryGetValue(source, out var existing))
        {
            return (List<T>)existing;
        }

        var clone = new List<T>();
        Register(source, clone);

        foreach (var item in source)
        {
            clone.Add(Clone(item)!);
        }

        return clone;
    }

    public Dictionary<TKey, TValue>? CloneDictionary<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? source) where TKey : notnull
    {
        if (source is null)
        {
            return null;
        }

        if (clones.TryGetValue(source, out var existing))
        {
            return (Dictionary<TKey, TValue>)existing;
        }

        var clone = source is Dictionary<TKey, TValue> dictionary
            ? new Dictionary<TKey, TValue>(dictionary.Comparer)
            : new Dictionary<TKey, TValue>();
        Register(source, clone);

        foreach (var pair in source)
        {
            clone.Add(Clone(pair.Key)!, Clone(pair.Value)!);
        }

        return clone;
    }

    public HashSet<T>? CloneHashSet<T>(ISet<T>? source)
    {
        if (source is null)
        {
            return null;
        }

        if (clones.TryGetValue(source, out var existing))
        {
            return (HashSet<T>)existing;
        }

        var clone = source is HashSet<T> set ? new HashSet<T>(set.Comparer) : new HashSet<T>();
        Register(source, clone);

        foreach (var item in source)
        {
            clone.Add(Clone(item)!);
        }

        return clone;
    }

    private sealed class ReferenceComparer : IEqualityComparer<object>
    {
        public static readonly ReferenceComparer Instance = new();

        public new bool Equals(object? x, object? y) => ReferenceEquals(x, y);

        public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
    }
}
