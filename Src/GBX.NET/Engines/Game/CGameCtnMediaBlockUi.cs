namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - UI.
/// </summary>
/// <remarks>ID: 0x0307D000</remarks>
[Node(0x0307D000)]
[NodeExtension("GameCtnMediaBlockUi")]
public class CGameCtnMediaBlockUi : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasTwoKeys
{
    private TimeSingle start;
    private TimeSingle end = TimeSingle.FromSeconds(3);
    private CControlList? userInterface;

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0307D000))]
    [AppliedWithChunk(typeof(Chunk0307D001))]
    public TimeSingle Start { get => start; set => start = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0307D000))]
    [AppliedWithChunk(typeof(Chunk0307D001))]
    public TimeSingle End { get => end; set => end = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0307D000))]
    public CControlList? UserInterface { get => userInterface; set => userInterface = value; }

    internal CGameCtnMediaBlockUi()
    {

    }

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockUi 0x000 chunk
    /// </summary>
    [Chunk(0x0307D000)]
    public class Chunk0307D000 : Chunk<CGameCtnMediaBlockUi>
    {
        public override void ReadWrite(CGameCtnMediaBlockUi n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CControlList>(ref n.userInterface);
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnMediaBlockUi 0x001 chunk
    /// </summary>
    [Chunk(0x0307D001)]
    public class Chunk0307D001 : Chunk<CGameCtnMediaBlockUi>
    {
        public override void ReadWrite(CGameCtnMediaBlockUi n, GameBoxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
        }
    }

    #endregion

    #endregion
}
