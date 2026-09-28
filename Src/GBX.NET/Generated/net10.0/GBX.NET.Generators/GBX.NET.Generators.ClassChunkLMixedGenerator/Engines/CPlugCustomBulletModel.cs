namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09103000</remarks>
[Class(0x09103000)]
public partial class CPlugCustomBulletModel : CMwNod, IClass, IReadableWritable
{
    [Hexadecimal] public static new uint Id => 0x09103000;




    private string? bulletName;
    public string? BulletName { get => bulletName; set => bulletName = value; }

    private float bulletSpeed;
    public float BulletSpeed { get => bulletSpeed; set => bulletSpeed = value; }

    private float bulletMass;
    public float BulletMass { get => bulletMass; set => bulletMass = value; }

    private float bulletHitboxRadius;
    public float BulletHitboxRadius { get => bulletHitboxRadius; set => bulletHitboxRadius = value; }

    private float bulletFluidFriction;
    public float BulletFluidFriction { get => bulletFluidFriction; set => bulletFluidFriction = value; }

    private float bulletLifeTime;
    public float BulletLifeTime { get => bulletLifeTime; set => bulletLifeTime = value; }

    private bool bulletExplodeOnEndLife;
    public bool BulletExplodeOnEndLife { get => bulletExplodeOnEndLife; set => bulletExplodeOnEndLife = value; }

    private int bulletRebounds;
    public int BulletRebounds { get => bulletRebounds; set => bulletRebounds = value; }

    private bool bulletBounceOnTechWall;
    public bool BulletBounceOnTechWall { get => bulletBounceOnTechWall; set => bulletBounceOnTechWall = value; }

    private int bulletPattern;
    public int BulletPattern { get => bulletPattern; set => bulletPattern = value; }

    private float u01;
    public float U01 { get => u01; set => u01 = value; }

    private float u02;
    public float U02 { get => u02; set => u02 = value; }

    private float u03;
    public float U03 { get => u03; set => u03 = value; }

    private float u04;
    public float U04 { get => u04; set => u04 = value; }

    private int patternBulletCount;
    public int PatternBulletCount { get => patternBulletCount; set => patternBulletCount = value; }

    private float patternBulletRadius;
    public float PatternBulletRadius { get => patternBulletRadius; set => patternBulletRadius = value; }

    private float patternBulletSpinSecond;
    public float PatternBulletSpinSecond { get => patternBulletSpinSecond; set => patternBulletSpinSecond = value; }

    private float patternBulletBlendDuration;
    public float PatternBulletBlendDuration { get => patternBulletBlendDuration; set => patternBulletBlendDuration = value; }

    private bool patternBulletApexRegroup;
    public bool PatternBulletApexRegroup { get => patternBulletApexRegroup; set => patternBulletApexRegroup = value; }

    private float patternBulletMinApexTime;
    public float PatternBulletMinApexTime { get => patternBulletMinApexTime; set => patternBulletMinApexTime = value; }

    private bool patternBulletRandomRotations;
    public bool PatternBulletRandomRotations { get => patternBulletRandomRotations; set => patternBulletRandomRotations = value; }

    private int noPatternBulletCount;
    public int NoPatternBulletCount { get => noPatternBulletCount; set => noPatternBulletCount = value; }

    private float noPatternBulletDispersionAngle;
    public float NoPatternBulletDispersionAngle { get => noPatternBulletDispersionAngle; set => noPatternBulletDispersionAngle = value; }

    private float noPatternBulletSpeedCoef;
    public float NoPatternBulletSpeedCoef { get => noPatternBulletSpeedCoef; set => noPatternBulletSpeedCoef = value; }

    private string? bulletAliveSound;
    public string? BulletAliveSound { get => bulletAliveSound; set => bulletAliveSound = value; }

    private string? bulletExplosionSound;
    public string? BulletExplosionSound { get => bulletExplosionSound; set => bulletExplosionSound = value; }

    private string? bulletShootingSound;
    public string? BulletShootingSound { get => bulletShootingSound; set => bulletShootingSound = value; }

