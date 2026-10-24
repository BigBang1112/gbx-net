namespace GBX.NET.Engines.Plug;

public partial class CPlugCustomBeamModel
{
    [Obsolete("Use FireSound instead.")]
    public string? U03 { get => FireSound; set => FireSound = value; }

    [Obsolete("Use ReloadingSound instead.")]
    public string? U04 { get => ReloadingSound; set => ReloadingSound = value; }

    [Obsolete("Use ExplosionSound instead.")]
    public string? U05 { get => ExplosionSound; set => ExplosionSound = value; }

    [Obsolete("Use FireSoundVolume instead.")]
    public float U06 { get => FireSoundVolume; set => FireSoundVolume = value; }

    [Obsolete("Use ReloadingSoundVolume instead.")]
    public float U07 { get => ReloadingSoundVolume; set => ReloadingSoundVolume = value; }

    [Obsolete("Use ExplosionSoundVolume instead.")]
    public float U08 { get => ExplosionSoundVolume; set => ExplosionSoundVolume = value; }

    [Obsolete("Use ShowPlayerExplosion instead.")]
    public bool U09 { get => ShowPlayerExplosion; set => ShowPlayerExplosion = value; }

    [Obsolete("Use ModifyFOV instead.")]
    public bool U11 { get => ModifyFOV; set => ModifyFOV = value; }

    [Obsolete("Use TriLaserRadiusAtOneMeter_Zoom0 instead.")]
    public float U12 { get => TriLaserRadiusAtOneMeter_Zoom0; set => TriLaserRadiusAtOneMeter_Zoom0 = value; }

    [Obsolete("Use TriLaserRadiusAtOneMeter_Zoom1 instead.")]
    public float U13 { get => TriLaserRadiusAtOneMeter_Zoom1; set => TriLaserRadiusAtOneMeter_Zoom1 = value; }
}
