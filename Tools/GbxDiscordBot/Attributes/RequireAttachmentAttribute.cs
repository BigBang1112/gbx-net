using Discord;
using Discord.Interactions;

namespace GbxDiscordBot.Attributes;

public class RequireAttachmentAttribute : PreconditionAttribute
{
    public override Task<PreconditionResult> CheckRequirementsAsync(
        IInteractionContext context,
        ICommandInfo commandInfo,
        IServiceProvider services)
    {
        return Task.FromResult(CheckRequirements(context));
    }

    private static PreconditionResult CheckRequirements(IInteractionContext context)
    {
        if (context.Interaction.Data is not IMessageCommandInteractionData messageData)
        {
            return PreconditionResult.FromError("Message data not found. Only valid for message commands.");
        }

        var message = messageData.Message;

        if (message.Attachments.Count == 0)
        {
            return PreconditionResult.FromError($"No attachments found in {message.GetJumpUrl()} to use for Gbx inspection.");
        }

        return PreconditionResult.FromSuccess();
    }
}
