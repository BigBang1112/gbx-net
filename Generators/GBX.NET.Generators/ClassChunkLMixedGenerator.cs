using ChunkL;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace GBX.NET.Generators;

public abstract class ClassChunkLMixedGenerator : IIncrementalGenerator
{
    protected record ChunkLFile(ChunkLDataModel DataModel, string Engine);
    protected record ClassDataModel(
        string Name,
        uint Id,
        string Engine,
        string? Inherits,
        INamedTypeSymbol? TypeSymbol);

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

        var combined = chunklFiles.Collect()
            .Combine(gbxClasses.Collect());

        var transformed = combined.Select((pair, token) =>
        {
            var (chunklFiles, gbxClasses) = pair;

            var classModels = new List<ClassDataModel>();
            var alreadyAdded = new HashSet<string>();

            var gbxClassDict = gbxClasses.ToImmutableDictionary(x => x.Name);

            foreach (var chunklFile in chunklFiles)
            {
                var symbol = gbxClassDict.GetValueOrDefault(chunklFile.DataModel.Header.Name);

                var name = chunklFile.DataModel.Header.Name;
                var id = chunklFile.DataModel.Header.Id;

                _ = chunklFile.DataModel.Header.Features.TryGetValue("inherits", out var inherits);
                inherits ??= symbol?.BaseType?.Name ?? "CMwNod";
                if (inherits == nameof(Object)) inherits = "CMwNod";
                if (name == "CMwNod") inherits = null;

                classModels.Add(new ClassDataModel(name, id, chunklFile.Engine, inherits, symbol));
                alreadyAdded.Add(name);
            }

            foreach (var gbxClass in gbxClasses)
            {
                if (alreadyAdded.Contains(gbxClass.Name))
                {
                    continue;
                }

                var id = (gbxClass.GetAttributes()
                    .FirstOrDefault(x => x.AttributeClass?.Name == "ClassAttribute")?
                    .ConstructorArguments[0].Value as uint?)
                    .GetValueOrDefault();

                var inherits = gbxClass.BaseType?.Name ?? "CMwNod";
                if (inherits == nameof(Object)) inherits = "CMwNod";
                if (gbxClass.Name == "CMwNod") inherits = null;

                classModels.Add(new ClassDataModel(gbxClass.Name, id, gbxClass.ContainingNamespace.Name, inherits, gbxClass));
            }

            return classModels.ToImmutableArray();
        });

        Initialize(context, transformed);
    }

    protected abstract void Initialize(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<ImmutableArray<ClassDataModel>> transformed);
}
