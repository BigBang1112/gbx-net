namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A062000</remarks>
[Class(0x0A062000)]
public partial class CSceneTrafficGraph : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0A062000;





    /// <summary>
    /// CSceneTrafficGraph 0x004 chunk
    /// </summary>
    [Chunk(0x0A062004)]
    public partial class Chunk0A062004 : Chunk<CSceneTrafficGraph>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A062004;

    }

    /// <summary>
    /// CSceneTrafficGraph 0x005 chunk
    /// </summary>
    [Chunk(0x0A062005)]
    public partial class Chunk0A062005 : Chunk<CSceneTrafficGraph>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A062005;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0A062004 => new Chunk0A062004(),
        0x0A062005 => new Chunk0A062005(),
        _ => base.NewChunk(chunkId),
    };
}
