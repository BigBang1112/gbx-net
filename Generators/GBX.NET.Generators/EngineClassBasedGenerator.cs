using ChunkL;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace GBX.NET.Generators;

public abstract class EngineClassBasedGenerator : IIncrementalGenerator
{
    protected record ChunkLFile(ChunkLDataModel DataModel, string Engine);
    protected record ClassDataModel(INamedTypeSymbol TypeSymbol, uint Id, ChunkLFile? ChunkL);

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
            })
            .Collect()
            .Select(static (chunkLFiles, token) =>
            {
                return chunkLFiles.ToImmutableDictionary(x => x.DataModel.Header.Name);
            });

        var gbxClasses = context.CompilationProvider
            .SelectMany(static (compilation, token) =>
            {
                var enginesNamespace = compilation.GlobalNamespace.GetNamespaceMembers()
                    .FirstOrDefault(x => x.Name == "GBX")
                    .GetNamespaceMembers()
                    .FirstOrDefault(x => x.Name == "NET")
                    .GetNamespaceMembers()
                    .FirstOrDefault(x => x.Name == "Engines");

                return enginesNamespace.GetNamespaceMembers()
                    .SelectMany(x => x.GetTypeMembers());
            });

        var combined = gbxClasses.Combine(chunklFiles);

        var transformed = combined.Select((pair, token) =>
        {
            var (gbxClass, chunkLFiles) = pair;
            var chunkl = chunkLFiles.GetValueOrDefault(gbxClass.Name);

            var id = (gbxClass.GetAttributes()
                .FirstOrDefault(x => x.AttributeClass?.Name == "ChunkLClassAttribute")?
                .ConstructorArguments[0].Value as uint?)
                .GetValueOrDefault(chunkl?.DataModel.Header.Id ?? 0);


            return new ClassDataModel(gbxClass, id, chunkl);
        });

        Initialize(context, transformed);
    }

    protected abstract void Initialize(IncrementalGeneratorInitializationContext context, IncrementalValuesProvider<ClassDataModel> transformed);
}