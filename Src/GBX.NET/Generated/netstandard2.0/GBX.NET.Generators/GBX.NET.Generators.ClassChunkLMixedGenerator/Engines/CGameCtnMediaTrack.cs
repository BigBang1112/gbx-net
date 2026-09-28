namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker track.
/// </summary>
/// <remarks>ID: 0x03078000</remarks>
[Class(0x03078000)]
public partial class CGameCtnMediaTrack : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03078000;




    private string? name;
    [AppliedWithChunk<Chunk03078001>]
    public string? Name { get => name; set => name = value; }

    private bool isKeepPlaying;
    [AppliedWithChunk<Chunk03078002>]
    [AppliedWithChunk<Chunk03078004>]
    [AppliedWithChunk<Chunk03078005>]
    public bool IsKeepPlaying { get => isKeepPlaying; set => isKeepPlaying = value; }

    private bool isReadOnly;
    [AppliedWithChunk<Chunk03078003>]
    [AppliedWithChunk<Chunk03078004>]
    [AppliedWithChunk<Chunk03078005>]
    public bool IsReadOnly { get => isReadOnly; set => isReadOnly = value; }

    private bool isCycling;
    [AppliedWithChunk<Chunk03078005>]
    public bool IsCycling { get => isCycling; set => isCycling = value; }

    private TimeSingle? repeatingSegmentStart;
    [AppliedWithChunk<Chunk03078005>]
    public TimeSingle? RepeatingSegmentStart { get => repeatingSegmentStart; set => repeatingSegmentStart = value; }

    private TimeSingle? repeatingSegmentEnd;
    [AppliedWithChunk<Chunk03078005>]
    public TimeSingle? RepeatingSegmentEnd { get => repeatingSegmentEnd; set => repeatingSegmentEnd = value; }


    /// <summary>
    /// CGameCtnMediaTrack 0x001 chunk (name and blocks)
    /// </summary>
    [Chunk(0x03078001, "name and blocks")]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03078001 : Chunk<CGameCtnMediaTrack>
    {
        /// <inheritdoc />
        public override uint Id => 0x03078001;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4 | GameVersion.TM2020;

        public int U01 = -1;

        public override void ReadWrite(CGameCtnMediaTrack n, GbxReaderWriter rw)
        {
            rw.String(ref n.name);
            rw.ListNodeRef_deprec<CGameCtnMediaBlock>(ref n.blocks!);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnMediaTrack 0x002 chunk (TMS/TMU IsKeepPlaying)
    /// </summary>
    [Chunk(0x03078002, "TMS/TMU IsKeepPlaying")]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU)]
    public partial class Chunk03078002 : Chunk<CGameCtnMediaTrack>
    {
        /// <inheritdoc />
        public override uint Id => 0x03078002;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU;


        public override void ReadWrite(CGameCtnMediaTrack n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isKeepPlaying);
        }
    }

    /// <summary>
    /// CGameCtnMediaTrack 0x003 chunk (TMS/TMU IsReadOnly)
    /// </summary>
    [Chunk(0x03078003, "TMS/TMU IsReadOnly")]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU)]
    public partial class Chunk03078003 : Chunk<CGameCtnMediaTrack>
    {
        /// <inheritdoc />
        public override uint Id => 0x03078003;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU;


        public override void ReadWrite(CGameCtnMediaTrack n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isReadOnly);
        }
    }

    /// <summary>
    /// CGameCtnMediaTrack 0x004 chunk (TMF parameters)
    /// </summary>
    [Chunk(0x03078004, "TMF parameters")]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk03078004 : Chunk<CGameCtnMediaTrack>
    {
        /// <inheritdoc />
        public override uint Id => 0x03078004;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;


        public override void ReadWrite(CGameCtnMediaTrack n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isKeepPlaying);
            rw.Boolean(ref n.isReadOnly);
        }
    }

    /// <summary>
    /// CGameCtnMediaTrack 0x005 chunk (MP parameters)
    /// </summary>
    [Chunk(0x03078005, "MP parameters")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4 | GameVersion.TM2020, 1, 1, 1)]
    public partial class Chunk03078005 : Chunk<CGameCtnMediaTrack>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03078005;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnMediaTrack n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Boolean(ref n.isKeepPlaying);
            rw.Boolean(ref n.isReadOnly);
            rw.Boolean(ref n.isCycling);
            if (Version >= 1)
            {
                rw.TimeSingleNullable(ref n.repeatingSegmentStart);
                rw.TimeSingleNullable(ref n.repeatingSegmentEnd);
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03078001 => new Chunk03078001(),
        0x03078002 => new Chunk03078002(),
        0x03078003 => new Chunk03078003(),
        0x03078004 => new Chunk03078004(),
        0x03078005 => new Chunk03078005(),
        _ => base.NewChunk(chunkId),
    };
}
