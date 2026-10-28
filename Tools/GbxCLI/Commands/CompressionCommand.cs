using GBX.NET;
using Spectre.Console;

namespace GbxCLI.Commands;

internal static class CompressionCommand
{
    public static async Task<int> RunAsync(string command, IEnumerator<string> argsEnumerator, TextWriter output)
    {
        var arguments = FileArguments.Parse(argsEnumerator, command, output, requiresOutput: true);

        if (arguments is null)
        {
            return 0;
        }

        await using var input = new FileStream(arguments.InputPath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        await using var destination = new FileStream(arguments.OutputPath!, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);

        if (command == "compress")
        {
            await Gbx.CompressAsync(input, destination);
        }
        else
        {
            await Gbx.DecompressAsync(input, destination);
        }

        await new CliConsole(output).WriteLineAsync($"[green]Written:[/] {Markup.Escape(arguments.OutputPath!)}");
        return 0;
    }
}
