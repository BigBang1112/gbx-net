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

    [NodeMember(ExactlyNamed = true)]
    public CGameCtnBlockInfoFlat? BlockInfoFlat
    {
        get => blockInfoFlat = GetNodeFromRefTable(blockInfoFlat, blockInfoFlatFile) as CGameCtnBlockInfoFlat;
        set => blockInfoFlat = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public CGameCtnBlockInfoClip? BlockInfoClip
    {
        get => blockInfoClip = GetNodeFromRefTable(blockInfoClip, blockInfoClipFile) as CGameCtnBlockInfoClip;
        set => blockInfoClip = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public CGameCtnBlockInfoRoad? BlockInfoRoad
    {
        get => blockInfoRoad = GetNodeFromRefTable(blockInfoRoad, blockInfoRoadFile) as CGameCtnBlockInfoRoad;
        set => blockInfoRoad = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public CGameCtnBlockInfoPylon? BlockInfoPylon
    {
        get => blockInfoPylon = GetNodeFromRefTable(blockInfoPylon, blockInfoPylonFile) as CGameCtnBlockInfoPylon;
        set => blockInfoPylon = value;
    }

    protected CGameCtnZoneFlat()
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
}
