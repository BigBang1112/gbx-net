using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Generators;

[Generator]
public class GbxReaderAndWriterArrayGenerator : IIncrementalGenerator
{
    private const bool Debug = false;

    public virtual void Initialize(IncrementalGeneratorInitializationContext context)
    {
        if (Debug && !Debugger.IsAttached) Debugger.Launch();

        var readerRefTypeMethods = context.CompilationProvider
            .Select(static (compilation, token) =>
            {
                var serializationNamespace = compilation.GlobalNamespace.GetNamespaceMembers()
                    .FirstOrDefault(x => x.Name == "GBX")
                    .GetNamespaceMembers()
                    .FirstOrDefault(x => x.Name == "NET")
                    .GetNamespaceMembers()
                    .FirstOrDefault(x => x.Name == "Serialization");

                return serializationNamespace.GetTypeMembers()
                    .First(x => x.Name == "IGbxReader")
                    .GetMembers()
                    .OfType<IMethodSymbol>()
                    .Where(x => !x.ReturnType.IsValueType
                        && x.Parameters.Length == 0
                        && x.Name.StartsWith("Read")
                        && !string.IsNullOrEmpty(x.ReturnType.Name)
                        && x.Name.Substring("Read".Length).StartsWith(x.ReturnType.Name))
                    .ToImmutableArray();
            });

        context.RegisterSourceOutput(readerRefTypeMethods, GenerateReaderSource);
    }

    private void GenerateReaderSource(SourceProductionContext context, ImmutableArray<IMethodSymbol> methodSymbols)
    {
        var sb = new StringBuilder();
        sb.AppendLine("namespace GBX.NET.Serialization;");
        sb.AppendLine();

        sb.AppendLine("partial interface IGbxReader");
        sb.AppendLine("{");

        foreach (var symbol in methodSymbols)
        {
            sb.AppendLine($"    {symbol.ReturnType}[] ReadArray{symbol.ReturnType.Name}(int length);");
            sb.AppendLine($"    {symbol.ReturnType}[] ReadArray{symbol.ReturnType.Name}();");
            sb.AppendLine($"    {symbol.ReturnType}[] ReadArray{symbol.ReturnType.Name}_deprec();");
            sb.AppendLine($"    IList<{symbol.ReturnType}> ReadList{symbol.ReturnType.Name}(int length);");
            sb.AppendLine($"    IList<{symbol.ReturnType}> ReadList{symbol.ReturnType.Name}();");
            sb.AppendLine($"    IList<{symbol.ReturnType}> ReadList{symbol.ReturnType.Name}_deprec();");
        }

        sb.AppendLine("}");

        sb.AppendLine();

        sb.AppendLine("partial class GbxReader");
        sb.AppendLine("{");

        foreach (var symbol in methodSymbols)
        {
            sb.Append("    public ");
            sb.Append(symbol.ReturnType);
            sb.Append("[] ReadArray");
            sb.Append(symbol.ReturnType.Name);
            sb.AppendLine("(int length)");
            sb.AppendLine("    {");
            sb.Append("        var array = new ");
            sb.Append(symbol.ReturnType);
            sb.AppendLine("[length];");
            sb.AppendLine();
            sb.AppendLine("        for (int i = 0; i < length; i++)");
            sb.AppendLine("        {");
            sb.Append("            array[i] = ");
            sb.Append(symbol.Name);
            sb.AppendLine("();");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        return array;");
            sb.AppendLine("    }");

            sb.AppendLine();

            sb.Append("    public ");
            sb.Append(symbol.ReturnType);
            sb.Append("[] ReadArray");
            sb.Append(symbol.ReturnType.Name);
            sb.Append("() => ReadArray");
            sb.Append(symbol.ReturnType.Name);
            sb.AppendLine("(ReadInt32());");

            sb.AppendLine();

            sb.Append("    public ");
            sb.Append(symbol.ReturnType);
            sb.Append("[] ReadArray");
            sb.Append(symbol.ReturnType.Name);
            sb.AppendLine("_deprec()");
            sb.AppendLine("    {");
            sb.AppendLine("        ReadDeprecVersion();");
            sb.Append("        return ReadArray");
            sb.Append(symbol.ReturnType.Name);
            sb.AppendLine("();");
            sb.AppendLine("    }");

            sb.AppendLine();

            sb.Append("    public IList<");
            sb.Append(symbol.ReturnType);
            sb.Append("> ReadList");
            sb.Append(symbol.ReturnType.Name);
            sb.AppendLine("(int length)");
            sb.AppendLine("    {");
            sb.Append("        var list = new List<");
            sb.Append(symbol.ReturnType);
            sb.AppendLine(">(length);");
            sb.AppendLine();
            sb.AppendLine("        for (int i = 0; i < length; i++)");
            sb.AppendLine("        {");
            sb.Append("            list.Add(");
            sb.Append(symbol.Name);
            sb.AppendLine("());");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        return list;");
            sb.AppendLine("    }");

            sb.AppendLine();

            sb.Append("    public IList<");
            sb.Append(symbol.ReturnType);
            sb.Append("> ReadList");
            sb.Append(symbol.ReturnType.Name);
            sb.Append("() => ReadList");
            sb.Append(symbol.ReturnType.Name);
            sb.AppendLine("(ReadInt32());");

            sb.AppendLine();

            sb.Append("    public IList<");
            sb.Append(symbol.ReturnType);
            sb.Append("> ReadList");
            sb.Append(symbol.ReturnType.Name);
            sb.AppendLine("_deprec()");
            sb.AppendLine("    {");
            sb.AppendLine("        ReadDeprecVersion();");
            sb.Append("        return ReadList");
            sb.Append(symbol.ReturnType.Name);
            sb.AppendLine("();");
            sb.AppendLine("    }");

            sb.AppendLine();
        }

        sb.AppendLine("}");

        context.AddSource("GbxReader", sb.ToString());
    }
}