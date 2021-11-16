using GBX.NET.Engines.Control;

namespace GBX.NET.Builders.Engines.Control;

public partial class CControlEffectSimiBuilder
{
    public class TMU : GameBuilder<ICControlEffectSimiBuilder, CControlEffectSimi>
    {
        public TMU(ICControlEffectSimiBuilder baseBuilder, CControlEffectSimi node) : base(baseBuilder, node) { }

        public override CControlEffectSimi Build()
        {
            Node.CreateChunk<CControlEffectSimi.Chunk07010004>();
            return Node;
        }
    }
}
