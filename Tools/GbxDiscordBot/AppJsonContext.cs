using GBX.NET;
using GBX.NET.Engines.MwFoundations;
using System.Text.Json.Serialization;

namespace GbxDiscordBot;

[JsonSerializable(typeof(CMwNod))]
[JsonSourceGenerationOptions(WriteIndented = true)]
internal sealed partial class AppJsonContext : JsonSerializerContext
{
}
