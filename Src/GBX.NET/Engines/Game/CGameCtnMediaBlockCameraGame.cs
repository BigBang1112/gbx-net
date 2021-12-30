namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Camera ingame (0x03084000)
/// </summary>
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

    public TimeSpan start;
    public TimeSpan end = TimeSpan.FromSeconds(3);
    public EGameCam? gameCam1;
    public EGameCam2? gameCam2;
    public int clipEntId = -1;
    public string? gameCam;

    #endregion

    #region Properties

    [NodeMember]
    public TimeSpan Start
    {
        get => start;
        set => start = value;
    }

    [NodeMember]
    public TimeSpan End
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
            rw.Single_s(ref n.start);
            rw.Single_s(ref n.end);
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
            rw.Single_s(ref n.start);
            rw.Single_s(ref n.end);
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
            rw.Single_s(ref n.start);
            rw.Single_s(ref n.end);
            rw.Id(ref n.gameCam);
            rw.Int32(ref n.clipEntId);
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
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GameBoxReaderWriter rw)
        {
            rw.Single_s(ref n.start);
            rw.Single_s(ref n.end);
            rw.Id(ref n.gameCam);

            rw.UntilFacade(Unknown); // Helicopter camera transform? 17 ints, sometimes 19
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

        public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Single_s(ref n.start);
            rw.Single_s(ref n.end);
            rw.EnumInt32<EGameCam2>(ref n.gameCam2);

            rw.UntilFacade(Unknown); // Helicopter camera transform? 17 ints, sometimes 19
        }
    }

    #endregion

    #endregion
}
