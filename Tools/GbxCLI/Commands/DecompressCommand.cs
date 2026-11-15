using GBX.NET;
using Spectre.Console;

namespace GbxCLI.Commands;

internal static class DecompressCommand
{
    public static async Task<int> RunAsync(IEnumerator<string> argsEnumerator, TextWriter output, CancellationToken cancellationToken)
    {
        string? inputPath = null;
        string? outputPath = null;
        var backup = true;

        while (argsEnumerator.MoveNext())
        {
            var arg = argsEnumerator.Current;

            if (Cli.TryHandleGlobalOption(arg, output))
            {
                return 0;
            }

            if (arg is "-o" or "--output")
            {
                if (outputPath is not null)
                {
                    throw new ArgumentException("Output can only be specified once.");
                }

                if (!argsEnumerator.MoveNext() || string.IsNullOrWhiteSpace(argsEnumerator.Current))
                {
                    throw new ArgumentException($"Missing file path after {arg}.");
                }

                outputPath = argsEnumerator.Current;
            }
            else if (arg == "--no-backup")
            {
                backup = false;
            }
            else if (inputPath is null)
            {
                inputPath = arg;
            }
            else
            {
                throw new ArgumentException($"Unexpected argument: {arg}.");
            }
        }

        if (string.IsNullOrWhiteSpace(inputPath))
        {
            throw new ArgumentException("Missing input file for decompress.");
        }

        outputPath ??= inputPath;
        
        var backupPath = await OutputFile.WriteAsync(outputPath, async (destination, token) =>
        {
            await using var input = new FileStream(inputPath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
            await Gbx.DecompressAsync(input, destination, token);
        }, backup, cancellationToken);

        var console = new CliConsole(output);

        if (backupPath is not null)
        {
            await console.WriteLineAsync($"[green]Backup:[/] {Markup.Escape(backupPath)}");
        }

        await console.WriteLineAsync($"[green]Written:[/] {Markup.Escape(outputPath)}");
        return 0;
    }
}
