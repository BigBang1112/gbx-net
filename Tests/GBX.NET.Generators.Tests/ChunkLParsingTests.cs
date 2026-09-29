using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using ChunkL.Syntax;
using GBX.NET.Generators.Analysis;
using GBX.NET.Generators.Parsing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace GBX.NET.Generators.Tests;

public class ChunkLParsingTests
{
    [Test]
    public async Task ParsesLayoutsAndCombinesThemWithExistingTypes()
    {
        const string source = """
            Example 0x03043000
            - inherits: CMwNod

            0x002 (header)
              version
              int HeaderValue

            0x003 (skippable)
              version
              Key[] Keys (list)
              v2+
                bool Enabled

            archive Key
              timefloat Time

            archive
              int Value

            enum Kind
              First = 1
              Second
            """;
        var additionalText = new MemoryAdditionalText("Engines/Game/DifferentName.CHUNKL", source);
        var result = await Run(CreateDriver(additionalText, new MemoryAdditionalText("ClassId.txt", "ignored")));
        await Assert.That(result.Diagnostics).IsEmpty();
        var file = await Assert.That(GetFiles(result)).HasSingleItem();
        await Assert.That(file.Path).IsEqualTo(additionalText.Path);
        await Assert.That(file.Source).IsSameReferenceAs(additionalText.Text);
        await Assert.That(file.Engine).IsEqualTo("Game");
        await Assert.That(file.TypeKey).IsEqualTo("GBX.NET.Engines.Game.Example");
        await Assert.That(file.Syntax.Header.ClassId).IsEqualTo("0x03043000");
        await Assert.That(file.Syntax.ClassAttributes.Single().Value).IsEqualTo("CMwNod");
        await Assert.That(file.Syntax.Chunks.Count).IsEqualTo(2);
        await Assert.That(file.Syntax.Chunks[0].Attributes!.Entries.Single().Name).IsEqualTo("header");
        await Assert.That(file.Syntax.Chunks[1].Attributes!.Entries.Single().Name).IsEqualTo("skippable");
        await Assert.That(file.Syntax.Chunks[1].Body).Contains(x => x is FieldDeclaration { Name: "Keys" });
        await Assert.That(file.Syntax.Archives.Count).IsEqualTo(2);
        await Assert.That(file.Syntax.Archives[0].Name).IsEqualTo("Key");
        await Assert.That(string.IsNullOrEmpty(file.Syntax.Archives[1].Name)).IsTrue();
        await Assert.That(file.Syntax.Enums.Single().Name).IsEqualTo("Kind");

        var generationInputs = result.TrackedSteps["GenerationInputs"].Single().Outputs.Single().Value;
        var inputs = await Assert.That(generationInputs)
            .IsTypeOf<(ImmutableDictionary<string, ExistingType>, ImmutableArray<ParsedChunkLFile>)>();
        await Assert.That(inputs.Item1.ContainsKey(file.TypeKey)).IsTrue();
        await Assert.That(inputs.Item2.Single()).IsSameReferenceAs(file);
    }

    [Test]
    public async Task ReportsParserErrorsAtTheirSourcePositionAndKeepsValidSiblingFiles()
    {
        var invalid = new MemoryAdditionalText("Engines/Game/Invalid.chunkl",
            "Invalid 0x03043000\n0x001\n int Value\n");
        var valid = new MemoryAdditionalText("Engines/Game/Valid.chunkl",
            "Valid 0x03044000\n0x001\n  int Value\n");
        var result = await Run(CreateDriver(invalid, valid));

        var diagnostic = await Assert.That(result.Diagnostics).HasSingleItem();
        await Assert.That(diagnostic.Id).IsEqualTo("GBXNETGEN101");
        await Assert.That(diagnostic.Severity).IsEqualTo(DiagnosticSeverity.Error);
        await Assert.That(diagnostic.GetMessage()).Contains("Unexpected token");
        await Assert.That(diagnostic.Location.GetLineSpan().Path).IsEqualTo(invalid.Path);
        await Assert.That(diagnostic.Location.GetLineSpan().StartLinePosition).IsEqualTo(new LinePosition(2, 1));
        await Assert.That(invalid.Text!.ToString(diagnostic.Location.SourceSpan)).IsEqualTo("i");
        await Assert.That(GetFiles(result).Single().Syntax.Header.ClassName).IsEqualTo("Valid");
    }

    [Test]
    public async Task ReportsUnreadableAndEmptyFilesWithoutThrowing()
    {
        var unreadable = new MemoryAdditionalText("Engines/Game/Unreadable.chunkl", null);
        var empty = new MemoryAdditionalText("Engines/Game/Empty.chunkl", "");
        var result = await Run(CreateDriver(unreadable, empty));

        await Assert.That(GetFiles(result)).IsEmpty();
        await Assert.That(result.Diagnostics).Contains(x => x.Id == "GBXNETGEN100" &&
            x.Location.GetLineSpan().Path == unreadable.Path);
        var emptyDiagnostics = result.Diagnostics.Where(x => x.Location.GetLineSpan().Path == empty.Path).ToArray();
        await Assert.That(emptyDiagnostics).IsNotEmpty();
        foreach (var diagnostic in emptyDiagnostics)
        {
            await Assert.That(diagnostic.Location.SourceSpan).IsEqualTo(new TextSpan(0, 0));
        }
    }

