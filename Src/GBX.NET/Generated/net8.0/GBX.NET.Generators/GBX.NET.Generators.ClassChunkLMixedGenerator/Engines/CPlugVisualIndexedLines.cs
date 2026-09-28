namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09009000</remarks>
[Class(0x09009000)]
public partial class CPlugVisualIndexedLines : CPlugVisualIndexed, IClass
{
    [Hexadecimal] public static new uint Id => 0x09009000;




    /// <summary>
    /// Creates a new instance of <see cref="CPlugVisualIndexedLines"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugVisualIndexedLines() { }


    /// <summary>
    /// CPlugVisualIndexedLines 0x001 chunk
    /// </summary>
    [Chunk(0x09009001)]
    public partial class Chunk09009001 : Chunk<CPlugVisualIndexedLines>
    {
        /// <inheritdoc />
        public override uint Id => 0x09009001;

        public int U01;

        public override void ReadWrite(CPlugVisualIndexedLines n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09009001 => new Chunk09009001(),
        _ => base.NewChunk(chunkId),
    };
}
