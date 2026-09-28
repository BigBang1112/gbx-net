namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0904A000</remarks>
[Class(0x0904A000)]
public partial class CPlugVisual2D : CPlugVisual, IClass
{
    [Hexadecimal] public static new uint Id => 0x0904A000;




    private Vertex2D[]? vertices2D;
    [AppliedWithChunk<Chunk0904A000>]
    public Vertex2D[]? Vertices2D { get => vertices2D; set => vertices2D = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugVisual2D"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugVisual2D() { }


    /// <summary>
    /// CPlugVisual2D 0x000 chunk
    /// </summary>
    [Chunk(0x0904A000)]
    public partial class Chunk0904A000 : Chunk<CPlugVisual2D>
    {
        /// <inheritdoc />
        public override uint Id => 0x0904A000;


        public override void ReadWrite(CPlugVisual2D n, GbxReaderWriter rw)
        {
            rw.ArrayReadableWritable<Vertex2D>(ref n.vertices2D!);
        }
    }


    public sealed partial class Vertex2D : IReadableWritable
    {

        private float u01;
        public float U01 { get => u01; set => u01 = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        private float u05;
        public float U05 { get => u05; set => u05 = value; }

        private float u06;
        public float U06 { get => u06; set => u06 = value; }

        private float u07;
        public float U07 { get => u07; set => u07 = value; }

        private float u08;
        public float U08 { get => u08; set => u08 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Single(ref u01);
            rw.Single(ref u02);
            rw.Single(ref u03);
            rw.Single(ref u04);
            rw.Single(ref u05);
            rw.Single(ref u06);
            rw.Single(ref u07);
            rw.Single(ref u08);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0904A000 => new Chunk0904A000(),
        _ => base.NewChunk(chunkId),
    };
}
