namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockTextBuilder
{
    public class TM2 : GameBuilder<CGameCtnMediaBlockTextBuilder, CGameCtnMediaBlockText>
    {
        public TM2(CGameCtnMediaBlockTextBuilder baseBuilder, CGameCtnMediaBlockText node) : base(baseBuilder, node) { }

        public override CGameCtnMediaBlockText Build()
        {
            Node.Effect = BaseBuilder.Effect ?? CControlEffectSimi.Create().ForTM2().Build();
            return Node;
        }
    }
}
