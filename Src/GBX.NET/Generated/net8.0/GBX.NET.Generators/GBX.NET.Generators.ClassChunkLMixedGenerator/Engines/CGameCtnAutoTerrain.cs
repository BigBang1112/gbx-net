namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03120000</remarks>
[Class(0x03120000)]
public partial class CGameCtnAutoTerrain : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03120000;




    private Int3 offset;
    [AppliedWithChunk<Chunk03120001>]
    public Int3 Offset { get => offset; set => offset = value; }

    private CGameCtnZoneGenealogy? genealogy;
    [AppliedWithChunk<Chunk03120001>]
    public CGameCtnZoneGenealogy? Genealogy { get => genealogy; set => genealogy = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnAutoTerrain"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnAutoTerrain() { }


    /// <summary>
    /// CGameCtnAutoTerrain 0x001 chunk
    /// </summary>
    [Chunk(0x03120001)]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03120001 : Chunk<CGameCtnAutoTerrain>
    {
        /// <inheritdoc />
        public override uint Id => 0x03120001;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnAutoTerrain n, GbxReaderWriter rw)
        {
            rw.Int3(ref n.offset);
            rw.NodeRef<CGameCtnZoneGenealogy>(ref n.genealogy);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03120001 => new Chunk03120001(),
        _ => base.NewChunk(chunkId),
    };
}
