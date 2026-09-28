namespace GBX.NET.Engines.Hms;

/// <remarks>ID: 0x06003000</remarks>
[Class(0x06003000)]
public partial class CHmsItem : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x06003000;




    private CPlugSolid? solid;
    [AppliedWithChunk<Chunk06003001>]
    public CPlugSolid? Solid { get => solid; set => solid = value; }


    /// <summary>
    /// CHmsItem 0x001 chunk (solid)
    /// </summary>
    [Chunk(0x06003001, "solid")]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF | GameVersion.MP3)]
    public partial class Chunk06003001 : Chunk<CHmsItem>
    {
        /// <inheritdoc />
        public override uint Id => 0x06003001;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF | GameVersion.MP3;


        public override void ReadWrite(CHmsItem n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugSolid>(ref n.solid);
        }
    }

    /// <summary>
    /// CHmsItem 0x00E chunk
    /// </summary>
    [Chunk(0x0600300E)]
    [ChunkGameVersion(GameVersion.TM10)]
    public partial class Chunk0600300E : Chunk<CHmsItem>
    {
        /// <inheritdoc />
        public override uint Id => 0x0600300E;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10;

        public ulong U01;
        public short U02;

        public override void ReadWrite(CHmsItem n, GbxReaderWriter rw)
        {
            rw.UInt64(ref U01);
            rw.Int16(ref U02);
        }
    }

    /// <summary>
    /// CHmsItem 0x010 chunk
    /// </summary>
    [Chunk(0x06003010)]
    [ChunkGameVersion(GameVersion.TMSX)]
    public partial class Chunk06003010 : Chunk<CHmsItem>
    {
        /// <inheritdoc />
        public override uint Id => 0x06003010;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX;

        public ulong U01;
        public short U02;

        public override void ReadWrite(CHmsItem n, GbxReaderWriter rw)
        {
            rw.UInt64(ref U01);
            rw.Int16(ref U02);
        }
    }

    /// <summary>
    /// CHmsItem 0x011 chunk
    /// </summary>
    [Chunk(0x06003011)]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3)]
    public partial class Chunk06003011 : Chunk<CHmsItem>
    {
        /// <inheritdoc />
        public override uint Id => 0x06003011;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3;

        public ulong U01;
        public short U02;

        public override void ReadWrite(CHmsItem n, GbxReaderWriter rw)
        {
            rw.UInt64(ref U01);
            rw.Int16(ref U02);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x06003001 => new Chunk06003001(),
        0x0600300E => new Chunk0600300E(),
        0x06003010 => new Chunk06003010(),
        0x06003011 => new Chunk06003011(),
        _ => base.NewChunk(chunkId),
    };
}
