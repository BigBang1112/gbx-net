namespace GbxCLI.Tests;

public class CliHelpTests
{
    [Test]
    [Arguments("-h")]
    [Arguments("--help")]
    public async Task Overview_ListsCommandsAndGlobalOptions(string option)
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        var exitCode = await Cli.RunAsync([option], output, error);
        var help = output.ToString();

        await Assert.That(exitCode).IsEqualTo(0);
        await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        await Assert.That(help).Contains("Commands:");
        await Assert.That(help).Contains("script <file>");
        await Assert.That(help).Contains("Global options:");
        await Assert.That(help).Contains("pack list <file>");
        await Assert.That(help).DoesNotContain("--resolved");
        await Assert.That(help).DoesNotContain("--output");
        await Assert.That(help).DoesNotContain("--interactive");
    }
}
