namespace GBX.NET.Engines.Game;

/// <summary>
/// Ghost data.
/// </summary>
/// <remarks>ID: 0x0303F000</remarks>
[Class(0x0303F000)]
public partial class CGameGhost : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0303F000;




    private uint? savedMobilClassId;
    [AppliedWithChunk<Chunk0303F004>]
    public uint? SavedMobilClassId { get => savedMobilClassId; set => savedMobilClassId = value; }

    private bool isReplaying;
    [AppliedWithChunk<Chunk0303F006>]
    public bool IsReplaying { get => isReplaying; set => isReplaying = value; }


    /// <summary>
    /// CGameGhost 0x003 chunk
    /// </summary>
    [Chunk(0x0303F003)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMPU | GameVersion.TMS | GameVersion.TMSX | GameVersion.TMNESWC)]
    public partial class Chunk0303F003 : Chunk<CGameGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303F003;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMPU | GameVersion.TMS | GameVersion.TMSX | GameVersion.TMNESWC;

    }

    /// <summary>
    /// CGameGhost 0x004 chunk
    /// </summary>
    [Chunk(0x0303F004)]
    [ChunkGameVersion(GameVersion.TMS | GameVersion.TMSX | GameVersion.TMNESWC)]
    public partial class Chunk0303F004 : Chunk<CGameGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303F004;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMS | GameVersion.TMSX | GameVersion.TMNESWC;


        public override void ReadWrite(CGameGhost n, GbxReaderWriter rw)
        {
            rw.UInt32(ref n.savedMobilClassId);
        }
    }

    /// <summary>
    /// CGameGhost 0x005 chunk
    /// </summary>
    [Chunk(0x0303F005)]
    [ChunkGameVersion(GameVersion.TMU | GameVersion.TMF)]
    public partial class Chunk0303F005 : Chunk<CGameGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303F005;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU | GameVersion.TMF;

    }

    /// <summary>
    /// CGameGhost 0x006 chunk
    /// </summary>
    [Chunk(0x0303F006)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0303F006 : Chunk0303F005
    {
        /// <inheritdoc />
        public override uint Id => 0x0303F006;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameGhost n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isReplaying);
            base.ReadWrite(n, rw);
        }
    }

    /// <summary>
    /// CGameGhost 0x007 skippable chunk
    /// </summary>
    [Chunk(0x0303F007)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0303F007 : SkippableChunk<CGameGhost>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303F007;

        /// <inheritdoc />
        public override bool Ignore => true;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0303F003 => new Chunk0303F003(),
        0x0303F004 => new Chunk0303F004(),
        0x0303F005 => new Chunk0303F005(),
        0x0303F006 => new Chunk0303F006(),
        0x0303F007 => new Chunk0303F007(),
        _ => base.NewChunk(chunkId),
    };
}
