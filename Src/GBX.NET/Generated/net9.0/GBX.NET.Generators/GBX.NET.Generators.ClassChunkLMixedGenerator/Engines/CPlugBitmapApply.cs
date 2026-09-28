namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09012000</remarks>
[Class(0x09012000)]
public partial class CPlugBitmapApply : CPlugBitmapAddress, IClass
{
    [Hexadecimal] public static new uint Id => 0x09012000;




    /// <summary>
    /// Creates a new instance of <see cref="CPlugBitmapApply"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugBitmapApply() { }


    /// <summary>
    /// CPlugBitmapApply 0x003 chunk
    /// </summary>
    [Chunk(0x09012003)]
    public partial class Chunk09012003 : Chunk<CPlugBitmapApply>
    {
        /// <inheritdoc />
        public override uint Id => 0x09012003;

        public int U01;
        public int U02;

        public override void ReadWrite(CPlugBitmapApply n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    /// <summary>
    /// CPlugBitmapApply 0x004 chunk
    /// </summary>
    [Chunk(0x09012004)]
    public partial class Chunk09012004 : Chunk<CPlugBitmapApply>
    {
        /// <inheritdoc />
        public override uint Id => 0x09012004;

        public uint U01;

        public override void ReadWrite(CPlugBitmapApply n, GbxReaderWriter rw)
        {
            rw.UInt32(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09012003 => new Chunk09012003(),
        0x09012004 => new Chunk09012004(),
        _ => base.NewChunk(chunkId),
    };
}
