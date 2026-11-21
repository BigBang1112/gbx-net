namespace GbxCLI.Tests;

/// <summary>
/// Tests for eval and script, which compile and run C# with Roslyn.
/// </summary>
public class ScriptingTests
{
    [Test]
    public async Task Eval_Expressions_ShareOneSession()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("input.Gbx", Fixtures.CreateGbx());

        var run = await CliRun.RunAsync("eval", path, "var count = Node.Keys.Count;", "Header.ClassId.ToString(\"X8\") + \":\" + count");

        await Assert.That(run.Error).IsEqualTo(string.Empty);
        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("Result: 03085000:1");
    }

    [Test]
    public async Task Eval_ZipArchive_IsTheGlobal()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("skin.zip", Fixtures.CreateZip(("a.txt", [1]), ("b.txt", [2])));

        var run = await CliRun.RunAsync("eval", path, "Entries.Count");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("Result: 2");
    }

    [Test]
    public async Task Eval_WithoutExpression_Fails()
    {
        var run = await CliRun.RunAsync("eval", "input.Gbx");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("Missing C# expression for eval.");
    }

    [Test]
    public async Task Eval_CompilationError_Fails()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("input.Gbx", Fixtures.CreateGbx());

        var run = await CliRun.RunAsync("eval", path, "MissingMember");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("MissingMember");
    }

    [Test]
    public async Task Eval_Interactive_RunsPromptAndContinuesAfterErrors()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("input.Gbx", Fixtures.CreateGbx());

        var run = await CliRun.RunInteractiveAsync("var x = 20;\nMissingMember\nx + 22\nq\n", "eval", path, "-i");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("C# prompt for");
        await Assert.That(run.Output).Contains("MissingMember");
        await Assert.That(run.Output).Contains("Result: 42");
    }

    [Test]
    public async Task Script_ArgumentsAfterScript_ArePassedAsArgs()
    {
        using var directory = new TempDirectory();
        var script = directory.Write("print.csx", "Print(string.Join(\"|\", Args));"u8.ToArray());

        var run = await CliRun.RunAsync("script", script, "-o", "--help", "x");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output.Trim()).IsEqualTo("-o|--help|x");
    }

    [Test]
    public async Task Script_DoubleDash_AllowsPathStartingWithDash()
    {
        using var directory = new TempDirectory();
        directory.Write("-dash.csx", "Output.WriteLine(Args.Length);"u8.ToArray());

        var run = await CliRun.RunAsync("script", "--", directory.Combine("-dash.csx"), "a");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output.Trim()).IsEqualTo("1");
    }

    [Test]
    public async Task Script_OptionBeforeScript_Fails()
    {
        var run = await CliRun.RunAsync("script", "-x");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("Unexpected argument for script: -x.");
    }
}
