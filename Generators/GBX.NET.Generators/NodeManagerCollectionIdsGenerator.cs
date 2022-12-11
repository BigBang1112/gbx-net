using Microsoft.CodeAnalysis;
using System.Text;

namespace GBX.NET.Generators;

[Generator]
public class NodeManagerCollectionsIdsGenerator : SourceGenerator
{
    public override bool Debug => false;

    public override void Execute(GeneratorExecutionContext context)
    {
        var collectionIdFileContents = context.AdditionalFiles
            .First(x => x.Path.EndsWith("CollectionID.txt"))
            .GetText(context.CancellationToken) ?? throw new Exception();
        
        var mappingBuilder = new StringBuilder("namespace GBX.NET;\n\n#nullable enable\n\n");
        mappingBuilder.AppendLine("public static partial class NodeManager");
        mappingBuilder.AppendLine("{");
        mappingBuilder.AppendLine("    public static string? GetCollectionName(int collectionId) => collectionId switch");
        mappingBuilder.AppendLine("    {");

        var dictionaryBuilder = new StringBuilder("namespace GBX.NET;\n\n#nullable enable\n\n");
        dictionaryBuilder.AppendLine("public static partial class NodeManager");
        dictionaryBuilder.AppendLine("{");
        dictionaryBuilder.AppendLine("    public static IReadOnlyDictionary<int, string> CollectionIds { get; } = new Dictionary<int, string>");
        dictionaryBuilder.AppendLine("    {");

        foreach (var textLine in collectionIdFileContents.Lines)
        {
            var stringLine = textLine.ToString();

            if (stringLine is null)
            {
                break;
            }

            var line = stringLine.AsSpan().TrimEnd();

            var spaceAtIndex = line.IndexOf(' ');

            if (spaceAtIndex == -1)
            {
                continue;
            }

            var key = line.Slice(0, spaceAtIndex);

            // Hack currently, should be fine with max 2 possible values
            var lastSpaceAtIndex = line.LastIndexOf(' ');
            var value = lastSpaceAtIndex == spaceAtIndex
                ? line.Slice(spaceAtIndex + 1)
                : line.Slice(spaceAtIndex + 1, lastSpaceAtIndex - spaceAtIndex - 1);
            //

            mappingBuilder.Append("        ");
            mappingBuilder.Append(key.ToString());
            mappingBuilder.Append(" => \"");
            mappingBuilder.Append(value.ToString());
            mappingBuilder.Append("\",\n");

            dictionaryBuilder.Append("        { ");
            dictionaryBuilder.Append(key.ToString());
            dictionaryBuilder.Append(", \"");
            dictionaryBuilder.Append(value.ToString());
            dictionaryBuilder.AppendLine("\" },");
        }

        mappingBuilder.AppendLine("        _ => null");
        mappingBuilder.AppendLine("    };");
        mappingBuilder.AppendLine("}");

        context.AddSource("NodeManager.CollectionIds.g.cs", mappingBuilder.ToString());

        dictionaryBuilder.AppendLine("    };");
        dictionaryBuilder.AppendLine("}");

        context.AddSource("NodeManager.CollectionIdsDict.g.cs", dictionaryBuilder.ToString());
    }
}
