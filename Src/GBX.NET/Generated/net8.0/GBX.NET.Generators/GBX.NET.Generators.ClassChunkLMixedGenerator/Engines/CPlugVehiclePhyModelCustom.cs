namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0911E000</remarks>
[Class(0x0911E000)]
public partial class CPlugVehiclePhyModelCustom : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0911E000;




    private float accelCoef;
    [AppliedWithChunk<Chunk0911E000>]
    public float AccelCoef { get => accelCoef; set => accelCoef = value; }

    private float controlCoef;
    [AppliedWithChunk<Chunk0911E000>]
    public float ControlCoef { get => controlCoef; set => controlCoef = value; }

    private float gravityCoef;
    [AppliedWithChunk<Chunk0911E000>]
    public float GravityCoef { get => gravityCoef; set => gravityCoef = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugVehiclePhyModelCustom"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugVehiclePhyModelCustom() { }


    /// <summary>
    /// CPlugVehiclePhyModelCustom 0x000 chunk
    /// </summary>
    [Chunk(0x0911E000)]
    public partial class Chunk0911E000 : Chunk<CPlugVehiclePhyModelCustom>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0911E000;

        public int Version { get; set; }


        public override void ReadWrite(CPlugVehiclePhyModelCustom n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref n.accelCoef);
            rw.Single(ref n.controlCoef);
            rw.Single(ref n.gravityCoef);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0911E000 => new Chunk0911E000(),
        _ => base.NewChunk(chunkId),
    };
}
