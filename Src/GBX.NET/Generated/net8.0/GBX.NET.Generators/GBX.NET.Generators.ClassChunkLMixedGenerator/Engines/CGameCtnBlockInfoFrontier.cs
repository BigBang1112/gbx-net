namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03050000</remarks>
[Class(0x03050000)]
public partial class CGameCtnBlockInfoFrontier : CGameCtnBlockInfo, IClass
{
    [Hexadecimal] public static new uint Id => 0x03050000;




    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnBlockInfoFrontier"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnBlockInfoFrontier() { }


    /// <summary>
    /// CGameCtnBlockInfoFrontier 0x000 chunk
    /// </summary>
    [Chunk(0x03050000)]
    public partial class Chunk03050000 : Chunk<CGameCtnBlockInfoFrontier>
    {
        /// <inheritdoc />
        public override uint Id => 0x03050000;

        public bool U01;

        public override void ReadWrite(CGameCtnBlockInfoFrontier n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03050000 => new Chunk03050000(),
        _ => base.NewChunk(chunkId),
    };
}
