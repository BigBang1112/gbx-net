using Discord.Interactions;
using Discord;

namespace GbxDiscordBot.Modules;

public sealed class GbxModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("gbx", "Inspect a Gbx file.")]
    public async Task Gbx(IAttachment file)
    {

    }
}