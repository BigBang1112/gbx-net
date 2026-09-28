namespace GBX.NET.Engines.Hms;

/// <remarks>ID: 0x06026000</remarks>
[Class(0x06026000)]
public partial class CHmsAmbientOcc : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x06026000;




    /// <summary>
    /// Creates a new instance of <see cref="CHmsAmbientOcc"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CHmsAmbientOcc() { }


    /// <summary>
    /// CHmsAmbientOcc 0x000 chunk
    /// </summary>
    [Chunk(0x06026000)]
    public partial class Chunk06026000 : Chunk<CHmsAmbientOcc>
    {
        /// <inheritdoc />
        public override uint Id => 0x06026000;

        public float U01;
        public float U02;
        public int U03;
        public float U04;
        public float U05;
        public float U06;

        public override void ReadWrite(CHmsAmbientOcc n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Int32(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x06026000 => new Chunk06026000(),
        _ => base.NewChunk(chunkId),
    };
}
