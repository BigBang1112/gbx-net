namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - UI (0x0307D000)
/// </summary>
[Node(0x0307D000)]
[NodeExtension("GameCtnMediaBlockUi")]
public class CGameCtnMediaBlockUi : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasTwoKeys
{
    #region Fields

    private TimeSingle start;
    private TimeSingle end = TimeSingle.FromSeconds(3);
    private CControlList? userInterface;

    #endregion

    #region Properties

    [NodeMember]
    public TimeSingle Start
    {
        get => start;
        set => start = value;
    }

    [NodeMember]
    public TimeSingle End
    {
        get => end;
        set => end = value;
    }

    public CControlList? UserInterface { get => userInterface; set => userInterface = value; }

    #endregion

    #region Constructors

    protected CGameCtnMediaBlockUi()
    {

    }

    #endregion

    #region Chunks

    #region 0x000 chunk

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
