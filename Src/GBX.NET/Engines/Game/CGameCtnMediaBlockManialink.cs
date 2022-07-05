namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Manialink.
/// </summary>
/// <remarks>ID: 0x0312A000</remarks>
[Node(0x0312A000)]
[NodeExtension("GameCtnMediaBlockManialink")]
public class CGameCtnMediaBlockManialink : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasTwoKeys
{
    #region Fields

    private TimeSingle start;
    private TimeSingle end = TimeSingle.FromSeconds(3);
    private string manialinkURL;

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

    [NodeMember]
    public string ManialinkURL
    {
        get => manialinkURL;
        set => manialinkURL = value;
    }

    #endregion

    #region Constructors

    protected CGameCtnMediaBlockManialink()
    {
        manialinkURL = null!;
    }

    #endregion

    #region Chunks

    // 0x000 chunk

    #region 0x001 chunk

    [Chunk(0x0312A001)]
    public class Chunk0312A001 : Chunk<CGameCtnMediaBlockManialink>, IVersionable
    {
        private int version;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnMediaBlockManialink n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.String(ref n.manialinkURL!);
        }
    }

    #endregion

    #endregion
}
