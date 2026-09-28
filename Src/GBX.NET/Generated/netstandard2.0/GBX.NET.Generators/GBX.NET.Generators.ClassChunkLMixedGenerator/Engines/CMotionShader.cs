namespace GBX.NET.Engines.Motion;

/// <remarks>ID: 0x0802B000</remarks>
[Class(0x0802B000)]
public partial class CMotionShader : CMotionTrack, IClass
{
    [Hexadecimal] public static new uint Id => 0x0802B000;




    /// <summary>
    /// Creates a new instance of <see cref="CMotionShader"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CMotionShader() { }


    /// <summary>
    /// CMotionShader 0x000 chunk
    /// </summary>
    [Chunk(0x0802B000)]
    public partial class Chunk0802B000 : Chunk<CMotionShader>
    {
        /// <inheritdoc />
        public override uint Id => 0x0802B000;

        public int U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CMotionShader n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0802B000 => new Chunk0802B000(),
        _ => base.NewChunk(chunkId),
    };
}
