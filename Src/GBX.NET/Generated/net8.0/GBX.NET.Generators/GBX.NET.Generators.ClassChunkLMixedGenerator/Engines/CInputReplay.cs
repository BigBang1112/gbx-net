namespace GBX.NET.Engines.Input;

/// <remarks>ID: 0x1300D000</remarks>
[Class(0x1300D000)]
public partial class CInputReplay : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x1300D000;




    /// <summary>
    /// Creates a new instance of <see cref="CInputReplay"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CInputReplay() { }


    /// <summary>
    /// CInputReplay 0x000 chunk
    /// </summary>
    [Chunk(0x1300D000)]
    public partial class Chunk1300D000 : Chunk<CInputReplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x1300D000;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x1300D000 => new Chunk1300D000(),
        _ => base.NewChunk(chunkId),
    };
}
