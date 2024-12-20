using GBX.NET.Tool.CLI;
using System.Text.Json.Serialization;

namespace NationsConverterWeb;

[JsonSerializable(typeof(UpdateInfo))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
internal sealed partial class GitHubJsonContext : JsonSerializerContext;
