using ChunkL;
using GBX.NET.Generators.Models;
using GBX.NET.Generators.SubGenerators;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Diagnostics;

namespace GBX.NET.Generators;

[Generator]
public partial class ClassChunkLMixedGenerator : IIncrementalGenerator
{
    private const bool Debug = false;

    public virtual void Initialize(IncrementalGeneratorInitializationContext context)
    {
        if (Debug && !Debugger.IsAttached)
        {
            Debugger.Launch();
        }

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

                var classList = ImmutableList.CreateBuilder<INamedTypeSymbol>();

                foreach (var engineNamespace in enginesNamespace.GetNamespaceMembers())
                {
                    foreach (var type in engineNamespace.GetTypeMembers())
                    {
                        classList.Add(type);
                    }
                }

                return classList.ToImmutable();
            });

        var combined = chunklFiles.Collect()
            .Combine(gbxClasses.Collect());

        var transformed = combined.Select((pair, token) =>
        {
            var (chunklFiles, gbxClasses) = pair;

            var classModels = ImmutableDictionary.CreateBuilder<string, ClassDataModel>();
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
                    .ToImmutableDictionary(x => x.Name + (x.IsGenericType ? "<>" : "")) ?? ImmutableDictionary<string, INamedTypeSymbol>.Empty;

                var headerChunkDict = ImmutableDictionary.CreateBuilder<uint, ChunkDataModel>();
                var chunkDict = ImmutableDictionary.CreateBuilder<uint, ChunkDataModel>();
                var archiveDict = ImmutableDictionary.CreateBuilder<string, ArchiveDataModel>();

                foreach (var chunkDef in chunklFile.DataModel.Body.ChunkDefinitions)
                {
                    var fullChunkId = chunkDef.Id > 0xFFF ? chunkDef.Id : id | chunkDef.Id;
                    var isSkippable = chunkDef.Properties.ContainsKey("skippable");
                    var description = chunkDef.Description;

                    if (chunkDef.Properties.ContainsKey("header"))
                    {
                        var headerChunkSymbol = nestedTypeSymbols.GetValueOrDefault($"HeaderChunk{fullChunkId:X8}");
                        headerChunkDict.Add(fullChunkId, new ChunkDataModel(fullChunkId, description, isSkippable, chunkDef, headerChunkSymbol));
                    }
                    else
                    {
                        var chunkSymbol = nestedTypeSymbols.GetValueOrDefault($"Chunk{fullChunkId:X8}");
                        chunkDict.Add(fullChunkId, new ChunkDataModel(fullChunkId, description, isSkippable, chunkDef, chunkSymbol));
                    }
                }

                var namelessArchive = default(ArchiveDataModel);

                foreach (var archiveDef in chunklFile.DataModel.Body.ArchiveDefinitions)
                {
                    if (string.IsNullOrEmpty(archiveDef.Name))
                    {
                        namelessArchive = new ArchiveDataModel(archiveDef, symbol);
                        continue;
                    }

                    var archiveSymbol = nestedTypeSymbols.GetValueOrDefault(archiveDef.Name);
                    archiveDict.Add(archiveDef.Name, new ArchiveDataModel(archiveDef, archiveSymbol));
                }

                var enums = chunklFile.DataModel.Body.EnumDefinitions
                    .ToImmutableDictionary(x => x.Name, def => new EnumDataModel(def, nestedTypeSymbols.GetValueOrDefault(def.Name)));

                var isAbstract = chunklFile.DataModel.Header.Features.TryGetValue("abstract", out _) || symbol?.IsAbstract == true;

                classModels.Add(name, new ClassDataModel(
                    name,
                    id,
                    chunklFile.Engine,
                    inherits,
                    chunklFile.DataModel.Header.Description,
                    symbol,
                    headerChunkDict.ToImmutable(),
                    chunkDict.ToImmutable(),
                    ImmutableList<INamedTypeSymbol>.Empty,
                    namelessArchive,
                    archiveDict.ToImmutable(),
                    enums,
                    isAbstract));
                alreadyAdded.Add(name);
            }

            // classes implemented without chunkl files
            foreach (var gbxClass in gbxClasses)
            {
                if (alreadyAdded.Contains(gbxClass.Name))
                {
                    continue;
                }

                var id = gbxClass.GetAttributes()
                    .FirstOrDefault(x => x.AttributeClass?.Name == "ClassAttribute")?
                    .ConstructorArguments[0].Value as uint?;

                var inherits = gbxClass.BaseType?.Name ?? "CMwNod";
                if (inherits == nameof(Object)) inherits = "CMwNod";
                if (gbxClass.Name == "CMwNod") inherits = null;

                var nestedTypeSymbols = gbxClass.GetTypeMembers();

                var headerChunkDict = ImmutableDictionary.CreateBuilder<uint, ChunkDataModel>();
                var chunkDict = ImmutableDictionary.CreateBuilder<uint, ChunkDataModel>();
                var chunksWithNoId = ImmutableList.CreateBuilder<INamedTypeSymbol>();
                var archives = ImmutableDictionary.CreateBuilder<string, ArchiveDataModel>();

                var isNamelessArchive = gbxClass.AllInterfaces.Any(x => x.Name is "IReadable" or "IWritable" or "IReadableWritable");

                foreach (var nestedSymbol in nestedTypeSymbols)
                {
                    if (nestedSymbol.AllInterfaces.Any(x => x.Name == "IChunk"))
                    {
                        var isHeaderChunk = nestedSymbol.AllInterfaces.Any(x => x.Name == "IHeaderChunk");
                        var isSkippableChunk = nestedSymbol.AllInterfaces.Any(x => x.Name == "ISkippableChunk");

                        var chunkAttribute = nestedSymbol.GetAttributes()
                            .FirstOrDefault(x => x.AttributeClass?.Name == "ChunkAttribute");

                        var chunkId = chunkAttribute?.ConstructorArguments[0].Value as uint?;
                        var chunkDescription = chunkAttribute?.ConstructorArguments.Length > 1 ? chunkAttribute.ConstructorArguments[1].Value as string : null;

                        if (chunkId is null)
                        {
                            chunksWithNoId.Add(nestedSymbol);
                            continue;
                        }

                        var chunk = new ChunkDataModel(chunkId.Value, chunkDescription, isSkippableChunk, ChunkLDefinition: null, nestedSymbol);

                        if (isHeaderChunk)
                        {
                            headerChunkDict.Add(chunkId.Value, chunk);
                        }
                        else
                        {
                            chunkDict.Add(chunkId.Value, chunk);
                        }
                    }

                    if (nestedSymbol.AllInterfaces.Any(x => x.Name is "IReadable" or "IWritable" or "IReadableWritable"))
                    {
                        archives.Add(nestedSymbol.Name, new ArchiveDataModel(ChunkLDefinition: null, nestedSymbol));
                    }
                }

                classModels.Add(gbxClass.Name, new ClassDataModel(
                    gbxClass.Name,
                    id,
                    gbxClass.ContainingNamespace.Name,
                    inherits,
                    Description: null,
                    gbxClass,
                    headerChunkDict.ToImmutable(),
                    chunkDict.ToImmutable(),
                    chunksWithNoId.ToImmutable(),
                    isNamelessArchive ? new ArchiveDataModel(ChunkLDefinition: null, gbxClass) : null,
                    archives.ToImmutable(),
                    ImmutableDictionary<string, EnumDataModel>.Empty,
                    gbxClass.IsAbstract));
            }

            return classModels.ToImmutable();
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
