using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Generators;

[Generator(LanguageNames.CSharp)]
public class ClassTypeGenerator : IIncrementalGenerator
{
    private const bool Debug = false;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        if (Debug && !Debugger.IsAttached)
        {
            Debugger.Launch();
        }

        var namesAndContents = context.CompilationProvider.Select(GetTypeSymbols);  

        context.RegisterSourceOutput(namesAndContents, GenerateSource);
    }

    private IEnumerable<INamespaceSymbol> RecurseNamespaces(INamespaceSymbol namespaceSymbol)
    {
        yield return namespaceSymbol;

        foreach (var n in namespaceSymbol.GetNamespaceMembers())
        {
            foreach (var nn in RecurseNamespaces(n))
            {
                yield return nn;
            }
        }
    }

    private IEnumerable<(INamedTypeSymbol, uint classId)> GetTypeSymbols(Compilation compilation, CancellationToken token)
    {
        var enginesNamespace = compilation.GlobalNamespace.GetNamespaceMembers()
            .FirstOrDefault(x => x.Name == "GBX")
            .GetNamespaceMembers()
            .FirstOrDefault(x => x.Name == "NET")
            .GetNamespaceMembers()
            .FirstOrDefault(x => x.Name == "Engines");

        foreach (var typeSymbol in RecurseNamespaces(enginesNamespace).SelectMany(x => x.GetTypeMembers()))
        {
            if (typeSymbol.AllInterfaces.Any(x => x.Name == "IClass"))
            {
                var attribute = typeSymbol.GetAttributes().FirstOrDefault(x => x.AttributeClass?.Name == "ClassAttribute");

                if (attribute is null)
                {
                    continue;
                }

                yield return (typeSymbol, classId: (uint)(attribute.ConstructorArguments[0].Value ?? -1));
            }
        }
    }

    private void GenerateSource(SourceProductionContext context, IEnumerable<(INamedTypeSymbol, uint classId)> symbols)
    {
        var builder = new StringBuilder();

        builder.AppendLine("using GBX.NET.Components;");
        builder.AppendLine();
        builder.AppendLine("namespace GBX.NET;");
        builder.AppendLine();
        builder.AppendLine("public static partial class ClassManager");
        builder.AppendLine("{");
        builder.AppendLine("    internal static Dictionary<Type, uint> ClassIds { get; } = new()");
        builder.AppendLine("    {");

        foreach (var (symbol, classId) in symbols)
        {
            builder.AppendLine($"        {{ typeof({symbol.Name}), 0x{classId:X8} }},");
        }

        builder.AppendLine("    };");
        builder.AppendLine();
        builder.AppendLine("    public static partial Type? GetType(uint classId) => classId switch");
        builder.AppendLine("    {");

        foreach (var (symbol, classId) in symbols)
        {
            builder.AppendLine($"        0x{classId:X8} => typeof({symbol.Name}),");
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");
        builder.AppendLine();
        builder.AppendLine("    internal static partial GbxHeader? NewHeader(GbxHeaderBasic basic, uint classId)");
        builder.AppendLine("    {");
        builder.AppendLine("        return classId switch");
        builder.AppendLine("        {");

        foreach (var (symbol, classId) in symbols)
        {
            builder.AppendLine($"            0x{classId:X8} => new GbxHeader<{symbol.Name}>(basic),");
        }

        builder.AppendLine("            _ => null");
        builder.AppendLine("        };");
        builder.AppendLine("    }");
        builder.AppendLine();
        builder.AppendLine("    internal static partial Gbx? NewGbx(GbxHeader header, IClass node)");
        builder.AppendLine("    {");
        builder.AppendLine("        return header.ClassId switch");
        builder.AppendLine("        {");

        foreach (var (symbol, classId) in symbols)
        {
            builder.AppendLine($"            0x{classId:X8} => new Gbx<{symbol.Name}>((GbxHeader<{symbol.Name}>)header, ({symbol.Name})node),");
        }

        builder.AppendLine("            _ => null");
        builder.AppendLine("        };");
        builder.AppendLine("    }");

        builder.AppendLine("}");

        context.AddSource("ClassManager.ClassType", builder.ToString());
    }
}