namespace GBX.NET.Engines.Game;

[Node(0x0305C000)]
public class CGameCtnZone : CMwNod
{
    private string? zoneId;
    private string? surfaceId;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0305C003))]
    public string? ZoneId { get => zoneId; set => zoneId = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0305C003))]
    public string? SurfaceId { get => surfaceId; set => surfaceId = value; }

    protected CGameCtnZone()
    {
        
    }

    #region 0x003 chunk

    /// <summary>
    /// CGameCtnZone 0x003 chunk
    /// </summary>
    [Chunk(0x0305C003)]
    public class Chunk0305C003 : Chunk<CGameCtnZone>
    {
        public int U01;
        public int U02;

        public override void ReadWrite(CGameCtnZone n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Id(ref n.zoneId);
            rw.Id(ref n.surfaceId);
            rw.Int32(ref U02);
        }
    }

    #endregion
}