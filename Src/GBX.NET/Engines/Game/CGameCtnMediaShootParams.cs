namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaShootParams
{
    public CGameCtnMediaShootParams()
    {
        // The native video constructor leaves the screenshot format outside its valid range.
        ExtScreen = (EExtScreen)3;
        VideoEncoding = new();
        AudioEncoding = new();
    }

    private bool LegacyMotionBlur
    {
        get => MotionBlur != EMotionBlur.None;
        set => MotionBlur = value ? EMotionBlur.Full : EMotionBlur.None;
    }

    private bool LegacyStereo3d
    {
        get => Stereo3d == EStereo3d.LeftNRight;
        set => Stereo3d = value ? EStereo3d.LeftNRight : EStereo3d.None;
    }

    public partial class VideoEnc
    {
        [Obsolete("Use Codec instead.")]
        public int U01 { get => (int)Codec; set => Codec = (EVideoCodec)value; }

        [Obsolete("Use Mode instead.")]
        public int U02 { get => (int)Mode; set => Mode = (EVideoMode)value; }

        [Obsolete("Use Bitrate instead.")]
        public int U03 { get => Bitrate; set => Bitrate = value; }

        [Obsolete("Use CQLevel instead.")]
        public int U04 { get => CQLevel; set => CQLevel = value; }
    }

    public partial class AudioEnc
    {
        [Obsolete("Use VbrQuality instead.")]
        public float U01 { get => VbrQuality; set => VbrQuality = value; }
    }
}
