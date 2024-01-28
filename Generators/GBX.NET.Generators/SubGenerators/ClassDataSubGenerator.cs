using GBX.NET.Generators.Models;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Text;

namespace GBX.NET.Generators.SubGenerators;

internal class ClassDataSubGenerator
{
    public static void GenerateSource(SourceProductionContext context, ImmutableArray<ClassDataModel> classInfos)
    {
        var inheritanceInverted = new Dictionary<string, Dictionary<uint, string>>();

        // does not work properly, misses recursion
        foreach (var classInfo in classInfos)
        {
            if (classInfo.Inherits is null || (classInfo.TypeSymbol is not null && classInfo.TypeSymbol.IsAbstract))
            {
                continue;
            }

            if (!inheritanceInverted.ContainsKey(classInfo.Inherits))
            {
                inheritanceInverted[classInfo.Inherits] = [];
            }

            inheritanceInverted[classInfo.Inherits].Add(classInfo.Id.GetValueOrDefault(), classInfo.Name);
        }

        foreach (var classInfo in classInfos)
        {
            var sb = new StringBuilder();

            sb.Append("namespace GBX.NET.Engines.");
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

            if (classInfo.TypeSymbol is null || !classInfo.TypeSymbol.IsAbstract)
            {
                sb.Append("        0x");
                sb.Append(classInfo.Id.GetValueOrDefault().ToString("X8"));
                sb.Append(" => new ");
                sb.Append(classInfo.Name);
                sb.AppendLine("(),");
            }

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

            context.AddSource($"Engines/{classInfo.Name}", sb.ToString());
        }

        foreach (var classInfo in classInfos)
        {
            if (classInfo.Id is null)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor(
                    "GBXNETGEN001",
                    "Class has no ID",
                    "{0} has no ID defined by ClassAttribute",
                    "GBX.NET.Generators",
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true),
                    classInfo.TypeSymbol?.Locations.FirstOrDefault(),
                    classInfo.Name));
            }

            foreach (var chunkInfo in classInfo.ChunksWithNoId)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor(
                    "GBXNETGEN002",
                    "Chunk has no ID",
                    "{0} has no ID defined by ChunkAttribute",
                    "GBX.NET.Generators",
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true),
                    chunkInfo.Locations.FirstOrDefault(),
                    chunkInfo.Name));
            }
        }
    }
}
