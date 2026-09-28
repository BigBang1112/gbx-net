namespace GBX.NET.Engines.Game;

/// <summary>
/// A map.
/// </summary>
/// <remarks>ID: 0x03043000</remarks>
[Class(0x03043000)]
public partial class CGameCtnChallenge : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03043000;




    private bool needUnlock;
    [AppliedWithChunk<HeaderChunk03043002>]
    [AppliedWithChunk<Chunk0304300F>]
    public bool NeedUnlock { get => needUnlock; set => needUnlock = value; }

    private int cost;
    [AppliedWithChunk<HeaderChunk03043002>]
    public int Cost { get => cost; set => cost = value; }

    private bool isLapRace;
    [AppliedWithChunk<HeaderChunk03043002>]
    [AppliedWithChunk<Chunk03043018>]
    public bool IsLapRace { get => isLapRace; set => isLapRace = value; }

    private PlayMode mode;
    [AppliedWithChunk<HeaderChunk03043002>]
    [AppliedWithChunk<Chunk0304301C>]
    public PlayMode Mode { get => mode; set => mode = value; }

    private bool hasClones;
    [AppliedWithChunk<HeaderChunk03043002>]
    public bool HasClones { get => hasClones; set => hasClones = value; }

    private EditorMode editor;
    [AppliedWithChunk<HeaderChunk03043002>]
    public EditorMode Editor { get => editor; set => editor = value; }

    private int nbCheckpoints;
    [AppliedWithChunk<HeaderChunk03043002>]
    public int NbCheckpoints { get => nbCheckpoints; set => nbCheckpoints = value; }

    private int nbLaps;
    [AppliedWithChunk<HeaderChunk03043002>]
    [AppliedWithChunk<Chunk03043018>]
    public int NbLaps { get => nbLaps; set => nbLaps = value; }

    private MapKind kindInHeader;
    [AppliedWithChunk<HeaderChunk03043003>]
    public MapKind KindInHeader { get => kindInHeader; set => kindInHeader = value; }

    private string? password;
    [AppliedWithChunk<HeaderChunk03043003>]
    [AppliedWithChunk<Chunk03043014>]
    public string? Password { get => password; set => password = value; }

    private Vec2 mapCoordOrigin;
    [AppliedWithChunk<HeaderChunk03043003>]
    [AppliedWithChunk<Chunk03043023>]
    [AppliedWithChunk<Chunk03043025>]
    public Vec2 MapCoordOrigin { get => mapCoordOrigin; set => mapCoordOrigin = value; }

    private Vec2 mapCoordTarget;
    [AppliedWithChunk<HeaderChunk03043003>]
    [AppliedWithChunk<Chunk03043025>]
    public Vec2 MapCoordTarget { get => mapCoordTarget; set => mapCoordTarget = value; }

    private UInt128 packMask;
    [AppliedWithChunk<HeaderChunk03043003>]
    public UInt128 PackMask { get => packMask; set => packMask = value; }

    private ulong lightmapCacheUid;
    [AppliedWithChunk<HeaderChunk03043003>]
    public ulong LightmapCacheUid { get => lightmapCacheUid; set => lightmapCacheUid = value; }

    private string? titleId;
    [AppliedWithChunk<HeaderChunk03043003>]
    [AppliedWithChunk<Chunk03043051>]
    public string? TitleId { get => titleId; set => titleId = value; }

    private string? xml;
    [AppliedWithChunk<HeaderChunk03043005>]
    public string? Xml { get => xml; set => xml = value; }

    private int authorVersion;
    [AppliedWithChunk<HeaderChunk03043008>]
    [AppliedWithChunk<Chunk03043042>]
    public int AuthorVersion { get => authorVersion; set => authorVersion = value; }

    private string? authorZone;
    [AppliedWithChunk<HeaderChunk03043008>]
    [AppliedWithChunk<Chunk03043042>]
    public string? AuthorZone { get => authorZone; set => authorZone = value; }

    private string? authorExtraInfo;
    [AppliedWithChunk<HeaderChunk03043008>]
    [AppliedWithChunk<Chunk03043042>]
    public string? AuthorExtraInfo { get => authorExtraInfo; set => authorExtraInfo = value; }

    private Ident? playerModel;
    [AppliedWithChunk<Chunk0304300D>]
    public Ident? PlayerModel { get => playerModel; set => playerModel = value; }

    private CGameCtnCollectorList? blockStock;
    [AppliedWithChunk<Chunk03043011>]
    public CGameCtnCollectorList? BlockStock { get => blockStock; set => blockStock = value; }

    private CGameCtnChallengeParameters? challengeParameters;
    [AppliedWithChunk<Chunk03043011>]
    public CGameCtnChallengeParameters? ChallengeParameters { get => challengeParameters; set => challengeParameters = value; }

    private MapKind kind;
    [AppliedWithChunk<Chunk03043011>]
    public MapKind Kind { get => kind; set => kind = value; }

    private List<Int3>? checkpoints;
    [AppliedWithChunk<Chunk03043017>]
    public List<Int3>? Checkpoints { get => checkpoints; set => checkpoints = value; }

    private PackDesc? modPackDesc;
    [AppliedWithChunk<Chunk03043019>]
    public PackDesc? ModPackDesc { get => modPackDesc; set => modPackDesc = value; }

    private CGameCtnMediaClip? clipIntro;
    [AppliedWithChunk<Chunk03043020>]
    [AppliedWithChunk<Chunk03043021>]
    [AppliedWithChunk<Chunk03043049>]
    public CGameCtnMediaClip? ClipIntro { get => clipIntro; set => clipIntro = value; }

    private CGameCtnMediaClipGroup? clipGroupInGame;
    [AppliedWithChunk<Chunk03043020>]
    [AppliedWithChunk<Chunk03043021>]
    [AppliedWithChunk<Chunk03043049>]
    public CGameCtnMediaClipGroup? ClipGroupInGame { get => clipGroupInGame; set => clipGroupInGame = value; }

    private CGameCtnMediaClipGroup? clipGroupEndRace;
    [AppliedWithChunk<Chunk03043020>]
    [AppliedWithChunk<Chunk03043021>]
    [AppliedWithChunk<Chunk03043049>]
    public CGameCtnMediaClipGroup? ClipGroupEndRace { get => clipGroupEndRace; set => clipGroupEndRace = value; }

    private PackDesc? customMusicPackDesc;
    [AppliedWithChunk<Chunk03043024>]
    public PackDesc? CustomMusicPackDesc { get => customMusicPackDesc; set => customMusicPackDesc = value; }

    private CGameCtnMediaClip? clipGlobal;
    [AppliedWithChunk<Chunk03043026>]
    public CGameCtnMediaClip? ClipGlobal { get => clipGlobal; set => clipGlobal = value; }

    private Mat3? thumbnailRotationMatrix;
    [AppliedWithChunk<Chunk03043027>]
    [AppliedWithChunk<Chunk03043028>]
    public Mat3? ThumbnailRotationMatrix { get => thumbnailRotationMatrix; set => thumbnailRotationMatrix = value; }

    private uint crc32;
    [AppliedWithChunk<Chunk03043029>]
    public uint Crc32 { get => crc32; set => crc32 = value; }

    private byte[]? challengeDecals;
    [AppliedWithChunk<Chunk03043034>]
    public byte[]? ChallengeDecals { get => challengeDecals; set => challengeDecals = value; }

    private SChallengeCardEventIds[]? challengeCardEventIds;
    [AppliedWithChunk<Chunk03043038>]
    public SChallengeCardEventIds[]? ChallengeCardEventIds { get => challengeCardEventIds; set => challengeCardEventIds = value; }

    private CSceneVehicleCarMarksSamples[]? carMarksBuffer;
    [AppliedWithChunk<Chunk0304303E>]
    public CSceneVehicleCarMarksSamples[]? CarMarksBuffer { get => carMarksBuffer; set => carMarksBuffer = value; }

    private CGameCtnMediaClip? clipPodium;
    [AppliedWithChunk<Chunk03043049>]
    public CGameCtnMediaClip? ClipPodium { get => clipPodium; set => clipPodium = value; }

    private CGameCtnMediaClip? clipAmbiance;
    [AppliedWithChunk<Chunk03043049>]
    public CGameCtnMediaClip? ClipAmbiance { get => clipAmbiance; set => clipAmbiance = value; }

    private Int3 clipTriggerSize = (1, 1, 1);
    [AppliedWithChunk<Chunk03043049>]
    public Int3 ClipTriggerSize { get => clipTriggerSize; set => clipTriggerSize = value; }

    private string? objectiveTextAuthor;
    [AppliedWithChunk<Chunk0304304B>]
    public string? ObjectiveTextAuthor { get => objectiveTextAuthor; set => objectiveTextAuthor = value; }

    private string? objectiveTextGold;
    [AppliedWithChunk<Chunk0304304B>]
    public string? ObjectiveTextGold { get => objectiveTextGold; set => objectiveTextGold = value; }

    private string? objectiveTextSilver;
    [AppliedWithChunk<Chunk0304304B>]
    public string? ObjectiveTextSilver { get => objectiveTextSilver; set => objectiveTextSilver = value; }

    private string? objectiveTextBronze;
    [AppliedWithChunk<Chunk0304304B>]
    public string? ObjectiveTextBronze { get => objectiveTextBronze; set => objectiveTextBronze = value; }

    private Int3 offzoneTriggerSize;
    [AppliedWithChunk<Chunk03043050>]
    public Int3 OffzoneTriggerSize { get => offzoneTriggerSize; set => offzoneTriggerSize = value; }

    private List<BoxInt3>? offzones;
    [AppliedWithChunk<Chunk03043050>]
    public List<BoxInt3>? Offzones { get => offzones; set => offzones = value; }

    private string? buildVersion;
    [AppliedWithChunk<Chunk03043051>]
    public string? BuildVersion { get => buildVersion; set => buildVersion = value; }

    private int decoBaseHeightOffset;
    [AppliedWithChunk<Chunk03043052>]
    public int DecoBaseHeightOffset { get => decoBaseHeightOffset; set => decoBaseHeightOffset = value; }

    private List<BotPath>? botPaths;
    [AppliedWithChunk<Chunk03043053>]
    public List<BotPath>? BotPaths { get => botPaths; set => botPaths = value; }

    private TimeSpan? dayTime;
    [AppliedWithChunk<Chunk03043056>]
    [AppliedWithChunk<Chunk0304306B>]
    public TimeSpan? DayTime { get => dayTime; set => dayTime = value; }

    private bool dynamicDaylight;
    [AppliedWithChunk<Chunk03043056>]
    [AppliedWithChunk<Chunk0304306B>]
    public bool DynamicDaylight { get => dynamicDaylight; set => dynamicDaylight = value; }

    private TimeInt32? dayDuration;
    [AppliedWithChunk<Chunk03043056>]
    [AppliedWithChunk<Chunk0304306B>]
    public TimeInt32? DayDuration { get => dayDuration; set => dayDuration = value; }

    private Vec3 worldDistortion;
    [AppliedWithChunk<Chunk03043059>]
    public Vec3 WorldDistortion { get => worldDistortion; set => worldDistortion = value; }

    private PaletteColor palette;
    [AppliedWithChunk<Chunk0304306C>]
    public PaletteColor Palette { get => palette; set => palette = value; }

    /// <summary>
    /// [SHeaderTMDesc] CGameCtnChallenge 0x002 header chunk (description)
    /// </summary>
    [Chunk(0x03043002, "description")]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMPU | GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020, 3, 4, 7, 7, 10, 10, 10, 11, 13, 13, 13, 13)]
    public partial class HeaderChunk03043002 : HeaderChunk<CGameCtnChallenge>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03043002;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMPU | GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }

        public byte U02;
        public int U05;
    }

    /// <summary>
    /// [SHeaderCommon] CGameCtnChallenge 0x003 header chunk (common)
    /// </summary>
    [Chunk(0x03043003, "common")]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMPU | GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020, 0, 1, 1, 1, 4, 4, 5, 5, 11, 11, 11, 11)]
    public partial class HeaderChunk03043003 : HeaderChunk<CGameCtnChallenge>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03043003;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMPU | GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }

        public uint U01;
        public int U02;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionByte(this);
            rw.Ident(ref n.mapInfo);
            rw.String(ref n.mapName);
            rw.EnumByte<MapKind>(ref n.kindInHeader);
            if (Version >= 1)
            {
                rw.UInt32(ref U01);
                rw.String(ref n.password);
                if (Version >= 2)
                {
                    rw.Ident(ref n.decoration);
                    if (Version >= 3)
                    {
                        rw.Vec2(ref n.mapCoordOrigin);
                        if (Version >= 4)
                        {
                            rw.Vec2(ref n.mapCoordTarget);
                            if (Version >= 5)
                            {
                                rw.UInt128(ref n.packMask);
                                if (Version >= 6)
                                {
                                    rw.String(ref n.mapType);
                                    rw.String(ref n.mapStyle);
                                    if (Version <= 8)
                                    {
                                        rw.Int32(ref U02);
                                    }
                                    if (Version >= 8)
                                    {
                                        rw.UInt64(ref n.lightmapCacheUid);
                                        if (Version >= 9)
                                        {
                                            n.LightmapVersion = rw.Byte(n.LightmapVersion);
                                            if (Version >= 11)
                                            {
                                                rw.Id(ref n.titleId);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// [SHeaderVersion] CGameCtnChallenge 0x004 header chunk (version)
    /// </summary>
    [Chunk(0x03043004, "version")]
    [ChunkGameVersion(GameVersion.TMPU | GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020, 2, 4, 4, 5, 5, 6, 6, 6, 6, 6, 6)]
    public partial class HeaderChunk03043004 : HeaderChunk<CGameCtnChallenge>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03043004;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMPU | GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
        }
    }

    /// <summary>
    /// [SHeaderCommunity] CGameCtnChallenge 0x005 header chunk (xml)
    /// </summary>
    [Chunk(0x03043005, "xml")]
    [ChunkGameVersion(GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class HeaderChunk03043005 : HeaderChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043005;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.String(ref n.xml);
        }
    }

    /// <summary>
    /// [SHeaderThumbnail] CGameCtnChallenge 0x007 header chunk (thumbnail)
    /// </summary>
    [Chunk(0x03043007, "thumbnail")]
    [ChunkGameVersion(GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class HeaderChunk03043007 : HeaderChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043007;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// [SHeaderAuthorInfo] CGameCtnChallenge 0x008 header chunk (author info)
    /// </summary>
    [Chunk(0x03043008, "author info")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class HeaderChunk03043008 : HeaderChunk<CGameCtnChallenge>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03043008;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref n.authorVersion);
            rw.String(ref n.authorLogin);
            rw.String(ref n.authorNickname);
            rw.String(ref n.authorZone);
            rw.String(ref n.authorExtraInfo);
        }
    }


    /// <summary>
    /// CGameCtnChallenge 0x00D chunk (vehicle)
    /// </summary>
    [Chunk(0x0304300D, "vehicle")]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMPU | GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0304300D : Chunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304300D;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMPU | GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.Ident(ref n.playerModel);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x00F chunk (TM1.0 block data)
    /// </summary>
    [Chunk(0x0304300F, "TM1.0 block data")]
    [ChunkGameVersion(GameVersion.TM10)]
    public partial class Chunk0304300F : Chunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304300F;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.Ident(ref n.mapInfo);
            rw.Int3(ref n.size);
            rw.ListNodeRef_deprec<CGameCtnBlock>(ref n.blocks!);
            rw.Boolean(ref n.needUnlock);
            rw.Ident(ref n.decoration);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x011 chunk (parameters)
    /// </summary>
    [Chunk(0x03043011, "parameters")]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMPU | GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043011 : Chunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043011;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMPU | GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnCollectorList>(ref n.blockStock);
            rw.NodeRef<CGameCtnChallengeParameters>(ref n.challengeParameters);
            rw.EnumInt32<MapKind>(ref n.kind);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x012 chunk (TM1.0 map name)
    /// </summary>
    [Chunk(0x03043012, "TM1.0 map name")]
    [ChunkGameVersion(GameVersion.TM10)]
    public partial class Chunk03043012 : Chunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043012;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.String(ref n.mapName);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x013 chunk (legacy block data)
    /// </summary>
    [Chunk(0x03043013, "legacy block data")]
    [ChunkGameVersion(GameVersion.TMPU)]
    public partial class Chunk03043013 : Chunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043013;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMPU;

        public bool U01;

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            n.MapInfo = r.ReadIdent();
            n.MapName = r.ReadString();
            n.Decoration = r.ReadIdent();
            n.Size = r.ReadInt3();
            U01 = r.ReadBoolean();
            n.Blocks = r.ReadListReadable<CGameCtnBlock>();
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(n.MapInfo);
            w.Write(n.MapName);
            w.Write(n.Decoration);
            w.Write(n.Size);
            w.Write(U01);
            w.WriteListWritable<CGameCtnBlock>(n.Blocks);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x014 skippable chunk (legacy password)
    /// </summary>
    [Chunk(0x03043014, "legacy password")]
    [ChunkGameVersion(GameVersion.TMPU | GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU)]
    public partial class Chunk03043014 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043014;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMPU | GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU;

        public int U01;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.String(ref n.password);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x016 skippable chunk
    /// </summary>
    [Chunk(0x03043016)]
    [ChunkGameVersion(GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC)]
    public partial class Chunk03043016 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043016;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC;

        /// <summary>
        /// code says DoBool, likely IsPlatform (Mode == 1), but maps yield something like 1698004
        /// </summary>
        public int U01;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01); // code says DoBool, likely IsPlatform (Mode == 1), but maps yield something like 1698004
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x017 skippable chunk (checkpoints)
    /// </summary>
    [Chunk(0x03043017, "checkpoints")]
    [ChunkGameVersion(GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF)]
    public partial class Chunk03043017 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043017;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.List<Int3>(ref n.checkpoints!);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x018 skippable chunk (laps)
    /// </summary>
    [Chunk(0x03043018, "laps")]
    [ChunkGameVersion(GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043018 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043018;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isLapRace);
            rw.Int32(ref n.nbLaps);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x019 skippable chunk (mod)
    /// </summary>
    [Chunk(0x03043019, "mod")]
    [ChunkGameVersion(GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043019 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043019;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.PackDesc(ref n.modPackDesc);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x01A chunk
    /// </summary>
    [Chunk(0x0304301A)]
    public partial class Chunk0304301A : Chunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304301A;

        /// <summary>
        /// assert: '!ReplayRecord || !ReplayRecord->m_Challenge' failed.
        /// </summary>
        public CMwNod? U01;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref U01); // assert: '!ReplayRecord || !ReplayRecord->m_Challenge' failed.
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x01B chunk (OldIgs)
    /// </summary>
    [Chunk(0x0304301B, "OldIgs")]
    public partial class Chunk0304301B : Chunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304301B;

        public int U01;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            if (U01>0)
            {
                throw new NotSupportedException("SOldIgs count > 0");
            }
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x01C skippable chunk (play mode)
    /// </summary>
    [Chunk(0x0304301C, "play mode")]
    [ChunkGameVersion(GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF)]
    public partial class Chunk0304301C : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304301C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.EnumInt32<PlayMode>(ref n.mode);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x01D chunk
    /// </summary>
    [Chunk(0x0304301D)]
    public partial class Chunk0304301D : Chunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304301D;

        /// <summary>
        /// assert: '!ReplayRecord || !ReplayRecord->m_Challenge' failed.
        /// </summary>
        public CMwNod? U01;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref U01); // assert: '!ReplayRecord || !ReplayRecord->m_Challenge' failed.
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x01E chunk
    /// </summary>
    [Chunk(0x0304301E)]
    public partial class Chunk0304301E : Chunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304301E;

        /// <summary>
        /// assert: '!ReplayRecord || !ReplayRecord->m_Challenge' failed.
        /// </summary>
        public CMwNod? U01;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref U01); // assert: '!ReplayRecord || !ReplayRecord->m_Challenge' failed.
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x01F chunk (block data)
    /// </summary>
    [Chunk(0x0304301F, "block data")]
    [ChunkGameVersion(GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020, 1, 1, 1, 1, 1, 1, 6, 6, 6, 6)]
    public partial class Chunk0304301F : Chunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304301F;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMS | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x020 chunk (legacy legacy mediatracker)
    /// </summary>
    [Chunk(0x03043020, "legacy legacy mediatracker")]
    public partial class Chunk03043020 : Chunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043020;

        public CGameCtnMediaClip? U01;
        public CGameCtnMediaClip? U02;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnMediaClip>(ref n.clipIntro);
            rw.NodeRef<CGameCtnMediaClip>(ref U01);
            rw.NodeRef<CGameCtnMediaClip>(ref U02);
            rw.NodeRef<CGameCtnMediaClipGroup>(ref n.clipGroupInGame);
            rw.NodeRef<CGameCtnMediaClipGroup>(ref n.clipGroupEndRace);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x021 chunk (legacy mediatracker)
    /// </summary>
    [Chunk(0x03043021, "legacy mediatracker")]
    [ChunkGameVersion(GameVersion.TMS | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF)]
    public partial class Chunk03043021 : Chunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043021;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMS | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnMediaClip>(ref n.clipIntro);
            rw.NodeRef<CGameCtnMediaClipGroup>(ref n.clipGroupInGame);
            rw.NodeRef<CGameCtnMediaClipGroup>(ref n.clipGroupEndRace);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x022 chunk
    /// </summary>
    [Chunk(0x03043022)]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043022 : Chunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043022;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int U01 = 1;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x023 chunk (map origin)
    /// </summary>
    [Chunk(0x03043023, "map origin")]
    public partial class Chunk03043023 : Chunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043023;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.Vec2(ref n.mapCoordOrigin);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x024 chunk (music)
    /// </summary>
    [Chunk(0x03043024, "music")]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043024 : Chunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043024;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.PackDesc(ref n.customMusicPackDesc);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x025 chunk (map origin and target)
    /// </summary>
    [Chunk(0x03043025, "map origin and target")]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043025 : Chunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043025;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.Vec2(ref n.mapCoordOrigin);
            rw.Vec2(ref n.mapCoordTarget);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x026 chunk (clip global)
    /// </summary>
    [Chunk(0x03043026, "clip global")]
    [ChunkGameVersion(GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043026 : Chunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043026;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnMediaClip>(ref n.clipGlobal);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x027 chunk (old realtime thumbnail)
    /// </summary>
    [Chunk(0x03043027, "old realtime thumbnail")]
    public partial class Chunk03043027 : Chunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043027;

        public byte U01;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.hasCustomCamThumbnail);
            if (!n.HasCustomCamThumbnail)
            {
                return;
            }
            rw.Byte(ref U01);
            rw.Mat3(ref n.thumbnailRotationMatrix);
            rw.Vec3(ref n.thumbnailPosition);
            rw.Single(ref n.thumbnailFov);
            rw.Single(ref n.thumbnailNearClipPlane);
            rw.Single(ref n.thumbnailFarClipPlane);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x028 chunk (old realtime thumbnail + comments)
    /// </summary>
    [Chunk(0x03043028, "old realtime thumbnail + comments")]
    [ChunkGameVersion(GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043028 : Chunk03043027
    {
        /// <inheritdoc />
        public override uint Id => 0x03043028;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.String(ref n.comments);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x029 skippable chunk (password)
    /// </summary>
    [Chunk(0x03043029, "password")]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043029 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043029;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.Checksum128(ref n.hashedPassword);
            rw.UInt32(ref n.crc32);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x02A chunk (CreatedWithSimpleEditor)
    /// </summary>
    [Chunk(0x0304302A, "CreatedWithSimpleEditor")]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0304302A : Chunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304302A;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            n.CreatedWithSimpleEditor = rw.Boolean(n.CreatedWithSimpleEditor);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x02D skippable chunk (realtime thumbnail + comments)
    /// </summary>
    [Chunk(0x0304302D, "realtime thumbnail + comments")]
    public partial class Chunk0304302D : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304302D;

        /// <summary>
        /// always 10
        /// </summary>
        public float U01;
        /// <summary>
        /// depth? 0 or 0.02
        /// </summary>
        public float U02;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.Vec3(ref n.thumbnailPosition);
            rw.Vec3(ref n.thumbnailPitchYawRoll);
            rw.Single(ref n.thumbnailFov);
            rw.Single(ref U01); // always 10
            rw.Single(ref U02); // depth? 0 or 0.02
            rw.Single(ref n.thumbnailNearClipPlane);
            rw.Single(ref n.thumbnailFarClipPlane);
            rw.String(ref n.comments);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x034 skippable chunk (ChallengeDecals)
    /// </summary>
    [Chunk(0x03043034, "ChallengeDecals")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043034 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043034;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.Data(ref n.challengeDecals);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x036 skippable chunk (realtime thumbnail + comments)
    /// </summary>
    [Chunk(0x03043036, "realtime thumbnail + comments")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043036 : Chunk0304302D
    {
        /// <inheritdoc />
        public override uint Id => 0x03043036;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x038 skippable chunk (ChallengeCardEventIds)
    /// </summary>
    [Chunk(0x03043038, "ChallengeCardEventIds")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043038 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043038;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.ArrayReadableWritable<SChallengeCardEventIds>(ref n.challengeCardEventIds!);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x03A skippable chunk
    /// </summary>
    [Chunk(0x0304303A)]
    public partial class Chunk0304303A : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304303A;

        /// <inheritdoc />
        public override bool Ignore => true;

    }

    /// <summary>
    /// CGameCtnChallenge 0x03D skippable chunk (lightmaps)
    /// </summary>
    [Chunk(0x0304303D, "lightmaps")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0304303D : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304303D;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;

    }

    /// <summary>
    /// CGameCtnChallenge 0x03E skippable chunk (CarMarksBuffer)
    /// </summary>
    [Chunk(0x0304303E, "CarMarksBuffer")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0304303E : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0304303E;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 1)
            {
                throw new ("");
            }
            rw.ArrayNodeRef_deprec<CSceneVehicleCarMarksSamples>(ref n.carMarksBuffer!);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x040 skippable chunk (items)
    /// </summary>
    [Chunk(0x03043040, "items")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020, 2, 4, 4, 7)]
    public partial class Chunk03043040 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043040;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x041 skippable chunk
    /// </summary>
    [Chunk(0x03043041)]
    public partial class Chunk03043041 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043041;

        /// <inheritdoc />
        public override bool Ignore => true;

    }

    /// <summary>
    /// CGameCtnChallenge 0x042 skippable chunk (author)
    /// </summary>
    [Chunk(0x03043042, "author")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043042 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03043042;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; } = 1;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref n.authorVersion);
            rw.String(ref n.authorLogin);
            rw.String(ref n.authorNickname);
            rw.String(ref n.authorZone);
            rw.String(ref n.authorExtraInfo);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x043 skippable chunk (genealogies)
    /// </summary>
    [Chunk(0x03043043, "genealogies")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043043 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043043;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x044 skippable chunk (metadata)
    /// </summary>
    [Chunk(0x03043044, "metadata")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043044 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043044;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x047 skippable chunk
    /// </summary>
    [Chunk(0x03043047)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT)]
    public partial class Chunk03043047 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03043047;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT;

        public int Version { get; set; }

        public string? U01;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref U01);
            if (Version >= 1)
            {
                throw new ("");
            }
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x048 skippable chunk (baked blocks)
    /// </summary>
    [Chunk(0x03043048, "baked blocks")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043048 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043048;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x049 chunk (mediatracker)
    /// </summary>
    [Chunk(0x03043049, "mediatracker")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043049 : Chunk<CGameCtnChallenge>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03043049;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; } = 2;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CGameCtnMediaClip>(ref n.clipIntro);
            rw.NodeRef<CGameCtnMediaClip>(ref n.clipPodium);
            rw.NodeRef<CGameCtnMediaClipGroup>(ref n.clipGroupInGame);
            rw.NodeRef<CGameCtnMediaClipGroup>(ref n.clipGroupEndRace);
            if (Version >= 2)
            {
                rw.NodeRef<CGameCtnMediaClip>(ref n.clipAmbiance);
            }
            if (Version >= 1)
            {
                rw.Int3(ref n.clipTriggerSize);
            }
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x04B skippable chunk (objectives)
    /// </summary>
    [Chunk(0x0304304B, "objectives")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0304304B : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304304B;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.String(ref n.objectiveTextAuthor);
            rw.String(ref n.objectiveTextGold);
            rw.String(ref n.objectiveTextSilver);
            rw.String(ref n.objectiveTextBronze);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x04D skippable chunk
    /// </summary>
    [Chunk(0x0304304D)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0304304D : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304304D;

        /// <inheritdoc />
        public override bool Ignore => true;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;

    }

    /// <summary>
    /// CGameCtnChallenge 0x04E skippable chunk
    /// </summary>
    [Chunk(0x0304304E)]
    public partial class Chunk0304304E : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304304E;

        /// <inheritdoc />
        public override bool Ignore => true;

    }

    /// <summary>
    /// CGameCtnChallenge 0x04F skippable chunk
    /// </summary>
    [Chunk(0x0304304F)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0304304F : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304304F;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x050 skippable chunk (offzones)
    /// </summary>
    [Chunk(0x03043050, "offzones")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043050 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03043050;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int3(ref n.offzoneTriggerSize);
            rw.List<BoxInt3>(ref n.offzones!);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x051 skippable chunk (title info)
    /// </summary>
    [Chunk(0x03043051, "title info")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043051 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03043051;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref n.titleId);
            rw.String(ref n.buildVersion);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x052 skippable chunk (deco height)
    /// </summary>
    [Chunk(0x03043052, "deco height")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043052 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03043052;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref n.decoBaseHeightOffset);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x053 skippable chunk (bot paths)
    /// </summary>
    [Chunk(0x03043053, "bot paths")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043053 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03043053;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ListReadableWritable<BotPath>(ref n.botPaths!);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x054 skippable chunk (embedded objects)
    /// </summary>
    [Chunk(0x03043054, "embedded objects")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043054 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043054;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x055 skippable chunk
    /// </summary>
    [Chunk(0x03043055)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043055 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043055;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x056 skippable chunk (light settings)
    /// </summary>
    [Chunk(0x03043056, "light settings")]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043056 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03043056;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; } = 3;

        public int U01;
        public int U02;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.TimeOfDay(ref n.dayTime);
            rw.Int32(ref U02);
            rw.Boolean(ref n.dynamicDaylight);
            rw.TimeInt32Nullable(ref n.dayDuration);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x057 skippable chunk
    /// </summary>
    [Chunk(0x03043057)]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043057 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043057;

        /// <inheritdoc />
        public override bool Ignore => true;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x058 skippable chunk (SubMapsInfos)
    /// </summary>
    [Chunk(0x03043058, "SubMapsInfos")]
    [ChunkGameVersion(GameVersion.MP4)]
    public partial class Chunk03043058 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03043058;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            if (U01>0)
            {
                throw new ("");
            }
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x059 skippable chunk (world distortion)
    /// </summary>
    [Chunk(0x03043059, "world distortion")]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03043059 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03043059;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }

        public CPlugBitmap? U01;
        public bool U02;
        public int U03;
        public int U04;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Vec3(ref n.worldDistortion);
            if (Version == 0)
            {
                rw.NodeRef<CPlugBitmap>(ref U01);
            }
            if (Version >= 1)
            {
                rw.Boolean(ref U02);
                if (U02)
                {
                    throw new ("");
                }
                if (Version >= 3)
                {
                    rw.Int32(ref U03);
                    rw.Int32(ref U04);
                }
            }
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x05A skippable chunk
    /// </summary>
    [Chunk(0x0304305A)]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk0304305A : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304305A;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

        public int U01;
        public int U02;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x05B skippable chunk (lightmaps TM2020)
    /// </summary>
    [Chunk(0x0304305B, "lightmaps TM2020")]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk0304305B : Chunk0304303D
    {
        /// <inheritdoc />
        public override uint Id => 0x0304305B;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x05C skippable chunk
    /// </summary>
    [Chunk(0x0304305C)]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk0304305C : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304305C;

        /// <inheritdoc />
        public override bool Ignore => true;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x05D skippable chunk
    /// </summary>
    [Chunk(0x0304305D)]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk0304305D : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304305D;

        /// <inheritdoc />
        public override bool Ignore => true;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x05E skippable chunk
    /// </summary>
    [Chunk(0x0304305E)]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk0304305E : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304305E;

        /// <inheritdoc />
        public override bool Ignore => true;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x05F skippable chunk (free blocks)
    /// </summary>
    [Chunk(0x0304305F, "free blocks")]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk0304305F : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304305F;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x060 skippable chunk
    /// </summary>
    [Chunk(0x03043060)]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk03043060 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03043060;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x061 skippable chunk
    /// </summary>
    [Chunk(0x03043061)]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk03043061 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043061;

        /// <inheritdoc />
        public override bool Ignore => true;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x062 skippable chunk (MapElemColor)
    /// </summary>
    [Chunk(0x03043062, "MapElemColor")]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk03043062 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043062;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x063 skippable chunk (AnimPhaseOffset)
    /// </summary>
    [Chunk(0x03043063, "AnimPhaseOffset")]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk03043063 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043063;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x064 skippable chunk (MT groups?)
    /// </summary>
    [Chunk(0x03043064, "MT groups?")]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk03043064 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043064;

        /// <inheritdoc />
        public override bool Ignore => true;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x065 skippable chunk (foreground pack desc)
    /// </summary>
    [Chunk(0x03043065, "foreground pack desc")]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk03043065 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043065;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x067 skippable chunk (launched checkpoints)
    /// </summary>
    [Chunk(0x03043067, "launched checkpoints")]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk03043067 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043067;

        /// <inheritdoc />
        public override bool Ignore => true;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x068 skippable chunk (MapElemLmQuality)
    /// </summary>
    [Chunk(0x03043068, "MapElemLmQuality")]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk03043068 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043068;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x069 skippable chunk (macroblock instances)
    /// </summary>
    [Chunk(0x03043069, "macroblock instances")]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk03043069 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x03043069;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnChallenge 0x06B skippable chunk (light settings 2)
    /// </summary>
    [Chunk(0x0304306B, "light settings 2")]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk0304306B : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x0304306B;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

        public int U01;
        public int U02;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.TimeOfDay(ref n.dayTime);
            rw.Int32(ref U02);
            rw.Boolean(ref n.dynamicDaylight);
            rw.TimeInt32Nullable(ref n.dayDuration);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x06C skippable chunk (color palette)
    /// </summary>
    [Chunk(0x0304306C, "color palette")]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk0304306C : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0304306C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.EnumByte<PaletteColor>(ref n.palette);
        }
    }

    /// <summary>
    /// CGameCtnChallenge 0x000 skippable chunk
    /// </summary>
    [Chunk(0x3F001000)]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk3F001000 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x3F001000;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;

    }

    /// <summary>
    /// CGameCtnChallenge 0x001 skippable chunk
    /// </summary>
    [Chunk(0x3F001001)]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk3F001001 : SkippableChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public override uint Id => 0x3F001001;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;

    }

    /// <summary>
    /// CGameCtnChallenge 0x002 skippable chunk
    /// </summary>
    [Chunk(0x3F001002)]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk3F001002 : Chunk3F001001
    {
        /// <inheritdoc />
        public override uint Id => 0x3F001002;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;

    }

    /// <summary>
    /// CGameCtnChallenge 0x003 skippable chunk
    /// </summary>
    [Chunk(0x3F001003)]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk3F001003 : Chunk3F001001
    {
        /// <inheritdoc />
        public override uint Id => 0x3F001003;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;

    }


    public sealed partial class SBakedClipsAdditionalData : IReadable, IWritable
    {
        public Ident? Clip1 { get; set; }
        public Ident? Clip2 { get; set; }
        public Ident? Clip3 { get; set; }
        public Ident? Clip4 { get; set; }
        public Int3 Coord { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            Clip1 = r.ReadIdent();
            Clip2 = r.ReadIdent();
            Clip3 = r.ReadIdent();
            Clip4 = r.ReadIdent();
            Coord = r.ReadInt3();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(Clip1);
            w.Write(Clip2);
            w.Write(Clip3);
            w.Write(Clip4);
            w.Write(Coord);
        }
    }

    public sealed partial class SChallengeCardEventIds : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        private Ident[]? u03;
        public Ident[]? U03 { get => u03; set => u03 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Id(ref u02);
            rw.ArrayIdent(ref u03!);
        }
    }

    public sealed partial class BotPath : IReadableWritable
    {

        private int clan;
        public int Clan { get => clan; set => clan = value; }

        private List<Vec3>? path;
        public List<Vec3>? Path { get => path; set => path = value; }

        private bool isFlying;
        public bool IsFlying { get => isFlying; set => isFlying = value; }

        private CGameWaypointSpecialProperty? waypointSpecialProperty;
        public CGameWaypointSpecialProperty? WaypointSpecialProperty { get => waypointSpecialProperty; set => waypointSpecialProperty = value; }

        private bool isAutonomous;
        public bool IsAutonomous { get => isAutonomous; set => isAutonomous = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref clan);
            rw.List<Vec3>(ref path!);
            rw.Boolean(ref isFlying);
            rw.NodeRef<CGameWaypointSpecialProperty>(ref waypointSpecialProperty);
            rw.Boolean(ref isAutonomous);
        }
    }


    /// <summary>
    /// The map's intended use.
    /// </summary>
    public enum MapKind
    {
        EndMarker,
        Campaign,
        Puzzle,
        Retro,
        TimeAttack,
        Rounds,
        InProgress,
        Campaign_7,
        Multi,
        Solo,
        Site,
        SoloNadeo,
        MultiNadeo,
    }

    public enum PaletteColor
    {
        Classic,
        Stunt,
        Red,
        Orange,
        Yellow,
        Lime,
        Green,
        Cyan,
        Blue,
        Purple,
        Pink,
        White,
        Black,
    }

    public enum EditorMode
    {
        Advanced,
        Simple,
        HasGhostBlocks,
        Gamepad = 4,
    }

    /// <summary>
    /// Map type in which the map was validated in.
    /// </summary>
    public enum PlayMode
    {
        Race,
        Platform,
        Puzzle,
        Crazy,
        Shortcut,
        Stunts,
        /// <summary>
        /// Any custom map type script.
        /// </summary>
        Script,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0304300D => new Chunk0304300D(),
        0x0304300F => new Chunk0304300F(),
        0x03043011 => new Chunk03043011(),
        0x03043012 => new Chunk03043012(),
        0x03043013 => new Chunk03043013(),
        0x03043014 => new Chunk03043014(),
        0x03043016 => new Chunk03043016(),
        0x03043017 => new Chunk03043017(),
        0x03043018 => new Chunk03043018(),
        0x03043019 => new Chunk03043019(),
        0x0304301A => new Chunk0304301A(),
        0x0304301B => new Chunk0304301B(),
        0x0304301C => new Chunk0304301C(),
        0x0304301D => new Chunk0304301D(),
        0x0304301E => new Chunk0304301E(),
        0x0304301F => new Chunk0304301F(),
        0x03043020 => new Chunk03043020(),
        0x03043021 => new Chunk03043021(),
        0x03043022 => new Chunk03043022(),
        0x03043023 => new Chunk03043023(),
        0x03043024 => new Chunk03043024(),
        0x03043025 => new Chunk03043025(),
        0x03043026 => new Chunk03043026(),
        0x03043027 => new Chunk03043027(),
        0x03043028 => new Chunk03043028(),
        0x03043029 => new Chunk03043029(),
        0x0304302A => new Chunk0304302A(),
        0x0304302D => new Chunk0304302D(),
        0x03043034 => new Chunk03043034(),
        0x03043036 => new Chunk03043036(),
        0x03043038 => new Chunk03043038(),
        0x0304303A => new Chunk0304303A(),
        0x0304303D => new Chunk0304303D(),
        0x0304303E => new Chunk0304303E(),
        0x03043040 => new Chunk03043040(),
        0x03043041 => new Chunk03043041(),
        0x03043042 => new Chunk03043042(),
        0x03043043 => new Chunk03043043(),
        0x03043044 => new Chunk03043044(),
        0x03043047 => new Chunk03043047(),
        0x03043048 => new Chunk03043048(),
        0x03043049 => new Chunk03043049(),
        0x0304304B => new Chunk0304304B(),
        0x0304304D => new Chunk0304304D(),
        0x0304304E => new Chunk0304304E(),
        0x0304304F => new Chunk0304304F(),
        0x03043050 => new Chunk03043050(),
        0x03043051 => new Chunk03043051(),
        0x03043052 => new Chunk03043052(),
        0x03043053 => new Chunk03043053(),
        0x03043054 => new Chunk03043054(),
        0x03043055 => new Chunk03043055(),
        0x03043056 => new Chunk03043056(),
        0x03043057 => new Chunk03043057(),
        0x03043058 => new Chunk03043058(),
        0x03043059 => new Chunk03043059(),
        0x0304305A => new Chunk0304305A(),
        0x0304305B => new Chunk0304305B(),
        0x0304305C => new Chunk0304305C(),
        0x0304305D => new Chunk0304305D(),
        0x0304305E => new Chunk0304305E(),
        0x0304305F => new Chunk0304305F(),
        0x03043060 => new Chunk03043060(),
        0x03043061 => new Chunk03043061(),
        0x03043062 => new Chunk03043062(),
        0x03043063 => new Chunk03043063(),
        0x03043064 => new Chunk03043064(),
        0x03043065 => new Chunk03043065(),
        0x03043067 => new Chunk03043067(),
        0x03043068 => new Chunk03043068(),
        0x03043069 => new Chunk03043069(),
        0x0304306B => new Chunk0304306B(),
        0x0304306C => new Chunk0304306C(),
        0x3F001000 => new Chunk3F001000(),
        0x3F001001 => new Chunk3F001001(),
        0x3F001002 => new Chunk3F001002(),
        0x3F001003 => new Chunk3F001003(),
        _ => base.NewChunk(chunkId),
    };
}
