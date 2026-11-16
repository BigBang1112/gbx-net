using GBX.NET;
using GBX.NET.LZO;
using GBX.NET.ZLib;
using GbxCLI.Browsing;
using GbxCLI.Commands;
using SimplestGitSourceGenerator;
using System.Reflection;

namespace GbxCLI;

internal static class Cli
{
    public static string Distribution { get; } = typeof(Cli).Assembly
        .GetCustomAttributes<AssemblyMetadataAttribute>()
        .FirstOrDefault(attribute => attribute.Key == "CliDistribution")?.Value ?? "standalone";

    /// <summary>
    /// Runs the CLI without input, as if standard input and output were redirected.
    /// </summary>
    public static Task<int> RunAsync(string[] args, TextWriter output, TextWriter error)
        => RunAsync(args, TextReader.Null, output, error, interactive: false);

    /// <summary>
    /// Runs the CLI. A thrown exception exits with 1, and anything else exits with 0.
    /// </summary>
    /// <param name="interactive">Whether the browser and prompts can run. By default, they run when the input and output are both a console.</param>
    public static async Task<int> RunAsync(string[] args, TextReader input, TextWriter output, TextWriter error, bool? interactive = null)
    {
        using var cancellationTokenSource = new CancellationTokenSource();

        // The first Ctrl+C stops between files and reports through the normal error path. A second one ends the process.
        void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = !cancellationTokenSource.IsCancellationRequested;
            cancellationTokenSource.Cancel();
        }

        Console.CancelKeyPress += OnCancelKeyPress;

        try
        {
            Gbx.LZO = new Lzo();
            Gbx.ZLib = new ZLib();

            var context = new CliContext(input, output, interactive ?? IsConsole(input, output), cancellationTokenSource.Token);
            await RunCoreAsync(args, context);
            return 0;
        }
        catch (Exception exception)
        {
            await CliConsole.Create(error).WriteLineAsync($"$f44gbx:$z {CliConsole.Escape(exception.Message)}");
            return 1;
        }
        finally
        {
            Console.CancelKeyPress -= OnCancelKeyPress;
        }
    }

    private static async Task RunCoreAsync(string[] args, CliContext context)
    {
        using var argsEnumerator = args.AsEnumerable().GetEnumerator();

        if (!argsEnumerator.MoveNext())
        {
            await RunWithoutArgumentsAsync(context);
            return;
        }

        switch (argsEnumerator.Current)
        {
            case "-h" or "--help":
                await CliHelp.WriteAsync(context.Console);
                break;
            case "-v" or "--version":
                await context.Console.WriteLineAsync($"gbx {CliConsole.Escape(SimplestGit.Tag)} $888({CliConsole.Escape(Distribution)})$z");
                break;
            case "inspect" or "parse":
                await InspectCommand.RunAsync(argsEnumerator, context);
                break;
            case "eval":
                await EvalCommand.RunAsync(argsEnumerator, context);
                break;
            case "script":
                await ScriptCommand.RunAsync(argsEnumerator, context);
                break;
            case "verify":
                await VerifyCommand.RunAsync(argsEnumerator, context);
                break;
            case "pack":
                await PackCommand.RunAsync(argsEnumerator, context);
                break;
            case "compress":
                await CompressionCommand.RunAsync(argsEnumerator, context, compress: true);
                break;
            case "decompress":
                await CompressionCommand.RunAsync(argsEnumerator, context, compress: false);
                break;
            case var arg when FileObject.TrySplit(arg, out _, out _):
                // This is what happens when a file is dragged onto gbx.exe.
                await InspectCommand.RunAsync(argsEnumerator, context, inputPath: arg);
                break;
            case var arg:
                throw new ArgumentException($"Unknown command: {arg}. Use --help for usage.");
        }
    }

    /// <summary>
    /// Asks for a path in an interactive console, so a file can be dragged into the window. Otherwise, writes the help.
    /// </summary>
    private static async Task RunWithoutArgumentsAsync(CliContext context)
    {
        if (!context.IsInteractive)
        {
            await CliHelp.WriteAsync(context.Console);
            return;
        }

        while (true)
        {
            await context.Console.WriteLineAsync("$888Drag a Gbx, PAK or ZIP file here or type its path, then press Enter. Press Enter alone for help.$z");
            await context.Console.WriteAsync("$888path>$z ");

            var path = CleanPath(await context.Input.ReadLineAsync(context.CancellationToken));

            if (path is null)
            {
                return;
            }

            if (path.Length == 0)
            {
                await CliHelp.WriteAsync(context.Console);

                // A double-clicked gbx.exe closes its window when the process ends.
                await context.Console.WriteLineAsync("\n$888Press Enter to close.$z");
                await context.Input.ReadLineAsync(context.CancellationToken);
                return;
            }

            if (FileObject.TrySplit(path, out _, out _))
            {
                await Browser.RunAsync(path, context);
                return;
            }

            await context.Console.WriteLineAsync($"$f44File '{CliConsole.Escape(path)}' was not found.$z");
        }
    }

    /// <summary>
    /// Removes the quotes that a console adds around a dragged path with spaces.
    /// </summary>
    private static string? CleanPath(string? line)
    {
        if (line is null)
        {
            return null;
        }

        var path = line.Trim();

        if (path.StartsWith("& "))
        {
            path = path[2..].TrimStart();
        }

        return path.Length >= 2 && (path[0] is '"' or '\'') && path[^1] == path[0] ? path[1..^1] : path;
    }

    private static bool IsConsole(TextReader input, TextWriter output)
        => input == Console.In && output == Console.Out && !Console.IsInputRedirected && !Console.IsOutputRedirected;
}
