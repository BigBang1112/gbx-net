using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GBX.NET.Generators.Generation;

internal sealed class IdentifierRewriter(Func<string, string> map, Func<ExpressionSyntax, bool>? isTimeInt32 = null,
    Func<string, string?>? nodeRef = null, Func<string, string?>? storedRef = null) : CSharpSyntaxRewriter
{
    private static readonly ExpressionSyntax TimeZero = SyntaxFactory.MemberAccessExpression(
        SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName("TimeInt32"), SyntaxFactory.IdentifierName("Zero"));

    public override SyntaxNode? VisitBinaryExpression(BinaryExpressionSyntax node)
    {
        if (nodeRef is not null && node.Kind() is SyntaxKind.EqualsExpression or SyntaxKind.NotEqualsExpression)
        {
            var candidate = Unwrap(node.Left).IsKind(SyntaxKind.NullLiteralExpression) ? node.Right :
                Unwrap(node.Right).IsKind(SyntaxKind.NullLiteralExpression) ? node.Left : null;
            if (candidate is not null && ReferenceHandle(candidate) is { } handle)
                return SyntaxFactory.ParseExpression(handle + ".Kind " + (node.IsKind(SyntaxKind.EqualsExpression) ? "==" : "!=") + " NodeRefKind.None");
        }
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
        var pattern = node.Pattern;
        var negated = false;
        while (pattern is UnaryPatternSyntax unary && unary.IsKind(SyntaxKind.NotPattern))
        {
            negated = !negated;
            pattern = unary.Pattern;
        }
        if (pattern is ConstantPatternSyntax constant && Unwrap(constant.Expression).IsKind(SyntaxKind.NullLiteralExpression) &&
            ReferenceHandle(node.Expression) is { } handle)
            return SyntaxFactory.ParseExpression(handle + ".Kind " + (negated ? "!=" : "==") + " NodeRefKind.None");
        var rewritten = (IsPatternExpressionSyntax)base.VisitIsPatternExpression(node)!;
        return rewritten.WithExpression(rewritten.Expression.WithTriviaFrom(node.Expression));
    }

    private static ExpressionSyntax Unwrap(ExpressionSyntax expression)
    {
        while (expression is ParenthesizedExpressionSyntax parentheses) expression = parentheses.Expression;
        return expression;
    }

    private string? ReferenceHandle(ExpressionSyntax expression)
    {
        expression = Unwrap(expression);
        var name = expression is IdentifierNameSyntax identifier ? identifier.Identifier.ValueText :
            expression is MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax { Identifier.ValueText: "n" or "this" } } member ? member.Name.Identifier.ValueText : null;
        return name is null ? null : nodeRef?.Invoke(name);
    }

    public override SyntaxNode? VisitIdentifierName(IdentifierNameSyntax node)
    {
        if (node.Parent is MemberAccessExpressionSyntax member && member.Name == node)
        {
            return node;
        }

        return SyntaxFactory.ParseExpression(map(node.Identifier.ValueText));
    }

    public override SyntaxNode? VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
    {
        if (storedRef is not null && node.Expression is IdentifierNameSyntax { Identifier.ValueText: "n" or "this" } &&
            storedRef(node.Name.Identifier.ValueText) is { } handle)
            return SyntaxFactory.ParseExpression(handle + ".CachedTarget");
        return base.VisitMemberAccessExpression(node);
    }
}
