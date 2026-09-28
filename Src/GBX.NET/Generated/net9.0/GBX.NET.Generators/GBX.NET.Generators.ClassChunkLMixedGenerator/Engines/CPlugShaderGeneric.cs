namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09004000</remarks>
[Class(0x09004000)]
public partial class CPlugShaderGeneric : CPlugShader, IClass
{
    [Hexadecimal] public static new uint Id => 0x09004000;




    /// <summary>
    /// Creates a new instance of <see cref="CPlugShaderGeneric"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugShaderGeneric() { }


    /// <summary>
    /// CPlugShaderGeneric 0x001 chunk
    /// </summary>
    [Chunk(0x09004001)]
    public partial class Chunk09004001 : Chunk<CPlugShaderGeneric>
    {
        /// <inheritdoc />
        public override uint Id => 0x09004001;

        public byte[]? U01;

        public override void ReadWrite(CPlugShaderGeneric n, GbxReaderWriter rw)
        {
            rw.Data(ref U01!, 88);
        }
    }

    /// <summary>
    /// CPlugShaderGeneric 0x002 chunk
    /// </summary>
    [Chunk(0x09004002)]
    public partial class Chunk09004002 : Chunk<CPlugShaderGeneric>
    {
        /// <inheritdoc />
        public override uint Id => 0x09004002;

        public byte[]? U01;

        public override void ReadWrite(CPlugShaderGeneric n, GbxReaderWriter rw)
        {
            rw.Data(ref U01!, 88);
        }
    }

    /// <summary>
    /// CPlugShaderGeneric 0x003 chunk
    /// </summary>
    [Chunk(0x09004003)]
    public partial class Chunk09004003 : Chunk<CPlugShaderGeneric>
    {
        /// <inheritdoc />
        public override uint Id => 0x09004003;

        public byte[]? U01;

        public override void ReadWrite(CPlugShaderGeneric n, GbxReaderWriter rw)
        {
            rw.Data(ref U01!, 88);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09004001 => new Chunk09004001(),
        0x09004002 => new Chunk09004002(),
        0x09004003 => new Chunk09004003(),
        _ => base.NewChunk(chunkId),
    };
}
