namespace GBX.NET.Engines.Plug;

public partial class CPlugSoundEngine2
{
    public partial class PerspectiveVolume
    {
        [Obsolete("Use Rpm instead.")]
        public PlugCurve? Release { get => Rpm; set => Rpm = value; }
    }

    public partial class AudioMotor
    {
        [Obsolete("Use IdleVolume instead.")]
        public float U01 { get => IdleVolume; set => IdleVolume = value; }

        [Obsolete("Use LimiterVolume instead.")]
        public float U02 { get => LimiterVolume; set => LimiterVolume = value; }
    }
}
