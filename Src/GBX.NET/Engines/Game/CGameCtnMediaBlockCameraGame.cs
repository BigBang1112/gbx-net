using GBX.NET.Builders.Engines.Game;

namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Camera ingame.
/// </summary>
/// <remarks>ID: 0x03084000</remarks>
[Node(0x03084000)]
public class CGameCtnMediaBlockCameraGame : CGameCtnMediaBlockCamera, CGameCtnMediaBlock.IHasTwoKeys
{
    #region Enums

    public enum EGameCam : int
    {
        Behind,
        Close,
        Internal,
        Orbital
    }

    public enum EGameCam2 : int
    {
        Default,
        Internal,
        External,
        Helico,
        Free,
        Spectator,
        External_2
    }

    #endregion

    #region Fields

    public TimeSingle start;
    public TimeSingle end = TimeSingle.FromSeconds(3);
    public EGameCam? gameCam1;
    public EGameCam2? gameCam2;
    public int clipEntId = -1;
    public string? gameCam;
    private Vec3? camPosition;
    private Vec3? camPitchYawRoll;
    private float? camFov;
    private float? camNearClipPlane;
    private float? camFarClipPlane;

    #endregion

    #region Properties

    [NodeMember]
    [AppliedWithChunk<Chunk03084000>]
    [AppliedWithChunk<Chunk03084001>]
    [AppliedWithChunk<Chunk03084003>]
    [AppliedWithChunk<Chunk03084004>]
    [AppliedWithChunk<Chunk03084005>]
    [AppliedWithChunk<Chunk03084006>]
    [AppliedWithChunk<Chunk03084007>]
    public TimeSingle Start { get => start; set => start = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk03084000>]
    [AppliedWithChunk<Chunk03084001>]
    [AppliedWithChunk<Chunk03084003>]
    [AppliedWithChunk<Chunk03084004>]
    [AppliedWithChunk<Chunk03084005>]
    [AppliedWithChunk<Chunk03084006>]
    [AppliedWithChunk<Chunk03084007>]
    public TimeSingle End { get => end; set => end = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk03084000>]
    [AppliedWithChunk<Chunk03084001>]
    public EGameCam? GameCam1 { get => gameCam1; set => gameCam1 = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk03084007>(sinceVersion: 2)]
    public EGameCam2? GameCam2 { get => gameCam2; set => gameCam2 = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk03084001>]
    [AppliedWithChunk<Chunk03084003>]
    [AppliedWithChunk<Chunk03084004>]
    [AppliedWithChunk<Chunk03084005>]
    [AppliedWithChunk<Chunk03084006>]
    [AppliedWithChunk<Chunk03084007>]
    public int ClipEntId { get => clipEntId; set => clipEntId = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk03084003>]
    [AppliedWithChunk<Chunk03084004>]
    [AppliedWithChunk<Chunk03084005>]
    [AppliedWithChunk<Chunk03084006>]
    [AppliedWithChunk<Chunk03084007>(sinceVersion: 0, upToVersion: 1)]
    public string? GameCam { get => gameCam; set => gameCam = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk03084004>]
    [AppliedWithChunk<Chunk03084005>]
    [AppliedWithChunk<Chunk03084006>]
    [AppliedWithChunk<Chunk03084007>]
    public Vec3? CamPosition { get => camPosition; set => camPosition = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk03084004>]
    [AppliedWithChunk<Chunk03084005>]
    [AppliedWithChunk<Chunk03084006>]
    [AppliedWithChunk<Chunk03084007>]
    public Vec3? CamPitchYawRoll { get => camPitchYawRoll; set => camPitchYawRoll = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk03084004>]
    [AppliedWithChunk<Chunk03084005>]
    [AppliedWithChunk<Chunk03084006>]
    [AppliedWithChunk<Chunk03084007>]
    public float? CamFov { get => camFov; set => camFov = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk03084004>]
    [AppliedWithChunk<Chunk03084005>]
    [AppliedWithChunk<Chunk03084006>]
    [AppliedWithChunk<Chunk03084007>]
    public float? CamNearClipPlane { get => camNearClipPlane; set => camNearClipPlane = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk03084004>]
    [AppliedWithChunk<Chunk03084005>]
    [AppliedWithChunk<Chunk03084006>]
    [AppliedWithChunk<Chunk03084007>]
    public float? CamFarClipPlane { get => camFarClipPlane; set => camFarClipPlane = value; }

