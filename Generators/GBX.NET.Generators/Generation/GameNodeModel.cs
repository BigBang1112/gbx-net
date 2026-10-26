using System.Runtime.CompilerServices;

namespace GBX.NET.Generators.Generation;

internal sealed class GameNodeModel(LayoutModel layout)
{
    public LayoutModel Layout { get; } = layout;
    public string InterfaceName => "I" + (Layout.Name.StartsWith("C", StringComparison.Ordinal) ? Layout.Name.Substring(1) : Layout.Name);
    public string InterfaceNamespace => "GBX.NET.Interfaces." + Layout.File.Engine;
    public string BuilderName => Layout.Name + "Builder";
    public string BuilderNamespace => "GBX.NET.Builders.Engines." + Layout.File.Engine;
    public List<string> Games { get; } = [];
    public List<GameMember> Members { get; } = [];
    public List<GameMethod> Methods { get; } = [];
    public List<string> MemberErrors { get; } = [];
    public List<GameChunk> Chunks { get; } = [];
    public Dictionary<string, string> RecipeErrors { get; } = new(StringComparer.Ordinal);
    public Dictionary<string, List<GameChunk>> Recipes { get; } = new(StringComparer.Ordinal);
    public string? MetadataError { get; private set; }

    public string Interface(string game = "") => "global::" + InterfaceNamespace + "." + InterfaceName + game;

    // Create finishes the games and members before any writer reads the common view.
    private GameMember[]? common;
    private HashSet<string>? commonNames;
    private Dictionary<string, GameMember>? membersByName;
    public IReadOnlyList<GameMember> Common => common ??= Members.Where(m => Games.All(m.Games.Contains)).ToArray();

    // An empty game denotes the common view shared by every game.
    public IEnumerable<string> Views => new[] { "" }.Concat(Games);
    public IEnumerable<GameMember> MembersFor(string game) => game.Length == 0 ? Common : Members.Where(m => m.Games.Contains(game));

    // Member names are unique within a node.
    public GameMember? MemberFor(string game, string name)
        => (membersByName ??= Members.ToDictionary(static m => m.Name, StringComparer.Ordinal)).TryGetValue(name, out var member) &&
            (game.Length == 0 ? IsCommon(name) : member.Games.Contains(game)) ? member : null;
    public IEnumerable<GameMethod> MethodsFor(string game) => Methods.Where(m => game.Length == 0 ? Games.All(m.Games.Contains) : m.Games.Contains(game));
    public bool IsCommon(string member) => (commonNames ??= new HashSet<string>(Common.Select(static m => m.Name), StringComparer.Ordinal)).Contains(member);
    public IEnumerable<string> BuildableGames => Games.Where(g => !RecipeErrors.ContainsKey(g));
    public bool HasBuilder => !Layout.IsAbstract && BuildableGames.Any();

