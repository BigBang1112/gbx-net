namespace GBX.NET.Engines.Hms;

/// <remarks>ID: 0x06023000</remarks>
[Class(0x06023000)]
public partial class CHmsLightMapMood : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x06023000;




    /// <summary>
    /// Creates a new instance of <see cref="CHmsLightMapMood"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CHmsLightMapMood() { }


    /// <summary>
    /// CHmsLightMapMood 0x000 chunk
    /// </summary>
    [Chunk(0x06023000)]
    public partial class Chunk06023000 : Chunk<CHmsLightMapMood>
    {
        /// <inheritdoc />
        public override uint Id => 0x06023000;

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;

        public override void ReadWrite(CHmsLightMapMood n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x06023000 => new Chunk06023000(),
        _ => base.NewChunk(chunkId),
    };
}
