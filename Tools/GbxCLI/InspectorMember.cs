namespace GbxCLI;

/// <param name="Error">Message of the exception thrown while reading the value, if any.</param>
internal sealed record InspectorMember(string Name, object? Value, string? Error = null);
