using Microsoft.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace GBX.NET.Generators;

[Generator]
public class NodeManagerMappingsGenerator : SourceGenerator
{
    public override bool Debug => false;

    public override void Execute(GeneratorExecutionContext context)
    {
        var classIdMappingsFileContents = context.AdditionalFiles
            .First(x => x.Path.EndsWith("ClassIDMappings.txt"))
            .GetText(context.CancellationToken) ?? throw new Exception();
        
        var mappingBuilder = new StringBuilder("namespace GBX.NET;\n\n");
        mappingBuilder.AppendLine("public static partial class NodeManager");
        mappingBuilder.AppendLine("{");
        mappingBuilder.AppendLine("    public static uint? GetMapping(uint classId) => classId switch");
        mappingBuilder.AppendLine("    {");

        var mappings = new List<(uint, uint)>();

        foreach (var textLine in classIdMappingsFileContents.Lines)
        {
            var stringLine = textLine.ToString();

            if (stringLine is null)
            {
                break;
            }

            if (stringLine == "")
            {
                continue;
            }

            var line = stringLine.AsSpan();

            var from = line.Slice(0, 8);
            var to = line.Slice(12, 8);

#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
            if (!uint.TryParse(from, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint key)
            || !uint.TryParse(to, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint value))
            {
                continue;
            }
#else
            if (!uint.TryParse(from.ToString(), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint key)
            || !uint.TryParse(to.ToString(), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint value))
            {
                continue;
            }
#endif

            mappingBuilder.Append("        ");
            mappingBuilder.Append(key);
            mappingBuilder.Append(" => ");
            mappingBuilder.Append(value);
            mappingBuilder.AppendLine(",");

            mappings.Add((key, value));
        }
        
        mappingBuilder.AppendLine("        _ => null");
        mappingBuilder.AppendLine("    };");
        mappingBuilder.AppendLine("}");

        context.AddSource("NodeManager.Mappings.g.cs", mappingBuilder.ToString());
        

        var reverseMappingBuilder = new StringBuilder("namespace GBX.NET;\n\n");
        reverseMappingBuilder.AppendLine("public static partial class NodeManager");
        reverseMappingBuilder.AppendLine("{");
        reverseMappingBuilder.AppendLine("    public static uint? GetReverseMapping(uint classId) => classId switch");
        reverseMappingBuilder.AppendLine("    {");

        mappings.Reverse();

        var alreadyAdded = new HashSet<uint>();

        foreach (var (key, value) in mappings)
        {
            if (alreadyAdded.Contains(value))
            {
                continue;
            }

            reverseMappingBuilder.Append("        ");
            reverseMappingBuilder.Append(value);
            reverseMappingBuilder.Append(" => ");
            reverseMappingBuilder.Append(key);
            reverseMappingBuilder.AppendLine(",");

            alreadyAdded.Add(value);
        }

        reverseMappingBuilder.AppendLine("        _ => null");
        reverseMappingBuilder.AppendLine("    };");
        reverseMappingBuilder.AppendLine("}");

        context.AddSource("NodeManager.ReverseMappings.g.cs", reverseMappingBuilder.ToString());
    }
}
