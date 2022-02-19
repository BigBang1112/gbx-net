namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Motion blur effect (0x03082000)
/// </summary>
[Node(0x03082000)]
[NodeExtension("GameCtnMediaBlockFxBlurMotion")]
public class CGameCtnMediaBlockFxBlurMotion : CGameCtnMediaBlockFxBlur, CGameCtnMediaBlock.IHasTwoKeys
{
    #region Fields

    public TimeSingle start;
    public TimeSingle end = TimeSingle.FromSeconds(3);

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

    protected CGameCtnMediaBlockFxBlurMotion()
    {

    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    [Chunk(0x03082000)]
    public class Chunk03082000 : Chunk<CGameCtnMediaBlockFxBlurMotion>
    {
        public override void ReadWrite(CGameCtnMediaBlockFxBlurMotion n, GameBoxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
        }
    }

    #endregion

    #endregion
}
