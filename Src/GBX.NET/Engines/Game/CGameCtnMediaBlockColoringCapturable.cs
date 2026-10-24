namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockColoringCapturable
{
    public partial class Key
    {
        [Obsolete("Use EmblemBlink instead.")]
        public bool U01 { get => EmblemBlink; set => EmblemBlink = value; }

        [Obsolete("Use FullIntensity instead.")]
        public bool U02 { get => FullIntensity; set => FullIntensity = value; }
    }
}
