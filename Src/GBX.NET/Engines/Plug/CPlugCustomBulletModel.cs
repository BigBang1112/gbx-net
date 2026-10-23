namespace GBX.NET.Engines.Plug;

public partial class CPlugCustomBulletModel
{
    [Obsolete("Use BulletExplosionDetectionRadius instead.")]
    public float U01 { get => BulletExplosionDetectionRadius; set => BulletExplosionDetectionRadius = value; }

    [Obsolete("Use BulletExplosionBlowRadius instead.")]
    public float U02 { get => BulletExplosionBlowRadius; set => BulletExplosionBlowRadius = value; }

    [Obsolete("Use BulletExplosionBlowValue instead.")]
    public float U03 { get => BulletExplosionBlowValue; set => BulletExplosionBlowValue = value; }

    [Obsolete("Use BulletExplosionDamageRadius instead.")]
    public float U04 { get => BulletExplosionDamageRadius; set => BulletExplosionDamageRadius = value; }

    [Obsolete("Use BulletExplosionBlowRadiusAttenuation instead.")]
    public float U05 { get => BulletExplosionBlowRadiusAttenuation; set => BulletExplosionBlowRadiusAttenuation = value; }

    [Obsolete("Use BulletExplosionDamageValue instead.")]
    public int U06 { get => BulletExplosionDamageValue; set => BulletExplosionDamageValue = value; }

    [Obsolete("Use BulletExplosionDamageRadiusAttenuation instead.")]
    public float U07 { get => BulletExplosionDamageRadiusAttenuation; set => BulletExplosionDamageRadiusAttenuation = value; }

    [Obsolete("Use BulletExplosionBlowVerticalScale instead.")]
    public float U12 { get => BulletExplosionBlowVerticalScale; set => BulletExplosionBlowVerticalScale = value; }

    [Obsolete("Use BulletExplosionOnAllPlayers instead.")]
    public bool U13 { get => BulletExplosionOnAllPlayers; set => BulletExplosionOnAllPlayers = value; }

    [Obsolete("Use BulletDirectHitDamageValue instead.")]
    public int U16 { get => BulletDirectHitDamageValue; set => BulletDirectHitDamageValue = value; }

    [Obsolete("Use BulletExplosionBlowOnAllPlayers instead.")]
    public bool U17 { get => BulletExplosionBlowOnAllPlayers; set => BulletExplosionBlowOnAllPlayers = value; }
}
