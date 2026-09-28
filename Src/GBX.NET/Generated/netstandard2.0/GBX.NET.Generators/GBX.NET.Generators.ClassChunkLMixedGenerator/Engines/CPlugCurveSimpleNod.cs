namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09185000</remarks>
[Class(0x09185000)]
public partial class CPlugCurveSimpleNod : CFuncKeysReal, IClass
{
    [Hexadecimal] public static new uint Id => 0x09185000;





    /// <summary>
    /// CPlugCurveSimpleNod 0x000 chunk
    /// </summary>
    [Chunk(0x09185000)]
    public partial class Chunk09185000 : Chunk<CPlugCurveSimpleNod>
    {
        /// <inheritdoc />
        public override uint Id => 0x09185000;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09185000 => new Chunk09185000(),
        _ => base.NewChunk(chunkId),
    };
}
