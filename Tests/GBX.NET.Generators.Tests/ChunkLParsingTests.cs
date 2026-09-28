using ChunkL.Syntax;
using GBX.NET.Generators.Analysis;
using GBX.NET.Generators.Parsing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Xunit;
using Xunit.Abstractions;

namespace GBX.NET.Generators.Tests;

public class ChunkLParsingTests(ITestOutputHelper output)
{
    [Fact]
    public void ParsesLayoutsAndCombinesThemWithExistingTypes()
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
        var result = Run(CreateDriver(additionalText, new MemoryAdditionalText("ClassId.txt", "ignored")));
        Assert.Empty(result.Diagnostics);
        var file = Assert.Single(GetFiles(result));
        Assert.Equal(additionalText.Path, file.Path);
        Assert.Same(additionalText.Text, file.Source);
        Assert.Equal("Game", file.Engine);
        Assert.Equal("GBX.NET.Engines.Game.Example", file.TypeKey);
        Assert.Equal("0x03043000", file.Syntax.Header.ClassId);
        Assert.Equal("CMwNod", Assert.Single(file.Syntax.ClassAttributes).Value);
        Assert.Equal(2, file.Syntax.Chunks.Count);
        Assert.Equal("header", Assert.Single(file.Syntax.Chunks[0].Attributes!.Entries).Name);
        Assert.Equal("skippable", Assert.Single(file.Syntax.Chunks[1].Attributes!.Entries).Name);
        Assert.Contains(file.Syntax.Chunks[1].Body, x => x is FieldDeclaration { Name: "Keys" });
        Assert.Equal(2, file.Syntax.Archives.Count);
        Assert.Equal("Key", file.Syntax.Archives[0].Name);
        Assert.True(string.IsNullOrEmpty(file.Syntax.Archives[1].Name));
        Assert.Equal("Kind", Assert.Single(file.Syntax.Enums).Name);

