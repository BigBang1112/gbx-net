namespace GBX.NET.Engines.Motion;

/// <remarks>ID: 0x08055000</remarks>
[Class(0x08055000)]
public partial class CMotionDayTime : CMotion, IClass
{
    [Hexadecimal] public static new uint Id => 0x08055000;




    /// <summary>
    /// Creates a new instance of <see cref="CMotionDayTime"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CMotionDayTime() { }


    /// <summary>
    /// CMotionDayTime 0x000 chunk
    /// </summary>
    [Chunk(0x08055000)]
    public partial class Chunk08055000 : Chunk<CMotionDayTime>
    {
        /// <inheritdoc />
        public override uint Id => 0x08055000;

        public int U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CMotionDayTime n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x08055000 => new Chunk08055000(),
        _ => base.NewChunk(chunkId),
    };
}
