using GBX.NET.Generators.Extensions;
using Microsoft.CodeAnalysis;
namespace GBX.NET.Generators;

[Generator]
public class AsHeaderGenerator : SourceGenerator
{
    private const string ConstIHeader = "IHeader";

    public override bool Debug => false;

    public override void Execute(GeneratorExecutionContext context)
    {
        var enginesNamespace = context.Compilation
            .GlobalNamespace
            .NavigateToNamespace("GBX.NET.Engines") ?? throw new Exception("GBX.NET.Engines namespace not found.");

        var engineTypesWithHeader = enginesNamespace.GetNamespaceMembers()
            .SelectMany(x => x.GetTypeMembers())
            .Where(x => x.Interfaces.Any(x => x.Name == ConstIHeader));

        foreach (var type in engineTypesWithHeader)
        {
            context.AddSource($"{type.Name}.g.cs", @$"namespace GBX.NET.Engines.{type.ContainingNamespace.Name};

public partial class {type.Name}
{{
    /// <summary>
    /// Gets the node casted to an interface that shows values used in the header.
    /// </summary>
    /// <returns>An <see cref=""IHeader""/> of the node.</returns>
    public {(type.BaseType?.Interfaces.Any(x => x.Name == ConstIHeader) == true ? "new " : "")}IHeader AsHeader()
    {{
        return this;
    }}
}}");
        }
    }
}
