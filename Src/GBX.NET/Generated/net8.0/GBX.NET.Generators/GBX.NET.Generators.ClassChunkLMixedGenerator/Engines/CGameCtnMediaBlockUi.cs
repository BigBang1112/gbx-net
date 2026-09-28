namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0307D000</remarks>
[Class(0x0307D000)]
public partial class CGameCtnMediaBlockUi : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasTwoKeys
{
    [Hexadecimal] public static new uint Id => 0x0307D000;

    TimeSingle IHasTwoKeys.Start { get => Start; set => Start = value; }
    TimeSingle IHasTwoKeys.End { get => End; set => End = value; }



    private CControlContainer? userInterface;
    [AppliedWithChunk<Chunk0307D000>]
    public CControlContainer? UserInterface { get => userInterface; set => userInterface = value; }

    private TimeSingle start;
    [AppliedWithChunk<Chunk0307D000>]
    [AppliedWithChunk<Chunk0307D001>]
    public TimeSingle Start { get => start; set => start = value; }

    private TimeSingle end;
    [AppliedWithChunk<Chunk0307D000>]
    [AppliedWithChunk<Chunk0307D001>]
    public TimeSingle End { get => end; set => end = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockUi"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockUi() { }


    /// <summary>
    /// CGameCtnMediaBlockUi 0x000 chunk
    /// </summary>
    [Chunk(0x0307D000)]
    public partial class Chunk0307D000 : Chunk<CGameCtnMediaBlockUi>
    {
        /// <inheritdoc />
        public override uint Id => 0x0307D000;


        public override void ReadWrite(CGameCtnMediaBlockUi n, GbxReaderWriter rw)
        {
            rw.NodeRef<CControlContainer>(ref n.userInterface);
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockUi 0x001 chunk
    /// </summary>
    [Chunk(0x0307D001)]
    public partial class Chunk0307D001 : Chunk<CGameCtnMediaBlockUi>
    {
        /// <inheritdoc />
        public override uint Id => 0x0307D001;


        public override void ReadWrite(CGameCtnMediaBlockUi n, GbxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0307D000 => new Chunk0307D000(),
        0x0307D001 => new Chunk0307D001(),
        _ => base.NewChunk(chunkId),
    };
}
