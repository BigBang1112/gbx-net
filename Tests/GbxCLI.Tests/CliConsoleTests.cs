using Spectre.Console;

using GBX.NET;
using GBX.NET.Components;
using GBX.NET.Engines.MwFoundations;

namespace GbxCLI.Tests;

public class CliConsoleTests
{
    [Test]
    public async Task WriteResult_ShowsStoredReferencesWithoutLoadingExternalNodes()
    {
        var table = new GbxRefTable();
        var calls = 0;
        table.ExternalNodes["external [red].Gbx"] = () =>
        {
            calls++;
            throw new InvalidOperationException("Must not load");
        };
        var references = new NodeRefList<CMwNod>();
        references.Add(new NodeRef<CMwNod>
        {
            Descriptor = table.CreateFile("external [red].Gbx")
        });
        using var output = new StringWriter();

        await new CliConsole(output).WriteResultAsync(references);

        await Assert.That(calls).IsEqualTo(0);
        await Assert.That(output.ToString()).Contains("Result: 1 items");
        await Assert.That(output.ToString()).Contains("  [0]: ExternalFile, Unresolved: external [red].Gbx");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task PlainOutput_PreservesLongLiteralValues(bool asynchronous)
    {
        using var output = new StringWriter();
        var console = new CliConsole(output);
        var path = $"{new string('x', 200)} [red] map.Gbx";
        var markup = $"[grey]File:[/] {Markup.Escape(path)}";

        if (asynchronous)
        {
            await console.WriteLineAsync(markup);
        }
        else
        {
            console.WriteLine(markup);
        }

        await Assert.That(output.ToString()).IsEqualTo($"File: {path}{Environment.NewLine}");
    }

    [Test]
    [NotInParallel]
    [Arguments(false)]
    [Arguments(true)]
    public async Task PlainOutput_GitHubActions_PreservesLongLiteralValues(bool asynchronous)
    {
        var githubActions = Environment.GetEnvironmentVariable("GITHUB_ACTIONS");

        try
        {
            Environment.SetEnvironmentVariable("GITHUB_ACTIONS", "true");
            using var output = new StringWriter();
            var console = new CliConsole(output);
            var path = $"{new string('x', 200)} [red] map.Gbx";
            var markup = $"[grey]File:[/] {Markup.Escape(path)}";

            if (asynchronous)
            {
                await console.WriteLineAsync(markup);
            }
            else
            {
                console.WriteLine(markup);
            }

            await Assert.That(output.ToString()).IsEqualTo($"File: {path}{Environment.NewLine}");
        }
        finally
        {
            Environment.SetEnvironmentVariable("GITHUB_ACTIONS", githubActions);
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task AnsiOutput_StylesLabelsWithoutInterpretingLiteralValues(bool asynchronous)
    {
        using var output = new StringWriter();
        var ansiConsole = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = new AnsiConsoleOutput(output),
            Ansi = AnsiSupport.Yes,
            ColorSystem = ColorSystemSupport.Standard,
            Interactive = InteractionSupport.No
        });
        ansiConsole.Profile.Width = 200;
        var console = new CliConsole(ansiConsole);
        var path = "literal [green] map.Gbx";
        var markup = $"[cyan]File:[/] {Markup.Escape(path)}";

        if (asynchronous)
        {
            await console.WriteLineAsync(markup);
        }
        else
        {
            console.WriteLine(markup);
        }

        await Assert.That(output.ToString()).Contains("\u001b[");
        await Assert.That(output.ToString()).Contains("File:");
        await Assert.That(output.ToString()).Contains(path);
    }

    [Test]
    public async Task WriteResult_PreservesNullAndLiteralMarkupAndBoundsCollections()
    {
        using var output = new StringWriter();
        var console = new CliConsole(output);

        await console.WriteResultAsync(null);
        await console.WriteResultAsync("literal [red] text");
        await console.WriteResultAsync(Enumerable.Range(0, int.MaxValue));

        await Assert.That(output.ToString()).Contains("(null)");
        await Assert.That(output.ToString()).Contains("literal [red] text");
        await Assert.That(output.ToString()).Contains("Result: (null)");
        await Assert.That(output.ToString()).Contains("[9]: 9");
        await Assert.That(output.ToString().Contains("[10]: 10")).IsFalse();
        await Assert.That(output.ToString()).Contains("More items (showing first 10)");
        await Assert.That(output.ToString().Contains('\u001b')).IsFalse();
    }

    [Test]
    public async Task ErrorOutput_PreservesLiteralMarkup()
    {
        using var output = new StringWriter();
        using var error = new StringWriter();
        var argument = "[red]unknown[/]";

        var exitCode = await Cli.RunAsync([argument], output, error);

        await Assert.That(exitCode).IsEqualTo(1);
        await Assert.That(output.ToString()).IsEqualTo(string.Empty);
        await Assert.That(error.ToString()).Contains($"gbx: Unknown command: {argument}.");
        await Assert.That(error.ToString().Contains('\u001b')).IsFalse();
    }
}
