using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;

namespace GbxDiscordBot;

public interface IDiscordBot : IAsyncDisposable, IDisposable
{
    Task StartAsync();
    Task StopAsync();

    Task<IUserMessage?> SendMessageAsync(ulong channelId, string? message = null, Embed? embed = null);
    Task<IThreadChannel?> CreateThreadAsync(ulong channelId, IMessage message, string name);
    Task<IUser?> GetUserAsync(ulong userId);
}

internal sealed class DiscordBot : IDiscordBot
{
    private readonly IServiceProvider _provider;
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _interactionService;
    private readonly IConfiguration _config;
    private readonly IHostEnvironment _env;
    private readonly ILogger<DiscordSocketClient> _logger;

    public DiscordBot(IServiceProvider provider,
        DiscordSocketClient client,
        InteractionService interactionService,
        IConfiguration config,
        IHostEnvironment env,
        ILogger<DiscordSocketClient> logger)
    {
        _provider = provider;
        _env = env;
        _client = client;
        _interactionService = interactionService;
        _config = config;
        _logger = logger;
    }

    public async Task StartAsync()
    {
        var token = _config["Discord:Token"] ?? throw new Exception("Token was not provided.");

        _logger.LogInformation("Starting bot...");
        _logger.LogInformation("Preparing modules...");

        _interactionService.Log += ClientLog;
        _interactionService.InteractionExecuted += InteractionExecuted;

        using var scope = _provider.CreateScope();
        await _interactionService.AddModulesAsync(typeof(Startup).Assembly, scope.ServiceProvider);

        _logger.LogInformation("Subscribing to events...");

        _client.Log += ClientLog;
        _client.InviteCreated += _ => Task.CompletedTask;
        _client.GuildScheduledEventCreated += _ => Task.CompletedTask;
        _client.Ready += ClientReady;
        _client.InteractionCreated += async interaction =>
        {
            var context = new SocketInteractionContext(_client, interaction);
            await _interactionService.ExecuteCommandAsync(context, _provider);
        };

        _logger.LogInformation("Loggin in...");

        await _client.LoginAsync(TokenType.Bot, _config["Discord:Token"]);

        _logger.LogInformation("Starting...");

        await _client.StartAsync();

        _logger.LogInformation("Started!");
    }

    public async Task StopAsync()
    {
        await _client.LogoutAsync();
        await _client.StopAsync();
    }

    public async Task<IUserMessage?> SendMessageAsync(ulong channelId, string? message = null, Embed? embed = null)
    {
        var channel = await _client.GetChannelAsync(channelId);

        if (channel is not IMessageChannel msgChannel)
        {
            return null;
        }

        return await msgChannel.SendMessageAsync(message, embed: embed);
    }

    public async Task<IThreadChannel?> CreateThreadAsync(ulong channelId, IMessage message, string name)
    {
        var channel = await _client.GetChannelAsync(channelId);

        if (channel is not ITextChannel textChannel)
        {
            return null;
        }

        return await textChannel.CreateThreadAsync(name, message: message);
    }

    public async Task<IUser?> GetUserAsync(ulong userId)
    {
        return await _client.GetUserAsync(userId);
    }

    private async Task ClientReady()
    {
        // Does not need to be called every Ready event
        await RegisterCommandsAsync(deleteMissing: true);
    }

    private Task ClientLog(LogMessage msg)
    {
        _logger.Log(msg.Severity switch
        {
            LogSeverity.Critical => LogLevel.Critical,
            LogSeverity.Error => LogLevel.Error,
            LogSeverity.Warning => LogLevel.Warning,
            LogSeverity.Info => LogLevel.Information,
            LogSeverity.Verbose => LogLevel.Debug,
            LogSeverity.Debug => LogLevel.Trace,
            _ => throw new NotImplementedException()
        },
            msg.Exception, "{message}", msg.Message ?? msg.Exception?.Message);

        return Task.CompletedTask;
    }

    private async Task InteractionExecuted(ICommandInfo commandInfo, IInteractionContext context, IResult result)
    {
        if (result.Error is null)
        {
            return;
        }

        switch (result)
        {
            case PreconditionResult precondition:
                await context.Interaction.RespondAsync(precondition.ErrorReason, ephemeral: true);
                break;
            case ExecuteResult executeResult:
                var sb = new StringBuilder("` ");
                sb.Append(executeResult.ErrorReason);
                sb.Append(" ` (");
                sb.Append(executeResult.Error);
                sb.Append(')');

                if (executeResult.Error is InteractionCommandError.Exception && executeResult.Exception.InnerException is not null)
                {
                    var innerException = executeResult.Exception.InnerException;

                    sb.AppendLine();
                    sb.AppendLine("```");
                    sb.Append(innerException.GetType().Name);
                    sb.Append(": ");

                    if (!string.IsNullOrEmpty(innerException.Message))
                    {
                        if (innerException.Message.Length > 1000)
                        {
                            sb.Append(executeResult.Exception.InnerException.Message.AsSpan(0, 1000));
                            sb.Append("...");
                        }
                        else
                        {
                            sb.Append(executeResult.Exception.InnerException.Message);
                        }
                    }

                    sb.AppendLine("```");
                    sb.Append("The full error has been sent to the owner.");
                }

                await context.Interaction.RespondAsync(embed: new EmbedBuilder()
                    .WithTitle("Error")
                    .WithDescription(sb.ToString())
                    .WithColor(Color.Red)
                    .Build(),
                    ephemeral: true);

                break;
        }
    }

    private async Task RegisterCommandsAsync(bool deleteMissing = true)
    {
        _logger.LogInformation("Registering commands...");

        if (_env.IsDevelopment())
        {
            await _interactionService.RegisterCommandsToGuildAsync(ulong.Parse(_config["Discord:TestGuildId"] ?? throw new Exception("Discord:TestGuildId was not provided.")), deleteMissing);
        }
        else
        {
            await _interactionService.RegisterCommandsGloballyAsync(deleteMissing);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _client.DisposeAsync();
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}