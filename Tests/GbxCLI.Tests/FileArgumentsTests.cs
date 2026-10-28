using GbxCLI.Commands;

namespace GbxCLI.Tests;

public class FileArgumentsTests
{
    [Test]
    [Arguments("-e")]
    [Arguments("--eval")]
    public void Parse_EvaluationOptionsAreNoLongerAccepted(string option)
    {
        using var argsEnumerator = new[] { "input.pak", option, "Files.Count" }.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();

        Assert.Throws<ArgumentException>(() => FileArguments.Parse(argsEnumerator, "parse", output, requiresOutput: false));
    }

    [Test]
    [Arguments("compress", "-e")]
    [Arguments("decompress", "--eval")]
    public void Parse_EvaluationIsRejectedForCompression(string command, string option)
    {
        using var argsEnumerator = new[] { "input.Gbx", option, "Node" }.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();

        Assert.Throws<ArgumentException>(() => FileArguments.Parse(argsEnumerator, command, output, requiresOutput: true));
    }

    [Test]
    [Arguments("verify", "-g", "TMSX", GBX.NET.GameVersion.TMSX)]
    [Arguments("verify", "--game", "tmnESWC", GBX.NET.GameVersion.TMNESWC)]
    [Arguments("pak verify", "--game", "TMF", GBX.NET.GameVersion.TMF)]
    public async Task Parse_GameSelectsSingleGameVersion(string command, string option, string value, GBX.NET.GameVersion expected)
    {
        using var argsEnumerator = new[] { option, value, "input.Gbx" }.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();

        var arguments = FileArguments.Parse(argsEnumerator, command, output, requiresOutput: false);

        await Assert.That(arguments!.Game).IsEqualTo(expected);
        await Assert.That(arguments.InputPath).IsEqualTo("input.Gbx");
    }

    [Test]
    public async Task Parse_GameDefaultsToUnspecified()
    {
        using var argsEnumerator = new[] { "input.Gbx" }.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();

        var arguments = FileArguments.Parse(argsEnumerator, "verify", output, requiresOutput: false);

        await Assert.That(arguments!.Game).IsEqualTo(GBX.NET.GameVersion.Unspecified);
    }

    [Test]
    [Arguments("Unspecified")]
    [Arguments("16")]
    [Arguments("TMF,TMSX")]
    [Arguments("Nope")]
    public async Task Parse_InvalidGameIsRejected(string value)
    {
        using var argsEnumerator = new[] { "--game", value, "input.Gbx" }.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();

        var exception = Assert.Throws<ArgumentException>(() => FileArguments.Parse(argsEnumerator, "verify", output, requiresOutput: false));

        await Assert.That(exception.Message).Contains($"Unknown game version: {value}.");
    }

    [Test]
    [Arguments(new[] { "--game" }, "Missing game version after --game.")]
    [Arguments(new[] { "-g", "TMF", "--game", "TMSX", "input.Gbx" }, "Game can only be specified once.")]
    public async Task Parse_GameRequiresOneValue(string[] args, string message)
    {
        using var argsEnumerator = args.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();

        var exception = Assert.Throws<ArgumentException>(() => FileArguments.Parse(argsEnumerator, "verify", output, requiresOutput: false));

        await Assert.That(exception.Message).IsEqualTo(message);
    }

    [Test]
    [Arguments("parse")]
    [Arguments("compress")]
    [Arguments("pak files")]
    public void Parse_GameIsRejectedOutsideVerify(string command)
    {
        using var argsEnumerator = new[] { "--game", "TMF", "input.Gbx" }.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();

        Assert.Throws<ArgumentException>(() => FileArguments.Parse(argsEnumerator, command, output, requiresOutput: false));
    }

    [Test]
    public async Task Parse_PreservesInputConsumedByDispatcher()
    {
        var args = new[] { "input file.Gbx" };
        using var argsEnumerator = args.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();
        await Assert.That(argsEnumerator.MoveNext()).IsTrue();

        var arguments = FileArguments.Parse(argsEnumerator, "parse", output, requiresOutput: false, inputPath: argsEnumerator.Current);

        await Assert.That(arguments!.InputPath).IsEqualTo("input file.Gbx");
        await Assert.That(arguments.Interactive).IsTrue();
        await Assert.That(output.ToString()).IsEqualTo(string.Empty);
    }

    [Test]
    [Arguments("-o")]
    [Arguments("--output")]
    public async Task Parse_ConsumesArgumentsAfterCommand(string outputOption)
    {
        var args = new[] { "compress", outputOption, "output file.Gbx", "input file.Gbx" };
        using var argsEnumerator = args.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();
        await Assert.That(argsEnumerator.MoveNext()).IsTrue();

        var arguments = FileArguments.Parse(argsEnumerator, "compress", output, requiresOutput: true);

        await Assert.That(arguments).IsNotNull();
        await Assert.That(arguments!.InputPath).IsEqualTo("input file.Gbx");
        await Assert.That(arguments.OutputPath).IsEqualTo("output file.Gbx");
        await Assert.That(argsEnumerator.MoveNext()).IsFalse();
    }

