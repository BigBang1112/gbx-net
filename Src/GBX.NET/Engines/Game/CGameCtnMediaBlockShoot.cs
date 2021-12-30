namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Shoot (0x03145000)
/// </summary>
/// <remarks>Better known as "Editing cut".</remarks>
[Node(0x03145000)]
public class CGameCtnMediaBlockShoot : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasTwoKeys
{
    #region Fields

    private TimeSpan start;
    private TimeSpan end = TimeSpan.FromSeconds(3);

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

    #endregion

    #region Constructors

    protected CGameCtnMediaBlockShoot()
    {

    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    [Chunk(0x03145000)]
    public class Chunk03145000 : Chunk<CGameCtnMediaBlockShoot>
    {
        public override void ReadWrite(CGameCtnMediaBlockShoot n, GameBoxReaderWriter rw)
        {
            rw.Single_s(ref n.start);
            rw.Single_s(ref n.end);
        }
    }

    #endregion

    #endregion
}
