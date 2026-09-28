namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x030A9000</remarks>
[Class(0x030A9000)]
public partial class CGameCtnMediaBlockTrails : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasTwoKeys
{
    [Hexadecimal] public static new uint Id => 0x030A9000;

    TimeSingle IHasTwoKeys.Start { get => Start; set => Start = value; }
    TimeSingle IHasTwoKeys.End { get => End; set => End = value; }



    private TimeSingle start;
    [AppliedWithChunk<Chunk030A9000>]
    public TimeSingle Start { get => start; set => start = value; }

    private TimeSingle end;
    [AppliedWithChunk<Chunk030A9000>]
    public TimeSingle End { get => end; set => end = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockTrails"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockTrails() { }


    /// <summary>
    /// CGameCtnMediaBlockTrails 0x000 chunk
    /// </summary>
    [Chunk(0x030A9000)]
    public partial class Chunk030A9000 : Chunk<CGameCtnMediaBlockTrails>
    {
        /// <inheritdoc />
        public override uint Id => 0x030A9000;


        public override void ReadWrite(CGameCtnMediaBlockTrails n, GbxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x030A9000 => new Chunk030A9000(),
        _ => base.NewChunk(chunkId),
    };
}
