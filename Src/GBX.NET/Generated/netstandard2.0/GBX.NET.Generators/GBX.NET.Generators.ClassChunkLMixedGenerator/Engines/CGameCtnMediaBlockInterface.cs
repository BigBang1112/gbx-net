namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03195000</remarks>
[Class(0x03195000)]
public partial class CGameCtnMediaBlockInterface : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasTwoKeys
{
    [Hexadecimal] public static new uint Id => 0x03195000;

    TimeSingle IHasTwoKeys.Start { get => Start; set => Start = value; }
    TimeSingle IHasTwoKeys.End { get => End; set => End = value; }



    private TimeSingle start;
    [AppliedWithChunk<Chunk03195000>]
    public TimeSingle Start { get => start; set => start = value; }

    private TimeSingle end;
    [AppliedWithChunk<Chunk03195000>]
    public TimeSingle End { get => end; set => end = value; }

    private bool showInterface;
    [AppliedWithChunk<Chunk03195000>]
    public bool ShowInterface { get => showInterface; set => showInterface = value; }

    private string? manialink;
    [AppliedWithChunk<Chunk03195000>]
    public string? Manialink { get => manialink; set => manialink = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockInterface"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockInterface() { }


    /// <summary>
    /// CGameCtnMediaBlockInterface 0x000 chunk
    /// </summary>
    [Chunk(0x03195000)]
    public partial class Chunk03195000 : Chunk<CGameCtnMediaBlockInterface>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03195000;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnMediaBlockInterface n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.Boolean(ref n.showInterface);
            rw.String(ref n.manialink);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03195000 => new Chunk03195000(),
        _ => base.NewChunk(chunkId),
    };
}
