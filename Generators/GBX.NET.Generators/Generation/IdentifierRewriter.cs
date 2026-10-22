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
                return SyntaxFactory.ParseExpression("(" + handle + "?.Kind ?? NodeRefKind.None) " + (node.IsKind(SyntaxKind.EqualsExpression) ? "==" : "!=") + " NodeRefKind.None");
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
            return SyntaxFactory.ParseExpression("(" + handle + "?.Kind ?? NodeRefKind.None) " + (negated ? "!=" : "==") + " NodeRefKind.None");
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
        var name = ReferenceName(expression);
        return name is null ? null : nodeRef?.Invoke(name);
    }

    internal static string? ReferenceName(ExpressionSyntax expression)
    {
        expression = Unwrap(expression);
        return expression is IdentifierNameSyntax identifier ? identifier.Identifier.ValueText :
            expression is MemberAccessExpressionSyntax { Expression: ThisExpressionSyntax } thisMember ? thisMember.Name.Identifier.ValueText :
            expression is MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax { Identifier.ValueText: "n" or "this" } } member ? member.Name.Identifier.ValueText : null;
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
        if (ReferenceName(node.Expression) is { } name && storedRef?.Invoke(name) is { } nodeHandle)
            return node.WithExpression(SyntaxFactory.ParseExpression("(" + nodeHandle + "?.CachedNode)"));
        if (storedRef is not null && (node.Expression is ThisExpressionSyntax || node.Expression is IdentifierNameSyntax { Identifier.ValueText: "n" or "this" }) &&
            storedRef(node.Name.Identifier.ValueText) is { } handle)
            return SyntaxFactory.ParseExpression(handle + "?.CachedNode");
        return base.VisitMemberAccessExpression(node);
    }
}
