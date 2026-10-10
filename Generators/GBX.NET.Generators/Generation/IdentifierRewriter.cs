using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GBX.NET.Generators.Generation;

internal sealed class IdentifierRewriter(Func<string, string> map, Func<ExpressionSyntax, bool>? isTimeInt32 = null) : CSharpSyntaxRewriter
{
    private static readonly ExpressionSyntax TimeZero = SyntaxFactory.MemberAccessExpression(
        SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName("TimeInt32"), SyntaxFactory.IdentifierName("Zero"));

    public override SyntaxNode? VisitBinaryExpression(BinaryExpressionSyntax node)
    {
        var rewritten = (BinaryExpressionSyntax)base.VisitBinaryExpression(node)!;
        if (isTimeInt32 is not null && node.Kind() is SyntaxKind.EqualsExpression or SyntaxKind.NotEqualsExpression or
            SyntaxKind.LessThanExpression or SyntaxKind.LessThanOrEqualExpression or
            SyntaxKind.GreaterThanExpression or SyntaxKind.GreaterThanOrEqualExpression)
        {
            if (IsZero(node.Left) && isTimeInt32(node.Right))
                rewritten = rewritten.WithLeft(TimeZero.WithTriviaFrom(rewritten.Left));
            if (IsZero(node.Right) && isTimeInt32(node.Left))
                rewritten = rewritten.WithRight(TimeZero.WithTriviaFrom(rewritten.Right));
        }

        return node.IsKind(SyntaxKind.IsExpression)
            ? rewritten.WithLeft(rewritten.Left.WithTriviaFrom(node.Left))
            : rewritten;
    }

    private static bool IsZero(ExpressionSyntax expression)
    {
        while (expression is ParenthesizedExpressionSyntax parentheses) expression = parentheses.Expression;
        return expression is LiteralExpressionSyntax { Token.Value: 0 };
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
