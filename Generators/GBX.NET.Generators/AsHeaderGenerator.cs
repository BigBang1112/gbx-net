using GBX.NET.Generators.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Diagnostics;

namespace GBX.NET.Generators;

[Generator]
public class AsHeaderGenerator : ISourceGenerator
{
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
        var enginesNamespace = context.Compilation.GlobalNamespace.NavigateToNamespace("GBX.NET.Engines") ?? throw new Exception("GBX.NET.Engines namespace not found.");
        var engineTypes = enginesNamespace.GetNamespaceMembers().SelectMany(x => x.GetTypeMembers());
    }
}
