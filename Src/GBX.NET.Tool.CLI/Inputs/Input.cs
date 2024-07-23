
namespace GBX.NET.Tool.CLI.Inputs;

internal abstract record Input
{
    public abstract Task<object?> ResolveAsync(CancellationToken cancellationToken);
}
