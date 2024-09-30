
namespace GBX.NET.Tool.CLI.Inputs;

public abstract record InputArgument
{
    public abstract Task<object?> ResolveAsync(CancellationToken cancellationToken);
}
