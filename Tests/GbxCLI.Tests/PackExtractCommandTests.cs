using GBX.NET;

namespace GbxCLI.Tests;

public class PackExtractCommandTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Extract_WholePak_PreservesFoldersAndExactContents(bool compressed)
    {
        using var directory = new TempDirectory();
        var path = directory.Write("archive.Pack.Gbx", Fixtures.CreatePak(compressed,
            ("Media/Nested/empty.bin", []),
            ("Media/Nested/large [blue].bin", [.. Enumerable.Range(0, 200003).Select(i => (byte)i)]),
            ("Media/unsupported.gbx", "GBX invalid body, extracted without parsing"u8.ToArray()),
            ("readme.txt", "Last entry, without the padding after it"u8.ToArray())));
        var output = directory.Combine("out $dir");

        var run = await CliRun.RunAsync("pack", "extract", path, "-o", output);

        await Assert.That(run.Error).IsEqualTo(string.Empty);
        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(Directory.GetFiles(output, "*", SearchOption.AllDirectories).Length).IsEqualTo(4);
        await Assert.That(await File.ReadAllBytesAsync(Path.Combine(output, "Media", "Nested", "large [blue].bin"))).IsEquivalentTo(Enumerable.Range(0, 200003).Select(i => (byte)i));
        await Assert.That(await File.ReadAllTextAsync(Path.Combine(output, "Media", "unsupported.gbx"))).IsEqualTo("GBX invalid body, extracted without parsing");
        await Assert.That(await File.ReadAllTextAsync(Path.Combine(output, "readme.txt"))).IsEqualTo("Last entry, without the padding after it");
        await Assert.That(run.Output).Contains($"Extracted 4 files to {output}.");
    }

    [Test]
    public async Task Extract_PakGbx_IsSavedAgainAsValidGbx()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("archive.pak", Fixtures.CreatePak(false, ("Media/Time.Gbx", Fixtures.CreateGbx())));
        var output = directory.Combine("out");

        var run = await CliRun.RunAsync("pack", "extract", path, "-o", output);
        var gbx = Gbx.Parse(Path.Combine(output, "Media", "Time.Gbx"));

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(gbx.Header.ClassId).IsEqualTo(0x03085000u);
        await Assert.That(gbx.Node).IsNotNull();
    }

    [Test]
    [Arguments("media/nested/one [BLUE].txt", "Media/Nested/One [blue].txt")]
    [Arguments("MEDIA\\NESTED\\ONE [blue].TXT", "Media/Nested/One [blue].txt")]
    [Arguments("Media/*.txt", "Media/Nested/One [blue].txt|Media/Two.txt")]
    [Arguments("Media/Tw?.txt", "Media/Two.txt")]
    [Arguments("*.bin", "Media/Other.bin")]
    public async Task Extract_Pattern_ExtractsOnlyMatches(string pattern, string expectedNames)
    {
        using var directory = new TempDirectory();
        var path = directory.Write("archive.pak", Fixtures.CreatePak(false,
            ("Media/Nested/One [blue].txt", "one"u8.ToArray()),
            ("Media/Two.txt", "two"u8.ToArray()),
            ("Media/Other.bin", [1, 2, 3]),
            ("readme.txt", "unselected"u8.ToArray())));
        var output = directory.Combine("out");

        var run = await CliRun.RunAsync("pack", "extract", "--output", output, path + "/" + pattern);
        var expected = expectedNames.Split('|');

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(Directory.GetFiles(output, "*", SearchOption.AllDirectories).Length).IsEqualTo(expected.Length);

        foreach (var name in expected)
        {
            await Assert.That(File.Exists(Path.Combine(output, name))).IsTrue();
        }
    }

    [Test]
    [Arguments(6)]
    [Arguments(18)]
    public async Task Extract_EncryptedEntry_UsesBuiltInKeyAndExcludesPadding(int version)
    {
        using var directory = new TempDirectory();
        var contents = "Unaligned payload"u8.ToArray();
        var path = directory.Write("TMNext.pak", Fixtures.CreateEncryptedPak(Fixtures.TMNextKey, false, version, contents, "Data\\sample.txt"));
        var output = directory.Combine("out");

        var run = await CliRun.RunAsync("pack", "extract", path + "/data/*.TXT", "-o", output);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(await File.ReadAllBytesAsync(Path.Combine(output, "Data", "sample.txt"))).IsEquivalentTo(contents);
    }

    [Test]
    public async Task Extract_Zip_ExtractsNestedArchiveEntries()
    {
        using var directory = new TempDirectory();
        var inner = Fixtures.CreateZip(("Folder/a.txt", "a"u8.ToArray()), ("b.txt", "b"u8.ToArray()));
        var path = directory.Write("outer.zip", Fixtures.CreateZip(("Inner.zip", inner)));
        var output = directory.Combine("out");

        var run = await CliRun.RunAsync("pack", "extract", path + "/Inner.zip/Folder/*", "-o", output);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(await File.ReadAllTextAsync(Path.Combine(output, "Folder", "a.txt"))).IsEqualTo("a");
        await Assert.That(File.Exists(Path.Combine(output, "b.txt"))).IsFalse();
    }

    [Test]
    [Arguments("missing.txt")]
    [Arguments("Data/*.gbx")]
    public async Task Extract_NoMatches_FailsWithoutCreatingOutput(string pattern)
    {
        using var directory = new TempDirectory();
        var path = directory.Write("archive.pak", Fixtures.CreatePak(false, ("readme.txt", "content"u8.ToArray())));
        var output = directory.Combine("out");

        var run = await CliRun.RunAsync("pack", "extract", path + "/" + pattern, "-o", output);

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("No files matching");
        await Assert.That(Directory.Exists(output)).IsFalse();
    }

    [Test]
    public async Task Extract_ExistingDestination_WritesNothing()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("archive.pak", Fixtures.CreatePak(false, ("first.txt", "first"u8.ToArray()), ("last.txt", "replacement"u8.ToArray())));
        var output = directory.Combine("out");
        var existing = directory.Write("out/last.txt", "original"u8.ToArray());

        var run = await CliRun.RunAsync("pack", "extract", path, "-o", output);

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("already exists");
        await Assert.That(await File.ReadAllTextAsync(existing)).IsEqualTo("original");
        await Assert.That(File.Exists(Path.Combine(output, "first.txt"))).IsFalse();
    }

    [Test]
    public async Task Extract_TwoEntriesToSameFile_WritesNothing()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("archive.pak", Fixtures.CreatePak(false, ("a/b.txt", [1]), ("a\\b.txt", [2])));
        var output = directory.Combine("out");

        var run = await CliRun.RunAsync("pack", "extract", path, "-o", output);

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("would both be extracted");
        await Assert.That(Directory.Exists(output)).IsFalse();
    }

    [Test]
    [Arguments("../escaped.txt")]
    [Arguments("Nested\\..\\..\\escaped.txt")]
    [Arguments("/escaped.txt")]
    public async Task Extract_UnsafePakEntry_IsRejected(string name)
    {
        using var directory = new TempDirectory();
        var path = directory.Write("archive.pak", Fixtures.CreatePak(false, ("safe.txt", [1]), (name, "content"u8.ToArray())));
        var output = directory.Combine("out");

        var run = await CliRun.RunAsync("pack", "extract", path, "-o", output);

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("not a safe relative file path");
        await Assert.That(Directory.Exists(output)).IsFalse();
        await Assert.That(File.Exists(directory.Combine("escaped.txt"))).IsFalse();
    }

    [Test]
    public async Task Extract_UnsafeZipEntry_IsRejectedAsZip()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("archive.zip", Fixtures.CreateZip(("../escape.txt", "content"u8.ToArray())));
        var output = directory.Combine("out");

        var run = await CliRun.RunAsync("pack", "extract", path, "-o", output);

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("not a safe relative file path");
        await Assert.That(run.Error.Contains("requires a PAK")).IsFalse();
        await Assert.That(File.Exists(directory.Combine("escape.txt"))).IsFalse();
    }

    [Test]
    public async Task Extract_EmptyArchive_CreatesOutputFolder()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("empty.pak", Fixtures.CreatePak(false));
        var output = directory.Combine("out");

        var run = await CliRun.RunAsync("pack", "extract", path, "-o", output);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output).Contains("Extracted 0 files");
        await Assert.That(Directory.Exists(output)).IsTrue();
    }

    [Test]
    public async Task Extract_TitlePackWithoutOutput_UsesTitleIdFolderInWorkingDirectory()
    {
        using var directory = new TempDirectory();
        var titleId = $"gbx-cli-{Guid.NewGuid():N}@nadeo";
        var contents = "Title pack entry"u8.ToArray();
        // The filename finds the key, because the title ID is not a known one.
        var path = directory.Write("TMNext.pak", Fixtures.CreateEncryptedPak(Fixtures.TMNextKey, false, 18, contents, "Data\\sample.txt", titleId));
        var extracted = Path.GetFullPath(titleId);

        try
        {
            var run = await CliRun.RunAsync("pack", "extract", path);

            await Assert.That(run.Error).IsEqualTo(string.Empty);
            await Assert.That(run.ExitCode).IsEqualTo(0);
            await Assert.That(await File.ReadAllBytesAsync(Path.Combine(extracted, "Data", "sample.txt"))).IsEquivalentTo(contents);
        }
        finally
        {
            if (Directory.Exists(extracted))
            {
                Directory.Delete(extracted, recursive: true);
            }
        }
    }

    [Test]
    public async Task Extract_OutputIsFile_FailsWithoutWriting()
    {
        using var directory = new TempDirectory();
        var path = directory.Write("archive.pak", Fixtures.CreatePak(false, ("readme.txt", "content"u8.ToArray())));

        var run = await CliRun.RunAsync("pack", "extract", path, "-o", path);

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("because it is a file. Choose a folder with -o.");
        await Assert.That(Directory.GetFileSystemEntries(directory.Path)).IsEquivalentTo([path]);
    }

    [Test]
    public async Task Extract_WithoutOutputOrTitleId_UsesNameWithoutWholeExtension()
    {
        using var directory = new TempDirectory();
        var name = $"gbx-cli-{Guid.NewGuid():N}";
        var path = directory.Write(name + ".Pack.Gbx", Fixtures.CreatePak(false, ("readme.txt", "content"u8.ToArray())));
        var extracted = Path.GetFullPath(name);

        try
        {
            var run = await CliRun.RunAsync("pack", "extract", path);

            await Assert.That(run.ExitCode).IsEqualTo(0);
            await Assert.That(await File.ReadAllTextAsync(Path.Combine(extracted, "readme.txt"))).IsEqualTo("content");
        }
        finally
        {
            if (Directory.Exists(extracted))
            {
                Directory.Delete(extracted, recursive: true);
            }
        }
    }
}