    [Test]
    [Arguments("-input.Gbx")]
    [Arguments("--input.Gbx")]
    [Arguments("--output-file.Gbx")]
    [Arguments("--help.Gbx")]
    [Arguments("--version.Gbx")]
    [Arguments("--interactive.Gbx")]
    [Arguments("-i.Gbx")]
    [Arguments("--eval.Gbx")]
    [Arguments("./--help")]
    [Arguments("--")]
    public async Task Parse_OnlyExactOptionsAreRecognized(string inputPath)
    {
        var args = new[] { "parse", inputPath };
        using var argsEnumerator = args.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();
        await Assert.That(argsEnumerator.MoveNext()).IsTrue();

        var arguments = FileArguments.Parse(argsEnumerator, "parse", output, requiresOutput: false);

        await Assert.That(arguments!.InputPath).IsEqualTo(inputPath);
        await Assert.That(output.ToString()).IsEqualTo(string.Empty);
    }

    [Test]
    [Arguments("-o")]
    [Arguments("--output")]
    public async Task Parse_OutputValueIsNotTreatedAsOption(string outputOption)
    {
        var args = new[] { "compress", "input.Gbx", outputOption, "--help" };
        using var argsEnumerator = args.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();
        await Assert.That(argsEnumerator.MoveNext()).IsTrue();

        var arguments = FileArguments.Parse(argsEnumerator, "compress", output, requiresOutput: true);

        await Assert.That(arguments!.InputPath).IsEqualTo("input.Gbx");
        await Assert.That(arguments.OutputPath).IsEqualTo("--help");
        await Assert.That(output.ToString()).IsEqualTo(string.Empty);
    }

    [Test]
    public void Parse_DuplicateOutputIsRejected()
    {
        var args = new[] { "compress", "input.Gbx", "-o", "first.Gbx", "--output", "second.Gbx" };
        using var argsEnumerator = args.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();
        argsEnumerator.MoveNext();

        Assert.Throws<ArgumentException>(() => FileArguments.Parse(argsEnumerator, "compress", output, requiresOutput: true));
    }

    [Test]
    public void Parse_ExtraInputIsRejected()
    {
        var args = new[] { "parse", "input.Gbx", "extra.Gbx" };
        using var argsEnumerator = args.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();
        argsEnumerator.MoveNext();

        Assert.Throws<ArgumentException>(() => FileArguments.Parse(argsEnumerator, "parse", output, requiresOutput: false));
    }

    [Test]
    [Arguments("-i", false)]
    [Arguments("--interactive", false)]
    [Arguments("-i", true)]
    [Arguments("--interactive", true)]
    public async Task Parse_InteractiveOptionCanPrecedeOrFollowInput(string option, bool optionFirst)
    {
        var args = optionFirst ? new[] { option, "input.Gbx" } : new[] { "input.Gbx", option };
        using var argsEnumerator = args.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();

        var arguments = FileArguments.Parse(argsEnumerator, "parse", output, requiresOutput: false);

        await Assert.That(arguments!.InputPath).IsEqualTo("input.Gbx");
        await Assert.That(arguments.Interactive).IsTrue();
        await Assert.That(argsEnumerator.MoveNext()).IsFalse();
    }

    [Test]
    [Arguments("-i")]
    [Arguments("--interactive")]
    public async Task Parse_InteractiveOptionPreservesConsumedFile(string option)
    {
        using var argsEnumerator = new[] { option }.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();

        var arguments = FileArguments.Parse(argsEnumerator, "parse", output, requiresOutput: false, inputPath: "input.Gbx");

        await Assert.That(arguments!.InputPath).IsEqualTo("input.Gbx");
        await Assert.That(arguments.Interactive).IsTrue();
    }

    [Test]
    [Arguments("compress", "-i")]
    [Arguments("compress", "--interactive")]
    [Arguments("decompress", "-i")]
    [Arguments("decompress", "--interactive")]
    public void Parse_InteractiveOptionIsRejectedForCompression(string command, string option)
    {
        using var argsEnumerator = new[] { "input.Gbx", option, "-o", "output.Gbx" }.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();

        Assert.Throws<ArgumentException>(() => FileArguments.Parse(argsEnumerator, command, output, requiresOutput: true));
    }

    [Test]
    public async Task Parse_ExplicitCommandRequiresInteractiveOption()
    {
        using var argsEnumerator = new[] { "input.Gbx" }.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();

        var arguments = FileArguments.Parse(argsEnumerator, "parse", output, requiresOutput: false);

        await Assert.That(arguments!.Interactive).IsFalse();
    }
}
