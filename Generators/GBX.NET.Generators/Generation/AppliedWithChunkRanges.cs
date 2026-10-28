using ChunkL;
using ChunkL.Syntax;
using System.Runtime.CompilerServices;

namespace GBX.NET.Generators.Generation;

internal static class AppliedWithChunkRanges
{
    internal readonly struct Range(int start, int end)
    {
        public int Start { get; } = start;
        public int End { get; } = end;
    }

    private static readonly Range All = new(0, int.MaxValue);

    // One walk collects the ranges of every field, so each chunk or archive body is
    // walked once per generation run instead of once per property. The models live
    // for a single run, and the weak tables release their entries afterwards.
    private static readonly ConditionalWeakTable<ChunkModel, Dictionary<string, List<Range>>> chunkRanges = new();
    private static readonly ConditionalWeakTable<ScopeModel, Dictionary<string, List<Range>>> archiveRanges = new();

    public static IReadOnlyList<Range> Get(ChunkModel chunk, IEnumerable<string> fields, IReadOnlyList<ChunkModel> chunks, uint classId)
        => Lookup(chunkRanges.GetValue(chunk, _ => Collect(chunk, chunks, classId)), fields);

    /// <summary>
    /// Gets the class archive versions that apply the fields. Archives with their own version report that version,
    /// other archives report the caller's version argument.
    /// </summary>
    public static IReadOnlyList<Range> GetArchive(ScopeModel scope, IEnumerable<string> fields)
        => Lookup(archiveRanges.GetValue(scope, static scope =>
        {
            var result = new Dictionary<string, List<Range>>(StringComparer.Ordinal);
            Walk(scope.Body, [All], scope, scope.HasVersion ? StoredVersionNames : ArgumentVersionNames, archive: true, onBase: null, result);
            return Normalize(result);
        }), fields);

    private static readonly string[] StoredVersionNames = ["Version"];
    private static readonly string[] ArgumentVersionNames = ["v"];

    private static IReadOnlyList<Range> Lookup(Dictionary<string, List<Range>> ranges, IEnumerable<string> fields)
    {
        List<Range>? single = null;
        List<Range>? combined = null;
        foreach (var field in fields)
        {
            if (!ranges.TryGetValue(field, out var found)) continue;
            if (single is null) single = found;
            else (combined ??= [.. single]).AddRange(found);
        }

        // Each stored list is already normalized, so a single field needs no copy.
        return combined is not null ? Normalize(combined) : single ?? (IReadOnlyList<Range>)[];
    }

    private static Dictionary<string, List<Range>> Collect(ChunkModel chunk, IReadOnlyList<ChunkModel> chunks, uint classId)
    {
        var result = new Dictionary<string, List<Range>>(StringComparer.Ordinal);
        Collect(chunk, chunks, classId, [All], result, []);
        return Normalize(result);
    }

    private static void Collect(ChunkModel chunk, IReadOnlyList<ChunkModel> chunks, uint classId,
        IReadOnlyList<Range> versions, Dictionary<string, List<Range>> result, HashSet<uint> visited)
    {
        if (!visited.Add(chunk.Id)) return;

        var inherited = LayoutModel.Attribute(chunk.Declaration.Attributes, "base");
        ChunkModel? parent = null;
        if (inherited is not null)
        {
            var id = LayoutModel.Hex(inherited);
            if (id <= 0xFFF) id |= classId;
            parent = chunks.FirstOrDefault(x => x.Id == id);
        }

        if (chunk.Scope.Body.Count == 0 && parent is not null)
        {
            Collect(parent, chunks, classId, versions, result, visited);
        }
        else
        {
            Walk(chunk.Scope.Body, versions, chunk.Scope, chunk.Scope.HasVersion ? StoredVersionNames : [], archive: false,
                parent is null ? null : current => Collect(parent, chunks, classId, current, result, visited), result);
        }

        visited.Remove(chunk.Id);
    }

