namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Shoot (better known as "Editing cut").
/// </summary>
/// <remarks>ID: 0x03145000</remarks>
[Node(0x03145000)]
public class CGameCtnMediaBlockShoot : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasTwoKeys
{
    private TimeSingle start;
    private TimeSingle end = TimeSingle.FromSeconds(3);

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03145000))]
    public TimeSingle Start { get => start; set => start = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03145000))]
    public TimeSingle End { get => end; set => end = value; }

    internal CGameCtnMediaBlockShoot()
    {

    }

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockShoot 0x000 chunk
    /// </summary>
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
}
