namespace GBX.NET.Engines.Script;

/// <remarks>ID: 0x11001000</remarks>
[Class(0x11001000)]
public partial class CScriptTraitsPersistent : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x11001000;





    /// <summary>
    /// CScriptTraitsPersistent 0x000 chunk
    /// </summary>
    [Chunk(0x11001000)]
    public partial class Chunk11001000 : Chunk<CScriptTraitsPersistent>
    {
        /// <inheritdoc />
        public override uint Id => 0x11001000;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x11001000 => new Chunk11001000(),
        _ => base.NewChunk(chunkId),
    };
}
