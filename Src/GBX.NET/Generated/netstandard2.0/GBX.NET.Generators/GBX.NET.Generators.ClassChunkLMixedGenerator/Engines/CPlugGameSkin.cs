namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090F4000</remarks>
[Class(0x090F4000)]
public partial class CPlugGameSkin : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090F4000;




    private string? dirName;
    [AppliedWithChunk<HeaderChunk090F4000>]
    [AppliedWithChunk<Chunk090F4004>]
    public string? DirName { get => dirName; set => dirName = value; }

    private string? painterTextureName;
    [AppliedWithChunk<HeaderChunk090F4000>]
    [AppliedWithChunk<Chunk090F4003>]
    [AppliedWithChunk<Chunk090F4004>]
    public string? PainterTextureName { get => painterTextureName; set => painterTextureName = value; }

    private string? painterSceneId;
    [AppliedWithChunk<HeaderChunk090F4000>]
    [AppliedWithChunk<Chunk090F4003>]
    [AppliedWithChunk<Chunk090F4004>]
    public string? PainterSceneId { get => painterSceneId; set => painterSceneId = value; }

    private HeaderFid[]? headerFids;
    [AppliedWithChunk<HeaderChunk090F4000>]
    public HeaderFid[]? HeaderFids { get => headerFids; set => headerFids = value; }

    private string? dirNameAlt;
    [AppliedWithChunk<HeaderChunk090F4000>]
    [AppliedWithChunk<Chunk090F4004>]
    public string? DirNameAlt { get => dirNameAlt; set => dirNameAlt = value; }

    private Fid[]? customizableFids;
    [AppliedWithChunk<Chunk090F4004>]
    public Fid[]? CustomizableFids { get => customizableFids; set => customizableFids = value; }

    /// <summary>
    /// [SHeader] CPlugGameSkin 0x000 header chunk
    /// </summary>
    [Chunk(0x090F4000)]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4, 2, 4, 5, 5, 5)]
    public partial class HeaderChunk090F4000 : HeaderChunk<CPlugGameSkin>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090F4000;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;

        public int Version { get; set; }

        /// <summary>
        /// something dialog?
        /// </summary>
        public bool U01;
        public string? U02;
        public int U03;
        public byte U04;

        public override void ReadWrite(CPlugGameSkin n, GbxReaderWriter rw)
        {
            rw.VersionByte(this);
            rw.String(ref n.dirName);
            if (Version >= 1)
            {
                rw.String(ref n.painterTextureName);
                rw.String(ref n.painterSceneId);
            }
            rw.ArrayReadableWritable<HeaderFid>(ref n.headerFids!, version: Version, byteLengthPrefix: true);
            if (Version >= 4)
            {
                rw.String(ref n.dirNameAlt);
                if (Version >= 5)
                {
                    rw.Boolean(ref U01); // something dialog?
                    if (Version >= 6)
                    {
                        rw.String(ref U02);
                        if (Version >= 7)
                        {
                            rw.Int32(ref U03);
                            if (Version >= 8)
                            {
                                rw.Byte(ref U04);
                            }
                        }
                    }
                }
            }
        }
    }


    /// <summary>
    /// CPlugGameSkin 0x000 chunk
    /// </summary>
    [Chunk(0x090F4000)]
    public partial class Chunk090F4000 : Chunk<CPlugGameSkin>
    {
        /// <inheritdoc />
        public override uint Id => 0x090F4000;

    }

    /// <summary>
    /// CPlugGameSkin 0x001 chunk
    /// </summary>
    [Chunk(0x090F4001)]
    public partial class Chunk090F4001 : Chunk<CPlugGameSkin>
    {
        /// <inheritdoc />
        public override uint Id => 0x090F4001;

    }

    /// <summary>
    /// CPlugGameSkin 0x003 chunk
    /// </summary>
    [Chunk(0x090F4003)]
    public partial class Chunk090F4003 : Chunk<CPlugGameSkin>
    {
        /// <inheritdoc />
        public override uint Id => 0x090F4003;


        public override void ReadWrite(CPlugGameSkin n, GbxReaderWriter rw)
        {
            rw.String(ref n.painterTextureName);
            rw.String(ref n.painterSceneId);
        }
    }

    /// <summary>
    /// CPlugGameSkin 0x004 chunk
    /// </summary>
    [Chunk(0x090F4004)]
    public partial class Chunk090F4004 : Chunk<CPlugGameSkin>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090F4004;

        public int Version { get; set; }

        /// <summary>
        /// something dialog?
        /// </summary>
        public bool U01;
        public string? U02;
        public int U03;

        public override void ReadWrite(CPlugGameSkin n, GbxReaderWriter rw)
        {
            rw.VersionByte(this);
            rw.String(ref n.dirName);
            if (Version >= 1)
            {
                rw.String(ref n.painterTextureName);
                rw.String(ref n.painterSceneId);
            }
            rw.ArrayReadableWritable<Fid>(ref n.customizableFids!, version: Version, byteLengthPrefix: true);
            if (Version >= 4)
            {
                rw.String(ref n.dirNameAlt);
                if (Version >= 5)
                {
                    rw.Boolean(ref U01); // something dialog?
                    if (Version >= 6)
                    {
                        rw.String(ref U02);
                        if (Version >= 7)
                        {
                            rw.Int32(ref U03);
                        }
                    }
                }
            }
        }
    }


    public sealed partial class Fid : IReadableWritable
    {
    }

    public sealed partial class HeaderFid : IReadableWritable
    {

        private uint classId;
        public uint ClassId { get => classId; set => classId = value; }

        private string? name;
        public string? Name { get => name; set => name = value; }

        private string? directory;
        public string? Directory { get => directory; set => directory = value; }

        private bool mipMaps;
        public bool MipMaps { get => mipMaps; set => mipMaps = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.UInt32(ref classId);
            rw.String(ref name);
            rw.String(ref directory);
            if (v >= 2)
            {
                rw.Boolean(ref mipMaps);
            }
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090F4000 => new Chunk090F4000(),
        0x090F4001 => new Chunk090F4001(),
        0x090F4003 => new Chunk090F4003(),
        0x090F4004 => new Chunk090F4004(),
        _ => base.NewChunk(chunkId),
    };
}
