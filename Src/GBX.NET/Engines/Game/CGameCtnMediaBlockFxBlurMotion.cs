namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Motion blur effect.
/// </summary>
/// <remarks>ID: 0x03082000</remarks>
[Node(0x03082000)]
[NodeExtension("GameCtnMediaBlockFxBlurMotion")]
public class CGameCtnMediaBlockFxBlurMotion : CGameCtnMediaBlockFxBlur, CGameCtnMediaBlock.IHasTwoKeys
{
    public TimeSingle start;
    public TimeSingle end = TimeSingle.FromSeconds(3);

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03082000))]
    public TimeSingle Start { get => start; set => start = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03082000))]
    public TimeSingle End { get => end; set => end = value; }

    protected CGameCtnMediaBlockFxBlurMotion()
    {

    }

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
}
