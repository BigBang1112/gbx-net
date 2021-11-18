namespace GBX.NET.Builders.Engines.Control;

public partial class CControlEffectSimiBuilder
{
    public class TM2020 : GameBuilder<ICControlEffectSimiBuilder, CControlEffectSimi>
    {
        public TM2020(ICControlEffectSimiBuilder baseBuilder, CControlEffectSimi node) : base(baseBuilder, node) { }

        public override CControlEffectSimi Build()
        {
            Node.CreateChunk<CControlEffectSimi.Chunk07010005>();
            return Node;
        }
    }
}
