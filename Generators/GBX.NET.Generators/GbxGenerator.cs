using GBX.NET.Generators.Analysis;
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

        // Register a terminal step so the analysis runs even before code generation exists.
        context.RegisterSourceOutput(existingTypes, static (_, _) =>
        {
            // Next: parse .chunkl AdditionalTexts and combine them with this index. Match
            // engine namespace + class name, then ChunkXXXXXXXX / HeaderChunkXXXXXXXX,
            // named archives and enums within that class. A nameless archive uses the
            // class's own members. Keep C# types without a matching .chunkl file too.
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
