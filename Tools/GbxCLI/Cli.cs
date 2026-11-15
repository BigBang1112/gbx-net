using GBX.NET;
using GBX.NET.LZO;
using GBX.NET.ZLib;
using GbxCLI.Commands;
using Spectre.Console;
using System.Reflection;
using SimplestGitSourceGenerator;

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
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        using var argsEnumerator = args.AsEnumerable().GetEnumerator();

        if (!argsEnumerator.MoveNext())
        {
            CliHelp.Write(output, distribution);
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
            "eval" => await EvalCommand.RunAsync(argsEnumerator, output, cancellationToken),
            "inspect" or "parse" => await InspectCommand.RunAsync(argsEnumerator, output, cancellationToken),
            "script" => await ScriptCommand.RunAsync(argsEnumerator, output, cancellationToken),
            "pack" => await PackCommand.RunAsync(argsEnumerator, output, cancellationToken),
            "verify" => await VerifyCommand.RunAsync(argsEnumerator, output, cancellationToken),
            "compress" => await CompressCommand.RunAsync(argsEnumerator, output, cancellationToken),
            "decompress" => await DecompressCommand.RunAsync(argsEnumerator, output, cancellationToken),
            _ when File.Exists(arg) => await InspectCommand.RunAsync(argsEnumerator, output, cancellationToken, inputPath: arg),
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
                CliHelp.Write(output, distribution);
                return true;
            case "-v":
            case "--version":
                new CliConsole(output).WriteLine($"[bold]gbx[/] {SimplestGit.Tag} [grey]({Markup.Escape(distribution)})[/]");
                return true;
            default:
                return false;
        }
    }
}
