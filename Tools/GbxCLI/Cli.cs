using GBX.NET;
using GBX.NET.LZO;
using GBX.NET.ZLib;
using GbxCLI.Browsing;
using GbxCLI.Commands;
using SimplestGitSourceGenerator;
using System.Globalization;
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
        // Output doesn't depend on the system language, so scripts can parse it. The project also enables InvariantGlobalization,
        // and this covers callers in another process configuration, such as tests. An async method restores the caller's culture.
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

        using var cancellationTokenSource = new CancellationTokenSource();
        CliContext? context = null;

        // Ctrl+C first cancels a running browser operation and keeps the session. Otherwise, the first one stops between
        // files and reports through the normal error path, and a second one ends the process.
        void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            if (context?.TryCancelOperation() == true)
            {
                e.Cancel = true;
                return;
            }

            e.Cancel = !cancellationTokenSource.IsCancellationRequested;
            cancellationTokenSource.Cancel();
        }

        Console.CancelKeyPress += OnCancelKeyPress;

        try
        {
            Gbx.LZO = new Lzo();
            Gbx.ZLib = new ZLib();

            context = new CliContext(input, output, interactive ?? IsConsole(input, output), cancellationTokenSource.Token);
            await RunCoreAsync(args, context);
            return 0;
        }
        catch (Exception exception)
        {
            await CliConsole.Create(error).WriteLineAsync($"$f44gbx:$z {CliConsole.Escape(exception.Message)}");

            if (context is { PauseOnError: true })
            {
                await WaitForEnterToCloseAsync(context);
            }

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
            context.PauseOnError = context.IsInteractive && ConsoleMode.OwnsConsoleWindow();
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
            case "set":
                await SetCommand.RunAsync(argsEnumerator, context);
                break;
            case "diff":
                await DiffCommand.RunAsync(argsEnumerator, context);
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
                // This is what happens when a file is dragged onto gbx.exe. Its window would close before an error could be read.
                context.PauseOnError = context.IsInteractive && ConsoleMode.OwnsConsoleWindow();
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
            await context.Console.WriteLineAsync("$888Drag a Gbx, PAK or ZIP file here or type its path, then press Enter.$z");
            await context.Console.WriteLineAsync("$888Press Enter alone for help.$z");
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
                await WaitForEnterToCloseAsync(context);
                return;
            }

            if (!FileObject.TrySplit(path, out _, out _))
            {
                await context.Console.WriteLineAsync($"$f44File '{CliConsole.Escape(path)}' was not found.$z");
                continue;
            }

            // A file that cannot be opened asks for another path, instead of ending the session.
            FileObject? file = null;

            try
            {
                if (!await context.TryRunCancellableAsync(async cancellationToken => file = await FileObject.OpenAsync(path, cancellationToken)))
                {
                    await context.Console.WriteLineAsync($"{CliConsole.Label}Opening was canceled.{CliConsole.Reset}");
                    continue;
                }
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                await context.Console.WriteLineAsync($"{CliConsole.Error}error:{CliConsole.Reset} {CliConsole.Escape(exception.Message)}");
                continue;
            }

            await using (file)
            {
                await Browser.RunAsync(file!, context);
            }

            return;
        }
    }

    /// <summary>
    /// Removes the quotes that a console adds around a dragged path with spaces.
    /// </summary>
    internal static string? CleanPath(string? line)
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

    /// <summary>
    /// Keeps the window open until Enter is pressed. In a console, other keys are ignored and not shown, so nothing can be typed after the prompt.
    /// </summary>
    private static async Task WaitForEnterToCloseAsync(CliContext context)
    {
        await context.Console.WriteAsync("\n$888Press Enter to close this window.$z");

        if (context.Input != Console.In || Console.IsInputRedirected)
        {
            try
            {
                await context.Input.ReadLineAsync(context.CancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Ctrl+C closes the window, too.
            }

            return;
        }

        // Polling lets Ctrl+C close the window too, which a blocking ReadKey would ignore until another key.
        while (!context.CancellationToken.IsCancellationRequested)
        {
            if (!Console.KeyAvailable)
            {
                await Task.Delay(50, CancellationToken.None);
                continue;
            }

            if (Console.ReadKey(intercept: true).Key == ConsoleKey.Enter)
            {
                break;
            }
        }

        await context.Console.WriteLineAsync(string.Empty);
    }

    private static bool IsConsole(TextReader input, TextWriter output)
        => input == Console.In && output == Console.Out && !Console.IsInputRedirected && !Console.IsOutputRedirected;
}
