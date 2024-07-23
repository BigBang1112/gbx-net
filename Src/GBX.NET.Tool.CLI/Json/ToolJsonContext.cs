using System.Text.Json.Serialization;

namespace GBX.NET.Tool.CLI;

[JsonSerializable(typeof(ConsoleSettings))]
[JsonSourceGenerationOptions(WriteIndented = true)]
internal sealed partial class ToolJsonContext : JsonSerializerContext;
