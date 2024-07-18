using System.Reflection;

namespace GBX.NET.Tool;

public sealed class ToolFunctionality<T> where T : ITool
{
    public required ConstructorInfo[] Constructors { get; init; }
}
