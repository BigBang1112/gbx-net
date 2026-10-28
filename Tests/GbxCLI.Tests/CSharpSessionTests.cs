using GBX.NET;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.PAK;
using Microsoft.CodeAnalysis.Scripting;

namespace GbxCLI.Tests;

public class CSharpSessionTests
{
    [Test]
    public async Task Evaluate_UsesCurrentObjectMembersAndMethods()
    {
        var session = new CSharpSession(new Vec3(2, 3, 6));

        await Assert.That(await session.EvaluateAsync("Value.X + Value.Y")).IsEqualTo<object?>(5f);
        await Assert.That(await session.EvaluateAsync("Value.GetSqrMagnitude()")).IsEqualTo<object?>(49f);
    }

    [Test]
    public async Task Evaluate_ArraysExposeTypedValueForLinq()
    {
        var session = new CSharpSession(new[] { 1, 2, 3 });

        await Assert.That(await session.EvaluateAsync("Value.Sum()")).IsEqualTo<object?>(6);
    }

    [Test]
    public async Task Evaluate_PreservesVariablesAndSupportsAsyncAndLinq()
    {
        var session = new CSharpSession(new CMwNod());

        await session.EvaluateAsync("var numbers = new[] { 1, 2, 3 };");
        var result = await session.EvaluateAsync("await Task.FromResult(numbers.Sum())");

        await Assert.That(result).IsEqualTo<object?>(6);
    }

    [Test]
    public async Task Evaluate_PakCanQueryFileIndexAndReturnBrowsableObjects()
    {
        await using var stream = new MemoryStream(ParseCommandTests.CreatePakData());
        await using var pak = await Pak.ParseAsync(stream);
        var session = new CSharpSession(pak);

        var result = await session.EvaluateAsync("Files.Values.Where(file => !file.IsEncrypted).ToArray()");

        await Assert.That(((PakFile[])result!).Single().Name).IsEqualTo("sample [blue].txt");
    }

    [Test]
    public async Task Evaluate_ErrorsDoNotDiscardPreviousSession()
    {
        var session = new CSharpSession(new CMwNod());
        await session.EvaluateAsync("var number = 42;");

        await Assert.ThrowsAsync<CompilationErrorException>(() => session.EvaluateAsync("MissingMember"));
        await Assert.ThrowsAsync<InvalidOperationException>(() => session.EvaluateAsync("throw new InvalidOperationException(\"sample\");"));

        await Assert.That(await session.EvaluateAsync("number")).IsEqualTo<object?>(42);
    }
}
