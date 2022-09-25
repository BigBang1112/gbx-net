using GBX.NET.Generators.Extensions;
using Microsoft.CodeAnalysis;
using System.Text;
using static GBX.NET.Generators.NodeManagerClassAndChunkGenerator;

namespace GBX.NET.Generators;

[Generator]
public class NodeManagerClassAndChunkGenerator : SourceGenerator
{
    public override bool Debug => false;

    public override void Execute(GeneratorExecutionContext context)
    {
        var enginesNamespace = context.Compilation
            .GlobalNamespace
            .NavigateToNamespace("GBX.NET.Engines") ?? throw new Exception("GBX.NET.Engines namespace not found.");

        var engineTypes = enginesNamespace.GetNamespaceMembers()
            .SelectMany(x => x.GetTypeMembers())
            .Where(x =>
            {
                var baseType = x.BaseType;

                while (baseType is not null && baseType.Name != "Node")
                {
                    baseType = baseType.BaseType;
                }

                return baseType is not null && baseType.Name == "Node";
            })
            .ToList();

        var builder = new StringBuilder("namespace GBX.NET;\n\n");
        builder.AppendLine("public static partial class NodeManager");
        builder.AppendLine("{");

        var engineTypeDetailedList = new List<EngineType>();

        GenerateGetClassTypeById(engineTypes, builder, engineTypeDetailedList);

        builder.AppendLine();

        GenerateClassIdsByType(builder, engineTypeDetailedList);

        builder.AppendLine();

        GenerateGetGbxExtensions(builder, engineTypeDetailedList);

        builder.AppendLine();

        GenerateGetChunkTypeById(builder, engineTypeDetailedList);

        builder.AppendLine();

        GenerateChunkIdsByType(builder, engineTypeDetailedList);

        builder.AppendLine();

        GemerateGetHeaderChunkTypeById(builder, engineTypeDetailedList);

        builder.AppendLine();

        GenerateHeaderChunkIdsByType(builder, engineTypeDetailedList);

        builder.AppendLine();
        
        GenerateWritingNotSupportedClassTypes(builder, engineTypeDetailedList);

        builder.AppendLine();
        
        builder.AppendLine("    public readonly record struct ChunkAttributes(string Description, bool ProcessSync, bool Ignore, bool AutoReadWrite);");
        builder.AppendLine();
        builder.AppendLine("    public static IReadOnlyDictionary<uint, ChunkAttributes> ChunkAttributesById { get; } = new Dictionary<uint, ChunkAttributes>");
        builder.AppendLine("    {");

        foreach (var engineType in engineTypeDetailedList)
        {
            foreach (var chunkType in engineType.ChunkTypes)
            {
                if (chunkType.IsHeaderChunk)
                {
                    continue;
                }

                builder.Append("        { ");
                builder.Append(chunkType.ChunkId);
                builder.Append(", ");
                GenerateChunkAttributesCtor(builder, chunkType);
                builder.AppendLine(" },");
            }
        }

        builder.AppendLine("    };");

        builder.AppendLine();

        builder.AppendLine("    public static IReadOnlyDictionary<uint, ChunkAttributes> HeaderChunkAttributesById { get; } = new Dictionary<uint, ChunkAttributes>");
        builder.AppendLine("    {");

        foreach (var engineType in engineTypeDetailedList)
        {
            foreach (var chunkType in engineType.ChunkTypes)
            {
                if (!chunkType.IsHeaderChunk)
                {
                    continue;
                }

                builder.Append("        { ");
                builder.Append(chunkType.ChunkId);
                builder.Append(", ");
                GenerateChunkAttributesCtor(builder, chunkType);
                builder.AppendLine(" },");
            }
        }

        builder.AppendLine("    };");

        builder.AppendLine();

        builder.AppendLine("    public static IReadOnlyDictionary<Type, ChunkAttributes> ChunkAttributesByType { get; } = new Dictionary<Type, ChunkAttributes>");
        builder.AppendLine("    {");

        foreach (var engineType in engineTypeDetailedList)
        {
            foreach (var chunkType in engineType.ChunkTypes)
            {
                builder.Append("        { typeof(");
                builder.Append(engineType.TypeSymbol.Name);
                builder.Append('.');
                builder.Append(chunkType.TypeSymbol.Name);
                builder.Append("), ");
                GenerateChunkAttributesCtor(builder, chunkType);
                builder.AppendLine(" },");
            }
        }

        builder.AppendLine("    };");

        builder.AppendLine();

        builder.AppendLine("    public static Chunk? GetNewChunk(uint chunkId) => chunkId switch");
        builder.AppendLine("    {");

        foreach (var engineType in engineTypeDetailedList)
        {
            foreach (var chunkType in engineType.ChunkTypes)
            {
                if (chunkType.IsHeaderChunk)
                {
                    continue;
                }

                builder.Append("        ");
                builder.Append(chunkType.ChunkId);
                builder.Append(" => new ");
                builder.Append(engineType.TypeSymbol.Name);
                builder.Append('.');
                builder.Append(chunkType.TypeSymbol.Name);
                builder.AppendLine("(),");
            }
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");

        builder.AppendLine();

        builder.AppendLine("    public static Node? GetNewNode(uint classId) => classId switch");
        builder.AppendLine("    {");

        foreach (var engineType in engineTypeDetailedList)
        {
            if (engineType.TypeSymbol.IsAbstract)
            {
                continue;
            }

            builder.Append("        ");
            builder.Append(engineType.ClassId);
            builder.Append(" => new ");
            builder.Append(engineType.TypeSymbol.Name);
            builder.AppendLine("(),");
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");

        builder.AppendLine();

        builder.AppendLine("    public static IHeaderChunk? GetNewHeaderChunk(uint chunkId) => chunkId switch");
        builder.AppendLine("    {");

        foreach (var engineType in engineTypeDetailedList)
        {
            foreach (var chunkType in engineType.ChunkTypes)
            {
                if (!chunkType.IsHeaderChunk)
                {
                    continue;
                }

                builder.Append("        ");
                builder.Append(chunkType.ChunkId);
                builder.Append(" => new ");
                builder.Append(engineType.TypeSymbol.Name);
                builder.Append('.');
                builder.Append(chunkType.TypeSymbol.Name);
                builder.AppendLine("(),");
            }
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");

        builder.AppendLine();

        builder.AppendLine("    public static bool IsSkippableChunk(uint chunkId) => chunkId switch");
        builder.AppendLine("    {");

        foreach (var engineType in engineTypeDetailedList)
        {
            foreach (var chunkType in engineType.ChunkTypes)
            {
                if (!chunkType.IsSkippableChunk)
                {
                    continue;
                }

                builder.Append("        ");
                builder.Append(chunkType.ChunkId);
                builder.AppendLine(" => true,");
            }
        }

        builder.AppendLine("        _ => false");
        builder.AppendLine("    };");

        builder.AppendLine();

        builder.AppendLine("    public static bool IsAsyncChunk(uint chunkId) => chunkId switch");
        builder.AppendLine("    {");

        foreach (var engineType in engineTypeDetailedList)
        {
            foreach (var chunkType in engineType.ChunkTypes)
            {
                if (!chunkType.OverridesReadAsync && !chunkType.OverridesWriteAsync && !chunkType.OverridesReadWriteAsync)
                {
                    continue;
                }

                builder.Append("        ");
                builder.Append(chunkType.ChunkId);
                builder.AppendLine(" => true,");
            }
        }

        builder.AppendLine("        _ => false");
        builder.AppendLine("    };");

        builder.AppendLine();

        builder.AppendLine("    public static bool IsReadAsyncChunk(uint chunkId) => chunkId switch");
        builder.AppendLine("    {");

        foreach (var engineType in engineTypeDetailedList)
        {
            foreach (var chunkType in engineType.ChunkTypes)
            {
                if (!chunkType.OverridesReadAsync)
                {
                    continue;
                }

                builder.Append("        ");
                builder.Append(chunkType.ChunkId);
                builder.AppendLine(" => true,");
            }
        }

        builder.AppendLine("        _ => false");
        builder.AppendLine("    };");

        builder.AppendLine();

        builder.AppendLine("    public static bool IsWriteAsyncChunk(uint chunkId) => chunkId switch");
        builder.AppendLine("    {");

        foreach (var engineType in engineTypeDetailedList)
        {
            foreach (var chunkType in engineType.ChunkTypes.Where(x => x.OverridesWriteAsync))
            {
                builder.Append("        ");
                builder.Append(chunkType.ChunkId);
                builder.AppendLine(" => true,");
            }
        }

        builder.AppendLine("        _ => false");
        builder.AppendLine("    };");
        
        builder.AppendLine();

        builder.AppendLine("    public static bool IsReadWriteAsyncChunk(uint chunkId) => chunkId switch");
        builder.AppendLine("    {");

        foreach (var engineType in engineTypeDetailedList)
        {
            foreach (var chunkType in engineType.ChunkTypes.Where(x => x.OverridesReadWriteAsync))
            {
                builder.Append("        ");
                builder.Append(chunkType.ChunkId);
                builder.AppendLine(" => true,");
            }
        }

        builder.AppendLine("        _ => false");
        builder.AppendLine("    };");
        
        builder.AppendLine("}");

        context.AddSource("NodeManager.ClassAndChunk.g.cs", builder.ToString());
    }

    private static void GenerateChunkAttributesCtor(StringBuilder builder, ChunkType chunkType)
    {
        builder.Append("new(Description: \"");
        builder.Append(chunkType.Description);
        builder.Append("\", ProcessSync: ");
        builder.Append(chunkType.ProcessSync ? "true" : "false");
        builder.Append(", Ignore: ");
        builder.Append(chunkType.Ignore ? "true" : "false");
        builder.Append(", AutoReadWrite: ");
        builder.Append(chunkType.AutoReadWrite ? "true" : "false");
        builder.Append(')');
    }

    private static void GenerateWritingNotSupportedClassTypes(StringBuilder builder, List<EngineType> engineTypeDetailedList)
    {
        builder.AppendLine("    public static IReadOnlyCollection<Type> WritingNotSupportedClassTypes { get; } = new HashSet<Type>");
        builder.AppendLine("    {");

        foreach (var engineType in engineTypeDetailedList)
        {
            if (!engineType.WritingNotSupported)
            {
                continue;
            }

            builder.Append("        typeof(");
            builder.Append(engineType.TypeSymbol.Name);
            builder.AppendLine("),");
        }

        builder.AppendLine("    };");
    }

    private static void GenerateHeaderChunkIdsByType(StringBuilder builder, List<EngineType> engineTypeDetailedList)
    {
        builder.AppendLine("    public static IReadOnlyDictionary<Type, uint> HeaderChunkIdsByType { get; } = new Dictionary<Type, uint>");
        builder.AppendLine("    {");

        foreach (var engineType in engineTypeDetailedList)
        {
            foreach (var chunkType in engineType.ChunkTypes)
            {
                if (!chunkType.IsHeaderChunk)
                {
                    continue;
                }

                builder.Append("        { typeof(");
                builder.Append(engineType.TypeSymbol.Name);
                builder.Append('.');
                builder.Append(chunkType.TypeSymbol.Name);
                builder.Append("), ");
                builder.Append(chunkType.ChunkId);
                builder.AppendLine(" },");
            }
        }

        builder.AppendLine("    };");
    }

    private static void GemerateGetHeaderChunkTypeById(StringBuilder builder, List<EngineType> engineTypeDetailedList)
    {
        builder.AppendLine("    public static Type? GetHeaderChunkTypeById(uint chunkId) => chunkId switch");
        builder.AppendLine("    {");

        foreach (var engineType in engineTypeDetailedList)
        {
            foreach (var chunkType in engineType.ChunkTypes)
            {
                if (!chunkType.IsHeaderChunk)
                {
                    continue;
                }

                builder.Append("        ");
                builder.Append(chunkType.ChunkId);
                builder.Append(" => typeof(");
                builder.Append(engineType.TypeSymbol.Name);
                builder.Append('.');
                builder.Append(chunkType.TypeSymbol.Name);
                builder.AppendLine("),");
            }
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");
    }

    private static void GenerateChunkIdsByType(StringBuilder builder, List<EngineType> engineTypeDetailedList)
    {
        builder.AppendLine("    public static IReadOnlyDictionary<Type, uint> ChunkIdsByType { get; } = new Dictionary<Type, uint>");
        builder.AppendLine("    {");

        foreach (var engineType in engineTypeDetailedList)
        {
            foreach (var chunkType in engineType.ChunkTypes)
            {
                if (chunkType.IsHeaderChunk)
                {
                    continue;
                }

                builder.Append("        { typeof(");
                builder.Append(engineType.TypeSymbol.Name);
                builder.Append('.');
                builder.Append(chunkType.TypeSymbol.Name);
                builder.Append("), ");
                builder.Append(chunkType.ChunkId);
                builder.AppendLine(" },");
            }
        }

        builder.AppendLine("    };");
    }

    private static void GenerateGetChunkTypeById(StringBuilder builder, List<EngineType> engineTypeDetailedList)
    {
        builder.AppendLine("    public static Type? GetChunkTypeById(uint chunkId) => chunkId switch");
        builder.AppendLine("    {");

        foreach (var engineType in engineTypeDetailedList)
        {
            foreach (var chunkTypeSymbol in engineType.TypeSymbol.GetTypeMembers())
            {
                var baseType = chunkTypeSymbol.BaseType;

                while (baseType is not null && baseType.Name != "Chunk")
                {
                    baseType = baseType.BaseType;
                }

                if (baseType is null)
                {
                    continue;
                }

                var attributes = chunkTypeSymbol.GetAttributes();

                var chunkId = default(uint?);
                var description = "";
                var processSync = false;
                var ignore = false;
                var autoReadWrite = false;

                foreach (var att in attributes)
                {
                    switch (att.AttributeClass?.Name)
                    {
                        case "ChunkAttribute":

                            if (chunkId.HasValue)
                            {
                                throw new Exception("Chunk already has an ID defined.");
                            }

                            chunkId = Convert.ToUInt32(att.ConstructorArguments[0].Value);
                            description = Convert.ToString(att.ConstructorArguments[1].Value);

                            foreach (var arg in att.NamedArguments)
                            {
                                switch (arg.Key)
                                {
                                    case "ProcessSync":
                                        processSync = Convert.ToBoolean(arg.Value.Value);
                                        break;
                                }
                            }

                            break;

                        case "IgnoreChunkAttribute":
                            ignore = true;
                            break;

                        case "AutoReadWriteChunkAttribute":
                            autoReadWrite = true;
                            break;
                    }
                }

                if (chunkId is null)
                {
                    continue;
                }

                if (engineType.TypeSymbol.Name == "CGameCtnMediaBlockTimeSpeed" && chunkTypeSymbol.Name == "Chunk03085000")
                {
                    continue;
                }

                var isHeaderChunk = chunkTypeSymbol.AllInterfaces.Any(x => x.Name == "IHeaderChunk");
                var isSkippableChunk = !isHeaderChunk && chunkTypeSymbol.AllInterfaces.Any(x => x.Name == "ISkippableChunk");

                var overridesReadAsync = false;
                var overridesWriteAsync = false;
                var overridesReadWriteAsync = false;

                foreach (var member in chunkTypeSymbol.GetMembers())
                {
                    if (member is not IMethodSymbol methodSymbol || !methodSymbol.IsOverride)
                    {
                        continue;
                    }

                    switch (member.Name)
                    {
                        case "ReadAsync":
                            overridesReadAsync = true;
                            break;
                        case "WriteAsync":
                            overridesWriteAsync = true;
                            break;
                        case "ReadWriteAsync":
                            overridesReadWriteAsync = true;
                            break;
                    }
                }
                
                engineType.ChunkTypes.Add(new(
                    chunkTypeSymbol,
                    chunkId.Value,
                    description,
                    isHeaderChunk,
                    isSkippableChunk,
                    processSync,
                    ignore,
                    autoReadWrite,
                    overridesReadAsync,
                    overridesWriteAsync,
                    overridesReadWriteAsync));

                if (isHeaderChunk)
                {
                    continue;
                }

                builder.Append("        ");
                builder.Append(chunkId.Value);
                builder.Append(" => typeof(");
                builder.Append(engineType.TypeSymbol.Name);
                builder.Append('.');
                builder.Append(chunkTypeSymbol.Name);
                builder.AppendLine("),");
            }
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");
    }

    private static void GenerateGetClassTypeById(List<INamedTypeSymbol> engineTypes, StringBuilder builder, List<EngineType> engineTypeDetailedList)
    {
        builder.AppendLine("    public static Type? GetClassTypeById(uint classId) => classId switch");
        builder.AppendLine("    {");

        foreach (var engineType in engineTypes)
        {
            var attributes = engineType.GetAttributes();

            var classId = default(uint?);
            var moreClassIds = default(List<uint>);
            var nodeExtensions = default(List<string>);
            var writingNotSupported = false;

            foreach (var att in attributes)
            {
                switch (att.AttributeClass?.Name)
                {
                    case "NodeAttribute":

                        if (classId is null)
                        {
                            classId = Convert.ToUInt32(att.ConstructorArguments.First().Value);
                            break;
                        }

                        moreClassIds ??= new();
                        moreClassIds.Add(Convert.ToUInt32(att.ConstructorArguments.First().Value));

                        break;
                    case "NodeExtensionAttribute":
                        nodeExtensions ??= new();
                        nodeExtensions.Add(Convert.ToString(att.ConstructorArguments.First().Value));
                        break;
                    case "WritingNotSupportedAttribute":
                        writingNotSupported = true;
                        break;
                }
            }

            if (classId is null)
            {
                throw new Exception($"NodeAttribute not found on {engineType.Name}.");
            }

            builder.Append("        ");
            builder.Append(classId);
            builder.Append(" => typeof(");
            builder.Append(engineType.Name);
            builder.AppendLine("),");

            if (moreClassIds is not null)
            {
                foreach (var id in moreClassIds)
                {
                    builder.Append("        ");
                    builder.Append(id);
                    builder.Append(" => typeof(");
                    builder.Append(engineType.Name);
                    builder.AppendLine("),");
                }
            }

            engineTypeDetailedList.Add(new(engineType,
                classId.Value, moreClassIds ?? Enumerable.Empty<uint>(),
                nodeExtensions ?? Enumerable.Empty<string>(), writingNotSupported));
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");
    }

    private static void GenerateClassIdsByType(StringBuilder builder, List<EngineType> engineTypeDetailedList)
    {
        builder.AppendLine("    public static IReadOnlyDictionary<Type, uint> ClassIdsByType { get; } = new Dictionary<Type, uint>");
        builder.AppendLine("    {");

        foreach (var engineTypeDetailed in engineTypeDetailedList)
        {
            var engineType = engineTypeDetailed.TypeSymbol;
            var id = engineTypeDetailed.ClassId;

            builder.Append("        { typeof(");
            builder.Append(engineType.Name);
            builder.Append("), ");
            builder.Append(id);
            builder.AppendLine(" },");
        }

        builder.AppendLine("    };");
    }

    private static void GenerateGetGbxExtensions(StringBuilder builder, List<EngineType> engineTypeDetailedList)
    {
        builder.AppendLine("    public static IEnumerable<string> GetGbxExtensions(uint classId)");
        builder.AppendLine("    {");
        builder.AppendLine("        switch (classId)");
        builder.AppendLine("        {");

        foreach (var engineTypeDetailed in engineTypeDetailedList)
        {
            if (!engineTypeDetailed.NodeExtensions.Any())
            {
                continue;
            }

            builder.Append("            case ");
            builder.Append(engineTypeDetailed.ClassId);
            builder.AppendLine(":");

            foreach (var extension in engineTypeDetailed.NodeExtensions)
            {
                builder.Append("                yield return \"");
                builder.Append(extension);
                builder.AppendLine("\";");
            }

            builder.AppendLine("                break;");
        }
        builder.AppendLine("        }");
        builder.AppendLine("    }");
    }

    public record EngineType(INamedTypeSymbol TypeSymbol, uint ClassId,
        IEnumerable<uint> MoreClassIds, IEnumerable<string> NodeExtensions, bool WritingNotSupported)
    {
        public List<ChunkType> ChunkTypes { get; } = new();
    }

    public record ChunkType(INamedTypeSymbol TypeSymbol, uint ChunkId,
        string Description, bool IsHeaderChunk, bool IsSkippableChunk,
        bool ProcessSync, bool Ignore, bool AutoReadWrite,
        bool OverridesReadAsync, bool OverridesWriteAsync, bool OverridesReadWriteAsync);
}
