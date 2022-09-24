using Microsoft.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace GBX.NET.Generators;

[Generator]
public class NodeManagerNamesGenerator : SourceGenerator
{
    public override bool Debug => false;

    public override void Execute(GeneratorExecutionContext context)
    {
        var classIdFileContents = context.AdditionalFiles
            .First(x => x.Path.EndsWith("ClassID.txt"))
            .GetText(context.CancellationToken) ?? throw new Exception();

        var namesBuilder = new StringBuilder("namespace GBX.NET;\n\n#nullable enable\n\n");
        namesBuilder.AppendLine("public static partial class NodeManager");
        namesBuilder.AppendLine("{");
        namesBuilder.AppendLine("    public static string? GetName(uint classId) => classId switch");
        namesBuilder.AppendLine("    {");

        var extensionsBuilder = new StringBuilder("namespace GBX.NET;\n\n#nullable enable\n\n");
        extensionsBuilder.AppendLine("public static partial class NodeManager");
        extensionsBuilder.AppendLine("{");
        extensionsBuilder.AppendLine("    public static string? GetExtension(uint classId) => classId switch");
        extensionsBuilder.AppendLine("    {");

        Span<char> classIdSpan = stackalloc char[] { '0', '0', '0', '0', '0', '0', '0', '0' };
        var engineNameSpan = new ReadOnlySpan<char>();

        foreach (var textLine in classIdFileContents.Lines)
        {
            var line = textLine.ToString().AsSpan();
            
            if (!line.StartsWith(stackalloc char[] { ' ', ' ' }))
            {
                var engine = line.Slice(0, 2);
                classIdSpan[0] = engine[0];
                classIdSpan[1] = engine[1];

                if (line.Length > 3)
                {
                    engineNameSpan = line.Slice(3, line.Length - 3);
                }

                continue;
            }

            var classIdPart = line.Slice(2, 3);

            for (var i = 0; i < 3; i++)
            {
                classIdSpan[2 + i] = classIdPart[i];
            }

            var classNameWithExtensionSpan = new ReadOnlySpan<char>();

            if (line.Length <= 6)
            {
                continue;
            }

            classNameWithExtensionSpan = line.Slice(6, line.Length - 6);
            var classNameWithExtensionSpaceIndex = classNameWithExtensionSpan.IndexOf(' ');
            var noExtension = classNameWithExtensionSpaceIndex == -1;

            var classNameSpan = noExtension
                ? classNameWithExtensionSpan
                : classNameWithExtensionSpan.Slice(0, classNameWithExtensionSpaceIndex);

            var extensionSpan = noExtension
                ? new ReadOnlySpan<char>()
                : classNameWithExtensionSpan.Slice(classNameWithExtensionSpaceIndex + 1,
                    length: classNameWithExtensionSpan.Length - classNameWithExtensionSpaceIndex - 1);

#if NET462 || NETSTANDARD2_0
            if (!uint.TryParse(classIdSpan.ToString(), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint classID))
#else
            if (!uint.TryParse(classIdSpan, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint classID))
#endif
            {
                continue;
            }

#if NET6_0_OR_GREATER
            var fullName = string.Concat(engineNameSpan, "::", classNameSpan); // .NET Core 3+
#else
            var fullNameSpan = new Span<char>(new char[engineNameSpan.Length + 2 + classNameSpan.Length]);

            var ii = 0;
            engineNameSpan.CopyTo(fullNameSpan.Slice(ii, engineNameSpan.Length));
            ii += engineNameSpan.Length;

            new ReadOnlySpan<char>(new char[] { ':', ':' }).CopyTo(fullNameSpan.Slice(ii, 2));
            ii += 2;

            classNameSpan.CopyTo(fullNameSpan.Slice(ii, classNameSpan.Length));

            var fullName = fullNameSpan.ToString();
#endif

            namesBuilder.Append("        ");
            namesBuilder.Append(classID);
            namesBuilder.Append(" => \"");
            namesBuilder.Append(fullName);
            namesBuilder.AppendLine("\",");

            if (!extensionSpan.IsEmpty)
            {
                extensionsBuilder.Append("        ");
                extensionsBuilder.Append(classID);
                extensionsBuilder.Append(" => \"");
                extensionsBuilder.Append(extensionSpan.ToString());
                extensionsBuilder.AppendLine("\",");
            }
        }

        namesBuilder.AppendLine("        _ => null");
        namesBuilder.AppendLine("    };");
        namesBuilder.AppendLine("}");

        context.AddSource("NodeManager.Names.g.cs", namesBuilder.ToString());

        extensionsBuilder.AppendLine("        _ => null");
        extensionsBuilder.AppendLine("    };");
        extensionsBuilder.AppendLine("}");

        context.AddSource("NodeManager.Extensions.g.cs", extensionsBuilder.ToString());
    }
}
