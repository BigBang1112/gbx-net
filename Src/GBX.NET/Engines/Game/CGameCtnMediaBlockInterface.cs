namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Interface.
/// </summary>
/// <remarks>ID: 0x03195000</remarks>
[Node(0x03195000)]
[NodeExtension("GameCtnMediaBlockInterface")]
public class CGameCtnMediaBlockInterface : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasTwoKeys
{
    private TimeSingle start;
    private TimeSingle end = TimeSingle.FromSeconds(3);
    private bool showInterface;
    private string manialink;

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03195000))]
    public TimeSingle Start { get => start; set => start = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03195000))]
    public TimeSingle End { get => end; set => end = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03195000))]
    public bool ShowInterface { get => showInterface; set => showInterface = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03195000))]
    public string Manialink { get => manialink; set => manialink = value; }

    protected CGameCtnMediaBlockInterface()
    {
        manialink = "";
    }

    #region Chunks

    #region 0x000 chunk

    [Chunk(0x03195000)]
    public class Chunk03195000 : Chunk<CGameCtnMediaBlockInterface>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnMediaBlockInterface n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.Boolean(ref n.showInterface);
            rw.String(ref n.manialink!);
        }
    }

    #endregion

    #endregion
}