    [Test]
    public void PropagatesCancellation()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        Assert.Throws<OperationCanceledException>(() => ChunkLParsing.Parse(
            new MemoryAdditionalText("Engines/Game/Example.chunkl", "Example 0x03043000"), cancellation.Token));
    }

    [Test]
    public async Task ReusesUnchangedFileParsingWhenAnotherLayoutChanges()
    {
        var original = new MemoryAdditionalText("Engines/Game/Example.chunkl", "Example 0x03043000");
        var other = new MemoryAdditionalText("Engines/Plug/Other.chunkl", "Other 0x09000000");
        var compilation = CreateCompilation();
        var driver = CreateDriver(original, other).RunGenerators(compilation);
        var first = await Assert.That(driver.GetRunResult().Results).HasSingleItem();
        var otherFile = GetFiles(first).Single(x => x.Path == other.Path);

        driver = driver.ReplaceAdditionalText(original,
            new MemoryAdditionalText(original.Path, "Example 0x03043000\n0x001\n  int Value\n"))
            .RunGenerators(compilation);
        var second = await Assert.That(driver.GetRunResult().Results).HasSingleItem();
        await Assert.That(second.Diagnostics).IsEmpty();
        await Assert.That(GetFiles(second).Single(x => x.Path == other.Path)).IsSameReferenceAs(otherFile);
        await Assert.That(GetFiles(second).Single(x => x.Path == original.Path).Syntax.Chunks).HasSingleItem();
        var parseOutputs = second.TrackedSteps["ChunkLParseResults"].SelectMany(x => x.Outputs).ToArray();
        await Assert.That(parseOutputs).Contains(x => x.Reason == IncrementalStepRunReason.Cached);
        await Assert.That(parseOutputs).Contains(x => x.Reason == IncrementalStepRunReason.Modified);
    }

    [Test]
    public async Task GeneratesEveryRealLayoutWithoutParserOrGenerationDiagnostics()
    {
        var directory = TestPaths.GetEngineDirectory();
        var files = Directory.EnumerateFiles(directory, "*.chunkl", SearchOption.AllDirectories)
            .OrderBy(x => x, StringComparer.Ordinal)
            .Select(path => new MemoryAdditionalText(path, File.ReadAllText(path))).ToArray();
        var result = await Run(CreateDriver(files));
        var parseResults = result.TrackedSteps["ChunkLParseResults"].SelectMany(x => x.Outputs)
            .Select(x => (Parsing.ChunkLParseResult)x.Value!).ToArray();

        await Assert.That(parseResults.Length).IsEqualTo(files.Length);
        await Assert.That(result.Diagnostics).IsEmpty();
        await Assert.That(result.GeneratedSources.Count(x => x.HintName.StartsWith("Engines/", StringComparison.Ordinal))).IsEqualTo(files.Length);
        await Assert.That(parseResults).All(x => x.File is not null ||
            x.Diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error));
        await Assert.That(GetFiles(result).Length).IsEqualTo(parseResults.Count(x => x.File is not null));
        await Assert.That(result.Diagnostics.Length).IsEqualTo(parseResults.Sum(x => x.Diagnostics.Length));
        TestContext.Current!.Output.WriteLine($"Parsed {GetFiles(result).Length}/{files.Length} layouts successfully; " +
            $"reported {result.Diagnostics.Length} parser diagnostics for the current corpus.");
    }

    private static CSharpCompilation CreateCompilation() => CSharpCompilation.Create("ParsingOnly",
        new[] { CSharpSyntaxTree.ParseText("namespace GBX.NET.Engines.Game; public partial class Example;",
            new CSharpParseOptions(LanguageVersion.Preview)) });

    private static GeneratorDriver CreateDriver(params AdditionalText[] files) => CSharpGeneratorDriver.Create(
        new[] { new GbxGenerator().AsSourceGenerator() }, additionalTexts: files,
        driverOptions: new GeneratorDriverOptions(default, trackIncrementalGeneratorSteps: true));

    private static async Task<GeneratorRunResult> Run(GeneratorDriver driver)
    {
        var result = await Assert.That(driver.RunGenerators(CreateCompilation()).GetRunResult().Results).HasSingleItem();
        await Assert.That(result.Exception).IsNull();
        return result;
    }

    private static ImmutableArray<ParsedChunkLFile> GetFiles(GeneratorRunResult result) =>
        (ImmutableArray<ParsedChunkLFile>)result.TrackedSteps["ParsedChunkLFiles"].Single().Outputs.Single().Value!;

    private sealed class MemoryAdditionalText(string path, string? source) : AdditionalText
    {
        public override string Path { get; } = path;
        public SourceText? Text { get; } = source is null ? null : SourceText.From(source);
        public override SourceText? GetText(CancellationToken cancellationToken = default) => Text;
    }
}
