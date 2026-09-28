namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03170000</remarks>
[Class(0x03170000)]
public partial class CGamePlayerProfileChunk_ScriptPersistentTraits : CGamePlayerProfileChunk, IClass
{
    [Hexadecimal] public static new uint Id => 0x03170000;





    /// <summary>
    /// CGamePlayerProfileChunk_ScriptPersistentTraits 0x000 skippable chunk
    /// </summary>
    [Chunk(0x03170000)]
    public partial class Chunk03170000 : SkippableChunk<CGamePlayerProfileChunk_ScriptPersistentTraits>
    {
        /// <inheritdoc />
        public override uint Id => 0x03170000;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03170000 => new Chunk03170000(),
        _ => base.NewChunk(chunkId),
    };
}
