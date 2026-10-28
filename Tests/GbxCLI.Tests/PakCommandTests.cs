using GBX.NET;
using GBX.NET.Engines.MwFoundations;
using System.Text;

namespace GbxCLI.Tests;

public class PakCommandTests
{
    [Test]
    public async Task Files_PrintsAllNestedPathsInOrderWithoutPreviewLimits()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-files-");

        try
        {
            var path = Path.Combine(directory.FullName, "archive.Pack.Gbx");
            var names = Enumerable.Range(0, 15).Select(i => $"file [blue] {i:D2}.txt")
                .Append(new string('z', 300) + ".txt").Reverse().ToArray();
            await File.WriteAllBytesAsync(path, CreatePakData(names));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "files", path], output, error);
            var expected = string.Join(Environment.NewLine, names.Order(StringComparer.Ordinal)
                .Select(name => Path.Combine("Data", "Nested [red]", name))) + Environment.NewLine;

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(output.ToString()).IsEqualTo(expected);
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            using var reopened = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Files_EmptyArchive_PrintsNothing()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-files-");

        try
        {
            var path = Path.Combine(directory.FullName, "empty.pak");
            await File.WriteAllBytesAsync(path, CreatePakData([]));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "files", path], output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(output.ToString()).IsEqualTo(string.Empty);
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments(true, "files")]
    [Arguments(false, "files")]
    [Arguments(true, "verify")]
    [Arguments(false, "verify")]
    [Arguments(true, "extract")]
    [Arguments(false, "extract")]
    public async Task Subcommand_UnavailableIndexOrGbx_ReportsError(bool privatePak, string command)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-files-");

        try
        {
            var path = Path.Combine(directory.FullName, "unknown.pak");

            if (privatePak)
            {
                await File.WriteAllBytesAsync(path, ParseCommandTests.CreatePakData(privateHeader: true));
            }
            else
            {
                new Gbx<CMwNod>(new CMwNod()) { BodyCompression = GbxCompression.Uncompressed }.Save(path);
            }

            using var output = new StringWriter();
            using var error = new StringWriter();
            var exitCode = await Cli.RunAsync(["pak", command, path], output, error);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(output.ToString()).IsEqualTo(string.Empty);
            await Assert.That(error.ToString()).Contains(privatePak ? "requires a matching key" : "requires a PAK");
            using var reopened = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("", "Missing PAK subcommand")]
    [Arguments("files", "Missing input file for pak files")]
    [Arguments("verify", "Missing input file for pak verify")]
    [Arguments("extract", "Missing input file for pak extract")]
    [Arguments("unknown", "Unknown PAK subcommand")]
    public async Task Pak_MissingOrUnknownArguments_ReportsError(string subcommand, string expectedError)
    {
        using var output = new StringWriter();
        using var error = new StringWriter();
        string[] args = subcommand.Length == 0 ? ["pak"] : ["pak", subcommand];

        var exitCode = await Cli.RunAsync(args, output, error);

        await Assert.That(exitCode).IsEqualTo(1);
        await Assert.That(output.ToString()).IsEqualTo(string.Empty);
        await Assert.That(error.ToString()).Contains(expectedError);
    }

    [Test]
    [Arguments("--help", "files")]
    [Arguments("--version", "files")]
    [Arguments("--help", "verify")]
    [Arguments("--version", "verify")]
    [Arguments("--help", "extract")]
    [Arguments("--version", "extract")]
    public async Task Subcommand_GlobalOptions_DoNotRequireAnArchive(string option, string command)
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        var exitCode = await Cli.RunAsync(["pak", command, option], output, error);

        await Assert.That(exitCode).IsEqualTo(0);
        await Assert.That(output.ToString()).Contains(option == "--help" ? $"pak {command} <file>" : "gbx 0.1.0");
        await Assert.That(error.ToString()).IsEqualTo(string.Empty);
    }

    [Test]
    [Arguments("--resolved", "--unresolved")]
    [Arguments("--unresolved", "--resolved")]
    public async Task Files_ConflictingResolutionFilters_ReportError(string first, string second)
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        var exitCode = await Cli.RunAsync(["pak", "files", first, "unused.pak", second], output, error);

        await Assert.That(exitCode).IsEqualTo(1);
        await Assert.That(output.ToString()).IsEqualTo(string.Empty);
        await Assert.That(error.ToString()).Contains("cannot be combined");
    }

    [Test]
    [Arguments("verify", "--resolved")]
    [Arguments("verify", "--unresolved")]
    [Arguments("extract", "--resolved")]
    [Arguments("extract", "--unresolved")]
    public async Task Subcommand_ResolutionFiltersOnlyApplyToFiles_ReportError(string command, string option)
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        var exitCode = await Cli.RunAsync(["pak", command, "unused.pak", option], output, error);

        await Assert.That(exitCode).IsEqualTo(1);
        await Assert.That(output.ToString()).IsEqualTo(string.Empty);
        await Assert.That(error.ToString()).Contains($"does not accept {option}");
    }

    private static byte[] CreatePakData(IReadOnlyList<string> names)
    {
        var original = ParseCommandTests.CreatePakData();
        var singleFileEntrySize = 36 + Encoding.UTF8.GetByteCount("sample [blue].txt");
        var folderCountOffset = original.Length - singleFileEntrySize - 8;
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        writer.Write(original.AsSpan(0, folderCountOffset));
        writer.Write(2);
        writer.Write(-1);
        WriteString(writer, "Data");
        writer.Write(0);
        WriteString(writer, "Nested [red]");
        writer.Write(names.Count);

        foreach (var name in names)
        {
            writer.Write(1);
            WriteString(writer, name);
            writer.Write(new byte[20]); // Unknown field, sizes, offset, and class ID.
            writer.Write(0x4000000000000UL);
        }

        return stream.ToArray();
    }

    [Test]
    [Arguments("", "", "Media/b.txt|a.Gbx|Media/Texture/c.dds")]
    [Arguments("/Media/*", "", "Media/b.txt|Media/Texture/c.dds")]
    [Arguments("/c.dds", "", "Media/Texture/c.dds")]
    [Arguments("", "--resolved", "Media/b.txt|a.Gbx|Media/Texture/c.dds")]
    [Arguments("", "--unresolved", "")]
    public async Task Files_ZipArchive_ListsMatchingFileEntries(string selection, string option, string expectedEntries)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-zip-");

        try
        {
            var path = Path.Combine(directory.FullName, "Extras.zip");
            CreateZip(path, ["a.Gbx", "Media/", "Media/b.txt", "Media/Texture/c.dds"]);
            using var output = new StringWriter();
            using var error = new StringWriter();
            string[] args = option.Length == 0 ? ["pak", "files", path + selection] : ["pak", "files", path + selection, option];

            var exitCode = await Cli.RunAsync(args, output, error);
            var expected = string.Concat(expectedEntries.Split('|', StringSplitOptions.RemoveEmptyEntries)
                .Select(entry => entry.Replace('/', Path.DirectorySeparatorChar))
                .Order(StringComparer.Ordinal)
                .Select(entry => entry + Environment.NewLine));

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(output.ToString()).IsEqualTo(expected);
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Verify_ZipArchive_ParsesGbxEntriesAndReportsFailures()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-zip-");

        try
        {
            var path = Path.Combine(directory.FullName, "Extras.zip");
            CreateZip(path, new Dictionary<string, byte[]>
            {
                ["Valid.Gbx"] = PakEntryTests.CreateGbx(compressed: false),
                ["Hashed"] = PakEntryTests.CreateGbx(compressed: false),
                ["Broken.Gbx"] = "GBX"u8.ToArray(),
                ["Readme.txt"] = "text"u8.ToArray(),
            });
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "verify", path], output, error);
            var lines = output.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(lines.Length).IsEqualTo(2);
            await Assert.That(lines[0]).StartsWith("Broken.Gbx: ");
            await Assert.That(lines[1]).IsEqualTo("Parsed 2 Gbx files; 1 failed; 1 non-Gbx files skipped.");
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Extract_ZipArchive_WritesMatchingEntriesWithoutOverwriting()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-zip-");

        try
        {
            var path = Path.Combine(directory.FullName, "Extras.zip");
            var extracted = Path.Combine(directory.FullName, "out");
            CreateZip(path, new Dictionary<string, byte[]>
            {
                ["Media/"] = [],
                ["Media/a.txt"] = "first"u8.ToArray(),
                ["Media/Texture/b.dds"] = "second"u8.ToArray(),
                ["Other.txt"] = "other"u8.ToArray(),
            });
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "extract", path + "/Media/*", "-o", extracted], output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(await File.ReadAllTextAsync(Path.Combine(extracted, "Media", "a.txt"))).IsEqualTo("first");
            await Assert.That(await File.ReadAllTextAsync(Path.Combine(extracted, "Media", "Texture", "b.dds"))).IsEqualTo("second");
            await Assert.That(File.Exists(Path.Combine(extracted, "Other.txt"))).IsFalse();
            await Assert.That(output.ToString()).Contains($"Extracted 2 files to {extracted}.");

            using var secondOutput = new StringWriter();
            using var secondError = new StringWriter();
            var secondExitCode = await Cli.RunAsync(["pak", "extract", path + "/Media/*", "-o", extracted], secondOutput, secondError);

            await Assert.That(secondExitCode).IsEqualTo(1);
            await Assert.That(secondError.ToString()).Contains("already exists");
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Extract_ZipArchive_RejectsEntriesOutsideOutputDirectory()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-zip-");

        try
        {
            var path = Path.Combine(directory.FullName, "Evil.zip");
            CreateZip(path, new Dictionary<string, byte[]> { ["../escape.txt"] = "x"u8.ToArray() });
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "extract", path, "-o", Path.Combine(directory.FullName, "out")], output, error);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(error.ToString()).Contains("The ZIP entry '..");
            await Assert.That(File.Exists(Path.Combine(directory.FullName, "escape.txt"))).IsFalse();
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    private static void CreateZip(string path, string[] entries)
    {
        CreateZip(path, entries.ToDictionary(entry => entry, _ => Array.Empty<byte>()));
    }

    private static void CreateZip(string path, IReadOnlyDictionary<string, byte[]> entries)
    {
        using var archive = System.IO.Compression.ZipFile.Open(path, System.IO.Compression.ZipArchiveMode.Create);

        foreach (var (name, data) in entries)
        {
            using var stream = archive.CreateEntry(name).Open();
            stream.Write(data);
        }
    }

    private static void WriteString(BinaryWriter writer, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        writer.Write(bytes.Length);
        writer.Write(bytes);
    }
}
