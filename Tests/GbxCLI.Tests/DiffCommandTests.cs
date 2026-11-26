namespace GbxCLI.Tests;

public class DiffCommandTests
{
    [Test]
    public async Task Diff_ChangedFile_ListsEveryDifference()
    {
        using var directory = new TempDirectory();
        var left = directory.Write("left.Gbx", Fixtures.CreateGbx());
        var right = directory.Combine("right.Gbx");
        await CliRun.RunAsync("set", left, "Keys[0].TimeValue=7", "Keys[0].Tangent=1", "-o", right);

        var run = await CliRun.RunAsync("diff", left, right);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(Lines(run)).IsEquivalentTo([
            "Node.Keys[0].Tangent: 0.25 -> 1",
            "Node.Keys[0].TimeValue: 42.5 -> 7",
            "2 differences."]);
    }

    [Test]
    public async Task Diff_DifferentItemCount_ReportsCount()
    {
        using var directory = new TempDirectory();
        var left = directory.Write("left.Gbx", Fixtures.CreateGbx());
        var right = directory.Combine("right.Gbx");
        await CliRun.RunAsync("eval", left, "Node.Keys.Add(new GBX.NET.Engines.Game.CGameCtnMediaBlockTime.Key())", "-o", right);

        var run = await CliRun.RunAsync("diff", left, right);

        await Assert.That(run.Output).Contains("Node.Keys.Count: 1 items -> 2 items");
    }

    [Test]
    public async Task Diff_SameFile_ReportsNoDifferences()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("a.Gbx", Fixtures.CreateGbx());

        var run = await CliRun.RunAsync("diff", path, path, "--strict");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output.Trim()).IsEqualTo("No differences.");
    }

    [Test]
    public async Task Diff_MaxAndStrict_StopAndFail()
    {
        using var directory = new TempDirectory();
        var left = directory.Write("left.Gbx", Fixtures.CreateGbx());
        var right = directory.Combine("right.Gbx");
        await CliRun.RunAsync("set", left, "Keys[0].TimeValue=7", "Keys[0].Tangent=1", "-o", right);

        var run = await CliRun.RunAsync("diff", left, right, "--max", "1", "--strict");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(Lines(run)).IsEquivalentTo([
            "Node.Keys[0].Tangent: 0.25 -> 1",
            "Stopped after 1 difference. Use --max 0 to list all of them."]);
        await Assert.That(run.Error).Contains("The Gbx files differ.");
    }

    [Test]
    public async Task Diff_InvalidMax_Fails()
    {
        var run = await CliRun.RunAsync("diff", "a.Gbx", "b.Gbx", "--max", "-1");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("--max takes a number of differences");
    }

    [Test]
    public async Task Eval_Write_ListsChangesBeforeWriting()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("a.Gbx", Fixtures.CreateGbx());

        var changed = await CliRun.RunAsync("eval", path, "Node.Keys[0].TimeValue = 1;", "-n");
        var unchanged = await CliRun.RunAsync("eval", path, "Node.Keys[0].TimeValue", "-n");

        await Assert.That(Lines(changed)[^3..]).IsEquivalentTo(["Changes:", "  Node.Keys[0].TimeValue: 42.5 -> 1", "Dry run, so nothing was written."]);
        await Assert.That(unchanged.Output).Contains("No changes.");
    }

    private static string[] Lines(CliRun run) => run.Output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
}
