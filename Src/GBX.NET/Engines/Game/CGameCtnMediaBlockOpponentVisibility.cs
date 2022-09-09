namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Opponent visibility.
/// </summary>
/// <remarks>ID: 0x0338B000</remarks>
[Node(0x0338B000)]
public partial class CGameCtnMediaBlockOpponentVisibility : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasTwoKeys
{
    private TimeSingle start;
    private TimeSingle end = TimeSingle.FromSeconds(3);
    private EVisibility visibility;

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0338B000))]
    public TimeSingle Start { get => start; set => start = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0338B000))]
    public TimeSingle End { get => end; set => end = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0338B001))]
    public EVisibility Visibility { get => visibility; set => visibility = value; }

    protected CGameCtnMediaBlockOpponentVisibility()
    {

    }

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockOpponentVisibility 0x000 chunk
    /// </summary>
    [Chunk(0x0338B000)]
    public class Chunk0338B000 : Chunk<CGameCtnMediaBlockOpponentVisibility>
    {
        public override void ReadWrite(CGameCtnMediaBlockOpponentVisibility n, GameBoxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnMediaBlockOpponentVisibility 0x001 chunk
    /// </summary>
    [Chunk(0x0338B001)]
    public class Chunk0338B001 : Chunk<CGameCtnMediaBlockOpponentVisibility>
    {
        public override void ReadWrite(CGameCtnMediaBlockOpponentVisibility n, GameBoxReaderWriter rw)
        {
            rw.EnumInt32<EVisibility>(ref n.visibility);
        }
    }

    #endregion

    #endregion
}
