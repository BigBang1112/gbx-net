namespace GbxCLI.Commands;

internal static class EvalCommand
{
    public static async Task<int> RunAsync(IEnumerator<string> argsEnumerator, TextWriter output, CancellationToken cancellationToken)
    {
        string? inputPath = null;
        var expressions = new List<string>();
        var interactive = false;

        while (argsEnumerator.MoveNext())
        {
            var arg = argsEnumerator.Current;

            if (arg is "-i" or "--interactive")
            {
                interactive = true;
                continue;
            }

            if (inputPath is null)
            {
                inputPath = arg;
            }
            else if (string.IsNullOrWhiteSpace(arg))
            {
                throw new ArgumentException("C# expressions for eval cannot be empty.");
            }
            else
            {
                expressions.Add(arg);
            }
        }

        if (string.IsNullOrWhiteSpace(inputPath))
        {
            throw new ArgumentException("Missing input file for eval.");
        }

        if (interactive)
        {
            return await Cli.RunInteractiveAsync(output);
        }

        if (expressions.Count == 0)
        {
            throw new ArgumentException("Missing C# expression for eval.");
        }

        var console = new CliConsole(output);
        await using var file = await FileObject.OpenAsync(inputPath, fullGbx: true, cancellationToken);
        var session = new CSharpSession(file.Value);

        foreach (var expression in expressions)
        {
            var result = await session.EvaluateAsync(expression, cancellationToken);
            await console.WriteResultAsync(result);
        }

        return 0;
    }
}
