using Spectre.Console;

namespace GbxCLI;

internal sealed class CliConsole
{
    private const int resultEntryLimit = 50;

    private readonly IAnsiConsole console;

    public CliConsole(TextWriter writer)
    {
        var output = new AnsiConsoleOutput(writer);

        console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = output,
            Ansi = output.IsTerminal ? AnsiSupport.Detect : AnsiSupport.No,
            Interactive = InteractionSupport.No
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

    public async Task WriteResultAsync(object? value)
    {
        await WriteLineAsync(Markup.Escape(value?.ToString() ?? "(null)"));

        if (value is null)
        {
            return;
        }

        var entries = FileInspector.GetEntries(value).Take(resultEntryLimit + 1).ToArray();

        foreach (var entry in entries.Take(resultEntryLimit))
        {
            var text = entry.Error is null ? entry.Value?.ToString() ?? "(null)" : $"(error: {entry.Error})";
            await WriteLineAsync(Markup.Escape($"  {entry.Name}: {text}"));
        }

        if (entries.Length > resultEntryLimit)
        {
            await WriteLineAsync("[grey]  More members available through a narrower query.[/]");
        }
    }
}
