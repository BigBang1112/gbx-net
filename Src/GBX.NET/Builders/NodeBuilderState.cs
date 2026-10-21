namespace GBX.NET.Builders;

// Values and the ordered assignments are kept together. Copying an assignment
// snapshots its container without cloning the child nodes it refers to.
internal sealed class NodeBuilderState<T> where T : class
{
    private readonly List<Assignment> assignments = [];

    public TValue Get<TValue>(string name)
    {
        for (var i = assignments.Count - 1; i >= 0; i--)
        {
            if (assignments[i].Name == name)
            {
                return assignments[i] is Assignment<TValue> assignment ? assignment.Value : default!;
            }
        }

        return default!;
    }

    public void Set<TValue>(string name, TValue value, GameVersion games,
        Func<TValue, TValue> copy, Action<T, TValue> assign)
    {
        assignments.Add(new Assignment<TValue>(name, copy(value), games, copy, assign));
    }

    public NodeBuilderState<T> Snapshot()
    {
        var snapshot = new NodeBuilderState<T>();
        foreach (var assignment in assignments)
        {
            snapshot.assignments.Add(assignment.Snapshot());
        }

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

        public abstract Assignment Snapshot();
        public abstract void Apply(T node);
    }

    private sealed class Assignment<TValue>(string name, TValue value, GameVersion games,
        Func<TValue, TValue> copy, Action<T, TValue> assign) : Assignment(name, games)
    {
        public TValue Value { get; } = value;

        public override Assignment Snapshot() => new Assignment<TValue>(Name, copy(Value), Games, copy, assign);
        public override void Apply(T node) => assign(node, copy(Value));
    }
}
