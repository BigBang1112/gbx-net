namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockTextBuilder
{
    public class TMSX : GameBuilder<ICGameCtnMediaBlockTextBuilder, CGameCtnMediaBlockText>
    {
        public TMSX(ICGameCtnMediaBlockTextBuilder baseBuilder, CGameCtnMediaBlockText node) : base(baseBuilder, node) { }

        public override CGameCtnMediaBlockText Build()
        {
            Node.Effect = BaseBuilder.Effect ?? CControlEffectSimi.Create().ForTMSX().Build();
            Node.CreateChunk<CGameCtnMediaBlockText.Chunk030A8003>();
            return Node;
        }
    }
}
