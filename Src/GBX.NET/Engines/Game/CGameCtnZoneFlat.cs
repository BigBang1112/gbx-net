namespace GBX.NET.Engines.Game;

[Node(0x0305D000)]
public class CGameCtnZoneFlat : CGameCtnZone
{
    private CGameCtnBlockInfoFlat? blockInfoFlat;
    private int? blockInfoFlatIndex;
    private CGameCtnBlockInfoClip? blockInfoClip;
    private int? blockInfoClipIndex;
    private CGameCtnBlockInfoRoad? blockInfoRoad;
    private int? blockInfoRoadIndex;
    private CGameCtnBlockInfoPylon? blockInfoPylon;
    private int? blockInfoPylonIndex;

    [NodeMember(ExactlyNamed = true)]
    public CGameCtnBlockInfoFlat? BlockInfoFlat
    {
        get => blockInfoFlat = GetNodeFromRefTable(blockInfoFlat, blockInfoFlatIndex) as CGameCtnBlockInfoFlat;
        set => blockInfoFlat = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public CGameCtnBlockInfoClip? BlockInfoClip
    {
        get => blockInfoClip = GetNodeFromRefTable(blockInfoClip, blockInfoClipIndex) as CGameCtnBlockInfoClip;
        set => blockInfoClip = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public CGameCtnBlockInfoRoad? BlockInfoRoad
    {
        get => blockInfoRoad = GetNodeFromRefTable(blockInfoRoad, blockInfoRoadIndex) as CGameCtnBlockInfoRoad;
        set => blockInfoRoad = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public CGameCtnBlockInfoPylon? BlockInfoPylon
    {
        get => blockInfoPylon = GetNodeFromRefTable(blockInfoPylon, blockInfoPylonIndex) as CGameCtnBlockInfoPylon;
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
            rw.NodeRef<CGameCtnBlockInfoFlat>(ref n.blockInfoFlat, ref n.blockInfoFlatIndex);
            rw.NodeRef<CGameCtnBlockInfoClip>(ref n.blockInfoClip, ref n.blockInfoClipIndex);
            rw.NodeRef<CGameCtnBlockInfoRoad>(ref n.blockInfoRoad, ref n.blockInfoRoadIndex);
            rw.NodeRef<CGameCtnBlockInfoPylon>(ref n.blockInfoPylon, ref n.blockInfoPylonIndex);
        }
    }

    #endregion
}
