namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09051000</remarks>
[Class(0x09051000)]
public partial class CPlugTreeGenerator : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x09051000;




    /// <summary>
    /// Creates a new instance of <see cref="CPlugTreeGenerator"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugTreeGenerator() { }


    /// <summary>
    /// CPlugTreeGenerator 0x000 chunk
    /// </summary>
    [Chunk(0x09051000)]
    public partial class Chunk09051000 : Chunk<CPlugTreeGenerator>
    {
        /// <inheritdoc />
        public override uint Id => 0x09051000;

        public uint U01;

        public override void ReadWrite(CPlugTreeGenerator n, GbxReaderWriter rw)
        {
            rw.UInt32(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09051000 => new Chunk09051000(),
        _ => base.NewChunk(chunkId),
    };
}
