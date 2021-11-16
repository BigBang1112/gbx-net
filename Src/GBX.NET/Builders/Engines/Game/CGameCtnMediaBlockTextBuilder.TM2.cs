using GBX.NET.Engines.Control;
using GBX.NET.Engines.Game;
using System;

namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockTextBuilder
{
    public class TM2 : GameBuilder<ICGameCtnMediaBlockTextBuilder, CGameCtnMediaBlockText>
    {
        public TM2(ICGameCtnMediaBlockTextBuilder baseBuilder, CGameCtnMediaBlockText node) : base(baseBuilder, node) { }

        public override CGameCtnMediaBlockText Build()
        {
            Node.Effect = BaseBuilder.Effect ?? CControlEffectSimi.Create().ForTM2().Build();
            return Node;
        }
    }
}
