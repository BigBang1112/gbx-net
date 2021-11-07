using GBX.NET.Engines.Control;
using GBX.NET.Engines.Game;
using System;

namespace GBX.NET.Builders.Engines.Game
{
    public partial class CGameCtnMediaBlockTextBuilder
    {
        public class TMUF : GameBuilder<ICGameCtnMediaBlockTextBuilder, CGameCtnMediaBlockText>
        {
            public TMUF(ICGameCtnMediaBlockTextBuilder baseBuilder, CGameCtnMediaBlockText node) : base(baseBuilder, node) { }

            public override CGameCtnMediaBlockText Build()
            {
                Node.Effect = BaseBuilder.Effect ?? CControlEffectSimi.Create().ForTMUF().Build();
                return Node;
            }
        }
    }
}
