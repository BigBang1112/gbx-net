namespace GBX.NET.Builders;

public abstract class GameBuilder<TBuilder, TNode> where TBuilder : Builder where TNode : CMwNod
{
    protected TBuilder BaseBuilder { get; }
    protected TNode Node { get; }

    public GameBuilder(TBuilder baseBuilder, TNode node)
    {
        BaseBuilder = baseBuilder;
        Node = node;
    }

    public abstract TNode Build();
}
