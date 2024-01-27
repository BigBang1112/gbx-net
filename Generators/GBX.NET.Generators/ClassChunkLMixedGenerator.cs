using ChunkL;
using ChunkL.Structure;
using GBX.NET.Generators.Models;
using GBX.NET.Generators.SubGenerators;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Text;

namespace GBX.NET.Generators;

[Generator]
public partial class ClassChunkLMixedGenerator : IIncrementalGenerator
{
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

                var nestedTypeSymbols = symbol?.GetTypeMembers()
                    .ToImmutableDictionary(x => x.Name) ?? ImmutableDictionary<string, INamedTypeSymbol>.Empty;

                var headerChunkDict = new Dictionary<uint, ChunkDataModel>();
                var chunkDict = new Dictionary<uint, ChunkDataModel>();

                foreach (var chunkDef in chunklFile.DataModel.Body.ChunkDefinitions)
                {
                    var fullChunkId = id | chunkDef.Id;

                    if (chunkDef.Properties.TryGetValue("header", out _))
                    {
                        var headerChunkSymbol = nestedTypeSymbols.GetValueOrDefault($"HeaderChunk{fullChunkId:X8}");
                        headerChunkDict.Add(fullChunkId, new ChunkDataModel(fullChunkId, chunkDef, headerChunkSymbol));
                    }
                    else
                    {
                        var chunkSymbol = nestedTypeSymbols.GetValueOrDefault($"Chunk{fullChunkId:X8}");
                        chunkDict.Add(fullChunkId, new ChunkDataModel(fullChunkId, chunkDef, chunkSymbol));
                    }
                }

                foreach (var archiveDef in chunklFile.DataModel.Body.ArchiveDefinitions)
                {

                }

                classModels.Add(new ClassDataModel(name, id, chunklFile.Engine, inherits, symbol, headerChunkDict, chunkDict, []));
                alreadyAdded.Add(name);
            }

            // classes implemented without chunkl files
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

                var nestedTypeSymbols = gbxClass.GetTypeMembers();

                var headerChunkDict = new Dictionary<uint, ChunkDataModel>();
                var chunkDict = new Dictionary<uint, ChunkDataModel>();
                var chunksWithNoId = new List<INamedTypeSymbol>();

                foreach (var nestedSymbol in nestedTypeSymbols)
                {
                    if (nestedSymbol.AllInterfaces.Any(x => x.Name == "IChunk"))
                    {
                        var isHeaderChunk = nestedSymbol.AllInterfaces.Any(x => x.Name == "IHeaderChunk");

                        var chunkId = nestedSymbol.GetAttributes()
                            .FirstOrDefault(x => x.AttributeClass?.Name == "ChunkAttribute")?
                            .ConstructorArguments[0].Value as uint?;

                        if (chunkId is null)
                        {
                            chunksWithNoId.Add(nestedSymbol);
                            continue;
                        }

                        var chunk = new ChunkDataModel(chunkId.Value, ChunkLDefinition: null, nestedSymbol);

                        if (isHeaderChunk)
                        {
                            headerChunkDict.Add(chunkId.Value, chunk);
                        }
                        else
                        {
                            chunkDict.Add(chunkId.Value, chunk);
                        }
                    }

                    // if archive definition
                }

                classModels.Add(new ClassDataModel(gbxClass.Name, id, gbxClass.ContainingNamespace.Name, inherits, gbxClass, headerChunkDict, chunkDict, chunksWithNoId));
            }

            return classModels.ToImmutableArray();
        });

        var classIdContents = context.AdditionalTextsProvider
            .Where(static file =>
            {
                return file.Path.EndsWith("ClassId.txt") && Path.GetDirectoryName(file.Path).EndsWith("Resources");
            })
            .Select((additionalText, cancellationToken) =>
            {
                var text = additionalText.GetText(cancellationToken) ?? throw new Exception("Could not get text from file.");
                return text.ToString();
            });

        var transformedWithClassIdTxt = transformed.Combine(classIdContents.Collect());
        context.RegisterSourceOutput(transformedWithClassIdTxt, ClassManagerGetNameIdSubGenerator.GenerateGetNameSource);
        context.RegisterSourceOutput(transformedWithClassIdTxt, ClassManagerGetNameIdSubGenerator.GenerateGetIdSource);

        context.RegisterSourceOutput(transformed, ClassDataSubGenerator.GenerateSource);
        context.RegisterSourceOutput(transformed, ClassManagerSubGenerator.GenerateSource);
    }
}
