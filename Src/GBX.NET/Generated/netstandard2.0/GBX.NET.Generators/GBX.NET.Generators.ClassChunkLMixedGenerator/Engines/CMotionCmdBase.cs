namespace GBX.NET.Engines.Motion;

/// <remarks>ID: 0x08029000</remarks>
[Class(0x08029000)]
public partial class CMotionCmdBase : CMwCmd, IClass
{
    [Hexadecimal] public static new uint Id => 0x08029000;




    /// <summary>
    /// Creates a new instance of <see cref="CMotionCmdBase"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CMotionCmdBase() { }


    /// <summary>
    /// CMotionCmdBase 0x002 chunk
    /// </summary>
    [Chunk(0x08029002)]
    public partial class Chunk08029002 : Chunk<CMotionCmdBase>
    {
        /// <inheritdoc />
        public override uint Id => 0x08029002;

        public int U01;
        public float U02;
        public int U03;
        public int U04;
        public int U05;
        public int U06;

        public override void ReadWrite(CMotionCmdBase n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Single(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x08029002 => new Chunk08029002(),
        _ => base.NewChunk(chunkId),
    };
}
