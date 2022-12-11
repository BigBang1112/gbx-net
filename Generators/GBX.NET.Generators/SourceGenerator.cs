using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace GBX.NET.Generators;

public abstract class SourceGenerator : ISourceGenerator
{
    public abstract bool Debug { get; }

    public virtual void Initialize(GeneratorInitializationContext context)
    {
#if DEBUG
        if (!Debugger.IsAttached && Debug)
        {
            Debugger.Launch();
        }
#endif
    }

    public abstract void Execute(GeneratorExecutionContext context);
}
