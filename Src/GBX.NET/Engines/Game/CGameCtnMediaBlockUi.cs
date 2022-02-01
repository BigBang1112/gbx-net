namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - UI (0x0307D000)
/// </summary>
[Node(0x0307D000)]
[NodeExtension("GameCtnMediaBlockUi")]
public class CGameCtnMediaBlockUi : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasTwoKeys
{
    #region Fields

    private TimeSpan start;
    private TimeSpan end = TimeSpan.FromSeconds(3);
    private CControlList? userInterface;

    #endregion

    #region Properties

    [NodeMember]
    public TimeSpan Start
    {
        get => start;
        set => start = value;
    }

    [NodeMember]
    public TimeSpan End
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
            rw.Single_s(ref n.start);
            rw.Single_s(ref n.end);
        }
    }

    #endregion

    #region 0x001 chunk

    [Chunk(0x0307D001)]
    public class Chunk0307D001 : Chunk<CGameCtnMediaBlockUi>
    {
        public override void ReadWrite(CGameCtnMediaBlockUi n, GameBoxReaderWriter rw)
        {
            rw.Single_s(ref n.start);
            rw.Single_s(ref n.end);
        }
    }

    #endregion

    #endregion
}
