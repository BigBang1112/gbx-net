namespace GBX.NET.Builders;

public interface IBuilderForTMU<TBuilder, TNode> where TBuilder : IBuilder where TNode : CMwNod
{
    GameBuilder<TBuilder, TNode> ForTMU();
}
