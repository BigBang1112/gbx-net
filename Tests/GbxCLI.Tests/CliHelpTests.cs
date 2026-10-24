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
        await Assert.That(help).Contains("gbx <command> --help");
        await Assert.That(help).DoesNotContain("--resolved");
        await Assert.That(help).DoesNotContain("--output");
        await Assert.That(help).DoesNotContain("--interactive");
    }

    [Test]
    [Arguments("parse", "--interactive", "--output|--resolved|--unresolved")]
    [Arguments("eval", "--interactive", "--output|--resolved|--unresolved")]
    [Arguments("script", "-- args", "--output|--resolved|--unresolved|--interactive")]
    [Arguments("verify", "", "--resolved|--unresolved|--output|--interactive")]
    [Arguments("pak files", "--resolved|--unresolved", "--output|--interactive")]
    [Arguments("pak verify", "", "--resolved|--unresolved|--output|--interactive")]
    [Arguments("pak extract", "--output", "--resolved|--unresolved|--interactive")]
    [Arguments("compress", "--output", "--resolved|--unresolved|--interactive")]
    [Arguments("decompress", "--output", "--resolved|--unresolved|--interactive")]
    public async Task CommandHelp_ListsOnlyApplicableOptions(string command, string expected, string unrelated)
    {
        foreach (var option in new[] { "-h", "--help" })
        {
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync([.. command.Split(' '), option], output, error);
            var help = output.ToString();

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            await Assert.That(help).Contains($"Usage: gbx {command} <file>");
            await Assert.That(help).Contains("Global options:");
            await Assert.That(help).Contains("--help");
            await Assert.That(help).Contains("--version");

            foreach (var item in expected.Split('|', StringSplitOptions.RemoveEmptyEntries))
            {
                await Assert.That(help).Contains(item);
            }

            foreach (var item in unrelated.Split('|'))
            {
                await Assert.That(help).DoesNotContain(item);
            }
        }
    }

    [Test]
    public async Task PakHelp_ListsPakSubcommandsAndPointsToTheirHelp()
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        var exitCode = await Cli.RunAsync(["pak", "--help"], output, error);
        var help = output.ToString();

        await Assert.That(exitCode).IsEqualTo(0);
        await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        await Assert.That(help).Contains("Usage: gbx pak <command>");
        await Assert.That(help).Contains("pak files <file>");
        await Assert.That(help).Contains("pak verify <file>");
        await Assert.That(help).Contains("pak extract <file>");
        await Assert.That(help).Contains("gbx pak <command> --help");
        await Assert.That(help).DoesNotContain("parse <file>");
        await Assert.That(help).DoesNotContain("--resolved");
    }

    [Test]
    public async Task CommandHelp_AfterInputAndOptions_DoesNotOpenFiles()
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        var exitCode = await Cli.RunAsync(["pak", "files", "missing.pak", "--unresolved", "--help"], output, error);

        await Assert.That(exitCode).IsEqualTo(0);
        await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        await Assert.That(output.ToString()).Contains("Usage: gbx pak files <file>");
        await Assert.That(output.ToString()).Contains("--unresolved");
    }
}
