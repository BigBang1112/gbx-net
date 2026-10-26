namespace GBX.NET.Generators.Generation;

// Declares a member once for every game interface that shares it. A member
// projecting a child node becomes generic, so each game closes the group
// with its own child interface.
internal sealed class InterfaceMemberGroup(GameNodeModel node, string[] games, List<GameMember> members)
{
    public string Name { get; private set; } = "";
    public string[] Games { get; } = games;
    public List<GameMember> Members { get; } = members;
    public List<GameNodeModel> Children { get; } = [];

    public string Parameters => Children.Count == 0 ? "" : "<" + string.Join(", ", Children.Select(static (_, i) => "T" + i)) + ">";
    public string Contract(string game) => "global::" + node.InterfaceNamespace + "." + Name +
        (Children.Count == 0 ? "" : "<" + string.Join(", ", Children.Select(c => c.Interface(game))) + ">");

    public static List<InterfaceMemberGroup> Create(GameNodeModel node, GameVersionModel model)
    {
        var groups = new Dictionary<string, InterfaceMemberGroup>(StringComparer.Ordinal);
        var parent = model.Parent(node);
        foreach (var member in node.Members)
        {
            var games = node.Games.Where(game => member.Games.Contains(game) && model.OwnsProjection(node, game, member) &&
                // A game-specific base node contract can introduce another
                // declaration with this name. Keep the unifying declaration on
                // the derived game interface in that case.
                !(parent is not null && parent.Games.Contains(game) && parent.MemberFor(game, member.Name) is not null)).ToArray();
            foreach (var matching in games.GroupBy(game =>
            {
                var type = model.Project(member, game);
                if (type == member.Type) return type;
                return type.Replace(model.Child(member).Interface(game), "TChild");
            }, StringComparer.Ordinal))
            {
                var shared = matching.ToArray();
                if (shared.Length < 2) continue;
                var key = string.Join("|", shared);
                if (!groups.TryGetValue(key, out var group)) groups.Add(key, group = new InterfaceMemberGroup(node, shared, []));
                group.Members.Add(member);
                if (model.Project(member, shared[0]) != member.Type)
                {
                    var child = model.Child(member);
                    if (!group.Children.Contains(child)) group.Children.Add(child);
                }
            }
        }
        var result = groups.OrderBy(static p => p.Key, StringComparer.Ordinal).Select(static p => p.Value).ToList();
        for (var i = 0; i < result.Count; i++) result[i].Name = node.InterfaceName + "Members" + (i + 1);
        return result;
    }
}
