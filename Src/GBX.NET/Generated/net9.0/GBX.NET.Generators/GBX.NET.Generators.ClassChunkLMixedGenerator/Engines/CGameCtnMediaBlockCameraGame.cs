namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03084000</remarks>
[Class(0x03084000)]
public partial class CGameCtnMediaBlockCameraGame : CGameCtnMediaBlockCamera, IClass, CGameCtnMediaBlock.IHasTwoKeys
{
    [Hexadecimal] public static new uint Id => 0x03084000;

    TimeSingle IHasTwoKeys.Start { get => Start; set => Start = value; }
    TimeSingle IHasTwoKeys.End { get => End; set => End = value; }



    private TimeSingle start;
    [AppliedWithChunk<Chunk03084000>]
    [AppliedWithChunk<Chunk03084001>]
    [AppliedWithChunk<Chunk03084003>]
    [AppliedWithChunk<Chunk03084004>]
    [AppliedWithChunk<Chunk03084005>]
    [AppliedWithChunk<Chunk03084006>]
    [AppliedWithChunk<Chunk03084007>]
    public TimeSingle Start { get => start; set => start = value; }

    private TimeSingle end;
    [AppliedWithChunk<Chunk03084000>]
    [AppliedWithChunk<Chunk03084001>]
    [AppliedWithChunk<Chunk03084003>]
    [AppliedWithChunk<Chunk03084004>]
    [AppliedWithChunk<Chunk03084005>]
    [AppliedWithChunk<Chunk03084006>]
    [AppliedWithChunk<Chunk03084007>]
    public TimeSingle End { get => end; set => end = value; }

    private EGameCamOld? gameCamOld;
    [AppliedWithChunk<Chunk03084000>]
    [AppliedWithChunk<Chunk03084001>]
    public EGameCamOld? GameCamOld { get => gameCamOld; set => gameCamOld = value; }

    private int clipEntId;
    [AppliedWithChunk<Chunk03084001>]
    [AppliedWithChunk<Chunk03084003>]
    [AppliedWithChunk<Chunk03084004>]
    [AppliedWithChunk<Chunk03084005>]
    [AppliedWithChunk<Chunk03084006>]
    [AppliedWithChunk<Chunk03084007>]
    public int ClipEntId { get => clipEntId; set => clipEntId = value; }

    private string? gameCamId;
    [AppliedWithChunk<Chunk03084003>]
    [AppliedWithChunk<Chunk03084004>]
    [AppliedWithChunk<Chunk03084005>]
    [AppliedWithChunk<Chunk03084006>]
    [AppliedWithChunk<Chunk03084007>]
    public string? GameCamId { get => gameCamId; set => gameCamId = value; }

    private Vec3 camPosition;
    [AppliedWithChunk<Chunk03084004>]
    [AppliedWithChunk<Chunk03084005>]
    [AppliedWithChunk<Chunk03084006>]
    [AppliedWithChunk<Chunk03084007>]
    public Vec3 CamPosition { get => camPosition; set => camPosition = value; }

    private Vec3 camPitchYawRoll;
    [AppliedWithChunk<Chunk03084004>]
    [AppliedWithChunk<Chunk03084005>]
    [AppliedWithChunk<Chunk03084006>]
    [AppliedWithChunk<Chunk03084007>]
    public Vec3 CamPitchYawRoll { get => camPitchYawRoll; set => camPitchYawRoll = value; }

    private float camFov = 90;
    [AppliedWithChunk<Chunk03084004>]
    [AppliedWithChunk<Chunk03084005>]
    [AppliedWithChunk<Chunk03084006>]
    [AppliedWithChunk<Chunk03084007>]
    public float CamFov { get => camFov; set => camFov = value; }

    private float camNearClipPlane = -1;
    [AppliedWithChunk<Chunk03084004>]
    [AppliedWithChunk<Chunk03084005>]
    [AppliedWithChunk<Chunk03084006>]
    [AppliedWithChunk<Chunk03084007>]
    public float CamNearClipPlane { get => camNearClipPlane; set => camNearClipPlane = value; }

    private float camFarClipPlane = -1;
    [AppliedWithChunk<Chunk03084004>]
    [AppliedWithChunk<Chunk03084005>]
    [AppliedWithChunk<Chunk03084006>]
    [AppliedWithChunk<Chunk03084007>]
    public float CamFarClipPlane { get => camFarClipPlane; set => camFarClipPlane = value; }

