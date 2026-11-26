namespace GbxCLI.Tests;

public class InspectCommandTests
{
    [Test]
    [Arguments("inspect", false)]
    [Arguments("parse", true)]
    public async Task Inspect_Gbx_WritesSummary(string command, bool compressed)
    {
        using var directory = new TempDirectory();
        var path = directory.Write("input.Gbx", Fixtures.CreateGbx(compressed));

        var run = await CliRun.RunAsync(command, path);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Error).IsEqualTo(string.Empty);
        await Assert.That(run.Output).Contains($"File: {path}");
        await Assert.That(run.Output).Contains("Class ID: 0x03085000");
        await Assert.That(run.Output).Contains($"Body compression: {(compressed ? "Compressed" : "Uncompressed")}");
        await Assert.That(run.Output).Contains("Parse status: Complete");
        await Assert.That(run.Output).Contains("Node: CGameCtnMediaBlockTime");
        await Assert.That(run.Output).Contains("TimeValue: 42.5");
        await Assert.That(run.Output).Contains("Preview limits:");
        await Assert.That(run.Output.Contains('\u001b')).IsFalse();
    }

    [Test]
    public async Task Inspect_Pak_WritesArchiveSummary()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("archive.pak", Fixtures.CreatePakData());

        var run = await CliRun.RunAsync("inspect", path);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("Format: NadeoPak");
        await Assert.That(run.Output).Contains("Version: 6");
        await Assert.That(run.Output).Contains("Header encryption: Unencrypted");
        await Assert.That(run.Output).Contains("Files: 1");
    }

    [Test]
    public async Task Inspect_PrivatePakWithoutKey_SaysIndexNeedsKey()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("Unknown.pak", Fixtures.CreateEncryptedPak(Fixtures.TMNextKey, publicHeader: false));

        var run = await CliRun.RunAsync("inspect", path);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("Header encryption: Encrypted");
        await Assert.That(run.Output).Contains("Files: Unavailable (private header requires a key).");
    }

    [Test]
    [Arguments("TMNext.pak")]
    [Arguments("TMNext_2014-04-30.pak")]
    public async Task Inspect_PrivatePakWithKnownName_ListsFiles(string name)
    {
        using var directory = new TempDirectory();
        var path = directory.Write(name, Fixtures.CreateEncryptedPak(Fixtures.TMNextKey, publicHeader: false));

        var run = await CliRun.RunAsync("inspect", path);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("Files: 1");
    }

    [Test]
    public async Task Inspect_Zip_CountsFileEntries()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("skin.zip", Fixtures.CreateZip(("a.txt", [1]), ("Folder/", []), ("Folder/b.txt", [2])));

        var run = await CliRun.RunAsync("inspect", path);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("Format: ZIP");
        await Assert.That(run.Output).Contains("Files: 2");
    }

    [Test]
    [Arguments("Inner.zip/Map.Gbx")]
    [Arguments("inner.zip\\MAP.gbx")]
    [Arguments("Inner.zip/*.Gbx")]
    public async Task Inspect_NestedZipEntry_ParsesGbx(string entryPath)
    {
        using var directory = new TempDirectory();
        var inner = Fixtures.CreateZip(("Map.Gbx", Fixtures.CreateGbx()), ("readme.txt", [1]));
        var path = directory.Write("outer.zip", Fixtures.CreateZip(("Inner.zip", inner)));

        var run = await CliRun.RunAsync("inspect", path + "/" + entryPath);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Error).IsEqualTo(string.Empty);
        await Assert.That(run.Output).Contains("Class ID: 0x03085000");
    }

    [Test]
    public async Task Inspect_AmbiguousEntry_Fails()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("skin.zip", Fixtures.CreateZip(("A/Map.Gbx", Fixtures.CreateGbx()), ("B/Map.Gbx", Fixtures.CreateGbx())));

        var run = await CliRun.RunAsync("inspect", path + "/Map.Gbx");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("ambiguous");
    }

    [Test]
    public async Task Inspect_PathInsideGbx_Fails()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("input.Gbx", Fixtures.CreateGbx());

        var run = await CliRun.RunAsync("inspect", path + "/entry");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("is a Gbx file, so it has no entry 'entry'");
    }

    [Test]
    public async Task Inspect_UnsupportedFile_Fails()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("notes.txt", "hello"u8.ToArray());

        var run = await CliRun.RunAsync("inspect", path);

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("unsupported format");
    }

    [Test]
    public async Task Inspect_InteractiveWithoutConsole_WritesSummary()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("input.Gbx", Fixtures.CreateGbx());

        var run = await CliRun.RunAsync("inspect", path, "-i");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("Class ID: 0x03085000");
    }
}
