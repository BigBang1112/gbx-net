namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x0501E000</remarks>
[Class(0x0501E000)]
public partial class CFuncTreeRotate : CFuncTree, IClass
{
    [Hexadecimal] public static new uint Id => 0x0501E000;




    private Vec3 axis;
    [AppliedWithChunk<Chunk0501E001>]
    public Vec3 Axis { get => axis; set => axis = value; }

    private float angleMin;
    [AppliedWithChunk<Chunk0501E001>]
    public float AngleMin { get => angleMin; set => angleMin = value; }

    private float angleMax;
    [AppliedWithChunk<Chunk0501E001>]
    public float AngleMax { get => angleMax; set => angleMax = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CFuncTreeRotate"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CFuncTreeRotate() { }


    /// <summary>
    /// CFuncTreeRotate 0x001 chunk
    /// </summary>
    [Chunk(0x0501E001)]
    [ChunkGameVersion(GameVersion.TM10)]
    public partial class Chunk0501E001 : Chunk<CFuncTreeRotate>
    {
        /// <inheritdoc />
        public override uint Id => 0x0501E001;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10;


        public override void ReadWrite(CFuncTreeRotate n, GbxReaderWriter rw)
        {
            rw.Vec3(ref n.axis);
            rw.Single(ref n.angleMin);
            rw.Single(ref n.angleMax);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0501E001 => new Chunk0501E001(),
        _ => base.NewChunk(chunkId),
    };
}
