using GBX.NET.PAK;
using System.Buffers.Binary;

namespace GbxCLI.Tests;

public class VerifyCommandTests
{
    [Test]
    public async Task Verify_Directory_ParsesGbxFilesRecursivelyAndReportsFailures()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-verify-");

        try
        {
            var path = directory.FullName;
            var unknown = PakEntryTests.CreateGbx(compressed: false);
            BinaryPrimitives.WriteUInt32LittleEndian(unknown.AsSpan(9), 0x7FFF0000);
            Directory.CreateDirectory(Path.Combine(path, "Sub", "Deeper"));
            await File.WriteAllBytesAsync(Path.Combine(path, "Valid.Gbx"), PakEntryTests.CreateGbx(compressed: false));
            await File.WriteAllBytesAsync(Path.Combine(path, "Sub", "Renamed.dat"), PakEntryTests.CreateGbx(compressed: true));
            await File.WriteAllBytesAsync(Path.Combine(path, "Sub", "Broken.Gbx"), "Invalid Gbx header"u8.ToArray());
            await File.WriteAllBytesAsync(Path.Combine(path, "Sub", "Deeper", "Unknown.Gbx"), unknown);
            await File.WriteAllBytesAsync(Path.Combine(path, "Archive.Pack.Gbx"), [.. Pak.Magic.Select(c => (byte)c), 0]);
            await File.WriteAllTextAsync(Path.Combine(path, "Readme.txt"), "text");
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["verify", path], output, error);
            var lines = output.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(lines.Length).IsEqualTo(3);
            await Assert.That(lines[0]).StartsWith(Path.Combine("Sub", "Broken.Gbx") + ": ");
            await Assert.That(lines[1]).StartsWith(Path.Combine("Sub", "Deeper", "Unknown.Gbx") + ": NotSupportedException: Root class 0x7FFF0000");
            await Assert.That(lines[2]).IsEqualTo("Parsed 2 Gbx files; 2 failed; 2 non-Gbx files skipped.");
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Verify_ValidFile_Succeeds()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-verify-");

        try
        {
            var path = Path.Combine(directory.FullName, "Valid.Gbx");
            await File.WriteAllBytesAsync(path, PakEntryTests.CreateGbx(compressed: true));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["verify", path], output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(output.ToString()).IsEqualTo($"Parsed 1 Gbx files; 0 failed; 0 non-Gbx files skipped.{Environment.NewLine}");
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            using var reopened = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Verify_GameOption_ParsesWithSelectedGame()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-verify-");

        try
        {
            var path = Path.Combine(directory.FullName, "Valid.Gbx");
            await File.WriteAllBytesAsync(path, PakEntryTests.CreateGbx(compressed: true));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["verify", "--game", "TMSX", path], output, error);

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
    public async Task Verify_BrokenFile_ReportsFailure()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-verify-");

        try
        {
            var path = Path.Combine(directory.FullName, "Broken.Gbx");
            await File.WriteAllBytesAsync(path, "GBX"u8.ToArray());
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["verify", path], output, error);
            var lines = output.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(lines.Length).IsEqualTo(2);
            await Assert.That(lines[0]).StartsWith(path + ": ");
            await Assert.That(lines[1]).IsEqualTo("Parsed 0 Gbx files; 1 failed; 0 non-Gbx files skipped.");
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("Readme.txt", "is not a Gbx file")]
    [Arguments("Missing.Gbx", "was not found")]
    public async Task Verify_InvalidInput_Fails(string name, string message)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-verify-");

        try
        {
            await File.WriteAllTextAsync(Path.Combine(directory.FullName, "Readme.txt"), "text");
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["verify", Path.Combine(directory.FullName, name)], output, error);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(output.ToString()).IsEqualTo(string.Empty);
            await Assert.That(error.ToString()).Contains(message);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }
}
