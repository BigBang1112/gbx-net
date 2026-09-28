namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090F5000</remarks>
[Class(0x090F5000)]
public partial class CPlugFxHdrScales_Tech3 : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090F5000;




    /// <summary>
    /// Creates a new instance of <see cref="CPlugFxHdrScales_Tech3"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugFxHdrScales_Tech3() { }


    /// <summary>
    /// CPlugFxHdrScales_Tech3 0x000 chunk
    /// </summary>
    [Chunk(0x090F5000)]
    public partial class Chunk090F5000 : Chunk<CPlugFxHdrScales_Tech3>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090F5000;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public float U04;

        public override void ReadWrite(CPlugFxHdrScales_Tech3 n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            if (Version >= 1)
            {
                rw.Single(ref U02);
            }
            rw.Single(ref U03);
            rw.Single(ref U04);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090F5000 => new Chunk090F5000(),
        _ => base.NewChunk(chunkId),
    };
}
