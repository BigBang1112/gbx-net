namespace GBX.NET.Engines.GameData;

/// <summary>
/// Collector. Something that can have an icon.
/// </summary>
/// <remarks>ID: 0x2E001000</remarks>
[Class(0x2E001000)]
public partial class CGameCtnCollector : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x2E001000;




    private Ident ident = Ident.Empty;
    [AppliedWithChunk<HeaderChunk2E001003>]
    [AppliedWithChunk<Chunk2E001002>]
    [AppliedWithChunk<Chunk2E00100B>]
    public Ident Ident { get => ident; set => ident = value; }

    private string? pageName;
    [AppliedWithChunk<HeaderChunk2E001003>]
    public string? PageName { get => pageName; set => pageName = value; }

    private string? parentCollectorId;
    [AppliedWithChunk<HeaderChunk2E001003>]
    public string? ParentCollectorId { get => parentCollectorId; set => parentCollectorId = value; }

    private ECollectorFlags flags;
    [AppliedWithChunk<HeaderChunk2E001003>]
    public ECollectorFlags Flags { get => flags; set => flags = value; }

    private int copperPrice;
    [AppliedWithChunk<HeaderChunk2E001003>]
    [AppliedWithChunk<Chunk2E001007>]
    public int CopperPrice { get => copperPrice; set => copperPrice = value; }

    private string? name;
    [AppliedWithChunk<HeaderChunk2E001003>]
    [AppliedWithChunk<Chunk2E00100C>]
    public string? Name { get => name; set => name = value; }

    private EProdState prodState;
    [AppliedWithChunk<HeaderChunk2E001003>]
    [AppliedWithChunk<Chunk2E001011>]
    public EProdState ProdState { get => prodState; set => prodState = value; }

    private DateTime? lightmapComputeTime;
    [AppliedWithChunk<HeaderChunk2E001006>]
    public DateTime? LightmapComputeTime { get => lightmapComputeTime; set => lightmapComputeTime = value; }

    private string? defaultSkinName;
    [AppliedWithChunk<HeaderChunk2E001008>]
    public string? DefaultSkinName { get => defaultSkinName; set => defaultSkinName = value; }

    private ESkinKind skinKind;
    [AppliedWithChunk<Chunk2E001006>]
    public ESkinKind SkinKind { get => skinKind; set => skinKind = value; }

    private bool isInternal;
    [AppliedWithChunk<Chunk2E001007>]
    [AppliedWithChunk<Chunk2E001011>]
    public bool IsInternal { get => isInternal; set => isInternal = value; }

    private bool needUnlock;
    [AppliedWithChunk<Chunk2E001007>]
    public bool NeedUnlock { get => needUnlock; set => needUnlock = value; }

    private string? description;
    [AppliedWithChunk<Chunk2E00100D>]
    public string? Description { get => description; set => description = value; }

    private bool iconUseAutoRender;
    [AppliedWithChunk<Chunk2E00100E>]
    public bool IconUseAutoRender { get => iconUseAutoRender; set => iconUseAutoRender = value; }

    private int iconQuarterRotationY;
    [AppliedWithChunk<Chunk2E00100E>]
    public int IconQuarterRotationY { get => iconQuarterRotationY; set => iconQuarterRotationY = value; }

    private CPlugFileZip? defaultSkin;
    [AppliedWithChunk<Chunk2E001010>]
    public CPlugFileZip? DefaultSkin { get => defaultSkinFile?.GetNode(ref defaultSkin) ?? defaultSkin; set => defaultSkin = value; }
    private Components.GbxRefTableFile? defaultSkinFile;
    public Components.GbxRefTableFile? DefaultSkinFile { get => defaultSkinFile; set => defaultSkinFile = value; }
    public CPlugFileZip? GetDefaultSkin(GbxReadSettings settings = default, bool exceptions = false) => defaultSkinFile?.GetNode(ref defaultSkin, settings, exceptions) ?? defaultSkin;

    private string? skinDirectory;
    [AppliedWithChunk<Chunk2E001010>]
    public string? SkinDirectory { get => skinDirectory; set => skinDirectory = value; }

    private bool isAdvanced;
    [AppliedWithChunk<Chunk2E001011>]
    public bool IsAdvanced { get => isAdvanced; set => isAdvanced = value; }

    /// <summary>
    /// [SHeaderDesc] CGameCtnCollector 0x003 header chunk (desc)
    /// </summary>
    [Chunk(0x2E001003, "desc")]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020, 3, 4, 5, 7, 8, 8, 8)]
    public partial class HeaderChunk2E001003 : HeaderChunk<CGameCtnCollector>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E001003;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }

        public string? U01;

        public override void ReadWrite(CGameCtnCollector n, GbxReaderWriter rw)
        {
            rw.Ident(ref n.ident);
            rw.VersionInt32(this);
            rw.String(ref n.pageName);
            if (Version == 5)
            {
                rw.Id(ref U01);
            }
            if (Version >= 4)
            {
                rw.Id(ref n.parentCollectorId);
            }
            if (Version >= 3)
            {
                rw.EnumInt32<ECollectorFlags>(ref n.flags);
                rw.Int16(ref n.catalogPosition);
                if (Version <= 5)
                {
                    rw.Byte(ref n.nbAvailableMin);
                    rw.Int32(ref n.copperPrice);
                    rw.Int16(ref n.nbAvailableMax);
                }
                if (Version >= 7)
                {
                    rw.String(ref n.name);
                    if (Version >= 8)
                    {
                        rw.EnumByte<EProdState>(ref n.prodState);
                    }
                }
            }
        }
    }

    /// <summary>
    /// [SHeaderIcon] CGameCtnCollector 0x004 header chunk (icon)
    /// </summary>
    [Chunk(0x2E001004, "icon")]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class HeaderChunk2E001004 : HeaderChunk<CGameCtnCollector>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E001004;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// [SHeaderLightMap] CGameCtnCollector 0x006 header chunk (lightmap compute time)
    /// </summary>
    [Chunk(0x2E001006, "lightmap compute time")]
    [ChunkGameVersion(GameVersion.TMT | GameVersion.MP3 | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class HeaderChunk2E001006 : HeaderChunk<CGameCtnCollector>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E001006;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMT | GameVersion.MP3 | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnCollector n, GbxReaderWriter rw)
        {
            rw.FileTime(ref n.lightmapComputeTime);
        }
    }

    /// <summary>
    /// [SHeaderDefaultSkin] CGameCtnCollector 0x008 header chunk (default skin)
    /// </summary>
    [Chunk(0x2E001008, "default skin")]
    public partial class HeaderChunk2E001008 : HeaderChunk<CGameCtnCollector>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E001008;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnCollector n, GbxReaderWriter rw)
        {
            rw.VersionByte(this);
            rw.String(ref n.defaultSkinName);
        }
    }


    /// <summary>
    /// CGameCtnCollector 0x002 chunk (Ident)
    /// </summary>
    [Chunk(0x2E001002, "Ident")]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX)]
    public partial class Chunk2E001002 : Chunk<CGameCtnCollector>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E001002;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX;


        public override void ReadWrite(CGameCtnCollector n, GbxReaderWriter rw)
        {
            rw.Ident(ref n.ident);
        }
    }

    /// <summary>
    /// CGameCtnCollector 0x006 chunk
    /// </summary>
    [Chunk(0x2E001006)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF)]
    public partial class Chunk2E001006 : Chunk<CGameCtnCollector>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E001006;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF;


        public override void ReadWrite(CGameCtnCollector n, GbxReaderWriter rw)
        {
            rw.EnumInt32<ESkinKind>(ref n.skinKind);
        }
    }

    /// <summary>
    /// CGameCtnCollector 0x007 chunk
    /// </summary>
    [Chunk(0x2E001007)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF)]
    public partial class Chunk2E001007 : Chunk<CGameCtnCollector>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E001007;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF;


        public override void ReadWrite(CGameCtnCollector n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isInternal);
            rw.Boolean(ref n.needUnlock);
            rw.Int32(ref n.catalogPosition);
            rw.Int32(ref n.nbAvailableMin);
            rw.Int32(ref n.copperPrice);
            rw.Int32(ref n.nbAvailableMax);
        }
    }

    /// <summary>
    /// CGameCtnCollector 0x008 chunk
    /// </summary>
    [Chunk(0x2E001008)]
    public partial class Chunk2E001008 : Chunk<CGameCtnCollector>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E001008;

        public CPlugGameSkin? U01;

        public override void ReadWrite(CGameCtnCollector n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugGameSkin>(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnCollector 0x009 chunk
    /// </summary>
    [Chunk(0x2E001009)]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk2E001009 : Chunk<CGameCtnCollector>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E001009;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnCollector 0x00A chunk
    /// </summary>
    [Chunk(0x2E00100A)]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk2E00100A : Chunk<CGameCtnCollector>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E00100A;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;

        public string? U01;

        public override void ReadWrite(CGameCtnCollector n, GbxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnCollector 0x00B chunk (ident)
    /// </summary>
    [Chunk(0x2E00100B, "ident")]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk2E00100B : Chunk<CGameCtnCollector>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E00100B;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnCollector n, GbxReaderWriter rw)
        {
            rw.Ident(ref n.ident);
        }
    }

    /// <summary>
    /// CGameCtnCollector 0x00C chunk (collector name)
    /// </summary>
    [Chunk(0x2E00100C, "collector name")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk2E00100C : Chunk<CGameCtnCollector>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E00100C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnCollector n, GbxReaderWriter rw)
        {
            rw.String(ref n.name);
        }
    }

    /// <summary>
    /// CGameCtnCollector 0x00D chunk (description)
    /// </summary>
    [Chunk(0x2E00100D, "description")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk2E00100D : Chunk<CGameCtnCollector>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E00100D;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnCollector n, GbxReaderWriter rw)
        {
            rw.String(ref n.description);
        }
    }

    /// <summary>
    /// CGameCtnCollector 0x00E chunk (icon render)
    /// </summary>
    [Chunk(0x2E00100E, "icon render")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk2E00100E : Chunk<CGameCtnCollector>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E00100E;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;


        public override void ReadWrite(CGameCtnCollector n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.iconUseAutoRender);
            rw.Int32(ref n.iconQuarterRotationY);
        }
    }

    /// <summary>
    /// CGameCtnCollector 0x010 chunk
    /// </summary>
    [Chunk(0x2E001010)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020, 2, 2, 2, 4)]
    public partial class Chunk2E001010 : Chunk<CGameCtnCollector>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E001010;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }

        public CMwNod? U01;

        public override void ReadWrite(CGameCtnCollector n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugFileZip>(ref n.defaultSkin, ref n.defaultSkinFile);
            rw.String(ref n.skinDirectory);
            if (Version >= 2)
            {
                if (n.SkinDirectory==null||n.SkinDirectory=="")
                {
                    rw.NodeRef<CMwNod>(ref U01);
                }
            }
        }
    }

    /// <summary>
    /// CGameCtnCollector 0x011 chunk
    /// </summary>
    [Chunk(0x2E001011)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020, 0, 1, 1, 1)]
    public partial class Chunk2E001011 : Chunk<CGameCtnCollector>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E001011;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnCollector n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Boolean(ref n.isInternal);
            rw.Boolean(ref n.isAdvanced);
            rw.Int32(ref n.catalogPosition);
            if (Version >= 1)
            {
                rw.EnumByte<EProdState>(ref n.prodState);
            }
        }
    }

    /// <summary>
    /// CGameCtnCollector 0x012 chunk
    /// </summary>
    [Chunk(0x2E001012)]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk2E001012 : Chunk<CGameCtnCollector>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E001012;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

        public int U01;
        public int U02;
        public int U03;
        public int U04;

        public override void ReadWrite(CGameCtnCollector n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
        }
    }



    public enum ESkinKind
    {
        None,
        Text,
        Image,
        Vehicle,
    }

    public enum EProdState
    {
        Aborted,
        GameBox,
        DevBuild,
        Release,
    }

    public enum ECollectorFlags
    {
        None,
        UnknownValue,
        IsInternal,
        IsAdvanced = 4,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x2E001002 => new Chunk2E001002(),
        0x2E001006 => new Chunk2E001006(),
        0x2E001007 => new Chunk2E001007(),
        0x2E001008 => new Chunk2E001008(),
        0x2E001009 => new Chunk2E001009(),
        0x2E00100A => new Chunk2E00100A(),
        0x2E00100B => new Chunk2E00100B(),
        0x2E00100C => new Chunk2E00100C(),
        0x2E00100D => new Chunk2E00100D(),
        0x2E00100E => new Chunk2E00100E(),
        0x2E001010 => new Chunk2E001010(),
        0x2E001011 => new Chunk2E001011(),
        0x2E001012 => new Chunk2E001012(),
        _ => base.NewChunk(chunkId),
    };
}
