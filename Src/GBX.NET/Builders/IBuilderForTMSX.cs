using GBX.NET.Engines.MwFoundations;
using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Builders
{
    public interface IBuilderForTMSX<TBuilder, TNode> where TBuilder : IBuilder where TNode : CMwNod
    {
        GameBuilder<TBuilder, TNode> ForTMSX();
    }
}
