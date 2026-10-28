namespace GBX.NET.Builders;

// Values and the ordered assignments are kept together. An assignment owns a
// private copy of its value and never changes it, so snapshots can share
// assignments. Applying one copies the container again for the built node,
// without cloning the child nodes it refers to.
internal sealed class NodeBuilderState<T> where T : class
{
    private readonly List<Assignment> assignments = [];

    public void Set<TValue>(NodeBuilderProperty<TValue> property, TValue value, GameVersion games)
    {
        assignments.Add(new Assignment<TValue>(property, property.Copy(value), games));
    }

    public NodeBuilderState<T> Snapshot()
    {
        var snapshot = new NodeBuilderState<T>();
        snapshot.assignments.AddRange(assignments);
        return snapshot;
    }

    public void Apply(T node, GameVersion game)
    {
        foreach (var assignment in assignments)
        {
            if ((assignment.Games & game) == 0)
            {
                throw new InvalidOperationException($"Member '{assignment.Name}' cannot be assigned for {game}.");
            }

            assignment.Apply(node);
        }
    }

    private abstract class Assignment(string name, GameVersion games)
    {
        public string Name { get; } = name;
        public GameVersion Games { get; } = games;

        public abstract void Apply(T node);
    }

    private sealed class Assignment<TValue>(NodeBuilderProperty<TValue> property, TValue value, GameVersion games)
        : Assignment(property.Name, games)
    {
        public override void Apply(T node) => property.Assign(node, value);
    }
}
