
using GBX.NET.Exceptions;
using Microsoft.Extensions.Logging;

namespace GBX.NET.Tool.CLI.InputArguments;

public sealed record DirectoryInputArgument(string DirectoryPath) : InputArgument
{
    public override Task<object?> ResolveAsync(ILogger logger, CancellationToken cancellationToken)
    {
        var files = Directory.EnumerateFiles(DirectoryPath, "*.*", SearchOption.AllDirectories);
        
        var tasks = files.Select<string, Task<object?>>(async file =>
        {
            try
            {
                logger.LogInformation("Reading file {File}...", Path.GetFileName(file));
                await using var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
                return await Gbx.ParseAsync(stream, cancellationToken: cancellationToken);
            }
            catch (NotAGbxException)
            {
                return await File.ReadAllBytesAsync(file, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to read file {File}.", Path.GetFileName(file));
                return null;
            }
        });

        return Task.FromResult((object?)tasks);
    }
}
