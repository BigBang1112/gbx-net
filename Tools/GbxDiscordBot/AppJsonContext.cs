using GbxDiscordBot.Models;
using System.Text.Json.Serialization;

namespace GbxDiscordBot;

[JsonSerializable(typeof(GbxReadSettingsModel))]
internal sealed partial class AppJsonContext : JsonSerializerContext
{
}
