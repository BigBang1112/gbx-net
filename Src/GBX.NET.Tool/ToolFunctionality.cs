using System.Reflection;

namespace GBX.NET.Tool;

public sealed class ToolFunctionality<T> where T : ITool
{
    public required ConstructorInfo[] Constructors { get; init; }
    public required MethodInfo[] ProduceMethods { get; init; }
    public required MethodInfo[] MutateMethods { get; init; }
    public required Type? ConfigType { get; init; }
}
