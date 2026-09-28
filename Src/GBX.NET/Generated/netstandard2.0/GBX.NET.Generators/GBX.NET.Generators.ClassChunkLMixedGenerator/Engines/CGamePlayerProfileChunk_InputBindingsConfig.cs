namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0312F000</remarks>
[Class(0x0312F000)]
public partial class CGamePlayerProfileChunk_InputBindingsConfig : CGamePlayerProfileChunk, IClass
{
    [Hexadecimal] public static new uint Id => 0x0312F000;





    /// <summary>
    /// CGamePlayerProfileChunk_InputBindingsConfig 0x000 skippable chunk
    /// </summary>
    [Chunk(0x0312F000)]
    public partial class Chunk0312F000 : SkippableChunk<CGamePlayerProfileChunk_InputBindingsConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0312F000;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0312F000 => new Chunk0312F000(),
        _ => base.NewChunk(chunkId),
    };
}
