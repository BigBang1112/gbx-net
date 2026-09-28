namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090A8000</remarks>
[Class(0x090A8000)]
public partial class CPlugBitmapAtlas : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x090A8000;




    private SubTex[]? subTextures;
    [AppliedWithChunk<Chunk090A8002>]
    public SubTex[]? SubTextures { get => subTextures; set => subTextures = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugBitmapAtlas"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugBitmapAtlas() { }


    /// <summary>
    /// CPlugBitmapAtlas 0x002 chunk
    /// </summary>
    [Chunk(0x090A8002)]
    public partial class Chunk090A8002 : Chunk<CPlugBitmapAtlas>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090A8002;

        public int Version { get; set; }


        public override void ReadWrite(CPlugBitmapAtlas n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<SubTex>(ref n.subTextures!, version: Version);
        }
    }


    public sealed partial class SubTex : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        private int u05;
        public int U05 { get => u05; set => u05 = value; }

        private int u06;
        public int U06 { get => u06; set => u06 = value; }

        private int u07;
        public int U07 { get => u07; set => u07 = value; }

        private int u08;
        public int U08 { get => u08; set => u08 = value; }

        private float u09;
        public float U09 { get => u09; set => u09 = value; }

        private float u10;
        public float U10 { get => u10; set => u10 = value; }

        private float u11;
        public float U11 { get => u11; set => u11 = value; }

        private bool u12;
        public bool U12 { get => u12; set => u12 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref u01);
            rw.Id(ref u02);
            rw.Int32(ref u03);
            rw.Int32(ref u04);
            rw.Int32(ref u05);
            rw.Int32(ref u06);
            rw.Int32(ref u07);
            rw.Int32(ref u08);
            if (v >= 1)
            {
                rw.Single(ref u09);
                rw.Single(ref u10);
                if (v >= 2)
                {
                    rw.Single(ref u11);
                    if (v >= 3)
                    {
                        rw.Boolean(ref u12);
                    }
                }
            }
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090A8002 => new Chunk090A8002(),
        _ => base.NewChunk(chunkId),
    };
}
