using GBX.NET;
using GBX.NET.Exceptions;
using GbxExplorerOld.Client.Exceptions;
using GbxExplorerOld.Client.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Text;

namespace GbxExplorerOld.Client.Services;

public class GbxService : IGbxService
{
    private static readonly byte[] gbxMagic = Encoding.ASCII.GetBytes("GBX");

    private readonly IJSRuntime _js;
    private readonly ISettingsService _settings;
    private readonly ILogger _logger;

    public List<GbxModel> List { get; } = new();
    public GbxModel? SelectedGbx { get; set; }

    public GbxService(IJSRuntime js, ISettingsService settings, ILogger logger)
    {
        _js = js;
        _settings = settings;
        _logger = logger;
    }

    public async Task<GbxModelBase?> ImportGbxAsync(IBrowserFile file,
                                                    Func<string, Task> asyncEvent,
                                                    CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("File size: {size} bytes", file.Size);
        _logger.LogDebug("Last modified: {lastModified}", file.LastModified.DateTime);
        _logger.LogInformation("Opening the file stream...");

        using var generalStream = file.OpenReadStream(_settings.MaxAllowedFileSize, cancellationToken);

        _logger.LogInformation("Verifying that the stream is a Gbx stream...");

        using var gbxStream = await VerifyAndCreateGbxStreamAsync(generalStream, cancellationToken);

        if (gbxStream is null)
        {
            _logger.LogWarning("{file} is not a Gbx file - missing 'GBX' magic.", file.Name);

            return null;
        }

        await asyncEvent("OnCopyToMemoryStreamAsync");

        _logger.LogInformation("Calculating SHA256...");

        var sha256 = await _js.InvokeAsync<string>("sha256", gbxStream.ToArray());

        await asyncEvent("OnCalculateHashAsync");
        
        _logger.LogDebug("SHA256: {sha256}", sha256);

        foreach (var existingGbx in List)
        {
            if (existingGbx.Sha256 == sha256)
            {
                _logger.LogInformation("This Gbx file is already in the list.");
                
                throw new ExistingGbxInListException();
            }
        }

        gbxStream.Position = 0;
        
        _logger.LogInformation("File is unique. Importing the Gbx...");

        try
        {
            Gbx gbx;

            using (var gbxnetScope = _logger.BeginScope("GBX.NET"))
            {
                gbx = await Gbx.ParseAsync(gbxStream, new()
                {
                    Logger = _logger,
                    OpenPlanetHookExtractMode = _settings.OpenPlanetHookExtractMode,
                    IgnoreExceptionsInBody = true
                }, cancellationToken: cancellationToken);
            }

            _logger.LogInformation("Gbx successfully imported!");

            return new GbxModel(file.Name, file.LastModified, gbxStream.ToArray(), sha256, gbx);
        }
        catch (NotAGbxException)
        {
            _logger.LogWarning("{file} is not a Gbx file - missing 'GBX' magic.", file.Name);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Import has thrown {exName} -> {msg}", ex.GetType().Name, ex.Message);

            return new FaultyGbxModel(file.Name, file.LastModified, gbxStream.ToArray(), sha256, ex);
        }
    }

    private static async Task<MemoryStream?> VerifyAndCreateGbxStreamAsync(Stream stream, CancellationToken cancellationToken)
    {
        var magicBuffer = new byte[3];
        await stream.ReadAsync(magicBuffer, cancellationToken);

        if (!magicBuffer.SequenceEqual(gbxMagic))
        {
            return null;
        }
        
        var ms = new MemoryStream();

        await ms.WriteAsync(magicBuffer, cancellationToken);

        await stream.CopyToAsync(ms, cancellationToken);
        
        return ms;
    }
}
