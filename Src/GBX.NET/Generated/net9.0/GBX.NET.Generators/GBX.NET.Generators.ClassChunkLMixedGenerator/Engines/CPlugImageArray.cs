namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0914C000</remarks>
[Class(0x0914C000)]
public partial class CPlugImageArray : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0914C000;




    private string? folder;
    [AppliedWithChunk<Chunk0914C000>]
    public string? Folder { get => folder; set => folder = value; }

    private Elem[]? layers;
    [AppliedWithChunk<Chunk0914C000>]
    public Elem[]? Layers { get => layers; set => layers = value; }

    private CMwNod? material_VId;
    [AppliedWithChunk<Chunk0914C000>]
    public CMwNod? Material_VId { get => material_VId; set => material_VId = value; }

    private float maskScale;
    [AppliedWithChunk<Chunk0914C000>]
    public float MaskScale { get => maskScale; set => maskScale = value; }

    private string? folder_TextureDecals;
    [AppliedWithChunk<Chunk0914C000>]
    public string? Folder_TextureDecals { get => folder_TextureDecals; set => folder_TextureDecals = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugImageArray"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugImageArray() { }


    /// <summary>
    /// CPlugImageArray 0x000 chunk
    /// </summary>
    [Chunk(0x0914C000)]
    public partial class Chunk0914C000 : Chunk<CPlugImageArray>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0914C000;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CPlugImageArray n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref n.folder);
            rw.ArrayReadableWritable<Elem>(ref n.layers!, version: Version);
            if (Version >= 4)
            {
                rw.NodeRef<CMwNod>(ref n.material_VId);
                if (Version >= 5)
                {
                    rw.Single(ref n.maskScale);
                    if (Version >= 6)
                    {
                        rw.String(ref n.folder_TextureDecals);
                        if (Version >= 7)
                        {
                            rw.Int32(ref U01);
                        }
                    }
                }
            }
        }
    }


    public sealed partial class Elem : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

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

        private float u09;
        public float U09 { get => u09; set => u09 = value; }

        private float u10;
        public float U10 { get => u10; set => u10 = value; }

        private float u11;
        public float U11 { get => u11; set => u11 = value; }

        private float u12;
        public float U12 { get => u12; set => u12 = value; }

        private float u13;
        public float U13 { get => u13; set => u13 = value; }

        private float u14;
        public float U14 { get => u14; set => u14 = value; }

        private int u15;
        public int U15 { get => u15; set => u15 = value; }

        private int u16;
        public int U16 { get => u16; set => u16 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.String(ref u01);
            rw.Single(ref u02);
            rw.Single(ref u03);
            rw.Single(ref u04);
            rw.Single(ref u05);
            if (v >= 3)
            {
                rw.Single(ref u06);
                rw.Single(ref u07);
                rw.Single(ref u08);
                rw.Single(ref u09);
            }
            rw.Single(ref u10);
            if (v >= 1)
            {
                rw.Single(ref u11);
                rw.Single(ref u12);
                rw.Single(ref u13);
                rw.Single(ref u14);
                if (v >= 2)
                {
                    rw.Int32(ref u15);
                    rw.Int32(ref u16);
                }
            }
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0914C000 => new Chunk0914C000(),
        _ => base.NewChunk(chunkId),
    };
}
