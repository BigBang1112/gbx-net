using GBX.NET.Generators.Extensions;
using Microsoft.CodeAnalysis;

namespace GBX.NET.Generators;

[Generator]
public class InputsAdditionsGenerator : SourceGenerator
{
    public override bool Debug => false;

    public override void Execute(GeneratorExecutionContext context)
    {
        var enginesNamespace = context.Compilation
            .GlobalNamespace
            .NavigateToNamespace("GBX.NET.Inputs") ?? throw new Exception("GBX.NET.Inputs namespace not found.");

        var inputSymbols = enginesNamespace.GetTypeMembers()
            .Where(x => x.IsValueType && x.Interfaces.Any(x => x.Name.StartsWith("IInput")));

        foreach (var symbol in inputSymbols)
        {
            var members = symbol.GetMembers()
                .OfType<IMethodSymbol>()
                .Where(x => x.MethodKind is MethodKind.Constructor)
                .FirstOrDefault()
                .Parameters
                .Where(x => x.Name != "Time")
                .Select(x => ", " + x.Name);

            context.AddSource($"{symbol.Name}.g.cs", @$"namespace GBX.NET.Inputs;

public readonly partial record struct {symbol.Name}
{{
    IInput IInput.WithTime(TimeInt32 time) => new {symbol.Name}(time{string.Join("", members)});
}}");
        }
    }
}
