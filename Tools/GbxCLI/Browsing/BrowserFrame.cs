using GBX.NET.PAK;
using System.IO.Compression;

namespace GbxCLI.Browsing;

/// <summary>
/// One object shown in the browser, with its page and, for archives, its entry filter.
/// </summary>
/// <param name="Title">Path of the value from its file, such as <c>Node.Blocks[3]</c>, or the file name for a file.</param>
internal sealed class BrowserFrame(string title, object value, bool isFile = false)
{
    private IReadOnlyList<BrowserItem>? items;
    private string? filter;

    public string Title { get; } = title;

    public object Value { get; } = value;

    /// <summary>
    /// Whether the value is an opened file, so members of it start a new path.
    /// </summary>
    public bool IsFile { get; } = isFile;

    public int Page { get; set; }

    /// <summary>
    /// Number of members without a value, which are not listed.
    /// </summary>
    public int UnsetCount { get; private set; }

    public bool IsArchive => Value is Pak or ZipArchive;

    public string? Filter
    {
        get => filter;
        set
        {
            filter = value;
            items = null;
            Page = 0;
        }
    }

    public int PageCount => Math.Max(1, (Items.Count + Browser.PageSize - 1) / Browser.PageSize);

    public string TypeName => ValueFormatter.GetTypeName(Value.GetType());

    /// <summary>
    /// Path of a member of this value, such as <c>Node.Blocks</c> or <c>Node.Blocks[3]</c>.
    /// </summary>
    public string GetChildTitle(string name) => IsFile ? name : name.StartsWith('[') ? Title + name : $"{Title}.{name}";

    public IReadOnlyList<BrowserItem> Items => items ??= CreateItems();

    /// <summary>
    /// The archive that owns entries of this frame, set for archives opened by the browser.
    /// </summary>
    public FileObject? Owner { get; init; }

    private List<BrowserItem> CreateItems()
    {
        switch (Value)
        {
            case Pak pak:
                return [.. pak.Select(filter).Select(entry => new BrowserItem(
                    PathPattern.ToDisplay(entry.Path),
                    entry.IsUnstored ? "No stored data" : $"{entry.File.UncompressedSize:N0} bytes",
                    OpenEntryAsync: async cancellationToken => await FileObject.OpenEntryAsync(pak, entry, cancellationToken)))];
            case ZipArchive zip:
                return [.. zip.Select(filter).Select(entry => new BrowserItem(
                    PathPattern.ToDisplay(entry.FullName),
                    $"{entry.Length:N0} bytes",
                    OpenEntryAsync: cancellationToken => Owner!.OpenEntryAsync(entry, cancellationToken)))];
        }

        var isCollection = Value is System.Collections.IEnumerable;
        var members = ValueInspector.GetMembers(Value);

        if (!isCollection)
        {
            members = members.OrderBy(member => ConsoleValueWriter.IsComplex(member.Value) ? 1 : 0);
        }

        var result = new List<BrowserItem>();
        UnsetCount = 0;

        foreach (var member in members)
        {
            if (!isCollection && member.Value is null && member.Error is null)
            {
                UnsetCount++;
                continue;
            }

            if (ConsoleValueWriter.IsHiddenChunkMember(Value, member))
            {
                continue;
            }

            result.Add(member.Error is null
                ? new BrowserItem(member.Name, ValueFormatter.Describe(member.Value), member.IsFormatted && member.Value is string, member.Value)
                : new BrowserItem(member.Name, $"(error: {member.Error})", Error: member.Error));
        }

        return result;
    }
}
