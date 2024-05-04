using GBX.NET;
using GbxDiscordBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace GbxDiscordBot.Services;

public interface IGbxService
{
    Task<GbxModel> LoadGbxAsync(ulong userId, string url, string fileName, GbxReadSettings settings, CancellationToken cancellationToken = default);
    Task<Gbx?> GetGbxObjectAsync(GbxModel gbxModel, CancellationToken cancellationToken = default);
}

internal sealed class GbxService : IGbxService
{
    private readonly HttpClient _http;
    private readonly AppDbContext _db;
    private readonly IUserService _userService;
    private readonly IMemoryCache _cache;
    private readonly ILogger<GbxService> _logger;

    public readonly TimeSpan GbxCacheDuration = TimeSpan.FromMinutes(5);

    public GbxService(HttpClient http, AppDbContext db, IUserService userService, IMemoryCache cache, ILogger<GbxService> logger)
    {
        _http = http;
        _db = db;
        _userService = userService;
        _cache = cache;
        _logger = logger;
    }

    public async Task<GbxModel> LoadGbxAsync(ulong userId, string url, string fileName, GbxReadSettings settings, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(url));
        ArgumentException.ThrowIfNullOrEmpty(nameof(fileName));

        using var response = await _http.GetAsync(url, cancellationToken);

        response.EnsureSuccessStatusCode(); // handle this better

        using var netStream = await response.Content.ReadAsStreamAsync(cancellationToken);

        var gbx = await Gbx.ParseAsync(netStream, settings, cancellationToken);
        gbx.FilePath = fileName;

        var guid = Guid.NewGuid();

        var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);

        var gbxModel = new GbxModel
        {
            Guid = guid,
            User = _userService.GetRequiredCurrentUser(),
            FileName = fileName,
            Data = bytes,
            ReadSettings = new GbxReadSettingsModel() // TODO: from GbxReadSettings settings
        };

        await _db.Gbxs.AddAsync(gbxModel, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        _cache.Set($"Gbx_{userId}_{guid}", gbx, GbxCacheDuration);

        return gbxModel;
    }

    public async Task<Gbx?> GetGbxObjectAsync(GbxModel gbxModel, CancellationToken cancellationToken = default)
    {
        var userId = gbxModel.User.UserId;

        var cacheKey = $"Gbx_{userId}_{gbxModel.Guid}";

        if (_cache.TryGetValue(cacheKey, out Gbx? gbx))
        {
            return gbx;
        }

        var model = await _db.Gbxs.FirstOrDefaultAsync(x => x.User.UserId == userId && x.Guid == gbxModel.Guid, cancellationToken);

        if (gbxModel is null)
        {
            return null;
        }

        using var stream = new MemoryStream(gbxModel.Data);

        gbx = await Gbx.ParseAsync(stream, gbxModel.ReadSettings.ToReadSettings(), cancellationToken);

        _cache.Set(cacheKey, gbx, GbxCacheDuration);

        return gbx;
    }

    public async Task<GbxModel?> GetGbxModelAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        return await _db.Gbxs.FirstOrDefaultAsync(x => x.Guid == guid, cancellationToken);
    }
}
