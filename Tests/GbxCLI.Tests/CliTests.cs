namespace GbxCLI.Tests;

public class CliTests
{
    [Test]
    public async Task Run_NoArguments_WritesHelp()
    {
        var run = await CliRun.RunAsync();

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("Usage: gbx <command>");
        await Assert.That(run.Output).Contains("pack extract <archive>");
        await Assert.That(run.Output.Contains("Press Enter")).IsFalse();
    }

    [Test]
    [Arguments("-h")]
    [Arguments("--help")]
    public async Task Run_Help_WritesHelpWithoutFormattingCodes(string option)
    {
        var run = await CliRun.RunAsync(option);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("Global options:");
        await Assert.That(run.Output.Contains('$')).IsFalse();
        await Assert.That(run.Output.Contains('\u001b')).IsFalse();
    }

    [Test]
    [Arguments("-v")]
    [Arguments("--version")]
    public async Task Run_Version_WritesDistribution(string option)
    {
        var run = await CliRun.RunAsync(option);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).StartsWith("gbx ");
        await Assert.That(run.Output).Contains($"({Cli.Distribution})");
    }

    [Test]
    public async Task Run_UnknownCommand_FailsWithMessage()
    {
        var run = await CliRun.RunAsync("frobnicate");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Output).IsEqualTo(string.Empty);
        await Assert.That(run.Error.Trim()).IsEqualTo("gbx: Unknown command: frobnicate. Use --help for usage.");
    }

    [Test]
    [Arguments("inspect", "Usage: gbx inspect")]
    [Arguments("parse", "Usage: gbx inspect")]
    [Arguments("eval", "Usage: gbx eval")]
    [Arguments("script", "Usage: gbx script")]
    [Arguments("verify", "Usage: gbx verify")]
    [Arguments("pack", "Usage: gbx pack")]
    [Arguments("compress", "Usage: gbx compress")]
    [Arguments("decompress", "Usage: gbx decompress")]
    public async Task Run_CommandHelp_WritesCommandSection(string command, string expected)
    {
        var run = await CliRun.RunAsync(command, "--help");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains(expected);
    }

    [Test]
    [Arguments("pack", "list", "--help")]
    [Arguments("pack", "extract", "-h")]
    [Arguments("pack", "verify", "--help")]
    public async Task Run_PackSubcommandHelp_WritesHelp(string pack, string command, string option)
    {
        var run = await CliRun.RunAsync(pack, command, option);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("Usage: gbx");
    }

    [Test]
    [Arguments("inspect")]
    [Arguments("eval")]
    [Arguments("verify")]
    [Arguments("compress")]
    [Arguments("decompress")]
    public async Task Run_MissingInput_Fails(string command)
    {
        var run = await CliRun.RunAsync(command);

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains($"Missing input file for {command}.");
    }

    [Test]
    [Arguments("inspect", "a.Gbx", "b.Gbx", "inspect")]
    [Arguments("verify", "a", "b", "verify")]
    [Arguments("compress", "a.Gbx", "b.Gbx", "compress")]
    [Arguments("decompress", "a.Gbx", "--bogus", "decompress")]
    public async Task Run_ExtraArgument_UsesOneMessageFormat(string command, string first, string second, string name)
    {
        var run = await CliRun.RunAsync(command, first, second);

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains($"Unexpected argument for {name}: {second}.");
    }

    [Test]
    public async Task Run_PackWithoutSubcommand_Fails()
    {
        var run = await CliRun.RunAsync("pack");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("Missing command for pack");
    }

    [Test]
    public async Task Run_MissingOptionValue_Fails()
    {
        var run = await CliRun.RunAsync("compress", "a.Gbx", "-o");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("Missing value after --output.");
    }

    [Test]
    public async Task Run_ExistingFileWithoutCommand_WritesSummaryWhenNotInteractive()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("input.Gbx", Fixtures.CreateGbx());

        var run = await CliRun.RunAsync(path);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("Class ID: 0x03085000");
        await Assert.That(run.Output.Contains("Interactive mode")).IsFalse();
    }

    [Test]
    public async Task Run_ArchivePathWithoutCommand_OpensEntry()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("skin.zip", Fixtures.CreateZip(("Folder/Map.Gbx", Fixtures.CreateGbx())));

        var run = await CliRun.RunAsync(path + "/Folder/Map.Gbx");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("CGameCtnMediaBlockTime");
    }

    [Test]
    public async Task Run_Exception_WritesEscapedMessageWithoutStackTrace()
    {
        var run = await CliRun.RunAsync("inspect", "missing $file.Gbx");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error.Trim()).IsEqualTo("gbx: File 'missing $file.Gbx' was not found.");
    }

    [Test]
    public async Task Run_InteractiveWithoutArguments_OpensDraggedPath()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("dragged file.Gbx", Fixtures.CreateGbx());

        var run = await CliRun.RunInteractiveAsync($"\"{path}\"\nq\n");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("Drag a Gbx");
        await Assert.That(run.Output).Contains("Node (CGameCtnMediaBlockTime)");
    }

    [Test]
    public async Task Run_InteractiveWithoutArguments_EmptyLineWritesHelp()
    {
        var run = await CliRun.RunInteractiveAsync("\n\n");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("Global options:");
        await Assert.That(run.Output).Contains("Press Enter to close.");
    }

    [Test]
    public async Task Run_InteractiveWithoutArguments_MissingPathAsksAgain()
    {
        var run = await CliRun.RunInteractiveAsync("missing.Gbx\n");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("File 'missing.Gbx' was not found.");
    }
}
