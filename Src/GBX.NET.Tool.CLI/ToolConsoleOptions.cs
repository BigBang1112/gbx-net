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
    public YamlDotNet.Serialization.DeserializerBuilder? YmlDeserializer { get; init; }
    public YamlDotNet.Serialization.SerializerBuilder? YmlSerializer { get; init; }
    public YamlDotNet.Serialization.StaticContext? YmlContext { get; init; }

    /// <summary>
    /// GitHub repository identifier where the releases are located. Format: <c>user/repo</c>
    /// </summary>
    public string? GitHubRepo { get; set; }
}
