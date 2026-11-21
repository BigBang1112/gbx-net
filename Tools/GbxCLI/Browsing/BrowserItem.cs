namespace GbxCLI.Browsing;

/// <summary>
/// One numbered line in the browser: a member, a collection item or an archive entry.
/// </summary>
/// <param name="Description">Text after the name, without escaping.</param>
/// <param name="IsFormatted">Whether <paramref name="Description"/> can use Trackmania formatting, such as a map name.</param>
/// <param name="OpenEntryAsync">Opens an archive entry. Members and items have a <paramref name="Value"/> instead.</param>
internal sealed record BrowserItem(
    string Name,
    string Description,
    bool IsFormatted = false,
    object? Value = null,
    string? Error = null,
    Func<CancellationToken, Task<object>>? OpenEntryAsync = null);
