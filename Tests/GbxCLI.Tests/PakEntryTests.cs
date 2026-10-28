using GBX.NET;
using GBX.NET.Engines.Game;
using System.Text;

namespace GbxCLI.Tests;

public class PakEntryTests
{
    private const string BaseKey = "5F4FC43233B989DAED3918C1190F4A5A";
    private const string EntryPath = "Media\\Texture\\Time [blue].Gbx";

    [Test]
    [Arguments("parse", "/", false, 6)]
    [Arguments("parse", "\\", true, 18)]
    [Arguments("eval", "/", false, 18)]
    [Arguments("parse", "/", true, 18)]
    public async Task Command_EncryptedPakEntry_ParsesFullGbx(string command, string separator, bool compressed, int version)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-entry-");

        try
        {
            var archivePath = Path.Combine(directory.FullName, "TMNext.pak");
            var contents = CreateGbx(compressed);
            await File.WriteAllBytesAsync(archivePath, PakLoaderTests.CreateEncryptedPak(BaseKey, false, version, contents, EntryPath));
            var inputPath = archivePath + separator + EntryPath.Replace("\\", separator).ToLowerInvariant();
            using var output = new StringWriter();
            using var error = new StringWriter();
            string[] args = command == "eval" ? [command, inputPath, "Node.Keys[0].TimeValue"] : [command, inputPath];

            var exitCode = await Cli.RunAsync(args, output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);

            if (command == "parse")
            {
                await Assert.That(output.ToString()).Contains($"File: {inputPath}");
                await Assert.That(output.ToString()).Contains($"File size: {contents.Length:N0} bytes");
                await Assert.That(output.ToString()).Contains("Parse status: Complete");
                await Assert.That(output.ToString()).Contains("TimeValue: 42.5");
            }
            else
            {
                await Assert.That(output.ToString()).Contains("Result: 42.5");
            }

            using var reopened = new FileStream(archivePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Open_PackGbxEntry_CanReadOnlyItsHeader()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-entry-");

        try
        {
            var archivePath = Path.Combine(directory.FullName, "TMTurbo.Title.Pack.Gbx");
            await File.WriteAllBytesAsync(archivePath, PakLoaderTests.CreateEncryptedPak(
                "5038519A8AD9270786D2097400055A5D", false, 18, CreateGbx(false), EntryPath));

            await using (var file = await ParsedFile.OpenAsync(archivePath + "/" + EntryPath, fullGbx: false))
            {
                var gbx = (Gbx<CGameCtnMediaBlockTime>)file.Value;
                await Assert.That(gbx.Node.Chunks.Count).IsEqualTo(0);
            }

            using var reopened = new FileStream(archivePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("missing", "was not found in PAK")]
    [Arguments("private", "requires a matching key")]
    [Arguments("not-pak", "must point to a PAK")]
    [Arguments("not-gbx", "gbx:")]
    public async Task Parse_InvalidArchiveEntry_ReportsErrorAndReleasesArchive(string failure, string expectedError)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-entry-");

        try
        {
            var archivePath = Path.Combine(directory.FullName, failure == "private" ? "Unknown.pak" : "TMNext.pak");
            var contents = failure == "not-gbx" ? Encoding.ASCII.GetBytes("This is not a Gbx") : CreateGbx(false);
            var archive = failure == "not-pak" ? contents : PakLoaderTests.CreateEncryptedPak(BaseKey, false, 18, contents, EntryPath);
            await File.WriteAllBytesAsync(archivePath, archive);
            var inputPath = archivePath + "/" + (failure == "missing" ? "Missing.Gbx" : EntryPath);
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["parse", inputPath], output, error);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(output.ToString()).IsEqualTo(string.Empty);
            await Assert.That(error.ToString()).Contains(expectedError);
            using var reopened = new FileStream(archivePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Parse_RealFileUnderDirectoryNamedPak_UsesFilesystemFile()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-entry-");

        try
        {
            var subdirectory = Directory.CreateDirectory(Path.Combine(directory.FullName, "folder.pak"));
            var path = Path.Combine(subdirectory.FullName, "Time.Gbx");
            await File.WriteAllBytesAsync(path, CreateGbx(false));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["parse", path], output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(output.ToString()).Contains("TimeValue: 42.5");
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    internal static byte[] CreateGbx(bool compressed)
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
}
