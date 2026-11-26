namespace GbxCLI.Tests;

internal sealed record CliRun(int ExitCode, string Output, string Error)
{
    /// <summary>
    /// Runs the CLI with redirected output and no input.
    /// </summary>
    public static async Task<CliRun> RunAsync(params string[] args)
    {
        using var output = new StringWriter();
        using var error = new StringWriter();
        var exitCode = await Cli.RunAsync(args, output, error);
        return new(exitCode, output.ToString(), error.ToString());
    }

    /// <summary>
    /// Runs the CLI as if it were in a console, with scripted input such as <c>"3\n..\nq\n"</c>.
    /// </summary>
    public static async Task<CliRun> RunInteractiveAsync(string input, params string[] args)
    {
        using var reader = new StringReader(input);
        using var output = new StringWriter();
        using var error = new StringWriter();
        var exitCode = await Cli.RunAsync(args, reader, output, error, interactive: true);
        return new(exitCode, output.ToString(), error.ToString());
    }
}
