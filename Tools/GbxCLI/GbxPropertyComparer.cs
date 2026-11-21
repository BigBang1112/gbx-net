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

    public static string? FindDifference(Gbx original, Gbx reparsed, CancellationToken cancellationToken)
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
            (IDictionary a, IDictionary b) => CompareDictionary(a, b, path),
            (Exception a, Exception b) => Compare(a.Message, b.Message, $"{path}.Message"),
            (IEnumerable a, IEnumerable b) => CompareItems(a, b, path),
            _ => CompareMembers(left, right, path)
        };
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
