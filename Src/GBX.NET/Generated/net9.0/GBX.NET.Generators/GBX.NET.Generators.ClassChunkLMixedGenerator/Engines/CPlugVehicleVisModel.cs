namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090E7000</remarks>
[Class(0x090E7000)]
public partial class CPlugVehicleVisModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090E7000;





    /// <summary>
    /// CPlugVehicleVisModel 0x000 chunk
    /// </summary>
    [Chunk(0x090E7000)]
    public partial class Chunk090E7000 : Chunk<CPlugVehicleVisModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x090E7000;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090E7000 => new Chunk090E7000(),
        _ => base.NewChunk(chunkId),
    };
}
