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

    #endregion

    #region Properties

    [NodeMember]
    public TimeSingle Start
    {
        get => start;
        set => start = value;
    }

    [NodeMember]
    public TimeSingle End
    {
        get => end;
        set => end = value;
    }

    [NodeMember]
    public EGameCam? GameCam1
    {
        get => gameCam1;
        set => gameCam1 = value;
    }

    [NodeMember]
    public EGameCam2? GameCam2
    {
        get => gameCam2;
        set => gameCam2 = value;
    }

    [NodeMember]
    public int ClipEntId
    {
        get => clipEntId;
        set => clipEntId = value;
    }

    [NodeMember]
    public string? GameCam
    {
        get => gameCam;
        set => gameCam = value;
    }

    #endregion

    #region Constructors

    protected CGameCtnMediaBlockCameraGame()
    {

    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraGame 0x000 chunk
    /// </summary>
    [Chunk(0x03084000)]
    public class Chunk03084000 : Chunk<CGameCtnMediaBlockCameraGame>
    {
        public int U01;

        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GameBoxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraGame 0x001 chunk
    /// </summary>
    [Chunk(0x03084001)]
    public class Chunk03084001 : Chunk<CGameCtnMediaBlockCameraGame>
    {
        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GameBoxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.EnumInt32<EGameCam>(ref n.gameCam1);
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
    public class Chunk03084004 : Chunk<CGameCtnMediaBlockCameraGame>
    {
        public float[]? U01;
        public float[]? U02;

        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GameBoxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.Id(ref n.gameCam);
            rw.Int32(ref n.clipEntId);

            // GmCamFreeVal
            // // GmLocFreeVal
            rw.Array<float>(ref U01, count: 6);
            // // GmLensVal
            rw.Array<float>(ref U02, count: 5);
        }
    }

    #endregion

    #region 0x005 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraGame 0x005 chunk
    /// </summary>
    [Chunk(0x03084005)]
    public class Chunk03084005 : Chunk<CGameCtnMediaBlockCameraGame>
    {
        public float[]? U01;
        public float[]? U02;
        public bool U03;

        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GameBoxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.Id(ref n.gameCam);
            rw.Int32(ref n.clipEntId);

            // GmCamFreeVal
            // // GmLocFreeVal
            rw.Array<float>(ref U01, count: 6);
            // // GmLensVal
            rw.Array<float>(ref U02, count: 5);

            rw.Boolean(ref U03);
        }
    }

    #endregion

    #region 0x006 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraGame 0x005 chunk
    /// </summary>
    [Chunk(0x03084006)]
    public class Chunk03084006 : Chunk<CGameCtnMediaBlockCameraGame>
    {
        public float[]? U01;
        public float[]? U02;
        public bool U03;
        public bool U04;

        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GameBoxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.Id(ref n.gameCam);
            rw.Int32(ref n.clipEntId);

            // GmCamFreeVal
            // // GmLocFreeVal
            rw.Array<float>(ref U01, count: 6);
            // // GmLensVal
            rw.Array<float>(ref U02, count: 5);

            rw.Boolean(ref U03);
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

        public int Version
        {
            get => version;
            set => version = value;
        }

        public float[]? U01;
        public float[]? U02;
        public bool U03;
        public bool U04;
        public bool U05;
        public float U06;
        public int U07;

        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);

            if (version <= 1)
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
            rw.Array<float>(ref U01, count: 6);
            // // GmLensVal
            rw.Array<float>(ref U02, count: 5);

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
