namespace GbxCLI.Commands;

internal static class ScriptCommand
{
    public static async Task<int> RunAsync(IEnumerator<string> argsEnumerator, TextWriter output)
    {
        var arguments = ScriptArguments.Parse(argsEnumerator, output);

        if (arguments is null)
        {
            return 0;
        }

        var path = Path.GetFullPath(arguments.InputPath);
        var code = await File.ReadAllTextAsync(path);
        var session = new CSharpSession(new ScriptGlobals(arguments.Args, output), path);
        await session.EvaluateAsync(code);
        return 0;
    }
}
