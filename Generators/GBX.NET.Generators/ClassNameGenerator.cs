using ChunkL;
using GBX.NET.Generators.Utils;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Generators;

[Generator]
public class ClassNameGenerator : ClassChunkLMixedGenerator
{
    private const bool Debug = false;

    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {
        if (Debug && !Debugger.IsAttached)
        {
            Debugger.Launch();
        }

        base.Initialize(context);
    }

    protected override void Initialize(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<ImmutableArray<ClassDataModel>> transformed)
    {
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

        var combined = transformed.Combine(classIdContents.Collect());

        context.RegisterSourceOutput(combined, GenerateGetNameSource);
        context.RegisterSourceOutput(combined, GenerateGetIdSource);
    }

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

    private void GenerateGetNameSource(SourceProductionContext context, (ImmutableArray<ClassDataModel> ExistingClasses, ImmutableArray<string> ClassNames) source)
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
            builder.AppendLine($"        0x{classInfo.Id:X8} => nameof(GBX.NET.Engines.{classInfo.Engine}.{classInfo.Name}),");
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
                builder.AppendLine($"            0x{pair.Key:X8} => null,");
            }
            else
            {
                builder.AppendLine($"            0x{pair.Key:X8} => \"{pair.Value}\",");
            }
        }

        builder.AppendLine("            _ => null");
        builder.AppendLine("        };");
        builder.AppendLine("    }");
        builder.AppendLine("}");

        context.AddSource("ClassManager.GetName", builder.ToString());
    }

    private void GenerateGetIdSource(SourceProductionContext context, (ImmutableArray<ClassDataModel> ExistingClasses, ImmutableArray<string> ClassNames) source)
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
            builder.AppendLine($"        nameof(GBX.NET.Engines.{classInfo.Engine}.{classInfo.Name}) => 0x{classInfo.Id:X8},");
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
                builder.AppendLine($"            \"{pair.Value}\" => 0x{pair.Key:X8},");
                alreadyAdded.Add(pair.Value);
            }
        }

        builder.AppendLine("            _ => null");
        builder.AppendLine("        };");
        builder.AppendLine("    }");
        builder.AppendLine("}");

        context.AddSource("ClassManager.GetId", builder.ToString());
    }
}