using GBX.NET.Generators.Analysis;

namespace GBX.NET.Generators.Generation;

// Writes the fluent node builders, their shared assignment descriptors, and the
// Create() factories. These are emitted into the builder package.
internal static class NodeBuilderWriter
{
    public static string Builder(GameNodeModel node, GameVersionModel model)
    {
        var plan = model.Plan;
        var code = CodeWriter.Generated(node.BuilderNamespace);
        plan.Existing.TryGetValue(node.BuilderNamespace + "." + node.BuilderName, out var existing);
        EngineWriter.RequirePartial(existing);
        code.Open($"public abstract class {node.BuilderName}Base<TSelf> : NodeBuilder<{node.Layout.QualifiedName}, TSelf> where TSelf : {node.BuilderName}Base<TSelf>");
        code.Line($"private protected {node.BuilderName}Base() {{ }}");
        code.Line($"private protected {node.BuilderName}Base(NodeBuilderState<{node.Layout.QualifiedName}> state) : base(state) {{ }}");
        var gameMasks = node.Members.Where(static m => m.Writable).GroupBy(GameMask)
            .Where(static group => group.Count() > 1 || group.Any(static m => m.IsReference &&
                m.Field?.StorageDeclaration.Type.ArrayDimensions == 0))
            .OrderBy(static group => group.Key, StringComparer.Ordinal)
            .Select(static (group, index) => new KeyValuePair<string, string>(group.Key, "SupportedGames" + (index + 1)))
            .ToDictionary(static pair => pair.Key, static pair => pair.Value, StringComparer.Ordinal);
        foreach (var mask in gameMasks)
            code.Line($"private protected const GameVersion {mask.Value} = {mask.Key};");
        var referencedHelpers = ReferencedAssignmentHelpers(node, plan);
        foreach (var member in node.Members.Where(static m => m.Writable))
        {
            if (referencedHelpers.Contains("Set" + member.Name))
                code.Line($"private protected static void Set{member.Name}(NodeBuilderState<{node.Layout.QualifiedName}> state, {member.Type} value) => {Assignment(member, "value", gameMasks)};");
            if (member.IsReference && member.Field?.StorageDeclaration.Type.ArrayDimensions == 0 &&
                referencedHelpers.Contains("Set" + member.Name + "Reference"))
            {
                var element = GameNodeModel.Qualify(ReferenceModel.Element(member.Field), member.Owner, plan);
                code.Line($"private protected static void Set{member.Name}Reference(NodeBuilderState<{node.Layout.QualifiedName}> state, NodeRef<{element}> value) => {Assignment(member, "value", gameMasks, reference: true)};");
            }
        }
        WriteMembers(code, node, "", null, model, gameMasks);
        code.Close();
        var (groups, stages) = BuilderMemberGroup.Create(node, plan);
        foreach (var group in groups)
        {
            var parent = group.Parent?.Name ?? node.BuilderName + "Base";
            code.Open($"public abstract class {group.Name}<TSelf> : {parent}<TSelf> where TSelf : {group.Name}<TSelf>");
            code.Line($"private protected {group.Name}(NodeBuilderState<{node.Layout.QualifiedName}> state) : base(state) {{ }}");
            WriteMembers(code, node, "", null, model, gameMasks, group.Members.Except(group.Parent?.Members ?? []));
            code.Close();
        }
        code.Open($"public partial class {node.BuilderName} : {node.BuilderName}Base<{node.BuilderName}>");
        var recipes = WriteRecipes(code, node);
        foreach (var game in node.Games)
        {
            if (!SyntaxOverlap.Method(existing, "For" + game)) code.Line($"public {game} For{game}() => new(state.Snapshot());");
            if (game == "MP4")
            {
                if (!SyntaxOverlap.Method(existing, "ForTM2")) code.Line("public MP4 ForTM2() => ForMP4();");
                if (!SyntaxOverlap.Method(existing, "ForSM")) code.Line("public MP4 ForSM() => ForMP4();");
            }
        }
        foreach (var game in node.Games)
        {
            plan.Existing.TryGetValue(node.BuilderNamespace + "." + node.BuilderName + "+" + game, out var stage);
            EngineWriter.RequirePartial(stage);
            var group = stages[game];
            code.Open($"public partial class {game} : {group?.Name ?? node.BuilderName + "Base"}<{game}>");
            code.Line($"internal {game}(NodeBuilderState<{node.Layout.QualifiedName}> state) : base(state) {{ }}");
            WriteMembers(code, node, game, stage, model, gameMasks, inheritedMembers: group?.Members);
            WriteBuild(code, node, game, stage, recipes[game]);
            code.Close();
        }
        code.Close();
        return code.ToString();
    }

