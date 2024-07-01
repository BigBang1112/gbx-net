using Discord.Interactions;
using Discord;
using GbxDiscordBot.Services;
using GBX.NET;
using GbxDiscordBot.Attributes;

namespace GbxDiscordBot.Modules;

public sealed class GbxModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IGbxService _gbx;
    private readonly IResponseService _response;

    public GbxModule(IGbxService gbx, IResponseService response)
    {
        _gbx = gbx;
        _response = response;
    }

    [RequireAllowedUser]
    [SlashCommand("gbx", "Inspect a Gbx file.")]
    public async Task Gbx(IAttachment file, bool secretly = false)
    {
        await InspectAsync(file, secretly);
    }

    [RequireAllowedUser]
    [RequireAttachment]
    [MessageCommand("Inspect Gbx...")]
    public async Task GbxContext(IMessage message)
    {
        await InspectAsync(message.Attachments.First(), inspectedMessage: message);
    }

    [RequireAllowedUser]
    [RequireAttachment]
    [MessageCommand("Inspect Gbx... (secretly)")]
    public async Task GbxContextEphemeral(IMessage message)
    {
        await InspectAsync(message.Attachments.First(), secretly: true, inspectedMessage: message);
    }

    private async Task InspectAsync(IAttachment file, bool secretly = false, IMessage? inspectedMessage = null)
    {
        await DeferAsync(secretly);

        var gbxModel = await _gbx.LoadGbxAsync(Context.User.Id, file.Url, file.Filename, new GbxReadSettings()
        {
            IgnoreExceptionsInBody = true // in case of an exception in body, disallow modification
        });

        var gbx = await _gbx.GetGbxObjectAsync(gbxModel);

        if (gbx is null)
        {
            // gbx has been discarded very early or weirdly timed out
            var ir = await _response.UnavailableAsync(gbxModel);
            await FollowupAsync(ir.Message, ir.Embeds, ephemeral: secretly);
            return;
        }

        if (gbx.Node is null)
        {
            // show gbx properties response instead of main node response
            var ir = await _response.GbxPropertiesAsync(gbxModel);
            await FollowupAsync(ir.Message, ir.Embeds, ephemeral: secretly);
            return;
        }

        // show main node response
        var response = await _response.MainNodeAsync(gbx, gbx.Node, gbxModel, inspectedMessage);

        if (response.Attachments?.Any() == true)
        {
            await FollowupWithFilesAsync(response.Attachments, response.Message, response.Embeds, components: response.Components, ephemeral: secretly);
        }
        else
        {
            await FollowupAsync(response.Message, response.Embeds, components: response.Components, ephemeral: secretly);
        }
    }

    [ComponentInteraction("discard")]
    public async Task Discard()
    {
        await DeferAsync();

        // _gbx.RemoveGbxAsync()

        await DeleteOriginalResponseAsync();
    }
}