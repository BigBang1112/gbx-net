namespace GbxCLI.Commands;

internal static class EvalCommand
{
    public static async Task RunAsync(IEnumerator<string> args, CliContext context)
    {
        string? inputPath = null;
        var expressions = new List<string>();
        var interactive = false;

        while (args.MoveNext())
        {
            switch (args.Current)
            {
                case "-h" or "--help" when inputPath is null:
                    await CliHelp.WriteCommandAsync(context.Console, "eval");
                    return;
                case "-i" or "--interactive":
                    interactive = true;
                    break;
                case var arg when inputPath is null:
                    inputPath = arg;
                    break;
                case var arg when string.IsNullOrWhiteSpace(arg):
                    throw new ArgumentException("C# expressions for eval cannot be empty.");
                case var arg:
                    expressions.Add(arg);
                    break;
            }
        }

        if (string.IsNullOrWhiteSpace(inputPath))
        {
            throw ArgumentExtensions.MissingInput("eval");
        }

        if (!interactive && expressions.Count == 0)
        {
            throw new ArgumentException("Missing C# expression for eval.");
        }

        await using var file = await FileObject.OpenAsync(inputPath, context.CancellationToken);
        var session = new CSharpSession(file.Value);

        foreach (var expression in expressions)
        {
            var result = await session.EvaluateAsync(expression, context.CancellationToken);
            await new ConsoleValueWriter(context.Console).WriteResultAsync(result);
        }

        if (interactive)
        {
            await CSharpPrompt.RunAsync(session, file, context);
        }
    }
}