    #endregion

    #region Constructors

    internal CGameCtnMediaBlockCameraGame()
    {

    }

    public static CGameCtnMediaBlockCameraGameBuilder Create() => new();

    #endregion

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraGame 0x000 chunk
    /// </summary>
    [Chunk(0x03084000)]
    public class Chunk03084000 : Chunk<CGameCtnMediaBlockCameraGame>
    {
        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GameBoxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.EnumInt32<EGameCam>(ref n.gameCam1);
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraGame 0x001 chunk
    /// </summary>
    [Chunk(0x03084001)]
    public class Chunk03084001 : Chunk03084000
    {
        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GameBoxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.Int32(ref n.clipEntId);
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraGame 0x003 chunk
    /// </summary>
    [Chunk(0x03084003)]
    public class Chunk03084003 : Chunk<CGameCtnMediaBlockCameraGame>
    {
        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GameBoxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.Id(ref n.gameCam);
            rw.Int32(ref n.clipEntId);
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraGame 0x004 chunk
    /// </summary>
    [Chunk(0x03084004)]
    public class Chunk03084004 : Chunk03084003
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GameBoxReaderWriter rw)
        {
            base.ReadWrite(n, rw);

            // GmCamFreeVal
            // // GmLocFreeVal
            rw.Vec3(ref n.camPosition);
            rw.Vec3(ref n.camPitchYawRoll);
            // // GmLensVal
            rw.Single(ref n.camFov, defaultValue: 90);
            rw.Single(ref U01); // always 10
            rw.Single(ref U02); // depth? 0 or 0.02
            rw.Single(ref n.camNearClipPlane, defaultValue: -1);
            rw.Single(ref n.camFarClipPlane, defaultValue: -1);
        }
    }

    #endregion

    #region 0x005 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraGame 0x005 chunk
    /// </summary>
    [Chunk(0x03084005)]
    public class Chunk03084005 : Chunk03084004
    {
        public bool U03;

        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GameBoxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.Boolean(ref U03);
        }
    }

    #endregion

    #region 0x006 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraGame 0x005 chunk
    /// </summary>
    [Chunk(0x03084006)]
    public class Chunk03084006 : Chunk03084005
    {
        public bool U04;

        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GameBoxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.Boolean(ref U04);
        }
    }

    #endregion

    #region 0x007 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraGame 0x007 chunk
    /// </summary>
    [Chunk(0x03084007)]
    public class Chunk03084007 : Chunk<CGameCtnMediaBlockCameraGame>, IVersionable
    {
        private int version;

        public float U01 = 10;
        public float U02 = 0; // depth? 0 or 0.02
        public bool U03;
        public bool U04;
        public bool U05;
        public float U06;
        public int U07;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);

            if (version < 2)
            {
                rw.Id(ref n.gameCam);
            }

            if (version >= 2)
            {
                rw.EnumInt32<EGameCam2>(ref n.gameCam2);
            }

            rw.Int32(ref n.clipEntId);

            // GmCamFreeVal
            // // GmLocFreeVal
            rw.Vec3(ref n.camPosition);
            rw.Vec3(ref n.camPitchYawRoll);
            // // GmLensVal
            rw.Single(ref n.camFov, defaultValue: 90);
            rw.Single(ref U01); // always 10
            rw.Single(ref U02); // depth? 0 or 0.02
            rw.Single(ref n.camNearClipPlane, defaultValue: -1);
            rw.Single(ref n.camFarClipPlane, defaultValue: -1);

            rw.Boolean(ref U03);
            rw.Boolean(ref U04);
            rw.Boolean(ref U05);

            if (version >= 1)
            {
                rw.Single(ref U06);

                if (version >= 3)
                {
                    rw.Int32(ref U07);
                }
            }
        }
    }

    #endregion

    #endregion
}
