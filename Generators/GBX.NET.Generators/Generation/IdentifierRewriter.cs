using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GBX.NET.Generators.Generation;

internal sealed class IdentifierRewriter(Func<string, string> map) : CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitIdentifierName(IdentifierNameSyntax node)
    {
        if (node.Parent is MemberAccessExpressionSyntax member && member.Name == node)
        {
            return node;
        }
        
        return SyntaxFactory.ParseExpression(map(node.Identifier.ValueText));
    }
}
