namespace GbxCLI;

/// <summary>
/// A value that <see cref="MemberPath.Assign"/> changed.
/// </summary>
/// <param name="Path">The resolved member path, such as <c>Node.MapName</c>.</param>
internal sealed record MemberChange(string Path, object? OldValue, object? NewValue);
