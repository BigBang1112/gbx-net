namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0304E000</remarks>
[Class(0x0304E000)]
public abstract partial class CGameCtnBlockInfo : CGameCtnCollector, IClass
{
    [Hexadecimal] public static new uint Id => 0x0304E000;




    private bool isPillar;
    [AppliedWithChunk<Chunk0304E009>]
    [AppliedWithChunk<Chunk0304E02F>]
    public bool IsPillar { get => isPillar; set => isPillar = value; }

    private CSceneMobil? groundHelperMobil;
    [AppliedWithChunk<Chunk0304E00B>]
    [AppliedWithChunk<Chunk0304E00E>]
    public CSceneMobil? GroundHelperMobil { get => groundHelperMobil; set => groundHelperMobil = value; }

    private CSceneMobil? airHelperMobil;
    [AppliedWithChunk<Chunk0304E00B>]
    [AppliedWithChunk<Chunk0304E00E>]
    public CSceneMobil? AirHelperMobil { get => airHelperMobil; set => airHelperMobil = value; }

    private CSceneMobil? constructionModeHelperMobil;
    [AppliedWithChunk<Chunk0304E00B>]
    [AppliedWithChunk<Chunk0304E00E>]
    public CSceneMobil? ConstructionModeHelperMobil { get => constructionModeHelperMobil; set => constructionModeHelperMobil = value; }

    private Iso4? spawnLocGround;
    [AppliedWithChunk<Chunk0304E00C>]
    public Iso4? SpawnLocGround { get => spawnLocGround; set => spawnLocGround = value; }

    private Iso4? spawnLocAir;
    [AppliedWithChunk<Chunk0304E00C>]
    public Iso4? SpawnLocAir { get => spawnLocAir; set => spawnLocAir = value; }

    private bool isReplacement;
    [AppliedWithChunk<Chunk0304E00D>]
    public bool IsReplacement { get => isReplacement; set => isReplacement = value; }

    private EWayPointType wayPointType;
    [AppliedWithChunk<Chunk0304E00E>]
    [AppliedWithChunk<Chunk0304E026>]
    public EWayPointType WayPointType { get => wayPointType; set => wayPointType = value; }

    private bool noRespawn;
    [AppliedWithChunk<Chunk0304E00F>]
    public bool NoRespawn { get => noRespawn; set => noRespawn = value; }

    private bool iconAutoUseGround;
    [AppliedWithChunk<Chunk0304E013>]
    public bool IconAutoUseGround { get => iconAutoUseGround; set => iconAutoUseGround = value; }

    private CPlugCharPhySpecialProperty? charPhySpecialProperty;
    [AppliedWithChunk<Chunk0304E020>]
    public CPlugCharPhySpecialProperty? CharPhySpecialProperty { get => charPhySpecialProperty; set => charPhySpecialProperty = value; }

    private CMwNod? podiumInfo;
    /// <summary>
    /// CGamePodiumInfo or CPlugMediaClipList
    /// </summary>
    [AppliedWithChunk<Chunk0304E020>]
    public CMwNod? PodiumInfo { get => podiumInfo; set => podiumInfo = value; }

    private CMwNod? introInfo;
    /// <summary>
    /// CGamePodiumInfo or CPlugMediaClipList
    /// </summary>
    [AppliedWithChunk<Chunk0304E020>]
    public CMwNod? IntroInfo { get => introInfo; set => introInfo = value; }

    private bool charPhySpecialPropertyCustomizable;
    [AppliedWithChunk<Chunk0304E020>]
    public bool CharPhySpecialPropertyCustomizable { get => charPhySpecialPropertyCustomizable; set => charPhySpecialPropertyCustomizable = value; }

    private CGameCtnBlockInfoVariantGround? variantBaseGround;
    [AppliedWithChunk<Chunk0304E023>]
    public CGameCtnBlockInfoVariantGround? VariantBaseGround { get => variantBaseGround; set => variantBaseGround = value; }

    private CGameCtnBlockInfoVariantAir? variantBaseAir;
    [AppliedWithChunk<Chunk0304E023>]
    public CGameCtnBlockInfoVariantAir? VariantBaseAir { get => variantBaseAir; set => variantBaseAir = value; }

