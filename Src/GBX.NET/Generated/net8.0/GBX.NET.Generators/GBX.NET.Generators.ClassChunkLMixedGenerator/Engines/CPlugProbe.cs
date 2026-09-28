namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09106000</remarks>
[Class(0x09106000)]
public partial class CPlugProbe : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x09106000;




    /// <summary>
    /// Creates a new instance of <see cref="CPlugProbe"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugProbe() { }


    /// <summary>
    /// CPlugProbe 0x000 chunk
    /// </summary>
    [Chunk(0x09106000)]
    public partial class Chunk09106000 : Chunk<CPlugProbe>
    {
        /// <inheritdoc />
        public override uint Id => 0x09106000;

        public int U01;
        public Vec3 U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;

        public override void ReadWrite(CPlugProbe n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Vec3(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09106000 => new Chunk09106000(),
        _ => base.NewChunk(chunkId),
    };
}