    private static void WriteBuild(CodeWriter code, GameNodeModel node, string game, ExistingType? stage, int recipe)
    {
        if (SyntaxOverlap.Has(stage, "Build") || SyntaxOverlap.Has(stage, "BuildInterface"))
            throw new InvalidOperationException("Build and BuildInterface are reserved for generated construction. Use the partial hooks.");
        code.Line($"public {node.Interface(game)} BuildInterface() => Build();");
        var build = $"BuildNode({node.BuilderName}.CreateRecipe{recipe}(GameVersion.{game}), GameVersion.{game})";
        if (stage?.Methods.Any(static m => m.Identifier.ValueText == "CustomizeNode" &&
            (m.Body is not null || m.ExpressionBody is not null)) == true)
        {
            code.Open($"public {node.Layout.QualifiedName} Build()");
            code.Line($"var node = {build};");
            code.Line("CustomizeNode(node);");
            code.Line("return node;");
            code.Close();
        }
        else
        {
            code.Line($"public {node.Layout.QualifiedName} Build() => {build};");
        }
        code.Line($"partial void CustomizeNode({node.Layout.QualifiedName} node);");
    }

    private static HashSet<string> ReferencedAssignmentHelpers(GameNodeModel node, GenerationPlan plan)
    {
        var key = node.BuilderNamespace + "." + node.BuilderName;
        // These names may be unresolved in the input compilation: the helpers
        // themselves are generated. Inspect syntax, including method groups,
        // nameof and documentation references in every handwritten partial.
        return new HashSet<string>(plan.Existing.Values.Where(type => type.Key == key || type.Key.StartsWith(key + "+", StringComparison.Ordinal))
            .SelectMany(static type => type.Declarations)
            .SelectMany(static declaration => declaration.DescendantNodes(descendIntoTrivia: true).OfType<IdentifierNameSyntax>())
            .Select(static name => name.Identifier.ValueText).Where(static name => name.StartsWith("Set", StringComparison.Ordinal)), StringComparer.Ordinal);
    }

    private static string GameMask(GameMember member)
        => string.Join(" | ", member.Games.OrderBy(static g => g, StringComparer.Ordinal).Select(static g => "GameVersion." + g));

    private static string Assignment(GameMember member, string value, IReadOnlyDictionary<string, string> gameMasks,
        bool reference = false, bool fluent = false)
    {
        var mask = GameMask(member);
        if (gameMasks.TryGetValue(mask, out var sharedMask)) mask = sharedMask;
        var descriptors = $"global::GBX.NET.Builders.Engines.{member.Owner.File.Engine}.{member.Owner.Name}BuilderAssignments";
        var method = fluent ? (reference ? "SetReference" : "Set") : "state.Set";
        return $"{method}({descriptors}.Get{member.Name}{(reference ? "Reference" : "")}Assignment(), {value}, {mask})";
    }

