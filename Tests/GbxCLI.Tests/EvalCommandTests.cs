using GBX.NET;
using GBX.NET.Engines.MwFoundations;

namespace GbxCLI.Tests;

public class EvalCommandTests
{
    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Eval_DetectsFormatAndEvaluatesExpressionsInOrder(bool isPak)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-eval-");

        try
        {
            var inputPath = Path.Combine(directory.FullName, "input");

            if (isPak)
            {
                await File.WriteAllBytesAsync(inputPath, ParseCommandTests.CreatePakData());
            }
            else
            {
                new Gbx<CMwNod>(new CMwNod()) { BodyCompression = GbxCompression.Uncompressed }.Save(inputPath);
            }

            using var output = new StringWriter();
            using var error = new StringWriter();
            var query = isPak ? "Files.Values.Select(file => file.Name).ToArray()" : "Node.Chunks.Count";

            var exitCode = await Cli.RunAsync(["eval", inputPath, "var number = 42;", "number", query,
                "new { Detail = new { Name = \"nested [red]\", Value = 42 }, Data = new byte[] { 1, 2, 3 } }"], output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(output.ToString()).Contains($"Result: (null){Environment.NewLine}Result: 42{Environment.NewLine}");
            await Assert.That(output.ToString()).Contains(isPak ? "[0]: sample [blue].txt" : $"Result: 42{Environment.NewLine}Result: 0{Environment.NewLine}");
            await Assert.That(output.ToString()).Contains("    Name: nested [red]");
            await Assert.That(output.ToString()).Contains("    Value: 42");
            await Assert.That(output.ToString()).Contains("  Data: 3 bytes (hex): 010203");
            await Assert.That(output.ToString().Contains("File:")).IsFalse();
            await Assert.That(output.ToString().Contains("> ")).IsFalse();
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
    [Arguments("MissingMember", "CS0103")]
    [Arguments("throw new InvalidOperationException(\"literal [red] failure\");", "literal [red] failure")]
    public async Task Eval_ErrorsUseStandardErrorAndReleaseFile(string code, string expectedError)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-eval-");

        try
        {
            var inputPath = Path.Combine(directory.FullName, "input.pak");
            await File.WriteAllBytesAsync(inputPath, ParseCommandTests.CreatePakData());
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["eval", inputPath, code], output, error);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(output.ToString()).IsEqualTo(string.Empty);
            await Assert.That(error.ToString()).Contains(expectedError);
            await Assert.That(error.ToString().Contains('\u001b')).IsFalse();

            using var reopened = new FileStream(inputPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("Not a supported file")]
    [Arguments("GBX")]
    [Arguments("NadeoPak")]
    public async Task Eval_ParsingErrorsReleaseFile(string contents)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-eval-");

        try
        {
            var inputPath = Path.Combine(directory.FullName, "input");
            await File.WriteAllTextAsync(inputPath, contents);
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["eval", inputPath, "1 + 1"], output, error);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(output.ToString()).IsEqualTo(string.Empty);
            await Assert.That(error.ToString()).Contains("gbx:");
            await Assert.That(error.ToString().Contains("Use --help")).IsFalse();

            using var reopened = new FileStream(inputPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("-i")]
    [Arguments("--interactive")]
    public async Task Eval_InteractiveShowsPlaceholderWithoutOpeningFile(string option)
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        var exitCode = await Cli.RunAsync(["eval", "input.Gbx", option], output, error);

        await Assert.That(exitCode).IsEqualTo(0);
        await Assert.That(output.ToString()).Contains("Interactive mode is not implemented yet");
        await Assert.That(output.ToString().Contains('\u001b')).IsFalse();
        await Assert.That(error.ToString()).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task Eval_InteractiveDoesNotEvaluateInitialExpressions()
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        var exitCode = await Cli.RunAsync(["eval", "input.Gbx", "MissingMember", "--interactive"], output, error);

        await Assert.That(exitCode).IsEqualTo(0);
        await Assert.That(output.ToString()).Contains("Interactive mode is not implemented yet");
        await Assert.That(error.ToString()).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task Eval_InteractiveHelpDoesNotRequireTerminalOrFile()
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        var exitCode = await Cli.RunAsync(["eval", "--interactive", "--help"], output, error);

        await Assert.That(exitCode).IsEqualTo(0);
        await Assert.That(output.ToString()).Contains("eval <file> [code...]");
        await Assert.That(error.ToString()).IsEqualTo(string.Empty);
    }
}