    public static Dictionary<string, GameNodeModel> Create(GenerationPlan plan, GameSymbols symbols)
    {
        var knownGames = symbols.Games.Where(static g => g.Value > 0 && (g.Value & (g.Value - 1)) == 0)
            .Select(static g => g.Key).ToArray();
        var nodes = plan.Layouts.Values.ToDictionary(static l => l.Name, static l => new GameNodeModel(l), StringComparer.Ordinal);

        // Derived nodes share their inherited chunks, along with the versions each chunk caches.
        var gameChunks = new Dictionary<ChunkModel, GameChunk>();

        foreach (var node in nodes.Values)
        {
            var declared = new HashSet<(uint Id, bool IsHeader)>();
            foreach (var owner in Hierarchy(node.Layout, plan.Layouts))
            {
                foreach (var chunk in owner.Chunks)
                {
                    // Match virtual chunk dispatch: a derived declaration with the
                    // same ID replaces the inherited implementation.
                    if (!declared.Add((chunk.Id, chunk.IsHeader))) continue;
                    if (!gameChunks.TryGetValue(chunk, out var gameChunk)) gameChunks.Add(chunk, gameChunk = new GameChunk(owner, chunk));
                    node.Chunks.Add(gameChunk);
                }
            }

            foreach (var game in knownGames)
            {
                if (node.Layout.Games.Length > 0 && !node.Layout.Games.Contains(game)) continue;
                if (node.Chunks.Any(c => c.Chunk.Declaration.VersionQualifiers.Any(q => q.Label == game)) ||
                    plan.Existing.ContainsKey(node.InterfaceNamespace + "." + node.InterfaceName + game))
                    node.Games.Add(game);
            }

            foreach (var attribute in node.Layout.Existing?.Attributes.Where(static a => a.Name.ToString().EndsWith("NodeBuilderRecipe", StringComparison.Ordinal) ||
                a.Name.ToString().EndsWith("NodeBuilderRecipeAttribute", StringComparison.Ordinal)) ?? [])
            {
                try
                {
                    var arguments = attribute.ArgumentList?.Arguments ?? default;
                    if (arguments.Count == 0) throw new InvalidOperationException("NodeBuilderRecipe requires an exact game context.");
                    // A combined mask would otherwise be read as its last flag.
                    if (arguments[0].Expression is not (IdentifierNameSyntax or MemberAccessExpressionSyntax))
                        throw new InvalidOperationException("NodeBuilderRecipe requires one known GameVersion value.");
                    var gameName = SyntaxOverlap.Simple(arguments[0].Expression.ToString());
                    if (!knownGames.Contains(gameName)) throw new InvalidOperationException("NodeBuilderRecipe requires one known GameVersion value.");
                    if (node.Layout.Games.Length > 0 && !node.Layout.Games.Contains(gameName))
                        throw new InvalidOperationException("NodeBuilderRecipe conflicts with the class game qualifier.");
                    if (node.Recipes.ContainsKey(gameName)) throw new InvalidOperationException("Duplicate NodeBuilderRecipe for " + gameName);
                    var selected = new List<GameChunk>();
                    foreach (var argument in arguments.Skip(1))
                    {
                        if (argument.Expression is not TypeOfExpressionSyntax chunkType)
                            throw new InvalidOperationException("NodeBuilderRecipe chunk arguments must use typeof(ChunkType).");
                        var chunkName = SyntaxOverlap.Simple(chunkType.Type.ToString());
                        var chunk = node.Chunks.FirstOrDefault(c => c.Chunk.Name == chunkName);
                        if (chunk is null) throw new InvalidOperationException("Unknown recipe chunk: " + chunkName);
                        if (selected.Any(c => c.Chunk.IsHeader == chunk.Chunk.IsHeader && c.Chunk.Id == chunk.Chunk.Id))
                            throw new InvalidOperationException("Duplicate recipe chunk: " + chunkName);
                        selected.Add(chunk);
                    }
                    node.Recipes.Add(gameName, selected);
                    if (!node.Games.Contains(gameName)) node.Games.Add(gameName);
                }
                catch (InvalidOperationException exception)
                {
                    node.MetadataError = exception.Message;
                    node.Games.Clear();
                    break;
                }
            }

        }

        foreach (var node in nodes.Values.Where(static n => n.Games.Count > 0).ToArray())
        {
            foreach (var owner in Hierarchy(node.Layout, plan.Layouts).Where(static l => l.IsAbstract))
            {
                var parent = nodes[owner.Name];
                foreach (var game in node.Games.Where(g => !parent.Games.Contains(g))) parent.Games.Add(game);
            }
        }

        // Nodes are visited in hash order, which varies between builds. Keep
        // abstract nodes in GameVersion order, like the nodes declaring the games.
        foreach (var node in nodes.Values.Where(static n => n.Layout.IsAbstract && n.Games.Count > 1))
        {
            var ordered = node.Games.OrderBy(game => Array.IndexOf(knownGames, game)).ToArray();
            node.Games.Clear();
            node.Games.AddRange(ordered);
        }

        var versionedMethods = new Dictionary<LayoutModel, List<(MethodDeclarationSyntax, AttributeArgumentListSyntax)>>();
        foreach (var node in nodes.Values)
        {
            if (node.Games.Count == 0) continue;
            var properties = new Dictionary<string, GameMember>(StringComparer.Ordinal);
            foreach (var owner in Hierarchy(node.Layout, plan.Layouts).Reverse())
            {
                foreach (var field in owner.Scope.Fields.Where(static f => !f.IsUnknown && !f.IsLocal && !f.IsVersion))
                {
                    if (field.Occurrences.Any(static x => LayoutModel.Has(x.Attributes, "inherited"))) continue;
                    if (owner.Existing?.MembersByName.TryGetValue(field.Name, out var declarations) == true &&
                        declarations.OfType<PropertyDeclarationSyntax>().Any(static p => p.ExplicitInterfaceSpecifier is null &&
                            !p.Modifiers.Any(SyntaxKind.PublicKeyword))) continue;
                    var reference = ReferenceModel.IsNode(field.StorageDeclaration, owner, plan.Layouts, plan.Existing);
                    var type = reference
                        ? (field.StorageDeclaration.Type.ArrayDimensions == 0 ? ReferenceModel.Element(field) + "?" : ReferenceModel.CollectionType(field) + "?")
                        : WireTypes.CSharp(field.StorageDeclaration) + (WireTypes.Nullable(field) ? "?" : "");
                    if (!reference && owner.FindProperty(field.Name) is null &&
                        SyntaxOverlap.MemberType(owner.Existing, SyntaxOverlap.Backing(field.Name).TrimStart('@')) is { } backingType)
                        type = backingType;
                    properties[field.Name] = new GameMember(owner, field.Name, Qualify(type, owner, plan), true, field, reference);
                }

                foreach (var property in owner.File.Syntax.Properties)
                {
                    properties[property.Name] = new GameMember(owner, property.Name,
                        Qualify(WireTypes.CSharp(property.Type) + (property.Type.IsNullable ? "?" : ""), owner, plan),
                        property.Accessors.OfType<ChunkL.Syntax.SetterAccessor>().Any(), owner.Scope.FindField(property.Name), false);
                }

                foreach (var property in owner.Existing?.Properties ?? [])
                {
                    if (property.ExplicitInterfaceSpecifier is not null || !property.Modifiers.Any(SyntaxKind.PublicKeyword) ||
                        property.Modifiers.Any(SyntaxKind.StaticKeyword)) continue;
                    if (property.Identifier.ValueText is "Id" or "References" or "GameVersion" or "Chunks") continue;
                    symbols.PropertyTypes.TryGetValue(GameSymbols.PropertyKey(owner, property), out var memberType);
                    var name = property.Identifier.ValueText;
                    properties.TryGetValue(name, out var previous);
                    var writable = property.AccessorList?.Accessors.Any(static a => a.IsKind(SyntaxKind.SetAccessorDeclaration) &&
                        !a.Modifiers.Any(SyntaxKind.PrivateKeyword) && !a.Modifiers.Any(SyntaxKind.ProtectedKeyword)) == true;
                    properties[name] = new GameMember(owner, name, Qualify(memberType ?? property.Type.ToString(), owner, plan),
                        writable, previous?.Field, previous?.IsReference == true, property);
                }
            }

            var writers = node.Games.ToDictionary(static game => game, game => node.Recipes.TryGetValue(game, out var recipe) ? recipe :
                node.Chunks.Where(c => c.Chunk.Declaration.VersionQualifiers.Any(q => q.Label == game)).ToList(), StringComparer.Ordinal);
            foreach (var member in properties.Values.OrderBy(static m => m.Name, StringComparer.Ordinal))
            {
                HashSet<string>? names = null;
                foreach (var game in node.Games)
                {
                    var contract = ExistingContract(plan, node, game, member.Name);
                    if (contract is not null) member.Games.Add(game);
                    else if (writers[game].Any(c => c.Owner == member.Owner && Available(c, member, names ??= AppliedNames(member), game)))
                        member.Games.Add(game);
                }
                if (member.Games.Count > 0) node.Members.Add(member);
            }

            foreach (var owner in Hierarchy(node.Layout, plan.Layouts))
            {
                if (!versionedMethods.TryGetValue(owner, out var candidates))
                    versionedMethods.Add(owner, candidates = VersionedMethods(owner));

                foreach (var (method, attributeArguments) in candidates)
                {
                    if (method.TypeParameterList is not null || attributeArguments.Arguments.Count != 1)
                    {
                        node.MemberErrors.Add(method.Identifier.ValueText + ": generic or version-scoped method requires an explicit interface contract");
                        continue;
                    }
                    if (!symbols.MethodMasks.TryGetValue(GameSymbols.MethodKey(owner, method), out var mask)) continue;
                    var parameters = string.Join(", ", method.ParameterList.Parameters.Select(p => p.WithType(
                        SyntaxFactory.ParseTypeName(Qualify(p.Type?.ToString() ?? "object", owner, plan))
                            .WithTrailingTrivia(SyntaxFactory.Space)).WithoutTrivia().ToString()));
                    var arguments = string.Join(", ", method.ParameterList.Parameters.Select(p =>
                        string.Concat(p.Modifiers.Where(static m => m.IsKind(SyntaxKind.RefKeyword) || m.IsKind(SyntaxKind.OutKeyword) ||
                            m.IsKind(SyntaxKind.InKeyword)).Select(static m => m.Text + " ")) + p.Identifier.Text));
                    var model = new GameMethod(owner, method, Qualify(method.ReturnType.ToString(), owner, plan), parameters, arguments);
                    foreach (var game in node.Games)
                    {
                        if ((mask & symbols.Games.Single(g => g.Key == game).Value) != 0) model.Games.Add(game);
                    }
                    if (model.Games.Count > 0 && !node.Methods.Any(m => m.Declaration.Identifier.ValueText == method.Identifier.ValueText &&
                        m.Parameters == parameters)) node.Methods.Add(model);
                }
            }

            foreach (var game in node.Games)
            {
                if (!node.Recipes.ContainsKey(game))
                    node.Recipes[game] = node.Chunks.Where(c => c.Chunk.Declaration.VersionQualifiers.Any(q => q.Label == game)).ToList();
                var invalid = node.Recipes[game]
                    .FirstOrDefault(c => !Writable(c, plan, new HashSet<uint>()));
                if (invalid is not null) node.RecipeErrors[game] = $"chunk 0x{invalid.Chunk.Id:X8} has no writable implementation";
                var incompatible = node.Recipes[game].FirstOrDefault(c => c.Chunk.Declaration.VersionQualifiers.Count > 0 &&
                    !c.Chunk.Declaration.VersionQualifiers.Any(q => q.Label == game));
                if (incompatible is not null) node.RecipeErrors[game] = $"chunk 0x{incompatible.Chunk.Id:X8} is not a documented {game} writer";
            }
        }
        return nodes;
    }

