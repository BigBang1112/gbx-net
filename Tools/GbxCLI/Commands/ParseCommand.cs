namespace GbxCLI.Commands;

internal static class ParseCommand
{
    public static async Task<int> RunAsync(IEnumerator<string> argsEnumerator, TextWriter output, string? inputPath = null)
    {
        var arguments = FileArguments.Parse(argsEnumerator, "parse", output, requiresOutput: false, inputPath);

        if (arguments is null)
        {
            return 0;
        }

        if (arguments.Interactive)
        {
            return await Cli.RunInteractiveAsync(output);
        }

        await using var file = await ParsedFile.OpenAsync(arguments.InputPath, fullGbx: true);
        await file.WriteSummaryAsync(new CliConsole(output));

        return 0;
    }
}
