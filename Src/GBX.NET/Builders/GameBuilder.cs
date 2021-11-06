using GBX.NET.Engines.MwFoundations;
using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Builders
{
    public abstract class GameBuilder<TBaseBuilder, TClass>
        where TBaseBuilder : IBuilder
        where TClass : CMwNod
    {
        protected TBaseBuilder BaseBuilder { get; }
        protected TClass Node { get; }

        public GameBuilder(TBaseBuilder baseBuilder, TClass node)
        {
            BaseBuilder = baseBuilder;
            Node = node;
        }

        public abstract TClass Build();
    }
}
