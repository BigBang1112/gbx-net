namespace GBX.NET.Engines.Scene;

[Node(0x0A029000)]
[NodeExtension("VehicleTuning")]
public class CSceneVehicleCarTuning : CSceneVehicleTuning
{
    private string name = "";
    private CFuncKeysReal? accel;
    private CFuncKeysReal? maxSideFriction;
    private CFuncKeysReal? rolloverLateral;
    private CFuncKeysReal? lateralContactSlowDown;
    private CFuncKeysReal? steerSlowDown;
    private CFuncKeysReal? steerDriveTorque;
    private CFuncKeysReal? steerRadius;
    private CFuncKeysReal? maxFrictionTorque;
    private CFuncKeysReal? maxFrictionForce;
    private CFuncKeysReal? slippingAccel;
    private CFuncKeysReal? steerCoef;
    private CFuncKeysReal? smoothInputSteerDuration;
    private CFuncKeysReal? modulation;
    private CFuncKeysReal? burnoutRadius;
    private CFuncKeysReal? rolloverLateralRatio;
    private CFuncKeysReal? rearGearAccel;
    private CFuncKeysReal? lateralSpeed;
    private CFuncKeysReal? burnoutRollover;
    private CFuncKeysReal? donutRollover;

    [NodeMember]
    [AppliedWithChunk<Chunk0A029001>]
    public string Name { get => name; set => name = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0A029024>]
    public CFuncKeysReal? Accel { get => accel; set => accel = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0A029028>]
    public CFuncKeysReal? MaxSideFriction { get => maxSideFriction; set => maxSideFriction = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0A029029>]
    public CFuncKeysReal? RolloverLateral { get => rolloverLateral; set => rolloverLateral = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0A02902A>]
    public CFuncKeysReal? LateralContactSlowDown { get => lateralContactSlowDown; set => lateralContactSlowDown = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0A02902B>]
    public CFuncKeysReal? SteerSlowDown { get => steerSlowDown; set => steerSlowDown = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0A029030>]
    public CFuncKeysReal? SteerDriveTorque { get => steerDriveTorque; set => steerDriveTorque = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0A029036>]
    public CFuncKeysReal? SteerRadius { get => steerRadius; set => steerRadius = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0A029037>]
    public CFuncKeysReal? MaxFrictionTorque { get => maxFrictionTorque; set => maxFrictionTorque = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0A029038>]
    public CFuncKeysReal? MaxFrictionForce { get => maxFrictionForce; set => maxFrictionForce = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0A02903D>]
    public CFuncKeysReal? SlippingAccel { get => slippingAccel; set => slippingAccel = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0A02903F>]
    public CFuncKeysReal? SteerCoef { get => steerCoef; set => steerCoef = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0A029040>]
    public CFuncKeysReal? SmoothInputSteerDuration { get => smoothInputSteerDuration; set => smoothInputSteerDuration = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0A029049>]
    public CFuncKeysReal? Modulation { get => modulation; set => modulation = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0A02904F>]
    public CFuncKeysReal? BurnoutRadius { get => burnoutRadius; set => burnoutRadius = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0A029052>]
    public CFuncKeysReal? RolloverLateralRatio { get => rolloverLateralRatio; set => rolloverLateralRatio = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0A02905D>]
    public CFuncKeysReal? RearGearAccel { get => rearGearAccel; set => rearGearAccel = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0A02905D>]
    public CFuncKeysReal? LateralSpeed { get => lateralSpeed; set => lateralSpeed = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0A02905D>]
    public CFuncKeysReal? BurnoutRollover { get => burnoutRollover; set => burnoutRollover = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0A02905D>]
    public CFuncKeysReal? DonutRollover { get => donutRollover; set => donutRollover = value; }

    internal CSceneVehicleCarTuning()
    {

    }

