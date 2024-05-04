using Discord;

namespace GbxDiscordBot.Models;

public sealed class InteractionResponse
{
    public string? Message { get; set; }
    public Embed[]? Embeds { get; set; }
    public MessageComponent? Components { get; set; }
}
