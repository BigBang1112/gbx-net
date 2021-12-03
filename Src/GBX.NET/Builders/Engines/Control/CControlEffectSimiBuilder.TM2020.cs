namespace GBX.NET.Builders.Engines.Control;

public partial class CControlEffectSimiBuilder
{
    public class TM2020 : GameBuilder<ICControlEffectSimiBuilder, CControlEffectSimi>
    {
        public int ColorBlendMode { get; set; }
        public bool IsContinousEffect { get; set; }
        public bool IsInterpolated { get; set; }

        public TM2020(ICControlEffectSimiBuilder baseBuilder, CControlEffectSimi node) : base(baseBuilder, node) { }

        public TM2020 WithColorBlendMode(int colorBlendMode)
        {
            ColorBlendMode = colorBlendMode;
            return this;
        }

        public TM2020 ContinousEffect()
        {
            IsContinousEffect = true;
            return this;
        }

        public TM2020 Interpolated()
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
