namespace GBX.NET.Engines.Plug;

[Node(0x090ED000)] // previously CSceneVehicleCarTuning
[NodeExtension("VehicleTuning")]
public class CPlugVehicleCarPhyTuning : CPlugVehiclePhyTuning
{
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
    private float[]? engineGearRatios;
    private float[]? engineAutoGearMaxRPMs;
    private float[]? engineAutoGearMinRPMs;

    [NodeMember]
    [AppliedWithChunk<Chunk090ED024>]
    public CFuncKeysReal? Accel { get => accel; set => accel = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090ED028>]
    public CFuncKeysReal? MaxSideFriction { get => maxSideFriction; set => maxSideFriction = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090ED029>]
    public CFuncKeysReal? RolloverLateral { get => rolloverLateral; set => rolloverLateral = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090ED02A>]
    public CFuncKeysReal? LateralContactSlowDown { get => lateralContactSlowDown; set => lateralContactSlowDown = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090ED02B>]
    public CFuncKeysReal? SteerSlowDown { get => steerSlowDown; set => steerSlowDown = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090ED030>]
    public CFuncKeysReal? SteerDriveTorque { get => steerDriveTorque; set => steerDriveTorque = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090ED036>]
    public CFuncKeysReal? SteerRadius { get => steerRadius; set => steerRadius = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090ED037>]
    public CFuncKeysReal? MaxFrictionTorque { get => maxFrictionTorque; set => maxFrictionTorque = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090ED038>]
    public CFuncKeysReal? MaxFrictionForce { get => maxFrictionForce; set => maxFrictionForce = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090ED03D>]
    public CFuncKeysReal? SlippingAccel { get => slippingAccel; set => slippingAccel = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090ED03F>]
    public CFuncKeysReal? SteerCoef { get => steerCoef; set => steerCoef = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090ED040>]
    public CFuncKeysReal? SmoothInputSteerDuration { get => smoothInputSteerDuration; set => smoothInputSteerDuration = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090ED049>]
    public CFuncKeysReal? Modulation { get => modulation; set => modulation = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090ED04F>]
    public CFuncKeysReal? BurnoutRadius { get => burnoutRadius; set => burnoutRadius = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090ED052>]
    public CFuncKeysReal? RolloverLateralRatio { get => rolloverLateralRatio; set => rolloverLateralRatio = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090ED05D>]
    public CFuncKeysReal? RearGearAccel { get => rearGearAccel; set => rearGearAccel = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090ED05D>]
    public CFuncKeysReal? LateralSpeed { get => lateralSpeed; set => lateralSpeed = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090ED05D>]
    public CFuncKeysReal? BurnoutRollover { get => burnoutRollover; set => burnoutRollover = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090ED05D>]
    public CFuncKeysReal? DonutRollover { get => donutRollover; set => donutRollover = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090ED096>]
    public float[]? EngineGearRatios { get => engineGearRatios; set => engineGearRatios = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090ED096>]
    public float[]? EngineAutoGearMaxRPMs { get => engineAutoGearMaxRPMs; set => engineAutoGearMaxRPMs = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090ED096>]
    public float[]? EngineAutoGearMinRPMs { get => engineAutoGearMinRPMs; set => engineAutoGearMinRPMs = value; }

    internal CPlugVehicleCarPhyTuning()
    {

    }

