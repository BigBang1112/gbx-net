namespace GBX.NET.Tool;

public sealed class ToolFunctionality<T> where T : ITool
{
    public required object?[] InputParameters { get; init; }
}
