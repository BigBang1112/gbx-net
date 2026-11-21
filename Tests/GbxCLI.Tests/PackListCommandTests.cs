namespace GbxCLI.Tests;

public class PackListCommandTests
{
    [Test]
    public async Task List_Zip_WritesFileEntriesInOrder()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("skin.zip", Fixtures.CreateZip(("b.txt", [1]), ("Folder/", []), ("Folder/a.txt", [2])));

        var run = await CliRun.RunAsync("pack", "list", path);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(Lines(run.Output)).IsEquivalentTo([Path.Combine("Folder", "a.txt"), "b.txt"]);
    }

    [Test]
    [Arguments("Media/*", "Media/One.txt|Media/Sub/Two.txt")]
    [Arguments("*.bin", "Other.bin")]
    [Arguments("two.TXT", "Media/Sub/Two.txt")]
    [Arguments("Media\\Sub\\Two.txt", "Media/Sub/Two.txt")]
    public async Task List_PakPattern_WritesMatches(string pattern, string expected)
    {
        using var directory = new TempDirectory();
        var path = directory.Write("archive.pak", Fixtures.CreatePak(false,
            ("Media/One.txt", [1]), ("Media/Sub/Two.txt", [2]), ("Other.bin", [3])));

        var run = await CliRun.RunAsync("pack", "list", path + "/" + pattern);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(Lines(run.Output)).IsEquivalentTo(expected.Split('|').Select(name => name.Replace('/', Path.DirectorySeparatorChar)));
    }

    [Test]
    public async Task List_PatternWithoutMatches_Fails()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("archive.pak", Fixtures.CreatePak(false, ("a.txt", [1])));

        var run = await CliRun.RunAsync("pack", "list", path + "/missing.txt");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("No files matching 'missing.txt'");
    }

    [Test]
    public async Task List_ResolvedAndUnresolved_FilterHashedNames()
    {
        using var directory = new TempDirectory();
        var hash = "0123456789ABCDEF0123456789ABCDEF00";
        var path = directory.Write("archive.pak", Fixtures.CreatePak(false, ("Media/" + hash, [1]), ("Media/Named.txt", [2])));

        var resolved = await CliRun.RunAsync("pack", "list", path, "--resolved");
        var unresolved = await CliRun.RunAsync("pack", "list", path, "--unresolved");

        await Assert.That(Lines(resolved.Output)).IsEquivalentTo([Path.Combine("Media", "Named.txt")]);
        await Assert.That(Lines(unresolved.Output)).IsEquivalentTo([Path.Combine("Media", hash)]);
    }

    [Test]
    public async Task List_UnresolvedZip_ListsNothing()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("skin.zip", Fixtures.CreateZip(("a.txt", [1])));

        var run = await CliRun.RunAsync("pack", "list", path, "--unresolved");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task List_ResolvedAndUnresolvedTogether_Fails()
    {
        var run = await CliRun.RunAsync("pack", "list", "archive.pak", "--resolved", "--unresolved");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("cannot be combined");
    }

    [Test]
    public async Task List_NestedZip_ListsInnerEntries()
    {
        using var directory = new TempDirectory();
        var inner = Fixtures.CreateZip(("x.Gbx", [1]), ("y.txt", [2]));
        var path = directory.Write("outer.zip", Fixtures.CreateZip(("Inner.zip", inner)));

        var run = await CliRun.RunAsync("pack", "list", path + "/Inner.zip/*.Gbx");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(Lines(run.Output)).IsEquivalentTo(["x.Gbx"]);
    }

    [Test]
    public async Task List_PrivatePakWithoutKey_Fails()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("Unknown.pak", Fixtures.CreateEncryptedPak(Fixtures.TMNextKey, publicHeader: false));

        var run = await CliRun.RunAsync("pack", "list", path);

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("requires a matching key");
    }

    [Test]
    public async Task List_Gbx_Fails()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("input.Gbx", Fixtures.CreateGbx());

        var run = await CliRun.RunAsync("pack", "list", path);

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("not a PAK (NadeoPak) or ZIP archive");
    }

    private static string[] Lines(string output) => output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
}
