using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Generators;

[Generator(LanguageNames.CSharp)]
public class InputWithTimeGenerator : IIncrementalGenerator
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

    private IEnumerable<INamedTypeSymbol> GetTypeSymbols(Compilation compilation, CancellationToken token)
    {
        var enginesNamespace = compilation.GlobalNamespace.GetNamespaceMembers()
            .FirstOrDefault(x => x.Name == "GBX")
            .GetNamespaceMembers()
            .FirstOrDefault(x => x.Name == "NET")
            .GetNamespaceMembers()
            .FirstOrDefault(x => x.Name == "Inputs");

        foreach (var typeSymbol in RecurseNamespaces(enginesNamespace).SelectMany(x => x.GetTypeMembers())
            .Where(x => x.IsValueType && x.Interfaces.Any(x => x.Name.StartsWith("IInput"))))
        {
            yield return typeSymbol;
        }
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

    private void GenerateSource(SourceProductionContext context, IEnumerable<INamedTypeSymbol> inputSymbols)
    {
        var builder = new StringBuilder();
        builder.AppendLine("namespace GBX.NET.Inputs;");

        foreach (var symbol in inputSymbols)
        {
            var members = symbol.GetMembers()
                .OfType<IMethodSymbol>()
                .Where(x => x.MethodKind is MethodKind.Constructor)
                .FirstOrDefault()
                .Parameters
                .Where(x => x.Name != "Time")
                .Select(x => ", " + x.Name);

            builder.AppendLine();
            builder.AppendLine($"public readonly partial record struct {symbol.Name}");
            builder.AppendLine("{");
            builder.AppendLine($"    IInput IInput.WithTime(TimeInt32 time) => new {symbol.Name}(time{string.Join("", members)});");
            builder.AppendLine("}");
        }

        context.AddSource("Input.WithTime", builder.ToString());
    }
}