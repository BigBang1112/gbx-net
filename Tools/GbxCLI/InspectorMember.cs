namespace GbxCLI;

internal sealed record InspectorMember(string Name, Type Type, object? Value, string? Error = null);
