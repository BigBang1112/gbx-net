namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090EC000</remarks>
[Class(0x090EC000)]
public partial class CPlugVehiclePhyTunings : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090EC000;




    private CPlugVehiclePhyTuning[]? tuning;
    [AppliedWithChunk<Chunk090EC000>]
    public CPlugVehiclePhyTuning[]? Tuning { get => tuning; set => tuning = value; }

    private int tuningIndex;
    [AppliedWithChunk<Chunk090EC000>]
    public int TuningIndex { get => tuningIndex; set => tuningIndex = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugVehiclePhyTunings"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugVehiclePhyTunings() { }


    /// <summary>
    /// CPlugVehiclePhyTunings 0x000 chunk
    /// </summary>
    [Chunk(0x090EC000)]
    public partial class Chunk090EC000 : Chunk<CPlugVehiclePhyTunings>
    {
        /// <inheritdoc />
        public override uint Id => 0x090EC000;


        public override void ReadWrite(CPlugVehiclePhyTunings n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CPlugVehiclePhyTuning>(ref n.tuning!);
            rw.Int32(ref n.tuningIndex);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090EC000 => new Chunk090EC000(),
        _ => base.NewChunk(chunkId),
    };
}
