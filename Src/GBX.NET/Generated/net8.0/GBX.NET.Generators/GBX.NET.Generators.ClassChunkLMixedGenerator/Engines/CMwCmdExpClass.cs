namespace GBX.NET.Engines.MwFoundations;

/// <remarks>ID: 0x01056000</remarks>
[Class(0x01056000)]
public partial class CMwCmdExpClass : CMwCmdExp, IClass
{
    [Hexadecimal] public static new uint Id => 0x01056000;




    /// <summary>
    /// Creates a new instance of <see cref="CMwCmdExpClass"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CMwCmdExpClass() { }


    /// <summary>
    /// CMwCmdExpClass 0x000 chunk
    /// </summary>
    [Chunk(0x01056000)]
    public partial class Chunk01056000 : Chunk<CMwCmdExpClass>
    {
        /// <inheritdoc />
        public override uint Id => 0x01056000;

        public int U01;

        public override void ReadWrite(CMwCmdExpClass n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x01056000 => new Chunk01056000(),
        _ => base.NewChunk(chunkId),
    };
}
