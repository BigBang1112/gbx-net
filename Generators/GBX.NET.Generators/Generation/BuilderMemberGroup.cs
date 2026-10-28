namespace GBX.NET.Generators.Generation;

internal sealed class BuilderMemberGroup(List<GameMember> members)
{
    public List<GameMember> Members { get; } = members;
    public BuilderMemberGroup? Parent { get; private set; }
    public string Name { get; private set; } = "";

    public static (List<BuilderMemberGroup> Groups, Dictionary<string, BuilderMemberGroup?> Stages) Create(
        GameNodeModel node, GenerationPlan plan)
    {
        var common = new HashSet<string>(node.Common.Select(static m => m.Name), StringComparer.Ordinal);
        var games = node.Games.ToArray();
        var customized = new HashSet<string>(StringComparer.Ordinal);
        foreach (var game in games)
        {
            if (!plan.Existing.TryGetValue(node.BuilderNamespace + "." + node.BuilderName + "+" + game, out var stage)) continue;
            foreach (var member in node.Members)
            {
                if (SyntaxOverlap.Has(stage, member.Name) || stage.Methods.Any(m =>
                    m.Identifier.ValueText == "With" + member.Name || m.Identifier.ValueText == "With" + member.Name + "Reference"))
                    customized.Add(member.Name);
            }
        }

        var shareable = node.Members.Where(m => m.Writable && !common.Contains(m.Name) && !customized.Contains(m.Name)).ToList();
        var candidates = new Dictionary<string, BuilderMemberGroup>(StringComparer.Ordinal);
        foreach (var member in shareable)
        {
            var supported = games.Where(member.Games.Contains).ToArray();
            if (supported.Length < 2) continue;
            var shared = shareable.Where(m => supported.All(m.Games.Contains)).ToList();
            var key = string.Join("|", shared.Select(static m => m.Name));
            if (!candidates.ContainsKey(key)) candidates.Add(key, new BuilderMemberGroup(shared));
        }

        var groups = candidates.Values.OrderBy(static g => g.Members.Count)
            .ThenBy(static g => string.Join("|", g.Members.Select(static m => m.Name)), StringComparer.Ordinal).ToList();
        foreach (var group in groups)
            group.Parent = groups.LastOrDefault(g => g.Members.Count < group.Members.Count && g.Members.All(group.Members.Contains));
        var stages = new Dictionary<string, BuilderMemberGroup?>(StringComparer.Ordinal);
        foreach (var game in games)
            stages.Add(game, groups.LastOrDefault(g => g.Members.All(m => m.Games.Contains(game))));
        var uses = groups.ToDictionary(static g => g, static _ => 0);
        foreach (var selected in stages.Values)
        {
            for (var group = selected; group is not null; group = group.Parent) uses[group]++;
        }
        // A base used by only one stage costs more metadata than keeping those
        // members on the stage. Retain only bases that actually share members.
        groups.RemoveAll(g => uses[g] < 2);
        foreach (var game in games)
        {
            while (stages[game] is { } group && !groups.Contains(group)) stages[game] = group.Parent;
        }
        for (var i = 0; i < groups.Count; i++)
        {
            var group = groups[i];
            while (group.Parent is { } parent && !groups.Contains(parent)) group.Parent = parent.Parent;
            group.Name = node.BuilderName + "Members" + (i + 1);
        }
        return (groups, stages);
    }
}
