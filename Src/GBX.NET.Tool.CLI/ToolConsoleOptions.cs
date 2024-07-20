using System.Text.Json.Serialization;

namespace GBX.NET.Tool.CLI;

public sealed record ToolConsoleOptions
{
    public JsonSerializerContext? JsonSerializerContext { get; init; }
}
