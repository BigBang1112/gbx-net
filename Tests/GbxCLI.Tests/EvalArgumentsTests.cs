using GbxCLI.Commands;

namespace GbxCLI.Tests;

public class EvalArgumentsTests
{
    [Test]
    public async Task Parse_ConsumesSharedEnumeratorAndPreservesExpressionOrder()
    {
        using var argsEnumerator = new[] { "eval", "input.pak", "var number = 42;", "number", "Files.Count" }.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();
        argsEnumerator.MoveNext();

        var arguments = EvalArguments.Parse(argsEnumerator, output);

        await Assert.That(arguments!.InputPath).IsEqualTo("input.pak");
        await Assert.That(arguments.Expressions.Count).IsEqualTo(3);
        await Assert.That(arguments.Expressions[0]).IsEqualTo("var number = 42;");
        await Assert.That(arguments.Expressions[1]).IsEqualTo("number");
        await Assert.That(arguments.Expressions[2]).IsEqualTo("Files.Count");
        await Assert.That(arguments.Interactive).IsFalse();
        await Assert.That(argsEnumerator.MoveNext()).IsFalse();
        await Assert.That(output.ToString()).IsEqualTo(string.Empty);
    }

    [Test]
    [Arguments("-i", true)]
    [Arguments("-i", false)]
    [Arguments("--interactive", true)]
    [Arguments("--interactive", false)]
    public async Task Parse_InteractiveOptionCanPrecedeOrFollowInput(string option, bool optionFirst)
    {
        using var argsEnumerator = (optionFirst ? new[] { option, "input.Gbx" } : new[] { "input.Gbx", option }).AsEnumerable().GetEnumerator();
        using var output = new StringWriter();

        var arguments = EvalArguments.Parse(argsEnumerator, output);

        await Assert.That(arguments!.InputPath).IsEqualTo("input.Gbx");
        await Assert.That(arguments.Interactive).IsTrue();
        await Assert.That(arguments.Expressions.Count).IsEqualTo(0);
    }

    [Test]
    public async Task Parse_InteractiveSessionCanHaveInitialExpressions()
    {
        using var argsEnumerator = new[] { "input.Gbx", "var number = 42;", "-i", "number" }.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();

        var arguments = EvalArguments.Parse(argsEnumerator, output);

        await Assert.That(arguments!.Interactive).IsTrue();
        await Assert.That(arguments.Expressions.Count).IsEqualTo(2);
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    public void Parse_EmptyExpressionsAreRejected(string expression)
    {
        using var argsEnumerator = new[] { "input.Gbx", expression }.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();

        Assert.Throws<ArgumentException>(() => EvalArguments.Parse(argsEnumerator, output));
    }

    [Test]
    public void Parse_NoninteractiveEvaluationRequiresCode()
    {
        using var argsEnumerator = new[] { "input.Gbx" }.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();

        Assert.Throws<ArgumentException>(() => EvalArguments.Parse(argsEnumerator, output));
    }

    [Test]
    public void Parse_InteractiveSessionStillRequiresFile()
    {
        using var argsEnumerator = new[] { "-i" }.AsEnumerable().GetEnumerator();
        using var output = new StringWriter();

        Assert.Throws<ArgumentException>(() => EvalArguments.Parse(argsEnumerator, output));
    }
}
