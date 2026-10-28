namespace GBX.NET.Engines.Plug;

public partial class CPlugFxSystem
{
    public partial class EffectNode
    {
        public bool IsSoundSurface => soundRef?.CachedNode is CPlugSoundSurface
            || soundRef?.File?.FilePath.EndsWith(".SoundSurface.Gbx", StringComparison.OrdinalIgnoreCase) == true;
    }
}
