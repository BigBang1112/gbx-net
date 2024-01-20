using ChunkL;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Generators;

[Generator(LanguageNames.CSharp)]
public class OldClassTypeGenerator : IIncrementalGenerator
{
    private const bool Debug = false;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        if (Debug && !Debugger.IsAttached)
        {
            Debugger.Launch();
        }

        var symbolAndClassId = context.CompilationProvider.Select(GetTypeSymbols);

        var chunklFiles = context.AdditionalTextsProvider
            .Where(static file => file.Path.EndsWith(".chunkl", StringComparison.OrdinalIgnoreCase))
            .Select((chunklFile, token) =>
            {
                if (chunklFile.GetText()?.ToString() is not string chunklText)
                {
                    throw new Exception("Could not get text from file.");
                }

                using var reader = new StringReader(chunklText);

                return new ChunkLData(
                    DataModel: ChunkLSerializer.Deserialize(reader),
                    Engine: Path.GetFileName(Path.GetDirectoryName(chunklFile.Path)));
            });

        var combined = symbolAndClassId.Combine(chunklFiles.Collect());

        var transformed = combined.Select(static (source, token) =>
        {
            var (classes, chunkLData) = source;

            var dict = new Dictionary<uint, ClassInfo>();

            foreach (var (symbol, classId) in classes)
            {
                dict.Add(classId, new ClassInfo(classId, symbol.ToDisplayString(), symbol.IsAbstract));
            }

            foreach (var chunkl in chunkLData)
            {
                if (dict.ContainsKey(chunkl.DataModel.Header.Id))
                {
                    continue;
                }

                var classInfo = new ClassInfo(
                    ClassId: chunkl.DataModel.Header.Id,
                    FullClassName: $"GBX.NET.Engines.{chunkl.Engine}.{chunkl.DataModel.Header.Name}",
                    IsAbstract: false);

                dict.Add(chunkl.DataModel.Header.Id, classInfo);
            }

            return dict.Select(x => x.Value).ToImmutableArray();
        });

        context.RegisterSourceOutput(transformed, GenerateSource);
    }

    private record ChunkLData(ChunkLDataModel DataModel, string Engine);
    private record ClassInfo(uint ClassId, string FullClassName, bool IsAbstract);

    private IEnumerable<INamespaceSymbol> RecurseNamespaces(INamespaceSymbol namespaceSymbol)
    {
        yield return namespaceSymbol;

        foreach (var n in namespaceSymbol.GetNamespaceMembers())
        {
            foreach (var nn in RecurseNamespaces(n))
            {
                yield return nn;
            }
        }
    }

    private IEnumerable<(INamedTypeSymbol, uint classId)> GetTypeSymbols(Compilation compilation, CancellationToken token)
    {
        var enginesNamespace = compilation.GlobalNamespace.GetNamespaceMembers()
            .FirstOrDefault(x => x.Name == "GBX")
            .GetNamespaceMembers()
            .FirstOrDefault(x => x.Name == "NET")
            .GetNamespaceMembers()
            .FirstOrDefault(x => x.Name == "Engines");

        foreach (var typeSymbol in RecurseNamespaces(enginesNamespace).SelectMany(x => x.GetTypeMembers()))
        {
            if (typeSymbol.AllInterfaces.Any(x => x.Name == "IClass"))
            {
                var attribute = typeSymbol.GetAttributes().FirstOrDefault(x => x.AttributeClass?.Name == "ClassAttribute");

                if (attribute is null)
                {
                    continue;
                }

                yield return (typeSymbol, classId: (uint)(attribute.ConstructorArguments[0].Value ?? -1));
            }
        }
    }

    private void GenerateSource(SourceProductionContext context, ImmutableArray<ClassInfo> classInfos)
    {
        var builder = new StringBuilder();

        builder.AppendLine("using GBX.NET.Components;");
        builder.AppendLine();
        builder.AppendLine("namespace GBX.NET.Managers;");
        builder.AppendLine();
        builder.AppendLine("public static partial class ClassManager");
        builder.AppendLine("{");
        builder.AppendLine("    internal static Dictionary<Type, uint> ClassIds { get; } = new()");
        builder.AppendLine("    {");

        foreach (var classInfo in classInfos)
        {
            builder.AppendLine($"        {{ typeof({classInfo.FullClassName}), 0x{classInfo.ClassId:X8} }},");
        }

        builder.AppendLine("    };");
        builder.AppendLine();
        builder.AppendLine("    public static partial Type? GetType(uint classId) => classId switch");
        builder.AppendLine("    {");

        foreach (var classInfo in classInfos)
        {
            builder.AppendLine($"        0x{classInfo.ClassId:X8} => typeof({classInfo.FullClassName}),");
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");
        builder.AppendLine();
        builder.AppendLine("    internal static partial IClass? New(uint classId) => classId switch");
        builder.AppendLine("    {");

        foreach (var classInfo in classInfos)
        {
            if (classInfo.IsAbstract)
            {
                continue;
            }

            builder.AppendLine($"        0x{classInfo.ClassId:X8} => new {classInfo.FullClassName}(),");
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");
        builder.AppendLine();
        builder.AppendLine("    internal static partial GbxHeader? NewHeader(GbxHeaderBasic basic, uint classId) => classId switch");
        builder.AppendLine("    {");

        foreach (var classInfo in classInfos)
        {
            builder.AppendLine($"        0x{classInfo.ClassId:X8} => new GbxHeader<{classInfo.FullClassName}>(basic),");
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");
        builder.AppendLine();
        builder.AppendLine("    internal static partial Gbx? NewGbx(GbxHeader header, IClass node) => header.ClassId switch");
        builder.AppendLine("    {");

        foreach (var classInfo in classInfos)
        {
            builder.AppendLine($"        0x{classInfo.ClassId:X8} => new Gbx<{classInfo.FullClassName}>((GbxHeader<{classInfo.FullClassName}>)header, ({classInfo.FullClassName})node),");
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");

        builder.AppendLine("}");

        context.AddSource("ClassManager.ClassType", builder.ToString());
    }
}