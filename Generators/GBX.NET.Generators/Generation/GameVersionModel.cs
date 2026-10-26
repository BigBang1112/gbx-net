namespace GBX.NET.Generators.Generation;

// Projection and contract queries shared by the interface and builder writers.
// A projection replaces a node-typed member with the child node's interface
// for the viewed game. An empty game denotes the common view.
internal sealed class GameVersionModel(GenerationPlan plan, Dictionary<string, GameNodeModel> nodes, bool defaultImplementations)
{
    private readonly Dictionary<string, List<InterfaceMemberGroup>> interfaceGroups = new(StringComparer.Ordinal);

    public GenerationPlan Plan { get; } = plan;
    public Dictionary<string, GameNodeModel> Nodes { get; } = nodes;

    // Projections implemented on the node need one explicit implementation per
    // game, each named after its full interface path. A default implementation
    // on the interface keeps a short name, and a generic member group shares
    // one body across games. Older runtimes keep the explicit implementations.
    public bool DefaultImplementations { get; } = defaultImplementations;

    public GameNodeModel Child(GameMember member) => Nodes[WireTypes.Map(member.Field!.StorageDeclaration.Type.Name)];

    public GameNodeModel? Parent(GameNodeModel node) => node.Layout.BaseName is not null &&
        Nodes.TryGetValue(node.Layout.BaseName, out var parent) && parent.Games.Count > 0 ? parent : null;

    public List<InterfaceMemberGroup> InterfaceGroups(GameNodeModel node)
    {
        if (!interfaceGroups.TryGetValue(node.Layout.Name, out var groups))
            interfaceGroups.Add(node.Layout.Name, groups = InterfaceMemberGroup.Create(node, this));
        return groups;
    }

    public string Project(GameMember member, string game)
    {
        var field = member.Field;
        if (field is null || !(member.IsReference || member.Type.Contains("NodeRefList<")) || field.StorageDeclaration.Type.ArrayDimensions > 1 ||
            !Nodes.TryGetValue(WireTypes.Map(field.StorageDeclaration.Type.Name), out var child) || child.Games.Count == 0 ||
            game.Length > 0 && !child.Games.Contains(game)) return member.Type;
        if (field.StorageDeclaration.Type.ArrayDimensions == 0)
            return child.Interface(game) + (member.Type.EndsWith("?", StringComparison.Ordinal) ? "?" : "");
        return "IReadOnlyList<" + child.Interface(game) + (field.Occurrences.Any(static o => o.Type.IsNullable) ? "?" : "") + ">" +
            (member.Type.EndsWith("?", StringComparison.Ordinal) ? "?" : "");
    }

    public string ProjectReturn(GameMethod method, string game)
    {
        var type = method.ReturnType;
        foreach (var child in Nodes.Values.Where(n => n.Games.Count > 0 && (game.Length == 0 || n.Games.Contains(game))))
        {
            if (type == child.Layout.QualifiedName || type == child.Layout.QualifiedName + "?" ||
                type == "IEnumerable<" + child.Layout.QualifiedName + ">" || type == "IEnumerable<" + child.Layout.QualifiedName + "?>")
                return type.Replace(child.Layout.QualifiedName, child.Interface(game));
        }
        return type;
    }

    // The member is projected for this view by the node itself: no handwritten
    // contract or base node interface supplies it, and a game view does not
    // repeat a common declaration with the same projection.
    public bool OwnsProjection(GameNodeModel node, string game, GameMember member)
    {
        if (ExistingProperty(node, game, member) || InheritedProperty(node, game, member)) return false;
        return !(game.Length > 0 && node.IsCommon(member.Name) && Project(member, game) == Project(member, ""));
    }

    public bool OwnsMethod(GameNodeModel node, string game, GameMethod method)
        => !ExistingMethod(node, game, method) && !InheritedMethod(node, game, method);

    public bool ExistingProperty(GameNodeModel node, string game, GameMember member)
    {
        return Plan.Existing.TryGetValue(node.InterfaceNamespace + "." + node.InterfaceName + game, out var type) &&
            type.Properties.Any(p => p.Identifier.ValueText == member.Name) ||
            game.Length > 0 && GameNodeModel.ExistingContract(Plan, node, game, member.Name) is not null &&
            Project(member, game) == member.Type;
    }

    public bool InheritedProperty(GameNodeModel node, string game, GameMember member)
    {
        if (Parent(node) is not { } parent ||
            game.Length > 0 && (!parent.Games.Contains(game) || node.IsCommon(member.Name))) return false;
        var inherited = parent.MemberFor(game, member.Name);
        if (GameNodeModel.ExistingContract(Plan, parent, game, member.Name) is { } existing &&
            (SyntaxOverlap.Normalize(GameNodeModel.Qualify(existing.Type.ToString(), member.Owner, Plan)) !=
                SyntaxOverlap.Normalize(Project(member, game)) ||
             existing.AccessorList?.Accessors.Any(static a => a.IsKind(SyntaxKind.SetAccessorDeclaration)) != member.Writable)) return false;
        return inherited is not null && inherited.Owner == member.Owner && inherited.Type == member.Type &&
            inherited.Writable == member.Writable && Project(inherited, game) == Project(member, game);
    }

    private bool InheritedMethod(GameNodeModel node, string game, GameMethod method)
    {
        if (Parent(node) is not { } parent ||
            game.Length > 0 && (!parent.Games.Contains(game) || node.Games.All(method.Games.Contains))) return false;
        return parent.MethodsFor(game).Any(m => m.Owner == method.Owner && m.Declaration == method.Declaration &&
            ProjectReturn(m, game) == ProjectReturn(method, game));
    }

    private bool ExistingMethod(GameNodeModel node, string game, GameMethod method)
        => Plan.Existing.TryGetValue(node.InterfaceNamespace + "." + node.InterfaceName + game, out var existing) &&
            SyntaxOverlap.Method(existing, method.Declaration.Identifier.ValueText, method.Declaration.ParameterList.Parameters
                .Select(static p => p.Type?.ToString() ?? "object").ToArray());

    // The interface that declares the member for this view.
    public string PropertyContract(GameNodeModel node, string game, GameMember member)
    {
        if (game.Length > 0 && node.IsCommon(member.Name) && Project(member, game) == Project(member, ""))
            game = "";
        if (InheritedProperty(node, game, member))
            return PropertyContract(Nodes[node.Layout.BaseName!], game, member);
        var group = InterfaceGroups(node).FirstOrDefault(g => g.Games.Contains(game) && g.Members.Any(m => m.Name == member.Name));
        if (group is not null) return group.Contract(game);
        return node.Interface(game);
    }
}
