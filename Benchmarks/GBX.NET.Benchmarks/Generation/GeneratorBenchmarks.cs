using BenchmarkDotNet.Attributes;
using GBX.NET.Generators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace GBX.NET.Benchmarks.Generation;

[MemoryDiagnoser]
[BenchmarkCategory("Generator")]
public class GeneratorBenchmarks
{
    private CSharpCompilation compilation = null!;
    private GeneratorDriver freshDriver = null!;
    private GeneratorDriver cachedDriver = null!;
    private GeneratorDriver changedLayoutDriver = null!;

    [GlobalSetup]
    public void Setup()
    {
        var root = Path.Combine(AppContext.BaseDirectory, "GeneratorInputs");
        var options = new CSharpParseOptions(LanguageVersion.CSharp14, preprocessorSymbols:
        [
            "TRACE", "RELEASE", "NET", "NET10_0", "NETCOREAPP", "NET5_0_OR_GREATER",
            "NET6_0_OR_GREATER", "NET7_0_OR_GREATER", "NET8_0_OR_GREATER",
            "NET9_0_OR_GREATER", "NET10_0_OR_GREATER", "NETCOREAPP1_0_OR_GREATER",
            "NETCOREAPP1_1_OR_GREATER", "NETCOREAPP2_0_OR_GREATER", "NETCOREAPP2_1_OR_GREATER",
            "NETCOREAPP2_2_OR_GREATER", "NETCOREAPP3_0_OR_GREATER", "NETCOREAPP3_1_OR_GREATER"
        ]);
        var sources = Directory.GetFiles(Path.Combine(root, "Source"), "*.cs", SearchOption.AllDirectories)
            .Order(StringComparer.Ordinal)
            .Select(path => CSharpSyntaxTree.ParseText(File.ReadAllText(path), options, path))
            .ToArray();
        var layouts = Directory.GetFiles(Path.Combine(root, "Engines"), "*.chunkl", SearchOption.AllDirectories)
            .Order(StringComparer.Ordinal)
            .Select(path => new GeneratorAdditionalText(path, File.ReadAllText(path)))
            .ToArray();
        var resources = Directory.GetFiles(Path.Combine(root, "Resources"), "*.txt")
            .Order(StringComparer.Ordinal)
            .Select(path => new GeneratorAdditionalText(path, File.ReadAllText(path)));

        // GbxGenerator analyzes declarations syntactically, so metadata references and
        // the other generators' output are unnecessary for this generation-only workload.
        compilation = CSharpCompilation.Create("GBX.NET", sources,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
        freshDriver = CSharpGeneratorDriver.Create([new GbxGenerator().AsSourceGenerator()],
            additionalTexts: layouts.Concat(resources), parseOptions: options);
        cachedDriver = freshDriver.RunGenerators(compilation);
        Validate(cachedDriver);

        var original = layouts.Single(file => System.IO.Path.GetFileName(file.Path) == "CGameCtnChallenge.chunkl");
        var changed = new GeneratorAdditionalText(original.Path,
            original.GetText().ToString() + "\n0xFFE\n  int BenchmarkValue\n");
        changedLayoutDriver = cachedDriver.ReplaceAdditionalText(original, changed);
        var changedResult = changedLayoutDriver.RunGenerators(compilation);
        Validate(changedResult);
        if (!changedResult.GetRunResult().GeneratedTrees.Any(tree => tree.GetText().ToString().Contains("BenchmarkValue", StringComparison.Ordinal)))
        {
            throw new InvalidOperationException("The layout edit did not produce the benchmark property.");
        }
    }

    [Benchmark]
    public GeneratorDriver FreshGeneration() => freshDriver.RunGenerators(compilation);

    [Benchmark]
    public GeneratorDriver CachedGeneration() => cachedDriver.RunGenerators(compilation);

    [Benchmark]
    public GeneratorDriver ChangedLayoutGeneration() => changedLayoutDriver.RunGenerators(compilation);

    private static void Validate(GeneratorDriver driver)
    {
        var result = driver.GetRunResult();
        var errors = result.Diagnostics.Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error).ToArray();
        if (errors.Length > 0 || result.Results.Any(generator => generator.Exception is not null) || result.GeneratedTrees.IsEmpty)
        {
            throw new InvalidOperationException("Generator benchmark setup failed: " + string.Join(Environment.NewLine, errors.Select(error => error.ToString())));
        }
    }
}
