namespace GbxCLI.Tests;

/// <summary>
/// Tests for set, eval and verify with a folder or pattern, which run on many files.
/// </summary>
public class BatchTests
{
    [Test]
    public async Task Set_Folder_WritesChangedFilesThenReportsThemUnchanged()
    {
        using var directory = new TempDirectory();
        var a = directory.Write("Maps/a.Gbx", Fixtures.CreateGbx());
        var b = directory.Write("Maps/Sub/b.Gbx", Fixtures.CreateGbx());
        directory.Write("Maps/notes.txt", [1]);
        directory.Write("Maps/old.Gbx.bak", Fixtures.CreateGbx());
        directory.Write("Maps/text.Gbx", "not a Gbx"u8.ToArray());

        var first = await CliRun.RunAsync("set", directory.Combine("Maps"), "Keys[0].TimeValue=7");
        var second = await CliRun.RunAsync("set", directory.Combine("Maps"), "Keys[0].TimeValue=7");

        await Assert.That(first.ExitCode).IsEqualTo(0);
        await Assert.That(Lines(first)).IsEquivalentTo([
            "a.Gbx: changed (Node.Keys[0].TimeValue: 42.5 -> 7)",
            $"{Path.Combine("Sub", "b.Gbx")}: changed (Node.Keys[0].TimeValue: 42.5 -> 7)",
            "Changed 2 Gbx files; 0 unchanged; 0 failed; 1 non-Gbx files skipped."]);
        await Assert.That(Fixtures.ReadTimeValue(a)).IsEqualTo(7f);
        await Assert.That(Fixtures.ReadTimeValue(b)).IsEqualTo(7f);
        await Assert.That(File.Exists(a + ".bak")).IsTrue();
        await Assert.That(File.Exists(directory.Combine("Maps", "old.Gbx.bak.bak"))).IsFalse();

        await Assert.That(second.ExitCode).IsEqualTo(0);
        await Assert.That(second.Output).Contains("Changed 0 Gbx files; 2 unchanged; 0 failed; 1 non-Gbx files skipped.");
    }

    [Test]
    public async Task Set_Pattern_SelectsMatchingFilesOnly()
    {
        using var directory = new TempDirectory();
        var map = directory.Write("Maps/a.Map.Gbx", Fixtures.CreateGbx());
        var other = Fixtures.CreateGbx();
        var otherPath = directory.Write("Maps/b.Other.Gbx", other);

        var run = await CliRun.RunAsync("set", directory.Combine("Maps", "*.Map.Gbx"), "Keys[0].TimeValue=1");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("a.Map.Gbx: changed");
        await Assert.That(run.Output.Contains("b.Other.Gbx")).IsFalse();
        await Assert.That(Fixtures.ReadTimeValue(map)).IsEqualTo(1f);
        await Assert.That(await File.ReadAllBytesAsync(otherPath)).IsEquivalentTo(other);
    }

    [Test]
    public async Task Set_BatchDryRun_WritesNothing()
    {
        using var directory = new TempDirectory();
        var original = Fixtures.CreateGbx();
        var path = directory.Write(Path.Combine("Maps", "a.Gbx"), original);

        var run = await CliRun.RunAsync("set", directory.Combine("Maps"), "Keys[0].TimeValue=1", "-n");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("a.Gbx: would change (Node.Keys[0].TimeValue: 42.5 -> 1)");
        await Assert.That(run.Output).Contains("1 Gbx files would change; 0 unchanged; 0 failed; 0 non-Gbx files skipped. Dry run, so nothing was written.");
        await Assert.That(Directory.GetFiles(directory.Combine("Maps"))).IsEquivalentTo([path]);
        await Assert.That(await File.ReadAllBytesAsync(path)).IsEquivalentTo(original);
    }

