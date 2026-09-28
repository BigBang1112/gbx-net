namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090E9000</remarks>
[Class(0x090E9000)]
public partial class CPlugVehicleMaterialGroup : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090E9000;




    /// <summary>
    /// Creates a new instance of <see cref="CPlugVehicleMaterialGroup"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugVehicleMaterialGroup() { }


    /// <summary>
    /// CPlugVehicleMaterialGroup 0x000 chunk
    /// </summary>
    [Chunk(0x090E9000)]
    public partial class Chunk090E9000 : Chunk<CPlugVehicleMaterialGroup>
    {
        /// <inheritdoc />
        public override uint Id => 0x090E9000;

        public int[]? U01;

        public override void ReadWrite(CPlugVehicleMaterialGroup n, GbxReaderWriter rw)
        {
            rw.Array<int>(ref U01!);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090E9000 => new Chunk090E9000(),
        _ => base.NewChunk(chunkId),
    };
}
