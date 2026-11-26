using GBX.NET;
using GBX.NET.Components;
using GBX.NET.Serialization.Chunking;
using System.Collections;
using System.Net;
using System.Runtime.CompilerServices;

namespace GbxCLI;

/// <summary>
/// Compares two Gbx files property by property, for <c>diff</c>, <c>verify --roundtrip</c> and the check before a Gbx is written.
/// </summary>
internal sealed class GbxPropertyComparer
{
    private readonly HashSet<(object Left, object Right)> visited = new(EqualityComparer<(object Left, object Right)>.Create(
        (a, b) => ReferenceEquals(a.Left, b.Left) && ReferenceEquals(a.Right, b.Right),
        pair => HashCode.Combine(RuntimeHelpers.GetHashCode(pair.Left), RuntimeHelpers.GetHashCode(pair.Right))));

    private readonly List<GbxDifference> differences = [];
    private readonly bool hasExternalReferences;
    private readonly int limit;
    private readonly CancellationToken cancellationToken;

    private GbxPropertyComparer(bool hasExternalReferences, int limit, CancellationToken cancellationToken)
    {
        this.hasExternalReferences = hasExternalReferences;
        this.limit = limit;
        this.cancellationToken = cancellationToken;
    }

    private bool IsFull => differences.Count >= limit;

    /// <summary>
    /// Finds the first difference, as <see cref="FindDifferences"/> does.
    /// </summary>
    /// <returns>The first difference as <c>Path from A to B.</c>, or null if there is none.</returns>
    public static string? FindDifference(Gbx left, Gbx right, CancellationToken cancellationToken)
        => FindDifferences(left, right, limit: 1, cancellationToken).FirstOrDefault()?.ToSentence();

    /// <summary>
    /// Compares the header class ID, unknown user data and the node properties recursively, and lists the differences in order.
    /// </summary>
    /// <remarks>
    /// Reading a lazily parsed member, such as the lightmap cache of a map, can change a stored node reference that the comparison
    /// reads before it. When only one side was read before, such as by C# expressions, that looks like a difference. So after
    /// a difference, both sides are read completely, which parses such members on both, and compared again.
    /// </remarks>
    /// <param name="limit">The number of differences after which the comparison stops.</param>
    public static IReadOnlyList<GbxDifference> FindDifferences(Gbx left, Gbx right, int limit = int.MaxValue, CancellationToken cancellationToken = default)
    {
        var hasExternalReferences = ValueInspector.HasExternalReferences(left) || ValueInspector.HasExternalReferences(right);

        if (FindDifferencesOnce(left, right, hasExternalReferences, limit, cancellationToken) is { Count: 0 } none)
        {
            return none;
        }

        var touched = new HashSet<object>(ReferenceEqualityComparer.Instance);

        ReadAll(left.Node, hasExternalReferences, touched, cancellationToken);
        ReadAll(right.Node, hasExternalReferences, touched, cancellationToken);

        return FindDifferencesOnce(left, right, hasExternalReferences, limit, cancellationToken);
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

    private static List<GbxDifference> FindDifferencesOnce(Gbx left, Gbx right, bool hasExternalReferences, int limit, CancellationToken cancellationToken)
    {
        var comparer = new GbxPropertyComparer(hasExternalReferences, limit, cancellationToken);

        comparer.Compare(left.Header.ClassId, right.Header.ClassId, "Header.ClassId");
        comparer.Compare((left.Header as GbxHeaderUnknown)?.UserData, (right.Header as GbxHeaderUnknown)?.UserData, "Header.UserData");
        comparer.Compare(left.Node, right.Node, "Node");

        return comparer.differences;
    }

    private void Compare(object? left, object? right, string path)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (IsFull || ReferenceEquals(left, right)) return;

        if (left is null || right is null || left.GetType() != right.GetType())
        {
            Changed(path, left, right);
            return;
        }

        if (left is byte[] leftBytes && right is byte[] rightBytes)
        {
            CompareBytes(leftBytes, rightBytes, path);
            return;
        }

        if (left is ITuple leftTuple && right is ITuple rightTuple)
        {
            for (var i = 0; i < leftTuple.Length; i++)
            {
                Compare(leftTuple[i], rightTuple[i], $"{path}.Item{i + 1}");
            }

            return;
        }

        if (left is string or IPAddress || left is not IEnumerable && ValueInspector.IsScalar(left))
        {
            if (!left.Equals(right)) Changed(path, left, right);
            return;
        }

        if (left.GetType().IsValueType)
        {
            // Default immutable arrays cannot be enumerated, but two default values compare equal.
            if (left is IEnumerable && left.Equals(right)) return;
        }
        else if (!visited.Add((left, right)))
        {
            return;
        }

        switch (left, right)
        {
            case (NodeRef a, NodeRef b):
                if (a.Kind != b.Kind) Changed($"{path}.Kind", a.Kind, b.Kind);
                else if (a.Kind == NodeRefKind.Embedded) Compare(a.Node, b.Node, path);
                else Compare(a.Descriptor, b.Descriptor, path);
                break;
            case (GbxRefTableFile a, GbxRefTableFile b):
                Compare(a.FilePath.Replace('\\', '/'), b.FilePath.Replace('\\', '/'), $"{path}.FilePath");
                Compare(a.RefTable.AncestorLevel, b.RefTable.AncestorLevel, $"{path}.AncestorLevel");
                CompareDescriptor(a, b, path);
                break;
            case (GbxRefTableResource a, GbxRefTableResource b):
                Compare(a.ResourceIndex, b.ResourceIndex, $"{path}.ResourceIndex");
                CompareDescriptor(a, b, path);
                break;
            case (ZlibData a, ZlibData b):
                CompareZlibData(a, b, path);
                break;
            case (IDictionary a, IDictionary b):
                CompareDictionary(a, b, path);
                break;
            case (Exception a, Exception b):
                Compare(a.Message, b.Message, $"{path}.Message");
                break;
            case (IEnumerable<IChunk> a, IEnumerable<IChunk> b):
                CompareChunks(a, b, path);
                break;
            case (IEnumerable a, IEnumerable b):
                CompareItems(a, b, path);
                break;
            default:
                CompareMembers(left, right, path);
                break;
        }
    }

