using GBX.NET;
using GBX.NET.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GbxDiscordBot;

internal sealed class Startup : IHostedService
{
    private readonly IDiscordBot _bot;
    private readonly ILzo _lzo;
    private readonly ICrc32 _crc32;
    private readonly ILogger<Startup> _logger;

    public Startup(
        IDiscordBot bot,
        ILzo lzo,
        ICrc32 crc32,
        ILogger<Startup> logger)
    {
        _bot = bot;
        _lzo = lzo;
        _crc32 = crc32;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Gbx.LZO = _lzo;
        Gbx.CRC32 = _crc32;

        _logger.LogInformation("Starting bot...");

        await _bot.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _bot.StopAsync();
    }
}