using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Generators;

[Generator(LanguageNames.CSharp)]
public class LibGenerator : IIncrementalGenerator
{
    private const bool Debug = false;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        if (Debug && !Debugger.IsAttached)
        {
            Debugger.Launch();
        }
    }
}