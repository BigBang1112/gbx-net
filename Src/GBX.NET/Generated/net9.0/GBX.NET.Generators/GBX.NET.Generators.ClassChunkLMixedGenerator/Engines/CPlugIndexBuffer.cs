namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09057000</remarks>
[Class(0x09057000)]
public partial class CPlugIndexBuffer : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x09057000;





    /// <summary>
    /// CPlugIndexBuffer 0x000 chunk
    /// </summary>
    [Chunk(0x09057000)]
    public partial class Chunk09057000 : Chunk<CPlugIndexBuffer>
    {
        /// <inheritdoc />
        public override uint Id => 0x09057000;

    }

    /// <summary>
    /// CPlugIndexBuffer 0x001 chunk
    /// </summary>
    [Chunk(0x09057001)]
    public partial class Chunk09057001 : Chunk<CPlugIndexBuffer>
    {
        /// <inheritdoc />
        public override uint Id => 0x09057001;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09057000 => new Chunk09057000(),
        0x09057001 => new Chunk09057001(),
        _ => base.NewChunk(chunkId),
    };
}
