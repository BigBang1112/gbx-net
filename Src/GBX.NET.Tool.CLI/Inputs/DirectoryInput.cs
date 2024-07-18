﻿
using GBX.NET.Exceptions;

namespace GBX.NET.Tool.CLI.Inputs;

internal sealed record DirectoryInput(string DirectoryPath) : Input
{
    public override async Task<object?> ResolveAsync(CancellationToken cancellationToken)
    {
        var files = Directory.GetFiles(DirectoryPath, "*.*", SearchOption.AllDirectories);
        
        var tasks = files.Select<string, Task<object?>>(async file =>
        {
            try
            {
                await using var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
                return await Gbx.ParseAsync(stream, cancellationToken: cancellationToken);
            }
            catch (NotAGbxException)
            {
                return await File.ReadAllBytesAsync(file, cancellationToken);
            }
        });

        return await Task.WhenAll(tasks);
    }
}
