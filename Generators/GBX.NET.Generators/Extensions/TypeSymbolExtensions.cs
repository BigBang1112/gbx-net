using Microsoft.CodeAnalysis;

namespace GBX.NET.Generators.Extensions;

internal static class TypeSymbolExtensions
{
    public static IEnumerable<ISymbol> GetAllMembers(this ITypeSymbol symbol)
    {
        var currentSymbol = symbol;

        while (currentSymbol is not null)
        {
            foreach (var s in currentSymbol.GetMembers())
            {
                yield return s;
            }

            currentSymbol = currentSymbol.BaseType;
        }
    }
}
