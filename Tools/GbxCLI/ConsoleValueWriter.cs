using GBX.NET;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Serialization.Chunking;
using System.Collections;

namespace GbxCLI;

/// <summary>
/// Writes a value and its members as an indented tree, within limits that keep the output readable.
/// </summary>
internal sealed class ConsoleValueWriter(CliConsole console)
{
    public const int CollectionLimit = 10;
    public const int ChunkLimit = 100;
    public const int DepthLimit = 4;
    public const int LineLimit = 1000;

    private readonly HashSet<object> visited = new(ReferenceEqualityComparer.Instance);
    private int lines;

    /// <inheritdoc cref="ValueInspector.GetMembers" path="/param[@name='hasExternalReferences']"/>
    public bool HasExternalReferences { get; set; } = true;

    public async Task WriteResultAsync(object? value)
    {
        await WriteValueAsync("Result", value, 0);
        await WriteTruncationNoticeAsync();
    }

    public async Task WriteTruncationNoticeAsync()
    {
        if (lines >= LineLimit)
        {
            await console.WriteLineAsync($"{CliConsole.Label}  ... Detail line limit reached.{CliConsole.Reset}");
        }
    }

    /// <param name="formatted">Whether the value is a member marked with <see cref="GBX.NET.Attributes.SupportsFormattingAttribute"/>.</param>
    public async Task WriteValueAsync(string name, object? value, int depth, bool formatted = false)
    {
        if (lines >= LineLimit)
        {
            return;
        }

        await LineAsync(name, value, depth, formatted: formatted && value is string);

        // "0 items" already says everything about an empty collection, and ("Id", "Collection", "Author") about an Ident.
        if (value is null || ValueInspector.IsScalar(value) || value is NodeRef { Kind: not NodeRefKind.Embedded } or ICollection { Count: 0 } or Ident)
        {
            return;
        }

        if (!visited.Add(value))
        {
            await LineAsync("...", "Already shown (shared or circular reference)", depth + 1);
            return;
        }

        if (depth >= DepthLimit)
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

        await WriteMembersAsync(value, depth);
    }

    private async Task WriteMembersAsync(object value, int depth)
    {
        var isCollection = value is IEnumerable;
        var limit = value is IChunkSet or IHeaderChunkSet ? ChunkLimit : CollectionLimit;
        var members = ValueInspector.GetMembers(value, hasExternalReferences: HasExternalReferences);

        if (!isCollection)
        {
            // Keep scalar properties visible before expanding potentially large subtrees.
            members = members.OrderBy(member => IsComplex(member.Value) ? 1 : 0);
        }

        var count = 0;
        var nullCount = 0;

        foreach (var member in members)
        {
            if (lines >= LineLimit)
            {
                break;
            }

            if (isCollection && count++ >= limit)
            {
                await LineAsync("...", $"More items (showing first {limit})", depth + 1);
                break;
            }

            if (!isCollection && member.Value is null && member.Error is null)
            {
                nullCount++;
                continue;
            }

            if (IsHiddenChunkMember(value, member))
            {
                continue;
            }

            if (member.Error is not null)
            {
                await LineAsync(member.Name, $"(error: {member.Error})", depth + 1);
            }
            else
            {
                await WriteValueAsync(member.Name, member.Value, depth + 1, member.IsFormatted);
            }
        }

        if (nullCount > 0 && lines < LineLimit)
        {
            await LineAsync("Unset properties", nullCount, depth + 1);
        }
    }

    /// <summary>
    /// Writes one <c>name: value</c> line.
    /// </summary>
    /// <param name="formatted">Write the value as text that can use Trackmania formatting, so map names show in their in-game colors.</param>
    public async Task LineAsync(string name, object? value, int depth = 1, bool formatted = false)
    {
        if (lines >= LineLimit)
        {
            return;
        }

        var text = ValueFormatter.Clean(ValueFormatter.Describe(value));
        text = formatted ? console.Formatted(text) : CliConsole.Escape(text);

        await console.WriteLineAsync($"{new string(' ', depth * 2)}{CliConsole.Label}{CliConsole.Escape(name)}:{CliConsole.Reset} {text}");
        lines++;
    }

    /// <summary>
    /// Whether a member opens a subtree, so it is listed after the scalar members.
    /// </summary>
    public static bool IsComplex(object? value)
        => value is IEnumerable and not string and not byte[] or CMwNod or NodeRef { Kind: NodeRefKind.Embedded };

    /// <summary>
    /// The chunk ID, version and owning node are already part of the chunk description.
    /// </summary>
    public static bool IsHiddenChunkMember(object owner, InspectorMember member)
        => owner is IChunk && member.Name is "Id" or "Version" or "Node";
}
