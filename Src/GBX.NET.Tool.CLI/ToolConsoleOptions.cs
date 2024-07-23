using System.Text.Json.Serialization;

namespace GBX.NET.Tool.CLI;

public sealed record ToolConsoleOptions
{
    public string IntroText { get; init; } = string.Empty;
    public JsonSerializerContext? JsonSerializerContext { get; init; }
}
