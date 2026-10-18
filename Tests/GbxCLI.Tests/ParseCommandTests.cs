using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.MwFoundations;
using System.Text;

namespace GbxCLI.Tests;

public class ParseCommandTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Parse_PrintsFullBodyNodeAndChunkDetails(bool compressed)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-summary-");

        try
        {
            var path = Path.Combine(directory.FullName, "body [red].Gbx");
            var node = new CGameCtnMediaBlockTime
            {
                Keys = [new() { Time = new(1.5f), TimeValue = 42.5f, Tangent = 0.25f }]
            };
            node.CreateChunk<CGameCtnMediaBlockTime.Chunk03085000>();
            Gbx.LZO = new GBX.NET.LZO.Lzo();
            new Gbx<CGameCtnMediaBlockTime>(node)
            {
                BodyCompression = compressed ? GbxCompression.Compressed : GbxCompression.Uncompressed
            }.Save(path);

            using var output = new StringWriter();
            using var error = new StringWriter();
            var exitCode = await Cli.RunAsync(["parse", path], output, error);
            var text = output.ToString();

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(text).Contains($"File: {path}");
            await Assert.That(text).Contains("Class ID: 0x03085000");
            await Assert.That(text).Contains("Parse status: Complete");
            await Assert.That(text).Contains("Node: CGameCtnMediaBlockTime");
            await Assert.That(text).Contains("0x03085000 (Chunk03085000)");
            await Assert.That(text).Contains("TimeValue: 42.5");
            await Assert.That(text).Contains("Tangent: 0.25");
            await Assert.That(text.Contains('\u001b')).IsFalse();
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("archive [green].pak")]
    [Arguments("renamed.Gbx")]
    [Arguments("no-extension")]
    public async Task Parse_DetectsPakBySignature(string fileName)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-");

        try
        {
            var inputPath = Path.Combine(directory.FullName, fileName);
            await File.WriteAllBytesAsync(inputPath, CreatePakData());
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["parse", inputPath], output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(output.ToString()).Contains($"File: {inputPath}");
            await Assert.That(output.ToString()).Contains("Format: NadeoPak");
            await Assert.That(output.ToString()).Contains("Version: 6");
            await Assert.That(output.ToString()).Contains("Header encryption: Unencrypted");
            await Assert.That(output.ToString()).Contains("Files: 1");
            await Assert.That(output.ToString().Contains('\u001b')).IsFalse();
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);

            using var reopened = new FileStream(inputPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Parse_GbxWithPakExtensionStillUsesGbxParser()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-");

        try
        {
            var inputPath = Path.Combine(directory.FullName, "renamed.pak");
            new Gbx<CMwNod>(new CMwNod()) { BodyCompression = GbxCompression.Uncompressed }.Save(inputPath);
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["parse", inputPath], output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(output.ToString()).Contains("Header: GbxHeader");
            await Assert.That(output.ToString()).Contains("Body compression: Uncompressed");
            await Assert.That(output.ToString().Contains("NadeoPak")).IsFalse();
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Parse_PrivatePakHeaderReportsUnavailableFileIndex()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-");

        try
        {
            var inputPath = Path.Combine(directory.FullName, "private.pak");
            await File.WriteAllBytesAsync(inputPath, CreatePakData(privateHeader: true));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["parse", inputPath], output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(output.ToString()).Contains("Format: NadeoPak");
            await Assert.That(output.ToString()).Contains("Header encryption: Encrypted");
            await Assert.That(output.ToString()).Contains("Files: Unavailable (private header requires a key).");
            await Assert.That(output.ToString().Contains("Files: 0")).IsFalse();
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("")]
    [Arguments("GB")]
    [Arguments("NadeoPa")]
    [Arguments("Not a supported file")]
    public async Task Parse_UnsupportedOrIncompleteSignatureReportsFormatError(string contents)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-");

        try
        {
            var inputPath = Path.Combine(directory.FullName, "invalid.pak");
            await File.WriteAllTextAsync(inputPath, contents);
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["parse", inputPath], output, error);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(output.ToString()).IsEqualTo(string.Empty);
            await Assert.That(error.ToString()).Contains("Unsupported file format");
            await Assert.That(error.ToString()).Contains("Gbx (GBX) or PAK (NadeoPak)");
            await Assert.That(error.ToString().Contains("Use --help")).IsFalse();

            using var reopened = new FileStream(inputPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("GBX")]
    [Arguments("NadeoPak")]
    public async Task Parse_TruncatedKnownFormatReportsParserErrorAndReleasesFile(string signature)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-");

        try
        {
            var inputPath = Path.Combine(directory.FullName, "truncated");
            await File.WriteAllTextAsync(inputPath, signature);
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["parse", inputPath], output, error);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(output.ToString()).IsEqualTo(string.Empty);
            await Assert.That(error.ToString()).Contains("gbx:");
            await Assert.That(error.ToString().Contains("Unsupported file format")).IsFalse();
            await Assert.That(error.ToString().Contains("Use --help")).IsFalse();

            using var reopened = new FileStream(inputPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    internal static byte[] CreatePakData(bool privateHeader = false)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        writer.Write("NadeoPak"u8);
        writer.Write(6);
        writer.Write(new byte[32]);
        writer.Write(privateHeader ? 1u : 0u);

        if (privateHeader)
        {
            return stream.ToArray();
        }

        writer.Write(new byte[16]);
        writer.Write(0u);
        writer.Write(0);
        writer.Write(0);
        writer.Write(0);
        writer.Write(new byte[16]);
        writer.Write(0);
        WritePakString(writer, "author [red]");
        WritePakString(writer, "Author");
        WritePakString(writer, string.Empty);
        WritePakString(writer, string.Empty);
        writer.Write(0u);
        writer.Write(0);
        writer.Write(1);
        writer.Write(-1);
        WritePakString(writer, "sample [blue].txt");
        writer.Write(0);
        writer.Write(0);
        writer.Write(0);
        writer.Write(0u);
        writer.Write(0u);
        writer.Write(0x4000000000000UL);
        return stream.ToArray();
    }

    private static void WritePakString(BinaryWriter writer, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        writer.Write(bytes.Length);
        writer.Write(bytes);
    }
}
