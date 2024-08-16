using System.Text.Json;
using System.Text.Json.Serialization;

namespace GBX.NET.Tool.CLI;

public sealed record ToolConsoleOptions
{
    public string IntroText { get; init; } = string.Empty;
    public JsonSerializerContext? JsonContext { get; init; }
    public JsonSerializerOptions JsonOptions { get; init; } = new()
    {
        WriteIndented = true
    };

    public YamlDotNet.Serialization.IDeserializer? YmlDeserializer { get; init; }
    public YamlDotNet.Serialization.ISerializer? YmlSerializer { get; init; }
}