    // One descriptor class per declaring node is shared by every builder that
    // inherits the property.
    public static void WriteAssignments(SourceProductionContext context, GameVersionModel model)
    {
        var owners = model.Nodes.Values.Where(static n => n.HasBuilder).SelectMany(static n => n.Members).Where(static m => m.Writable)
            .GroupBy(static m => m.Owner).OrderBy(static g => g.Key.File.TypeKey, StringComparer.Ordinal);
        foreach (var owner in owners)
        {
            var code = CodeWriter.Generated("GBX.NET.Builders.Engines." + owner.Key.File.Engine);
            code.Open($"internal static class {owner.Key.Name}BuilderAssignments");
            var copies = new Dictionary<(string Type, string Body), string>();
            string CopyDelegate(string type, string body)
            {
                if (!copies.TryGetValue((type, body), out var method))
                    copies.Add((type, body), method = "Copy" + copies.Count);
                return $"new Func<{type}, {type}>({method})";
            }

            var index = 0;
            foreach (var member in owner.GroupBy(static m => m.Name, StringComparer.Ordinal).Select(static g => g.First()).OrderBy(static m => m.Name, StringComparer.Ordinal))
            {
                // Lazy descriptors let the trimmer discard unused setters.
                // Eager initialization would root every setter on the owner.
                var name = SymbolDisplay.FormatLiteral(member.Name, true);
                var property = SyntaxOverlap.Escape(member.Name);
                var copy = Copy(member.Type, "value", 0);
                var copyDelegate = copy == "value" ? "" : ", " + CopyDelegate(member.Type, $" => {copy};");
                // Ordinal private names share metadata strings across owner types.
                var cache = "cached" + index;
                var assign = "Assign" + index++;
                code.Line($"private static NodeBuilderProperty<{member.Type}>? {cache};");
                // The descriptor already owns its delegates. Explicit constructors
                // avoid another compiler-generated cache for every method group.
                code.Line($"internal static NodeBuilderProperty<{member.Type}> Get{member.Name}Assignment() => {cache} ??= new({name}, new Action<object, {member.Type}>({assign}){copyDelegate});");
                code.Line($"private static void {assign}(object node, {member.Type} value) => (({owner.Key.QualifiedName})node).{property} = value;");
                if (member.IsReference && member.Field?.StorageDeclaration.Type.ArrayDimensions == 0)
                {
                    var element = GameNodeModel.Qualify(ReferenceModel.Element(member.Field), member.Owner, model.Plan);
                    var referenceCopy = CopyDelegate($"NodeRef<{element}>",
                        $" {{ var copy = new NodeRef<{element}>(); copy.CopyFrom(value); return copy; }}");
                    code.Line($"private static NodeBuilderProperty<NodeRef<{element}>>? {cache}Reference;");
                    code.Line($"internal static NodeBuilderProperty<NodeRef<{element}>> Get{member.Name}ReferenceAssignment() => {cache}Reference ??= new({name}, new Action<object, NodeRef<{element}>>({assign}Reference), {referenceCopy});");
                    code.Line($"private static void {assign}Reference(object node, NodeRef<{element}> value) => (({owner.Key.QualifiedName})node).References.{property}.CopyFrom(value);");
                }
            }
            foreach (var copy in copies)
                code.Line($"private static {copy.Key.Type} {copy.Value}({copy.Key.Type} value){copy.Key.Body}");
            code.Close();
            context.AddSource($"Builders/{owner.Key.File.Engine}/{owner.Key.Name}BuilderAssignments.g.cs", code.ToString());
        }
    }

    public static void WriteFactories(SourceProductionContext context, IEnumerable<GameNodeModel> nodes)
    {
        // The builder package cannot add members to the node classes. Static
        // extension members keep Node.Create() available beside the node namespace.
        // Each needs its own container: the members lower to static methods
        // with identical signatures. A handwritten Create() takes precedence.
        var factories = nodes.Where(static n => n.Layout.Existing?.Methods.Any(static m => m.Identifier.ValueText == "Create" &&
            m.Modifiers.Any(SyntaxKind.StaticKeyword) && m.TypeParameterList is null && m.ParameterList.Parameters.Count == 0) != true);
        foreach (var engine in factories.GroupBy(static n => (n.Layout.File.Engine, n.Layout.File.Namespace)))
        {
            var code = CodeWriter.Generated(engine.Key.Namespace);
            foreach (var node in engine)
            {
                code.Open($"public static class {node.BuilderName}Extensions");
                code.Open($"extension({node.Layout.QualifiedName})");
                code.Line($"public static global::{node.BuilderNamespace}.{node.BuilderName} Create() => new();");
                code.Close();
                code.Close();
                code.BlankLine();
            }
            context.AddSource($"Builders/{engine.Key.Engine}/{engine.Key.Engine}NodeBuilderExtensions.g.cs", code.ToString());
        }
    }

