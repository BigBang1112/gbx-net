using ChunkL;
using ChunkL.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GBX.NET.Generators.Generation;

internal static class AppliedWithChunkRanges
{
    internal readonly struct Range(int start, int end)
    {
        public int Start { get; } = start;
        public int End { get; } = end;
    }

    private static readonly Range All = new(0, int.MaxValue);

    public static IReadOnlyList<Range> Get(ChunkModel chunk, string field, IReadOnlyList<ChunkModel> chunks)
    {
        var result = new List<Range>();
        Collect(chunk, field, chunks, [All], result, new HashSet<uint>());
        return Normalize(result);
    }

    private static void Collect(ChunkModel chunk, string field, IReadOnlyList<ChunkModel> chunks,
        IReadOnlyList<Range> versions, List<Range> result, HashSet<uint> visited)
    {
        if (!visited.Add(chunk.Id)) return;

        var inherited = LayoutModel.Attribute(chunk.Declaration.Attributes, "base");
        ChunkModel? parent = null;
        if (inherited is not null)
        {
            var id = LayoutModel.Hex(inherited);
            if (id <= 0xFFF) id |= chunk.Id & 0xFFFFF000;
            parent = chunks.FirstOrDefault(x => x.Id == id);
        }

        if (chunk.Scope.Body.Count == 0 && parent is not null)
        {
            Collect(parent, field, chunks, versions, result, visited);
        }
        else
        {
            Walk(chunk.Scope.Body, versions);
        }

        visited.Remove(chunk.Id);

        void Walk(IEnumerable<IBodyStatement> statements, IReadOnlyList<Range> current)
        {
            if (current.Count == 0) return;

            foreach (var statement in statements)
            {
                switch (statement)
                {
                    case FieldDeclaration declaration when declaration.IsSpecialKeyword && declaration.Type.Name == "base":
                        if (parent is not null) Collect(parent, field, chunks, current, result, visited);
                        break;

                    case FieldDeclaration declaration when declaration.Name == field &&
                        chunk.Scope.Occurrences.TryGetValue(declaration, out var occurrence) && !occurrence.IsUnknown:
                        result.AddRange(current);
                        break;

                    case VersionCondition version:
                        var end = version.Kind switch
                        {
                            VersionConditionKind.Exact or VersionConditionKind.LessOrEqual => version.Version,
                            VersionConditionKind.Range => version.VersionEnd ?? int.MaxValue,
                            _ => int.MaxValue
                        };
                        var start = version.Kind == VersionConditionKind.LessOrEqual ? 0 : version.Version;
                        Walk(version.Body, Intersect(current, [new Range(start, end)]));
                        break;

                    case IfStatement conditional:
                    {
                        var remaining = current;
                        VisitBranch(conditional.Condition, conditional.Body);
                        foreach (var alternative in conditional.ElseIfs)
                        {
                            VisitBranch(alternative.Condition, alternative.Body);
                        }
                        if (conditional.Else is not null) Walk(conditional.Else.Body, remaining);

                        void VisitBranch(ChunkL.Syntax.Expression condition, IEnumerable<IBodyStatement> body)
                        {
                            var allowed = chunk.Scope.HasVersion ? Condition(condition) : null;
                            Walk(body, allowed is null ? remaining : Intersect(remaining, allowed));
                            if (allowed is not null) remaining = Except(remaining, allowed);
                        }
                        break;
                    }

                    case BlockStatement block:
                        Walk(block.Body, current);
                        break;

                    case LoopStatement loop:
                        Walk(loop.Body, current);
                        break;

                    case WhileStatement loop:
                        Walk(loop.Body, current);
                        break;

                    case SwitchStatement selection:
                        foreach (var branch in selection.Cases) Walk(branch.Body, current);
                        if (selection.Default is not null) Walk(selection.Default.Body, current);
                        break;
                }
            }
        }
    }

    private static IReadOnlyList<Range>? Condition(ChunkL.Syntax.Expression condition)
    {
        var text = ChunkLParser.WriteExpression(condition);
        return Condition(SyntaxFactory.ParseExpression(text));
    }

