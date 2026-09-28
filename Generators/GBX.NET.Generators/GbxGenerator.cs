using GBX.NET.Generators.Analysis;
using GBX.NET.Generators.Parsing;
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

        // Register a terminal step so both stages run before code generation exists.
        context.RegisterSourceOutput(generationInputs, static (_, _) =>
        {
            // Next: match each parsed layout's TypeKey to the existing type index, then
            // match ChunkXXXXXXXX / HeaderChunkXXXXXXXX,
            // named archives and enums within that class. A nameless archive uses the
            // class's own members. Keep C# types without a matching .chunkl file too.
            // Run ChunkL's local semantic analysis as part of planning, and resolve
            // external types using both parsed layouts and existing implementations.
            //
            // Build a generation plan before emitting source: reuse existing properties
            // and fields independently, respect attributes and declared base types,
            // and compare method signatures (including overloads and explicit interface
            // implementations). An empty partial chunk/archive still needs generated
            // members; a custom Read, Write or ReadWrite can replace only that method.
            // Check constructors, Id, Version and chunk factories in the same way.
            //
            // The index deliberately contains syntax, not resolved symbols. Resolve
            // supported aliases/qualified names and source-declared inheritance in that
            // later matching stage, or report ambiguity rather than assuming a match.
            // Emit only missing declarations into compatible partial types; diagnose
            // incompatible/non-partial declarations instead of generating duplicates.
        });
    }
}
