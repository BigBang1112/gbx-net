using System.Diagnostics.CodeAnalysis;

namespace GBX.NET.Tool.CLI;

public sealed record ToolConsoleRunResult<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T>(ToolConsole<T> Tool)
    where T : class, ITool;
