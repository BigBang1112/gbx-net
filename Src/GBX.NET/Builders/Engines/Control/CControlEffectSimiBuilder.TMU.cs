namespace GBX.NET.Builders.Engines.Control;

public partial class CControlEffectSimiBuilder
{
    public class TMU : GameBuilder<CControlEffectSimiBuilder, CControlEffectSimi>
    {
        public int ColorBlendMode { get; set; }
        public bool IsContinousEffect { get; set; }

        public TMU(CControlEffectSimiBuilder baseBuilder, CControlEffectSimi node) : base(baseBuilder, node) { }

        public TMU WithColorBlendMode(int colorBlendMode)
        {
            ColorBlendMode = colorBlendMode;
            return this;
        }

        public TMU ContinousEffect()
        {
            IsContinousEffect = true;
            return this;
        }

        public override CControlEffectSimi Build()
        {
            Node.CreateChunk<CControlEffectSimi.Chunk07010004>();
            Node.ColorBlendMode = ColorBlendMode;
            Node.IsContinousEffect = IsContinousEffect;
            return Node;
        }
    }
}
