namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Manialink.
/// </summary>
/// <remarks>ID: 0x0312A000</remarks>
[Node(0x0312A000)]
[NodeExtension("GameCtnMediaBlockManialink")]
public class CGameCtnMediaBlockManialink : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasTwoKeys
{
    private TimeSingle start;
    private TimeSingle end = TimeSingle.FromSeconds(3);
    private string manialinkURL;

    [NodeMember]
    [AppliedWithChunk<Chunk0312A001>]
    public TimeSingle Start { get => start; set => start = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0312A001>]
    public TimeSingle End { get => end; set => end = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0312A001>]
    public string ManialinkURL { get => manialinkURL; set => manialinkURL = value; }

    internal CGameCtnMediaBlockManialink()
    {
        manialinkURL = "";
    }

    #region Chunks

    // 0x000 chunk

    #region 0x001 chunk

    [Chunk(0x0312A001)]
    public class Chunk0312A001 : Chunk<CGameCtnMediaBlockManialink>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

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
