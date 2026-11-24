using GBX.NET;
using GBX.NET.Components;
using GBX.NET.Serialization.Chunking;
using System.Collections;
using System.Net;
using System.Runtime.CompilerServices;

namespace GbxCLI;

internal sealed class GbxPropertyComparer(bool hasExternalReferences, CancellationToken cancellationToken)
{
    private readonly HashSet<(object Left, object Right)> visited = new(EqualityComparer<(object Left, object Right)>.Create(
        (a, b) => ReferenceEquals(a.Left, b.Left) && ReferenceEquals(a.Right, b.Right),
        pair => HashCode.Combine(RuntimeHelpers.GetHashCode(pair.Left), RuntimeHelpers.GetHashCode(pair.Right))));

    /// <summary>
    /// Compares the header class ID, unknown user data and the node properties recursively.
    /// </summary>
    /// <remarks>
    /// Reading a lazily parsed member, such as the lightmap cache of a map, can change a stored node reference that the comparison
    /// reads before it. When only one side was read before, such as by C# expressions, that looks like a difference. So after
    /// a difference, both sides are read completely, which parses such members on both, and compared again.
    /// </remarks>
    /// <returns>The first difference as <c>Path from A to B.</c>, or null if there is none.</returns>
    public static string? FindDifference(Gbx original, Gbx reparsed, CancellationToken cancellationToken)
    {
        if (FindDifferenceOnce(original, reparsed, cancellationToken) is null)
        {
            return null;
        }

        var hasExternalReferences = ValueInspector.HasExternalReferences(original) || ValueInspector.HasExternalReferences(reparsed);
        var touched = new HashSet<object>(ReferenceEqualityComparer.Instance);

        ReadAll(original.Node, hasExternalReferences, touched, cancellationToken);
        ReadAll(reparsed.Node, hasExternalReferences, touched, cancellationToken);

        return FindDifferenceOnce(original, reparsed, cancellationToken);
    }

    /// <summary>
    /// Reads every member recursively, the same way the comparison does.
    /// </summary>
    private static void ReadAll(object? value, bool hasExternalReferences, HashSet<object> touched, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (value is null or string || value is not IEnumerable && ValueInspector.IsScalar(value)
            || !value.GetType().IsValueType && !touched.Add(value))
        {
            return;
        }

        // An external reference is never resolved, so no other file is loaded.
        if (value is NodeRef reference)
        {
            ReadAll(reference.Kind == NodeRefKind.Embedded ? reference.Node : null, hasExternalReferences, touched, cancellationToken);
            return;
        }

        // Collections that are structs, such as immutable arrays, are skipped, because a default one cannot be enumerated.
        if (value is IEnumerable && value.GetType().IsValueType)
        {
            return;
        }

        foreach (var member in ValueInspector.GetMembers(value, hasExternalReferences: hasExternalReferences))
        {
            ReadAll(member.Value, hasExternalReferences, touched, cancellationToken);
        }
    }

    private static string? FindDifferenceOnce(Gbx original, Gbx reparsed, CancellationToken cancellationToken)
    {
        var comparer = new GbxPropertyComparer(
            ValueInspector.HasExternalReferences(original) || ValueInspector.HasExternalReferences(reparsed), cancellationToken);

        return comparer.Compare(original.Header.ClassId, reparsed.Header.ClassId, "Header.ClassId")
            ?? comparer.Compare((original.Header as GbxHeaderUnknown)?.UserData, (reparsed.Header as GbxHeaderUnknown)?.UserData, "Header.UserData")
            ?? comparer.Compare(original.Node, reparsed.Node, "Node");
    }

    private string? Compare(object? left, object? right, string path)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (ReferenceEquals(left, right)) return null;
        if (left is null || right is null || left.GetType() != right.GetType()) return Changed(path, left, right);

        if (left is byte[] leftBytes && right is byte[] rightBytes)
        {
            var common = leftBytes.AsSpan().CommonPrefixLength(rightBytes);
            return common < Math.Min(leftBytes.Length, rightBytes.Length)
                ? Changed($"{path}[{common}]", leftBytes[common], rightBytes[common])
                : Compare(leftBytes.Length, rightBytes.Length, $"{path}.Length");
        }

        if (left is ITuple leftTuple && right is ITuple rightTuple)
        {
            for (var i = 0; i < leftTuple.Length; i++)
            {
                if (Compare(leftTuple[i], rightTuple[i], $"{path}.Item{i + 1}") is { } difference) return difference;
            }

            return null;
        }

        if (left is string or IPAddress || left is not IEnumerable && ValueInspector.IsScalar(left))
        {
            return left.Equals(right) ? null : Changed(path, left, right);
        }

        if (left.GetType().IsValueType)
        {
            // Default immutable arrays cannot be enumerated, but two default values compare equal.
            if (left is IEnumerable && left.Equals(right)) return null;
        }
        else if (!visited.Add((left, right)))
        {
            return null;
        }

