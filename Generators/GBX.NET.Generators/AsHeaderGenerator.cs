using GBX.NET.Generators.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Diagnostics;

namespace GBX.NET.Generators;

[Generator]
public class AsHeaderGenerator : ISourceGenerator
{
    private const string ConstIHeader = "IHeader";

    public void Initialize(GeneratorInitializationContext context)
    {
#if DEBUG
        if (!Debugger.IsAttached)
        {
            //Debugger.Launch();
        }
#endif
    }

    public void Execute(GeneratorExecutionContext context)
    {
        // GBX.NET assembly
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
