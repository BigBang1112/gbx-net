namespace GBX.NET.Engines.Graphic;

/// <remarks>ID: 0x04004000</remarks>
[Class(0x04004000)]
public partial class GxFog : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x04004000;




    /// <summary>
    /// Creates a new instance of <see cref="GxFog"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public GxFog() { }


    /// <summary>
    /// GxFog 0x000 chunk
    /// </summary>
    [Chunk(0x04004000)]
    public partial class Chunk04004000 : Chunk<GxFog>
    {
        /// <inheritdoc />
        public override uint Id => 0x04004000;

        public bool U01;
        public int U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;
        public float U11;

        public override void ReadWrite(GxFog n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Int32(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);
            rw.Single(ref U10);
            rw.Single(ref U11);
        }
    }

    /// <summary>
    /// GxFog 0x001 chunk
    /// </summary>
    [Chunk(0x04004001)]
    public partial class Chunk04004001 : Chunk<GxFog>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x04004001;

        public int Version { get; set; }

        public Vec3 U01;

        public override void ReadWrite(GxFog n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Vec3(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x04004000 => new Chunk04004000(),
        0x04004001 => new Chunk04004001(),
        _ => base.NewChunk(chunkId),
    };
}
