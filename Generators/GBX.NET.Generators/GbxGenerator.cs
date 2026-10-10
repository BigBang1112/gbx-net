using GBX.NET.Generators.Analysis;
using GBX.NET.Generators.Parsing;
using GBX.NET.Generators.Generation;
using Microsoft.CodeAnalysis;

namespace GBX.NET.Generators;

[Generator]
public sealed class GbxGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var existingTypes = context.SyntaxProvider.CreateSyntaxProvider(
                static (node, _) => ExistingTypeAnalysis.IsCandidate(node),
                static (syntaxContext, token) => ExistingTypeAnalysis.Read(syntaxContext.Node, token))
            .Where(static declaration => declaration is not null)
            .Select(static (declaration, _) => declaration!)
            .Collect()
            .Select(static (declarations, token) => ExistingTypeAnalysis.Merge(declarations, token))
            .WithTrackingName("ExistingEngineTypes");

        var parseResults = context.AdditionalTextsProvider
            .Where(static file => file.Path.EndsWith(".chunkl", StringComparison.OrdinalIgnoreCase))
            .Select(static (file, token) => ChunkLParsing.Parse(file, token))
            .WithTrackingName("ChunkLParseResults");

        context.RegisterSourceOutput(parseResults, static (sourceContext, result) =>
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
        context.RegisterSourceOutput(generationPlan, EngineWriter.Generate);

        var resources = context.AdditionalTextsProvider
            .Where(static file => file.Path.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            .Select(static (file, token) => new KeyValuePair<string, string>(System.IO.Path.GetFileName(file.Path), file.GetText(token)?.ToString() ?? ""))
            .Collect();
        context.RegisterSourceOutput(generationPlan.Combine(resources), static (sourceContext, input) =>
        {
            if (!input.Right.Any(static x => x.Key == "CollectionId.txt")) return;
            ManagerWriter.Generate(sourceContext, input.Left.Layouts, input.Right);
        });
    }
}
