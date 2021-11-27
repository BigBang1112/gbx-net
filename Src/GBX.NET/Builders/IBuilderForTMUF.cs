namespace GBX.NET.Builders;

public interface IBuilderForTMUF<TBuilder, TNode> where TBuilder : IBuilder where TNode : CMwNod
{
    GameBuilder<TBuilder, TNode> ForTMUF();
}