    private static void Walk(IEnumerable<IBodyStatement> statements, IReadOnlyList<Range> current, ScopeModel scope,
        string[] versionNames, bool archive, Action<IReadOnlyList<Range>>? onBase, Dictionary<string, List<Range>> result)
    {
        if (current.Count == 0) return;

        foreach (var statement in statements)
        {
            switch (statement)
            {
                case FieldDeclaration declaration when declaration.IsSpecialKeyword && declaration.Type.Name == "base":
                    onBase?.Invoke(current);
                    break;

                case FieldDeclaration declaration when declaration.Name is { } name &&
                    scope.Occurrences.TryGetValue(declaration, out var occurrence) && !occurrence.IsUnknown && !occurrence.IsLocal:
                    if (!result.TryGetValue(name, out var ranges)) result.Add(name, ranges = []);
                    ranges.AddRange(current);
                    break;

                // An archive with its own version reports that version, so caller version branches do not narrow it.
                case VersionCondition version when archive && scope.HasVersion && LayoutModel.Has(version.Attributes, "chunk"):
                    Walk(version.Body, current, scope, versionNames, archive, onBase, result);
                    break;

                case VersionCondition version:
                    var end = version.Kind switch
                    {
                        VersionConditionKind.Exact or VersionConditionKind.LessOrEqual => version.Version,
                        VersionConditionKind.Range => version.VersionEnd ?? int.MaxValue,
                        _ => int.MaxValue
                    };
                    var start = version.Kind == VersionConditionKind.LessOrEqual ? 0 : version.Version;
                    Walk(version.Body, Intersect(current, [new Range(start, end)]), scope, versionNames, archive, onBase, result);
                    break;

                case IfStatement conditional:
                {
                    var remaining = current;
                    VisitBranch(conditional.Condition, conditional.Body);
                    foreach (var alternative in conditional.ElseIfs)
                    {
                        VisitBranch(alternative.Condition, alternative.Body);
                    }
                    if (conditional.Else is not null) Walk(conditional.Else.Body, remaining, scope, versionNames, archive, onBase, result);

                    void VisitBranch(ChunkL.Syntax.Expression condition, IEnumerable<IBodyStatement> body)
                    {
                        var allowed = versionNames.Length > 0 ? Condition(condition, versionNames) : null;
                        Walk(body, allowed is null ? remaining : Intersect(remaining, allowed), scope, versionNames, archive, onBase, result);
                        if (allowed is not null) remaining = Except(remaining, allowed);
                    }
                    break;
                }

                case BlockStatement block:
                    Walk(block.Body, current, scope, versionNames, archive, onBase, result);
                    break;

                case LoopStatement loop:
                    Walk(loop.Body, current, scope, versionNames, archive, onBase, result);
                    break;

                case WhileStatement loop:
                    Walk(loop.Body, current, scope, versionNames, archive, onBase, result);
                    break;

                case SwitchStatement selection:
                    foreach (var branch in selection.Cases) Walk(branch.Body, current, scope, versionNames, archive, onBase, result);
                    if (selection.Default is not null) Walk(selection.Default.Body, current, scope, versionNames, archive, onBase, result);
                    break;
            }
        }
    }

    private static IReadOnlyList<Range>? Condition(ChunkL.Syntax.Expression condition, string[] versionNames)
    {
        var text = ChunkLParser.WriteExpression(condition);
        return Condition(SyntaxFactory.ParseExpression(text), versionNames);
    }

    private static IReadOnlyList<Range>? Condition(ExpressionSyntax expression, string[] versionNames)
    {
        if (expression is ParenthesizedExpressionSyntax parentheses) return Condition(parentheses.Expression, versionNames);
        if (expression is PrefixUnaryExpressionSyntax unary && unary.IsKind(SyntaxKind.LogicalNotExpression))
        {
            var inner = Condition(unary.Operand, versionNames);
            return inner is null ? null : Except([All], inner);
        }

        if (expression is not BinaryExpressionSyntax binary) return null;

        if (binary.IsKind(SyntaxKind.LogicalAndExpression) || binary.IsKind(SyntaxKind.LogicalOrExpression))
        {
            var left = Condition(binary.Left, versionNames);
            var right = Condition(binary.Right, versionNames);
            if (left is null || right is null) return null;
            return binary.IsKind(SyntaxKind.LogicalAndExpression) ? Intersect(left, right) : Normalize(left.Concat(right));
        }

        var versionOnLeft = binary.Left is IdentifierNameSyntax leftName && versionNames.Contains(leftName.Identifier.ValueText);
        var versionOnRight = binary.Right is IdentifierNameSyntax rightName && versionNames.Contains(rightName.Identifier.ValueText);
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

    private static Dictionary<string, List<Range>> Normalize(Dictionary<string, List<Range>> ranges)
    {
        foreach (var key in ranges.Keys.ToArray())
        {
            ranges[key] = Normalize(ranges[key]);
        }
        return ranges;
    }

    private static List<Range> Normalize(IEnumerable<Range> ranges)
    {
        var sorted = new List<Range>(ranges);
        sorted.Sort(static (x, y) => x.Start != y.Start ? x.Start.CompareTo(y.Start) : x.End.CompareTo(y.End));

        var result = new List<Range>(sorted.Count);
        foreach (var range in sorted)
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
