namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0316A000</remarks>
[Class(0x0316A000)]
public partial class CGameCtnMediaBlockCharVis : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasTwoKeys
{
    [Hexadecimal] public static new uint Id => 0x0316A000;

    TimeSingle IHasTwoKeys.Start { get => Start; set => Start = value; }
    TimeSingle IHasTwoKeys.End { get => End; set => End = value; }



    private TimeSingle start;
    [AppliedWithChunk<Chunk0316A000>]
    public TimeSingle Start { get => start; set => start = value; }

    private TimeSingle end;
    [AppliedWithChunk<Chunk0316A000>]
    public TimeSingle End { get => end; set => end = value; }

    private CPlugDataTape? dataTape;
    [AppliedWithChunk<Chunk0316A000>]
    public CPlugDataTape? DataTape { get => dataTape; set => dataTape = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockCharVis"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockCharVis() { }


    /// <summary>
    /// CGameCtnMediaBlockCharVis 0x000 chunk
    /// </summary>
    [Chunk(0x0316A000)]
    public partial class Chunk0316A000 : Chunk<CGameCtnMediaBlockCharVis>
    {
        /// <inheritdoc />
        public override uint Id => 0x0316A000;


        public override void ReadWrite(CGameCtnMediaBlockCharVis n, GbxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.NodeRef<CPlugDataTape>(ref n.dataTape);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0316A000 => new Chunk0316A000(),
        _ => base.NewChunk(chunkId),
    };
}
