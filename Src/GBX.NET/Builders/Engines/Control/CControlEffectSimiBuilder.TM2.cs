namespace GBX.NET.Builders.Engines.Control;

public partial class CControlEffectSimiBuilder
{
    public class TM2 : GameBuilder<CControlEffectSimiBuilder, CControlEffectSimi>
    {
        public int ColorBlendMode { get; set; }
        public bool IsContinousEffect { get; set; }
        public bool IsInterpolated { get; set; }

        public TM2(CControlEffectSimiBuilder baseBuilder, CControlEffectSimi node) : base(baseBuilder, node) { }

        public TM2 WithColorBlendMode(int colorBlendMode)
        {
            ColorBlendMode = colorBlendMode;
            return this;
        }

        public TM2 ContinousEffect()
        {
            IsContinousEffect = true;
            return this;
        }

        public TM2 Interpolated()
        {
            IsInterpolated = true;
            return this;
        }

        public override CControlEffectSimi Build()
        {
            Node.CreateChunk<CControlEffectSimi.Chunk07010005>();
            Node.ColorBlendMode = ColorBlendMode;
            Node.IsContinousEffect = IsContinousEffect;
            Node.IsInterpolated = IsInterpolated;
            return Node;
        }
    }
}