    #region 0x000 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x000 chunk
    /// </summary>
    [Chunk(0x090ED000)]
    public class Chunk090ED000 : Chunk<CPlugVehicleCarPhyTuning>
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

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED001)]
    public class Chunk090ED001 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            n.Name = rw.Id(n.Name!)!;
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x002 chunk
    /// </summary>
    [Chunk(0x090ED002)]
    public class Chunk090ED002 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED004)]
    public class Chunk090ED004 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public bool U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    #endregion

    #region 0x005 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x005 chunk
    /// </summary>
    [Chunk(0x090ED005)]
    public class Chunk090ED005 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x007 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x007 chunk
    /// </summary>
    [Chunk(0x090ED007)]
    public class Chunk090ED007 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x009 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x009 chunk
    /// </summary>
    [Chunk(0x090ED009)]
    public class Chunk090ED009 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x00B chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x00B chunk
    /// </summary>
    [Chunk(0x090ED00B)]
    public class Chunk090ED00B : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED00D)]
    public class Chunk090ED00D : Chunk<CPlugVehicleCarPhyTuning>
    {
        public int U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x00E chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x00E chunk
    /// </summary>
    [Chunk(0x090ED00E)]
    public class Chunk090ED00E : Chunk<CPlugVehicleCarPhyTuning>
    {
        public int U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x010 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x010 chunk
    /// </summary>
    [Chunk(0x090ED010)]
    public class Chunk090ED010 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public int U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED013)]
    public class Chunk090ED013 : Chunk<CPlugVehicleCarPhyTuning>
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

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED014)]
    public class Chunk090ED014 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED016)]
    public class Chunk090ED016 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED017)]
    public class Chunk090ED017 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED018)]
    public class Chunk090ED018 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED019)]
    public class Chunk090ED019 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED01A)]
    public class Chunk090ED01A : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x01B chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x01B chunk
    /// </summary>
    [Chunk(0x090ED01B)]
    public class Chunk090ED01B : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x01D chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x01D chunk
    /// </summary>
    [Chunk(0x090ED01D)]
    public class Chunk090ED01D : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED01E)]
    public class Chunk090ED01E : Chunk<CPlugVehicleCarPhyTuning>
    {
        public int U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x01F chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x01F chunk
    /// </summary>
    [Chunk(0x090ED01F)]
    public class Chunk090ED01F : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED020)]
    public class Chunk090ED020 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED023)]
    public class Chunk090ED023 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED024)]
    public class Chunk090ED024 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.accel);
        }
    }

    #endregion

    #region 0x026 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x026 chunk
    /// </summary>
    [Chunk(0x090ED026)]
    public class Chunk090ED026 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED027)]
    public class Chunk090ED027 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED028)]
    public class Chunk090ED028 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U03;
        public float U04;
        public float U05;
        public float U06;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.NodeRef(ref n.maxSideFriction);
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
    [Chunk(0x090ED029)]
    public class Chunk090ED029 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.NodeRef(ref n.rolloverLateral);
        }
    }

    #endregion

    #region 0x02A chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x02A chunk
    /// </summary>
    [Chunk(0x090ED02A)]
    public class Chunk090ED02A : Chunk<CPlugVehicleCarPhyTuning>
    {
        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref n.lateralContactSlowDown);
        }
    }

    #endregion

    #region 0x02B chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x02B chunk
    /// </summary>
    [Chunk(0x090ED02B)]
    public class Chunk090ED02B : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U02;
        public float U03;
        public int U04;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref n.steerSlowDown);
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
    [Chunk(0x090ED02C)]
    public class Chunk090ED02C : Chunk<CPlugVehicleCarPhyTuning>
    {
        public CFuncKeysReal? U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED02D)]
    public class Chunk090ED02D : Chunk<CPlugVehicleCarPhyTuning>
    {
        public int U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x02E chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x02E chunk
    /// </summary>
    [Chunk(0x090ED02E)]
    public class Chunk090ED02E : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED030)]
    public class Chunk090ED030 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref n.steerDriveTorque);
        }
    }

    #endregion

    #region 0x031 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x031 chunk
    /// </summary>
    [Chunk(0x090ED031)]
    public class Chunk090ED031 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED032)]
    public class Chunk090ED032 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED033)]
    public class Chunk090ED033 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x034 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x034 chunk
    /// </summary>
    [Chunk(0x090ED034)]
    public class Chunk090ED034 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x035 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x035 chunk
    /// </summary>
    [Chunk(0x090ED035)]
    public class Chunk090ED035 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public bool U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    #endregion

    #region 0x036 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x036 chunk
    /// </summary>
    [Chunk(0x090ED036)]
    public class Chunk090ED036 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.NodeRef(ref n.steerRadius);
        }
    }

    #endregion

    #region 0x037 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x037 chunk
    /// </summary>
    [Chunk(0x090ED037)]
    public class Chunk090ED037 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref n.maxFrictionTorque);
        }
    }

    #endregion

    #region 0x038 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x038 chunk
    /// </summary>
    [Chunk(0x090ED038)]
    public class Chunk090ED038 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U03;
        public float U04;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.NodeRef(ref n.maxFrictionForce);
            rw.Single(ref U03);
            rw.Single(ref U04);
        }
    }

    #endregion

    #region 0x039 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x039 chunk
    /// </summary>
    [Chunk(0x090ED039)]
    public class Chunk090ED039 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED03A)]
    public class Chunk090ED03A : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED03B)]
    public class Chunk090ED03B : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x03C chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x03C chunk
    /// </summary>
    [Chunk(0x090ED03C)]
    public class Chunk090ED03C : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x03D chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x03D chunk
    /// </summary>
    [Chunk(0x090ED03D)]
    public class Chunk090ED03D : Chunk<CPlugVehicleCarPhyTuning>
    {
        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref n.slippingAccel);
        }
    }

    #endregion

    #region 0x03E chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x03E chunk
    /// </summary>
    [Chunk(0x090ED03E)]
    public class Chunk090ED03E : Chunk<CPlugVehicleCarPhyTuning>
    {
        public int U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x03F chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x03F chunk
    /// </summary>
    [Chunk(0x090ED03F)]
    public class Chunk090ED03F : Chunk<CPlugVehicleCarPhyTuning>
    {
        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref n.steerCoef);
        }
    }

    #endregion

    #region 0x040 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x040 chunk
    /// </summary>
    [Chunk(0x090ED040)]
    public class Chunk090ED040 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref n.smoothInputSteerDuration);
        }
    }

    #endregion

    #region 0x041 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x041 chunk
    /// </summary>
    [Chunk(0x090ED041)]
    public class Chunk090ED041 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public int U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED042)]
    public class Chunk090ED042 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public int U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x043 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x043 chunk
    /// </summary>
    [Chunk(0x090ED043)]
    public class Chunk090ED043 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public int U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x044 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x044 chunk
    /// </summary>
    [Chunk(0x090ED044)]
    public class Chunk090ED044 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x046 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x046 chunk
    /// </summary>
    [Chunk(0x090ED046)]
    public class Chunk090ED046 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public CFuncKeysReal? U04;
        public CFuncKeysReal? U05;
        public CFuncKeysReal? U06;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED047)]
    public class Chunk090ED047 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x048 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x048 chunk
    /// </summary>
    [Chunk(0x090ED048)]
    public class Chunk090ED048 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public CFuncKeysReal? U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    #endregion

    #region 0x049 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x049 chunk
    /// </summary>
    [Chunk(0x090ED049)]
    public class Chunk090ED049 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref n.modulation);
        }
    }

    #endregion

    #region 0x04D chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x04D chunk
    /// </summary>
    [Chunk(0x090ED04D)]
    public class Chunk090ED04D : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x04E chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x04E chunk
    /// </summary>
    [Chunk(0x090ED04E)]
    public class Chunk090ED04E : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED04F)]
    public class Chunk090ED04F : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.NodeRef(ref n.burnoutRadius);
        }
    }

    #endregion

    #region 0x051 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x051 chunk
    /// </summary>
    [Chunk(0x090ED051)]
    public class Chunk090ED051 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x052 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x052 chunk
    /// </summary>
    [Chunk(0x090ED052)]
    public class Chunk090ED052 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref n.rolloverLateralRatio);
        }
    }

    #endregion

    #region 0x053 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x053 chunk
    /// </summary>
    [Chunk(0x090ED053)]
    public class Chunk090ED053 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x056 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x056 chunk
    /// </summary>
    [Chunk(0x090ED056)]
    public class Chunk090ED056 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED057)]
    public class Chunk090ED057 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float[]? U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Array(ref U01);
        }
    }

    #endregion

    #region 0x058 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x058 chunk
    /// </summary>
    [Chunk(0x090ED058)]
    public class Chunk090ED058 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x059 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x059 chunk
    /// </summary>
    [Chunk(0x090ED059)]
    public class Chunk090ED059 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED05A)]
    public class Chunk090ED05A : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED05B)]
    public class Chunk090ED05B : Chunk<CPlugVehicleCarPhyTuning>
    {
        public CFuncKeysReal? U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED05C)]
    public class Chunk090ED05C : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
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
    [Chunk(0x090ED05D)]
    public class Chunk090ED05D : Chunk<CPlugVehicleCarPhyTuning>
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

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.NodeRef(ref n.rearGearAccel);
            rw.NodeRef(ref n.lateralSpeed);
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
            rw.NodeRef(ref n.burnoutRollover);
            rw.NodeRef(ref n.donutRollover);
            rw.Single(ref U24);
            rw.Single(ref U25);
            rw.Single(ref U26);
            rw.Single(ref U27);
            rw.Single(ref U28);
            rw.Single(ref U29);
            rw.Array(ref U30);
            rw.Array(ref U31);
            rw.Array(ref U32);
            rw.Int32(ref U33);
        }
    }

    #endregion

    #region 0x05E chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x05E chunk
    /// </summary>
    [Chunk(0x090ED05E)]
    public class Chunk090ED05E : Chunk<CPlugVehicleCarPhyTuning>
    {
        public CFuncKeysReal? U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    #endregion

    #region 0x05F chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x05F chunk
    /// </summary>
    [Chunk(0x090ED05F)]
    public class Chunk090ED05F : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x060 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x060 chunk
    /// </summary>
    [Chunk(0x090ED060)]
    public class Chunk090ED060 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public int U04;
        public int U05;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
        }
    }

    #endregion

    #region 0x061 chunk

    /// <summary>
    /// CSceneVehicleCarTuning 0x061 chunk
    /// </summary>
    [Chunk(0x090ED061)]
    public class Chunk090ED061 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public Node? U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    #endregion

    #region 0x062 chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x062 chunk
    /// </summary>
    [Chunk(0x090ED062)]
    public class Chunk090ED062 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x063 chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x063 chunk
    /// </summary>
    [Chunk(0x090ED063)]
    public class Chunk090ED063 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x064 chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x064 chunk
    /// </summary>
    [Chunk(0x090ED064)]
    public class Chunk090ED064 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x065 chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x065 chunk
    /// </summary>
    [Chunk(0x090ED065)]
    public class Chunk090ED065 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x066 chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x066 chunk
    /// </summary>
    [Chunk(0x090ED066)]
    public class Chunk090ED066 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x06A chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x06A chunk
    /// </summary>
    [Chunk(0x090ED06A)]
    public class Chunk090ED06A : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    #endregion

    #region 0x06E chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x06E chunk
    /// </summary>
    [Chunk(0x090ED06E)]
    public class Chunk090ED06E : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x072 chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x072 chunk
    /// </summary>
    [Chunk(0x090ED072)]
    public class Chunk090ED072 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public int U04;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Int32(ref U04);
        }
    }

    #endregion

    #region 0x077 chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x077 chunk
    /// </summary>
    [Chunk(0x090ED077)]
    public class Chunk090ED077 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public CFuncKeysReal? U05;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.NodeRef<CFuncKeysReal>(ref U05);
        }
    }

    #endregion

    #region 0x078 chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x078 chunk
    /// </summary>
    [Chunk(0x090ED078)]
    public class Chunk090ED078 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x079 chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x079 chunk
    /// </summary>
    [Chunk(0x090ED079)]
    public class Chunk090ED079 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public bool U01;
        public CFuncKeysReal? U02;
        public CFuncKeysReal? U03;
        public float U04;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.NodeRef<CFuncKeysReal>(ref U02);
            rw.NodeRef<CFuncKeysReal>(ref U03);
            rw.Single(ref U04);
        }
    }

    #endregion

    #region 0x07D chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x07D chunk
    /// </summary>
    [Chunk(0x090ED07D)]
    public class Chunk090ED07D : Chunk<CPlugVehicleCarPhyTuning>
    {
        public int U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
        }
    }

    #endregion

    #region 0x082 chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x082 chunk
    /// </summary>
    [Chunk(0x090ED082)]
    public class Chunk090ED082 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public bool U01;
        public CFuncKeysReal? U02;
        public CFuncKeysReal? U03;
        public CFuncKeysReal? U04;
        public CFuncKeysReal? U05;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.NodeRef<CFuncKeysReal>(ref U02);
            rw.NodeRef<CFuncKeysReal>(ref U03);
            rw.NodeRef<CFuncKeysReal>(ref U04);
            rw.NodeRef<CFuncKeysReal>(ref U05);
        }
    }

    #endregion

    #region 0x084 chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x084 chunk
    /// </summary>
    [Chunk(0x090ED084)]
    public class Chunk090ED084 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
        }
    }

    #endregion

    #region 0x085 chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x085 chunk
    /// </summary>
    [Chunk(0x090ED085)]
    public class Chunk090ED085 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public bool U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
        }
    }

    #endregion

    #region 0x086 chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x086 chunk
    /// </summary>
    [Chunk(0x090ED086)]
    public class Chunk090ED086 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    #endregion

    #region 0x088 chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x088 chunk
    /// </summary>
    [Chunk(0x090ED088)]
    public class Chunk090ED088 : Chunk<CPlugVehicleCarPhyTuning>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x089 chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x089 chunk
    /// </summary>
    [Chunk(0x090ED089)]
    public class Chunk090ED089 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    { 
        public CFuncKeysReal? U01;
        public CFuncKeysReal? U02;
        public float? U03;
        public float? U04;
        public float? U05;
        public float? U06;
        public float? U07;
        public float? U08;
        public float? U09;
        public float? U10;
        public float? U11;
        public float? U12;
        public float? U13;
        public float? U14;
        public float? U15;
        public float? U16;
        public float? U17;
        public float? U18;
        public bool? U19;
        public CFuncKeysReal? U20;
        public CFuncKeysReal? U21;
        public CFuncKeysReal? U22;

        public int Version { get; set; } = 9;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version >= 10)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }
            
            rw.NodeRef<CFuncKeysReal>(ref U01);
            
            if (Version < 3)
            {
                rw.NodeRef<CFuncKeysReal>(ref U02);
            }
            
            if (Version >= 2)
            {
                rw.Single(ref U03);
                rw.Single(ref U04);
                rw.Single(ref U05);
                rw.Single(ref U06);
                
                if (Version >= 4)
                {
                    rw.Single(ref U07);
                    rw.Single(ref U08);
                    rw.Single(ref U09);
                    rw.Single(ref U10);
                    rw.Single(ref U11);
                    rw.Single(ref U12);
                    rw.Single(ref U13);
                    rw.Single(ref U14);
                    rw.Single(ref U15);
                    rw.Single(ref U16);
                    rw.Single(ref U17);
                    
                    if (Version >= 6)
                    {
                        rw.Single(ref U18);

                        if (Version >= 7)
                        {
                            rw.Boolean(ref U19);

                            if (Version >= 8)
                            {
                                rw.NodeRef<CFuncKeysReal>(ref U20);
                                rw.NodeRef<CFuncKeysReal>(ref U21);

                                if (Version >= 9)
                                {
                                    rw.NodeRef<CFuncKeysReal>(ref U22);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region 0x08A chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x08A chunk
    /// </summary>
    [Chunk(0x090ED08A)]
    public class Chunk090ED08A : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public CFuncKeysReal? U05;
        public CFuncKeysReal? U06;
        public CFuncKeysReal? U07;
        public CFuncKeysReal? U08;
        public float? U09;
        public float? U10;
        public CFuncKeysReal? U11;
        public CFuncKeysReal? U12;
        public int? U13;
        public float? U14;
        public CFuncKeysReal? U15;
        public bool? U16;

        public int Version { get; set; } = 7;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            
            if (Version > 7)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.NodeRef<CFuncKeysReal>(ref U05);
            rw.NodeRef<CFuncKeysReal>(ref U06);
            rw.NodeRef<CFuncKeysReal>(ref U07);
            rw.NodeRef<CFuncKeysReal>(ref U08);

            if (Version >= 1)
            {
                if (Version < 4)
                {
                    rw.Single(ref U09);
                    rw.Single(ref U10);
                }
                else
                {
                    rw.NodeRef<CFuncKeysReal>(ref U11);
                    rw.NodeRef<CFuncKeysReal>(ref U12);
                }

                if (Version >= 3)
                {
                    rw.Int32(ref U13);

                    if (Version == 5)
                    {
                        rw.Single(ref U14);
                    }
                    
                    if (Version >= 6)
                    {
                        rw.NodeRef<CFuncKeysReal>(ref U15);

                        if (Version >= 7)
                        {
                            rw.Boolean(ref U16);
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region 0x08B chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x08B chunk
    /// </summary>
    [Chunk(0x090ED08B)]
    public class Chunk090ED08B : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        public float? U01;
        public float? U02;
        public CFuncKeysReal? U03;
        public float? U04;
        public CFuncKeysReal? U05;
        public float? U06;
        public float? U07;
        public CFuncKeysReal? U08;
        public bool? U09;
        public float? U10;
        public float? U11;
        public CFuncKeysReal? U12;
        public int? U13;
        public int? U14;
        public CFuncKeysReal? U15;
        public float? U16;
        public CFuncKeysReal? U17;
        public float? U18;
        public CFuncKeysReal? U19;
        public float? U20;
        public CFuncKeysReal? U21;
        public CFuncKeysReal? U22;
        public CFuncKeysReal? U23;
        public float? U24;
        public float? U25;
        public float? U26;
        public float? U27;
        public float? U28;
        public CFuncKeysReal? U29;
        public float? U30;
        public CFuncKeysReal? U31;
        public CFuncKeysReal? U32;
        public CFuncKeysReal? U33;
        public int? U34;
        public int? U35;
        public CFuncKeysReal? U36;
        public float? U37;
        public float? U38;
        public float? U39;
        public int? U40;
        public int? U41;
        public int? U42;
        public bool? U43;
        public float? U44;
        public float? U45;
        public CFuncKeysReal? U46;
        public int? U47;
        public bool? U48;
        public float? U49;
        public float? U50;
        public float? U51;
        public float? U52;
        public float? U53;
        public CFuncKeysReal? U54;
        public CFuncKeysReal? U55;
        public int? U56;
        public float? U57;
        public float? U58;
        public CFuncKeysReal? U59;
        public CFuncKeysReal? U60;
        public float? U61;
        public CFuncKeysReal? U62;

        public int Version { get; set; } = 34;
        
        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            
            if (Version > 34)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }
            
            if (Version < 2)
            {
                rw.Single(ref U01);
            }
            
            if (Version < 10)
            {
                rw.Single(ref U02);
                rw.NodeRef<CFuncKeysReal>(ref U03);
                
                if (Version < 9)
                {
                    rw.Single(ref U04);

                    if (Version < 8)
                    {
                        rw.NodeRef<CFuncKeysReal>(ref U05);
                    }
                }
            }
            
            rw.Single(ref U06);
            
            if (Version < 9)
            {
                rw.Single(ref U07);
            }
            
            if (Version < 10)
            {
                rw.NodeRef<CFuncKeysReal>(ref U08);
            }
            
            if (Version >= 1)
            {
                rw.Boolean(ref U09);
            }
            
            if (Version < 9)
            {
                rw.Single(ref U10);
                rw.Single(ref U11);
            }
            
            if (Version >= 4)
            {
                rw.NodeRef<CFuncKeysReal>(ref U12);

                if (Version >= 5)
                {
                    if (Version < 9)
                    {
                        rw.Int32(ref U13);
                        rw.Int32(ref U14);
                    }
                    else
                    {
                        rw.NodeRef<CFuncKeysReal>(ref U15);

                        if (Version >= 13)
                        {
                            if (Version < 22)
                            {
                                rw.Single(ref U16);
                            }
                            else
                            {
                                rw.NodeRef<CFuncKeysReal>(ref U17);
                            }

                            rw.Single(ref U18);
                        }
                    }
                    
                    if (Version >= 6)
                    {
                        rw.NodeRef<CFuncKeysReal>(ref U19);

                        if (Version >= 7)
                        {
                            rw.Single(ref U20);

                            if (Version >= 10)
                            {
                                rw.NodeRef<CFuncKeysReal>(ref U21);
                                rw.NodeRef<CFuncKeysReal>(ref U22);
                                rw.NodeRef<CFuncKeysReal>(ref U23);
                                
                                if (Version >= 11)
                                {
                                    rw.Single(ref U24);
                                    rw.Single(ref U25);
                                    rw.Single(ref U26);
                                    rw.Single(ref U27);
                                    rw.Single(ref U28);
                                    rw.NodeRef<CFuncKeysReal>(ref U29);

                                    if (Version >= 12)
                                    {
                                        rw.Single(ref U30);

                                        if (Version >= 14)
                                        {
                                            rw.NodeRef<CFuncKeysReal>(ref U31);
                                            
                                            if (Version >= 15)
                                            {
                                                rw.NodeRef<CFuncKeysReal>(ref U32);
                                                
                                                if (Version >= 16)
                                                {
                                                    rw.NodeRef<CFuncKeysReal>(ref U33);

                                                    if (Version >= 17)
                                                    {
                                                        rw.Int32(ref U34);
                                                        
                                                        if (Version >= 18)
                                                        {
                                                            rw.Int32(ref U35);
                                                            
                                                            if (Version >= 19)
                                                            {
                                                                rw.NodeRef<CFuncKeysReal>(ref U36);
                                                                
                                                                if (Version >= 20)
                                                                {
                                                                    rw.Single(ref U37);
                                                                    rw.Single(ref U38);
                                                                    rw.Single(ref U39);
                                                                    
                                                                    if (Version >= 21)
                                                                    {
                                                                        rw.Int32(ref U40);
                                                                        rw.Int32(ref U41);
                                                                        rw.Int32(ref U42);

                                                                        if (Version >= 23)
                                                                        {
                                                                            rw.Boolean(ref U43);
                                                                            rw.Single(ref U44);
                                                                            rw.Single(ref U45);
                                                                            rw.NodeRef<CFuncKeysReal>(ref U46);

                                                                            if (Version >= 24)
                                                                            {
                                                                                rw.Int32(ref U47);

                                                                                if (Version >= 25)
                                                                                {
                                                                                    rw.Boolean(ref U48);
                                                                                    rw.Single(ref U49);
                                                                                    
                                                                                    if (Version >= 26)
                                                                                    {
                                                                                        rw.Single(ref U50);
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
            
            if (Version - 0x1bU < 7)
            {
                rw.Single(ref U51);
                rw.Single(ref U52);
                rw.Single(ref U53);
                rw.NodeRef<CFuncKeysReal>(ref U54);
            }
            
            if (Version - 0x1cU < 6)
            {
                rw.NodeRef<CFuncKeysReal>(ref U55);
            }
            
            if (Version >= 29)
            {
                rw.Int32(ref U56);
                rw.Single(ref U57);
                rw.Single(ref U58);

                if (Version >= 34)
                {
                    return;
                }

                if (Version >= 30)
                {
                    rw.NodeRef<CFuncKeysReal>(ref U59);
                    
                    if (Version >= 31)
                    {
                        rw.NodeRef<CFuncKeysReal>(ref U60);

                        if (Version >= 32)
                        {
                            rw.Single(ref U61);
                            rw.NodeRef<CFuncKeysReal>(ref U62);
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region 0x08C chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x08C chunk
    /// </summary>
    [Chunk(0x090ED08C)]
    public class Chunk090ED08C : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float? U05;

        public int Version { get; set; } = 1;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            
            if (Version > 1)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);

            if (Version >= 1)
            {
                rw.Single(ref U05);
            }
        }
    }

    #endregion

    #region 0x08D chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x08D chunk
    /// </summary>
    [Chunk(0x090ED08D)]
    public class Chunk090ED08D : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        public float U01;
        public bool U02;
        public bool? U03;
        public float? U04;
        public float? U05;
        public float? U06;
        public float? U07;
        public float U08;
        public bool? U09;

        public int Version { get; set; } = 6;
        
        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 6)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            rw.Single(ref U01);
            rw.Boolean(ref U02);
            
            if (Version >= 2)
            {
                rw.Boolean(ref U03);
            }
            
            if (Version >= 1)
            {
                rw.Single(ref U04);
                rw.Single(ref U05);
            }
            
            if (Version == 3)
            {
                rw.Single(ref U06);
                rw.Single(ref U07);
            }
            
            if (Version >= 4)
            {
                rw.Single(ref U08);
            }

            if (Version >= 5)
            {
                rw.Boolean(ref U09);

                if (Version == 5)
                {
                    U09 = false;
                }
            }
        }
    }

    #endregion

    #region 0x08E chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x08E chunk
    /// </summary>
    [Chunk(0x090ED08E)]
    public class Chunk090ED08E : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        public ExternalNode<CPlugCamControlModel>[] U01 = Array.Empty<ExternalNode<CPlugCamControlModel>>();
        public ExternalNode<CPlugCamControlModel>[] U02 = Array.Empty<ExternalNode<CPlugCamControlModel>>();
        public CPlugCamControlModel? U03;
        public CPlugCamControlModel? U04;

        public int Version { get; set; } = 3;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 3)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            rw.Int32(10);
            rw.ArrayNode<CPlugCamControlModel>(ref U01!);

            if (Version >= 1)
            {
                rw.Int32(10);
                rw.ArrayNode<CPlugCamControlModel>(ref U02!);

                if (Version >= 2)
                {
                    rw.NodeRef<CPlugCamControlModel>(ref U03);

                    if (Version >= 3)
                    {
                        rw.NodeRef<CPlugCamControlModel>(ref U04);
                    }
                }
            }
        }
    }

    #endregion

    #region 0x094 chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x094 chunk
    /// </summary>
    [Chunk(0x090ED094)]
    public class Chunk090ED094 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        public float U01;
        public CFuncKeysReal? U02;
        public int U03;
        public float U04;
        public CFuncKeysReal? U05;
        public int U06;
        public int? U07;
        public int? U08;
        public float? U09;
        public CFuncKeysReal? U10;
        public int? U11;
        public int? U12;

        public int Version { get; set; } = 2;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 2)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            rw.Single(ref U01);
            rw.NodeRef<CFuncKeysReal>(ref U02);
            rw.Int32(ref U03);
            rw.Single(ref U04);
            rw.NodeRef<CFuncKeysReal>(ref U05);
            rw.Int32(ref U06);

            if (Version >= 1)
            {
                rw.Int32(ref U07);
                rw.Int32(ref U08);
                
                if (Version >= 2)
                {
                    rw.Single(ref U09);
                    rw.NodeRef<CFuncKeysReal>(ref U10);
                    rw.Int32(ref U11);
                    rw.Int32(ref U12);
                }
            }
        }
    }

    #endregion

    #region 0x095 chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x095 chunk
    /// </summary>
    [Chunk(0x090ED095)]
    public class Chunk090ED095 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        public int U01;
        public float U02;
        public CFuncKeysReal? U03;
        public bool U04;
        public float U05;
        public float U06;
        public float U07;

        public int Version { get; set; }

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            rw.Int32(ref U01);
            rw.Single(ref U02);
            rw.NodeRef<CFuncKeysReal>(ref U03);
            rw.Boolean(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
        }
    }

    #endregion

    #region 0x096 chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x096 chunk
    /// </summary>
    [Chunk(0x090ED096)]
    public class Chunk090ED096 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
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
        public float? U10;
        public float U11;
        public float U12;
        public float U13;
        public float U14;
        public float U15;
        public float U16;
        public float U17;
        public bool U18;
        public float U19;

        public int Version { get; set; } = 5;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 5)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);

            if (Version >= 2)
            {
                rw.Single(ref U10);
            }

            rw.Single(ref U11);
            rw.Single(ref U12);
            rw.Single(ref U13);
            rw.Single(ref U14);
            rw.Single(ref U15);
            rw.Single(ref U16);
            rw.Array<float>(ref n.engineGearRatios);
            rw.Array<float>(ref n.engineAutoGearMaxRPMs);
            rw.Array<float>(ref n.engineAutoGearMinRPMs);

            if (Version >= 3)
            {
                rw.Single(ref U17);

                if (Version >= 4)
                {
                    rw.Boolean(ref U18);

                    if (Version >= 5)
                    {
                        rw.Single(ref U19);
                    }
                }
            }
        }
    }

    #endregion

    #region 0x097 chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x097 chunk
    /// </summary>
    [Chunk(0x090ED097)]
    public class Chunk090ED097 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public bool U07;
        public CFuncKeysReal? U08;
        
        public int Version { get; set; } = 2;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Boolean(ref U07);
            
            if (Version >= 2)
            {
                rw.NodeRef<CFuncKeysReal>(ref U08);
            }
        }
    }

    #endregion

    #region 0x098 chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x098 chunk
    /// </summary>
    [Chunk(0x090ED098)]
    public class Chunk090ED098 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        public float U01;
        public int U02;
        public float U03;
        public float U04;
        public int U05;
        public float U06;
        public float U07;
        public float U08;
        public bool U09;

        public int Version { get; set; } = 2;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            rw.Single(ref U01);
            rw.Int32(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Int32(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);

            if (Version >= 2)
            {
                rw.Boolean(ref U09);
            }
        }
    }

    #endregion

    #region 0x099 chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x099 chunk
    /// </summary>
    [Chunk(0x090ED099)]
    public class Chunk090ED099 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        public CFuncKeysReal? U01;

        public int Version { get; set; }

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CFuncKeysReal>(ref U01);
        }
    }

    #endregion

    #region 0x09A chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x09A chunk
    /// </summary>
    [Chunk(0x090ED09A)]
    public class Chunk090ED09A : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        public CPlugVehicleGearBox? U01;
        public GameBoxRefTable.File? U01File;

        public int Version { get; set; }

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugVehicleGearBox>(ref U01, ref U01File);
        }
    }

    #endregion

    #region 0x09B chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x09B chunk
    /// </summary>
    [Chunk(0x090ED09B)]
    public class Chunk090ED09B : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public int U07;
        public int U08;
        public int U09;
        public float? U10;
        public float? U11;
        public float? U12;
        public float? U13;
        public float? U14;
        public float? U15;
        public float? U16;
        public float? U17;
        public float? U18;
        public float? U19;
        public float? U20;
        public float? U21;
        public float? U22;
        public float? U23;
        public float? U24;
        public float? U25;
        public float? U26;
        public float? U27;
        public float? U28;
        public float? U29;
        public float? U30;

        public int Version { get; set; } = 3;
        
        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Int32(ref U07);
            rw.Int32(ref U08);
            rw.Int32(ref U09);
            
            if (Version >= 1)
            {
                rw.Single(ref U10);
                rw.Single(ref U11);
                rw.Single(ref U12);
                rw.Single(ref U13);
                rw.Single(ref U14);
                rw.Single(ref U15);
                rw.Single(ref U16);
                rw.Single(ref U17);
                rw.Single(ref U18);
                rw.Single(ref U19);
                rw.Single(ref U20);
                rw.Single(ref U21);
                rw.Single(ref U22);
                
                if (Version >= 2)
                {
                    rw.Single(ref U23);
                    
                    if (Version >= 3)
                    {
                        rw.Single(ref U24);
                        rw.Single(ref U25);
                        rw.Single(ref U26);
                        rw.Single(ref U27);
                        rw.Single(ref U28);
                        rw.Single(ref U29);
                        rw.Single(ref U30);
                    }
                }
            }
        }
    }

    #endregion

    #region 0x09C chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x09C chunk
    /// </summary>
    [Chunk(0x090ED09C)]
    public class Chunk090ED09C : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        public float? U01;
        public CFuncKeysReal? U02;
        public CFuncKeysReal? U03;
        public CFuncKeysReal? U04;
        public float? U05;
        public float? U06;
        public CFuncKeysReal? U07;
        public CFuncKeysReal? U08;
        public CFuncKeysReal? U09;
        public CFuncKeysReal? U10;
        public CFuncKeysReal? U11;
        public CFuncKeysReal? U12;
        public CFuncKeysReal? U13;
        public float? U14;
        public float? U15;
        public CFuncKeysReal? U16;
        public float? U17;
        public CFuncKeysReal? U18;
        public float? U19;
        public float? U20;
        public float? U21;
        public float? U22;
        public int? U23;
        public CFuncKeysReal? U24;
        public CFuncKeysReal? U25;
        public float? U26;
        public float? U27;
        public CFuncKeysReal? U28;
        public CFuncKeysReal? U29;
        public CFuncKeysReal? U30;
        public CFuncKeysReal? U31;
        public float? U32;
        public float? U33;
        public CFuncKeysReal? U34;
        public float? U35;

        public int Version { get; set; } = 20;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            
            if (Version < 18)
            {
                rw.Single(ref U01);
            }
            else
            {
                rw.NodeRef<CFuncKeysReal>(ref U02);
                rw.NodeRef<CFuncKeysReal>(ref U03);
                rw.NodeRef<CFuncKeysReal>(ref U04);
            }

            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.NodeRef<CFuncKeysReal>(ref U07);
            rw.NodeRef<CFuncKeysReal>(ref U08);
            rw.NodeRef<CFuncKeysReal>(ref U09);
            rw.NodeRef<CFuncKeysReal>(ref U10);
            rw.NodeRef<CFuncKeysReal>(ref U11);
            
            if (Version >= 1)
            {
                rw.NodeRef<CFuncKeysReal>(ref U12);
                rw.NodeRef<CFuncKeysReal>(ref U13);
            }
            
            if (Version >= 2)
            {
                rw.Single(ref U14);
            }
            
            if (Version == 3)
            {
                rw.Single(ref U15);
            }
            
            if (Version >= 4)
            {
                rw.NodeRef<CFuncKeysReal>(ref U16);

                if (Version >= 5)
                {
                    rw.Single(ref U17);
                    rw.NodeRef<CFuncKeysReal>(ref U18);
                    rw.Single(ref U19);

                    rw.Single(ref U20);
                    if (Version < 14)
                    {
                        U20 *= 10000;
                    }

                    rw.Single(ref U21);
                    if (Version < 14)
                    {
                        U21 *= 100;
                    }

                    if (Version >= 6)
                    {
                        rw.Single(ref U22);

                        if (Version >= 7)
                        {
                            rw.Int32(ref U23);
                            rw.NodeRef<CFuncKeysReal>(ref U24);

                            if (Version >= 8)
                            {
                                rw.NodeRef<CFuncKeysReal>(ref U25);

                                if (Version >= 9)
                                {
                                    rw.Single(ref U26);
                                    if (Version < 14)
                                    {
                                        U26 *= 10000;
                                    }

                                    if (Version >= 10)
                                    {
                                        rw.Single(ref U27);

                                        if (Version >= 11)
                                        {
                                            rw.NodeRef<CFuncKeysReal>(ref U28);
                                            rw.NodeRef<CFuncKeysReal>(ref U29);

                                            if (Version >= 12)
                                            {
                                                rw.NodeRef<CFuncKeysReal>(ref U30);

                                                if (Version >= 13)
                                                {
                                                    rw.NodeRef<CFuncKeysReal>(ref U31);

                                                    if (Version >= 15)
                                                    {
                                                        rw.Single(ref U32);

                                                        if (Version >= 16)
                                                        {
                                                            rw.Single(ref U33);

                                                            if (Version >= 19)
                                                            {
                                                                rw.NodeRef<CFuncKeysReal>(ref U34);

                                                                if (Version >= 20)
                                                                {
                                                                    rw.Single(ref U35);
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

    #endregion

    #region 0x09D chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x09D chunk
    /// </summary>
    [Chunk(0x090ED09D)]
    public class Chunk090ED09D : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        public bool? U01;
        public float? U02;
        public float? U03;
        public float U04;
        public float U05;
        public bool? U06;
        public float U07;
        public float U08;
        public float? U09;
        public CFuncKeysReal? U10;
        public CFuncKeysReal? U11;
        public CFuncKeysReal? U12;
        public CFuncKeysReal? U13;
        public CFuncKeysReal? U14;
        public CFuncKeysReal? U15;
        public CFuncKeysReal? U16;
        public float? U17;
        public float? U18;
        public float? U19;
        public CFuncKeysReal? U20;
        public CFuncKeysReal? U21;
        public CFuncKeysReal? U22;
        public CFuncKeysReal? U23;
        public CFuncKeysReal? U24;
        public float? U25;
        public CFuncKeysReal? U26;
        public float? U27;
        public float? U28;
        public float? U29;
        public CFuncKeysReal? U30;
        public float? U31;
        public CFuncKeysReal? U32;
        public CFuncKeysReal? U33;
        public CFuncKeysReal? U34;
        public float? U35;
        public int? U36;
        public float? U37;
        public float? U38;
        public CFuncKeysReal? U39;
        public bool? U40;

        public int Version { get; set; } = 19;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            
            if (Version == 0)
            {
                rw.Boolean(ref U01);
                rw.Single(ref U02);
                rw.Single(ref U03);
                rw.Single(ref U04);
                rw.Single(ref U05);
                rw.Boolean(ref U06);
                rw.Single(ref U07);
                rw.Single(ref U08);
            }
            else
            {
                rw.NodeRef<CFuncKeysReal>(ref U10);
                rw.Single(ref U05);
                rw.NodeRef<CFuncKeysReal>(ref U11);
                rw.Single(ref U09);
                rw.Single(ref U08);
                rw.Single(ref U04);
                rw.Single(ref U07);
                rw.NodeRef<CFuncKeysReal>(ref U12);
                rw.NodeRef<CFuncKeysReal>(ref U13);
            }
            
            if (Version >= 2)
            {
                rw.NodeRef<CFuncKeysReal>(ref U14);

                if (Version >= 3)
                {
                    rw.NodeRef<CFuncKeysReal>(ref U15);
                    rw.NodeRef<CFuncKeysReal>(ref U16);

                    if (Version >= 4)
                    {
                        rw.Single(ref U17);
                        rw.Single(ref U18);

                        if (Version >= 5)
                        {
                            rw.Single(ref U19);
                            rw.NodeRef<CFuncKeysReal>(ref U20);
                            rw.NodeRef<CFuncKeysReal>(ref U21);
                            rw.NodeRef<CFuncKeysReal>(ref U22);
                            rw.NodeRef<CFuncKeysReal>(ref U23);

                            if (Version >= 6)
                            {
                                rw.NodeRef<CFuncKeysReal>(ref U24);

                                if (Version >= 8)
                                {
                                    rw.Single(ref U25);
                                    rw.NodeRef<CFuncKeysReal>(ref U26);

                                    if (Version >= 9)
                                    {
                                        rw.Single(ref U27);
                                        rw.Single(ref U28);
                                        rw.Single(ref U29);

                                        if (Version >= 10)
                                        {
                                            rw.NodeRef<CFuncKeysReal>(ref U30);

                                            if (Version >= 11)
                                            {
                                                rw.Single(ref U31);

                                                if (Version >= 12)
                                                {
                                                    rw.NodeRef<CFuncKeysReal>(ref U32);

                                                    if (Version >= 13)
                                                    {
                                                        rw.NodeRef<CFuncKeysReal>(ref U33);

                                                        if (Version >= 14)
                                                        {
                                                            rw.NodeRef<CFuncKeysReal>(ref U34);

                                                            if (Version >= 15)
                                                            {
                                                                rw.Single(ref U35);
                                                                rw.Int32(ref U36);

                                                                if (Version >= 16)
                                                                {
                                                                    rw.Single(ref U37);

                                                                    if (Version >= 17)
                                                                    {
                                                                        rw.Single(ref U38);

                                                                        if (Version >= 18)
                                                                        {
                                                                            rw.NodeRef<CFuncKeysReal>(ref U39);

                                                                            if (Version >= 19)
                                                                            {
                                                                                rw.Boolean(ref U40);
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

    #endregion

    #region 0x09E chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x09E chunk
    /// </summary>
    [Chunk(0x090ED09E)]
    public class Chunk090ED09E : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        public float U01;
        public float U02;
        public float U03;
        public float? U04;
        public int? U05;
        public float? U06;
        public float? U07;
        public float? U08;
        public CFuncKeysReal? U09;
        public float? U10;
        public float? U11;

        public int Version { get; set; } = 5;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);

            if (Version >= 1)
            {
                rw.Single(ref U04);

                if (Version >= 2)
                {
                    rw.Int32(ref U05);
                    rw.Single(ref U06);

                    if (Version >= 3)
                    {
                        rw.Single(ref U07);

                        if (Version >= 4)
                        {
                            rw.Single(ref U08);
                            rw.NodeRef<CFuncKeysReal>(ref U09);
                            rw.Single(ref U10);

                            if (Version >= 5)
                            {
                                rw.Single(ref U11);
                            }
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region 0x09F chunk

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x09F chunk
    /// </summary>
    [Chunk(0x090ED09F)]
    public class Chunk090ED09F : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        public CFuncKeysReal? U01;
        public float U02;
        public float U03;
        public float U04;
        public float? U05;
        public CFuncKeysReal? U06;
        public CFuncKeysReal? U07;
        public float? U08;
        public float? U09;
        public float? U10;
        public float? U11;
        public float? U12;
        public float? U13;
        public float? U14;
        public CFuncKeysReal? U15;
        public float? U16;
        public CFuncKeysReal? U17;
        public CFuncKeysReal? U18;
        public float? U19;
        public float? U20;
        public float? U21;
        public float? U22;
        public float? U23;
        public Vec3? U24;
        public Vec3? U25;
        public CFuncKeysReal? U26;
        public CFuncKeysReal? U27;
        public bool? U28;
        public int? U29;
        public int? U30;
        public int? U31;
        public int? U32;
        public int? U33;

        public int Version { get; set; } = 13;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            rw.NodeRef<CFuncKeysReal>(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);

            if (Version >= 1)
            {
                rw.Single(ref U05);

                if (Version >= 2)
                {
                    rw.NodeRef<CFuncKeysReal>(ref U06);

                    if (Version >= 3)
                    {
                        rw.NodeRef<CFuncKeysReal>(ref U07);

                        if (Version >= 4)
                        {
                            rw.Single(ref U08);
                            rw.Single(ref U09);
                            rw.Single(ref U10);
                            rw.Single(ref U11);
                            rw.Single(ref U12);
                            rw.Single(ref U13);

                            if (Version >= 5)
                            {
                                rw.Single(ref U14);

                                if (Version >= 6)
                                {
                                    rw.NodeRef<CFuncKeysReal>(ref U15);

                                    if (Version >= 7)
                                    {
                                        rw.Single(ref U16);
                                        rw.NodeRef<CFuncKeysReal>(ref U17);
                                        rw.NodeRef<CFuncKeysReal>(ref U18);
                                        rw.Single(ref U19);

                                        if (Version >= 8)
                                        {
                                            rw.Single(ref U20);
                                            rw.Single(ref U21);
                                            rw.Single(ref U22);
                                            rw.Single(ref U23);

                                            if (Version >= 9)
                                            {
                                                rw.Vec3(ref U24);
                                                rw.Vec3(ref U25);

                                                if (Version >= 10)
                                                {
                                                    rw.NodeRef<CFuncKeysReal>(ref U26);

                                                    if (Version >= 11)
                                                    {
                                                        rw.NodeRef<CFuncKeysReal>(ref U27);

                                                        if (Version >= 12)
                                                        {
                                                            rw.Boolean(ref U28);

                                                            if (Version >= 13)
                                                            {
                                                                rw.Int32(ref U29);
                                                                rw.Int32(ref U30);
                                                                rw.Int32(ref U31);
                                                                rw.Int32(ref U32);
                                                                rw.Int32(ref U33);
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

    #endregion
}