        return (left, right) switch
        {
            (NodeRef a, NodeRef b) => Compare(a.Kind, b.Kind, $"{path}.Kind")
                ?? (a.Kind == NodeRefKind.Embedded ? Compare(a.Node, b.Node, path) : Compare(a.Descriptor, b.Descriptor, path)),
            (GbxRefTableFile a, GbxRefTableFile b) => Compare(a.FilePath.Replace('\\', '/'), b.FilePath.Replace('\\', '/'), $"{path}.FilePath")
                ?? Compare(a.RefTable.AncestorLevel, b.RefTable.AncestorLevel, $"{path}.AncestorLevel") ?? CompareDescriptor(a, b, path),
            (GbxRefTableResource a, GbxRefTableResource b) => Compare(a.ResourceIndex, b.ResourceIndex, $"{path}.ResourceIndex")
                ?? CompareDescriptor(a, b, path),
            (ZlibData a, ZlibData b) => CompareZlibData(a, b, path),
            (IDictionary a, IDictionary b) => CompareDictionary(a, b, path),
            (Exception a, Exception b) => Compare(a.Message, b.Message, $"{path}.Message"),
            (IEnumerable a, IEnumerable b) => CompareItems(a, b, path),
            _ => CompareMembers(left, right, path)
        };
    }

    /// <summary>
    /// Saved compressed data can differ while the data is the same, such as by compression level. Parsed data is compared through
    /// the members it was parsed into, and saving it again can change the compressed data nested in it. Otherwise, the
    /// decompressed data is compared.
    /// </summary>
    private string? CompareZlibData(ZlibData left, ZlibData right, string path) => left.Parsed && right.Parsed
        ? null
        : Compare(left.UncompressedSize, right.UncompressedSize, $"{path}.UncompressedSize")
            ?? Compare(Decompress(left), Decompress(right), $"{path}.Decompressed");

    private static byte[] Decompress(ZlibData data)
    {
        using var reader = data.OpenDecompressedReader();
        return reader.ReadBytes(data.UncompressedSize);
    }

    private string? CompareDescriptor(GbxRefTableNode left, GbxRefTableNode right, string path)
        => Compare(left.Flags, right.Flags, $"{path}.Flags") ?? Compare(left.UseFile, right.UseFile, $"{path}.UseFile");

    private string? CompareDictionary(IDictionary left, IDictionary right, string path)
    {
        if (left.Count != right.Count) return Changed($"{path}.Count", left.Count, right.Count);

        foreach (DictionaryEntry entry in left)
        {
            var entryPath = $"{path}[{ValueFormatter.Describe(entry.Key)}]";
            if (!right.Contains(entry.Key)) return $"{entryPath} (key missing after save).";
            if (Compare(entry.Value, right[entry.Key], entryPath) is { } difference) return difference;
        }

        return null;
    }

    private string? CompareItems(IEnumerable left, IEnumerable right, string path)
    {
        using var leftItems = GetItems(left).GetEnumerator();
        using var rightItems = GetItems(right).GetEnumerator();

        for (var index = 0; ; index++)
        {
            var hasLeft = leftItems.MoveNext();
            var hasRight = rightItems.MoveNext();
            if (hasLeft != hasRight) return $"{path} (collection length differs at index {index}).";
            if (!hasLeft) return null;

            var itemPath = leftItems.Current is IChunk chunk ? $"{path}[0x{chunk.Id:X8}]" : $"{path}[{index}]";
            if (Compare(leftItems.Current, rightItems.Current, itemPath) is { } difference) return difference;
        }
    }

    private static IEnumerable<object?> GetItems(IEnumerable collection) => collection switch
    {
        INodeRefList references => references.EnumerateReferences(),
        IEnumerable<IChunk> chunks => chunks.OrderBy(chunk => chunk.Id),
        _ => collection.Cast<object?>()
    };

    private string? CompareMembers(object left, object right, string path)
    {
        var leftMembers = ValueInspector.GetMembers(left, hasExternalReferences: hasExternalReferences);
        var rightMembers = ValueInspector.GetMembers(right, hasExternalReferences: hasExternalReferences);

        foreach (var (a, b) in leftMembers.Zip(rightMembers))
        {
            var memberPath = $"{path}.{a.Name}";
            if (a.Error != b.Error) return Changed($"{memberPath} (getter error)", a.Error, b.Error);
            if (a.Error is null && Compare(a.Value, b.Value, memberPath) is { } difference) return difference;
        }

        return null;
    }

    private static string Changed(string path, object? left, object? right)
        => $"{path} from {ValueFormatter.Clean(ValueFormatter.Describe(left))} to {ValueFormatter.Clean(ValueFormatter.Describe(right))}.";
}
