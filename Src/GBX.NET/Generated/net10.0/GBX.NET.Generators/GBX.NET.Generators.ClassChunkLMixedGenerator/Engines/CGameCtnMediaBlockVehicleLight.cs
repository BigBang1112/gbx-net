namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03133000</remarks>
[Class(0x03133000)]
public partial class CGameCtnMediaBlockVehicleLight : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasTwoKeys
{
    [Hexadecimal] public static new uint Id => 0x03133000;

    TimeSingle IHasTwoKeys.Start { get => Start; set => Start = value; }
    TimeSingle IHasTwoKeys.End { get => End; set => End = value; }



    private TimeSingle start;
    [AppliedWithChunk<Chunk03133000>]
    public TimeSingle Start { get => start; set => start = value; }

    private TimeSingle end;
    [AppliedWithChunk<Chunk03133000>]
    public TimeSingle End { get => end; set => end = value; }

    private int target;
    [AppliedWithChunk<Chunk03133001>]
    public int Target { get => target; set => target = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockVehicleLight"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockVehicleLight() { }


    /// <summary>
    /// CGameCtnMediaBlockVehicleLight 0x000 chunk
    /// </summary>
    [Chunk(0x03133000)]
    public partial class Chunk03133000 : Chunk<CGameCtnMediaBlockVehicleLight>
    {
        /// <inheritdoc />
        public override uint Id => 0x03133000;


        public override void ReadWrite(CGameCtnMediaBlockVehicleLight n, GbxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockVehicleLight 0x001 chunk (target)
    /// </summary>
    [Chunk(0x03133001, "target")]
    public partial class Chunk03133001 : Chunk<CGameCtnMediaBlockVehicleLight>
    {
        /// <inheritdoc />
        public override uint Id => 0x03133001;


        public override void ReadWrite(CGameCtnMediaBlockVehicleLight n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.target);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03133000 => new Chunk03133000(),
        0x03133001 => new Chunk03133001(),
        _ => base.NewChunk(chunkId),
    };
}
