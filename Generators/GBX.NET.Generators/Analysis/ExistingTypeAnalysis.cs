using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Threading;

namespace GBX.NET.Generators.Analysis;

internal static class ExistingTypeAnalysis
{
    private const string EnginesNamespace = "GBX.NET.Engines";

    public static bool IsCandidate(SyntaxNode node) =>
        node is BaseTypeDeclarationSyntax;

    public static ExistingType? Read(SyntaxNode node, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var declaration = (BaseTypeDeclarationSyntax)node;
        var ancestors = declaration.Ancestors().ToArray();
        var @namespace = string.Join(".", ancestors.OfType<BaseNamespaceDeclarationSyntax>()
            .Reverse().Select(static x => string.Join(".", x.Name.DescendantTokens()
                .Where(static t => t.IsKind(SyntaxKind.IdentifierToken))
                .Select(static t => t.ValueText))));

        if (!@namespace.StartsWith(EnginesNamespace + ".", StringComparison.Ordinal))
        {
            return null;
        }

        var containingTypes = ancestors.OfType<TypeDeclarationSyntax>().Reverse().ToArray();
        // File-local types cannot be augmented from generated files. A partial inside
        // a file-local container has the same restriction, so omit that subtree too.
        if (declaration.Modifiers.Any(static x => x.ValueText == "file") ||
            containingTypes.Any(static x => x.Modifiers.Any(static t => t.ValueText == "file")))
        {
            return null;
        }

        var containingTypeKey = containingTypes.Length == 0 ? null :
            @namespace + "." + string.Join("+", containingTypes.Select(GetMetadataName));
        var key = (containingTypeKey is null ? @namespace + "." : containingTypeKey + "+") +
            GetMetadataName(declaration);

        return new ExistingType(@namespace, declaration.Identifier.ValueText, GetArity(declaration),
            containingTypeKey, key, ImmutableArray.Create(declaration));
    }

    public static ImmutableDictionary<string, ExistingType> Merge(
        ImmutableArray<ExistingType> declarations, CancellationToken token)
    {
        var types = ImmutableDictionary.CreateBuilder<string, ExistingType>(StringComparer.Ordinal);

        foreach (var group in declarations.GroupBy(static x => x.Key, StringComparer.Ordinal))
        {
            token.ThrowIfCancellationRequested();
            var first = group.First();
            types.Add(group.Key, new ExistingType(first.Namespace, first.Name, first.Arity,
                first.ContainingTypeKey, group.Key,
                group.SelectMany(static x => x.Declarations).ToImmutableArray()));
        }

        return types.ToImmutable();
    }

    private static int GetArity(BaseTypeDeclarationSyntax declaration) =>
        (declaration as TypeDeclarationSyntax)?.TypeParameterList?.Parameters.Count ?? 0;

    private static string GetMetadataName(BaseTypeDeclarationSyntax declaration)
    {
        var arity = GetArity(declaration);
        return declaration.Identifier.ValueText + (arity == 0 ? "" : "`" + arity);
    }

}