    private CGameCtnBlockInfoVariantGround[]? additionalVariantsGround;
    [AppliedWithChunk<Chunk0304E027>]
    public CGameCtnBlockInfoVariantGround[]? AdditionalVariantsGround { get => additionalVariantsGround; set => additionalVariantsGround = value; }

    private string? symmetricalBlockInfoId;
    [AppliedWithChunk<Chunk0304E028>]
    public string? SymmetricalBlockInfoId { get => symmetricalBlockInfoId; set => symmetricalBlockInfoId = value; }

    private Direction dir;
    [AppliedWithChunk<Chunk0304E028>]
    public Direction Dir { get => dir; set => dir = value; }

    private CPlugFogVolumeBox? fogVolumeBox;
    [AppliedWithChunk<Chunk0304E029>]
    public CPlugFogVolumeBox? FogVolumeBox { get => fogVolumeBoxFile?.GetNode(ref fogVolumeBox) ?? fogVolumeBox; set => fogVolumeBox = value; }
    private Components.GbxRefTableFile? fogVolumeBoxFile;
    public Components.GbxRefTableFile? FogVolumeBoxFile { get => fogVolumeBoxFile; set => fogVolumeBoxFile = value; }
    public CPlugFogVolumeBox? GetFogVolumeBox(GbxReadSettings settings = default, bool exceptions = false) => fogVolumeBoxFile?.GetNode(ref fogVolumeBox, settings, exceptions) ?? fogVolumeBox;

    private CPlugSound? sound1;
    [AppliedWithChunk<Chunk0304E02A>]
    public CPlugSound? Sound1 { get => sound1File?.GetNode(ref sound1) ?? sound1; set => sound1 = value; }
    private Components.GbxRefTableFile? sound1File;
    public Components.GbxRefTableFile? Sound1File { get => sound1File; set => sound1File = value; }
    public CPlugSound? GetSound1(GbxReadSettings settings = default, bool exceptions = false) => sound1File?.GetNode(ref sound1, settings, exceptions) ?? sound1;

    private CPlugSound? sound2;
    [AppliedWithChunk<Chunk0304E02A>]
    public CPlugSound? Sound2 { get => sound2File?.GetNode(ref sound2) ?? sound2; set => sound2 = value; }
    private Components.GbxRefTableFile? sound2File;
    public Components.GbxRefTableFile? Sound2File { get => sound2File; set => sound2File = value; }
    public CPlugSound? GetSound2(GbxReadSettings settings = default, bool exceptions = false) => sound2File?.GetNode(ref sound2, settings, exceptions) ?? sound2;

    private Iso4 sound1Loc;
    [AppliedWithChunk<Chunk0304E02A>]
    [AppliedWithChunk<Chunk0304E02A>]
    public Iso4 Sound1Loc { get => sound1Loc; set => sound1Loc = value; }

    private Iso4 sound2Loc;
    [AppliedWithChunk<Chunk0304E02A>]
    [AppliedWithChunk<Chunk0304E02A>]
    public Iso4 Sound2Loc { get => sound2Loc; set => sound2Loc = value; }

    private EBaseType baseType;
    [AppliedWithChunk<Chunk0304E02B>]
    public EBaseType BaseType { get => baseType; set => baseType = value; }

    private CGameCtnBlockInfoVariantAir[]? additionalVariantsAir;
    [AppliedWithChunk<Chunk0304E02C>]
    public CGameCtnBlockInfoVariantAir[]? AdditionalVariantsAir { get => additionalVariantsAir; set => additionalVariantsAir = value; }

    private EMultiDir pillarShapeMultiDir;
    [AppliedWithChunk<Chunk0304E02F>]
    public EMultiDir PillarShapeMultiDir { get => pillarShapeMultiDir; set => pillarShapeMultiDir = value; }

    private CPlugGameSkinAndFolder? materialModifier;
    [AppliedWithChunk<Chunk0304E031>]
    public CPlugGameSkinAndFolder? MaterialModifier { get => materialModifierFile?.GetNode(ref materialModifier) ?? materialModifier; set => materialModifier = value; }
    private Components.GbxRefTableFile? materialModifierFile;
    public Components.GbxRefTableFile? MaterialModifierFile { get => materialModifierFile; set => materialModifierFile = value; }
    public CPlugGameSkinAndFolder? GetMaterialModifier(GbxReadSettings settings = default, bool exceptions = false) => materialModifierFile?.GetNode(ref materialModifier, settings, exceptions) ?? materialModifier;