    // Public instance methods with a game version attribute and an argument list.
    private static List<(MethodDeclarationSyntax, AttributeArgumentListSyntax)> VersionedMethods(LayoutModel owner)
    {
        var result = new List<(MethodDeclarationSyntax, AttributeArgumentListSyntax)>();
        foreach (var method in owner.Existing?.Methods.Where(static m => m.Modifiers.Any(SyntaxKind.PublicKeyword) &&
            !m.Modifiers.Any(SyntaxKind.StaticKeyword) && m.ExplicitInterfaceSpecifier is null) ?? [])
        {
            var attribute = method.AttributeLists.SelectMany(static l => l.Attributes)
                .FirstOrDefault(static a => a.Name.ToString().Contains("MethodGameVersion"));
            if (attribute?.ArgumentList is { } arguments) result.Add((method, arguments));
        }
        return result;
    }

    // A custom property is applied wherever a field its getter reads is applied.
    private static HashSet<string> AppliedNames(GameMember member)
    {
        var names = new HashSet<string>(StringComparer.Ordinal) { member.Name };
        if (member.Owner.FindProperty(member.Name) is { } property)
        {
            foreach (var getter in property.Accessors.OfType<ChunkL.Syntax.GetterAccessor>())
            {
                foreach (var identifier in SyntaxFactory.ParseExpression(ChunkL.ChunkLParser.WriteExpression(getter.Expression))
                    .DescendantNodesAndSelf().OfType<IdentifierNameSyntax>()) names.Add(identifier.Identifier.ValueText);
            }
        }
        return names;
    }

