using GbxCLI.Browsing;

namespace GbxCLI.Commands;

internal static class InspectCommand
{
    /// <param name="inputPath">The path given without a command, which opens the browser.</param>
    public static async Task RunAsync(IEnumerator<string> args, CliContext context, string? inputPath = null)
    {
        var interactive = inputPath is not null;

        while (args.MoveNext())
        {
            switch (args.Current)
            {
                case "-h" or "--help":
                    await CliHelp.WriteCommandAsync(context.Console, "inspect");
                    return;
                case "-i" or "--interactive":
                    interactive = true;
                    break;
                case var arg when inputPath is null:
                    inputPath = arg;
                    break;
                case var arg:
                    throw ArgumentExtensions.Unexpected("inspect", arg);
            }
        }

        if (string.IsNullOrWhiteSpace(inputPath))
        {
            throw ArgumentExtensions.MissingInput("inspect");
        }

        // With redirected input or output, the summary keeps scripts and pipes working.
        if (interactive && context.IsInteractive)
        {
            await Browser.RunAsync(inputPath, context);
            return;
        }

        await using var file = await FileObject.OpenAsync(inputPath, context.CancellationToken);
        await file.WriteSummaryAsync(context.Console);
    }
}
