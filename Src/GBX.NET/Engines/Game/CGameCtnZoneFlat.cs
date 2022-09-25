namespace GBX.NET.Engines.Game;

[Node(0x0305D000)]
public class CGameCtnZoneFlat : CGameCtnZone
{
    private CGameCtnBlockInfoFlat? blockInfoFlat;
    private GameBoxRefTable.File? blockInfoFlatFile;
    private CGameCtnBlockInfoClip? blockInfoClip;
    private GameBoxRefTable.File? blockInfoClipFile;
    private CGameCtnBlockInfoRoad? blockInfoRoad;
    private GameBoxRefTable.File? blockInfoRoadFile;
    private CGameCtnBlockInfoPylon? blockInfoPylon;
    private GameBoxRefTable.File? blockInfoPylonFile;
    private bool groundOnly;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0305D001))]
    public CGameCtnBlockInfoFlat? BlockInfoFlat
    {
        get => blockInfoFlat = GetNodeFromRefTable(blockInfoFlat, blockInfoFlatFile) as CGameCtnBlockInfoFlat;
        set => blockInfoFlat = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0305D001))]
    public CGameCtnBlockInfoClip? BlockInfoClip
    {
        get => blockInfoClip = GetNodeFromRefTable(blockInfoClip, blockInfoClipFile) as CGameCtnBlockInfoClip;
        set => blockInfoClip = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0305D001))]
    public CGameCtnBlockInfoRoad? BlockInfoRoad
    {
        get => blockInfoRoad = GetNodeFromRefTable(blockInfoRoad, blockInfoRoadFile) as CGameCtnBlockInfoRoad;
        set => blockInfoRoad = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0305D001))]
    public CGameCtnBlockInfoPylon? BlockInfoPylon
    {
        get => blockInfoPylon = GetNodeFromRefTable(blockInfoPylon, blockInfoPylonFile) as CGameCtnBlockInfoPylon;
        set => blockInfoPylon = value;
    }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0305D002))]
    public bool GroundOnly { get => groundOnly; set => groundOnly = value; }

    internal CGameCtnZoneFlat()
    {

    }
    
    #region 0x001 chunk

    /// <summary>
    /// CGameCtnZoneFlat 0x001 chunk
    /// </summary>
    [Chunk(0x0305D001)]
    public class Chunk0305D001 : Chunk<CGameCtnZoneFlat>
    {
        public override void ReadWrite(CGameCtnZoneFlat n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnBlockInfoFlat>(ref n.blockInfoFlat, ref n.blockInfoFlatFile);
            rw.NodeRef<CGameCtnBlockInfoClip>(ref n.blockInfoClip, ref n.blockInfoClipFile);
            rw.NodeRef<CGameCtnBlockInfoRoad>(ref n.blockInfoRoad, ref n.blockInfoRoadFile);
            rw.NodeRef<CGameCtnBlockInfoPylon>(ref n.blockInfoPylon, ref n.blockInfoPylonFile);
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnZoneFlat 0x002 chunk
    /// </summary>
    [Chunk(0x0305D002)]
    public class Chunk0305D002 : SkippableChunk<CGameCtnZoneFlat>
    {
        public int U01;
        public bool U02;

        public override void ReadWrite(CGameCtnZoneFlat n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Boolean(ref n.groundOnly);
            rw.Boolean(ref U02);
        }
    }

    #endregion
}
