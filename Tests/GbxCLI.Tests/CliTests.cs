using GBX.NET;
using GBX.NET.Engines.MwFoundations;
using System.Reflection;
using TUnit.Assertions.Enums;

namespace GbxCLI.Tests;

public class CliTests
{
    [Test]
    public async Task Startup_InitializesCompressionProviders()
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        var exitCode = await Cli.RunAsync(["--help"], output, error);

        await Assert.That(exitCode).IsEqualTo(0);
        await Assert.That(Gbx.LZO is GBX.NET.LZO.Lzo).IsTrue();
        await Assert.That(Gbx.ZLib is GBX.NET.ZLib.ZLib).IsTrue();
        await Assert.That(error.ToString()).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task NoArguments_ShowsHelpWithoutWaitingForRedirectedOutput()
    {
        using var output = new StringWriter();
        using var error = new StringWriter();
        using var helpOutput = new StringWriter();

        var exitCode = await Cli.RunAsync([], output, error);
        var helpExitCode = await Cli.RunAsync(["--help"], helpOutput, error);

        await Assert.That(exitCode).IsEqualTo(0);
        await Assert.That(helpExitCode).IsEqualTo(0);
        await Assert.That(output.ToString()).Contains("Usage: gbx");
        await Assert.That(output.ToString()).IsEqualTo(helpOutput.ToString());
        await Assert.That(output.ToString().Contains("Press any key")).IsFalse();
        await Assert.That(error.ToString()).IsEqualTo(string.Empty);
    }

    [Test]
    [Arguments("-h")]
    [Arguments("--help")]
    [Arguments("-v")]
    [Arguments("--version")]
    public async Task InformationalOption_Succeeds(string option)
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        var exitCode = await Cli.RunAsync([option], output, error);

