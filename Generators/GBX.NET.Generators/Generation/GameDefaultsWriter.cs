using ChunkL;
using ChunkL.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace GBX.NET.Generators.Generation;

internal static class GameDefaultsWriter
{
    private static IEnumerable<FieldModel> StoredFields(ScopeModel scope, bool chunk)
        => scope.Fields.Where(x => chunk ? x.IsUnknown || x.IsVersion :
            !x.Occurrences.Any(static d => LayoutModel.Has(d.Attributes, "inherited")));

    public static bool Enabled(ScopeModel scope, bool chunk)
    {
        if (!StoredFields(scope, chunk).Any(static f => f.Occurrences.Any(static d => d.GameDefaults.Count > 0)))
            return false;

        // A handwritten game constructor owns initialization for its partial type.
        if (scope.Existing?.Constructors.Any(static c => c.ParameterList.Parameters.Count == 1 &&
            SyntaxOverlap.Normalize(c.ParameterList.Parameters[0].Type?.ToString() ?? "") == "GameVersion") == true)
            return false;

        if (scope.Existing?.Constructors.Any(static c => !c.Modifiers.Any(SyntaxKind.StaticKeyword) &&
            c.ParameterList.Parameters.Count == 0) == true || scope.Existing?.PrimaryConstructors.Any() == true)
            throw new InvalidOperationException("Game-specific defaults on a type with a handwritten constructor require a handwritten GameVersion constructor.");

        return true;
    }

    public static bool Write(CodeWriter code, LayoutModel layout, ScopeModel scope, string name,
        IReadOnlyDictionary<string, LayoutModel> layouts, bool chunk, ConstructorDeclaration? constructor = null)
    {
        if (!Enabled(scope, chunk)) return false;

        var assignments = constructor is null ? new HashSet<string>(StringComparer.Ordinal) :
            new HashSet<string>(ScopeModel.Walk(constructor.Body).OfType<ComputedAssignment>()
                .Select(static a => a.TargetName), StringComparer.Ordinal);
        var defaults = new List<(FieldModel Field, FieldDeclaration Declaration, string? Game, Expression Value)>();

        foreach (var field in StoredFields(scope, chunk))
        {
            var entries = new Dictionary<string, Expression>(StringComparer.Ordinal);
            foreach (var declaration in field.Occurrences.OrderBy(static d => d.Position.Start.Line).ThenBy(static d => d.Position.Start.Column))
            {
                var values = declaration.GameDefaults.Select(static d => (Game: d.Game, d.Value));
                if (declaration.DefaultValue is { } fallback)
                    values = values.Prepend(("", fallback));
                foreach (var (game, value) in values)
                {
                    if (entries.TryGetValue(game, out var previous))
                    {
                        if (!SyntaxFactory.AreEquivalent(SyntaxFactory.ParseExpression(ChunkLParser.WriteExpression(previous)),
                            SyntaxFactory.ParseExpression(ChunkLParser.WriteExpression(value))))
                            throw new InvalidOperationException($"Conflicting defaults for {field.Name} in {(game.Length == 0 ? "the fallback" : game)}.");
                        continue;
                    }
                    entries.Add(game, value);
                    if (!assignments.Contains(field.Name))
                        defaults.Add((field, declaration, game.Length == 0 ? null : game, value));
                }
            }
        }

        code.BlankLine();
        code.Open($"public {name}() : this(GameVersion.Unspecified)");
        code.Close();
        code.BlankLine();
        code.Open($"public {name}(GameVersion gameVersion)");

        var writer = new SerializationWriter(code, layout, scope, layouts, SerializationMode.ReadWrite, chunk);
        // Only combine adjacent defaults so selected expressions retain their source order.
        var ordered = defaults.OrderBy(static d => d.Declaration.Position.Start.Line)
            .ThenBy(static d => d.Declaration.Position.Start.Column)
            .Where(static d => d.Game != "Unspecified").ToArray();
        var blocks = new List<List<(string? Condition, List<string> Statements)>>();
        for (var index = 0; index < ordered.Length;)
        {
            var field = ordered[index].Field;
            var start = index++;
            while (index < ordered.Length && ReferenceEquals(ordered[index].Field, field)) index++;
            var entries = ordered.Skip(start).Take(index - start).ToArray();
            var branches = new List<(string? Condition, List<string> Statements)>();

            foreach (var group in entries.Where(static d => d.Game is not null)
                .GroupBy(static d => ChunkLParser.WriteExpression(d.Value), StringComparer.Ordinal))
            {
                var condition = string.Join(" || ", group.Select(static d => "gameVersion == GameVersion." + d.Game));
                branches.Add((condition, [Assignment(layout, scope, field, group.First().Value, writer, chunk)]));
            }

            foreach (var entry in entries.Where(static d => d.Game is null))
            {
                var games = field.Occurrences.SelectMany(static d => d.GameDefaults).Select(static d => d.Game)
                    .Where(static g => g != "Unspecified").Distinct(StringComparer.Ordinal).ToArray();
                // A default declared later must still be selected at its own source position.
                var condition = games.Length == entries.Count(static d => d.Game is not null) ? null :
                    string.Join(" && ", games.Select(static g => "gameVersion != GameVersion." + g));
                branches.Add((condition, [Assignment(layout, scope, field, entry.Value, writer, chunk)]));
            }

            if (blocks.Count > 0 && blocks.Last().Select(static b => b.Condition)
                .SequenceEqual(branches.Select(static b => b.Condition), StringComparer.Ordinal))
            {
                for (var branch = 0; branch < branches.Count; branch++)
                    blocks.Last()[branch].Statements.AddRange(branches[branch].Statements);
            }
            else
                blocks.Add(branches);
        }

        foreach (var block in blocks)
        {
            code.BlankLine();
            for (var branch = 0; branch < block.Count; branch++)
            {
                var (condition, statements) = block[branch];
                if (condition is not null)
                    code.Open((branch == 0 ? "if (" : "else if (") + condition + ")");
                else if (branch > 0)
                    code.Open("else");

                foreach (var statement in statements) code.Line(statement);
                if (condition is not null || branch > 0) code.Close();
            }
        }

        if (constructor is not null) writer.Write(constructor.Body);
        code.Close();
        return true;
    }

    private static string Assignment(LayoutModel layout, ScopeModel scope, FieldModel field, Expression value,
        SerializationWriter writer, bool chunk)
    {
        var type = field.IsVersion ? "int" : WireTypes.CSharp(field.Declaration) + (WireTypes.Nullable(field) ? "?" : "");
        var expression = EngineWriter.Default(layout, type, field.Declaration, value)!;
        // Defaults run in a constructor, where class members need no node parameter.
        if (ChunkLParser.WriteExpression(value) != "empty")
            expression = writer.Expression(value, type);
        if (LayoutModel.Has(field.Declaration.Attributes, "time") && field.Declaration.Type.ArrayDimensions == 0 &&
            ChunkLParser.WriteExpression(value) is not ("null" or "default" or "empty"))
            expression = "new " + WireTypes.Map(field.Declaration.Type.Name, field.Declaration.Attributes) + "(" + expression + ")";
        var target = SyntaxOverlap.Escape(field.Name);
        var backing = SyntaxOverlap.Backing(field.Name);
        if (!chunk && !field.IsVersion && (!SyntaxOverlap.Has(scope.Existing, field.Name) ||
            SyntaxOverlap.Has(scope.Existing, backing.TrimStart('@'))))
            target = backing;
        return target + " = " + expression + ";";
    }
}
