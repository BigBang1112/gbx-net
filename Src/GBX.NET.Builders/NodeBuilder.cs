namespace GBX.NET.Builders;

/// <summary>
/// Shares assignment state and construction helpers between generated node builders.
/// </summary>
/// <typeparam name="TNode">The node constructed by the builder.</typeparam>
/// <typeparam name="TSelf">The fluent builder type.</typeparam>
public abstract class NodeBuilder<TNode, TSelf> where TNode : class where TSelf : NodeBuilder<TNode, TSelf>
{
    private protected readonly NodeBuilderState<TNode> state;

    private protected NodeBuilder() => state = new();
    private protected NodeBuilder(NodeBuilderState<TNode> state) => this.state = state;

    private protected TSelf Set<TValue>(NodeBuilderProperty<TValue> property, TValue value, GameVersion games)
    {
        state.Set(property, value, games);
        return (TSelf)this;
    }

    private protected TSelf SetReference<TValue>(NodeBuilderProperty<TValue> property, TValue value, GameVersion games)
        where TValue : class
    {
        if (value is null) throw new ArgumentNullException(nameof(value));
        return Set(property, value, games);
    }

    private protected TNode BuildNode(TNode node, GameVersion game)
    {
        state.Apply(node, game);
        return node;
    }
}
