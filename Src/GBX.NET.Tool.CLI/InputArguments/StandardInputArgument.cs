namespace GBX.NET.Tool.CLI.InputArguments;

public sealed record StandardInputArgument(Stream Stream) : InputArgument
{
    public override Task<object?> ResolveAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult((object?)Stream);
    }
}
