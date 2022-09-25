namespace GBX.NET.Engines.Game;

[Node(0x0305E000)]
public class CGameCtnZoneFrontier : CGameCtnZone
{
    private CGameCtnBlockInfoFrontier? blockInfoFrontier;
    private string? parentZoneId;
    private string? childZoneId;

    internal CGameCtnZoneFrontier()
    {
        
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0305E001))]
    public CGameCtnBlockInfoFrontier? BlockInfoFrontier { get => blockInfoFrontier; set => blockInfoFrontier = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0305E001))]
    public string? ParentZoneId { get => parentZoneId; set => parentZoneId = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0305E001))]
    public string? ChildZoneId { get => childZoneId; set => childZoneId = value; }

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnZoneFrontier 0x001 chunk
    /// </summary>
    [Chunk(0x0305E001)]
    public class Chunk0305E001 : Chunk<CGameCtnZoneFrontier>
    {
        public override void ReadWrite(CGameCtnZoneFrontier n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnBlockInfoFrontier>(ref n.blockInfoFrontier);
            rw.Id(ref n.parentZoneId);
            rw.Id(ref n.childZoneId);
        }
    }

    #endregion
}