    [Test]
    public async Task Set_BatchWithFailure_ContinuesAndFails()
    {
        using var directory = new TempDirectory();
        var broken = Fixtures.BrokenGbx;
        var brokenPath = directory.Write("Maps/a.Gbx", broken);
        var path = directory.Write("Maps/b.Gbx", Fixtures.CreateGbx());

        var run = await CliRun.RunAsync("set", directory.Combine("Maps"), "Keys[0].TimeValue=1");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Output).StartsWith("a.Gbx: ");
        await Assert.That(run.Output).Contains("b.Gbx: changed");
        await Assert.That(run.Output).Contains("Changed 1 Gbx files; 0 unchanged; 1 failed; 0 non-Gbx files skipped.");
        await Assert.That(run.Error).Contains("1 Gbx files failed.");
        await Assert.That(Fixtures.ReadTimeValue(path)).IsEqualTo(1f);
        await Assert.That(await File.ReadAllBytesAsync(brokenPath)).IsEquivalentTo(broken);
    }

    [Test]
    public async Task Set_ArchivePattern_WritesToOutputFolder()
    {
        using var directory = new TempDirectory();
        var zip = directory.Write("maps.zip", Fixtures.CreateZip(("a.Gbx", Fixtures.CreateGbx()), ("Sub/b.Gbx", Fixtures.CreateGbx()), ("notes.txt", [1])));
        var output = directory.Combine("out");

        var withoutOutput = await CliRun.RunAsync("set", $"{zip}/*", "Keys[0].TimeValue=1");
        var withOutput = await CliRun.RunAsync("set", $"{zip}/*", "Keys[0].TimeValue=1", "-o", output);

        await Assert.That(withoutOutput.ExitCode).IsEqualTo(1);
        await Assert.That(withoutOutput.Error).Contains("Use -o to write them to a folder.");
        await Assert.That(withOutput.ExitCode).IsEqualTo(0);
        await Assert.That(withOutput.Output).Contains($"{Path.Combine(zip, "a.Gbx")}: changed");
        await Assert.That(withOutput.Output).Contains("Changed 2 Gbx files; 0 unchanged; 0 failed; 1 non-Gbx files skipped.");
        await Assert.That(Fixtures.ReadTimeValue(Path.Combine(output, "a.Gbx"))).IsEqualTo(1f);
        await Assert.That(Fixtures.ReadTimeValue(Path.Combine(output, "Sub", "b.Gbx"))).IsEqualTo(1f);
    }

    [Test]
    public async Task Set_FolderWithOutput_KeepsRelativePathsAndInputs()
    {
        using var directory = new TempDirectory();
        var original = Fixtures.CreateGbx();
        var input = directory.Write("Maps/Sub/a.Gbx", original);
        var output = directory.Combine("out");

        var run = await CliRun.RunAsync("set", directory.Combine("Maps"), "Keys[0].TimeValue=1", "-o", output);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(Fixtures.ReadTimeValue(Path.Combine(output, "Sub", "a.Gbx"))).IsEqualTo(1f);
        await Assert.That(await File.ReadAllBytesAsync(input)).IsEquivalentTo(original);
    }

    [Test]
    public async Task Set_PatternWithoutMatches_Fails()
    {
        using var directory = new TempDirectory();
        directory.Write("Maps/a.Gbx", Fixtures.CreateGbx());

        var run = await CliRun.RunAsync("set", directory.Combine("Maps", "*.Map.Gbx"), "Keys[0].TimeValue=1");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("No Gbx files were found for");
    }

    [Test]
    public async Task Eval_Folder_ShowsValuesOfEachFile()
    {
        using var directory = new TempDirectory();
        directory.Write("Maps/a.Gbx", Fixtures.CreateGbx());
        directory.Write("Maps/b.Gbx", Fixtures.CreateGbx());

        var run = await CliRun.RunAsync("eval", directory.Combine("Maps"), "var key = Node.Keys[0];", "key.TimeValue", "key.Tangent");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(Lines(run)).IsEquivalentTo([
            "a.Gbx: 42.5 | 0.25",
            "b.Gbx: 42.5 | 0.25",
            "Evaluated 2 Gbx files; 0 failed; 0 non-Gbx files skipped."]);
    }

    [Test]
    public async Task Eval_FolderWrite_OnlyWritesChangedFiles()
    {
        using var directory = new TempDirectory();
        var a = directory.Write("Maps/a.Gbx", Fixtures.CreateGbx());
        var b = directory.Write("Maps/b.Gbx", Fixtures.CreateGbx());
        await CliRun.RunAsync("set", b, "Keys[0].TimeValue=5", "--no-backup");
        var unchanged = await File.ReadAllBytesAsync(b);

        var run = await CliRun.RunAsync("eval", directory.Combine("Maps"), "if (Node.Keys[0].TimeValue == 42.5f) Node.Keys[0].TimeValue = 1;", "-w");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(Lines(run)).IsEquivalentTo([
            "a.Gbx: changed (first difference: Node.Keys[0].TimeValue from 42.5 to 1)",
            "b.Gbx: unchanged",
            "Changed 1 Gbx files; 1 unchanged; 0 failed; 0 non-Gbx files skipped."]);
        await Assert.That(Fixtures.ReadTimeValue(a)).IsEqualTo(1f);
        await Assert.That(await File.ReadAllBytesAsync(b)).IsEquivalentTo(unchanged);
        await Assert.That(File.Exists(b + ".bak")).IsFalse();
    }

    [Test]
    public async Task Eval_FolderCompilationError_FailsEachFile()
    {
        using var directory = new TempDirectory();
        directory.Write("Maps/a.Gbx", Fixtures.CreateGbx());
        directory.Write("Maps/b.Gbx", Fixtures.CreateGbx());

        var run = await CliRun.RunAsync("eval", directory.Combine("Maps"), "MissingMember");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Output).Contains("a.Gbx: CompilationErrorException:");
        await Assert.That(run.Output).Contains("b.Gbx: CompilationErrorException:");
        await Assert.That(run.Error).Contains("2 Gbx files failed.");
    }

    [Test]
    public async Task Eval_FolderInteractive_Fails()
    {
        using var directory = new TempDirectory();
        directory.Write("Maps/a.Gbx", Fixtures.CreateGbx());

        var run = await CliRun.RunAsync("eval", directory.Combine("Maps"), "-i");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("The C# prompt (-i) opens one file, not a folder or pattern.");
    }

    [Test]
    public async Task Verify_DiskPattern_VerifiesMatchingFiles()
    {
        using var directory = new TempDirectory();
        directory.Write("Maps/a.Map.Gbx", Fixtures.CreateGbx());
        directory.Write("Maps/Sub/b.Map.Gbx", Fixtures.CreateGbx());
        directory.Write("Maps/c.Other.Gbx", Fixtures.BrokenGbx);

        var run = await CliRun.RunAsync("verify", directory.Combine("Maps", "*.Map.Gbx"));

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output.Trim()).IsEqualTo("Parsed 2 Gbx files; 0 failed; 0 non-Gbx files skipped.");
    }

    [Test]
    public async Task Script_GetGbxFiles_SelectsLikeSet()
    {
        using var directory = new TempDirectory();
        directory.Write("Maps/a.Map.Gbx", Fixtures.CreateGbx());
        directory.Write("Maps/Sub/b.Map.Gbx", Fixtures.CreateGbx());
        directory.Write("Maps/a.Map.Gbx.bak", Fixtures.CreateGbx());
        var script = directory.Write("list.csx", "foreach (var file in GetGbxFiles(Args[0])) Print(Path.GetFileName(file));"u8.ToArray());

        var run = await CliRun.RunAsync("script", script, directory.Combine("Maps"));

        await Assert.That(run.Error).IsEqualTo(string.Empty);
        await Assert.That(Lines(run)).IsEquivalentTo(["a.Map.Gbx", "b.Map.Gbx"]);
    }

    private static string[] Lines(CliRun run) => run.Output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
}
