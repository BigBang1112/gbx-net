using ChunkL;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GBX.NET.Generators.Generation;

internal static class GameDefaultAttributesWriter
{
    public static void Write(CodeWriter code, FieldModel field)
    {
        var defaults = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var declaration in field.Occurrences)
        {
            foreach (var entry in declaration.GameDefaults)
            {
                var value = ChunkLParser.WriteExpression(entry.Value);
                if (defaults.TryGetValue(entry.Game, out var previous))
                {
                    if (!SyntaxFactory.AreEquivalent(SyntaxFactory.ParseExpression(previous), SyntaxFactory.ParseExpression(value)))
                        throw new InvalidOperationException($"Conflicting defaults for {field.Name} in {entry.Game}.");
                    continue;
                }

                defaults.Add(entry.Game, value);
            }
        }

        foreach (var entry in defaults)
        {
            var expression = SyntaxFactory.ParseExpression(entry.Value);
            var literal = expression is LiteralExpressionSyntax || expression is PrefixUnaryExpressionSyntax
                { Operand: LiteralExpressionSyntax } && (expression.IsKind(SyntaxKind.UnaryMinusExpression) || expression.IsKind(SyntaxKind.UnaryPlusExpression));
            code.Line(literal && entry.Value != "default"
                ? $"[GameVersionDefault(GameVersion.{entry.Key}, {entry.Value})]"
                : $"[GameVersionDefault(GameVersion.{entry.Key}, DefaultExpression = {Microsoft.CodeAnalysis.CSharp.SymbolDisplay.FormatLiteral(entry.Value, true)})]");
        }
    }
}
