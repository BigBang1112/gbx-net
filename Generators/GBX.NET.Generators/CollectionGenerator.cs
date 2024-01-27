using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Generators;

[Generator]
public class CollectionGenerator : IIncrementalGenerator
{
    private const bool Debug = false;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        if (Debug && !Debugger.IsAttached)
        {
            Debugger.Launch();
        }

        var contents = context.AdditionalTextsProvider
            .Where(static file =>
            {
                return file.Path.EndsWith("CollectionId.txt") && Path.GetDirectoryName(file.Path).EndsWith("Resources");
            })
            .Select((additionalText, cancellationToken) =>
            {
                var text = additionalText.GetText(cancellationToken) ?? throw new Exception("Could not get text from file.");
                return text.Lines;
            });

        context.RegisterSourceOutput(contents, GenerateSource);
    }

    private void GenerateSource(SourceProductionContext context, TextLineCollection lines)
    {
        var builder = new StringBuilder();

        builder.AppendLine("namespace GBX.NET.Managers;");
        builder.AppendLine();
        builder.AppendLine("public static partial class CollectionManager");
        builder.AppendLine("{");
        builder.AppendLine($"    public static partial string? GetName(int id) => id switch");
        builder.AppendLine("    {");

        foreach (var line in lines)
        {
            var split = line.ToString().Split(' ');

            builder.AppendLine($"        {int.Parse(split[0])} => \"{split[1]}\",");
        }

        builder.AppendLine("        _ => CustomCollections.TryGetValue(id, out var v) ? v : null");
        builder.AppendLine("    };");
        builder.AppendLine("}");

        context.AddSource("CollectionManager", builder.ToString());
    }
}