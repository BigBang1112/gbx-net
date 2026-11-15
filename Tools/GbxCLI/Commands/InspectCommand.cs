namespace GbxCLI.Commands;

internal static class InspectCommand
{
    /// <param name="inputPath">The file given without a command, which opens it interactively.</param>
    public static async Task<int> RunAsync(IEnumerator<string> argsEnumerator, TextWriter output, CancellationToken cancellationToken, string? inputPath = null)
    {
        var interactive = inputPath is not null;

        while (argsEnumerator.MoveNext())
        {
            var arg = argsEnumerator.Current;

            if (arg is "-i" or "--interactive")
            {
                interactive = true;
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
            throw new ArgumentException("Missing input file for inspect.");
        }

        if (interactive)
        {
            return await Cli.RunInteractiveAsync(output);
        }

        await using var file = await FileObject.OpenAsync(inputPath, fullGbx: true, cancellationToken);
        await file.WriteSummaryAsync(new CliConsole(output));

        return 0;
    }
}
