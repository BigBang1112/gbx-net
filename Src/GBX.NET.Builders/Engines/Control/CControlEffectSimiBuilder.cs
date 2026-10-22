namespace GBX.NET.Builders.Engines.Control;

public partial class CControlEffectSimiBuilder
{
    [Obsolete("Use ForTMF() instead.")]
    public TMF ForTMUF() => ForTMF();

    public CControlEffectSimiBuilder Centered() => WithCentered(true);

    public partial class TMF
    {
        [Obsolete("Use the EColorBlendMode overload instead.")]
        public TMF WithColorBlendMode(int value) => WithColorBlendMode((CControlEffectSimi.EColorBlendMode)value);

        public TMF ContinousEffect() => WithIsContinousEffect(true);
        public TMF Interpolated() => WithIsInterpolated(true);
    }

    public partial class MP4
    {
        [Obsolete("Use the EColorBlendMode overload instead.")]
        public MP4 WithColorBlendMode(int value) => WithColorBlendMode((CControlEffectSimi.EColorBlendMode)value);

        public MP4 ContinousEffect() => WithIsContinousEffect(true);
        public MP4 Interpolated() => WithIsInterpolated(true);
    }

    public partial class TM2020
    {
        [Obsolete("Use the EColorBlendMode overload instead.")]
        public TM2020 WithColorBlendMode(int value) => WithColorBlendMode((CControlEffectSimi.EColorBlendMode)value);

        public TM2020 ContinousEffect() => WithIsContinousEffect(true);
        public TM2020 Interpolated() => WithIsInterpolated(true);
    }
}