    private static Dictionary<string, int> WriteRecipes(CodeWriter code, GameNodeModel node)
    {
        // A chunk created with the game constructor sets its own version, so
        // games differing only in such versions share one recipe body.
        string Key(GameChunk candidate, string game) => candidate.Owner.QualifiedName + "." + candidate.Chunk.Name +
            (EmitsVersion(candidate) ? ":" + candidate.Version(game) : "");
        var recipes = node.Games.GroupBy(game => string.Join("|", node.Recipes[game].Select(c => Key(c, game)))).ToArray();
        var result = new Dictionary<string, int>(StringComparer.Ordinal);
        var gameCtor = GameDefaultsWriter.Enabled(node.Layout.Scope, false) || HasGameConstructor(node.Layout.Existing);
        var keys = recipes.Select(recipe => node.Recipes[recipe.First()].Select(c => Key(c, recipe.First())).ToArray()).ToArray();
        for (var i = 0; i < recipes.Length; i++)
        {
            var recipe = recipes[i];
            var game = recipe.First();
            foreach (var selected in recipe) result.Add(selected, i);
            code.Open($"private static {node.Layout.QualifiedName} CreateRecipe{i}(GameVersion game)");
            // Reuse only an exact ordered prefix, including chunk versions.
            // Passing the selected game preserves constructor defaults.
            var prefix = Enumerable.Range(0, recipes.Length).Where(p => keys[p].Length < keys[i].Length &&
                    keys[p].SequenceEqual(keys[i].Take(keys[p].Length)))
                .OrderByDescending(p => keys[p].Length).DefaultIfEmpty(-1).First();
            code.Line(prefix >= 0 ? $"var node = CreateRecipe{prefix}(game);" :
                $"var node = new {node.Layout.QualifiedName}({(gameCtor ? "game" : "")});");
            foreach (var candidate in node.Recipes[game].Skip(prefix >= 0 ? keys[prefix].Length : 0))
            {
                var version = EmitsVersion(candidate) ? $" {{ Version = {candidate.Version(game)} }}" : "";
                code.Line($"node.Chunks.Add(new {candidate.Owner.QualifiedName}.{candidate.Chunk.Name}({(HasChunkGameConstructor(candidate) ? "game" : "")}){version});");
            }
            code.Line("return node;");
            code.Close();
        }
        return result;
    }

    private static bool HasChunkGameConstructor(GameChunk candidate)
        => GameDefaultsWriter.Enabled(candidate.Chunk.Scope, true) || HasGameConstructor(candidate.Chunk.Scope.Existing);

    private static bool EmitsVersion(GameChunk candidate) => candidate.HasVersion && !HasChunkGameConstructor(candidate);

    private static bool HasGameConstructor(ExistingType? type) => type?.Constructors.Any(static c =>
        c.ParameterList.Parameters.Count == 1 && SyntaxOverlap.Normalize(c.ParameterList.Parameters[0].Type?.ToString() ?? "") == "GameVersion") == true;

    private static void WriteMembers(CodeWriter code, GameNodeModel node, string game, ExistingType? existing,
        GameVersionModel model, IReadOnlyDictionary<string, string> gameMasks,
        IEnumerable<GameMember>? selectedMembers = null,
        IEnumerable<GameMember>? inheritedMembers = null)
    {
        var stage = game.Length == 0 ? "TSelf" : game;
        var self = game.Length == 0 ? "(TSelf)this" : "this";
        foreach (var member in (selectedMembers ?? node.MembersFor(game)).Where(static m => m.Writable))
        {
            if (game.Length > 0 && (node.IsCommon(member.Name) || inheritedMembers?.Contains(member) == true))
            {
                WriteScopedOverloads(code, game, member, model, "base.");
                continue;
            }
            var name = SyntaxOverlap.Escape(member.Name);
            var type = member.Type;
            var method = "With" + member.Name;
            if (!SyntaxOverlap.Method(existing, method, type))
            {
                var arrayParameter = type.TrimEnd('?').EndsWith("[]", StringComparison.Ordinal) ? "params " : "";
                if (existing?.Properties.Any(p => p.Identifier.ValueText == member.Name) == true)
                {
                    code.Open($"public {stage} {method}({arrayParameter}{type} value)");
                    code.Line($"{name} = value;");
                    code.Line("return " + self + ";");
                    code.Close();
                }
                else
                {
                    code.Line($"public {stage} {method}({arrayParameter}{type} value) => {Assignment(member, "value", gameMasks, fluent: true)};");
                }
            }

            if (member.IsReference && member.Field?.StorageDeclaration.Type.ArrayDimensions == 0)
            {
                var element = GameNodeModel.Qualify(ReferenceModel.Element(member.Field), member.Owner, model.Plan);
                if (!SyntaxOverlap.Method(existing, method + "Reference", $"NodeRef<{element}>"))
                {
                    code.Line($"public {stage} {method}Reference(NodeRef<{element}> value) => {Assignment(member, "value", gameMasks, reference: true, fluent: true)};");
                }
            }

            var elementType = CollectionElement(member.Type);
            if (elementType is not null && type.TrimEnd('?').EndsWith("[]", StringComparison.Ordinal) &&
                !SyntaxOverlap.Method(existing, method, $"IEnumerable<{elementType}>"))
            {
                code.Line($"public {stage} {method}(IEnumerable<{elementType}> values) => {method}(values.ToArray());");
            }
            if (elementType is not null && !type.TrimEnd('?').EndsWith("[]", StringComparison.Ordinal) &&
                !SyntaxOverlap.Method(existing, method, elementType + "[]"))
            {
                var conversion = member.Type.Contains("NodeRefList<") ? $"new NodeRefList<{elementType}>(values)" :
                    member.Type.Contains("List<") ? $"new List<{elementType}>(values)" : "values";
                code.Line($"public {stage} {method}(params {elementType}[] values) => {method}({conversion});");
                if (!SyntaxOverlap.Method(existing, method, $"IEnumerable<{elementType}>"))
                {
                    code.Line($"public {stage} {method}(IEnumerable<{elementType}> values) => {method}(values.ToArray());");
                }
            }
            WriteScopedOverloads(code, game, member, model, "");
        }
    }

