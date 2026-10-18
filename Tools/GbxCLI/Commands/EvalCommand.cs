namespace GbxCLI.Commands;

internal static class EvalCommand
{
    public static async Task<int> RunAsync(IEnumerator<string> argsEnumerator, TextWriter output)
    {
        var arguments = EvalArguments.Parse(argsEnumerator, output);

        if (arguments is null)
        {
            return 0;
        }

        if (arguments.Interactive)
        {
            return await Cli.RunInteractiveAsync(output);
        }

        var console = new CliConsole(output);
        await using var file = await ParsedFile.OpenAsync(arguments.InputPath, fullGbx: true);
        var session = new CSharpSession(file.Value);

        foreach (var expression in arguments.Expressions)
        {
            var result = await session.EvaluateAsync(expression);
            await console.WriteResultAsync(result);
        }

        return 0;
    }
}