    private CPlugParticleEmitterSubModel[]? aliveParticleEmitterSubModels;
    public CPlugParticleEmitterSubModel[]? AliveParticleEmitterSubModels { get => aliveParticleEmitterSubModels; set => aliveParticleEmitterSubModels = value; }

    private CPlugParticleEmitterSubModel[]? explosionParticleEmitterSubModels;
    public CPlugParticleEmitterSubModel[]? ExplosionParticleEmitterSubModels { get => explosionParticleEmitterSubModels; set => explosionParticleEmitterSubModels = value; }

    private float u05;
    public float U05 { get => u05; set => u05 = value; }

    private int u06;
    public int U06 { get => u06; set => u06 = value; }

    private float u07;
    public float U07 { get => u07; set => u07 = value; }

    private int u08;
    public int U08 { get => u08; set => u08 = value; }

    private int u09;
    public int U09 { get => u09; set => u09 = value; }

    private int u10;
    public int U10 { get => u10; set => u10 = value; }

    private float bulletLifeTimeAfterFirstImpact;
    public float BulletLifeTimeAfterFirstImpact { get => bulletLifeTimeAfterFirstImpact; set => bulletLifeTimeAfterFirstImpact = value; }

    private float bulletImpactBouncingN;
    public float BulletImpactBouncingN { get => bulletImpactBouncingN; set => bulletImpactBouncingN = value; }

    private float bulletImpactBouncingT;
    public float BulletImpactBouncingT { get => bulletImpactBouncingT; set => bulletImpactBouncingT = value; }

    private bool u11 = true;
    public bool U11 { get => u11; set => u11 = value; }

    private float bulletHomingDist;
    public float BulletHomingDist { get => bulletHomingDist; set => bulletHomingDist = value; }

    private float bulletHomingAngularSpeed;
    public float BulletHomingAngularSpeed { get => bulletHomingAngularSpeed; set => bulletHomingAngularSpeed = value; }

    private float bulletHomingPeriod;
    public float BulletHomingPeriod { get => bulletHomingPeriod; set => bulletHomingPeriod = value; }

    private bool bulletShowPlayerExplosion;
    public bool BulletShowPlayerExplosion { get => bulletShowPlayerExplosion; set => bulletShowPlayerExplosion = value; }

    private float u12;
    public float U12 { get => u12; set => u12 = value; }

    private string? bulletReboundSound;
    public string? BulletReboundSound { get => bulletReboundSound; set => bulletReboundSound = value; }

    private float bulletAliveSoundVolume;
    public float BulletAliveSoundVolume { get => bulletAliveSoundVolume; set => bulletAliveSoundVolume = value; }

    private float bulletExplosionSoundVolume;
    public float BulletExplosionSoundVolume { get => bulletExplosionSoundVolume; set => bulletExplosionSoundVolume = value; }

    private float bulletShootingSoundVolume;
    public float BulletShootingSoundVolume { get => bulletShootingSoundVolume; set => bulletShootingSoundVolume = value; }

    private float bulletReboundSoundVolume;
    public float BulletReboundSoundVolume { get => bulletReboundSoundVolume; set => bulletReboundSoundVolume = value; }

    private bool bulletShowDebris;
    public bool BulletShowDebris { get => bulletShowDebris; set => bulletShowDebris = value; }

    private bool bulletModifyFOV;
    public bool BulletModifyFOV { get => bulletModifyFOV; set => bulletModifyFOV = value; }

    private bool u13;
    public bool U13 { get => u13; set => u13 = value; }

    private bool bulletIsFlare;
    public bool BulletIsFlare { get => bulletIsFlare; set => bulletIsFlare = value; }

    private float bulletFlareAttractionRadius;
    public float BulletFlareAttractionRadius { get => bulletFlareAttractionRadius; set => bulletFlareAttractionRadius = value; }

    private float bulletFlareExplosionRadius;
    public float BulletFlareExplosionRadius { get => bulletFlareExplosionRadius; set => bulletFlareExplosionRadius = value; }

    private string? bulletHomingSound;
    public string? BulletHomingSound { get => bulletHomingSound; set => bulletHomingSound = value; }

