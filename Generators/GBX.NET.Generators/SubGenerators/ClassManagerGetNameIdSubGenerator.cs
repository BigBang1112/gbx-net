using GBX.NET.Generators.Models;
using GBX.NET.Generators.Utils;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Text;

namespace GBX.NET.Generators.SubGenerators;

internal static class ClassManagerGetNameIdSubGenerator
{
    private static Dictionary<uint, string> GetClassIdNameDictionary(ImmutableArray<string> classNames)
    {
        using var ms = new MemoryStream();
        using var writer = new StreamWriter(ms, Encoding.UTF8, 4096, leaveOpen: true);

        foreach (var className in classNames)
        {
            writer.WriteLine(className);
        }

        writer.Flush();
        ms.Position = 0;

        using var reader = new StreamReader(ms);

        return ClassIdParser.Parse(reader);
    }

    public static void GenerateGetNameSource(SourceProductionContext context, (ImmutableDictionary<string, ClassDataModel> ExistingClasses, ImmutableArray<string> ClassNames) source)
    {
        var (existingClasses, classNames) = source;
        var classIdNameDict = GetClassIdNameDictionary(classNames);

        var builder = new StringBuilder();

        builder.AppendLine("namespace GBX.NET.Managers;");
        builder.AppendLine();
        builder.AppendLine("public static partial class ClassManager");
        builder.AppendLine("{");
        builder.AppendLine("#if !DEBUG");
        builder.AppendLine("    public static partial string? GetName(uint classId) => classId switch");
        builder.AppendLine("    {");

        foreach (var classInfo in existingClasses)
        {
            builder.Append("        0x");
            builder.Append(classInfo.Value.Id.GetValueOrDefault().ToString("X8"));
            builder.Append(" => nameof(");
            builder.Append(classInfo.Key);
            builder.AppendLine("),");
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");
        builder.AppendLine("#endif");
        builder.AppendLine();
        builder.AppendLine("    public static partial string? GetName(uint classId, bool all)");
        builder.AppendLine("    {");
        builder.AppendLine("#if !DEBUG");
        builder.AppendLine("        if (!all) return GetName(classId);");
        builder.AppendLine("#endif");
        builder.AppendLine("        return classId switch");
        builder.AppendLine("        {");

        foreach (var pair in classIdNameDict)
        {
            if (string.IsNullOrEmpty(pair.Value))
            {
                builder.Append($"            0x");
                builder.Append(pair.Key.ToString("X8"));
                builder.AppendLine(" => null,");
            }
            else
            {
                builder.Append("            0x");
                builder.Append(pair.Key.ToString("X8"));
                builder.Append(" => \"");
                builder.Append(pair.Value);
                builder.AppendLine("\",");
            }
        }

        builder.AppendLine("            0x3F001000 => \"CGameCtnChallenge (Unlimiter)\",");
        builder.AppendLine("            _ => null");
        builder.AppendLine("        };");
        builder.AppendLine("    }");
        builder.AppendLine("}");

        context.AddSource("Managers/ClassManager.GetName", builder.ToString());
    }

    public static void GenerateGetIdSource(SourceProductionContext context, (ImmutableDictionary<string, ClassDataModel> ExistingClasses, ImmutableArray<string> ClassNames) source)
    {
        var (existingClasses, classNames) = source;
        var classIdNameDict = GetClassIdNameDictionary(classNames);

        var builder = new StringBuilder();

        builder.AppendLine("namespace GBX.NET.Managers;");
        builder.AppendLine();
        builder.AppendLine("public static partial class ClassManager");
        builder.AppendLine("{");
        builder.AppendLine("#if !DEBUG");
        builder.AppendLine("    public static partial uint? GetId(string className) => className switch");
        builder.AppendLine("    {");

        foreach (var classInfo in existingClasses)
        {
            builder.Append("        nameof(");
            builder.Append(classInfo.Key);
            builder.Append(") => 0x");
            builder.Append(classInfo.Value.Id.GetValueOrDefault().ToString("X8"));
            builder.AppendLine(",");
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");
        builder.AppendLine("#endif");
        builder.AppendLine();
        builder.AppendLine("    public static partial uint? GetId(string className, bool all)");
        builder.AppendLine("    {");
        builder.AppendLine("#if !DEBUG");
        builder.AppendLine("        if (!all) return GetId(className);");
        builder.AppendLine("#endif");
        builder.AppendLine("        return className switch");
        builder.AppendLine("        {");

        var alreadyAdded = new HashSet<string>();

        foreach (var pair in classIdNameDict)
        {
            if (!string.IsNullOrEmpty(pair.Value) && !alreadyAdded.Contains(pair.Value))
            {
                builder.Append("            \"");
                builder.Append(pair.Value);
                builder.Append("\" => 0x");
                builder.Append(pair.Key.ToString("X8"));
                builder.AppendLine(",");
                alreadyAdded.Add(pair.Value);
            }
        }

        builder.AppendLine("            _ => null");
        builder.AppendLine("        };");
        builder.AppendLine("    }");
        builder.AppendLine("}");

        context.AddSource("Managers/ClassManager.GetId", builder.ToString());
    }
}