    /// <summary>
    /// Reports the first different byte, or else a different length, so a large array gives one line.
    /// </summary>
    private void CompareBytes(byte[] left, byte[] right, string path)
    {
        var common = left.AsSpan().CommonPrefixLength(right);

        if (common < Math.Min(left.Length, right.Length))
        {
            Changed($"{path}[{common}]", left[common], right[common]);
        }
        else if (left.Length != right.Length)
        {
            Changed($"{path}.Length", left.Length, right.Length);
        }
    }

    /// <summary>
    /// Saved compressed data can differ while the data is the same, such as by compression level. Parsed data is compared through
    /// the members it was parsed into, and saving it again can change the compressed data nested in it. Otherwise, the
    /// decompressed data is compared.
    /// </summary>
    private void CompareZlibData(ZlibData left, ZlibData right, string path)
    {
        if (left.Parsed && right.Parsed)
        {
            return;
        }

        if (left.UncompressedSize != right.UncompressedSize)
        {
            Changed($"{path}.UncompressedSize", left.UncompressedSize, right.UncompressedSize);
            return;
        }

        CompareBytes(Decompress(left), Decompress(right), $"{path}.Decompressed");
    }

    private static byte[] Decompress(ZlibData data)
    {
        using var reader = data.OpenDecompressedReader();
        return reader.ReadBytes(data.UncompressedSize);
    }

    private void CompareDescriptor(GbxRefTableNode left, GbxRefTableNode right, string path)
    {
        Compare(left.Flags, right.Flags, $"{path}.Flags");
        Compare(left.UseFile, right.UseFile, $"{path}.UseFile");
    }

