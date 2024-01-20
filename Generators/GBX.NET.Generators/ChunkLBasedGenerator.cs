using ChunkL;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace GBX.NET.Generators;

public abstract class ChunkLBasedGenerator : IIncrementalGenerator
{
    protected record ChunkLFile(ChunkLDataModel DataModel, string Engine);
    protected record ClassDataModel(ChunkLFile ChunkL, INamedTypeSymbol? TypeSymbol);

    public virtual void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var chunklFiles = context.AdditionalTextsProvider
            .Where(static file =>
            {
                return file.Path.EndsWith(".chunkl", StringComparison.OrdinalIgnoreCase);
            })
            .Select(static (chunklFile, token) =>
            {
                if (chunklFile.GetText(token)?.ToString() is not string chunklText)
                {
                    throw new Exception("Could not get text from file.");
                }

                using var reader = new StringReader(chunklText);

                return new ChunkLFile(
                    DataModel: ChunkLSerializer.Deserialize(reader),
                    Engine: Path.GetFileName(Path.GetDirectoryName(chunklFile.Path)));
            });

        var gbxClasses = context.CompilationProvider
            .Select(static (compilation, token) =>
            {
                var enginesNamespace = compilation.GlobalNamespace.GetNamespaceMembers()
                    .FirstOrDefault(x => x.Name == "GBX")
                    .GetNamespaceMembers()
                    .FirstOrDefault(x => x.Name == "NET")
                    .GetNamespaceMembers()
                    .FirstOrDefault(x => x.Name == "Engines");

                return enginesNamespace.GetNamespaceMembers()
                    .SelectMany(x => x.GetTypeMembers())
                    .ToImmutableDictionary(x => x.Name);
            });

        var combined = chunklFiles.Combine(gbxClasses);

        var transformed = combined.Select((pair, token) =>
        {
            var (chunklFile, gbxClasses) = pair;

            var typeSymbol = gbxClasses.GetValueOrDefault(chunklFile.DataModel.Header.Name);

            return new ClassDataModel(chunklFile, typeSymbol);
        });

        Initialize(context, transformed);
    }

    protected abstract void Initialize(IncrementalGeneratorInitializationContext context, IncrementalValuesProvider<ClassDataModel> transformed);
}
