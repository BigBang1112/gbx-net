namespace GBX.NET.Engines.Game;

[Node(0x0305C000)]
public class CGameCtnZone : CMwNod
{
    private string? zoneId;
    private string? surfaceId;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0305C003>]
    public string? ZoneId { get => zoneId; set => zoneId = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0305C003>]
    public string? SurfaceId { get => surfaceId; set => surfaceId = value; }

    internal CGameCtnZone()
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

    #region 0x004 skippable chunk

    /// <summary>
    /// CGameCtnZone 0x004 skippable chunk
    /// </summary>
    [Chunk(0x0305C004), IgnoreChunk]
    public class Chunk0305C004 : SkippableChunk<CGameCtnZone>
    {
        
    }

    #endregion

    #region 0x005 chunk

    /// <summary>
    /// CGameCtnZone 0x005 chunk
    /// </summary>
    [Chunk(0x0305C005)]
    public class Chunk0305C005 : Chunk<CGameCtnZone>
    {
        public bool U01;

        public override void ReadWrite(CGameCtnZone n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    #endregion

    #region 0x006 chunk

    /// <summary>
    /// CGameCtnZone 0x006 chunk
    /// </summary>
    [Chunk(0x0305C006)]
    public class Chunk0305C006 : Chunk<CGameCtnZone>
    {
        public bool U01;
        
        public override void ReadWrite(CGameCtnZone n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    #endregion
}