    private void CompareDictionary(IDictionary left, IDictionary right, string path)
    {
        foreach (DictionaryEntry entry in left)
        {
            var entryPath = $"{path}[{ValueFormatter.Describe(entry.Key)}]";

            if (right.Contains(entry.Key))
            {
                Compare(entry.Value, right[entry.Key], entryPath);
            }
            else
            {
                Report(entryPath, Describe(entry.Value), GbxDifference.Missing);
            }
        }

        foreach (DictionaryEntry entry in right)
        {
            if (!left.Contains(entry.Key))
            {
                Report($"{path}[{ValueFormatter.Describe(entry.Key)}]", GbxDifference.Missing, Describe(entry.Value));
            }
        }
    }

    /// <summary>
    /// Matches chunks by ID, so a chunk that only one side has doesn't shift the others.
    /// </summary>
    private void CompareChunks(IEnumerable<IChunk> left, IEnumerable<IChunk> right, string path)
    {
        var rightChunks = right.GroupBy(chunk => chunk.Id).ToDictionary(group => group.Key, group => group.First());
        var leftIds = new HashSet<uint>();

        foreach (var chunk in left.OrderBy(chunk => chunk.Id))
        {
            leftIds.Add(chunk.Id);
            var chunkPath = $"{path}[0x{chunk.Id:X8}]";

            if (rightChunks.TryGetValue(chunk.Id, out var other))
            {
                Compare(chunk, other, chunkPath);
            }
            else
            {
                Report(chunkPath, Describe(chunk), GbxDifference.Missing);
            }
        }

        foreach (var chunk in rightChunks.Values.Where(chunk => !leftIds.Contains(chunk.Id)).OrderBy(chunk => chunk.Id))
        {
            Report($"{path}[0x{chunk.Id:X8}]", GbxDifference.Missing, Describe(chunk));
        }
    }

    /// <summary>
    /// Compares the items at the same index, then reports the count if one side has more.
    /// </summary>
    private void CompareItems(IEnumerable left, IEnumerable right, string path)
    {
        using var leftItems = GetItems(left).GetEnumerator();
        using var rightItems = GetItems(right).GetEnumerator();

        var index = 0;
        bool hasLeft, hasRight;

        while ((hasLeft = leftItems.MoveNext()) & (hasRight = rightItems.MoveNext()))
        {
            Compare(leftItems.Current, rightItems.Current, $"{path}[{index++}]");
        }

        if (hasLeft || hasRight)
        {
            var leftCount = index + (hasLeft ? 1 + Count(leftItems) : 0);
            var rightCount = index + (hasRight ? 1 + Count(rightItems) : 0);
            Report($"{path}.Count", $"{leftCount:N0} items", $"{rightCount:N0} items");
        }
    }

    private static int Count(IEnumerator<object?> items)
    {
        var count = 0;

        while (items.MoveNext())
        {
            count++;
        }

        return count;
    }

    private static IEnumerable<object?> GetItems(IEnumerable collection) => collection switch
    {
        INodeRefList references => references.EnumerateReferences(),
        _ => collection.Cast<object?>()
    };

    private void CompareMembers(object left, object right, string path)
    {
        var leftMembers = ValueInspector.GetMembers(left, hasExternalReferences: hasExternalReferences);
        var rightMembers = ValueInspector.GetMembers(right, hasExternalReferences: hasExternalReferences);

        foreach (var (a, b) in leftMembers.Zip(rightMembers))
        {
            if (IsFull) return;

            var memberPath = $"{path}.{a.Name}";

            if (a.Error != b.Error)
            {
                Changed($"{memberPath} (getter error)", a.Error, b.Error);
            }
            else if (a.Error is null)
            {
                Compare(a.Value, b.Value, memberPath);
            }
        }
    }

    private void Changed(string path, object? left, object? right) => Report(path, Describe(left), Describe(right));

    private void Report(string path, string left, string right)
    {
        if (!IsFull)
        {
            differences.Add(new GbxDifference(path, left, right));
        }
    }

    private static string Describe(object? value) => ValueFormatter.Clean(ValueFormatter.Describe(value));
}
