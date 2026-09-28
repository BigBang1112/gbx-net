using GBX.NET.Generators.Analysis;
using GBX.NET.Generators.Parsing;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace GBX.NET.Generators.Generation;

// Keep the syntax inventory and parsed layouts separate from emission. Both engine
// source and manager registries consume the same identities and validation results.
// A malformed layout is reported independently while valid siblings still generate.
internal sealed class GenerationPlan
{
    public ImmutableDictionary<string, ExistingType> Existing { get; }
    public ImmutableDictionary<string, LayoutModel> Layouts { get; }
    public ImmutableArray<Diagnostic> Diagnostics { get; }

    private GenerationPlan(ImmutableDictionary<string, ExistingType> existing,
        ImmutableDictionary<string, LayoutModel> layouts, ImmutableArray<Diagnostic> diagnostics)
    {
        Existing = existing; 
        Layouts = layouts; 
        Diagnostics = diagnostics;
    }

    public static GenerationPlan Create((ImmutableDictionary<string, ExistingType> Existing,
        ImmutableArray<ParsedChunkLFile> Files) inputs, CancellationToken token)
    {
        var layouts = ImmutableDictionary.CreateBuilder<string, LayoutModel>(StringComparer.Ordinal);
        var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();
        var ids = new HashSet<uint>();

        foreach (var file in inputs.Files.OrderBy(static x => x.TypeKey, StringComparer.Ordinal))
        {
            token.ThrowIfCancellationRequested();

            try
            {
                var layout = new LayoutModel(file, inputs.Existing);

                EngineWriter.RequirePartial(layout.Existing);

                foreach (var chunk in layout.Chunks)
                {
                    EngineWriter.RequirePartial(chunk.Scope.Existing);
                }

                foreach (var archive in layout.Archives.Values)
                {
                    EngineWriter.RequirePartial(archive.Existing);
                }

                if (layouts.ContainsKey(layout.Name))
                {
                    throw new InvalidOperationException("Ambiguous class name: " + layout.Name);
                }

                if (!ids.Add(layout.Id))
                {
                    throw new InvalidOperationException($"Duplicate class ID: 0x{layout.Id:X8}");
                }

                layouts.Add(layout.Name, layout);
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                diagnostics.Add(EngineWriter.Diagnostic(file, exception));
            }
        }
        
        return new GenerationPlan(inputs.Existing, layouts.ToImmutable(), diagnostics.ToImmutable());
    }
}