    private float bulletHomingSoundVolume;
    public float BulletHomingSoundVolume { get => bulletHomingSoundVolume; set => bulletHomingSoundVolume = value; }

    private bool u14;
    public bool U14 { get => u14; set => u14 = value; }

    private float bulletGuidedAngularSpeed;
    public float BulletGuidedAngularSpeed { get => bulletGuidedAngularSpeed; set => bulletGuidedAngularSpeed = value; }

    private int bulletHomingLockDuration;
    public int BulletHomingLockDuration { get => bulletHomingLockDuration; set => bulletHomingLockDuration = value; }

    private bool bulletIsWard;
    public bool BulletIsWard { get => bulletIsWard; set => bulletIsWard = value; }

    private float bulletWardRadius;
    public float BulletWardRadius { get => bulletWardRadius; set => bulletWardRadius = value; }

    private bool u15;
    public bool U15 { get => u15; set => u15 = value; }

    private int bulletGuidedMinLifeTime;
    public int BulletGuidedMinLifeTime { get => bulletGuidedMinLifeTime; set => bulletGuidedMinLifeTime = value; }

    private float bulletGunSpeedCoef;
    public float BulletGunSpeedCoef { get => bulletGunSpeedCoef; set => bulletGunSpeedCoef = value; }

    private int bulletType;
    public int BulletType { get => bulletType; set => bulletType = value; }

    private int bulletHomingDamageMinAngle_Deg;
    public int BulletHomingDamageMinAngle_Deg { get => bulletHomingDamageMinAngle_Deg; set => bulletHomingDamageMinAngle_Deg = value; }

    private int bulletHomingDamageMaxAngle_Deg;
    public int BulletHomingDamageMaxAngle_Deg { get => bulletHomingDamageMaxAngle_Deg; set => bulletHomingDamageMaxAngle_Deg = value; }

    private float bulletRecoil;
    public float BulletRecoil { get => bulletRecoil; set => bulletRecoil = value; }

    private float bulletGunSpeedCoefRatioMin;
    public float BulletGunSpeedCoefRatioMin { get => bulletGunSpeedCoefRatioMin; set => bulletGunSpeedCoefRatioMin = value; }

    private float bulletGunSpeedCoefRatioMax;
    public float BulletGunSpeedCoefRatioMax { get => bulletGunSpeedCoefRatioMax; set => bulletGunSpeedCoefRatioMax = value; }

    private int u16;
    public int U16 { get => u16; set => u16 = value; }

