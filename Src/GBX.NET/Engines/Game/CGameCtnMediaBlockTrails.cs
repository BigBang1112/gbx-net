namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Trails.
/// </summary>
/// <remarks>ID: 0x030A9000</remarks>
[Node(0x030A9000)]
[NodeExtension("GameCtnMediaBlockTrails")]
public class CGameCtnMediaBlockTrails : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasTwoKeys
{
    private TimeSingle start = TimeSingle.Zero;
    private TimeSingle end = TimeSingle.FromSeconds(3);

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk030A9000))]
    public TimeSingle Start { get => start; set => start = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk030A9000))]
    public TimeSingle End { get => end; set => end = value; }

    protected CGameCtnMediaBlockTrails()
    {

    }

    #region 0x000 chunk

    [Chunk(0x030A9000)]
    public class Chunk030A9000 : Chunk<CGameCtnMediaBlockTrails>
    {
        public override void ReadWrite(CGameCtnMediaBlockTrails n, GameBoxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
        }
    }

    #endregion
}
