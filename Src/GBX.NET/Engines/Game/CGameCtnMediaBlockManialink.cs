namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Manialink (0x0312A000)
/// </summary>
[Node(0x0312A000)]
public sealed class CGameCtnMediaBlockManialink : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasTwoKeys
{
    #region Fields

    private TimeSpan start;
    private TimeSpan end = TimeSpan.FromSeconds(3);
    private string manialinkURL;

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

    [NodeMember]
    public string ManialinkURL
    {
        get => manialinkURL;
        set => manialinkURL = value;
    }

    #endregion

    #region Constructors

    private CGameCtnMediaBlockManialink()
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
            rw.Single_s(ref n.start);
            rw.Single_s(ref n.end);
            rw.String(ref n.manialinkURL!);
        }
    }

    #endregion

    #endregion
}
