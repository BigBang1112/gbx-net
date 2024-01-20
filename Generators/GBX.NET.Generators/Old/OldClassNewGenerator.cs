using ChunkL;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Generators;

[Generator(LanguageNames.CSharp)]
public class OldClassNewGenerator : IIncrementalGenerator
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
            var chunkLPerName = chunkLData.ToDictionary(x => x.DataModel.Header.Name);

            var dict = new Dictionary<uint, ClassInfo>();

            foreach (var (symbol, classId) in classes)
            {
                if (symbol.IsAbstract)
                {
                    continue;
                }

                var hasAssociatedChunkL = chunkLPerName.TryGetValue(symbol.Name, out var associatedChunkL);
                var chunklInherits = default(string?);
                _ = associatedChunkL?.DataModel.Header.Features.TryGetValue("inherits", out chunklInherits);
                var inherits = symbol.BaseType?.Name ?? chunklInherits ?? "CMwNod";
                if (inherits == "Object") inherits = null;

                dict.Add(classId, new ClassInfo(
                    ClassId: classId,
                    Name: symbol.Name,
                    Engine: symbol.ContainingNamespace.ToDisplayString(),
                    Inherits: inherits));
            }

            foreach (var chunkl in chunkLData)
            {
                if (dict.ContainsKey(chunkl.DataModel.Header.Id))
                {
                    continue;
                }

                _ = chunkl.DataModel.Header.Features.TryGetValue("inherits", out var inherits);

                var classInfo = new ClassInfo(
                    ClassId: chunkl.DataModel.Header.Id,
                    Name: chunkl.DataModel.Header.Name,
                    Engine: $"GBX.NET.Engines.{chunkl.Engine}",
                    Inherits: inherits ?? "CMwNod");

                dict.Add(chunkl.DataModel.Header.Id, classInfo);
            }

            return dict.Select(x => x.Value).ToImmutableArray();
        });

        context.RegisterSourceOutput(transformed, GenerateSource);
    }

    private record ChunkLData(ChunkLDataModel DataModel, string Engine);
    private record ClassInfo(uint ClassId, string Name, string Engine, string? Inherits);

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
        var inheritanceInverted = new Dictionary<string, Dictionary<uint, string>>();

        foreach (var classInfo in classInfos)
        {
            if (classInfo.Inherits is null)
            {
                continue;
            }

            if (!inheritanceInverted.ContainsKey(classInfo.Inherits))
            {
                inheritanceInverted[classInfo.Inherits] = [];
            }

            inheritanceInverted[classInfo.Inherits].Add(classInfo.ClassId, classInfo.Name);
        }

        foreach (var classInfo in classInfos)
        {
            var sb = new StringBuilder();

            sb.Append("namespace ");
            sb.Append(classInfo.Engine);
            sb.AppendLine(";");

            sb.AppendLine();

            sb.Append("public partial class ");
            sb.AppendLine(classInfo.Name);
            sb.AppendLine("{");

            sb.Append("    public static ");

            if (classInfo.Name != "CMwNod")
            {
                sb.Append("new ");
            }

            sb.AppendLine("IClass? New(uint classId) => classId switch");
            sb.AppendLine("    {");

            sb.Append("        0x");
            sb.Append(classInfo.ClassId.ToString("X8"));
            sb.Append(" => new ");
            sb.Append(classInfo.Name);
            sb.AppendLine("(),");

            if (inheritanceInverted.TryGetValue(classInfo.Name, out var classes))
            {
                foreach (var classIdAndName in classes)
                {
                    sb.Append("        0x");
                    sb.Append(classIdAndName.Key.ToString("X8"));
                    sb.Append(" => new ");
                    sb.Append(classIdAndName.Value);
                    sb.AppendLine("(),");
                }
            }

            sb.AppendLine("        _ => null");

            sb.AppendLine("    };");

            sb.AppendLine("}");

            context.AddSource(classInfo.Name, sb.ToString());
        }
    }
}