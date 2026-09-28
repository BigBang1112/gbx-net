namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03059000</remarks>
[Class(0x03059000)]
public partial class CGameCtnBlockSkin : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03059000;




    private string? text;
    [AppliedWithChunk<Chunk03059000>]
    [AppliedWithChunk<Chunk03059001>]
    [AppliedWithChunk<Chunk03059002>]
    public string? Text { get => text; set => text = value; }

    private PackDesc? packDesc;
    [AppliedWithChunk<Chunk03059001>]
    [AppliedWithChunk<Chunk03059002>]
    public PackDesc? PackDesc { get => packDesc; set => packDesc = value; }

    private PackDesc? parentPackDesc;
    [AppliedWithChunk<Chunk03059002>]
    public PackDesc? ParentPackDesc { get => parentPackDesc; set => parentPackDesc = value; }

    private PackDesc? foregroundPackDesc;
    [AppliedWithChunk<Chunk03059003>]
    public PackDesc? ForegroundPackDesc { get => foregroundPackDesc; set => foregroundPackDesc = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnBlockSkin"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnBlockSkin() { }


    /// <summary>
    /// CGameCtnBlockSkin 0x000 chunk (text)
    /// </summary>
    [Chunk(0x03059000, "text")]
    public partial class Chunk03059000 : Chunk<CGameCtnBlockSkin>
    {
        /// <inheritdoc />
        public override uint Id => 0x03059000;

        public string? U01;

        public override void ReadWrite(CGameCtnBlockSkin n, GbxReaderWriter rw)
        {
            rw.String(ref n.text);
            rw.String(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnBlockSkin 0x001 chunk (skin)
    /// </summary>
    [Chunk(0x03059001, "skin")]
    public partial class Chunk03059001 : Chunk<CGameCtnBlockSkin>
    {
        /// <inheritdoc />
        public override uint Id => 0x03059001;


        public override void ReadWrite(CGameCtnBlockSkin n, GbxReaderWriter rw)
        {
            rw.String(ref n.text);
            rw.PackDesc(ref n.packDesc);
        }
    }

    /// <summary>
    /// CGameCtnBlockSkin 0x002 chunk (skin + parent skin)
    /// </summary>
    [Chunk(0x03059002, "skin + parent skin")]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk03059002 : Chunk<CGameCtnBlockSkin>
    {
        /// <inheritdoc />
        public override uint Id => 0x03059002;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;


        public override void ReadWrite(CGameCtnBlockSkin n, GbxReaderWriter rw)
        {
            rw.String(ref n.text);
            rw.PackDesc(ref n.packDesc);
            rw.PackDesc(ref n.parentPackDesc);
        }
    }

    /// <summary>
    /// CGameCtnBlockSkin 0x003 chunk (secondary skin)
    /// </summary>
    [Chunk(0x03059003, "secondary skin")]
    public partial class Chunk03059003 : Chunk<CGameCtnBlockSkin>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03059003;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnBlockSkin n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.PackDesc(ref n.foregroundPackDesc);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03059000 => new Chunk03059000(),
        0x03059001 => new Chunk03059001(),
        0x03059002 => new Chunk03059002(),
        0x03059003 => new Chunk03059003(),
        _ => base.NewChunk(chunkId),
    };
}
