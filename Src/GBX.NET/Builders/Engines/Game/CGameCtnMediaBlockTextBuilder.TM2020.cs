namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockTextBuilder
{
    public class TM2020 : GameBuilder<ICGameCtnMediaBlockTextBuilder, CGameCtnMediaBlockText>
    {
        public TM2020(ICGameCtnMediaBlockTextBuilder baseBuilder, CGameCtnMediaBlockText node) : base(baseBuilder, node) { }

        public override CGameCtnMediaBlockText Build()
        {
            Node.Effect = BaseBuilder.Effect ?? CControlEffectSimi.Create().ForTM2020().Build();
            return Node;
        }
    }
}
