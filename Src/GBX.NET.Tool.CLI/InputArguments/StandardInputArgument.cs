using GBX.NET.Exceptions;

namespace GBX.NET.Tool.CLI.InputArguments;

public sealed record StandardInputArgument(Stream Stream) : InputArgument
{
    public override async Task<object?> ResolveAsync(CancellationToken cancellationToken)
    {
        var position = Stream.Position;

        try
        {
            if (Gbx.IsCompressed(Stream))
            {
                Stream.Position = position;
                return await Gbx.ParseAsync(Stream, cancellationToken: cancellationToken);
            }
        }
        catch (NotAGbxException)
        {

        }

        Stream.Position = position;
        return Task.FromResult((object?)Stream);
    }
}