    private static IReadOnlyList<Range>? Condition(ExpressionSyntax expression)
    {
        if (expression is ParenthesizedExpressionSyntax parentheses) return Condition(parentheses.Expression);
        if (expression is PrefixUnaryExpressionSyntax unary && unary.IsKind(SyntaxKind.LogicalNotExpression))
        {
            var inner = Condition(unary.Operand);
            return inner is null ? null : Except([All], inner);
        }

        if (expression is not BinaryExpressionSyntax binary) return null;

        if (binary.IsKind(SyntaxKind.LogicalAndExpression) || binary.IsKind(SyntaxKind.LogicalOrExpression))
        {
            var left = Condition(binary.Left);
            var right = Condition(binary.Right);
            if (left is null || right is null) return null;
            return binary.IsKind(SyntaxKind.LogicalAndExpression) ? Intersect(left, right) : Normalize(left.Concat(right));
        }

        var versionOnLeft = binary.Left is IdentifierNameSyntax { Identifier.ValueText: "Version" };
        var versionOnRight = binary.Right is IdentifierNameSyntax { Identifier.ValueText: "Version" };
        if (versionOnLeft == versionOnRight) return null;

        var value = versionOnLeft ? Integer(binary.Right) : Integer(binary.Left);
        if (value is null) return null;

        var comparison = binary.Kind();
        if (versionOnRight)
        {
            comparison = comparison switch
            {
                SyntaxKind.LessThanExpression => SyntaxKind.GreaterThanExpression,
                SyntaxKind.LessThanOrEqualExpression => SyntaxKind.GreaterThanOrEqualExpression,
                SyntaxKind.GreaterThanExpression => SyntaxKind.LessThanExpression,
                SyntaxKind.GreaterThanOrEqualExpression => SyntaxKind.LessThanOrEqualExpression,
                _ => comparison
            };
        }

        var bound = (long)value.Value;
        return comparison switch
        {
            SyntaxKind.EqualsExpression => Clip(bound, bound),
            SyntaxKind.NotEqualsExpression => Except([All], Clip(bound, bound)),
            SyntaxKind.LessThanExpression => Clip(0, bound - 1),
            SyntaxKind.LessThanOrEqualExpression => Clip(0, bound),
            SyntaxKind.GreaterThanExpression => Clip(bound + 1, int.MaxValue),
            SyntaxKind.GreaterThanOrEqualExpression => Clip(bound, int.MaxValue),
            _ => null
        };
    }

    private static int? Integer(ExpressionSyntax expression) => expression switch
    {
        LiteralExpressionSyntax literal when literal.Token.Value is int value => value,
        PrefixUnaryExpressionSyntax unary when unary.IsKind(SyntaxKind.UnaryMinusExpression) &&
            unary.Operand is LiteralExpressionSyntax { Token.Value: int value } => -value,
        _ => null
    };

    private static IReadOnlyList<Range> Clip(long start, long end)
    {
        start = Math.Max(start, 0);
        end = Math.Min(end, int.MaxValue);
        return start > end ? [] : [new Range((int)start, (int)end)];
    }

    private static IReadOnlyList<Range> Intersect(IReadOnlyList<Range> left, IReadOnlyList<Range> right)
    {
        var result = new List<Range>();
        foreach (var a in left)
        {
            foreach (var b in right)
            {
                var start = Math.Max(a.Start, b.Start);
                var end = Math.Min(a.End, b.End);
                if (start <= end) result.Add(new Range(start, end));
            }
        }
        return Normalize(result);
    }

    private static IReadOnlyList<Range> Except(IReadOnlyList<Range> source, IReadOnlyList<Range> removed)
    {
        var result = new List<Range>();
        var exclusions = Normalize(removed);
        foreach (var range in source)
        {
            var start = (long)range.Start;
            foreach (var excluded in exclusions)
            {
                if (excluded.End < start || excluded.Start > range.End) continue;
                if (excluded.Start > start) result.Add(new Range((int)start, excluded.Start - 1));
                start = (long)excluded.End + 1;
                if (start > range.End) break;
            }
            if (start <= range.End) result.Add(new Range((int)start, range.End));
        }
        return Normalize(result);
    }

    private static IReadOnlyList<Range> Normalize(IEnumerable<Range> ranges)
    {
        var result = new List<Range>();
        foreach (var range in ranges.OrderBy(static x => x.Start).ThenBy(static x => x.End))
        {
            if (range.Start > range.End) continue;
            if (result.Count == 0 || (long)range.Start > (long)result[result.Count - 1].End + 1)
            {
                result.Add(range);
            }
            else
            {
                var last = result.Count - 1;
                result[last] = new Range(result[last].Start, Math.Max(result[last].End, range.End));
            }
        }
        return result;
    }
}
