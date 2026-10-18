using GBX.NET;
using GBX.NET.LZO;
using GBX.NET.ZLib;
using GbxCLI.Commands;
using Spectre.Console;
using System.Reflection;

namespace GbxCLI;

internal static class Cli
{
    private static readonly string distribution = typeof(Cli).Assembly
        .GetCustomAttributes<AssemblyMetadataAttribute>()
        .FirstOrDefault(attribute => attribute.Key == "CliDistribution")?.Value ?? "standalone";

    public static async Task<int> RunAsync(string[] args, TextWriter output, TextWriter error)
    {
        try
        {
            Gbx.LZO = new Lzo();
            Gbx.ZLib = new ZLib();

            return await RunCoreAsync(args, output);
        }
        catch (Exception exception)
        {
            await new CliConsole(error).WriteLineAsync($"[red]gbx:[/] {Markup.Escape(exception.Message)}");
            return 1;
        }
    }

    private static async Task<int> RunCoreAsync(string[] args, TextWriter output)
    {
        using var argsEnumerator = args.AsEnumerable().GetEnumerator();

        if (!argsEnumerator.MoveNext())
        {
            WriteHelp(output);
            await WaitForExitAsync(output);
            return 0;
        }

        var arg = argsEnumerator.Current;

        if (TryHandleGlobalOption(arg, output))
        {
            return 0;
        }

        return arg switch
        {
            "parse" => await ParseCommand.RunAsync(argsEnumerator, output),
            "eval" => await EvalCommand.RunAsync(argsEnumerator, output),
            "compress" or "decompress" => await CompressionCommand.RunAsync(arg, argsEnumerator, output),
            _ when File.Exists(arg) => await ParseCommand.RunAsync(argsEnumerator, output, inputPath: arg),
            _ => throw new ArgumentException($"Unknown command: {arg}. Use --help for usage.")
        };
    }

    private static async Task WaitForExitAsync(TextWriter output)
    {
        if (output != Console.Out || !Environment.UserInteractive || Console.IsInputRedirected || Console.IsOutputRedirected)
        {
            return;
        }

        try
        {
            _ = Console.KeyAvailable;
            await new CliConsole(output).WriteLineAsync("\n[grey]Press any key to continue...[/]");
            await Task.Run(() => Console.ReadKey(intercept: true));
        }
        catch (Exception exception) when (exception is InvalidOperationException or IOException or PlatformNotSupportedException)
        {
        }
    }

    internal static async Task<int> RunInteractiveAsync(TextWriter output)
    {
        await new CliConsole(output).WriteLineAsync("[grey]Interactive mode is not implemented yet. Use gbx --help for noninteractive commands.[/]");
        return 0;
    }

    public static bool TryHandleGlobalOption(string arg, TextWriter output)
    {
        switch (arg)
        {
            case "-h":
            case "--help":
                WriteHelp(output);
                return true;
            case "-v":
            case "--version":
                new CliConsole(output).WriteLine($"[bold]gbx[/] {typeof(Cli).Assembly.GetName().Version?.ToString(3)} [grey]({Markup.Escape(distribution)})[/]");
                return true;
            default:
                return false;
        }
    }

    private static void WriteHelp(TextWriter output)
    {
        var console = new CliConsole(output);
        console.WriteLine($"[bold]GBX.NET.CLI[/] [grey]({Markup.Escape(distribution)})[/]");
        console.WriteLine(string.Empty);
        console.WriteLine("""
            [bold]Usage:[/] [cyan]gbx[/] [[command]] <file> [[options]]

            [bold]Commands:[/]
              [cyan]parse[/] <file>                     Detect and inspect a Gbx or PAK file (default).
              [cyan]eval[/] <file> [[code...]]            Evaluate C# against a Gbx or PAK file.
              [cyan]decompress[/] <file> -o <output>     Decompress a Gbx body.
              [cyan]compress[/] <file> -o <output>       Compress a Gbx body.

            [bold]Options:[/]
              [cyan]-i, --interactive[/]      Interactive mode (not implemented yet).
              [cyan]-o, --output[/] <file>    Write a new file without overwriting.
              [cyan]-h, --help[/]             Show this help.
              [cyan]-v, --version[/]          Show the CLI version.

            [bold]Examples:[/]
              gbx parse "My Map.Map.Gbx"
              gbx parse "My Map.Map.Gbx" --interactive
              gbx eval "My Map.Map.Gbx" "Node.Chunks.Count"
            """);
    }
}
