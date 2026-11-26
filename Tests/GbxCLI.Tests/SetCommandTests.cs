using GBX.NET;
using GBX.NET.Engines.Game;

namespace GbxCLI.Tests;

public class SetCommandTests
{
    [Test]
    public async Task Set_NodeMember_ReplacesInputAndKeepsBackup()
    {
        using var directory = new TempDirectory();
        var original = Fixtures.CreateGbx();
        var path = directory.Write("input.Gbx", original);

        var run = await CliRun.RunAsync("set", path, "Keys[0].TimeValue=7.5");
        var lines = run.Output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        await Assert.That(run.Error).IsEqualTo(string.Empty);
        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(lines).IsEquivalentTo(["Node.Keys[0].TimeValue: 42.5 -> 7.5", $"Written: {path}", $"Backup: {path}.bak"]);
        await Assert.That(Fixtures.ReadTimeValue(path)).IsEqualTo(7.5f);
        await Assert.That(await File.ReadAllBytesAsync(path + ".bak")).IsEquivalentTo(original);
    }

    [Test]
    public async Task Set_DryRun_WritesNothing()
    {
        using var directory = new TempDirectory();
        var original = Fixtures.CreateGbx();
        var path = directory.Write("input.Gbx", original);

        var run = await CliRun.RunAsync("set", path, "Node.Keys[0].Tangent=1", "--dry-run");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("Node.Keys[0].Tangent: 0.25 -> 1");
        await Assert.That(run.Output).Contains("Dry run, so nothing was written.");
        await Assert.That(Directory.GetFiles(directory.Path)).IsEquivalentTo([path]);
        await Assert.That(await File.ReadAllBytesAsync(path)).IsEquivalentTo(original);
    }

    [Test]
    public async Task Set_Output_KeepsInput()
    {
        using var directory = new TempDirectory();
        var original = Fixtures.CreateGbx();
        var path = directory.Write("input.Gbx", original);
        var output = directory.Combine("output.Gbx");

        var run = await CliRun.RunAsync("set", path, "Keys[0].TimeValue=1", "-o", output);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(await File.ReadAllBytesAsync(path)).IsEquivalentTo(original);
        await Assert.That(Fixtures.ReadTimeValue(output)).IsEqualTo(1f);
    }

    [Test]
    public async Task Set_InsideArchive_NeedsOutput()
    {
        using var directory = new TempDirectory();
        var zip = directory.Write("skin.zip", Fixtures.CreateZip(("a.Gbx", Fixtures.CreateGbx())));
        var output = directory.Combine("a.Gbx");

        var withoutOutput = await CliRun.RunAsync("set", $"{zip}/a.Gbx", "Keys[0].TimeValue=1");
        var dryRun = await CliRun.RunAsync("set", $"{zip}/a.Gbx", "Keys[0].TimeValue=1", "-n");
        var withOutput = await CliRun.RunAsync("set", $"{zip}/a.Gbx", "Keys[0].TimeValue=1", "-o", output);

        await Assert.That(withoutOutput.ExitCode).IsEqualTo(1);
        await Assert.That(withoutOutput.Error).Contains("Use -o to write it to a file.");
        await Assert.That(dryRun.ExitCode).IsEqualTo(0);
        await Assert.That(withOutput.ExitCode).IsEqualTo(0);
        await Assert.That(Fixtures.ReadTimeValue(output)).IsEqualTo(1f);
    }

    [Test]
    [Arguments("Missing=1", "Neither the Gbx nor its CGameCtnMediaBlockTime node has a member 'Missing'.")]
    [Arguments("Keys[5].TimeValue=1", "Node.Keys has 1 items, so it has no item [5].")]
    [Arguments("Keys[0].TimeValue=abc", "'abc' is not a valid Single.")]
    [Arguments("Keys=1", "cannot be set from text.")]
    public async Task Set_InvalidAssignment_FailsWithoutWriting(string assignment, string message)
    {
        using var directory = new TempDirectory();
        var original = Fixtures.CreateGbx();
        var path = directory.Write("input.Gbx", original);

        var run = await CliRun.RunAsync("set", path, assignment);

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains(message);
        await Assert.That(Directory.GetFiles(directory.Path)).IsEquivalentTo([path]);
        await Assert.That(await File.ReadAllBytesAsync(path)).IsEquivalentTo(original);
    }

    [Test]
    public async Task Set_ArgumentWithoutValue_Fails()
    {
        var run = await CliRun.RunAsync("set", "input.Gbx", "Node.MapName");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("Unexpected argument for set: Node.MapName.");
    }

    [Test]
    public async Task Set_WithoutAssignment_Fails()
    {
        var run = await CliRun.RunAsync("set", "input.Gbx");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("Missing <member>=<value> for set.");
    }
}
