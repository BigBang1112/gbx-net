using ChunkL;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Generators;

[Generator(LanguageNames.CSharp)]
public class ClassIdGenerator : IIncrementalGenerator
{
    private const bool Debug = false;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        if (Debug && !Debugger.IsAttached)
        {
            Debugger.Launch();
        }

        var existingClasses = context.CompilationProvider.Select((compilation, token) =>
        {
            var enginesNamespace = compilation.GlobalNamespace.GetNamespaceMembers()
                .FirstOrDefault(x => x.Name == "GBX")
                .GetNamespaceMembers()
                .FirstOrDefault(x => x.Name == "NET")
                .GetNamespaceMembers()
                .FirstOrDefault(x => x.Name == "Engines");

            var dict = enginesNamespace.GetNamespaceMembers()
                .SelectMany(x => x.GetTypeMembers())
                .ToDictionary(x => x.Name);

            return dict;
        });

        var classIdTextFiles = context.AdditionalTextsProvider
            .Where(static file => file.Path.EndsWith("ClassId.txt") && Path.GetDirectoryName(file.Path).EndsWith("Resources"));

        var classIdContents = classIdTextFiles.Select((additionalText, cancellationToken) =>
        {
            var text = additionalText.GetText(cancellationToken) ?? throw new Exception("Could not get text from file.");
            return text.ToString();
        });

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

        var combined = classIdContents
            .Combine(existingClasses)
            .Combine(chunklFiles.Collect());

        var transformed = combined.Select(static (source, token) =>
        {
            var ((classIdContents, existingClasses), chunkLData) = source;

            return new GetNameGenerationSource(classIdContents, existingClasses, chunkLData);
        });

        context.RegisterSourceOutput(transformed, GenerateSource);
    }

    private record ChunkLData(ChunkLDataModel DataModel, string Engine);
    private record GetNameGenerationSource(string Contents, Dictionary<string, INamedTypeSymbol> ExistingClasses, ImmutableArray<ChunkLData> ChunkLData);

    private void GenerateSource(SourceProductionContext context, GetNameGenerationSource source)
    {
        using var reader = new StringReader(source.Contents);
        var dict = Parser.Parse(reader);

        var builder = new StringBuilder();

        builder.AppendLine("namespace GBX.NET.Managers;");
        builder.AppendLine();
        builder.AppendLine("public static partial class ClassManager");
        builder.AppendLine("{");
        builder.AppendLine("    public static partial string? GetName(uint classId) => classId switch");
        builder.AppendLine("    {");

        foreach (var pair in dict)
        {
            if (source.ExistingClasses.TryGetValue(pair.Value, out var existingClass))
            {
                builder.AppendLine($"        0x{pair.Key:X8} => nameof({existingClass.ToDisplayString()}),");
            }
        }

        foreach (var chunkl in source.ChunkLData)
        {
            if (source.ExistingClasses.ContainsKey(chunkl.DataModel.Header.Name))
            {
                continue;
            }

            builder.AppendLine($"        0x{chunkl.DataModel.Header.Id:X8} => nameof(GBX.NET.Engines.{chunkl.Engine}.{chunkl.DataModel.Header.Name}),");
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");
        builder.AppendLine();
        builder.AppendLine("    public static partial string? GetName(uint classId, bool all)");
        builder.AppendLine("    {");
        builder.AppendLine("        if (!all) return GetName(classId);");
        builder.AppendLine("        return classId switch");
        builder.AppendLine("        {");

        foreach (var pair in dict)
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

    private static class Parser
    {
        public static Dictionary<uint, string> Parse(TextReader reader)
        {
            var result = new Dictionary<uint, string>();

            var currentEngine = new ReadOnlySpan<char>();

            byte currentEngineByte = 0;

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var lineSpan = line.AsSpan().TrimEnd();

                if (lineSpan.Length == 0)
                {
                    continue;
                }

                var isClassId = lineSpan[0] is ' ' or '\t';

                if (currentEngine.IsEmpty && isClassId)
                {
                    throw new Exception("Invalid format.");
                }

                if (isClassId)
                {
                    var classSpan = lineSpan.TrimStart();

                    var spaceIndex = classSpan.IndexOf(' ');

                    //var currentEngineName = string.Empty;
                    ReadOnlySpan<char> currentClass;
                    string currentClassName;

                    if (spaceIndex == -1)
                    {
                        currentClass = classSpan;
                        currentClassName = string.Empty;
                    }
                    else
                    {
                        currentClass = classSpan.Slice(0, spaceIndex);
                        currentClassName = classSpan.Slice(spaceIndex + 1).ToString();
                    }

                    var a = currentClass[0];
                    var aOver9 = currentClass[0] > '9';
                    var b = currentClass[1];
                    var bOver9 = currentClass[1] > '9';
                    var c = currentClass[2];
                    var cOver9 = currentClass[2] > '9';

                    var classPart = (ushort)(
                          (a - (aOver9 ? 'A' : '0') + (aOver9 ? 10 : 0)) << 8
                        | (b - (bOver9 ? 'A' : '0') + (bOver9 ? 10 : 0)) << 4
                        | (c - (cOver9 ? 'A' : '0') + (cOver9 ? 10 : 0)));

                    // combine engine and class like EECCC000
                    var classId = (uint)((currentEngineByte << 24) | (classPart << 12));

                    result.Add(classId, currentClassName);
                }
                else
                {
                    var spaceIndex = lineSpan.IndexOf(' ');

                    if (spaceIndex == -1)
                    {
                        currentEngine = lineSpan;
                        //currentEngineName = string.Empty;
                    }
                    else
                    {
                        currentEngine = lineSpan.Slice(0, spaceIndex);
                        //currentEngineName = lineSpan.Slice(spaceIndex + 1).ToString();
                    }

                    var a = currentEngine[0];
                    var aOver9 = currentEngine[0] > '9';
                    var b = currentEngine[1];
                    var bOver9 = currentEngine[1] > '9';

                    currentEngineByte = (byte)(
                          (a - (aOver9 ? 'A' : '0') + (aOver9 ? 10 : 0)) << 4
                        | (b - (bOver9 ? 'A' : '0') + (bOver9 ? 10 : 0)));
                }
            }

            return result;
        }
    }
}