namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09107000</remarks>
[Class(0x09107000)]
public partial class CPlugCustomBeamModel : CMwNod, IClass, IReadableWritable
{
    [Hexadecimal] public static new uint Id => 0x09107000;




    private string? bulletName;
    public string? BulletName { get => bulletName; set => bulletName = value; }

    private CPlugBeamEmitterSubModel[]? u01;
    public CPlugBeamEmitterSubModel[]? U01 { get => u01; set => u01 = value; }

    private CPlugParticleEmitterSubModel[]? u02;
    public CPlugParticleEmitterSubModel[]? U02 { get => u02; set => u02 = value; }

    private string? u03;
    public string? U03 { get => u03; set => u03 = value; }

    private string? u04;
    public string? U04 { get => u04; set => u04 = value; }

    private string? u05;
    public string? U05 { get => u05; set => u05 = value; }

    private float u06;
    public float U06 { get => u06; set => u06 = value; }

    private float u07;
    public float U07 { get => u07; set => u07 = value; }

    private float u08;
    public float U08 { get => u08; set => u08 = value; }

    private bool u09;
    public bool U09 { get => u09; set => u09 = value; }

    private bool u10;
    public bool U10 { get => u10; set => u10 = value; }

    private bool u11;
    public bool U11 { get => u11; set => u11 = value; }

    private float u12;
    public float U12 { get => u12; set => u12 = value; }

    private float u13;
    public float U13 { get => u13; set => u13 = value; }

    private int laserDamage;
    public int LaserDamage { get => laserDamage; set => laserDamage = value; }

    private float laserRadiusDamage;
    public float LaserRadiusDamage { get => laserRadiusDamage; set => laserRadiusDamage = value; }

    private float bulletVsRadius;
    public float BulletVsRadius { get => bulletVsRadius; set => bulletVsRadius = value; }

    private Vec3 visualOffsetFirstPerson;
    public Vec3 VisualOffsetFirstPerson { get => visualOffsetFirstPerson; set => visualOffsetFirstPerson = value; }

    private byte beamType;
    public byte BeamType { get => beamType; set => beamType = value; }

    private float laserDispersionAngle;
    public float LaserDispersionAngle { get => laserDispersionAngle; set => laserDispersionAngle = value; }

    private bool damageAttenuationWithDist;
    public bool DamageAttenuationWithDist { get => damageAttenuationWithDist; set => damageAttenuationWithDist = value; }

    private float u14;
    public float U14 { get => u14; set => u14 = value; }

    private int u15;
    public int U15 { get => u15; set => u15 = value; }

    private CFuncKeysReal? damageAttenuationFromDist;
    public CFuncKeysReal? DamageAttenuationFromDist { get => damageAttenuationFromDist; set => damageAttenuationFromDist = value; }

    private float maxDistance;
    public float MaxDistance { get => maxDistance; set => maxDistance = value; }

    private byte u16;
    public byte U16 { get => u16; set => u16 = value; }

    private float laserRadius;
    public float LaserRadius { get => laserRadius; set => laserRadius = value; }

    private float blowRadius;
    public float BlowRadius { get => blowRadius; set => blowRadius = value; }

    private float blowValue;
    public float BlowValue { get => blowValue; set => blowValue = value; }

    private bool laserShowAdvancedCrosshair;
    public bool LaserShowAdvancedCrosshair { get => laserShowAdvancedCrosshair; set => laserShowAdvancedCrosshair = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugCustomBeamModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugCustomBeamModel() { }

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        rw.String(ref bulletName);
        rw.ArrayNodeRef_deprec<CPlugBeamEmitterSubModel>(ref u01!);
        rw.ArrayNodeRef_deprec<CPlugParticleEmitterSubModel>(ref u02!);
        rw.Id(ref u03);
        rw.Id(ref u04);
        rw.Id(ref u05);
        if (v >= 1)
        {
            rw.Single(ref u06);
            rw.Single(ref u07);
            rw.Single(ref u08);
            if (v >= 2)
            {
                rw.Boolean(ref u09);
                rw.Boolean(ref u10);
                if (v >= 3)
                {
                    rw.Boolean(ref u11);
                    if (v >= 4)
                    {
                        rw.Single(ref u12);
                        rw.Single(ref u13);
                        if (v >= 5)
                        {
                            rw.Int32(ref laserDamage);
                            if (v >= 6)
                            {
                                rw.Single(ref laserRadiusDamage);
                                if (v >= 7)
                                {
                                    rw.Single(ref bulletVsRadius);
                                    if (v >= 8)
                                    {
                                        rw.Vec3(ref visualOffsetFirstPerson);
                                        rw.Byte(ref beamType);
                                        if (v >= 9)
                                        {
                                            rw.Single(ref laserDispersionAngle);
                                            if (v >= 10)
                                            {
                                                rw.Boolean(ref damageAttenuationWithDist);
                                                rw.Single(ref u14);
                                                rw.Int32(ref u15);
                                                if (v >= 11)
                                                {
                                                    rw.NodeRef<CFuncKeysReal>(ref damageAttenuationFromDist);
                                                    if (v >= 12)
                                                    {
                                                        rw.Single(ref maxDistance);
                                                        rw.Byte(ref u16);
                                                        rw.Single(ref laserRadius);
                                                        if (v >= 13)
                                                        {
                                                            rw.Single(ref blowRadius);
                                                            rw.Single(ref blowValue);
                                                            if (v >= 14)
                                                            {
                                                                rw.Boolean(ref laserShowAdvancedCrosshair);
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
