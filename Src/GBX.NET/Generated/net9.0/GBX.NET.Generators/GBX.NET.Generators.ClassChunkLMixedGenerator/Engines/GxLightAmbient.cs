namespace GBX.NET.Engines.Graphic;

/// <remarks>ID: 0x04005000</remarks>
[Class(0x04005000)]
public partial class GxLightAmbient : GxLight, IClass
{
    [Hexadecimal] public static new uint Id => 0x04005000;




    private float shadeMinY;
    [AppliedWithChunk<Chunk04005000>]
    public float ShadeMinY { get => shadeMinY; set => shadeMinY = value; }

    private float shadeMaxY;
    [AppliedWithChunk<Chunk04005000>]
    public float ShadeMaxY { get => shadeMaxY; set => shadeMaxY = value; }

    /// <summary>
    /// Creates a new instance of <see cref="GxLightAmbient"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public GxLightAmbient() { }


    /// <summary>
    /// GxLightAmbient 0x000 chunk
    /// </summary>
    [Chunk(0x04005000)]
    public partial class Chunk04005000 : Chunk<GxLightAmbient>
    {
        /// <inheritdoc />
        public override uint Id => 0x04005000;


        public override void ReadWrite(GxLightAmbient n, GbxReaderWriter rw)
        {
            rw.Single(ref n.shadeMinY);
            rw.Single(ref n.shadeMaxY);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x04005000 => new Chunk04005000(),
        _ => base.NewChunk(chunkId),
    };
}
