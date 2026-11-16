using GBX.NET;

namespace GbxCLI.Tests;

public class CompressionCommandTests
{
    [Test]
    public async Task CompressAndDecompress_RoundTrip_ChangesBodyCompression()
    {
        using var directory = new TempDirectory();
        var input = directory.Write("input $1.Gbx", Fixtures.CreateGbx());
        var compressed = directory.Combine("compressed.Gbx");
        var decompressed = directory.Combine("decompressed.Gbx");

        var compress = await CliRun.RunAsync("compress", input, "-o", compressed);
        var decompress = await CliRun.RunAsync("decompress", compressed, "--output", decompressed);

        await Assert.That(compress.ExitCode).IsEqualTo(0);
        await Assert.That(compress.Output.Trim()).IsEqualTo($"Written: {compressed}");
        await Assert.That(decompress.ExitCode).IsEqualTo(0);
        await Assert.That(Gbx.ParseHeader(compressed).Header.Basic.CompressionOfBody).IsEqualTo(GbxCompression.Compressed);
        await Assert.That(Gbx.ParseHeader(decompressed).Header.Basic.CompressionOfBody).IsEqualTo(GbxCompression.Uncompressed);
        await Assert.That(Directory.GetFiles(directory.Path, "*.tmp").Length).IsEqualTo(0);
    }

    [Test]
    [Arguments("compress")]
    [Arguments("decompress")]
    public async Task Run_WithoutOutput_ReplacesInputAndWritesBackupAfterWritten(string command)
    {
        using var directory = new TempDirectory();
        var original = Fixtures.CreateGbx(compressed: command == "decompress");
        var input = directory.Write("input.Gbx", original);

        var run = await CliRun.RunAsync(command, input);
        var lines = run.Output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(lines).IsEquivalentTo([$"Written: {input}", $"Backup: {input}.bak"]);
        await Assert.That(lines[0]).StartsWith("Written:");
        await Assert.That(await File.ReadAllBytesAsync(input + ".bak")).IsEquivalentTo(original);
    }

    [Test]
    public async Task Run_NoBackup_KeepsNoBackup()
    {
        using var directory = new TempDirectory();
        var input = directory.Write("input.Gbx", Fixtures.CreateGbx());

        var run = await CliRun.RunAsync("compress", input, "--no-backup");

        await Assert.That(run.ExitCode).IsEqualTo(0);
        await Assert.That(run.Output.Contains("Backup")).IsFalse();
        await Assert.That(File.Exists(input + ".bak")).IsFalse();
    }

    [Test]
    public async Task Run_InvalidInput_LeavesNoTemporaryFile()
    {
        using var directory = new TempDirectory();
        var input = directory.Write("invalid.Gbx", "not a Gbx"u8.ToArray());

        var run = await CliRun.RunAsync("compress", input);

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(Directory.GetFiles(directory.Path)).IsEquivalentTo([input]);
        await Assert.That(await File.ReadAllTextAsync(input)).IsEqualTo("not a Gbx");
    }

    [Test]
    public async Task Run_OutputTwice_Fails()
    {
        var run = await CliRun.RunAsync("compress", "a.Gbx", "-o", "b.Gbx", "-o", "c.Gbx");

        await Assert.That(run.ExitCode).IsEqualTo(1);
        await Assert.That(run.Error).Contains("Output can only be specified once.");
    }
}