    private static bool Available(GameChunk candidate, GameMember member, HashSet<string> names, string game)
    {
        var version = candidate.Version(game);
        if (AppliedWithChunkRanges.Get(candidate.Chunk, names, candidate.Owner.Chunks, candidate.Owner.Id)
            .Any(r => r.Start <= version && r.End >= version)) return true;
        foreach (var attribute in member.Existing?.AttributeLists.SelectMany(static a => a.Attributes) ?? [])
        {
            if (attribute.Name.ToString().Contains("AppliedWithChunk") && attribute.Name.ToString().Contains(candidate.Chunk.Name))
            {
                var args = attribute.ArgumentList?.Arguments;
                var since = args?.Count > 0 && int.TryParse(args.Value[0].Expression.ToString(), out var start) ? start : 0;
                var until = args?.Count > 1 && int.TryParse(args.Value[1].Expression.ToString(), out var end) ? end : int.MaxValue;
                if (version >= since && version <= until) return true;
            }
        }
        return false;
    }

    private static bool Writable(GameChunk candidate, GenerationPlan plan, HashSet<uint> visited)
    {
        var chunk = candidate.Chunk;
        if (!visited.Add(chunk.Id)) return false;
        if (LayoutModel.Has(chunk.Declaration.Attributes, "ignore")) return false;
        var methods = chunk.Scope.Existing?.Methods;
        if (methods?.Any(static m => m.Identifier.ValueText is "ReadWrite" or "Write" &&
            (m.Body is not null || m.ExpressionBody is not null)) == true) return true;
        if (LayoutModel.Has(chunk.Declaration.Attributes, "demonstration")) return false;
        if (chunk.Scope.Body.Count > 0 || LayoutModel.Has(chunk.Declaration.Attributes, "empty")) return true;
        var inherited = LayoutModel.Attribute(chunk.Declaration.Attributes, "base");
        if (inherited is null) return false;
        var id = LayoutModel.Hex(inherited);
        if (id <= 0xFFF) id |= candidate.Owner.Id;
        var parent = candidate.Owner.Chunks.FirstOrDefault(c => c.Id == id);
        return parent is not null && Writable(new GameChunk(candidate.Owner, parent), plan, visited);
    }