        await Assert.That(exitCode).IsEqualTo(0);
        await Assert.That(output.ToString()).Contains("gbx");
        await Assert.That(output.ToString().Contains("Press any key")).IsFalse();
        await Assert.That(error.ToString()).IsEqualTo(string.Empty);
    }

    [Test]
    [Arguments("-h")]
    [Arguments("--help")]
    [Arguments("-v")]
    [Arguments("--version")]
    public async Task InformationalOutput_IdentifiesDistribution(string option)
    {
        using var output = new StringWriter();
        using var error = new StringWriter();
        var distribution = typeof(Cli).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(attribute => attribute.Key == "CliDistribution").Value;

        var exitCode = await Cli.RunAsync([option], output, error);

        await Assert.That(exitCode).IsEqualTo(0);
        await Assert.That(distribution is ".NET tool" or "standalone").IsTrue();
        await Assert.That(output.ToString()).Contains($"({distribution})");
        await Assert.That(error.ToString()).IsEqualTo(string.Empty);
    }

    [Test]
    [Arguments("unknown")]
    [Arguments("--unknown")]
    [Arguments("info")]
    [Arguments("eval")]
    [Arguments("parse")]
    [Arguments("script")]
    [Arguments("compress")]
    [Arguments("decompress")]
    public async Task InvalidArguments_ReportError(string argument)
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        var exitCode = await Cli.RunAsync([argument], output, error);

        await Assert.That(exitCode).IsEqualTo(1);
        await Assert.That(output.ToString()).IsEqualTo(string.Empty);
        await Assert.That(error.ToString()).Contains("gbx:");
    }

    [Test]
    [Arguments("parse")]
    [Arguments("compress")]
    [Arguments("decompress")]
    [Arguments("eval")]
    [Arguments("pak")]
    [Arguments("script")]
    public async Task CommandHandler_AcceptsGlobalOptions(string command)
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        await Assert.That(await Cli.RunAsync([command, "--help"], output, error)).IsEqualTo(0);
        await Assert.That(output.ToString()).Contains("Usage: gbx");

        output.GetStringBuilder().Clear();

        await Assert.That(await Cli.RunAsync([command, "--version"], output, error)).IsEqualTo(0);
        await Assert.That(output.ToString()).Contains("gbx 0.1.0");
        await Assert.That(error.ToString()).IsEqualTo(string.Empty);
    }

    [Test]
    [Arguments("-o")]
    [Arguments("--output")]
    public async Task OutputOption_RequiresValue(string option)
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        var exitCode = await Cli.RunAsync(["decompress", "input.Gbx", option], output, error);

        await Assert.That(exitCode).IsEqualTo(1);
        await Assert.That(error.ToString()).Contains($"Missing file path after {option}");
    }

    [Test]
    public async Task ExampleCommands_RoundTripWithoutOverwritingFiles()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-");

        try
        {
            var inputPath = Path.Combine(directory.FullName, "input [blue] file.Gbx");
            var compressedPath = Path.Combine(directory.FullName, "compressed [green].Gbx");
            var decompressedPath = Path.Combine(directory.FullName, "decompressed.Gbx");
            var gbx = new Gbx<CMwNod>(new CMwNod())
            {
                BodyCompression = GbxCompression.Uncompressed
            };
            gbx.Save(inputPath);

            using var output = new StringWriter();
            using var error = new StringWriter();

            await Assert.That(await Cli.RunAsync(["parse", inputPath], output, error)).IsEqualTo(0);
            await Assert.That(output.ToString()).Contains($"File: {inputPath}");
            await Assert.That(output.ToString()).Contains("Body compression: Uncompressed");
            await Assert.That(await Cli.RunAsync(["compress", inputPath, "-o", compressedPath], output, error)).IsEqualTo(0);
            await Assert.That(output.ToString()).Contains($"Written: {compressedPath}");
            await Assert.That(Gbx.IsCompressed(compressedPath)).IsTrue();
            await Assert.That(await Cli.RunAsync(["decompress", "--output", decompressedPath, compressedPath], output, error)).IsEqualTo(0);
            await Assert.That(File.ReadAllBytes(decompressedPath)).IsEquivalentTo(File.ReadAllBytes(inputPath), CollectionOrdering.Matching);
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);

            var compressedBytes = File.ReadAllBytes(compressedPath);
            await Assert.That(await Cli.RunAsync(["compress", inputPath, "-o", compressedPath], output, error)).IsEqualTo(1);
            await Assert.That(File.ReadAllBytes(compressedPath)).IsEquivalentTo(compressedBytes, CollectionOrdering.Matching);
            await Assert.That(await Cli.RunAsync(["decompress", inputPath, "-o", inputPath], output, error)).IsEqualTo(1);
            await Assert.That(File.ReadAllBytes(inputPath)).IsEquivalentTo(File.ReadAllBytes(decompressedPath), CollectionOrdering.Matching);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task ExistingFileWithoutCommand_ShowsInteractivePlaceholder()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-");

        try
        {
            var inputPath = Path.Combine(directory.FullName, "input file.Gbx");
            var gbx = new Gbx<CMwNod>(new CMwNod())
            {
                BodyCompression = GbxCompression.Uncompressed
            };
            gbx.Save(inputPath);

            using var output = new StringWriter();
            using var explicitOutput = new StringWriter();
            using var error = new StringWriter();

            await Assert.That(await Cli.RunAsync([inputPath], output, error)).IsEqualTo(0);
            await Assert.That(output.ToString()).Contains("Interactive mode is not implemented yet");
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);

            error.GetStringBuilder().Clear();

            await Assert.That(await Cli.RunAsync(["parse", inputPath], explicitOutput, error)).IsEqualTo(0);
            await Assert.That(explicitOutput.ToString()).Contains("Body compression: Uncompressed");
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task ExistingInvalidFileWithoutCommand_IsNotTreatedAsUnknownCommand()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-");

        try
        {
            var inputPath = Path.Combine(directory.FullName, "invalid.Gbx");
            File.WriteAllText(inputPath, "Not a Gbx file.");
            using var output = new StringWriter();
            using var error = new StringWriter();

            await Assert.That(await Cli.RunAsync([inputPath], output, error)).IsEqualTo(0);
            await Assert.That(output.ToString()).Contains("Interactive mode is not implemented yet");
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task MissingFileWithoutCommand_SuggestsHelp()
    {
        var inputPath = Path.Combine(Path.GetTempPath(), $"gbx-cli-{Guid.NewGuid():N}.Gbx");
        using var output = new StringWriter();
        using var error = new StringWriter();

        await Assert.That(await Cli.RunAsync([inputPath], output, error)).IsEqualTo(1);
        await Assert.That(output.ToString()).IsEqualTo(string.Empty);
        await Assert.That(error.ToString()).Contains("Use --help");
    }

    [Test]
    [Arguments("-i")]
    [Arguments("--interactive")]
    public async Task InteractiveParse_ShowsPlaceholderWithoutOpeningFile(string option)
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        var exitCode = await Cli.RunAsync(["parse", "input.Gbx", option], output, error);

        await Assert.That(exitCode).IsEqualTo(0);
        await Assert.That(output.ToString()).Contains("Interactive mode is not implemented yet");
        await Assert.That(output.ToString().Contains('\u001b')).IsFalse();
        await Assert.That(error.ToString()).IsEqualTo(string.Empty);
    }

    [Test]
    [Arguments("-i")]
    [Arguments("--interactive")]
    public async Task InteractiveDefaultCommand_ShowsPlaceholderWithoutParsingFile(string option)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-");

        try
        {
            var inputPath = Path.Combine(directory.FullName, "input.Gbx");
            File.WriteAllText(inputPath, "Existing file");
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync([inputPath, option], output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(output.ToString()).Contains("Interactive mode is not implemented yet");
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task InteractiveParse_HelpDoesNotRequireTerminalOrFile()
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        var exitCode = await Cli.RunAsync(["parse", "--interactive", "--help"], output, error);

        await Assert.That(exitCode).IsEqualTo(0);
        await Assert.That(output.ToString()).Contains("parse <file>");
        await Assert.That(output.ToString()).Contains("-i, --interactive");
        await Assert.That(error.ToString()).IsEqualTo(string.Empty);
    }
}
