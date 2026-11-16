using GBX.NET;

namespace GbxCLI.Commands;

/// <summary>
/// The <c>compress</c> and <c>decompress</c> commands, which only differ in the operation.
/// </summary>
internal static class CompressionCommand
{
    public static async Task RunAsync(IEnumerator<string> args, CliContext context, bool compress)
    {
        var command = compress ? "compress" : "decompress";
        string? inputPath = null;
        string? outputPath = null;
        var backup = true;

        while (args.MoveNext())
        {
            switch (args.Current)
            {
                case "-h" or "--help":
                    await CliHelp.WriteCommandAsync(context.Console, command);
                    return;
                case "-o" or "--output":
                    outputPath = outputPath is null ? args.NextValue("--output") : throw new ArgumentException("Output can only be specified once.");
                    break;
                case "--no-backup":
                    backup = false;
                    break;
                case var arg when inputPath is null:
                    inputPath = arg;
                    break;
                case var arg:
                    throw ArgumentExtensions.Unexpected(command, arg);
            }
        }

        if (string.IsNullOrWhiteSpace(inputPath))
        {
            throw ArgumentExtensions.MissingInput(command);
        }

        outputPath ??= inputPath;

        var backupPath = await OutputFile.WriteAsync(outputPath, async (destination, cancellationToken) =>
        {
            await using var input = new FileStream(inputPath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);

            if (compress)
            {
                await Gbx.CompressAsync(input, destination, cancellationToken);
            }
            else
            {
                await Gbx.DecompressAsync(input, destination, cancellationToken);
            }
        }, backup, context.CancellationToken);

        await context.Console.WriteLineAsync($"{CliConsole.Written}Written:{CliConsole.Reset} {CliConsole.Escape(outputPath)}");

        if (backupPath is not null)
        {
            await context.Console.WriteLineAsync($"{CliConsole.Written}Backup:{CliConsole.Reset} {CliConsole.Escape(backupPath)}");
        }
    }
}
