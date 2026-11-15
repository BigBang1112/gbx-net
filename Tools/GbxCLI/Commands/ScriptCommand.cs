namespace GbxCLI.Commands;

internal static class ScriptCommand
{
    public static async Task<int> RunAsync(IEnumerator<string> argsEnumerator, TextWriter output, CancellationToken cancellationToken)
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

        var path = Path.GetFullPath(inputPath);
        var code = await File.ReadAllTextAsync(path, cancellationToken);
        var session = new CSharpSession(new ScriptGlobals(arguments.ToArray(), output), path);
        
        await session.EvaluateAsync(code, cancellationToken);
        
        return 0;
    }
}
