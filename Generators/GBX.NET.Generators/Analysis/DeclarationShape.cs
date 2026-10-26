namespace GBX.NET.Generators.Analysis;

/// <summary>
/// Compares two versions of a handwritten declaration by the syntax the generator reads.
/// Member bodies are compared by presence and kind only, so an edit inside one leaves
/// the declaration equal and the generated code cached.
/// </summary>
internal static class DeclarationShape
{
    public static bool Equals(BaseTypeDeclarationSyntax left, BaseTypeDeclarationSyntax right, bool bodies)
    {
        if (!SameImports(left, right)) return false;

        // Tokens keep their trivia: generated code copies type and parameter text verbatim.
        return bodies ? left.ToFullString() == right.ToFullString() : Same(left, right);
    }

    private static bool Same(SyntaxNodeOrToken left, SyntaxNodeOrToken right)
    {
        if (left.RawKind != right.RawKind) return false;
        if (left.IsToken) return left.ToFullString() == right.ToFullString();

        var leftNode = left.AsNode()!;
        if (IsBody(leftNode)) return true;

        var leftChildren = leftNode.ChildNodesAndTokens();
        var rightChildren = right.AsNode()!.ChildNodesAndTokens();
        if (leftChildren.Count != rightChildren.Count) return false;

        for (var i = 0; i < leftChildren.Count; i++)
        {
            if (!Same(leftChildren[i], rightChildren[i])) return false;
        }

        return true;
    }

    private static bool IsBody(SyntaxNode node) => node is BlockSyntax or ArrowExpressionClauseSyntax &&
        node.Parent is BaseMethodDeclarationSyntax or AccessorDeclarationSyntax or BasePropertyDeclarationSyntax;

    // Parameter comparison resolves aliases from the enclosing namespaces and the compilation unit.
    private static bool SameImports(SyntaxNode left, SyntaxNode right)
    {
        var leftImports = Imports(left);
        var rightImports = Imports(right);
        if (leftImports.Count != rightImports.Count) return false;

        for (var i = 0; i < leftImports.Count; i++)
        {
            if (leftImports[i].ToString() != rightImports[i].ToString()) return false;
        }

        return true;
    }

    private static List<UsingDirectiveSyntax> Imports(SyntaxNode node) => node.Ancestors()
        .OfType<BaseNamespaceDeclarationSyntax>().SelectMany(static x => x.Usings)
        .Concat(node.SyntaxTree.GetCompilationUnitRoot().Usings).ToList();
}
