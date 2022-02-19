namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Shoot (0x03145000)
/// </summary>
/// <remarks>Better known as "Editing cut".</remarks>
[Node(0x03145000)]
public class CGameCtnMediaBlockShoot : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasTwoKeys
{
    #region Fields

    private TimeSingle start;
    private TimeSingle end = TimeSingle.FromSeconds(3);

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
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
        }
    }

    #endregion

    #endregion
}