    private CPlugGameSkinAndFolder? materialModifier2;
    /// <summary>
    /// not verified
    /// </summary>
    [AppliedWithChunk<Chunk0304E031>]
    public CPlugGameSkinAndFolder? MaterialModifier2 { get => materialModifier2File?.GetNode(ref materialModifier2) ?? materialModifier2; set => materialModifier2 = value; }
    private Components.GbxRefTableFile? materialModifier2File;
    public Components.GbxRefTableFile? MaterialModifier2File { get => materialModifier2File; set => materialModifier2File = value; }
    public CPlugGameSkinAndFolder? GetMaterialModifier2(GbxReadSettings settings = default, bool exceptions = false) => materialModifier2File?.GetNode(ref materialModifier2, settings, exceptions) ?? materialModifier2;


    /// <summary>
    /// CGameCtnBlockInfo 0x005 chunk
    /// </summary>
    [Chunk(0x0304E005)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF)]
    public partial class Chunk0304E005 : Chunk<CGameCtnBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E005;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF;

    }

    /// <summary>
    /// CGameCtnBlockInfo 0x009 chunk
    /// </summary>
    [Chunk(0x0304E009)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT)]
    public partial class Chunk0304E009 : Chunk<CGameCtnBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E009;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT;


        public override void ReadWrite(CGameCtnBlockInfo n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isPillar);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfo 0x00B chunk
    /// </summary>
    [Chunk(0x0304E00B)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX)]
    public partial class Chunk0304E00B : Chunk<CGameCtnBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E00B;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX;

        public int U01;

        public override void ReadWrite(CGameCtnBlockInfo n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.NodeRef<CSceneMobil>(ref n.groundHelperMobil);
            rw.NodeRef<CSceneMobil>(ref n.airHelperMobil);
            rw.NodeRef<CSceneMobil>(ref n.constructionModeHelperMobil);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfo 0x00C chunk
    /// </summary>
    [Chunk(0x0304E00C)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF)]
    public partial class Chunk0304E00C : Chunk<CGameCtnBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E00C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMF;


        public override void ReadWrite(CGameCtnBlockInfo n, GbxReaderWriter rw)
        {
            rw.Iso4(ref n.spawnLocGround);
            rw.Iso4(ref n.spawnLocAir);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfo 0x00D chunk
    /// </summary>
    [Chunk(0x0304E00D)]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMF)]
    public partial class Chunk0304E00D : Chunk<CGameCtnBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E00D;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMF;


        public override void ReadWrite(CGameCtnBlockInfo n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isReplacement);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfo 0x00E chunk
    /// </summary>
    [Chunk(0x0304E00E)]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk0304E00E : Chunk<CGameCtnBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E00E;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;


        public override void ReadWrite(CGameCtnBlockInfo n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EWayPointType>(ref n.wayPointType);
            rw.NodeRef<CSceneMobil>(ref n.groundHelperMobil);
            rw.NodeRef<CSceneMobil>(ref n.airHelperMobil);
            rw.NodeRef<CSceneMobil>(ref n.constructionModeHelperMobil);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfo 0x00F chunk
    /// </summary>
    [Chunk(0x0304E00F)]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0304E00F : Chunk<CGameCtnBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E00F;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;


        public override void ReadWrite(CGameCtnBlockInfo n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.noRespawn);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfo 0x013 chunk
    /// </summary>
    [Chunk(0x0304E013)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0304E013 : Chunk<CGameCtnBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E013;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;


        public override void ReadWrite(CGameCtnBlockInfo n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.iconAutoUseGround);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfo 0x015 chunk
    /// </summary>
    [Chunk(0x0304E015)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0304E015 : Chunk<CGameCtnBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E015;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;

        /// <summary>
        /// node ref
        /// </summary>
        public int U01;
        public Iso4 U02;

        public override void ReadWrite(CGameCtnBlockInfo n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01); // node ref
            rw.Iso4(ref U02);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfo 0x017 chunk
    /// </summary>
    [Chunk(0x0304E017)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0304E017 : Chunk<CGameCtnBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E017;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;

        public bool U01;

        public override void ReadWrite(CGameCtnBlockInfo n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfo 0x020 chunk
    /// </summary>
    [Chunk(0x0304E020)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4, 5, 5, 6)]
    public partial class Chunk0304E020 : Chunk<CGameCtnBlockInfo>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E020;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;

        public int Version { get; set; }

        public CGameWaypointSpecialProperty? U01;
        public bool U02;
        public bool U03;
        /// <summary>
        /// MatModifier
        /// </summary>
        public string? U04;
        /// <summary>
        /// Grass
        /// </summary>
        public string? U05;

        public override void ReadWrite(CGameCtnBlockInfo n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugCharPhySpecialProperty>(ref n.charPhySpecialProperty);
            if (Version <= 6)
            {
                rw.NodeRef<CGameWaypointSpecialProperty>(ref U01);
            }
            if (Version >= 2)
            {
                rw.NodeRef<CMwNod>(ref n.podiumInfo); // CGamePodiumInfo or CPlugMediaClipList
                if (Version >= 3)
                {
                    rw.NodeRef<CMwNod>(ref n.introInfo); // CGamePodiumInfo or CPlugMediaClipList
                    if (Version >= 4)
                    {
                        rw.Boolean(ref n.charPhySpecialPropertyCustomizable);
                        if (Version == 5)
                        {
                            rw.Boolean(ref U02);
                        }
                        if (Version >= 8)
                        {
                            rw.Boolean(ref U03);
                            if (U03)
                            {
                                rw.String(ref U04); // MatModifier
                                rw.String(ref U05); // Grass
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CGameCtnBlockInfo 0x023 chunk
    /// </summary>
    [Chunk(0x0304E023)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0304E023 : Chunk<CGameCtnBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E023;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;


        public override void ReadWrite(CGameCtnBlockInfo n, GbxReaderWriter rw)
        {
            rw.Node<CGameCtnBlockInfoVariantGround>(ref n.variantBaseGround);
            rw.Node<CGameCtnBlockInfoVariantAir>(ref n.variantBaseAir);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfo 0x026 chunk
    /// </summary>
    [Chunk(0x0304E026)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0304E026 : Chunk<CGameCtnBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E026;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;


        public override void ReadWrite(CGameCtnBlockInfo n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EWayPointType>(ref n.wayPointType);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfo 0x027 chunk
    /// </summary>
    [Chunk(0x0304E027)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0304E027 : Chunk<CGameCtnBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E027;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;


        public override void ReadWrite(CGameCtnBlockInfo n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CGameCtnBlockInfoVariantGround>(ref n.additionalVariantsGround!);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfo 0x028 chunk
    /// </summary>
    [Chunk(0x0304E028)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0304E028 : Chunk<CGameCtnBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E028;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;


        public override void ReadWrite(CGameCtnBlockInfo n, GbxReaderWriter rw)
        {
            rw.Id(ref n.symmetricalBlockInfoId);
            rw.EnumInt32<Direction>(ref n.dir);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfo 0x029 chunk
    /// </summary>
    [Chunk(0x0304E029)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0304E029 : Chunk<CGameCtnBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E029;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;


        public override void ReadWrite(CGameCtnBlockInfo n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugFogVolumeBox>(ref n.fogVolumeBox, ref n.fogVolumeBoxFile);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfo 0x02A chunk
    /// </summary>
    [Chunk(0x0304E02A)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4, 1, 1, 2)]
    public partial class Chunk0304E02A : Chunk<CGameCtnBlockInfo>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E02A;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnBlockInfo n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugSound>(ref n.sound1, ref n.sound1File);
            rw.NodeRef<CPlugSound>(ref n.sound2, ref n.sound2File);
            if (Version <= 2)
            {
                rw.Iso4(ref n.sound1Loc);
                rw.Iso4(ref n.sound2Loc);
            }
            if (Version >= 3)
            {
                if (n.Sound1!=null||n.Sound1File!=null)
                {
                    rw.Iso4(ref n.sound1Loc);
                }
                if (n.Sound2!=null||n.Sound2File!=null)
                {
                    rw.Iso4(ref n.sound2Loc);
                }
            }
        }
    }

    /// <summary>
    /// CGameCtnBlockInfo 0x02B chunk
    /// </summary>
    [Chunk(0x0304E02B)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4, 1, 1, 1)]
    public partial class Chunk0304E02B : Chunk<CGameCtnBlockInfo>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E02B;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnBlockInfo n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.EnumInt32<EBaseType>(ref n.baseType);
            if (Version == 0)
            {
                throw new ("");
            }
        }
    }

    /// <summary>
    /// CGameCtnBlockInfo 0x02C chunk
    /// </summary>
    [Chunk(0x0304E02C)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0304E02C : Chunk<CGameCtnBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E02C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;


        public override void ReadWrite(CGameCtnBlockInfo n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CGameCtnBlockInfoVariantAir>(ref n.additionalVariantsAir!);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfo 0x02E chunk
    /// </summary>
    [Chunk(0x0304E02E)]
    [ChunkGameVersion(GameVersion.MP3)]
    public partial class Chunk0304E02E : Chunk<CGameCtnBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E02E;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3;

    }

    /// <summary>
    /// CGameCtnBlockInfo 0x02F chunk
    /// </summary>
    [Chunk(0x0304E02F)]
    [ChunkGameVersion(GameVersion.MP4, 0)]
    public partial class Chunk0304E02F : Chunk<CGameCtnBlockInfo>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E02F;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4;

        public int Version { get; set; }

        public byte U01;

        public override void ReadWrite(CGameCtnBlockInfo n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Boolean(ref n.isPillar, asByte: true);
            rw.EnumByte<EMultiDir>(ref n.pillarShapeMultiDir);
            if (Version >= 1)
            {
                rw.Byte(ref U01);
            }
        }
    }

    /// <summary>
    /// CGameCtnBlockInfo 0x031 chunk
    /// </summary>
    [Chunk(0x0304E031)]
    public partial class Chunk0304E031 : Chunk<CGameCtnBlockInfo>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0304E031;

        public int Version { get; set; }

        public CMwNod? U01;
        public Components.GbxRefTableFile? U01File;

        public override void ReadWrite(CGameCtnBlockInfo n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CMwNod>(ref U01, ref U01File);
            rw.NodeRef<CPlugGameSkinAndFolder>(ref n.materialModifier, ref n.materialModifierFile);
            if (Version >= 1)
            {
                rw.NodeRef<CPlugGameSkinAndFolder>(ref n.materialModifier2, ref n.materialModifier2File); // not verified
            }
        }
    }



    public enum EMultiDir
    {
        SameDir,
        SymmetricalDirs,
        AllDir,
        OpposedDirOnly,
        PerpendicularDirsOnly,
        NextDirOnly,
        PreviousDirOnly,
    }

    public enum ESelection
    {
        Random,
        Obsolete,
        AutoRotate,
    }

    public enum EWayPointType
    {
        Start,
        Finish,
        Checkpoint,
        None,
        StartFinish,
        Dispenser,
    }

    public enum EBaseType
    {
        None,
        Conductor,
        Generator,
        Collector,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0304E005 => new Chunk0304E005(),
        0x0304E009 => new Chunk0304E009(),
        0x0304E00B => new Chunk0304E00B(),
        0x0304E00C => new Chunk0304E00C(),
        0x0304E00D => new Chunk0304E00D(),
        0x0304E00E => new Chunk0304E00E(),
        0x0304E00F => new Chunk0304E00F(),
        0x0304E013 => new Chunk0304E013(),
        0x0304E015 => new Chunk0304E015(),
        0x0304E017 => new Chunk0304E017(),
        0x0304E020 => new Chunk0304E020(),
        0x0304E023 => new Chunk0304E023(),
        0x0304E026 => new Chunk0304E026(),
        0x0304E027 => new Chunk0304E027(),
        0x0304E028 => new Chunk0304E028(),
        0x0304E029 => new Chunk0304E029(),
        0x0304E02A => new Chunk0304E02A(),
        0x0304E02B => new Chunk0304E02B(),
        0x0304E02C => new Chunk0304E02C(),
        0x0304E02E => new Chunk0304E02E(),
        0x0304E02F => new Chunk0304E02F(),
        0x0304E031 => new Chunk0304E031(),
        _ => base.NewChunk(chunkId),
    };
}
