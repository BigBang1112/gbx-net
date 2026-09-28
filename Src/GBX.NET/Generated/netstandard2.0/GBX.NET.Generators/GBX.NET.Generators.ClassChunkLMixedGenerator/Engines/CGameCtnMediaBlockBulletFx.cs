namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03169000</remarks>
[Class(0x03169000)]
public partial class CGameCtnMediaBlockBulletFx : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasTwoKeys
{
    [Hexadecimal] public static new uint Id => 0x03169000;

    TimeSingle IHasTwoKeys.Start { get => Start; set => Start = value; }
    TimeSingle IHasTwoKeys.End { get => End; set => End = value; }



    private TimeSingle start;
    [AppliedWithChunk<Chunk03169000>]
    public TimeSingle Start { get => start; set => start = value; }

    private TimeSingle end;
    [AppliedWithChunk<Chunk03169000>]
    public TimeSingle End { get => end; set => end = value; }

    private CPlugDataTape? dataTape;
    [AppliedWithChunk<Chunk03169000>]
    public CPlugDataTape? DataTape { get => dataTape; set => dataTape = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockBulletFx"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockBulletFx() { }


    /// <summary>
    /// CGameCtnMediaBlockBulletFx 0x000 chunk
    /// </summary>
    [Chunk(0x03169000)]
    public partial class Chunk03169000 : Chunk<CGameCtnMediaBlockBulletFx>
    {
        /// <inheritdoc />
        public override uint Id => 0x03169000;


        public override void ReadWrite(CGameCtnMediaBlockBulletFx n, GbxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.NodeRef<CPlugDataTape>(ref n.dataTape);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03169000 => new Chunk03169000(),
        _ => base.NewChunk(chunkId),
    };
}
