namespace GbxCLI.Commands;

internal sealed record ScriptArguments(string InputPath, string[] Args)
{
    public static ScriptArguments? Parse(IEnumerator<string> argsEnumerator, TextWriter output)
    {
        string? inputPath = null;
        var arguments = new List<string>();
        var passThrough = false;

        while (argsEnumerator.MoveNext())
        {
            var arg = argsEnumerator.Current;

            if (!passThrough && arg == "--")
            {
                passThrough = true;
                continue;
            }

            if (!passThrough && Cli.TryHandleGlobalOption(arg, output, "script"))
            {
                return null;
            }

            if (inputPath is null)
            {
                if (!passThrough && arg.StartsWith('-'))
                {
                    throw new ArgumentException($"Unknown option for script: {arg}. Use gbx script --help for usage.");
                }

                inputPath = arg;
            }
            else
            {
                arguments.Add(arg);
            }
        }

        if (string.IsNullOrWhiteSpace(inputPath))
        {
            throw new ArgumentException("Missing C# script file for script.");
        }

        return new ScriptArguments(inputPath, [.. arguments]);
    }
}
