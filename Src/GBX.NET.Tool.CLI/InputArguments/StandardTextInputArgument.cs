
namespace GBX.NET.Tool.CLI.InputArguments;

public sealed record StandardTextInputArgument(TextReader Reader) : InputArgument
{
    public override Task<object?> ResolveAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult((object?)Reader);
    }
}
