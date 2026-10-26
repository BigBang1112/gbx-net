using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace GBX.NET.Generators.Tests;

public partial class GenerationTests
{
    [Test]
    public async Task Generate_GameInterfaces_MemberBodyEditKeepsGeneratedSources()
    {
        var (result, compilation) = RunEdit(
            """
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public int Custom { get => 2; }
                    public int Twice()
                    {
                        return Value * 2;
                    }
                }
            }
            """,
            """
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public int Custom { get => 3; }
                    public int Twice()
                    {
                        // Changed body.
                        return Value + Value;
                    }
                }
            }
            """);

        // The plan keeps syntax from the first compilation. Game symbols must still
        // read the declarations of the edited one.
        await Assert.That(result.Exception).IsNull();
        await Assert.That(result.TrackedSteps["PlannedEngineTypes"].SelectMany(static s => s.Outputs)
            .All(static o => o.Reason == IncrementalStepRunReason.Cached)).IsTrue();
        await Assert.That(result.TrackedSteps["GameSymbols"].SelectMany(static s => s.Outputs)
            .All(static o => o.Reason == IncrementalStepRunReason.Unchanged)).IsTrue();
        await Assert.That(result.TrackedOutputSteps["SourceOutput"].SelectMany(static s => s.Outputs)
            .All(static o => o.Reason is IncrementalStepRunReason.Cached or IncrementalStepRunReason.Unchanged)).IsTrue();
        await AssertSameAsFreshRun(result, compilation);
    }

    [Test]
    public async Task Generate_GameInterfaces_MemberSignatureEditRegenerates()
    {
        var (result, compilation) = RunEdit(
            """
            namespace GBX.NET.Engines.Game
            {
                public partial class Example { }
            }
            """,
            """
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public int Value { get; set; }
                }
            }
            """);

        await Assert.That(result.Exception).IsNull();
        await Assert.That(result.TrackedSteps["PlannedEngineTypes"].SelectMany(static s => s.Outputs)
            .All(static o => o.Reason == IncrementalStepRunReason.Modified)).IsTrue();
        await Assert.That(result.GeneratedSources.Single(static s => s.HintName == "Engines/Game/Example.g.cs").SourceText.ToString())
            .DoesNotContain("public int Value");
        await AssertSameAsFreshRun(result, compilation);
    }

    private static readonly CSharpParseOptions incrementalOptions = new(LanguageVersion.Preview);

    private static readonly Text incrementalLayout = new("Engines/Game/Example.chunkl", "Example 0x03043000\n0x001 [TM10]\n  int Value\n");

    // Runs the generator, replaces the handwritten partial and runs it again on the same driver.
    private static (GeneratorRunResult Result, Compilation Compilation) RunEdit(string before, string after)
    {
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Select(static x => MetadataReference.CreateFromFile(x));
        var partial = CSharpSyntaxTree.ParseText(before, incrementalOptions);
        var compilation = CSharpCompilation.Create("Fixture", [CSharpSyntaxTree.ParseText(Support + GameSupport, incrementalOptions), partial],
            references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        GeneratorDriver driver = CSharpGeneratorDriver.Create([new GbxGenerator().AsSourceGenerator()], additionalTexts: [incrementalLayout],
            parseOptions: incrementalOptions, driverOptions: new GeneratorDriverOptions(IncrementalGeneratorOutputKind.None, trackIncrementalGeneratorSteps: true));
        driver = driver.RunGenerators(compilation);

        var edited = compilation.ReplaceSyntaxTree(partial, CSharpSyntaxTree.ParseText(after, incrementalOptions));
        driver = driver.RunGenerators(edited);
        return (driver.GetRunResult().Results.Single(), edited);
    }

    private static async Task AssertSameAsFreshRun(GeneratorRunResult result, Compilation compilation)
    {
        var fresh = CSharpGeneratorDriver.Create([new GbxGenerator().AsSourceGenerator()], additionalTexts: [incrementalLayout], parseOptions: incrementalOptions)
            .RunGenerators(compilation).GetRunResult().Results.Single();
        await Assert.That(result.GeneratedSources.Select(static s => s.HintName + "\n" + s.SourceText))
            .IsEquivalentTo(fresh.GeneratedSources.Select(static s => s.HintName + "\n" + s.SourceText));
        await Assert.That(result.Diagnostics.Select(static d => d.ToString()))
            .IsEquivalentTo(fresh.Diagnostics.Select(static d => d.ToString()));
    }
}