    #region 0x000 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x000 chunk
    /// </summary>
    [Chunk(0x0A029000)]
    public class Chunk0A029000 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;
        public float U11;
        public float U12;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);
            rw.Single(ref U10);
            rw.Single(ref U11);
            rw.Single(ref U12);
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x001 chunk
    /// </summary>
    [Chunk(0x0A029001)]
    public class Chunk0A029001 : Chunk<CSceneVehicleCarTuning>
    {
        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Id(ref n.name!);
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x002 chunk
    /// </summary>
    [Chunk(0x0A029002)]
    public class Chunk0A029002 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x004 chunk
    /// </summary>
    [Chunk(0x0A029004)]
    public class Chunk0A029004 : Chunk<CSceneVehicleCarTuning>
    {
        public bool U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    #endregion

    #region 0x005 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x005 chunk
    /// </summary>
    [Chunk(0x0A029005)]
    public class Chunk0A029005 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x007 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x007 chunk
    /// </summary>
    [Chunk(0x0A029007)]
    public class Chunk0A029007 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x009 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x009 chunk
    /// </summary>
    [Chunk(0x0A029009)]
    public class Chunk0A029009 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x00B chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x00B chunk
    /// </summary>
    [Chunk(0x0A02900B)]
    public class Chunk0A02900B : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
        }
    }

    #endregion

    #region 0x00D chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x00D chunk
    /// </summary>
    [Chunk(0x0A02900D)]
    public class Chunk0A02900D : Chunk<CSceneVehicleCarTuning>
    {
        public int U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x00E chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x00E chunk
    /// </summary>
    [Chunk(0x0A02900E)]
    public class Chunk0A02900E : Chunk<CSceneVehicleCarTuning>
    {
        public int U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x010 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x010 chunk
    /// </summary>
    [Chunk(0x0A029010)]
    public class Chunk0A029010 : Chunk<CSceneVehicleCarTuning>
    {
        public int U01;
        public float U02;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Single(ref U02);
        }
    }

    #endregion

    #region 0x013 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x013 chunk
    /// </summary>
    [Chunk(0x0A029013)]
    public class Chunk0A029013 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;
        public float U11;
        public float U12;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);
            rw.Single(ref U10);
            rw.Single(ref U11);
            rw.Single(ref U12);
        }
    }

    #endregion

    #region 0x014 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x014 chunk
    /// </summary>
    [Chunk(0x0A029014)]
    public class Chunk0A029014 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    #endregion

    #region 0x016 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x016 chunk
    /// </summary>
    [Chunk(0x0A029016)]
    public class Chunk0A029016 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    #endregion

    #region 0x017 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x017 chunk
    /// </summary>
    [Chunk(0x0A029017)]
    public class Chunk0A029017 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
        }
    }

    #endregion

    #region 0x018 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x018 chunk
    /// </summary>
    [Chunk(0x0A029018)]
    public class Chunk0A029018 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
        }
    }

    #endregion

    #region 0x019 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x019 chunk
    /// </summary>
    [Chunk(0x0A029019)]
    public class Chunk0A029019 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
        }
    }

    #endregion

    #region 0x01A chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x01A chunk
    /// </summary>
    [Chunk(0x0A02901A)]
    public class Chunk0A02901A : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x01B chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x01B chunk
    /// </summary>
    [Chunk(0x0A02901B)]
    public class Chunk0A02901B : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x01D chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x01D chunk
    /// </summary>
    [Chunk(0x0A02901D)]
    public class Chunk0A02901D : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    #endregion

    #region 0x01E chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x01E chunk
    /// </summary>
    [Chunk(0x0A02901E)]
    public class Chunk0A02901E : Chunk<CSceneVehicleCarTuning>
    {
        public int U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x01F chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x01F chunk
    /// </summary>
    [Chunk(0x0A02901F)]
    public class Chunk0A02901F : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
        }
    }

    #endregion

    #region 0x020 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x020 chunk
    /// </summary>
    [Chunk(0x0A029020)]
    public class Chunk0A029020 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
        }
    }

    #endregion

    #region 0x023 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x023 chunk
    /// </summary>
    [Chunk(0x0A029023)]
    public class Chunk0A029023 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    #endregion

    #region 0x024 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x024 chunk
    /// </summary>
    [Chunk(0x0A029024)]
    public class Chunk0A029024 : Chunk<CSceneVehicleCarTuning>
    {
        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.accel);
        }
    }

    #endregion

    #region 0x026 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x026 chunk
    /// </summary>
    [Chunk(0x0A029026)]
    public class Chunk0A029026 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
        }
    }

    #endregion

    #region 0x027 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x027 chunk
    /// </summary>
    [Chunk(0x0A029027)]
    public class Chunk0A029027 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    #endregion

    #region 0x028 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x028 chunk
    /// </summary>
    [Chunk(0x0A029028)]
    public class Chunk0A029028 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U03;
        public float U04;
        public float U05;
        public float U06;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.NodeRef<CFuncKeysReal>(ref n.maxSideFriction);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
        }
    }

    #endregion

    #region 0x029 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x029 chunk
    /// </summary>
    [Chunk(0x0A029029)]
    public class Chunk0A029029 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.NodeRef<CFuncKeysReal>(ref n.rolloverLateral);
        }
    }

    #endregion

    #region 0x02A chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x02A chunk
    /// </summary>
    [Chunk(0x0A02902A)]
    public class Chunk0A02902A : Chunk<CSceneVehicleCarTuning>
    {
        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.lateralContactSlowDown);
        }
    }

    #endregion

    #region 0x02B chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x02B chunk
    /// </summary>
    [Chunk(0x0A02902B)]
    public class Chunk0A02902B : Chunk<CSceneVehicleCarTuning>
    {
        public float U02;
        public float U03;
        public int U04;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.steerSlowDown);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Int32(ref U04);
        }
    }

    #endregion

    #region 0x02C chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x02C chunk
    /// </summary>
    [Chunk(0x0A02902C)]
    public class Chunk0A02902C : Chunk<CSceneVehicleCarTuning>
    {
        public CFuncKeysReal? U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
        }
    }

    #endregion

    #region 0x02D chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x02D chunk
    /// </summary>
    [Chunk(0x0A02902D)]
    public class Chunk0A02902D : Chunk<CSceneVehicleCarTuning>
    {
        public int U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x02E chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x02E chunk
    /// </summary>
    [Chunk(0x0A02902E)]
    public class Chunk0A02902E : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    #endregion

    #region 0x030 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x030 chunk
    /// </summary>
    [Chunk(0x0A029030)]
    public class Chunk0A029030 : Chunk<CSceneVehicleCarTuning>
    {
        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.steerDriveTorque);
        }
    }

    #endregion

    #region 0x031 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x031 chunk
    /// </summary>
    [Chunk(0x0A029031)]
    public class Chunk0A029031 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    #endregion

    #region 0x032 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x032 chunk
    /// </summary>
    [Chunk(0x0A029032)]
    public class Chunk0A029032 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
        }
    }

    #endregion

    #region 0x033 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x033 chunk
    /// </summary>
    [Chunk(0x0A029033)]
    public class Chunk0A029033 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x034 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x034 chunk
    /// </summary>
    [Chunk(0x0A029034)]
    public class Chunk0A029034 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x035 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x035 chunk
    /// </summary>
    [Chunk(0x0A029035)]
    public class Chunk0A029035 : Chunk<CSceneVehicleCarTuning>
    {
        public bool U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    #endregion

    #region 0x036 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x036 chunk
    /// </summary>
    [Chunk(0x0A029036)]
    public class Chunk0A029036 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.NodeRef<CFuncKeysReal>(ref n.steerRadius);
        }
    }

    #endregion

    #region 0x037 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x037 chunk
    /// </summary>
    [Chunk(0x0A029037)]
    public class Chunk0A029037 : Chunk<CSceneVehicleCarTuning>
    {
        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.maxFrictionTorque);
        }
    }

    #endregion

    #region 0x038 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x038 chunk
    /// </summary>
    [Chunk(0x0A029038)]
    public class Chunk0A029038 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U03;
        public float U04;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.NodeRef<CFuncKeysReal>(ref n.maxFrictionForce);
            rw.Single(ref U03);
            rw.Single(ref U04);
        }
    }

    #endregion

    #region 0x039 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x039 chunk
    /// </summary>
    [Chunk(0x0A029039)]
    public class Chunk0A029039 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    #endregion

    #region 0x03A chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x03A chunk
    /// </summary>
    [Chunk(0x0A02903A)]
    public class Chunk0A02903A : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    #endregion

    #region 0x03B chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x03B chunk
    /// </summary>
    [Chunk(0x0A02903B)]
    public class Chunk0A02903B : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x03C chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x03C chunk
    /// </summary>
    [Chunk(0x0A02903C)]
    public class Chunk0A02903C : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x03D chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x03D chunk
    /// </summary>
    [Chunk(0x0A02903D)]
    public class Chunk0A02903D : Chunk<CSceneVehicleCarTuning>
    {
        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.slippingAccel);
        }
    }

    #endregion

    #region 0x03E chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x03E chunk
    /// </summary>
    [Chunk(0x0A02903E)]
    public class Chunk0A02903E : Chunk<CSceneVehicleCarTuning>
    {
        public int U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x03F chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x03F chunk
    /// </summary>
    [Chunk(0x0A02903F)]
    public class Chunk0A02903F : Chunk<CSceneVehicleCarTuning>
    {
        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.steerCoef);
        }
    }

    #endregion

    #region 0x040 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x040 chunk
    /// </summary>
    [Chunk(0x0A029040)]
    public class Chunk0A029040 : Chunk<CSceneVehicleCarTuning>
    {
        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.smoothInputSteerDuration);
        }
    }

    #endregion

    #region 0x041 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x041 chunk
    /// </summary>
    [Chunk(0x0A029041)]
    public class Chunk0A029041 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public int U02;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Int32(ref U02);
        }
    }

    #endregion

    #region 0x042 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x042 chunk
    /// </summary>
    [Chunk(0x0A029042)]
    public class Chunk0A029042 : Chunk<CSceneVehicleCarTuning>
    {
        public int U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x043 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x043 chunk
    /// </summary>
    [Chunk(0x0A029043)]
    public class Chunk0A029043 : Chunk<CSceneVehicleCarTuning>
    {
        public int U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x044 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x044 chunk
    /// </summary>
    [Chunk(0x0A029044)]
    public class Chunk0A029044 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x046 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x046 chunk
    /// </summary>
    [Chunk(0x0A029046)]
    public class Chunk0A029046 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public CFuncKeysReal? U04;
        public CFuncKeysReal? U05;
        public CFuncKeysReal? U06;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.NodeRef(ref U04);
            rw.NodeRef(ref U05);
            rw.NodeRef(ref U06);
        }
    }

    #endregion

    #region 0x047 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x047 chunk
    /// </summary>
    [Chunk(0x0A029047)]
    public class Chunk0A029047 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x048 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x048 chunk
    /// </summary>
    [Chunk(0x0A029048)]
    public class Chunk0A029048 : Chunk<CSceneVehicleCarTuning>
    {
        public CFuncKeysReal? U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    #endregion

    #region 0x049 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x049 chunk
    /// </summary>
    [Chunk(0x0A029049)]
    public class Chunk0A029049 : Chunk<CSceneVehicleCarTuning>
    {
        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.modulation);
        }
    }

    #endregion

    #region 0x04D chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x04D chunk
    /// </summary>
    [Chunk(0x0A02904D)]
    public class Chunk0A02904D : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x04E chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x04E chunk
    /// </summary>
    [Chunk(0x0A02904E)]
    public class Chunk0A02904E : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    #endregion

    #region 0x04F chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x04F chunk
    /// </summary>
    [Chunk(0x0A02904F)]
    public class Chunk0A02904F : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.NodeRef<CFuncKeysReal>(ref n.burnoutRadius);
        }
    }

    #endregion

    #region 0x051 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x051 chunk
    /// </summary>
    [Chunk(0x0A029051)]
    public class Chunk0A029051 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x052 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x052 chunk
    /// </summary>
    [Chunk(0x0A029052)]
    public class Chunk0A029052 : Chunk<CSceneVehicleCarTuning>
    {
        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.rolloverLateralRatio);
        }
    }

    #endregion

    #region 0x053 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x053 chunk
    /// </summary>
    [Chunk(0x0A029053)]
    public class Chunk0A029053 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x056 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x056 chunk
    /// </summary>
    [Chunk(0x0A029056)]
    public class Chunk0A029056 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
        }
    }

    #endregion

    #region 0x057 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x057 chunk
    /// </summary>
    [Chunk(0x0A029057)]
    public class Chunk0A029057 : Chunk<CSceneVehicleCarTuning>
    {
        public float[]? U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Array<float>(ref U01);
        }
    }

    #endregion

    #region 0x058 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x058 chunk
    /// </summary>
    [Chunk(0x0A029058)]
    public class Chunk0A029058 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion
    
    #region 0x059 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x059 chunk
    /// </summary>
    [Chunk(0x0A029059)]
    public class Chunk0A029059 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
        }
    }

    #endregion

    #region 0x05A chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x05A chunk
    /// </summary>
    [Chunk(0x0A02905A)]
    public class Chunk0A02905A : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
        }
    }

    #endregion

    #region 0x05B chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x05B chunk
    /// </summary>
    [Chunk(0x0A02905B)]
    public class Chunk0A02905B : Chunk<CSceneVehicleCarTuning>
    {
        public CFuncKeysReal? U01;
        public float U02;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
            rw.Single(ref U02);
        }
    }

    #endregion

    #region 0x05C chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x05C chunk
    /// </summary>
    [Chunk(0x0A02905C)]
    public class Chunk0A02905C : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    #endregion

    #region 0x05D chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x05D chunk
    /// </summary>
    [Chunk(0x0A02905D)]
    public class Chunk0A02905D : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;
        public float U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;
        public float U11;
        public float U12;
        public float U13;
        public int U14;
        public float U15;
        public float U16;
        public int U17;
        public float U18;
        public float U19;
        public float U20;
        public float U21;
        public float U24;
        public float U25;
        public float U26;
        public float U27;
        public float U28;
        public float U29;
        public float[]? U30;
        public float[]? U31;
        public float[]? U32;
        public int U33;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.NodeRef<CFuncKeysReal>(ref n.rearGearAccel);
            rw.NodeRef<CFuncKeysReal>(ref n.lateralSpeed);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);
            rw.Single(ref U10);
            rw.Single(ref U11);
            rw.Single(ref U12);
            rw.Single(ref U13);
            rw.Int32(ref U14);
            rw.Single(ref U15);
            rw.Single(ref U16);
            rw.Int32(ref U17);
            rw.Single(ref U18);
            rw.Single(ref U19);
            rw.Single(ref U20);
            rw.Single(ref U21);
            rw.NodeRef<CFuncKeysReal>(ref n.burnoutRollover);
            rw.NodeRef<CFuncKeysReal>(ref n.donutRollover);
            rw.Single(ref U24);
            rw.Single(ref U25);
            rw.Single(ref U26);
            rw.Single(ref U27);
            rw.Single(ref U28);
            rw.Single(ref U29);
            rw.Array<float>(ref U30);
            rw.Array<float>(ref U31);
            rw.Array<float>(ref U32);
            rw.Int32(ref U33);
        }
    }

    #endregion

    #region 0x05E chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x05E chunk
    /// </summary>
    [Chunk(0x0A02905E)]
    public class Chunk0A02905E : Chunk<CSceneVehicleCarTuning>
    {
        public CFuncKeysReal? U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    #endregion

    #region 0x05F chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x05F chunk
    /// </summary>
    [Chunk(0x0A02905F)]
    public class Chunk0A02905F : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x060 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x060 chunk
    /// </summary>
    [Chunk(0x0A029060)]
    public class Chunk0A029060 : Chunk<CSceneVehicleCarTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public int U04;
        public int U05;

        public override void ReadWrite(CSceneVehicleCarTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
        }
    }

    #endregion
}