    private bool u17;
    public bool U17 { get => u17; set => u17 = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugCustomBulletModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugCustomBulletModel() { }

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        rw.String(ref bulletName);
        rw.Single(ref bulletSpeed);
        rw.Single(ref bulletMass);
        rw.Single(ref bulletHitboxRadius);
        rw.Single(ref bulletFluidFriction);
        rw.Single(ref bulletLifeTime);
        rw.Boolean(ref bulletExplodeOnEndLife);
        rw.Int32(ref bulletRebounds);
        rw.Boolean(ref bulletBounceOnTechWall);
        rw.Int32(ref bulletPattern);
        rw.Single(ref u01);
        rw.Single(ref u02);
        rw.Single(ref u03);
        rw.Single(ref u04);
        rw.Int32(ref patternBulletCount);
        rw.Single(ref patternBulletRadius);
        rw.Single(ref patternBulletSpinSecond);
        rw.Single(ref patternBulletBlendDuration);
        rw.Boolean(ref patternBulletApexRegroup);
        rw.Single(ref patternBulletMinApexTime);
        rw.Boolean(ref patternBulletRandomRotations);
        rw.Int32(ref noPatternBulletCount);
        rw.Single(ref noPatternBulletDispersionAngle);
        rw.Single(ref noPatternBulletSpeedCoef);
        rw.Id(ref bulletAliveSound);
        rw.Id(ref bulletExplosionSound);
        rw.Id(ref bulletShootingSound);
        rw.ArrayNodeRef_deprec<CPlugParticleEmitterSubModel>(ref aliveParticleEmitterSubModels!);
        rw.ArrayNodeRef_deprec<CPlugParticleEmitterSubModel>(ref explosionParticleEmitterSubModels!);
        if (v >= 1)
        {
            rw.Single(ref u05);
            rw.Int32(ref u06);
            rw.Single(ref u07);
            if (v <= 2)
            {
                rw.Int32(ref u08);
                rw.Int32(ref u09);
                rw.Int32(ref u10);
            }
            rw.Single(ref bulletLifeTimeAfterFirstImpact);
            if (v >= 2)
            {
                rw.Single(ref bulletImpactBouncingN);
                rw.Single(ref bulletImpactBouncingT);
                if (v >= 4)
                {
                    if (v <= 18)
                    {
                        rw.Boolean(ref u11);
                    }
                    rw.Single(ref bulletHomingDist);
                    rw.Single(ref bulletHomingAngularSpeed);
                    rw.Single(ref bulletHomingPeriod);
                    if (v >= 5)
                    {
                        rw.Boolean(ref bulletShowPlayerExplosion);
                        if (v >= 6)
                        {
                            rw.Single(ref u12);
                            if (v >= 7)
                            {
                                rw.Id(ref bulletReboundSound);
                                if (v >= 8)
                                {
                                    rw.Single(ref bulletAliveSoundVolume);
                                    rw.Single(ref bulletExplosionSoundVolume);
                                    rw.Single(ref bulletShootingSoundVolume);
                                    rw.Single(ref bulletReboundSoundVolume);
                                    if (v >= 9)
                                    {
                                        rw.Boolean(ref bulletShowDebris);
                                        if (v >= 10)
                                        {
                                            rw.Boolean(ref bulletModifyFOV);
                                            if (v >= 11)
                                            {
                                                rw.Boolean(ref u13);
                                                if (v >= 12)
                                                {
                                                    rw.Boolean(ref bulletIsFlare);
                                                    if (v >= 13)
                                                    {
                                                        rw.Single(ref bulletFlareAttractionRadius);
                                                        rw.Single(ref bulletFlareExplosionRadius);
                                                        if (v >= 14)
                                                        {
                                                            rw.Id(ref bulletHomingSound);
                                                            rw.Single(ref bulletHomingSoundVolume);
                                                            if (v >= 15)
                                                            {
                                                                if (v <= 18)
                                                                {
                                                                    rw.Boolean(ref u14);
                                                                }
                                                                rw.Single(ref bulletGuidedAngularSpeed);
                                                                if (v >= 16)
                                                                {
                                                                    rw.Int32(ref bulletHomingLockDuration);
                                                                    if (v >= 17)
                                                                    {
                                                                        rw.Boolean(ref bulletIsWard);
                                                                        rw.Single(ref bulletWardRadius);
                                                                        if (v >= 18)
                                                                        {
                                                                            if (v == 18)
                                                                            {
                                                                                rw.Boolean(ref u15);
                                                                            }
                                                                            rw.Int32(ref bulletGuidedMinLifeTime);
                                                                            if (v >= 20)
                                                                            {
                                                                                rw.Single(ref bulletGunSpeedCoef);
                                                                                if (v >= 21)
                                                                                {
                                                                                    rw.Int32(ref bulletType);
                                                                                    if (v >= 22)
                                                                                    {
                                                                                        rw.Int32(ref bulletHomingDamageMinAngle_Deg);
                                                                                        rw.Int32(ref bulletHomingDamageMaxAngle_Deg);
                                                                                        if (v >= 23)
                                                                                        {
                                                                                            rw.Single(ref bulletRecoil);
                                                                                            if (v >= 24)
                                                                                            {
                                                                                                rw.Single(ref bulletGunSpeedCoefRatioMin);
                                                                                                rw.Single(ref bulletGunSpeedCoefRatioMax);
                                                                                                if (v >= 25)
                                                                                                {
                                                                                                    rw.Int32(ref u16);
                                                                                                    if (v >= 26)
                                                                                                    {
                                                                                                        rw.Boolean(ref u17);
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }




}
