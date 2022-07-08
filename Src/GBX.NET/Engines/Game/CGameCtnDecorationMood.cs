namespace GBX.NET.Engines.Game;

[Node(0x0303A000)]
public class CGameCtnDecorationMood : CMwNod
{
    private float latitude;
    private float longitude;
    private float deltaGMT;
    private float remappedStartDayTime;
    private CPlugGameSkin? remapping;
    private string? remapFolder;
    private int shadowCountCarHuman;
    private int shadowCountCarOpponent;
    private float shadowCarIntensity;
    private bool shadowScene;
    private bool backgroundIsLocallyLighted;

    [NodeMember(ExactlyNamed = true)]
    public float Latitude { get => latitude; set => latitude = value; }

    [NodeMember(ExactlyNamed = true)]
    public float Longitude { get => longitude; set => longitude = value; }

    [NodeMember(ExactlyNamed = true)]
    public float DeltaGMT { get => deltaGMT; set => deltaGMT = value; }

    [NodeMember(ExactlyNamed = true)]
    public float RemappedStartDayTime { get => remappedStartDayTime; set => remappedStartDayTime = value; }

    [NodeMember(ExactlyNamed = true)]
    public CPlugGameSkin? Remapping { get => remapping; set => remapping = value; }

    [NodeMember(ExactlyNamed = true)]
    public string? RemapFolder { get => remapFolder; set => remapFolder = value; }

    [NodeMember(ExactlyNamed = true)]
    public int ShadowCountCarHuman { get => shadowCountCarHuman; set => shadowCountCarHuman = value; }

    [NodeMember(ExactlyNamed = true)]
    public int ShadowCountCarOpponent { get => shadowCountCarOpponent; set => shadowCountCarOpponent = value; }

    [NodeMember(ExactlyNamed = true)]
    public float ShadowCarIntensity { get => shadowCarIntensity; set => shadowCarIntensity = value; }

    [NodeMember(ExactlyNamed = true)]
    public bool ShadowScene { get => shadowScene; set => shadowScene = value; }

    [NodeMember(ExactlyNamed = true)]
    public bool BackgroundIsLocallyLighted { get => backgroundIsLocallyLighted; set => backgroundIsLocallyLighted = value; }

    protected CGameCtnDecorationMood()
    {

    }

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnDecorationMood 0x000 chunk
    /// </summary>
    [Chunk(0x0303A000)]
    public class Chunk0303A000 : Chunk<CGameCtnDecorationMood>
    {
        public int U01;
        public int U02;

        public override void ReadWrite(CGameCtnDecorationMood n, GameBoxReaderWriter rw)
        {
            rw.Single(ref n.latitude);
            rw.Single(ref n.longitude);
            rw.Single(ref n.deltaGMT);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnDecorationMood 0x001 chunk
    /// </summary>
    [Chunk(0x0303A001)]
    public class Chunk0303A001 : Chunk<CGameCtnDecorationMood>
    {
        public override void ReadWrite(CGameCtnDecorationMood n, GameBoxReaderWriter rw)
        {
            rw.Single(ref n.remappedStartDayTime);
            rw.NodeRef<CPlugGameSkin>(ref n.remapping);
            rw.String(ref n.remapFolder);
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnDecorationMood 0x002 chunk
    /// </summary>
    [Chunk(0x0303A002)]
    public class Chunk0303A002 : Chunk<CGameCtnDecorationMood>
    {
        public override void ReadWrite(CGameCtnDecorationMood n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.shadowCountCarHuman);
            rw.Int32(ref n.shadowCountCarOpponent);
            rw.Single(ref n.shadowCarIntensity);
            rw.Boolean(ref n.shadowScene);
            rw.Boolean(ref n.backgroundIsLocallyLighted);
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CGameCtnDecorationMood 0x003 chunk
    /// </summary>
    [Chunk(0x0303A003)]
    public class Chunk0303A003 : Chunk<CGameCtnDecorationMood>
    {
        public bool U01;

        public override void ReadWrite(CGameCtnDecorationMood n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    #endregion
}