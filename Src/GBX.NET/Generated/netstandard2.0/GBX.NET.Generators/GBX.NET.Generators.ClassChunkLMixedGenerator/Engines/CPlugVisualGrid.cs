namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0903B000</remarks>
[Class(0x0903B000)]
public partial class CPlugVisualGrid : CPlugVisual3D, IClass
{
    [Hexadecimal] public static new uint Id => 0x0903B000;




    private int nbPointX;
    [AppliedWithChunk<Chunk0903B000>]
    public int NbPointX { get => nbPointX; set => nbPointX = value; }

    private int nbPointZ;
    [AppliedWithChunk<Chunk0903B000>]
    public int NbPointZ { get => nbPointZ; set => nbPointZ = value; }

    private float rangeX;
    [AppliedWithChunk<Chunk0903B000>]
    public float RangeX { get => rangeX; set => rangeX = value; }

    private float rangeZ;
    [AppliedWithChunk<Chunk0903B000>]
    public float RangeZ { get => rangeZ; set => rangeZ = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugVisualGrid"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugVisualGrid() { }


    /// <summary>
    /// CPlugVisualGrid 0x000 chunk
    /// </summary>
    [Chunk(0x0903B000)]
    public partial class Chunk0903B000 : Chunk<CPlugVisualGrid>
    {
        /// <inheritdoc />
        public override uint Id => 0x0903B000;


        public override void ReadWrite(CPlugVisualGrid n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.nbPointX);
            rw.Int32(ref n.nbPointZ);
            rw.Single(ref n.rangeX);
            rw.Single(ref n.rangeZ);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0903B000 => new Chunk0903B000(),
        _ => base.NewChunk(chunkId),
    };
}
