namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0311D000</remarks>
[Class(0x0311D000)]
public partial class CGameCtnZoneGenealogy : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0311D000;




    private int currentIndex;
    [AppliedWithChunk<Chunk0311D001>]
    [AppliedWithChunk<Chunk0311D002>]
    public int CurrentIndex { get => currentIndex; set => currentIndex = value; }

    private Direction dir;
    [AppliedWithChunk<Chunk0311D001>]
    [AppliedWithChunk<Chunk0311D002>]
    public Direction Dir { get => dir; set => dir = value; }

    private string[]? zoneIds;
    [AppliedWithChunk<Chunk0311D002>]
    public string[]? ZoneIds { get => zoneIds; set => zoneIds = value; }

    private string? currentZoneId;
    [AppliedWithChunk<Chunk0311D002>]
    public string? CurrentZoneId { get => currentZoneId; set => currentZoneId = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnZoneGenealogy"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnZoneGenealogy() { }


    /// <summary>
    /// CGameCtnZoneGenealogy 0x001 chunk
    /// </summary>
    [Chunk(0x0311D001)]
    public partial class Chunk0311D001 : Chunk<CGameCtnZoneGenealogy>
    {
        /// <inheritdoc />
        public override uint Id => 0x0311D001;


        public override void ReadWrite(CGameCtnZoneGenealogy n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.currentIndex);
            rw.EnumInt32<Direction>(ref n.dir);
        }
    }

    /// <summary>
    /// CGameCtnZoneGenealogy 0x002 chunk
    /// </summary>
    [Chunk(0x0311D002)]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0311D002 : Chunk<CGameCtnZoneGenealogy>
    {
        /// <inheritdoc />
        public override uint Id => 0x0311D002;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnZoneGenealogy n, GbxReaderWriter rw)
        {
            rw.ArrayId(ref n.zoneIds!);
            rw.Int32(ref n.currentIndex);
            rw.EnumInt32<Direction>(ref n.dir);
            rw.Id(ref n.currentZoneId);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0311D001 => new Chunk0311D001(),
        0x0311D002 => new Chunk0311D002(),
        _ => base.NewChunk(chunkId),
    };
}
