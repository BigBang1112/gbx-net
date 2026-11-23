using GBX.NET;
using GBX.NET.Engines.Game;

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
    public async Task Eval_Write_ReplacesInputAndKeepsBackup()
    {
        using var directory = new TempDirectory();
        var original = Fixtures.CreateGbx();
        var path = directory.Write("input.Gbx", original);

        var run = await CliRun.RunAsync("eval", path, "Node.Keys[0].TimeValue = 7.5f", "--write");

        await Assert.That(run.Error).IsEqualTo(string.Empty);
        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains($"Written: {path}");
        await Assert.That(Fixtures.ReadTimeValue(path)).IsEqualTo(7.5f);
        await Assert.That(await File.ReadAllBytesAsync(path + ".bak")).IsEquivalentTo(original);
    }

    [Test]
    public async Task Eval_Output_KeepsInputAndWritesNoBackup()
    {
        using var directory = new TempDirectory();
        var original = Fixtures.CreateGbx();
        var path = directory.Write("input.Gbx", original);
        var output = directory.Write("output.Gbx", [1, 2, 3]);

        var run = await CliRun.RunAsync("eval", path, "Node.Keys[0].TimeValue = 1", "-o", output, "--no-backup");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(await File.ReadAllBytesAsync(path)).IsEquivalentTo(original);
        await Assert.That(Fixtures.ReadTimeValue(output)).IsEqualTo(1f);
        await Assert.That(File.Exists(output + ".bak")).IsFalse();
    }

    [Test]
    public async Task Eval_WriteInsideArchive_FailsBeforeExpressions()
    {
        using var directory = new TempDirectory();
        var zip = directory.Write("skin.zip", Fixtures.CreateZip(("a.Gbx", Fixtures.CreateGbx())));

        var run = await CliRun.RunAsync("eval", $"{zip}/a.Gbx", "\"evaluated\"", "-w");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("Use -o to write it to a file.");
        await Assert.That(run.Output.Contains("evaluated")).IsFalse();
    }

    [Test]
    public async Task Eval_WriteArchive_Fails()
    {
        using var directory = new TempDirectory();
        var zip = directory.Write("skin.zip", Fixtures.CreateZip(("a.txt", [1])));

        var run = await CliRun.RunAsync("eval", zip, "1", "-w");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("Only a Gbx can be changed and written, not a ZipArchive.");
    }

    [Test]
    public async Task Eval_WriteUnknownRootClass_Fails()
    {
        using var directory = new TempDirectory();
        var data = Fixtures.CreateGbx();
        BitConverter.GetBytes(0x0BADC000u).CopyTo(data, 9);
        var path = directory.Write("unknown.Gbx", data);

        var run = await CliRun.RunAsync("eval", path, "1", "-w");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("Root class 0x0BADC000 is not supported, so its body cannot be saved.");
        await Assert.That(await File.ReadAllBytesAsync(path)).IsEquivalentTo(data);
    }

    [Test]
    [Arguments("-w", "-i")]
    [Arguments("--force")]
    [Arguments("--no-backup")]
    public async Task Eval_InvalidWriteOptions_Fail(params string[] options)
    {
        var run = await CliRun.RunAsync(["eval", "input.Gbx", "1", .. options]);

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("--");
    }

    [Test]
    public async Task Eval_InteractiveSave_WritesInputAndOtherFile()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("input.Gbx", Fixtures.CreateGbx());
        var output = directory.Combine("other file.Gbx");

        var run = await CliRun.RunInteractiveAsync($"Node.Keys[0].TimeValue = 3;\nsave\nsave --no-backup \"{output}\"\nq\n", "eval", path, "-i");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains($"Written: {path}");
        await Assert.That(run.Output).Contains($"Written: {output}");
        await Assert.That(Fixtures.ReadTimeValue(path)).IsEqualTo(3f);
        await Assert.That(Fixtures.ReadTimeValue(output)).IsEqualTo(3f);
    }

    [Test]
    public async Task Eval_InteractiveSaveInsideArchive_ShowsErrorAndContinues()
    {
        using var directory = new TempDirectory();
        var zip = directory.Write("skin.zip", Fixtures.CreateZip(("a.Gbx", Fixtures.CreateGbx())));

        var run = await CliRun.RunInteractiveAsync("save\n40 + 2\nq\n", "eval", $"{zip}/a.Gbx", "-i");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("error: A Gbx inside an archive cannot be written back to the archive.");
        await Assert.That(run.Output).Contains("Result: 42");
    }

    [Test]
    public async Task Script_SaveAsync_WritesGbx()
    {
        using var directory = new TempDirectory();
        var input = directory.Write("input.Gbx", Fixtures.CreateGbx());
        var output = directory.Combine("output.Gbx");
        var script = directory.Write("save.csx", """
            var gbx = Gbx.Parse<CGameCtnMediaBlockTime>(Args[0]);
            gbx.Node.Keys[0].TimeValue = 9;
            await SaveAsync(gbx, Args[1]);
            """u8.ToArray());

        var run = await CliRun.RunAsync("script", script, input, output);

        await Assert.That(run.Error).IsEqualTo(string.Empty);
        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output.Trim()).IsEqualTo($"Written: {output}");
        await Assert.That(Fixtures.ReadTimeValue(output)).IsEqualTo(9f);
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