        var inputs = Assert.IsType<(ImmutableDictionary<string, ExistingType>, ImmutableArray<ParsedChunkLFile>)>(
            Assert.Single(Assert.Single(result.TrackedSteps["GenerationInputs"]).Outputs).Value);
        Assert.True(inputs.Item1.ContainsKey(file.TypeKey));
        Assert.Same(file, Assert.Single(inputs.Item2));
    }

    [Fact]
    public void ReportsParserErrorsAtTheirSourcePositionAndKeepsValidSiblingFiles()
    {
        var invalid = new MemoryAdditionalText("Engines/Game/Invalid.chunkl",
            "Invalid 0x03043000\n0x001\n int Value\n");
        var valid = new MemoryAdditionalText("Engines/Game/Valid.chunkl",
            "Valid 0x03044000\n0x001\n  int Value\n");
        var result = Run(CreateDriver(invalid, valid));

        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal("GBXNETGEN101", diagnostic.Id);
        Assert.Equal(DiagnosticSeverity.Error, diagnostic.Severity);
        Assert.Contains("Unexpected token", diagnostic.GetMessage());
        Assert.Equal(invalid.Path, diagnostic.Location.GetLineSpan().Path);
        Assert.Equal(new LinePosition(2, 1), diagnostic.Location.GetLineSpan().StartLinePosition);
        Assert.Equal("i", invalid.Text!.ToString(diagnostic.Location.SourceSpan));
        Assert.Equal("Valid", Assert.Single(GetFiles(result)).Syntax.Header.ClassName);
    }

    [Fact]
    public void ReportsUnreadableAndEmptyFilesWithoutThrowing()
    {
        var unreadable = new MemoryAdditionalText("Engines/Game/Unreadable.chunkl", null);
        var empty = new MemoryAdditionalText("Engines/Game/Empty.chunkl", "");
        var result = Run(CreateDriver(unreadable, empty));

        Assert.Empty(GetFiles(result));
        Assert.Contains(result.Diagnostics, x => x.Id == "GBXNETGEN100" &&
            x.Location.GetLineSpan().Path == unreadable.Path);
        var emptyDiagnostics = result.Diagnostics.Where(x => x.Location.GetLineSpan().Path == empty.Path).ToArray();
        Assert.NotEmpty(emptyDiagnostics);
        Assert.All(emptyDiagnostics, x => Assert.Equal(new TextSpan(0, 0), x.Location.SourceSpan));
    }

    [Fact]
    public void PropagatesCancellation()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        Assert.Throws<OperationCanceledException>(() => ChunkLParsing.Parse(
            new MemoryAdditionalText("Engines/Game/Example.chunkl", "Example 0x03043000"), cancellation.Token));
    }

    [Fact]
    public void ReusesUnchangedFileParsingWhenAnotherLayoutChanges()
    {
        var original = new MemoryAdditionalText("Engines/Game/Example.chunkl", "Example 0x03043000");
        var other = new MemoryAdditionalText("Engines/Plug/Other.chunkl", "Other 0x09000000");
        var compilation = CreateCompilation();
        var driver = CreateDriver(original, other).RunGenerators(compilation);
        var first = Assert.Single(driver.GetRunResult().Results);
        var otherFile = GetFiles(first).Single(x => x.Path == other.Path);

        driver = driver.ReplaceAdditionalText(original,
            new MemoryAdditionalText(original.Path, "Example 0x03043000\n0x001\n  int Value\n"))
            .RunGenerators(compilation);
        var second = Assert.Single(driver.GetRunResult().Results);
        Assert.Empty(second.Diagnostics);
        Assert.Same(otherFile, GetFiles(second).Single(x => x.Path == other.Path));
        Assert.Single(GetFiles(second).Single(x => x.Path == original.Path).Syntax.Chunks);
        var parseOutputs = second.TrackedSteps["ChunkLParseResults"].SelectMany(x => x.Outputs).ToArray();
        Assert.Contains(parseOutputs, x => x.Reason == IncrementalStepRunReason.Cached);
        Assert.Contains(parseOutputs, x => x.Reason == IncrementalStepRunReason.Modified);
    }

    [Fact]
    public void GeneratesEveryRealLayoutWithoutParserOrGenerationDiagnostics()
    {
        var directory = Path.GetFullPath(Path.Combine(GetTestDirectory(), "../../Src/GBX.NET/Engines"));
        var files = Directory.EnumerateFiles(directory, "*.chunkl", SearchOption.AllDirectories)
            .OrderBy(x => x, StringComparer.Ordinal)
            .Select(path => new MemoryAdditionalText(path, File.ReadAllText(path))).ToArray();
        var result = Run(CreateDriver(files));
        var parseResults = result.TrackedSteps["ChunkLParseResults"].SelectMany(x => x.Outputs)
            .Select(x => Assert.IsType<Parsing.ChunkLParseResult>(x.Value)).ToArray();

        Assert.Equal(files.Length, parseResults.Length);
        Assert.Empty(result.Diagnostics);
        Assert.Equal(files.Length, result.GeneratedSources.Count(x => x.HintName.StartsWith("Engines/", StringComparison.Ordinal)));
        Assert.All(parseResults, x => Assert.True(x.File is not null ||
            x.Diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error)));
        Assert.Equal(parseResults.Count(x => x.File is not null), GetFiles(result).Length);
        Assert.Equal(parseResults.Sum(x => x.Diagnostics.Length), result.Diagnostics.Length);
        output.WriteLine($"Parsed {GetFiles(result).Length}/{files.Length} layouts successfully; " +
            $"reported {result.Diagnostics.Length} parser diagnostics for the current corpus.");
    }

    private static string GetTestDirectory([CallerFilePath] string path = "") => Path.GetDirectoryName(path)!;

    private static CSharpCompilation CreateCompilation() => CSharpCompilation.Create("ParsingOnly",
        new[] { CSharpSyntaxTree.ParseText("namespace GBX.NET.Engines.Game; public partial class Example;",
            new CSharpParseOptions(LanguageVersion.Preview)) });

    private static GeneratorDriver CreateDriver(params AdditionalText[] files) => CSharpGeneratorDriver.Create(
        new[] { new GbxGenerator().AsSourceGenerator() }, additionalTexts: files,
        driverOptions: new GeneratorDriverOptions(default, trackIncrementalGeneratorSteps: true));

    private static GeneratorRunResult Run(GeneratorDriver driver)
    {
        var result = Assert.Single(driver.RunGenerators(CreateCompilation()).GetRunResult().Results);
        Assert.Null(result.Exception);
        return result;
    }

    private static ImmutableArray<ParsedChunkLFile> GetFiles(GeneratorRunResult result) =>
        Assert.IsType<ImmutableArray<ParsedChunkLFile>>(
            Assert.Single(Assert.Single(result.TrackedSteps["ParsedChunkLFiles"]).Outputs).Value);

    private sealed class MemoryAdditionalText(string path, string? source) : AdditionalText
    {
        public override string Path { get; } = path;
        public SourceText? Text { get; } = source is null ? null : SourceText.From(source);
        public override SourceText? GetText(CancellationToken cancellationToken = default) => Text;
    }
}
