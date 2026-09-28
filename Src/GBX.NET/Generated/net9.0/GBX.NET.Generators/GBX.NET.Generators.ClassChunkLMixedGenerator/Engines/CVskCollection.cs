namespace GBX.NET.Engines.VirtualSkipper;

/// <remarks>ID: 0x21085000</remarks>
[Class(0x21085000)]
public partial class CVskCollection : CGameCtnCollection, IClass
{
    [Hexadecimal] public static new uint Id => 0x21085000;




    /// <summary>
    /// Creates a new instance of <see cref="CVskCollection"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CVskCollection() { }


    /// <summary>
    /// CVskCollection 0x000 chunk
    /// </summary>
    [Chunk(0x21085000)]
    public partial class Chunk21085000 : Chunk<CVskCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x21085000;

        public CMwNod? U01;
        public CMwNod? U02;

        public override void ReadWrite(CVskCollection n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref U01);
            rw.NodeRef<CMwNod>(ref U02);
        }
    }

    /// <summary>
    /// CVskCollection 0x001 chunk
    /// </summary>
    [Chunk(0x21085001)]
    public partial class Chunk21085001 : Chunk<CVskCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x21085001;

        public float U01;
        public float U02;
        public float U03;
        public float U04;

        public override void ReadWrite(CVskCollection n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x21085000 => new Chunk21085000(),
        0x21085001 => new Chunk21085001(),
        _ => base.NewChunk(chunkId),
    };
}
