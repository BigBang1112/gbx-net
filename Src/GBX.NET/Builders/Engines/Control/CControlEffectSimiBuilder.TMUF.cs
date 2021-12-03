namespace GBX.NET.Builders.Engines.Control;

public partial class CControlEffectSimiBuilder
{
    public class TMUF : GameBuilder<ICControlEffectSimiBuilder, CControlEffectSimi>
    {
        public int ColorBlendMode { get; set; }
        public bool IsContinousEffect { get; set; }
        public bool IsInterpolated { get; set; }

        public TMUF(ICControlEffectSimiBuilder baseBuilder, CControlEffectSimi node) : base(baseBuilder, node) { }

        public TMUF WithColorBlendMode(int colorBlendMode)
        {
            ColorBlendMode = colorBlendMode;
            return this;
        }

        public TMUF ContinousEffect()
        {
            IsContinousEffect = true;
            return this;
        }

        public TMUF Interpolated()
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
