namespace GbxCLI.Tests;

public class BrowserTests
{
    [Test]
    public async Task Browse_Gbx_OpensOnNodeWithNumberedMembers()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("input.Gbx", Fixtures.CreateGbx());

        var run = await CliRun.RunInteractiveAsync("q\n", path);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("Node (CGameCtnMediaBlockTime)");
        await Assert.That(run.Output).Contains("[1] ");
        await Assert.That(run.Output).Contains("Keys: 1 items");
        await Assert.That(run.Output).Contains("CGameCtnMediaBlockTime> ");
    }

    [Test]
    public async Task Browse_NumberBackAndRoot_Navigate()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("input.Gbx", Fixtures.CreateGbx());
        var keys = await FindItemAsync(path, "Keys");

        var run = await CliRun.RunInteractiveAsync($"{keys}\n1\n..\n/\n..\nq\n", "inspect", path, "-i");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("Node.Keys (");
        await Assert.That(run.Output).Contains("Node.Keys[0] (Key)");
        await Assert.That(run.Output).Contains("TimeValue: 42.5");
        await Assert.That(run.Output).Contains("input.Gbx (Gbx<CGameCtnMediaBlockTime>)");
        await Assert.That(run.Output).Contains("Already at the root.");
    }

    [Test]
    public async Task Browse_ScalarMember_WritesFullValue()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("input.Gbx", Fixtures.CreateGbx());
        var keys = await FindItemAsync(path, "Keys");

        var run = await CliRun.RunInteractiveAsync($"{keys}\n1\n{await FindItemAsync(path, "TimeValue", keys, 1)}\nq\n", path);

        // The prompt is followed by the value, because the input is not echoed.
        await Assert.That(run.Output).Contains("Key> TimeValue: 42.5" + Environment.NewLine);
    }

    [Test]
    public async Task Browse_InvalidInput_ShowsNoticesAndContinues()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("input.Gbx", Fixtures.CreateGbx());

        var run = await CliRun.RunInteractiveAsync("999\nzzz\nf *.Gbx\nn\np\n?\nq\n", path);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("There is no item 999.");
        await Assert.That(run.Output).Contains("Unknown command: zzz.");
        await Assert.That(run.Output).Contains("Filters only apply to archive entries.");
        await Assert.That(run.Output).Contains("Already on the last page.");
        await Assert.That(run.Output).Contains("Already on the first page.");
        await Assert.That(run.Output).Contains("Enter a number to open an item");
    }

    [Test]
    public async Task Browse_PakEntries_PagesFiltersAndOpensEntries()
    {
        using var directory = new TempDirectory();
        var files = Enumerable.Range(0, 25).Select(i => ($"Data/{i:00}.txt", new byte[] { (byte)i }))
            .Append(("Media/Time.Gbx", Fixtures.CreateGbx()))
            .ToArray();
        var path = directory.Write("archive.pak", Fixtures.CreatePak(false, files));

        var run = await CliRun.RunInteractiveAsync("n\nf Media/*\n1\n..\n..\nf\n1\nq\n", path);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("Page 1 of 2 (26 items)");
        await Assert.That(run.Output).Contains("Page 2 of 2 (26 items)");
        await Assert.That(run.Output).Contains("Filter: Media/* (1 entries)");
        await Assert.That(run.Output).Contains("Node (CGameCtnMediaBlockTime)");
        await Assert.That(run.Output).Contains("error: PAK entry 'Data/00.txt' is not a Gbx file.");
    }

    [Test]
    public async Task Browse_ZipEntry_OpensNestedArchive()
    {
        using var directory = new TempDirectory();
        var inner = Fixtures.CreateZip(("Map.Gbx", Fixtures.CreateGbx()));
        var path = directory.Write("outer.zip", Fixtures.CreateZip(("Inner.zip", inner)));

        var run = await CliRun.RunInteractiveAsync("1\n1\nq\n", path);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("Inner.zip (ZipArchive)");
        await Assert.That(run.Output).Contains("Node (CGameCtnMediaBlockTime)");
    }

    [Test]
    public async Task Browse_EndOfInput_Quits()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("input.Gbx", Fixtures.CreateGbx());

        var run = await CliRun.RunInteractiveAsync(string.Empty, path);

        await Assert.That(run.ExitCode).IsEqualTo(0);
    }

    /// <summary>
    /// Finds the number the browser gives a member, by reading the first page of the node or of an opened member.
    /// </summary>
    private static async Task<int> FindItemAsync(string path, string name, params int[] open)
    {
        var input = string.Concat(open.Select(number => $"{number}\n")) + "q\n";
        var run = await CliRun.RunInteractiveAsync(input, path);
        // Each rendered page starts after an empty line.
        var page = run.Output.Split(Environment.NewLine + Environment.NewLine)[^1];
        var line = page.Split(Environment.NewLine).Select(line => line.Trim()).First(line => line.Contains($"] {name}: "));

        return int.Parse(line[1..line.IndexOf(']')]);
    }
}
