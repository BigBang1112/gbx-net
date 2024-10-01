
using GBX.NET.Exceptions;

namespace GBX.NET.Tool.CLI.InputArguments;

public sealed record UriInputArgument(HttpClient Http, Uri Uri) : InputArgument
{
    public override async Task<object?> ResolveAsync(CancellationToken cancellationToken)
    {
        using var response = await Http.GetAsync(Uri, cancellationToken);

        response.EnsureSuccessStatusCode();

        try
        {
            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await Gbx.ParseAsync(stream, cancellationToken: cancellationToken);
        }
        catch (NotAGbxException)
        {
            return await response.Content.ReadAsByteArrayAsync(cancellationToken);
        }
    }
}
