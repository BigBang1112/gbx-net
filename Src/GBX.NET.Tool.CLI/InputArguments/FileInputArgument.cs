﻿
using GBX.NET.Exceptions;

namespace GBX.NET.Tool.CLI.InputArguments;

public sealed record FileInputArgument(string FilePath) : InputArgument
{
    public override async Task<object?> ResolveAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
            return await Gbx.ParseAsync(stream, cancellationToken: cancellationToken);
        }
        catch (NotAGbxException)
        {
            return File.ReadAllBytesAsync(FilePath, cancellationToken);
        }
    }
}
