namespace GBX.NET.Engines.Script;

/// <remarks>ID: 0x11002000</remarks>
[Class(0x11002000)]
public partial class CScriptTraitsMetadata : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x11002000;





    /// <summary>
    /// CScriptTraitsMetadata 0x000 chunk
    /// </summary>
    [Chunk(0x11001000)]
    public partial class Chunk11001000 : Chunk11002000
    {
        /// <inheritdoc />
        public override uint Id => 0x11001000;

    }

    /// <summary>
    /// CScriptTraitsMetadata 0x000 chunk
    /// </summary>
    [Chunk(0x11002000)]
    public partial class Chunk11002000 : Chunk<CScriptTraitsMetadata>
    {
        /// <inheritdoc />
        public override uint Id => 0x11002000;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x11001000 => new Chunk11001000(),
        0x11002000 => new Chunk11002000(),
        _ => base.NewChunk(chunkId),
    };
}
