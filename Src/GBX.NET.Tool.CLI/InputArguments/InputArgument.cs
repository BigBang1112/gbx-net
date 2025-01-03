
using Microsoft.Extensions.Logging;

namespace GBX.NET.Tool.CLI.InputArguments;

public abstract record InputArgument
{
    public abstract Task<object?> ResolveAsync(ILogger logger, CancellationToken cancellationToken);
}
