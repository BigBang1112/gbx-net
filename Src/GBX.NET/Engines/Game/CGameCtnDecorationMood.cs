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
    private CHmsLightMap? hmsLightMap;
    private GameBoxRefTable.File? hmsLightMapFile;
    private CHmsAmbientOcc? hmsAmbientOcc;
    private GameBoxRefTable.File? hmsAmbientOccFile;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303A000>]
    public float Latitude { get => latitude; set => latitude = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303A000>]
    public float Longitude { get => longitude; set => longitude = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303A000>]
    public float DeltaGMT { get => deltaGMT; set => deltaGMT = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303A001>]
    public float RemappedStartDayTime { get => remappedStartDayTime; set => remappedStartDayTime = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303A001>]
    public CPlugGameSkin? Remapping { get => remapping; set => remapping = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303A001>]
    public string? RemapFolder { get => remapFolder; set => remapFolder = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303A002>]
    public int ShadowCountCarHuman { get => shadowCountCarHuman; set => shadowCountCarHuman = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303A002>]
    public int ShadowCountCarOpponent { get => shadowCountCarOpponent; set => shadowCountCarOpponent = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303A002>]
    public float ShadowCarIntensity { get => shadowCarIntensity; set => shadowCarIntensity = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303A002>]
    public bool ShadowScene { get => shadowScene; set => shadowScene = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303A002>]
    public bool BackgroundIsLocallyLighted { get => backgroundIsLocallyLighted; set => backgroundIsLocallyLighted = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303A004>]
    public CHmsLightMap? HmsLightMap
    {
        get => hmsLightMap = GetNodeFromRefTable(hmsLightMap, hmsLightMapFile) as CHmsLightMap;
        set => hmsLightMap = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303A005>]
    public CHmsAmbientOcc? HmsAmbientOcc
    {
        get => hmsAmbientOcc = GetNodeFromRefTable(hmsAmbientOcc, hmsAmbientOccFile) as CHmsAmbientOcc;
        set => hmsAmbientOcc = value;
    }

    internal CGameCtnDecorationMood()
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

    #region 0x004 chunk

    /// <summary>
    /// CGameCtnDecorationMood 0x004 chunk
    /// </summary>
    [Chunk(0x0303A004)]
    public class Chunk0303A004 : Chunk<CGameCtnDecorationMood>
    {
        public override void ReadWrite(CGameCtnDecorationMood n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CHmsLightMap>(ref n.hmsLightMap, ref n.hmsLightMapFile);
        }
    }

    #endregion

    #region 0x005 chunk

    /// <summary>
    /// CGameCtnDecorationMood 0x005 chunk
    /// </summary>
    [Chunk(0x0303A005)]
    public class Chunk0303A005 : Chunk<CGameCtnDecorationMood>
    {
        public override void ReadWrite(CGameCtnDecorationMood n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CHmsAmbientOcc>(ref n.hmsAmbientOcc, ref n.hmsAmbientOccFile);
        }
    }

    #endregion
}