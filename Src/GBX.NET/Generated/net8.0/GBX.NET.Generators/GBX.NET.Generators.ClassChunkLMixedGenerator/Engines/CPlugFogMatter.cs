namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090D3000</remarks>
[Class(0x090D3000)]
public partial class CPlugFogMatter : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090D3000;




    /// <summary>
    /// Creates a new instance of <see cref="CPlugFogMatter"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugFogMatter() { }


    /// <summary>
    /// CPlugFogMatter 0x000 chunk
    /// </summary>
    [Chunk(0x090D3000)]
    public partial class Chunk090D3000 : Chunk<CPlugFogMatter>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090D3000;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public float U04;

        public override void ReadWrite(CPlugFogMatter n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090D3000 => new Chunk090D3000(),
        _ => base.NewChunk(chunkId),
    };
}
