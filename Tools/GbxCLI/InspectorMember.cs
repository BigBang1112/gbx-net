namespace GbxCLI;

/// <param name="Error">Message of the exception thrown while reading the value, if any.</param>
/// <param name="IsFormatted">Whether the member is text that the game shows with Trackmania formatting, such as a map name.</param>
internal sealed record InspectorMember(string Name, object? Value, string? Error = null, bool IsFormatted = false);
