using GBX.NET;
using GbxCLI.Commands;

namespace GbxCLI.Tests;

public class VerifyCommandTests
{
    [Test]
    public async Task Verify_Folder_WalksSubfoldersInNameOrderAndContinuesAfterFailures()
    {
        using var directory = new TempDirectory();
        directory.Write("b/Good.Gbx", Fixtures.CreateGbx());
        directory.Write("a/Broken.Gbx", Fixtures.BrokenGbx);
        directory.Write("c/notes.txt", "text"u8.ToArray());
        directory.Write("d/archive.zip", Fixtures.CreateZip(("Inner.Gbx", Fixtures.CreateGbx(compressed: true)), ("x.txt", [1])));

        var run = await CliRun.RunAsync("verify", directory.Path);
        var lines = run.Output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(lines.Length).IsEqualTo(2);
        await Assert.That(lines[0]).StartsWith(Path.Combine("a", "Broken.Gbx") + ": ");
        await Assert.That(lines[1]).IsEqualTo("Parsed 2 Gbx files; 1 failed; 2 non-Gbx files skipped.");
    }

    [Test]
    public async Task Verify_Strict_FailsAfterSummary()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("Broken.Gbx", Fixtures.BrokenGbx);

        var run = await CliRun.RunAsync("verify", path, "--strict");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Output).Contains("Parsed 0 Gbx files; 1 failed; 0 non-Gbx files skipped.");
        await Assert.That(run.Error).Contains("1 files failed verification.");
    }

    [Test]
    public async Task Verify_FailureWithoutStrict_ExitsWithZero()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("Broken.Gbx", Fixtures.BrokenGbx);

        var run = await CliRun.RunAsync("verify", path);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("1 failed");
    }

    [Test]
    [Arguments("verify")]
    [Arguments("pack verify")]
    public async Task Verify_Pak_ReportsEntryPaths(string command)
    {
        using var directory = new TempDirectory();
        var path = directory.Write("archive.pak", Fixtures.CreatePak(false,
            ("Media/Good.Gbx", Fixtures.CreateGbx()),
            ("Media/Broken.Gbx", Fixtures.BrokenGbx),
            ("readme.txt", "text"u8.ToArray())));

        var run = await CliRun.RunAsync([.. command.Split(' '), path]);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains(Path.Combine(path, "Media", "Broken.Gbx") + ": ");
        await Assert.That(run.Output).Contains("Parsed 1 Gbx files; 1 failed; 1 non-Gbx files skipped.");
    }

    [Test]
    public async Task Verify_PathInsidePak_VerifiesSelectedEntries()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("archive.pak", Fixtures.CreatePak(false,
            ("Media/Good.Gbx", Fixtures.CreateGbx()),
            ("Other/Broken.Gbx", Fixtures.BrokenGbx)));

        var run = await CliRun.RunAsync("verify", path + "/Media/*");

        await Assert.That(run.Output.Trim()).IsEqualTo("Parsed 1 Gbx files; 0 failed; 0 non-Gbx files skipped.");
    }

    [Test]
    public async Task Verify_PathInsideNestedZip_VerifiesSelectedEntries()
    {
        using var directory = new TempDirectory();
        var inner = Fixtures.CreateZip(("Good.Gbx", Fixtures.CreateGbx()), ("Broken.Gbx", Fixtures.BrokenGbx));
        var path = directory.Write("outer.zip", Fixtures.CreateZip(("Inner.zip", inner)));

        var run = await CliRun.RunAsync("verify", path + "/Inner.zip/Good.Gbx");

        await Assert.That(run.Output.Trim()).IsEqualTo("Parsed 1 Gbx files; 0 failed; 0 non-Gbx files skipped.");
    }

    [Test]
    public async Task Verify_PatternWithoutMatches_CountsFailure()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("archive.zip", Fixtures.CreateZip(("a.txt", [1])));

        var run = await CliRun.RunAsync("verify", path + "/*.Gbx");

        await Assert.That(run.Output).Contains("No files matching '*.Gbx'");
        await Assert.That(run.Output).Contains("1 failed");
    }

    [Test]
    [Arguments("TMF", GameVersion.TMF)]
    [Arguments("mp4", GameVersion.MP4)]
    public async Task ParseGameVersion_SingleName_IsAccepted(string value, GameVersion expected)
    {
        await Assert.That(VerifyCommand.ParseGameVersion(value)).IsEqualTo(expected);
    }

    [Test]
    [Arguments("3")]
    [Arguments("-1")]
    [Arguments("TMF, MP4")]
    [Arguments("Nope")]
    public async Task Verify_InvalidGame_Fails(string value)
    {
        var run = await CliRun.RunAsync("verify", "anything", "--game", value);

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains($"Unknown game version: {value}.");
    }
}
