using System.Text.Json.Serialization;

namespace GBX.NET.Tool.CLI;

[JsonSerializable(typeof(ConsoleOptions))]
[JsonSourceGenerationOptions(WriteIndented = true)]
internal sealed partial class MainJsonContext : JsonSerializerContext;
