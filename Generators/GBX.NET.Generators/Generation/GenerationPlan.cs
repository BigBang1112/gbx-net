using GBX.NET.Generators.Analysis;
using GBX.NET.Generators.Parsing;

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
        var ids = new HashSet<(uint Id, string? Game)>();

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

                foreach (var game in layout.Games.Length == 0 ? [null] : layout.Games.Select(static x => (string?)x))
                {
                    if (!ids.Add((layout.Id, game)))
                    {
                        throw new InvalidOperationException($"Duplicate class ID: 0x{layout.Id:X8}");
                    }
                }

                layouts.Add(layout.Name, layout);
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                diagnostics.Add(EngineWriter.Diagnostic(file, exception));
            }
        }

        foreach (var layout in layouts.Values)
        {
            var visited = new HashSet<string>(StringComparer.Ordinal) { layout.Name };
            for (var current = layout; current.BaseName is not null && layouts.TryGetValue(current.BaseName, out current) && visited.Add(current.Name);)
            {
                current.Descendants.Add(layout);
            }
        }

        return new GenerationPlan(inputs.Existing, layouts.ToImmutable(),
            diagnostics.Count == diagnostics.Capacity ? diagnostics.MoveToImmutable() : diagnostics.ToImmutable());
    }
}