    private EGameCam gameCam;
    [AppliedWithChunk<Chunk03084007>]
    public EGameCam GameCam { get => gameCam; set => gameCam = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockCameraGame"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockCameraGame() { }


    /// <summary>
    /// CGameCtnMediaBlockCameraGame 0x000 chunk
    /// </summary>
    [Chunk(0x03084000)]
    public partial class Chunk03084000 : Chunk<CGameCtnMediaBlockCameraGame>
    {
        /// <inheritdoc />
        public override uint Id => 0x03084000;


        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GbxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.EnumInt32<EGameCamOld>(ref n.gameCamOld);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockCameraGame 0x001 chunk
    /// </summary>
    [Chunk(0x03084001)]
    public partial class Chunk03084001 : Chunk03084000
    {
        /// <inheritdoc />
        public override uint Id => 0x03084001;


        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.Int32(ref n.clipEntId);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockCameraGame 0x003 chunk
    /// </summary>
    [Chunk(0x03084003)]
    public partial class Chunk03084003 : Chunk<CGameCtnMediaBlockCameraGame>
    {
        /// <inheritdoc />
        public override uint Id => 0x03084003;


        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GbxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.Id(ref n.gameCamId);
            rw.Int32(ref n.clipEntId);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockCameraGame 0x004 chunk
    /// </summary>
    [Chunk(0x03084004)]
    public partial class Chunk03084004 : Chunk03084003
    {
        /// <inheritdoc />
        public override uint Id => 0x03084004;

        /// <summary>
        /// always 10
        /// </summary>
        public float U01;
        /// <summary>
        /// depth? 0 or 0.02
        /// </summary>
        public float U02;

        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.Vec3(ref n.camPosition);
            rw.Vec3(ref n.camPitchYawRoll);
            rw.Single(ref n.camFov);
            rw.Single(ref U01); // always 10
            rw.Single(ref U02); // depth? 0 or 0.02
            rw.Single(ref n.camNearClipPlane);
            rw.Single(ref n.camFarClipPlane);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockCameraGame 0x005 chunk
    /// </summary>
    [Chunk(0x03084005)]
    public partial class Chunk03084005 : Chunk03084004
    {
        /// <inheritdoc />
        public override uint Id => 0x03084005;

        public bool U03;

        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.Boolean(ref U03);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockCameraGame 0x006 chunk
    /// </summary>
    [Chunk(0x03084006)]
    public partial class Chunk03084006 : Chunk03084005
    {
        /// <inheritdoc />
        public override uint Id => 0x03084006;

        public bool U04;

        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.Boolean(ref U04);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockCameraGame 0x007 chunk
    /// </summary>
    [Chunk(0x03084007)]
    public partial class Chunk03084007 : Chunk<CGameCtnMediaBlockCameraGame>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03084007;

        public int Version { get; set; }

        /// <summary>
        /// always 10
        /// </summary>
        public float U01;
        /// <summary>
        /// depth? 0 or 0.02
        /// </summary>
        public float U02;
        public bool U03;
        public bool U04;
        public bool U05;
        public float U06;
        public int U07;

        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            if (Version <= 1)
            {
                rw.Id(ref n.gameCamId);
            }
            if (Version >= 2)
            {
                rw.EnumInt32<EGameCam>(ref n.gameCam);
            }
            rw.Int32(ref n.clipEntId);
            rw.Vec3(ref n.camPosition);
            rw.Vec3(ref n.camPitchYawRoll);
            rw.Single(ref n.camFov);
            rw.Single(ref U01); // always 10
            rw.Single(ref U02); // depth? 0 or 0.02
            rw.Single(ref n.camNearClipPlane);
            rw.Single(ref n.camFarClipPlane);
            rw.Boolean(ref U03);
            rw.Boolean(ref U04);
            rw.Boolean(ref U05);
            if (Version >= 1)
            {
                rw.Single(ref U06);
                if (Version >= 3)
                {
                    rw.Int32(ref U07);
                }
            }
        }
    }



    public enum EGameCamOld
    {
        Behind,
        Close,
        Internal,
        Orbital,
    }

    public enum EGameCam
    {
        Default,
        Internal,
        External,
        Helico,
        Free,
        Spectator,
        External_2,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03084000 => new Chunk03084000(),
        0x03084001 => new Chunk03084001(),
        0x03084003 => new Chunk03084003(),
        0x03084004 => new Chunk03084004(),
        0x03084005 => new Chunk03084005(),
        0x03084006 => new Chunk03084006(),
        0x03084007 => new Chunk03084007(),
        _ => base.NewChunk(chunkId),
    };
}
