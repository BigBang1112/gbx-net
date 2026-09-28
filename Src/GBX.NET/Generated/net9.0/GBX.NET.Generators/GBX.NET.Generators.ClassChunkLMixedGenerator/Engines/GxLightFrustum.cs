namespace GBX.NET.Engines.Graphic;

/// <remarks>ID: 0x0400A000</remarks>
[Class(0x0400A000)]
public partial class GxLightFrustum : GxLightBall, IClass
{
    [Hexadecimal] public static new uint Id => 0x0400A000;




    /// <summary>
    /// Creates a new instance of <see cref="GxLightFrustum"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public GxLightFrustum() { }


    /// <summary>
    /// GxLightFrustum 0x004 chunk
    /// </summary>
    [Chunk(0x0400A004)]
    public partial class Chunk0400A004 : Chunk<GxLightFrustum>
    {
        /// <inheritdoc />
        public override uint Id => 0x0400A004;

        public int U01;
        public int U02;
        public int U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public int U08;

        public override void ReadWrite(GxLightFrustum n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Int32(ref U08);
        }
    }

    /// <summary>
    /// GxLightFrustum 0x006 chunk
    /// </summary>
    [Chunk(0x0400A006)]
    public partial class Chunk0400A006 : Chunk<GxLightFrustum>
    {
        /// <inheritdoc />
        public override uint Id => 0x0400A006;

        public bool U01;
        public BoxAligned U02;
        public uint U03;

        public override void ReadWrite(GxLightFrustum n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.BoxAligned(ref U02);
            rw.UInt32(ref U03);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0400A004 => new Chunk0400A004(),
        0x0400A006 => new Chunk0400A006(),
        _ => base.NewChunk(chunkId),
    };
}
