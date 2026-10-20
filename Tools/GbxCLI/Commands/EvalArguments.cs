namespace GbxCLI.Commands;

internal sealed record EvalArguments(string InputPath, IReadOnlyList<string> Expressions, bool Interactive)
{
    public static EvalArguments? Parse(IEnumerator<string> argsEnumerator, TextWriter output)
    {
        string? inputPath = null;
        var expressions = new List<string>();
        var interactive = false;

        while (argsEnumerator.MoveNext())
        {
            var arg = argsEnumerator.Current;

            if (Cli.TryHandleGlobalOption(arg, output, "eval"))
            {
                return null;
            }

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

        if (!interactive && expressions.Count == 0)
        {
            throw new ArgumentException("Missing C# expression for eval.");
        }

        return new EvalArguments(inputPath, expressions, interactive);
    }
}
