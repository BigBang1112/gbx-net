
using GBX.NET.Exceptions;
using Microsoft.Extensions.Logging;

namespace GBX.NET.Tool.CLI.InputArguments;

public sealed record UriInputArgument(HttpClient Http, Uri Uri) : InputArgument
{
    public override async Task<object?> ResolveAsync(ILogger logger, CancellationToken cancellationToken)
    {
        logger.LogInformation("Downloading {Uri}...", Uri);

        using var response = await Http.GetAsync(Uri, cancellationToken);

        response.EnsureSuccessStatusCode();

        try
        {
            logger.LogInformation("Reading {Uri} as Gbx...", Uri);
            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await Gbx.ParseAsync(stream, cancellationToken: cancellationToken);
        }
        catch (NotAGbxException)
        {
            logger.LogInformation("Reading {Uri} as data...", Uri);
            return await response.Content.ReadAsByteArrayAsync(cancellationToken);
        }
    }
}
