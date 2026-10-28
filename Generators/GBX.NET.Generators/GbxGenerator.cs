using GBX.NET.Generators.Analysis;
using GBX.NET.Generators.Parsing;
using GBX.NET.Generators.Generation;

namespace GBX.NET.Generators;

[Generator]
public sealed class GbxGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var buildersOnly = context.BuildersOnly().WithTrackingName("BuildersOnly");

        var librarySources = context.AdditionalTextsProvider
            .Where(static file => file.Path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
            .Combine(context.ParseOptionsProvider)
            .Select(static (input, token) => CSharpSyntaxTree.ParseText(input.Left.GetText(token)!,
                (CSharpParseOptions)input.Right, input.Left.Path, token))
            .Collect()
            .WithTrackingName("LibrarySources");

        var existingTypes = context.SyntaxProvider.CreateSyntaxProvider(
                static (node, _) => ExistingTypeAnalysis.IsCandidate(node),
                static (syntaxContext, token) => ExistingTypeAnalysis.Read(syntaxContext.Node, token))
            .Where(static declaration => declaration is not null)
            .Select(static (declaration, _) => declaration!)
            .Collect()
            .Combine(librarySources)
            .Select(static (inputs, token) => ExistingTypeAnalysis.Merge(inputs.Left.AddRange(inputs.Right
                .SelectMany(tree => tree.GetRoot(token).DescendantNodes().Where(ExistingTypeAnalysis.IsCandidate))
                .Select(node => ExistingTypeAnalysis.Read(node, token)).Where(static declaration => declaration is not null)
                .Select(static declaration => declaration!)), token))
            .WithComparer(ExistingTypeAnalysis.Comparer)
            .WithTrackingName("ExistingEngineTypes");

        var parseResults = context.AdditionalTextsProvider
            .Where(static file => file.Path.EndsWith(".chunkl", StringComparison.OrdinalIgnoreCase))
            .Select(static (file, token) => ChunkLParsing.Parse(file, token))
            .WithTrackingName("ChunkLParseResults");

        context.RegisterLibraryOutput(parseResults, buildersOnly, static (sourceContext, result) =>
        {
            foreach (var diagnostic in result.Diagnostics)
            {
                sourceContext.CancellationToken.ThrowIfCancellationRequested();
                sourceContext.ReportDiagnostic(diagnostic);
            }
        });

        var chunklFiles = parseResults
            .Where(static result => result.File is not null)
            .Select(static (result, _) => result.File!)
            .Collect()
            .WithTrackingName("ParsedChunkLFiles");

        var generationInputs = existingTypes.Combine(chunklFiles)
            .WithTrackingName("GenerationInputs");

        var generationPlan = generationInputs.Select(static (inputs, token) => GenerationPlan.Create(inputs, token))
            .WithTrackingName("PlannedEngineTypes");
        context.RegisterLibraryOutput(generationPlan, buildersOnly, EngineWriter.Generate);
        var gameSymbols = generationPlan.Combine(librarySources).Combine(context.CompilationProvider)
            .Select(static (input, _) => GameSymbols.Create(input.Left.Left, input.Right, input.Left.Right))
            .WithTrackingName("GameSymbols");
        context.RegisterSourceOutput(generationPlan.Combine(gameSymbols).Combine(buildersOnly),
            static (sourceContext, input) => GameVersionWriter.Generate(sourceContext, input.Left.Left, input.Left.Right, input.Right));

        var resources = context.AdditionalTextsProvider
            .Where(static file => file.Path.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            .Select(static (file, token) => new KeyValuePair<string, string>(System.IO.Path.GetFileName(file.Path), file.GetText(token)?.ToString() ?? ""))
            .Collect();
        context.RegisterLibraryOutput(generationPlan.Combine(resources), buildersOnly, static (sourceContext, input) =>
        {
            if (!input.Right.Any(static x => x.Key == "CollectionId.txt")) return;
            ManagerWriter.Generate(sourceContext, input.Left.Layouts, input.Right);
        });
    }
}
