namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03294000</remarks>
[Class(0x03294000)]
public partial class CGameCtnMediaBlockTurret : CGameCtnMediaBlock, IClass
{
    [Hexadecimal] public static new uint Id => 0x03294000;




    private CPlugDataTape? dataTape;
    [AppliedWithChunk<Chunk03294000>]
    public CPlugDataTape? DataTape { get => dataTape; set => dataTape = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockTurret"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockTurret() { }


    /// <summary>
    /// CGameCtnMediaBlockTurret 0x000 chunk
    /// </summary>
    [Chunk(0x03294000)]
    public partial class Chunk03294000 : Chunk<CGameCtnMediaBlockTurret>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03294000;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnMediaBlockTurret n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugDataTape>(ref n.dataTape);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03294000 => new Chunk03294000(),
        _ => base.NewChunk(chunkId),
    };
}