    // A game stage also accepts the child node's game interface.
    private static void WriteScopedOverloads(CodeWriter code, string game, GameMember member, GameVersionModel model, string prefix)
    {
        if (game.Length == 0 || model.Project(member, game) == member.Type) return;

        var method = "With" + member.Name;
        var child = model.Child(member);
        var childView = child.Interface(game);
        if (member.Field!.StorageDeclaration.Type.ArrayDimensions == 0)
        {
            var nullable = member.Type.EndsWith("?", StringComparison.Ordinal);
            code.Line($"public {game} {method}({childView}{(nullable ? "?" : "")} value) => {prefix}{method}({(nullable ? "value is null ? (" + member.Type + ")null : " : "")}value is {child.Layout.QualifiedName} node ? node : throw new ArgumentException(\"A GBX.NET node is required.\", nameof(value)));");
        }
        else
        {
            var nullable = member.Field.Occurrences.Any(static d => d.Type.IsNullable);
            if (nullable) childView += "?";
            var element = child.Layout.QualifiedName + (nullable ? "?" : "");
            code.Line($"public {game} {method}(params {childView}[] values) => {prefix}{method}(global::GBX.NET.Interfaces.NodeInterfaceReferences.ToNodes<{element}, {childView}>(values, {(nullable ? "true" : "false")}));");
            code.Line($"public {game} {method}(IEnumerable<{childView}> values) => {method}(values.ToArray());");
        }
    }

    private static string Copy(string sourceType, string value, int depth)
    {
        var type = sourceType.TrimEnd('?');
        if (type.Contains("NodeRefList<") && !type.EndsWith("[]", StringComparison.Ordinal))
        {
            return $"{value} is null ? null! : NodeBuilderReferences.CopyList({value})";
        }
        if (type.EndsWith("[]", StringComparison.Ordinal))
        {
            var element = type.Substring(0, type.Length - 2);
            var item = "item" + depth;
            var copied = Copy(element, item, depth + 1);
            return copied == item ? $"{value} is null ? null! : ({type}){value}.Clone()" :
                $"{value} is null ? null! : Array.ConvertAll({value}, static {item} => {copied})";
        }
        if (type.StartsWith("List<", StringComparison.Ordinal) || type.StartsWith("global::System.Collections.Generic.List<", StringComparison.Ordinal))
            return $"{value} is null ? null! : new {type}({value})";
        if (type.StartsWith("Dictionary<", StringComparison.Ordinal) || type.StartsWith("global::System.Collections.Generic.Dictionary<", StringComparison.Ordinal))
            return $"{value} is null ? null! : new {type}({value}, {value}.Comparer)";
        return value;
    }

    private static string? CollectionElement(string type)
    {
        type = type.TrimEnd('?');
        if (type.EndsWith("[]", StringComparison.Ordinal)) return type.Substring(0, type.Length - 2);
        if ((type.Contains("NodeRefList<") || type.StartsWith("List<", StringComparison.Ordinal) ||
            type.StartsWith("global::System.Collections.Generic.List<", StringComparison.Ordinal)) && type.EndsWith(">", StringComparison.Ordinal))
            return type.Substring(type.IndexOf('<') + 1, type.Length - type.IndexOf('<') - 2);
        return null;
    }
}