    public static PropertyDeclarationSyntax? ExistingContract(GenerationPlan plan, GameNodeModel node, string game, string member)
    {
        return Find(node.InterfaceNamespace + "." + node.InterfaceName + game, new HashSet<string>(StringComparer.Ordinal));

        PropertyDeclarationSyntax? Find(string key, HashSet<string> visited)
        {
            if (!visited.Add(key) || !plan.Existing.TryGetValue(key, out var type)) return null;
            var property = type.Properties.FirstOrDefault(p => p.Identifier.ValueText == member);
            if (property is not null) return property;
            foreach (var parent in type.BaseTypes)
            {
                if (Find(type.Namespace + "." + parent, visited) is { } inherited) return inherited;
            }
            return null;
        }
    }

    public static IEnumerable<LayoutModel> Hierarchy(LayoutModel layout, IReadOnlyDictionary<string, LayoutModel> layouts)
    {
        var visited = new HashSet<string>(StringComparer.Ordinal);
        for (var current = layout; visited.Add(current.Name);)
        {
            yield return current;
            if (current.BaseName is null || !layouts.TryGetValue(current.BaseName, out current)) yield break;
        }
    }

    // Every derived node qualifies its inherited members again, so remember the result per owner.
    private static readonly ConditionalWeakTable<LayoutModel, Dictionary<string, string>> qualifiedTypes = new();

    public static string Qualify(string type, LayoutModel owner, GenerationPlan plan)
    {
        // A driver runs its steps one at a time, and each layout belongs to a single plan.
        var qualified = qualifiedTypes.GetValue(owner, static _ => new Dictionary<string, string>(StringComparer.Ordinal));
        if (!qualified.TryGetValue(type, out var result))
            qualified.Add(type, result = new TypeQualifier(owner, plan).Visit(SyntaxFactory.ParseTypeName(type))!.ToString());
        return result;
    }

    private sealed class TypeQualifier(LayoutModel owner, GenerationPlan plan) : CSharpSyntaxRewriter
    {
        public override SyntaxNode? VisitIdentifierName(IdentifierNameSyntax node)
        {
            if (node.Parent is QualifiedNameSyntax qualified && qualified.Right == node || node.Parent is AliasQualifiedNameSyntax)
                return node;
            var name = node.Identifier.ValueText;
            foreach (var candidate in Hierarchy(owner, plan.Layouts))
            {
                if (candidate.Archives.ContainsKey(name) || candidate.File.Syntax.Enums.Any(e => e.Name == name) ||
                    candidate.File.Syntax.Flags.Any(f => f.Name == name) || plan.Existing.ContainsKey(candidate.File.TypeKey + "+" + name))
                    return SyntaxFactory.ParseName(candidate.QualifiedName + "." + name);
            }
            if (plan.Layouts.TryGetValue(name, out var layout)) return SyntaxFactory.ParseName(layout.QualifiedName);
            return node;
        }
    }
}

