using Discord;
using Discord.Interactions;
using GbxDiscordBot.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GbxDiscordBot.Attributes;

public class RequireAllowedUserAttribute : PreconditionAttribute
{
    public override async Task<PreconditionResult> CheckRequirementsAsync(
        IInteractionContext context,
        ICommandInfo commandInfo,
        IServiceProvider services)
    {
        if (context.User is null)
        {
            return PreconditionResult.FromError("User not found.");
        }

        var userService = services.GetRequiredService<IUserService>();
        var userModel = await userService.GetOrCreateUserAsync(context.User);

        if (userModel.IsBanned)
        {
            return PreconditionResult.FromError("User is banned.");
        }

        // handle rate limits here (3 per hour for everyone, 100 per hour for supporters)

        return PreconditionResult.FromSuccess();
    }
}
