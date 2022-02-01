namespace GBX.NET.Builders.Engines.Control;

public partial class CControlEffectSimiBuilder
{
    public class TMSX : GameBuilder<CControlEffectSimiBuilder, CControlEffectSimi>
    {
        public TMSX(CControlEffectSimiBuilder baseBuilder, CControlEffectSimi node) : base(baseBuilder, node) { }

        public override CControlEffectSimi Build()
        {
            Node.CreateChunk<CControlEffectSimi.Chunk07010002>();
            return Node;
        }
    }
}
