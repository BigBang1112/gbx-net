using GBX.NET;
using GBX.NET.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GbxDiscordBot;

internal sealed class Startup : IHostedService
{
    private readonly IServiceProvider _provider;
    private readonly IDiscordBot _bot;
    private readonly ILzo _lzo;
    private readonly ILogger<Startup> _logger;

    public Startup(
        IServiceProvider provider,
        IDiscordBot bot,
        ILzo lzo,
        ILogger<Startup> logger)
    {
        _provider = provider;
        _bot = bot;
        _lzo = lzo;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Gbx.LZO = _lzo;

        _logger.LogInformation("Syncing database...");

        await using var scope = _provider.CreateAsyncScope();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>().Database;

        if (db.IsRelational())
        {
            await db.MigrateAsync(cancellationToken);
        }

        _logger.LogInformation("Starting bot...");

        await _bot.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _bot.StopAsync();
    }
}