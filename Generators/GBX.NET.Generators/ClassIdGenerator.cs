using Microsoft.CodeAnalysis;
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

        var textFiles = context.AdditionalTextsProvider
            .Where(static file => file.Path.EndsWith("ClassId.txt") && Path.GetDirectoryName(file.Path).EndsWith("Resources"));

        var namesAndContents = textFiles.Select((additionalText, cancellationToken) =>
        {
            var text = additionalText.GetText(cancellationToken) ?? throw new Exception("Could not get text from file.");

            return text.ToString();
        });  

        context.RegisterSourceOutput(namesAndContents, GenerateSource);
    }

    private void GenerateSource(SourceProductionContext context, string contents)
    {
        using var reader = new StringReader(contents);
        var dict = Parser.Parse(reader);

        var builder = new StringBuilder();

        builder.AppendLine("using GBX.NET.Components;");
        builder.AppendLine();
        builder.AppendLine("namespace GBX.NET;");
        builder.AppendLine();
        builder.AppendLine("public static partial class ClassManager");
        builder.AppendLine("{");
        builder.AppendLine("    public static partial string GetName(uint classId) => classId switch");
        builder.AppendLine("    {");

        /*foreach (var pair in dict)
        {
            builder.AppendLine($"        0x{pair.Key:X8} => \"{pair.Value}\",");
        }*/

        builder.AppendLine("        _ => throw new NotImplementedException()");
        builder.AppendLine("    };");
        builder.AppendLine("}");

        context.AddSource("ClassManager.ClassId", builder.ToString());
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