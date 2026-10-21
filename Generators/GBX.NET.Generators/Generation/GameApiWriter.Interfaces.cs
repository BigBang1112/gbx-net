namespace GBX.NET.Generators.Generation;

internal static partial class GameApiWriter
{
    private sealed class InterfaceMemberGroup(GameNodeModel node, string[] games, List<GameMember> members)
    {
        public string Name { get; set; } = "";
        public string[] Games { get; } = games;
        public List<GameMember> Members { get; } = members;
        public List<GameNodeModel> Children { get; } = [];

        public string Parameters => Children.Count == 0 ? "" : "<" + string.Join(", ", Children.Select(static (_, i) => "T" + i)) + ">";
        public string Contract(string game) => "global::" + node.InterfaceNamespace + "." + Name +
            (Children.Count == 0 ? "" : "<" + string.Join(", ", Children.Select(c => c.Interface(game))) + ">");
    }

    private static List<InterfaceMemberGroup> CreateInterfaceGroups(GameNodeModel node, GenerationPlan plan, Dictionary<string, GameNodeModel> nodes)
    {
        var groups = new Dictionary<string, InterfaceMemberGroup>(StringComparer.Ordinal);
        foreach (var member in node.Members)
        {
            var games = node.Games.Where(game => member.Games.Contains(game) &&
                !ExistingProperty(plan, node, game, member, nodes) && !InheritedProperty(node, game, member, plan, nodes) &&
                !(node.Common.Any(m => m.Name == member.Name) && Project(member, game, nodes) == Project(member, "", nodes)) &&
                // A game-specific base node contract can introduce another
                // declaration with this name. Keep the unifying declaration on
                // the derived game interface in that case.
                !(node.Layout.BaseName is { } baseName && nodes.TryGetValue(baseName, out var parent) && parent.Games.Contains(game) &&
                  parent.Members.Any(m => m.Name == member.Name && m.Games.Contains(game)))).ToArray();
            foreach (var matching in games.GroupBy(game =>
            {
                var type = Project(member, game, nodes);
                if (type == member.Type) return type;
                var child = nodes[WireTypes.Map(member.Field!.StorageDeclaration.Type.Name)];
                return type.Replace(child.Interface(game), "TChild");
            }, StringComparer.Ordinal))
            {
                var shared = matching.ToArray();
                if (shared.Length < 2) continue;
                var key = string.Join("|", shared);
                if (!groups.TryGetValue(key, out var group)) groups.Add(key, group = new InterfaceMemberGroup(node, shared, []));
                group.Members.Add(member);
                if (Project(member, shared[0], nodes) != member.Type)
                {
                    var child = nodes[WireTypes.Map(member.Field!.StorageDeclaration.Type.Name)];
                    if (!group.Children.Contains(child)) group.Children.Add(child);
                }
            }
        }
        var result = groups.OrderBy(static p => p.Key, StringComparer.Ordinal).Select(static p => p.Value).ToList();
        for (var i = 0; i < result.Count; i++) result[i].Name = node.InterfaceName + "Members" + (i + 1);
        return result;
    }

    private static void WriteInterfaceGroups(CodeWriter code, GameNodeModel node, List<InterfaceMemberGroup> groups,
        Dictionary<string, GameNodeModel> nodes)
    {
        foreach (var group in groups)
        {
            code.Line("[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]");
            code.Open($"public interface {group.Name}{group.Parameters} : {node.Interface()}" +
                string.Concat(group.Children.Select(static (_, i) => $" where T{i} : class")));
            var game = group.Games[0];
            foreach (var member in group.Members)
            {
                var type = Project(member, game, nodes);
                for (var i = 0; i < group.Children.Count; i++) type = type.Replace(group.Children[i].Interface(game), "T" + i);
                var hidesCommon = node.Common.Any(m => m.Name == member.Name);
                code.Line($"{(hidesCommon ? "new " : "")}{type} {SyntaxOverlap.Escape(member.Name)} {{ get;{(member.Writable ? " set;" : "")} }}");
            }
            code.Close();
        }
    }
}
