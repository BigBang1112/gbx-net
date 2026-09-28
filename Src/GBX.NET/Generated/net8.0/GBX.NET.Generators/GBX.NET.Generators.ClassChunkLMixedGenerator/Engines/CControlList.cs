namespace GBX.NET.Engines.Control;

/// <remarks>ID: 0x0700F000</remarks>
[Class(0x0700F000)]
public partial class CControlList : CControlContainer, IClass
{
    [Hexadecimal] public static new uint Id => 0x0700F000;




    /// <summary>
    /// Creates a new instance of <see cref="CControlList"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CControlList() { }


    /// <summary>
    /// CControlList 0x007 chunk
    /// </summary>
    [Chunk(0x0700F007)]
    public partial class Chunk0700F007 : Chunk<CControlList>
    {
        /// <inheritdoc />
        public override uint Id => 0x0700F007;

        public int U01;
        public int U02;
        public int U03;
        public int U04;
        public int U05;
        public int U06;
        public int U07;
        public Unknown[]? U08;

        public override void ReadWrite(CControlList n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
            rw.Int32(ref U07);
            rw.ArrayReadableWritable<Unknown>(ref U08!);
        }
    }

    /// <summary>
    /// CControlList 0x00A chunk
    /// </summary>
    [Chunk(0x0700F00A)]
    public partial class Chunk0700F00A : Chunk<CControlList>
    {
        /// <inheritdoc />
        public override uint Id => 0x0700F00A;

        public float U01;
        public float U02;
        public float U03;
        public int U04;
        public bool U05;
        public string? U06;
        public CMwNod? U07;
        public float U08;
        public float U09;
        public bool U10;

        public override void ReadWrite(CControlList n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Int32(ref U04);
            rw.Boolean(ref U05);
            rw.Id(ref U06);
            if (U06==null||U06=="")
            {
                rw.NodeRef<CMwNod>(ref U07);
            }
            rw.Single(ref U08);
            rw.Single(ref U09);
            rw.Boolean(ref U10);
        }
    }


    public sealed partial class Unknown : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

        private CMwNod? u03;
        public CMwNod? U03 { get => u03; set => u03 = value; }

        private string? u04;
        public string? U04 { get => u04; set => u04 = value; }

        private bool u05;
        public bool U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.String(ref u01);
            rw.Single(ref u02);
            rw.NodeRef<CMwNod>(ref u03);
            rw.Id(ref u04);
            rw.Boolean(ref u05);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0700F007 => new Chunk0700F007(),
        0x0700F00A => new Chunk0700F00A(),
        _ => base.NewChunk(chunkId),
    };
}
