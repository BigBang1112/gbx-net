using Spectre.Console;

namespace GbxCLI;

internal sealed class CliConsole
{
    private readonly IAnsiConsole console;

    public CliConsole(TextWriter writer)
    {
        var output = new AnsiConsoleOutput(writer);

        console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = output,
            Ansi = output.IsTerminal ? AnsiSupport.Detect : AnsiSupport.No,
            Interactive = InteractionSupport.No,
            // CI enrichers can re-enable ANSI for redirected writers.
            Enrichment = new ProfileEnrichment { UseDefaultEnrichers = output.IsTerminal }
        });
    }

    public CliConsole(IAnsiConsole console)
    {
        this.console = console;
    }

    public void WriteLine(string markup)
    {
        if (console.Profile.Capabilities.Ansi)
        {
            console.MarkupLine(markup);
        }
        else
        {
            console.Profile.Out.Writer.WriteLine(Markup.Remove(markup));
        }
    }

    public Task WriteLineAsync(string markup)
    {
        if (!console.Profile.Capabilities.Ansi)
        {
            return console.Profile.Out.Writer.WriteLineAsync(Markup.Remove(markup));
        }

        console.MarkupLine(markup);
        return Task.CompletedTask;
    }

    public Task WriteResultAsync(object? value) => new ConsoleValueWriter(this).WriteResultAsync(value);
}
