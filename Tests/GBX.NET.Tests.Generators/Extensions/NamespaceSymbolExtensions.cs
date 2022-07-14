using Microsoft.CodeAnalysis;

namespace GBX.NET.Tests.Generators.Extensions;

internal static class NamespaceSymbolExtensions
{
    public static INamespaceSymbol NavigateToNamespace(this INamespaceSymbol namespaceSymbol, string namespacePath)
    {
        var parts = namespacePath.Split('.');

        foreach (var part in parts)
        {
            namespaceSymbol = namespaceSymbol.GetNamespaceMembers().First(x => x.Name == part);
        }
        
        return namespaceSymbol;
    }
}
