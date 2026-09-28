namespace GBX.NET.Engines.Motion;

/// <remarks>ID: 0x08001000</remarks>
[Class(0x08001000)]
public abstract partial class CMotion : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x08001000;




    /// <summary>
    /// Creates a new instance of <see cref="CMotion"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CMotion() { }


    /// <summary>
    /// CMotion 0x000 chunk
    /// </summary>
    [Chunk(0x08001000)]
    public partial class Chunk08001000 : Chunk<CMotion>
    {
        /// <inheritdoc />
        public override uint Id => 0x08001000;

        public string? U01;

        public override void ReadWrite(CMotion n, GbxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x08001000 => new Chunk08001000(),
        _ => base.NewChunk(chunkId),
    };
}
