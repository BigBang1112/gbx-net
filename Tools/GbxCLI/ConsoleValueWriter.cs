using GBX.NET;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Serialization.Chunking;
using Spectre.Console;
using System.Collections;
using System.Globalization;

namespace GbxCLI;

internal sealed class ConsoleValueWriter(CliConsole console)
{
    private const int collectionLimit = 10;
    private const int depthLimit = 4;
    private const int lineLimit = 1000;
    private const int textLimit = 240;

    private readonly HashSet<object> visited = new(ReferenceEqualityComparer.Instance);
    private int lines;

    public async Task WriteResultAsync(object? value)
    {
        await WriteValueAsync("Result", value, 0);
        await WriteTruncationNoticeAsync();
    }

    public async Task WriteTruncationNoticeAsync()
    {
        if (lines >= lineLimit)
        {
            await console.WriteLineAsync("[grey]  ... Detail line limit reached.[/]");
        }
    }

    public async Task WriteValueAsync(string name, object? value, int depth)
    {
        if (lines >= lineLimit)
        {
            return;
        }

        var description = Describe(value);
        await LineAsync(name, description, depth);

        if (value is null || value is byte[] || !FileInspector.CanBrowse(value)
            || value.GetType().IsValueType || value is NodeRef { Kind: not NodeRefKind.Embedded })
        {
            return;
        }

        if (!visited.Add(value))
        {
            await LineAsync("...", "Already shown (shared or circular reference)", depth + 1);
            return;
        }

        if (depth >= depthLimit)
        {
            await LineAsync("...", "Nesting limit reached", depth + 1);
            return;
        }

        if (value is NodeRef reference)
        {
            // Embedded handles never need an external resolver.
            await WriteValueAsync("Node", reference.Node, depth + 1);
            return;
        }

        var isCollection = value is IEnumerable;
        var limit = value is IChunkSet or IHeaderChunkSet ? 100 : collectionLimit;
        var count = 0;

        var entries = FileInspector.GetEntries(value, useStoredReferences: true);
        if (!isCollection)
        {
            // Keep scalar properties visible before expanding potentially large subtrees.
            entries = entries.OrderBy(entry => entry.Value is IEnumerable and not string and not byte[]
                or CMwNod or NodeRef { Kind: NodeRefKind.Embedded } ? 1 : 0);
        }

        var nullCount = 0;
        foreach (var entry in entries)
        {
            if (lines >= lineLimit)
            {
                break;
            }

            if (isCollection && count++ >= limit)
            {
                await LineAsync("...", $"More items (showing first {limit})", depth + 1);
                break;
            }

            if (!isCollection && entry.Value is null && entry.Error is null)
            {
                nullCount++;
                continue;
            }

            if (value is IChunk && entry.Name is ("Id" or "Version" or "Node"))
            {
                continue;
            }

            if (entry.Error is not null)
            {
                await LineAsync(entry.Name, $"(error: {entry.Error})", depth + 1);
            }
            else
            {
                await WriteValueAsync(entry.Name, entry.Value, depth + 1);
            }
        }

        if (nullCount > 0 && lines < lineLimit)
        {
            await LineAsync("Unset properties", nullCount, depth + 1);
        }
    }

    private static string Describe(object? value) => value switch
    {
        null => "(null)",
        string text => text.Length == 0 ? "\"\"" : text,
        byte[] bytes => $"{bytes.Length:N0} bytes (hex): {Convert.ToHexString(bytes.AsSpan(0, Math.Min(bytes.Length, 32)))}{(bytes.Length > 32 ? " ..." : "")}",
        NodeRef { Kind: NodeRefKind.None } => "(null)",
        NodeRef reference => reference.Kind == NodeRefKind.Embedded ? "Embedded node"
            : $"{reference.Kind}, {reference.State}: {reference.Descriptor}",
        CMwNod node => node.GetType().Name,
        IChunk chunk => $"0x{chunk.Id:X8} ({chunk.GetType().Name}){(chunk is IVersionable version ? $", version {version.Version}" : "")}",
        ICollection collection => $"{collection.Count:N0} items",
        IEnumerable => value.GetType().GetProperty("Count")?.GetValue(value) is int count ? $"{count:N0} items" : "Collection",
        Exception exception => exception.Message,
        _ => Convert.ToString(value, CultureInfo.InvariantCulture) ?? "(null)"
    };

    public async Task LineAsync(string name, object? value, int depth = 1)
    {
        if (lines >= lineLimit)
        {
            return;
        }

        var text = value is null ? "(null)" : Convert.ToString(value, CultureInfo.InvariantCulture) ?? "(null)";
        text = text.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t").Replace("\u001b", "\\u001b");
        if (text.Length > textLimit)
        {
            text = $"{text[..textLimit]} ... ({text.Length:N0} characters)";
        }

        await console.WriteLineAsync($"{new string(' ', depth * 2)}[grey]{Markup.Escape(name)}:[/] {Markup.Escape(text)}");
        lines++;
    }
}
