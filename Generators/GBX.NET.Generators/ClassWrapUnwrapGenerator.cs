using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace GBX.NET.Generators;

[Generator]
public class ClassWrapUnwrapGenerator : IIncrementalGenerator
{
    private const bool Debug = false;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        if (Debug && !Debugger.IsAttached)
        {
            Debugger.Launch();
        }

        var wrapContents = context.AdditionalTextsProvider
            .Where(static file =>
            {
                return file.Path.EndsWith("Wrap.txt") && Path.GetDirectoryName(file.Path).EndsWith("Resources");
            })
            .Select((additionalText, cancellationToken) =>
            {
                var text = additionalText.GetText(cancellationToken) ?? throw new Exception("Could not get text from file.");
                return text.Lines;
            });

        var unwrapContents = context.AdditionalTextsProvider
            .Where(static file =>
            {
                return file.Path.EndsWith("Unwrap.txt") && Path.GetDirectoryName(file.Path).EndsWith("Resources");
            })
            .Select((additionalText, cancellationToken) =>
            {
                var text = additionalText.GetText(cancellationToken) ?? throw new Exception("Could not get text from file.");
                return text.Lines;
            });

        context.RegisterSourceOutput(wrapContents, GenerateWrapSource);
        context.RegisterSourceOutput(unwrapContents, GenerateUnwrapSource);
    }

    private static ImmutableDictionary<uint, uint> GetClassIdDict(TextLineCollection lines)
    {
        var dict = ImmutableDictionary.CreateBuilder<uint, uint>();

        foreach (var line in lines)
        {
            var split = line.ToString().Split(' ');

            if (split.Length != 2) continue;

            if (!uint.TryParse(split[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var classId)) continue;
            if (!uint.TryParse(split[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var chunkId)) continue;

            dict.Add(classId, chunkId);
        }

        return dict.ToImmutable();
    }

    private void GenerateSource(SourceProductionContext context, TextLineCollection lines, string name)
    {
        var classIdDict = GetClassIdDict(lines);

        var builder = new StringBuilder();

        builder.AppendLine("namespace GBX.NET.Managers;");
        builder.AppendLine();
        builder.AppendLine("public static partial class ClassManager");
        builder.AppendLine("{");
        builder.AppendLine($"    internal static partial uint {name}(uint classId)");
        builder.AppendLine("    {");
        builder.AppendLine("        var chunkPart = classId & 0x00000FFF;");
        builder.AppendLine("        var classPart = classId & 0xFFFFF000;");
        builder.AppendLine();
        builder.AppendLine("        switch (classPart)");
        builder.AppendLine("        {");

        foreach (var pair in classIdDict)
        {
            builder.AppendLine($"            case 0x{pair.Key:X8}: classPart = 0x{pair.Value:X8}; break;");
        }

        builder.AppendLine("        }");
        builder.AppendLine();
        builder.AppendLine("        return classPart | chunkPart;");
        builder.AppendLine("    }");
        builder.AppendLine("}");

        context.AddSource($"ClassManager.{name}", builder.ToString());
    }

    private void GenerateWrapSource(SourceProductionContext context, TextLineCollection lines)
    {
        GenerateSource(context, lines, "Wrap");
    }

    private void GenerateUnwrapSource(SourceProductionContext context, TextLineCollection lines)
    {
        GenerateSource(context, lines, "Unwrap");
    }
}