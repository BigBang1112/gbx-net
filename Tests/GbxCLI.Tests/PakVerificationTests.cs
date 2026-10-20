using GBX.NET;
using GBX.NET.Engines.Game;
using System.Buffers.Binary;
using System.Text;

namespace GbxCLI.Tests;

public class PakVerificationTests
{
    [Test]
    public async Task Verify_ReportsHeaderAndBodyExceptionsAndContinuesThroughArchive()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-verify-");

        try
        {
            var path = Path.Combine(directory.FullName, "archive.pak");
            var invalidBody = CreateGbx(compressed: false);
            BinaryPrimitives.WriteUInt32LittleEndian(invalidBody.AsSpan(invalidBody.Length - 4), 0x03085FFF);
            using (var header = new MemoryStream(invalidBody))
            {
                _ = Gbx.ParseHeader(header);
            }

            await File.WriteAllBytesAsync(path, CreatePak(
                ("01-body [red].gbx", invalidBody),
                ("02-hashed-name", "GBX"u8.ToArray()),
                ("03-invalid.GBX", "Invalid Gbx header"u8.ToArray()),
                ("04-valid.gbx", CreateGbx(compressed: false)),
                ("05-renamed.dat", CreateGbx(compressed: true)),
                ("06-readme.txt", "Plain text document"u8.ToArray()),
                ("07-short.bin", new byte[] { 1 }),
                ("08-empty.bin", [])));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "verify", path], output, error);
            var lines = output.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(lines.Length).IsEqualTo(4);
            await Assert.That(lines[0].StartsWith("01-body [red].gbx: ", StringComparison.Ordinal)).IsTrue();
            await Assert.That(lines[0]).Contains("ChunkReadException");
            await Assert.That(lines[1].StartsWith("02-hashed-name: ", StringComparison.Ordinal)).IsTrue();
            await Assert.That(lines[2].StartsWith("03-invalid.GBX: ", StringComparison.Ordinal)).IsTrue();
            await Assert.That(lines[3]).IsEqualTo("Parsed 2 Gbx files; 3 failed; 3 non-Gbx files skipped.");
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            using var reopened = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments(6)]
    [Arguments(18)]
    public async Task Verify_EncryptedArchive_UsesBuiltInKeyAndParsesBody(int version)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-verify-");

        try
        {
            var path = Path.Combine(directory.FullName, "TMNext.pak");
            await File.WriteAllBytesAsync(path, PakLoaderTests.CreateEncryptedPak(
                "5F4FC43233B989DAED3918C1190F4A5A", false, version, CreateGbx(compressed: true), "hashed-name"));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "verify", path], output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(output.ToString()).IsEqualTo($"Parsed 1 Gbx files; 0 failed; 0 non-Gbx files skipped.{Environment.NewLine}");
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Verify_EmptyArchive_SucceedsWithZeroCounts()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-verify-");

        try
        {
            var path = Path.Combine(directory.FullName, "empty.Pack.Gbx");
            await File.WriteAllBytesAsync(path, CreatePak());
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "verify", path], output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(output.ToString()).IsEqualTo($"Parsed 0 Gbx files; 0 failed; 0 non-Gbx files skipped.{Environment.NewLine}");
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Verify_UnknownRootClass_ReportsFailureAndContinues()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-verify-");

        try
        {
            var path = Path.Combine(directory.FullName, "archive.pak");
            var unknown = CreateGbx(compressed: false);
            BinaryPrimitives.WriteUInt32LittleEndian(unknown.AsSpan(9), 0x7FFF0000);
            await File.WriteAllBytesAsync(path, CreatePak(
                ("unknown.gbx", unknown),
                ("valid.gbx", CreateGbx(compressed: false))));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "verify", path], output, error);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(output.ToString()).Contains("Root class 0x7FFF0000 is not supported");
            await Assert.That(output.ToString()).Contains("Parsed 1 Gbx files; 1 failed; 0 non-Gbx files skipped.");
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Verify_Directory_VerifiesTopLevelArchivesAndReportsTotals()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-verify-");

        try
        {
            var path = directory.FullName;
            await File.WriteAllBytesAsync(Path.Combine(path, "a.pak"), CreatePak(
                ("broken.gbx", "GBX"u8.ToArray()),
                ("valid.gbx", CreateGbx(compressed: false))));
            await File.WriteAllBytesAsync(Path.Combine(path, "b.Pack.Gbx"), CreatePak(("valid.gbx", CreateGbx(compressed: true))));
            await File.WriteAllBytesAsync(Path.Combine(path, "c.zip"), CreateZip(("Valid.Gbx", CreateGbx(compressed: false)), ("Readme.txt", "text"u8.ToArray())));
            await File.WriteAllBytesAsync(Path.Combine(path, "d.pak"), ParseCommandTests.CreatePakData(privateHeader: true));
            await File.WriteAllTextAsync(Path.Combine(path, "notes.txt"), "Not an archive");
            Directory.CreateDirectory(Path.Combine(path, "Sub"));
            await File.WriteAllBytesAsync(Path.Combine(path, "Sub", "nested.pak"), CreatePak(("broken.gbx", "GBX"u8.ToArray())));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "verify", path], output, error);
            var lines = output.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(lines.Length).IsEqualTo(6);
            await Assert.That(lines[0]).StartsWith($"a.pak{Path.DirectorySeparatorChar}broken.gbx: ");
            await Assert.That(lines[1]).IsEqualTo("a.pak: Parsed 1 Gbx files; 1 failed; 0 non-Gbx files skipped.");
            await Assert.That(lines[2]).IsEqualTo("b.Pack.Gbx: Parsed 1 Gbx files; 0 failed; 0 non-Gbx files skipped.");
            await Assert.That(lines[3]).IsEqualTo("c.zip: Parsed 1 Gbx files; 0 failed; 1 non-Gbx files skipped.");
            await Assert.That(lines[4]).StartsWith("d.pak: InvalidDataException: ");
            await Assert.That(lines[5]).IsEqualTo("Verified 4 archives (1 could not be opened). Parsed 3 Gbx files; 1 failed; 1 non-Gbx files skipped.");
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Verify_DirectoryWithoutArchives_Fails()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-verify-");

        try
        {
            await File.WriteAllTextAsync(Path.Combine(directory.FullName, "notes.txt"), "Not an archive");
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "verify", directory.FullName], output, error);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(output.ToString()).IsEqualTo(string.Empty);
            await Assert.That(error.ToString()).Contains("No PAK or ZIP archives were found");
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Verify_ZipContainingPack_VerifiesNestedBodiesAndReportsTheirPaths()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-nested-pak-verify-");

        try
        {
            var path = Path.Combine(directory.FullName, "skin.zip");
            await File.WriteAllBytesAsync(path, CreateZip(("Model.Pack.Gbx", CreatePak(
                ("good.gbx", CreateGbx(compressed: true)),
                ("bad.gbx", "GBX"u8.ToArray()),
                ("texture.dds", new byte[] { 1, 2 })))));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "verify", path], output, error);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(output.ToString()).Contains($"Model.Pack.Gbx{Path.DirectorySeparatorChar}bad.gbx: ");
            await Assert.That(output.ToString()).Contains("Parsed 1 Gbx files; 1 failed; 1 non-Gbx files skipped.");
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            using var reopened = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    private static byte[] CreateZip(params (string Name, byte[] Contents)[] files)
    {
        using var stream = new MemoryStream();

        using (var zip = new System.IO.Compression.ZipArchive(stream, System.IO.Compression.ZipArchiveMode.Create, leaveOpen: true))
        {
            foreach (var (name, contents) in files)
            {
                using var entry = zip.CreateEntry(name).Open();
                entry.Write(contents);
            }
        }

        return stream.ToArray();
    }

    private static byte[] CreateGbx(bool compressed)
    {
        Gbx.LZO = new GBX.NET.LZO.Lzo();
        var node = new CGameCtnMediaBlockTime
        {
            Keys = [new() { Time = new(1.5f), TimeValue = 42.5f, Tangent = 0.25f }]
        };
        node.CreateChunk<CGameCtnMediaBlockTime.Chunk03085000>();
        using var stream = new MemoryStream();
        new Gbx<CGameCtnMediaBlockTime>(node)
        {
            BodyCompression = compressed ? GbxCompression.Compressed : GbxCompression.Uncompressed
        }.Save(stream);
        return stream.ToArray();
    }

    private static byte[] CreatePak(params (string Name, byte[] Contents)[] files)
    {
        var original = ParseCommandTests.CreatePakData();
        var singleFileEntrySize = 36 + Encoding.UTF8.GetByteCount("sample [blue].txt");
        var fileCountOffset = original.Length - singleFileEntrySize - 4;
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        writer.Write(original.AsSpan(0, fileCountOffset));
        stream.Position = 68; // Version 6 stores the data start after the header MD5 and Gbx headers offset.
        writer.Write(4096);
        stream.Position = fileCountOffset;
        writer.Write(files.Length);
        var offset = 0U;

        foreach (var (name, contents) in files)
        {
            writer.Write(-1);
            var nameBytes = Encoding.UTF8.GetBytes(name);
            writer.Write(nameBytes.Length);
            writer.Write(nameBytes);
            writer.Write(0U);
            writer.Write(contents.Length);
            writer.Write(contents.Length);
            writer.Write(offset);
            writer.Write(0x03085000U);
            writer.Write(0x4000000000000UL);
            offset += (uint)contents.Length;
        }

        stream.SetLength(4096);
        stream.Position = 4096;

        foreach (var (_, contents) in files)
        {
            writer.Write(contents);
        }

        return stream.ToArray();
    }
}
