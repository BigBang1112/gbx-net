using System.IO.Compression;
using System.Text;

namespace GbxCLI.Tests;

public class PakExtractionTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Extract_WholeArchive_PreservesFoldersAndExactFileContents(bool compressed)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-extract-");

        try
        {
            var path = Path.Combine(directory.FullName, "archive.Pack.Gbx");
            (string Name, byte[] Contents)[] files =
            [
                ("Media/Nested/empty.bin", []),
                ("Media/Nested/large [blue].bin", Enumerable.Range(0, 200003).Select(i => (byte)i).ToArray()),
                ("Media/unsupported.gbx", "GBX invalid body, extracted without parsing"u8.ToArray()),
                ("readme.txt", "Last entry, without data from earlier entries"u8.ToArray())
            ];
            await File.WriteAllBytesAsync(path, CreatePak(compressed, files));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "extract", path], output, error);
            var extracted = path + ".extracted";

            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(Directory.GetFiles(extracted, "*", SearchOption.AllDirectories).Length).IsEqualTo(files.Length);

            foreach (var (name, contents) in files)
            {
                await Assert.That(Convert.ToHexString(await File.ReadAllBytesAsync(Path.Combine(extracted, name))))
                    .IsEqualTo(Convert.ToHexString(contents));
            }

            await Assert.That(output.ToString()).Contains($"Extracted 4 files to {extracted}.");
            using var reopened = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("media/nested/one [BLUE].txt", "Media/Nested/One [blue].txt", "-o")]
    [Arguments("MEDIA\\NESTED\\ONE [blue].TXT", "Media/Nested/One [blue].txt", "--output")]
    [Arguments("Media/*.txt", "Media/Nested/One [blue].txt|Media/Two.txt", "-o")]
    [Arguments("Media\\*.TXT", "Media/Nested/One [blue].txt|Media/Two.txt", "--output")]
    [Arguments("Media/Tw?.txt", "Media/Two.txt", "-o")]
    [Arguments("*.bin", "Media/Other.bin", "-o")]
    public async Task Extract_EntryOrWildcard_ExtractsOnlyMatches(string pattern, string expectedNames, string outputOption)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-extract-");

        try
        {
            var path = Path.Combine(directory.FullName, "archive.pak");
            var extracted = Path.Combine(directory.FullName, "output [red]");
            await File.WriteAllBytesAsync(path, CreatePak(false,
                ("Media/Nested/One [blue].txt", "one"u8.ToArray()),
                ("Media/Two.txt", "two"u8.ToArray()),
                ("Media/Other.bin", [1, 2, 3]),
                ("readme.txt", "unselected"u8.ToArray())));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "extract", outputOption, extracted, path + "/" + pattern], output, error);
            var expected = expectedNames.Split('|');

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            await Assert.That(Directory.GetFiles(extracted, "*", SearchOption.AllDirectories).Length).IsEqualTo(expected.Length);

            foreach (var name in expected)
            {
                await Assert.That(File.Exists(Path.Combine(extracted, name))).IsTrue();
            }

            await Assert.That(Directory.Exists(path + ".extracted")).IsFalse();
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments(6)]
    [Arguments(18)]
    public async Task Extract_EncryptedEntry_UsesBuiltInKeyAndExcludesPadding(int version)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-extract-");

        try
        {
            var path = Path.Combine(directory.FullName, "TMNext.pak");
            var contents = "Unaligned payload"u8.ToArray();
            await File.WriteAllBytesAsync(path, PakLoaderTests.CreateEncryptedPak(
                "5F4FC43233B989DAED3918C1190F4A5A", false, version, contents, "Data\\sample.txt"));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "extract", path + "/data/*.TXT"], output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            await Assert.That(Convert.ToHexString(await File.ReadAllBytesAsync(Path.Combine(path + ".extracted", "Data", "sample.txt"))))
                .IsEqualTo(Convert.ToHexString(contents));
            using var reopened = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("missing.txt")]
    [Arguments("Data/*.gbx")]
    public async Task Extract_NoMatches_ReportsErrorWithoutCreatingOutput(string pattern)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-extract-");

        try
        {
            var path = Path.Combine(directory.FullName, "archive.pak");
            await File.WriteAllBytesAsync(path, CreatePak(false, ("readme.txt", "content"u8.ToArray())));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "extract", path + "/" + pattern], output, error);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(error.ToString()).Contains("No files matching");
            await Assert.That(Directory.Exists(path + ".extracted")).IsFalse();
            using var reopened = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Extract_ExistingDestination_LeavesFilesIntact()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-extract-");

        try
        {
            var path = Path.Combine(directory.FullName, "archive.pak");
            await File.WriteAllBytesAsync(path, CreatePak(false,
                ("first.txt", "first"u8.ToArray()), ("last.txt", "replacement"u8.ToArray())));
            var extracted = Directory.CreateDirectory(path + ".extracted").FullName;
            var existing = Path.Combine(extracted, "last.txt");
            await File.WriteAllTextAsync(existing, "original");
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "extract", path], output, error);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(error.ToString()).Contains("already exists");
            await Assert.That(await File.ReadAllTextAsync(existing)).IsEqualTo("original");
            await Assert.That(File.Exists(Path.Combine(extracted, "first.txt"))).IsFalse();
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("../escaped.txt")]
    [Arguments("Nested\\..\\..\\escaped.txt")]
    [Arguments("/escaped.txt")]
    public async Task Extract_UnsafeEntry_RejectsPathsOutsideOutput(string name)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-extract-");

        try
        {
            var path = Path.Combine(directory.FullName, "archive.pak");
            await File.WriteAllBytesAsync(path, CreatePak(false, (name, "content"u8.ToArray())));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "extract", path], output, error);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(error.ToString()).Contains("not a safe relative file path");
            await Assert.That(Directory.Exists(path + ".extracted")).IsFalse();
            await Assert.That(File.Exists(Path.Combine(directory.FullName, "escaped.txt"))).IsFalse();
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Extract_EmptyArchive_CreatesOutputDirectory()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-extract-");

        try
        {
            var path = Path.Combine(directory.FullName, "empty.pak");
            await File.WriteAllBytesAsync(path, CreatePak(false));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "extract", path], output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(output.ToString()).Contains("Extracted 0 files");
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            await Assert.That(Directory.Exists(path + ".extracted")).IsTrue();
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    internal static byte[] CreatePak(bool compressed, params (string Name, byte[] Contents)[] files)
    {
        var original = ParseCommandTests.CreatePakData();
        var fileCountOffset = original.Length - (36 + Encoding.UTF8.GetByteCount("sample [blue].txt")) - 4;
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        writer.Write(original.AsSpan(0, fileCountOffset));
        stream.Position = 68; // Version 6 data start.
        writer.Write(4096);
        stream.Position = fileCountOffset;
        writer.Write(files.Length);
        var offset = 0U;
        var payloads = new List<byte[]>();

        foreach (var (name, contents) in files)
        {
            using var payload = new MemoryStream();

            if (compressed)
            {
                using var compressor = new ZLibStream(payload, CompressionLevel.Fastest, leaveOpen: true);
                compressor.Write(contents);
            }
            else
            {
                payload.Write(contents);
            }

            var bytes = payload.ToArray();
            payloads.Add(bytes);
            writer.Write(-1);
            var nameBytes = Encoding.UTF8.GetBytes(name);
            writer.Write(nameBytes.Length);
            writer.Write(nameBytes);
            writer.Write(0U);
            writer.Write(contents.Length);
            writer.Write(bytes.Length);
            writer.Write(offset);
            writer.Write(0U);
            writer.Write(0x4000000000000UL | (compressed ? 4UL : 0UL));
            offset += (uint)bytes.Length;
        }

        stream.SetLength(4096);
        stream.Position = 4096;

        foreach (var payload in payloads)
        {
            writer.Write(payload);
        }

        return stream.ToArray();
    }
}
