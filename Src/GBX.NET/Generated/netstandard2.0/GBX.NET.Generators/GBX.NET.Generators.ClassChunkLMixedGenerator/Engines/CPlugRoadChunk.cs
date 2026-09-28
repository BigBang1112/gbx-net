namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09128000</remarks>
[Class(0x09128000)]
public partial class CPlugRoadChunk : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x09128000;





    /// <summary>
    /// CPlugRoadChunk 0x000 chunk
    /// </summary>
    [Chunk(0x09128000)]
    public partial class Chunk09128000 : Chunk<CPlugRoadChunk>
    {
        /// <inheritdoc />
        public override uint Id => 0x09128000;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09128000 => new Chunk09128000(),
        _ => base.NewChunk(chunkId),
    };
}
