namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03049000</remarks>
[Class(0x03049000)]
public partial class CGameLeagueManager : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03049000;




    private int cacheDuration;
    [AppliedWithChunk<Chunk03049000>]
    public int CacheDuration { get => cacheDuration; set => cacheDuration = value; }

    private bool updated;
    [AppliedWithChunk<Chunk03049000>]
    public bool Updated { get => updated; set => updated = value; }

    private DateTime? timestamp;
    [AppliedWithChunk<Chunk03049000>]
    public DateTime? Timestamp { get => timestamp; set => timestamp = value; }

    private CGameLeague[]? leagues;
    [AppliedWithChunk<Chunk03049000>]
    public CGameLeague[]? Leagues { get => leagues; set => leagues = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameLeagueManager"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameLeagueManager() { }


    /// <summary>
    /// CGameLeagueManager 0x000 chunk
    /// </summary>
    [Chunk(0x03049000)]
    public partial class Chunk03049000 : Chunk<CGameLeagueManager>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03049000;

        public int Version { get; set; }


        public override void ReadWrite(CGameLeagueManager n, GbxReaderWriter rw)
        {
            rw.VersionByte(this);
            rw.Int32(ref n.cacheDuration);
            rw.Boolean(ref n.updated);
            if (n.Updated)
            {
                rw.SystemTime(ref n.timestamp);
                rw.ArrayNodeRef_deprec<CGameLeague>(ref n.leagues!);
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03049000 => new Chunk03049000(),
        _ => base.NewChunk(chunkId),
    };
}
