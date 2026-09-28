namespace GBX.NET.Engines.Graphic;

/// <remarks>ID: 0x04003000</remarks>
[Class(0x04003000)]
public partial class GxLightPoint : GxLightNotAmbient, IClass
{
    [Hexadecimal] public static new uint Id => 0x04003000;




    private float flareSize;
    [AppliedWithChunk<Chunk04003003>]
    [AppliedWithChunk<Chunk04003004>]
    public float FlareSize { get => flareSize; set => flareSize = value; }

    private float flareBiasZ;
    [AppliedWithChunk<Chunk04003004>]
    public float FlareBiasZ { get => flareBiasZ; set => flareBiasZ = value; }

    /// <summary>
    /// Creates a new instance of <see cref="GxLightPoint"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public GxLightPoint() { }


    /// <summary>
    /// GxLightPoint 0x003 chunk
    /// </summary>
    [Chunk(0x04003003)]
    public partial class Chunk04003003 : Chunk<GxLightPoint>
    {
        /// <inheritdoc />
        public override uint Id => 0x04003003;


        public override void ReadWrite(GxLightPoint n, GbxReaderWriter rw)
        {
            rw.Single(ref n.flareSize);
        }
    }

    /// <summary>
    /// GxLightPoint 0x004 chunk
    /// </summary>
    [Chunk(0x04003004)]
    public partial class Chunk04003004 : Chunk<GxLightPoint>
    {
        /// <inheritdoc />
        public override uint Id => 0x04003004;


        public override void ReadWrite(GxLightPoint n, GbxReaderWriter rw)
        {
            rw.Single(ref n.flareSize);
            rw.Single(ref n.flareBiasZ);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x04003003 => new Chunk04003003(),
        0x04003004 => new Chunk04003004(),
        _ => base.NewChunk(chunkId),
    };
}
