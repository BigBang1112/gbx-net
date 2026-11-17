namespace GbxCLI.Tests;

public class CliConsoleTests
{
    [Test]
    public async Task WriteLine_WithoutColors_WritesPlainText()
    {
        using var writer = new StringWriter();
        var console = new CliConsole(writer, colors: false);

        await console.WriteLineAsync($"{CliConsole.Label}Name:{CliConsole.Reset} {CliConsole.Escape("a $f00 b $$")}");

        await Assert.That(writer.ToString()).IsEqualTo("Name: a $f00 b $$" + Environment.NewLine);
    }

    [Test]
    public async Task WriteLine_WithColors_WritesAnsiAndEndsInReset()
    {
        using var writer = new StringWriter();
        var console = new CliConsole(writer, colors: true);

        await console.WriteLineAsync($"{CliConsole.Heading}Header{CliConsole.Reset}");

        var line = writer.ToString().TrimEnd();
        await Assert.That(line).Contains("\u001b[");
        await Assert.That(line).Contains("Header");
        await Assert.That(line.EndsWith('m')).IsTrue();
    }

    [Test]
    public async Task WriteLine_WithColorsAndLink_WritesLinkTextOnly()
    {
        using var writer = new StringWriter();
        var console = new CliConsole(writer, colors: true);

        await console.WriteLineAsync("$l[https://example.com]link$l");

        var line = writer.ToString().TrimEnd();
        await Assert.That(line).DoesNotContain("https://example.com");
        await Assert.That(System.Text.RegularExpressions.Regex.Replace(line, @"\u001b\[[0-9;]*m", "")).IsEqualTo("link");
    }

    [Test]
    public async Task Formatted_WithoutColors_KeepsOriginalValue()
    {
        using var writer = new StringWriter();
        var console = new CliConsole(writer, colors: false);

        await console.WriteLineAsync(console.Formatted("$s$f00My Map"));

        await Assert.That(writer.ToString().TrimEnd()).IsEqualTo("$s$f00My Map");
    }

    [Test]
    public async Task Formatted_WithColors_WritesFormattingAsColors()
    {
        using var writer = new StringWriter();
        var console = new CliConsole(writer, colors: true);

        await console.WriteLineAsync(console.Formatted("$f00My Map"));

        await Assert.That(writer.ToString()).Contains("\u001b[");
        await Assert.That(writer.ToString().Contains("$f00")).IsFalse();
    }

    [Test]
    public async Task Create_OtherWriter_HasNoColors()
    {
        using var writer = new StringWriter();

        await Assert.That(CliConsole.Create(writer).Colors).IsFalse();
    }
}
