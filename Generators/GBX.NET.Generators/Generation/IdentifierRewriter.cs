using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GBX.NET.Generators.Generation;

internal sealed class IdentifierRewriter(Func<string, string> map) : CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitBinaryExpression(BinaryExpressionSyntax node)
    {
        var rewritten = (BinaryExpressionSyntax)base.VisitBinaryExpression(node)!;
        return node.IsKind(SyntaxKind.IsExpression)
            ? rewritten.WithLeft(rewritten.Left.WithTriviaFrom(node.Left))
            : rewritten;
    }

    public override SyntaxNode? VisitIsPatternExpression(IsPatternExpressionSyntax node)
    {
        var rewritten = (IsPatternExpressionSyntax)base.VisitIsPatternExpression(node)!;
        return rewritten.WithExpression(rewritten.Expression.WithTriviaFrom(node.Expression));
    }

    public override SyntaxNode? VisitIdentifierName(IdentifierNameSyntax node)
    {
        if (node.Parent is MemberAccessExpressionSyntax member && member.Name == node)
        {
            return node;
        }
        
        return SyntaxFactory.ParseExpression(map(node.Identifier.ValueText));
    }
}
