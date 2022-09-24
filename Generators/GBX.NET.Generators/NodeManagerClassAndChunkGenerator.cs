using GBX.NET.Generators.Extensions;
using Microsoft.CodeAnalysis;
using System.Text;

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
        
        builder.AppendLine("    public static Type? GetClassTypeById(uint classId) => classId switch");
        builder.AppendLine("    {");

        foreach (var engineType in engineTypes)
        {
            var attributes = engineType.GetAttributes();
            
            var classId = default(uint?);
            var moreClassIds = default(List<uint>);
            var nodeExtensions = default(List<string>);

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
                nodeExtensions ?? Enumerable.Empty<string>()));
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");

        builder.AppendLine();
        
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

        builder.AppendLine();

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

        builder.AppendLine("}");

        context.AddSource("NodeManager.ClassAndChunk.g.cs", builder.ToString());
    }

    public record EngineType(INamedTypeSymbol TypeSymbol,
        uint ClassId, IEnumerable<uint> MoreClassIds,
        IEnumerable<string> NodeExtensions);
}
