namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09182000</remarks>
[Class(0x09182000)]
public partial class CPlugCloudsParam : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x09182000;




    private Vec2[]? points;
    [AppliedWithChunk<Chunk09182001>]
    public Vec2[]? Points { get => points; set => points = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugCloudsParam"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugCloudsParam() { }


    /// <summary>
    /// CPlugCloudsParam 0x001 chunk
    /// </summary>
    [Chunk(0x09182001)]
    public partial class Chunk09182001 : Chunk<CPlugCloudsParam>
    {
        /// <inheritdoc />
        public override uint Id => 0x09182001;


        public override void ReadWrite(CPlugCloudsParam n, GbxReaderWriter rw)
        {
            rw.Array<Vec2>(ref n.points!);
        }
    }

    /// <summary>
    /// CPlugCloudsParam 0x002 chunk
    /// </summary>
    [Chunk(0x09182002)]
    public partial class Chunk09182002 : Chunk<CPlugCloudsParam>
    {
        /// <inheritdoc />
        public override uint Id => 0x09182002;

        public int U01;
        public int U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public int U08;
        public int U09;

        public override void ReadWrite(CPlugCloudsParam n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Int32(ref U08);
            rw.Int32(ref U09);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09182001 => new Chunk09182001(),
        0x09182002 => new Chunk09182002(),
        _ => base.NewChunk(chunkId),
    };
}
