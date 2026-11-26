namespace GbxCLI.Commands;

internal static class ScriptCommand
{
    public static async Task RunAsync(IEnumerator<string> args, CliContext context)
    {
        string? scriptPath = null;
        var arguments = new List<string>();

        while (args.MoveNext())
        {
            switch (args.Current)
            {
                // Arguments after the script belong to it, also when they start with -.
                case var arg when scriptPath is not null:
                    arguments.Add(arg);
                    break;
                case "-h" or "--help":
                    await CliHelp.WriteCommandAsync(context.Console, "script");
                    return;
                case "--":
                    scriptPath = args.NextValue("--");
                    break;
                case var arg when arg.StartsWith('-'):
                    throw ArgumentExtensions.Unexpected("script", arg);
                case var arg:
                    scriptPath = arg;
                    break;
            }
        }

        if (string.IsNullOrWhiteSpace(scriptPath))
        {
            throw new ArgumentException("Missing C# script file for script.");
        }

        var path = Path.GetFullPath(scriptPath);
        var code = await File.ReadAllTextAsync(path, context.CancellationToken);
        var session = new CSharpSession(new ScriptGlobals([.. arguments], context.Output, context.CancellationToken), path);

        await session.EvaluateAsync(code, context.CancellationToken);
    }
}
