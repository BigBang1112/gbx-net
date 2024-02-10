using GBX.NET;
using GBX.NET.LZO;
using GBX.NET.Hashing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GbxDiscordBot;

internal sealed class Startup : IHostedService
{
    private readonly IDiscordBot _bot;
    private readonly ILogger<Startup> _logger;

    public Startup(
        IDiscordBot bot,
        ILogger<Startup> logger)
    {
        _bot = bot;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Gbx.LZO = new MiniLZO();
        Gbx.CRC32 = new CRC32();

        _logger.LogInformation("Starting bot...");

        await _bot.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _bot.StopAsync();
    }
}