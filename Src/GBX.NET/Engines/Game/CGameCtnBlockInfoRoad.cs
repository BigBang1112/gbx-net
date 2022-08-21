namespace GBX.NET.Engines.Game;

/// <summary>
/// CGameCtnBlockInfoRoad (0x03052000)
/// </summary>
[Node(0x03052000)]
[NodeExtension("TMEDRoad")]
[NodeExtension("EDRoad")]
public class CGameCtnBlockInfoRoad : CGameCtnBlockInfo
{
    private CGameCtnBlockInfoSlope? slope;
    private GameBoxRefTable.File? slopeFile;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03052000))]
    public CGameCtnBlockInfoSlope? Slope
    {
        get => slope = GetNodeFromRefTable(slope, slopeFile) as CGameCtnBlockInfoSlope;
        set => slope = value;
    }

    protected CGameCtnBlockInfoRoad()
    {

    }


    #region 0x000 chunk

    /// <summary>
    /// CGameCtnBlockInfoRoad 0x000 chunk
    /// </summary>
    [Chunk(0x03052000)]
    public class Chunk03052000 : Chunk<CGameCtnBlockInfoRoad>
    {
        public override void ReadWrite(CGameCtnBlockInfoRoad n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnBlockInfoSlope>(ref n.slope, ref n.slopeFile);
        }
    }

    #endregion
}
