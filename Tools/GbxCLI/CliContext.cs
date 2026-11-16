namespace GbxCLI;

/// <summary>
/// The input, output and state that every command receives.
/// </summary>
/// <param name="IsInteractive">Whether the browser and prompts can run, which needs a console for both input and output.</param>
internal sealed record CliContext(TextReader Input, TextWriter Output, bool IsInteractive, CancellationToken CancellationToken)
{
    public CliConsole Console { get; } = CliConsole.Create(Output);
}
