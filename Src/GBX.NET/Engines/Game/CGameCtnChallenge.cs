using System;
using System.IO.Compression;
using System.Security;
using System.Text;
using GBX.NET.BlockInfo;
using GBX.NET.Engines.Plug;

namespace GBX.NET.Engines.Game;

/// <summary>
/// A map.
/// </summary>
/// <remarks>ID: 0x03043000</remarks>
[Node(0x03043000)]
[NodeExtension("Challenge")]
[NodeExtension("Map")]
public partial class CGameCtnChallenge : CMwNod, CGameCtnChallenge.IHeader
{
    #region Enums

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
        MultiNadeo
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
        Script
    }

    /// <summary>
    /// In which editor settings this map was made.
    /// </summary>
    [Flags]
    public enum EditorMode
    {
        Advanced,
        Simple,
        HasGhostBlocks
    }

    /// <summary>
    /// Used by Virtual Skipper.
    /// </summary>
    public enum BoatName : byte
    {
        Acc,
        Multi,
        Melges,
        OffShore
    }

    /// <summary>
    /// Used by Virtual Skipper.
    /// </summary>
    public enum RaceMode : byte
    {
        FleetRace,
        MatchRace,
        TeamRace
    }

    /// <summary>
    /// Used by Virtual Skipper.
    /// </summary>
    public enum WindDirection : byte
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }

    /// <summary>
    /// Used by Virtual Skipper.
    /// </summary>
    public enum Weather : byte
    {
        Sunny,
        Cloudy,
        Rainy,
        Stormy
    }

    /// <summary>
    /// Used by Virtual Skipper.
    /// </summary>
    public enum StartDelay : byte
    {
        Immediate,
        OneMin,
        TwoMin,
        FiveMin,
        EightMin
    }

    /// <summary>
    /// Used by Virtual Skipper.
    /// </summary>
    public enum AILevel : byte
    {
        Easy,
        Intermediate,
        Expert,
        Pro
    }

    #endregion

    #region Fields

    private TimeInt32? bronzeTime; // Only used if ChallengeParameters is null
    private TimeInt32? silverTime; // Only used if ChallengeParameters is null
    private TimeInt32? goldTime; // Only used if ChallengeParameters is null
    private TimeInt32? authorTime; // Only used if ChallengeParameters is null
    private int? cost;
    private EditorMode editor;
    private int? authorScore;
    private bool? isLapRace;
    private int? nbLaps;
    private Ident mapInfo;
    private string mapName;
    private MapKind kindInHeader;
    private MapKind kind;
    private int? nbCheckpoints;
    private string? password;
    private Ident? decoration;
    private string? mapType;
    private ulong? lightmapCacheUID;
    private byte? lightmapVersion;
    private string? xml;
    private string? comments;
    private byte[]? thumbnail;
    private Vec2? mapCoordOrigin;
    private Vec2? mapCoordTarget;
    private string? mapStyle;
    private string? titleID;
    private int? authorVersion;
    private string? authorLogin;
    private string? authorNickname;
    private string? authorZone;
    private string? authorExtraInfo;
    private Ident? playerModel;
    private CGameCtnChallengeParameters? challengeParameters;
    private CGameCtnCollectorList? blockStock;
    private Int3[]? checkpoints;
    private FileRef? modPackDesc;
    private Int3? size;
    private bool? needUnlock;
    private IList<CGameCtnBlock>? blocks;
    private CGameCtnMediaClip? clipIntro;
    private CGameCtnMediaClipGroup? clipGroupInGame;
    private CGameCtnMediaClipGroup? clipGroupEndRace;
    private CGameCtnMediaClip? clipAmbiance;
    private CGameCtnMediaClip? clipPodium;
    private FileRef? customMusicPackDesc;
    private PlayMode? mode;
    private byte[]? hashedPassword;
    private uint? crc32;
    private Vec3? thumbnailPosition;
    private Vec3? thumbnailPitchYawRoll;
    private float? thumbnailFOV;
    private float? thumbnailNearClipPlane;
    private float? thumbnailFarClipPlane;
    private IList<CGameCtnAnchoredObject>? anchoredObjects;
    private CScriptTraitsMetadata? scriptMetadata;
    private List<List<EmbeddedFile>>? lightmapFrames;
    private CHmsLightMapCache? lightmapCache;
    private CGameCtnZoneGenealogy[]? genealogies;
    private string? objectiveTextAuthor;
    private string? objectiveTextGold;
    private string? objectiveTextSilver;
    private string? objectiveTextBronze;
    private IList<(Int3 start, Int3 end)>? offzones;
    private string? buildVersion;
    private int decoBaseHeightOffset;
    private IList<BotPath>? botPaths;
    private Dictionary<string, byte[]>? embeddedData;
    private byte[]? originalEmbedZip;
    private TimeSpan? dayTime;
    private bool dynamicDaylight;
    private TimeInt32? dayDuration;
    private bool? hasCustomCamThumbnail;
    private CSceneVehicleCarMarksSamples?[]? carMarksBuffer;

    #endregion

    #region Properties

    public HeaderChunkSet HeaderChunks { get; } = new();

    /// <summary>
    /// Time of the bronze medal. If <see cref="ChallengeParameters"/> is available, it uses the value from there instead.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043002>(sinceVersion: 1)]
    public TimeInt32? TMObjective_BronzeTime
    {
        get => ChallengeParameters is null ? bronzeTime : ChallengeParameters.BronzeTime;
        set
        {
            if (ChallengeParameters is not null)
            {
                ChallengeParameters.BronzeTime = value;
            }

            bronzeTime = value;
        }
    }

    /// <summary>
    /// Time of the silver medal. If <see cref="ChallengeParameters"/> is available, it uses the value from there instead.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043002>(sinceVersion: 1)]
    public TimeInt32? TMObjective_SilverTime
    {
        get => ChallengeParameters is null ? silverTime : ChallengeParameters.SilverTime;
        set
        {
            if (ChallengeParameters is not null)
            {
                ChallengeParameters.SilverTime = value;
            }

            silverTime = value;
        }
    }

    /// <summary>
    /// Time of the gold medal. If <see cref="ChallengeParameters"/> is available, it uses the value from there instead.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043002>(sinceVersion: 1)]
    public TimeInt32? TMObjective_GoldTime
    {
        get => ChallengeParameters is null ? goldTime : ChallengeParameters.GoldTime;
        set
        {
            if (ChallengeParameters is not null)
            {
                ChallengeParameters.GoldTime = value;
            }

            goldTime = value;
        }
    }

    /// <summary>
    /// Time of the author medal. If <see cref="ChallengeParameters"/> is available, it uses the value from there instead.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043002>(sinceVersion: 1)]
    public TimeInt32? TMObjective_AuthorTime
    {
        get => ChallengeParameters is null ? authorTime : ChallengeParameters.AuthorTime;
        set
        {
            if (ChallengeParameters is not null)
            {
                ChallengeParameters.AuthorTime = value;
            }

            authorTime = value;
        }
    }

    /// <summary>
    /// Display cost of the track (or copper cost) explaining the performance of the map.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043002>(sinceVersion: 4)]
    public int? Cost { get => cost; set => cost = value; }

    /// <summary>
    /// Usually author time or stunts score. If <see cref="ChallengeParameters"/> is available, it uses the value from there instead.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043002>(sinceVersion: 10)]
    public int? AuthorScore
    {
        get => ChallengeParameters is null ? authorScore : ChallengeParameters.AuthorScore;
        set
        {
            if (ChallengeParameters is not null)
            {
                ChallengeParameters.AuthorScore = value;
            }

            authorScore = value;
        }
    }

    /// <summary>
    /// In which editor settings the map was made.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043002>(sinceVersion: 11)]
    public EditorMode Editor { get => editor; set => editor = value; }

    /// <summary>
    /// If the map was made using the simple editor.
    /// </summary>
    [NodeMember]
    public bool CreatedWithSimpleEditor => (editor & EditorMode.Simple) != 0;

    /// <summary>
    /// If the map uses ghost blocks.
    /// </summary>
    [NodeMember]
    public bool HasGhostBlocks => (editor & EditorMode.HasGhostBlocks) != 0;

    /// <summary>
    /// If the map is a multilap.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043002>(sinceVersion: 5)]
    [AppliedWithChunk<Chunk03043018>]
    public bool? TMObjective_IsLapRace
    {
        get
        {
            DiscoverChunk<Chunk03043018>();
            return isLapRace;
        }
        set
        {
            DiscoverChunk<Chunk03043018>();
            isLapRace = value;
        }
    }

    /// <summary>
    /// Number of laps.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043002>(sinceVersion: 13)]
    [AppliedWithChunk<Chunk03043018>]
    public int? TMObjective_NbLaps
    {
        get
        {
            DiscoverChunk<Chunk03043018>();
            return nbLaps;
        }
        set
        {
            DiscoverChunk<Chunk03043018>();
            nbLaps = value;
        }
    }

    /// <summary>
    /// Number of checkpoints.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043002>(sinceVersion: 13)]
    public int? NbCheckpoints { get => nbCheckpoints; set => nbCheckpoints = value; }

    /// <summary>
    /// Map UID, environment, and author login.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043002>(sinceVersion: 0, upToVersion: 2)]
    [AppliedWithChunk<Chunk0304300F>]
    [AppliedWithChunk<Chunk03043013>]
    [AppliedWithChunk<Chunk0304301F>]
    public Ident MapInfo { get => mapInfo; set => mapInfo = value; }

    /// <summary>
    /// The map's UID.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043002>(sinceVersion: 0, upToVersion: 2)]
    [AppliedWithChunk<Chunk03043003>]
    [AppliedWithChunk<Chunk03043013>]
    [AppliedWithChunk<Chunk0304301F>]
    public string MapUid
    {
        get => mapInfo.Id;
        set => mapInfo = new Ident(value, mapInfo.Collection, mapInfo.Author);
    }

    /// <summary>
    /// Login of the map author.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043008>]
    [AppliedWithChunk<Chunk0304300F>]
    [AppliedWithChunk<Chunk03043013>]
    [AppliedWithChunk<Chunk0304301F>]
    [AppliedWithChunk<Chunk03043042>]
    public string AuthorLogin
    {
        get
        {
            DiscoverChunk<Chunk03043042>();

            return authorLogin is null ? mapInfo.Author : authorLogin;
        }
        set
        {
            DiscoverChunk<Chunk03043042>();
            authorLogin = value;

            mapInfo = new Ident(mapInfo.Id, mapInfo.Collection, value);
        }
    }

    /// <summary>
    /// The map's name.
    /// </summary>
    [NodeMember]
    [SupportsFormatting]
    [AppliedWithChunk<Chunk03043001>(sinceVersion: 0, upToVersion: 0)]
    [AppliedWithChunk<Chunk03043002>(sinceVersion: 0, upToVersion: 2)]
    [AppliedWithChunk<Chunk03043003>]
    [AppliedWithChunk<Chunk03043012>]
    [AppliedWithChunk<Chunk03043013>]
    [AppliedWithChunk<Chunk0304301F>]
    public string MapName { get => mapName; set => mapName = value; }

    /// <summary>
    /// The map's intended use or state, defined in the header. This defines the visibility in the the map browser.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043003>]
    public MapKind KindInHeader { get => kindInHeader; set => kindInHeader = value; }

    /// <summary>
    /// The map's intended use or state, defined in the body. This defines general validity.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043011>]
    public MapKind Kind { get => kind; set => kind = value; }

    /// <summary>
    /// Password of the map used by older maps.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043003>(sinceVersion: 1)]
    public string? Password
    {
        get
        {
            DiscoverChunk<Chunk03043014>();
            return password;
        }
        set
        {
            DiscoverChunk<Chunk03043014>();
            password = value;
        }
    }

    /// <summary>
    /// The map's decoration (time of the day or scenery)
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043003>(sinceVersion: 2)]
    [AppliedWithChunk<Chunk0304300F>]
    [AppliedWithChunk<Chunk03043013>]
    [AppliedWithChunk<Chunk0304301F>]
    public Ident? Decoration { get => decoration; set => decoration = value; }

    /// <summary>
    /// Name of the map type script.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043003>(sinceVersion: 6)]
    public string? MapType
    {
        get => ChallengeParameters is null ? mapType : ChallengeParameters.MapType;
        set
        {
            if (ChallengeParameters is not null)
            {
                ChallengeParameters.MapType = value;
            }

            mapType = value;
        }
    }

    /// <summary>
    /// Style of the map (Fullspeed, LOL, Tech), usually unused and defined by user.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043003>(sinceVersion: 6)]
    public string? MapStyle
    {
        get => ChallengeParameters is null ? mapStyle : ChallengeParameters.MapStyle;
        set
        {
            if (ChallengeParameters is not null)
            {
                ChallengeParameters.MapStyle = value;
            }

            mapStyle = value;
        }
    }

    /// <summary>
    /// UID of the lightmap data stored in cache.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043003>(sinceVersion: 8)]
    public ulong? LightmapCacheUID { get => lightmapCacheUID; set => lightmapCacheUID = value; }

    /// <summary>
    /// Version of the lightmap calculation.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043003>(sinceVersion: 9)]
    public byte? LightmapVersion { get => lightmapVersion; set => lightmapVersion = value; }

    /// <summary>
    /// XML track information and dependencies.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043005>]
    public string? XML { get => xml; set => xml = value; }

    /// <summary>
    /// Thumbnail JPEG data.
    /// </summary>
    [NodeMember]
    [JpegData]
    [AppliedWithChunk<Chunk03043007>(sinceVersion: 1)]
    public byte[]? Thumbnail { get => thumbnail; set => thumbnail = value; }

    /// <summary>
    /// The map's environment.
    /// </summary>
    [NodeMember]
    public Id Collection
    {
        get => mapInfo.Collection;
        set => mapInfo = new Ident(mapInfo.Id, value, mapInfo.Author);
    }

    /// <summary>
    /// Origin of the map.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043003>(sinceVersion: 3)]
    [AppliedWithChunk<Chunk03043023>]
    [AppliedWithChunk<Chunk03043025>]
    public Vec2? MapCoordOrigin { get => mapCoordOrigin; set => mapCoordOrigin = value; }

    /// <summary>
    /// Target of the map.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043003>(sinceVersion: 4)]
    [AppliedWithChunk<Chunk03043023>]
    [AppliedWithChunk<Chunk03043025>]
    public Vec2? MapCoordTarget { get => mapCoordTarget; set => mapCoordTarget = value; }

    /// <summary>
    /// Title pack the map was built in.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043003>(sinceVersion: 11)]
    [AppliedWithChunk<Chunk03043051>]
    public string? TitleID
    {
        get
        {
            DiscoverChunk<Chunk03043051>();
            return titleID;
        }
        set
        {
            DiscoverChunk<Chunk03043051>();
            titleID = value;
        }
    }

    /// <summary>
    /// Executable (game's) build version the map was built in.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043051>]
    public string? BuildVersion
    {
        get
        {
            DiscoverChunk<Chunk03043051>();
            return buildVersion;
        }
        set
        {
            DiscoverChunk<Chunk03043051>();
            buildVersion = value;
        }
    }

    /// <summary>
    /// The map's author comments.
    /// </summary>
    [NodeMember]
    [SupportsFormatting]
    [AppliedWithChunk<Chunk03043007>(sinceVersion: 1)]
    [AppliedWithChunk<Chunk03043028>]
    [AppliedWithChunk<Chunk0304302D>]
    public string? Comments
    {
        get
        {
            DiscoverChunk<Chunk03043036>();
            return comments;
        }
        set
        {
            DiscoverChunk<Chunk03043036>();
            comments = value;
        }
    }

    [NodeMember]
    [AppliedWithChunk<Chunk03043008>]
    [AppliedWithChunk<Chunk03043042>]
    public int? AuthorVersion
    {
        get
        {
            DiscoverChunk<Chunk03043042>();
            return authorVersion;
        }
        set
        {
            DiscoverChunk<Chunk03043042>();
            authorVersion = value;
        }
    }

    /// <summary>
    /// Nickname of the map author.
    /// </summary>
    [NodeMember]
    [SupportsFormatting]
    [AppliedWithChunk<Chunk03043008>]
    [AppliedWithChunk<Chunk03043042>]
    public string? AuthorNickname
    {
        get
        {
            DiscoverChunk<Chunk03043042>();
            return authorNickname;
        }
        set
        {
            DiscoverChunk<Chunk03043042>();
            authorNickname = value;
        }
    }

    /// <summary>
    /// Zone of the map author.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043008>]
    [AppliedWithChunk<Chunk03043042>]
    public string? AuthorZone
    {
        get
        {
            DiscoverChunk<Chunk03043042>();
            return authorZone;
        }
        set
        {
            DiscoverChunk<Chunk03043042>();
            authorZone = value;
        }
    }

    [NodeMember]
    [AppliedWithChunk<Chunk03043008>]
    [AppliedWithChunk<Chunk03043042>]
    public string? AuthorExtraInfo
    {
        get
        {
            DiscoverChunk<Chunk03043042>();
            return authorExtraInfo;
        }
        set
        {
            DiscoverChunk<Chunk03043042>();
            authorExtraInfo = value;
        }
    }

    /// <summary>
    /// The car's name, environment and author used on the map.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk0304300D>]
    public Ident? PlayerModel { get => playerModel; set => playerModel = value; }

    /// <summary>
    /// Map parameters.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043011>]
    public CGameCtnChallengeParameters? ChallengeParameters { get => challengeParameters; set => challengeParameters = value; }

    /// <summary>
    /// List of available puzzle pieces.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043011>]
    public CGameCtnCollectorList? BlockStock { get => blockStock; set => blockStock = value; }

    /// <summary>
    /// All checkpoints and their map coordinates. Used by TMUF and older games.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043017>]
    public Int3[]? Checkpoints
    {
        get
        {
            DiscoverChunk<Chunk03043017>();
            return checkpoints;
        }
        set
        {
            DiscoverChunk<Chunk03043017>();
            checkpoints = value;
        }
    }

    /// <summary>
    /// Reference to the mod (texture/resource pack) used on the map.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043019>]
    public FileRef? ModPackDesc
    {
        get
        {
            DiscoverChunk<Chunk03043019>();
            return modPackDesc;
        }
        set
        {
            DiscoverChunk<Chunk03043019>();
            modPackDesc = value;
        }
    }

    /// <summary>
    /// Map type in which the track was validated in.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043002>(sinceVersion: 7)]
    public PlayMode? Mode
    {
        get
        {
            DiscoverChunk<Chunk0304301C>();
            return mode;
        }
        set
        {
            DiscoverChunk<Chunk0304301C>();
            mode = value;
        }
    }

    /// <summary>
    /// Size of the map in block coordinates.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk0304300F>]
    [AppliedWithChunk<Chunk03043013>]
    [AppliedWithChunk<Chunk0304301F>]
    public Int3? Size { get => size; set => size = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0304300F>]
    public bool? NeedUnlock { get => needUnlock; set => needUnlock = value; }

    /// <summary>
    /// List of all blocks on the map. Can be null when only the header was read, or simply when <see cref="Chunk0304300F"/>, <see cref="Chunk03043013"/>, or <see cref="Chunk0304301F"/> is missing.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk0304300F>]
    [AppliedWithChunk<Chunk03043013>]
    [AppliedWithChunk<Chunk0304301F>]
    public IList<CGameCtnBlock>? Blocks { get => blocks; set => blocks = value; }

    /// <summary>
    /// Number of actual blocks on the map (doesn't include Unassigned1 and other blocks with <see cref="CGameCtnBlock.Flags"/> equal to -1).
    /// </summary>
    [NodeMember]
    public int? NbBlocks => Blocks?.Count(x => x.Name != "Unassigned1"); // in some Maniaplanet 3 maps, Unassigned1 blocks can have Flags == 0, so it is safer to compare with string

    [NodeMember]
    [AppliedWithChunk<Chunk03043048>]
    public IList<CGameCtnBlock>? BakedBlocks { get; set; }

    /// <summary>
    /// Number of actual baked blocks on the map (doesn't include Unassigned1 and other blocks with <see cref="CGameCtnBlock.Flags"/> equal to -1).
    /// </summary>
    [NodeMember]
    public int? NbBakedBlocks => BakedBlocks?.Count(x => x.Flags != -1);

    [NodeMember]
    [AppliedWithChunk<Chunk03043048>]
    public SBakedClipsAdditionalData[]? BakedClipsAdditionalData { get; set; }

    /// <summary>
    /// MediaTracker intro.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043021>]
    [AppliedWithChunk<Chunk03043049>]
    public CGameCtnMediaClip? ClipIntro { get => clipIntro; set => clipIntro = value; }

    /// <summary>
    /// MediaTracker ingame.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043021>]
    [AppliedWithChunk<Chunk03043049>]
    public CGameCtnMediaClipGroup? ClipGroupInGame { get => clipGroupInGame; set => clipGroupInGame = value; }

    /// <summary>
    /// MediaTracker end race.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043021>]
    [AppliedWithChunk<Chunk03043049>]
    public CGameCtnMediaClipGroup? ClipGroupEndRace { get => clipGroupEndRace; set => clipGroupEndRace = value; }

    /// <summary>
    /// MediaTracker ambiance.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043049>(sinceVersion: 2)]
    public CGameCtnMediaClip? ClipAmbiance { get => clipAmbiance; set => clipAmbiance = value; }

    /// <summary>
    /// MediaTracker podium.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043049>]
    public CGameCtnMediaClip? ClipPodium { get => clipPodium; set => clipPodium = value; }

    /// <summary>
    /// Reference to the custom music used on the map.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043024>]
    public FileRef? CustomMusicPackDesc { get => customMusicPackDesc; set => customMusicPackDesc = value; }

    /// <summary>
    /// Hashed password of the map, if it's password protected.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043029>]
    public byte[]? HashedPassword
    {
        get
        {
            DiscoverChunk<Chunk03043029>();
            return hashedPassword;
        }
        set
        {
            DiscoverChunk<Chunk03043029>();
            hashedPassword = value;
        }
    }

    /// <summary>
    /// CRC32 of the map.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043029>]
    public uint? CRC32
    {
        get
        {
            DiscoverChunk<Chunk03043029>();
            return crc32;
        }
        set
        {
            DiscoverChunk<Chunk03043029>();
            crc32 = value;
        }
    }

    /// <summary>
    /// Position of the thumbnail camera.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043027>]
    [AppliedWithChunk<Chunk03043028>]
    [AppliedWithChunk<Chunk0304302D>]
    [AppliedWithChunk<Chunk03043036>]
    public Vec3? ThumbnailPosition
    {
        get
        {
            DiscoverChunk<Chunk03043036>();
            return thumbnailPosition;
        }
        set
        {
            DiscoverChunk<Chunk03043036>();
            thumbnailPosition = value;
        }
    }

    /// <summary>
    /// Pitch, yaw and roll of the thumbnail camera in radians.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043027>]
    [AppliedWithChunk<Chunk03043028>]
    [AppliedWithChunk<Chunk0304302D>]
    [AppliedWithChunk<Chunk03043036>]
    public Vec3? ThumbnailPitchYawRoll
    {
        get
        {
            DiscoverChunk<Chunk03043036>();
            return thumbnailPitchYawRoll;
        }
        set
        {
            DiscoverChunk<Chunk03043036>();
            thumbnailPitchYawRoll = value;
        }
    }

    /// <summary>
    /// Thumbnail camera FOV.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043027>]
    [AppliedWithChunk<Chunk03043028>]
    [AppliedWithChunk<Chunk0304302D>]
    [AppliedWithChunk<Chunk03043036>]
    public float? ThumbnailFOV
    {
        get
        {
            DiscoverChunk<Chunk03043036>();
            return thumbnailFOV;
        }
        set
        {
            DiscoverChunk<Chunk03043036>();
            thumbnailFOV = value;
        }
    }

    [NodeMember]
    [AppliedWithChunk<Chunk03043027>]
    [AppliedWithChunk<Chunk03043028>]
    [AppliedWithChunk<Chunk0304302D>]
    [AppliedWithChunk<Chunk03043036>]
    public float? ThumbnailNearClipPlane
    {
        get
        {
            DiscoverChunk<Chunk03043036>();
            return thumbnailNearClipPlane;
        }
        set
        {
            DiscoverChunk<Chunk03043036>();
            thumbnailNearClipPlane = value;
        }
    }

    [NodeMember]
    [AppliedWithChunk<Chunk03043027>]
    [AppliedWithChunk<Chunk03043028>]
    [AppliedWithChunk<Chunk0304302D>]
    [AppliedWithChunk<Chunk03043036>]
    public float? ThumbnailFarClipPlane
    {
        get
        {
            DiscoverChunk<Chunk03043036>();
            return thumbnailFarClipPlane;
        }
        set
        {
            DiscoverChunk<Chunk03043036>();
            thumbnailFarClipPlane = value;
        }
    }

    /// <summary>
    /// List of all the available lightmap frames. Each frame can contain up to 3 different variants in either JPEG or WEBP format.
    /// </summary>
    [AppliedWithChunk<Chunk0304303D>]
    [AppliedWithChunk<Chunk0304305B>]
    public List<List<EmbeddedFile>>? LightmapFrames
    {
        get
        {
            DiscoverChunks<Chunk0304303D, Chunk0304305B>();
            return lightmapFrames;
        }
        set
        {
            DiscoverChunks<Chunk0304303D, Chunk0304305B>();
            lightmapFrames = value;
        }
    }

    /// <summary>
    /// Lightmap cache information.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk0304303D>]
    [AppliedWithChunk<Chunk0304305B>]
    public CHmsLightMapCache? LightmapCache
    {
        get
        {
            DiscoverChunks<Chunk0304303D, Chunk0304305B>();
            return lightmapCache;
        }
        set
        {
            DiscoverChunks<Chunk0304303D, Chunk0304305B>();
            lightmapCache = value;
        }
    }

    /// <summary>
    /// If the map has at least 1 lightmap frame.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk0304303D>]
    [AppliedWithChunk<Chunk0304305B>]
    public bool HasLightmaps => lightmapFrames?.Count > 0;

    /// <summary>
    /// List of all items and objects placed on the map.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043040>]
    public IList<CGameCtnAnchoredObject>? AnchoredObjects
    {
        get
        {
            DiscoverChunk<Chunk03043040>();
            return anchoredObjects;
        }
        set
        {
            DiscoverChunk<Chunk03043040>();
            anchoredObjects = value;
        }
    }

    [NodeMember]
    [AppliedWithChunk<Chunk03043043>]
    public CGameCtnZoneGenealogy[]? Genealogies
    {
        get
        {
            DiscoverChunk<Chunk03043043>();
            return genealogies;
        }
        set
        {
            DiscoverChunk<Chunk03043043>();
            genealogies = value;
        }
    }

    /// <summary>
    /// Metadata written into the map.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043044>]
    public CScriptTraitsMetadata? ScriptMetadata
    {
        get
        {
            DiscoverChunk<Chunk03043044>();
            return scriptMetadata;
        }
        set
        {
            DiscoverChunk<Chunk03043044>();
            scriptMetadata = value;
        }
    }

    [NodeMember]
    [AppliedWithChunk<Chunk0304304B>]
    public string? ObjectiveTextAuthor
    {
        get
        {
            DiscoverChunk<Chunk0304304B>();
            return objectiveTextAuthor;
        }
        set
        {
            DiscoverChunk<Chunk0304304B>();
            objectiveTextAuthor = value;
        }
    }

    [NodeMember]
    [AppliedWithChunk<Chunk0304304B>]
    public string? ObjectiveTextGold
    {
        get
        {
            DiscoverChunk<Chunk0304304B>();
            return objectiveTextGold;
        }
        set
        {
            DiscoverChunk<Chunk0304304B>();
            objectiveTextGold = value;
        }
    }

    [NodeMember]
    [AppliedWithChunk<Chunk0304304B>]
    public string? ObjectiveTextSilver
    {
        get
        {
            DiscoverChunk<Chunk0304304B>();
            return objectiveTextSilver;
        }
        set
        {
            DiscoverChunk<Chunk0304304B>();
            objectiveTextSilver = value;
        }
    }

    [NodeMember]
    [AppliedWithChunk<Chunk0304304B>]
    public string? ObjectiveTextBronze
    {
        get
        {
            DiscoverChunk<Chunk0304304B>();
            return objectiveTextBronze;
        }
        set
        {
            DiscoverChunk<Chunk0304304B>();
            objectiveTextBronze = value;
        }
    }

    /// <summary>
    /// List of offzones defined on the map, constructed with cubes made from start-to-end coordinates.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043050>]
    public IList<(Int3 start, Int3 end)>? Offzones
    {
        get
        {
            DiscoverChunk<Chunk03043050>();
            return offzones;
        }
        set
        {
            DiscoverChunk<Chunk03043050>();
            offzones = value;
        }
    }

    /// <summary>
    /// Minimum Y value of <see cref="Blocks"/>.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043052>]
    public int DecoBaseHeightOffset
    {
        get
        {
            DiscoverChunk<Chunk03043052>();
            return decoBaseHeightOffset;
        }
        set
        {
            DiscoverChunk<Chunk03043052>();
            decoBaseHeightOffset = value;
        }
    }

    /// <summary>
    /// Bot paths defined on the (Shootmania) map.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043053>]
    public IList<BotPath>? BotPaths
    {
        get
        {
            DiscoverChunk<Chunk03043053>();
            return botPaths;
        }
        set
        {
            DiscoverChunk<Chunk03043053>();
            botPaths = value;
        }
    }

    /// <summary>
    /// Embedded data in the map. Key defines a relative path to the resource. Value is the actual data, usually in Gbx format, sometimes in image format.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043054>]
    public Dictionary<string, byte[]>? EmbeddedData
    {
        get
        {
            DiscoverChunk<Chunk03043054>();
            return embeddedData;
        }
    }

    /// <summary>
    /// Exact time of the day in the map. Available since ManiaPlanet 4.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043056>]
    public TimeSpan? DayTime
    {
        get
        {
            DiscoverChunk<Chunk03043056>();
            return dayTime;
        }
        set
        {
            DiscoverChunk<Chunk03043056>();
            dayTime = value;
        }
    }

    /// <summary>
    /// If the map uses dynamic daylight cycle. Available since ManiaPlanet 4.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043056>]
    public bool DynamicDaylight
    {
        get
        {
            DiscoverChunk<Chunk03043056>();
            return dynamicDaylight;
        }
        set
        {
            DiscoverChunk<Chunk03043056>();
            dynamicDaylight = value;
        }
    }

    /// <summary>
    /// Duration of the day defined in real time. Available since ManiaPlanet 4.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043056>]
    public TimeInt32? DayDuration
    {
        get
        {
            DiscoverChunk<Chunk03043056>();
            return dayDuration;
        }
        set
        {
            DiscoverChunk<Chunk03043056>();
            dayDuration = value;
        }
    }

    /// <summary>
    /// If the thumbnail camera was customized. Only relevant up to TMUF.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03043027>]
    [AppliedWithChunk<Chunk03043028>]
    public bool? HasCustomCamThumbnail { get => hasCustomCamThumbnail; set => hasCustomCamThumbnail = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk03043069>]
    public IList<MacroblockInstance>? MacroblockInstances { get; set; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0304303E>]
    public CSceneVehicleCarMarksSamples?[]? CarMarksBuffer
    {
        get
        {
            DiscoverChunk<Chunk0304303E>();
            return carMarksBuffer;
        }
        set
        {
            DiscoverChunk<Chunk0304303E>();
            carMarksBuffer = value;
        }
    }

    #endregion

    #region Constructors

    internal CGameCtnChallenge()
    {
        mapInfo = Ident.Empty;
        mapName = "";
    }

    #endregion

    #region Methods

    /// <summary>
    /// Exports the map's thumbnail as JPEG.
    /// </summary>
    /// <param name="stream">Stream to export to.</param>
    public bool ExportThumbnail(Stream stream)
    {
        if (thumbnail is null)
        {
            return false;
        }

        stream.Write(thumbnail, 0, thumbnail.Length);

        return true;
    }

    /// <summary>
    /// Exports the map's thumbnail as JPEG.
    /// </summary>
    /// <param name="fileName">File to export to.</param>
    public bool ExportThumbnail(string fileName)
    {
        if (thumbnail is null)
        {
            return false;
        }

        using var fs = File.Create(fileName);

        return ExportThumbnail(fs);
    }

    /// <summary>
    /// Cracks the map password.
    /// </summary>
    public void CrackPassword()
    {
        password = null;

        if (hashedPassword is not null)
        {
            hashedPassword = new byte[16];
        }

        RemoveChunk<Chunk03043029>();
    }

    /// <summary>
    /// Gets the first block at this position.
    /// </summary>
    /// <param name="pos">Position of the block.</param>
    /// <returns>The first available block.</returns>
    public CGameCtnBlock? GetBlock(Int3 pos) => blocks?.FirstOrDefault(x => x.Coord == pos);

    /// <summary>
    /// Gets the first block at this position.
    /// </summary>
    /// <param name="x">X position.</param>
    /// <param name="y">Y position.</param>
    /// <param name="z">Z position.</param>
    /// <returns>The first available block.</returns>
    public CGameCtnBlock? GetBlock(int x, int y, int z) => GetBlock((x, y, z));

    /// <summary>
    /// Retrieves blocks.
    /// </summary>
    /// <returns>An enumerable of blocks.</returns>
    public IEnumerable<CGameCtnBlock> GetBlocks() => blocks ?? Enumerable.Empty<CGameCtnBlock>();

    /// <summary>
    /// Retrieves blocks at this position.
    /// </summary>
    /// <param name="pos">Position of the block.</param>
    /// <returns>An enumerable of blocks.</returns>
    public IEnumerable<CGameCtnBlock> GetBlocks(Int3 pos) => GetBlocks().Where(x => x.Coord == pos);

    /// <summary>
    /// Retrieves blocks at this position.
    /// </summary>
    /// <param name="x">X position.</param>
    /// <param name="y">Y position.</param>
    /// <param name="z">Z position.</param>
    /// <returns>An enumerable of blocks.</returns>
    public IEnumerable<CGameCtnBlock> GetBlocks(int x, int y, int z) => GetBlocks((x, y, z));

    /// <summary>
    /// Retrieves ghost blocks on the map.
    /// </summary>
    /// <returns>An enumerable of ghost blocks.</returns>
    public IEnumerable<CGameCtnBlock> GetGhostBlocks() => GetBlocks().Where(x => x.IsGhost);

    /// <summary>
    /// Retrieves baked blocks.
    /// </summary>
    /// <returns>An enumerable of baked blocks.</returns>
    public IEnumerable<CGameCtnBlock> GetBakedBlocks() => BakedBlocks ?? Enumerable.Empty<CGameCtnBlock>();

    /// <summary>
    /// Places a block in the map.
    /// </summary>
    /// <param name="blockModel">Block model name to place. Only the name is required, so using <see cref="Ident(string)"/> works too. Full <see cref="Ident"/> can be seen in TM1.0.</param>
    /// <param name="coord">Position on the map. Should be always under <see cref="Size"/>, otherwise an overflow can happen.</param>
    /// <param name="dir">Facing direction of the block.</param>
    /// <returns>A placed block.</returns>
    /// <exception cref="MemberNullException"><see cref="Blocks"/> is null.</exception>
    public CGameCtnBlock PlaceBlock(Ident blockModel, Int3 coord, Direction dir)
    {
        _ = Blocks ?? throw new MemberNullException(nameof(Blocks));

        var block = new CGameCtnBlock(blockModel.Id, dir, coord);

        block.CreateChunk<CGameCtnBlock.Chunk03057002>();

        Blocks.Add(block);

        return block;
    }

    /// <summary>
    /// Places a block in the map.
    /// </summary>
    /// <param name="blockModel">Block model name to place.</param>
    /// <param name="coord">Position on the map. Should be always under <see cref="Size"/>, otherwise an overflow can happen.</param>
    /// <param name="dir">Facing direction of the block.</param>
    /// <returns>A placed block.</returns>
    public CGameCtnBlock PlaceBlock(string blockModel, Int3 coord, Direction dir)
    {
        return PlaceBlock(new Ident(blockModel), coord, dir);
    }

    /// <summary>
    /// Removes all the blocks from the map that match the conditions defined by the specified predicate.
    /// </summary>
    /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the elements to remove.</param>
    /// <exception cref="MemberNullException"><see cref="Blocks"/> is null.</exception>
    /// <exception cref="NotSupportedException"><see cref="Blocks"/> is read-only.</exception>
    public void RemoveAllBlocks(Predicate<CGameCtnBlock> match)
    {
        _ = Blocks ?? throw new MemberNullException(nameof(Blocks));

        Blocks.RemoveAll(match);
    }

    /// <summary>
    /// Removes all the blocks from the map.
    /// </summary>
    /// <exception cref="MemberNullException"><see cref="Blocks"/> is null.</exception>
    /// <exception cref="NotSupportedException"><see cref="Blocks"/> is read-only.</exception>
    public void RemoveAllBlocks()
    {
        ClearBlocks();
    }

    /// <summary>
    /// Clears all the blocks from the map.
    /// </summary>
    /// <exception cref="MemberNullException"><see cref="Blocks"/> is null.</exception>
    /// <exception cref="NotSupportedException"><see cref="Blocks"/> is read-only.</exception>
    public void ClearBlocks()
    {
        _ = Blocks ?? throw new MemberNullException(nameof(Blocks));

        Blocks.Clear();
    }

    /// <summary>
    /// Retrieves items.
    /// </summary>
    /// <returns>An enumerable of items.</returns>
    public IEnumerable<CGameCtnAnchoredObject> GetAnchoredObjects() => AnchoredObjects ?? Enumerable.Empty<CGameCtnAnchoredObject>();

    /// <summary>
    /// Places an item on a map.
    /// </summary>
    /// <param name="itemModel">An item model identification (name, collection and author). Only the name is required, so using <see cref="Ident(string)"/> works too.</param>
    /// <param name="absolutePosition">Absolute position in the map.</param>
    /// <param name="pitchYawRoll">Rotation of the item in pitch, yaw, and roll format.</param>
    /// <param name="offsetPivot">Pivot location of the item (relative position of the point the item will rotate around).</param>
    /// <param name="variant">An item variant (trees from TM2020 have different variants).</param>
    /// <returns>Placed item.</returns>
    /// <exception cref="MemberNullException"><see cref="AnchoredObjects"/> is null.</exception>
    public CGameCtnAnchoredObject PlaceAnchoredObject(Ident itemModel, Vec3 absolutePosition, Vec3 pitchYawRoll, Vec3 offsetPivot = default, int variant = 0)
    {
        _ = AnchoredObjects ?? throw new MemberNullException(nameof(AnchoredObjects));

        CreateChunk<Chunk03043040>();

        var anchoredObject = new CGameCtnAnchoredObject(itemModel, absolutePosition, pitchYawRoll, offsetPivot, variant);

        anchoredObject.CreateChunk<CGameCtnAnchoredObject.Chunk03101002>();
        anchoredObject.CreateChunk<CGameCtnAnchoredObject.Chunk03101004>();

        AnchoredObjects.Add(anchoredObject);
        
        return anchoredObject;
    }

    /// <summary>
    /// Places an item on a map.
    /// </summary>
    /// <param name="itemModel">An item model identification (name).</param>
    /// <param name="absolutePosition">Absolute position in the map.</param>
    /// <param name="pitchYawRoll">Rotation of the item in pitch, yaw, and roll format.</param>
    /// <param name="offsetPivot">Pivot location of the item (relative position of the point the item will rotate around).</param>
    /// <param name="variant">An item variant (trees from TM2020 have different variants).</param>
    /// <returns>Placed item.</returns>
    /// <exception cref="MemberNullException"><see cref="AnchoredObjects"/> is null.</exception>
    public CGameCtnAnchoredObject PlaceAnchoredObject(string itemModel, Vec3 absolutePosition, Vec3 pitchYawRoll, Vec3 offsetPivot = default, int variant = 0)
    {
        return PlaceAnchoredObject(new Ident(itemModel), absolutePosition, pitchYawRoll, offsetPivot, variant);
    }

    /// <summary>
    /// Transfers the MediaTracker from <see cref="Chunk03043021"/> (up to TMUF) to <see cref="Chunk03043049"/> (ManiaPlanet and Trackmania®). If <see cref="Chunk03043049"/> is already presented, no action is performed.
    /// </summary>
    /// <param name="upscaleTriggerCoord">Defines how many times the same coord should repeat.</param>
    /// <returns>Returns true if any action was performed, otherwise false.</returns>
    public bool TransferMediaTrackerTo049(int upscaleTriggerCoord = 3)
    {
        var chunk021 = GetChunk<Chunk03043021>();
        var chunk049 = CreateChunk<Chunk03043049>();

        if (chunk021 is null)
        {
            return false;
        }

        ConvertMediaClip(ClipIntro);
        ConvertMediaClipGroup(ClipGroupInGame, upscaleTriggerCoord);
        ConvertMediaClipGroup(ClipGroupEndRace, upscaleTriggerCoord);

        RemoveChunk<Chunk03043021>();

        return true;

        static void ConvertMediaClip(CGameCtnMediaClip? node)
        {
            if (node is null)
            {
                return;
            }

            foreach (var track in node.Tracks)
            {
                ConvertMediaTrack(track);
            }
        }

        static void ConvertMediaClipGroup(CGameCtnMediaClipGroup? node, int upscTriggerCoord)
        {
            if (node is null)
            {
                return;
            }

            foreach (var clip in node.Clips)
            {
                var trigger = clip.Trigger;

                var coords = trigger.Coords.ToList();

                for (var i = 0; i < trigger.Coords.Length; i++)
                {
                    coords[i] = coords[i] * (upscTriggerCoord, 1, upscTriggerCoord);

                    for (var x = 0; x < upscTriggerCoord; x++)
                    {
                        for (var z = 0; z < upscTriggerCoord; z++)
                        {
                            coords.Add(coords[i] + new Int3(x, 0, z));
                        }
                    }
                }

                trigger.Coords = coords.ToArray();

                ConvertMediaClip(clip.Clip);
            }
        }

        static void ConvertMediaTrack(CGameCtnMediaTrack node)
        {
            var chunk001 = node.GetChunk<CGameCtnMediaTrack.Chunk03078001>();

            // Chunks 0x002 and 0x004 have to be replaced with 0x005 so that ManiaPlanet accepts the map
            node.TransferMediaTrackTo005();

            node.Blocks.RemoveAll(x => x is CGameCtnMediaBlockGhost); // Some ghosts can crash the game

            node.Blocks.RemoveAll(x => x is CGameCtnMediaBlockFxBloom);
            // FX Bloom is no longer supported and was remade into FxBloomHdr
            // TODO: convert to fx bloom hdr effectively
        }
    }

    /// <summary>
    /// Offsets all MediaTracker camera positions by <paramref name="offset"/>.
    /// </summary>
    /// <param name="offset">Amount of units to offset the cameras.</param>
    public void OffsetMediaTrackerCameras(Vec3 offset)
    {
        OffsetMediaTrackerCameras(offset, ClipIntro);
        OffsetMediaTrackerCameras(offset, ClipGroupInGame);
        OffsetMediaTrackerCameras(offset, ClipGroupEndRace);
        OffsetMediaTrackerCameras(offset, ClipAmbiance);
        OffsetMediaTrackerCameras(offset, ClipPodium);
    }

    private static void OffsetMediaTrackerCameras(Vec3 offset, CGameCtnMediaClipGroup? group)
    {
        if (group is null)
        {
            return;
        }

        foreach (var clip in group.Clips)
        {
            OffsetMediaTrackerCameras(offset, clip.Clip);
        }
    }

    private static void OffsetMediaTrackerCameras(Vec3 offset, CGameCtnMediaClip? clip)
    {
        if (clip is null)
        {
            return;
        }

        foreach (var track in clip.Tracks)
        {
            foreach (var block in track.Blocks)
            {
                switch (block)
                {
                    case CGameCtnMediaBlockCameraCustom c:
                        foreach (var key in c.Keys)
                            if (key.Anchor == -1)
                                key.Position += offset;
                        break;
                    case CGameCtnMediaBlockCameraPath p:
                        foreach (var key in p.Keys)
                            if (key.Anchor == -1)
                                key.Position += offset;
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Offsets all MediaTracker triggers by <paramref name="offset"/>.
    /// </summary>
    /// <param name="offset">Amount of units to offset the triggers.</param>
    public void OffsetMediaTrackerTriggers(Int3 offset)
    {
        OffsetMediaTrackerTriggers(offset, ClipGroupInGame);
        OffsetMediaTrackerTriggers(offset, ClipGroupEndRace);
    }

    private static void OffsetMediaTrackerTriggers(Int3 offset, CGameCtnMediaClipGroup? group)
    {
        if (group is null)
        {
            return;
        }

        foreach (var clip in group.Clips)
        {
            var trigger = clip.Trigger;
            trigger.Coords = trigger.Coords.Select(x => x + offset).ToArray();
        }
    }

    /// <summary>
    /// Enumerates through all of the embedded Gbxs and yields them entirely.
    /// </summary>
    /// <returns>An enumerable of <see cref="GameBox"/>.</returns>
    public IEnumerable<GameBox> GetEmbeddedGbxs()
    {
        if (EmbeddedData is null)
        {
            yield break;
        }

        foreach (var embed in EmbeddedData)
        {
            using var ms = new MemoryStream(embed.Value);

            GameBox? gbx;

            try
            {
                gbx = GameBox.Parse(ms);
            }
            catch
            {
                continue;
            }

            yield return gbx;
        }
    }

    /// <summary>
    /// Enumerates through all of the embedded Gbxs and yields their header data.
    /// </summary>
    /// <returns>An enumerable of <see cref="GameBox"/> with header data only.</returns>
    public IEnumerable<GameBox> GetEmbeddedGbxHeaders()
    {
        if (EmbeddedData is null)
        {
            yield break;
        }

        foreach (var embed in EmbeddedData)
        {
            using var ms = new MemoryStream(embed.Value);

            GameBox? gbx;

            try
            {
                gbx = GameBox.ParseHeader(ms);
            }
            catch
            {
                continue;
            }

            yield return gbx;
        }
    }

    /// <summary>
    /// Enumerates through all of the embedded nodes and yields their header data.
    /// </summary>
    /// <returns>An enumerable of nodes with header data only.</returns>
    public IEnumerable<Node> GetEmbeddedNodeHeaders()
    {
        foreach (var gbx in GetEmbeddedGbxHeaders())
        {
            if (gbx.Node is not null)
            {
                yield return gbx.Node;
            }
        }
    }

    /// <summary>
    /// Enumerates through all of the embedded items and yields their header data.
    /// </summary>
    /// <returns>An enumerable of <see cref="CGameItemModel"/> nodes with header data only.</returns>
    public IEnumerable<CGameItemModel> GetEmbeddedItemModelHeaders()
    {
        foreach (var node in GetEmbeddedNodeHeaders())
        {
            if (node is CGameItemModel itemModel)
            {
                yield return itemModel;
            }
        }
    }

    /// <summary>
    /// Enumerates through all of the embedded nodes and yields them entirely.
    /// </summary>
    /// <returns>An enumerable of nodes.</returns>
    public IEnumerable<Node> GetEmbeddedNodes()
    {
        foreach (var gbx in GetEmbeddedGbxs())
        {
            if (gbx.Node is not null)
            {
                yield return gbx.Node;
            }
        }
    }

    /// <summary>
    /// Enumerates through all of the embedded items and yields them entirely.
    /// </summary>
    /// <returns>An enumerable of <see cref="CGameItemModel"/> nodes.</returns>
    public IEnumerable<CGameItemModel> GetEmbeddedItemModels()
    {
        foreach (var node in GetEmbeddedNodes())
        {
            if (node is CGameItemModel itemModel)
            {
                yield return itemModel;
            }
        }
    }

    /// <summary>
    /// Enumerates through all of the embedded materials and yields them entirely.
    /// </summary>
    /// <returns>An enumerable of <see cref="CPlugMaterialUserInst"/> nodes.</returns>
    public IEnumerable<CPlugMaterialUserInst> GetEmbeddedMaterials()
    {
        foreach (var node in GetEmbeddedNodes())
        {
            if (node is CPlugMaterialUserInst mat)
            {
                yield return mat;
            }
        }
    }

    /// <summary>
    /// Extracts embed ZIP file based on the data in <see cref="EmbeddedData"/>. File metadata is simplified and the timestamp of extraction is used for all files. Stream must have permission to read.
    /// </summary>
    /// <param name="stream">Stream to write the ZIP data to.</param>
    /// <returns>False if there's nothing to extract, otherwise true.</returns>
    public bool ExtractEmbedZip(Stream stream)
    {
        if (EmbeddedData is null || EmbeddedData.Count == 0)
        {
            return false;
        }

        using var zip = new ZipArchive(stream, ZipArchiveMode.Create, true);

        foreach (var embed in EmbeddedData)
        {
            var entry = zip.CreateEntry(embed.Key.Replace('\\', '/'));

            using var s = entry.Open();

            s.Write(embed.Value, 0, embed.Value.Length);
        }

        return true;
    }

    /// <summary>
    /// Extracts embed ZIP file based on the data in <see cref="EmbeddedData"/>. File metadata is simplified and the timestamp of extraction is used for all files.
    /// </summary>
    /// <param name="fileName">New file to write the ZIP data to.</param>
    /// <exception cref="MemberNullException"><see cref="EmbeddedData"/> is null.</exception>
    public void ExtractEmbedZip(string fileName)
    {
        using var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);

        ExtractEmbedZip(fs); // ExtractEmbedZip(Stream stream) required Stream to be readable as well
    }

    /// <summary>
    /// Extracts embed ZIP file straight from the parsed map including original timestamps and other file metadata.
    /// </summary>
    /// <param name="stream">Stream to write the ZIP data to.</param>
    /// <returns>True if the map included embed ZIP previously, otherwise false.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
    /// <exception cref="NotSupportedException"><paramref name="stream"/> does not support writing.</exception>
    /// <exception cref="ObjectDisposedException"><paramref name="stream"/> was disposed.</exception>
    /// <exception cref="IOException">An I/O error occurred.</exception>
    public bool ExtractOriginalEmbedZip(Stream stream)
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));

        DiscoverChunk<Chunk03043054>();

        if (originalEmbedZip is null || originalEmbedZip.Length == 0)
        {
            return false;
        }

        using var ms = new MemoryStream(originalEmbedZip);

        ms.CopyTo(stream);

        return true;
    }

    /// <summary>
    /// Extracts embed ZIP file straight from the parsed map including original timestamps and other file metadata.
    /// </summary>
    /// <param name="fileName">New file to write the ZIP data to.</param>
    /// <returns>True if the map included embed ZIP previously, otherwise false.</returns>
    /// <exception cref="ArgumentException"><paramref name="fileName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by System.IO.Path.InvalidPathChars.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fileName"/> is null.</exception>
    /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="UnauthorizedAccessException"><paramref name="fileName"/> specified a file that is read-only. -or- <paramref name="fileName"/> specified a file that is hidden. -or- This operation is not supported on the current platform. -or- <paramref name="fileName"/> specified a directory. -or- The caller does not have the required permission.</exception>
    /// <exception cref="NotSupportedException"><paramref name="fileName"/> is in an invalid format.</exception>
    /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
    public bool ExtractOriginalEmbedZip(string fileName)
    {
        DiscoverChunk<Chunk03043054>();

        if (originalEmbedZip is null || originalEmbedZip.Length == 0)
        {
            return false;
        }

        File.WriteAllBytes(fileName, originalEmbedZip);

        return true;
    }

    /// <summary>
    /// Import a file to embed in the map by keeping the file name but relocating it in the embed ZIP.
    /// </summary>
    /// <param name="fileOnDisk">File to embed located on the disk.</param>
    /// <param name="relativeDirectory">Relative directory where the embed should be represented in the game, usually starts with <c>"Items/..."</c>, <c>"Blocks/..."</c> or <c>"Materials/..."</c>.</param>
    /// <param name="keepIcon">Keep the icon (chunk 0x2E001004) of the embedded GBX. Increases total embed size that is technically not needed.</param>
    /// <exception cref="MemberNullException"><see cref="EmbeddedData"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="fileOnDisk"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by System.IO.Path.InvalidPathChars.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fileOnDisk"/> is null.</exception>
    /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="UnauthorizedAccessException"><paramref name="fileOnDisk"/> specified a file that is read-only. -or- <paramref name="fileOnDisk"/> specified a file that is hidden. -or- This operation is not supported on the current platform. -or- <paramref name="fileOnDisk"/> specified a directory. -or- The caller does not have the required permission.</exception>
    /// <exception cref="FileNotFoundException">The file specified in path was not found.</exception>
    /// <exception cref="NotSupportedException"><paramref name="fileOnDisk"/> is in an invalid format.</exception>
    /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
    public void ImportFileToEmbed(string fileOnDisk, string relativeDirectory, bool keepIcon = false)
    {
        if (EmbeddedData is null)
            throw new MemberNullException(nameof(EmbeddedData));

        var data = File.ReadAllBytes(fileOnDisk);

        if (!keepIcon)
        {
            RemoveIconFromGbxData(ref data);
        }

        EmbeddedData[relativeDirectory + "/" + Path.GetFileName(fileOnDisk)] = data;
    }

    private static void RemoveIconFromGbxData(ref byte[] data)
    {
        using var gbxms = new MemoryStream(data);
        using var gbxr = new GameBoxReader(gbxms);

        if (!gbxr.HasMagic(GameBox.Magic))
        {
            return;
        }

        using var gbxOutms = new MemoryStream();
        using var gbxOutw = new GameBoxWriter(gbxOutms);

        var basic = gbxr.ReadBytes(6); // gbx basic

        var classID = gbxr.ReadUInt32();

        var userDataPos = gbxms.Position;

        var userData = gbxr.ReadBytes();

        using var msNewUserData = new MemoryStream();
        using var wNewUserData = new GameBoxWriter(msNewUserData);
        using var msUserData = new MemoryStream(userData);
        using var rUserData = new GameBoxReader(msUserData);

        var headers = rUserData.ReadArray(r => (
            chunkID: r.ReadUInt32(),
            size: (int)(r.ReadInt32() & ~0x80000000))
        );

        var contains004 = false;

        foreach (var (chunkID, size) in headers)
        {
            if (chunkID == 0x2E001004)
            {
                wNewUserData.Write(headers.Length - 1);
                contains004 = true;
            }
        }

        if (!contains004) wNewUserData.Write(headers.Length);

        foreach (var (chunkID, size) in headers)
        {
            if (chunkID != 0x2E001004)
            {
                wNewUserData.Write(chunkID);
                wNewUserData.Write(size);
            }
        }

        foreach (var (chunkID, size) in headers)
        {
            var chunkData = rUserData.ReadBytes(size);

            if (chunkID != 0x2E001004)
            {
                wNewUserData.Write(chunkData);
            }
        }

        gbxOutw.Write("GBX", StringLengthPrefix.None);
        gbxOutw.Write(basic, 0, basic.Length);
        gbxOutw.Write(classID);
        gbxOutw.Write((int)msNewUserData.Length);
        gbxOutw.Write(msNewUserData.ToArray(), 0, (int)msNewUserData.Length);

        gbxms.CopyTo(gbxOutms);

        data = gbxOutms.ToArray();
    }

    /// <summary>
    /// Embed objects from directories represented like from the user data directory.
    /// </summary>
    /// <param name="directoryOnDisk">Directory with folders <c>"Items/..."</c>, <c>"Blocks/..."</c> or <c>"Materials/..."</c>.</param>
    /// <exception cref="NotImplementedException">This method is not implemented yet.</exception>
    public void ImportUserDataToEmbed(string directoryOnDisk)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Place a macroblock instance on the map.
    /// </summary>
    /// <param name="macroblock">Macroblock template to place.</param>
    /// <param name="coord">Position on the map to place the macroblock on. Root coordinate of the macroblock is considered.</param>
    /// <param name="dir">Direction of the placed macroblock.</param>
    /// <exception cref="MemberNullException"><see cref="Blocks"/> is null.</exception>
    public void PlaceMacroblock(CGameCtnMacroBlockInfo macroblock, Int3 coord, Direction dir)
    {
        _ = Blocks ?? throw new MemberNullException(nameof(Blocks));

        if (macroblock.BlockSpawns is null)
        {
            return; // TODO: Support macroblock placing for item-only macroblocks (if they are possible)
        }

        var macroRad = (int)dir * (Math.PI / 2); // Rotation of the macroblock in radians needed for the formula to determine individual coords
        var macroBlocks = macroblock.BlockSpawns.Where(x => !x.IsFree).ToArray();
        var allCoords = macroBlocks.Select(x => x.Coord); // Creates an enumerable of macroblock block coords

        var min = new Int3(
            allCoords.Select(x => x.GetValueOrDefault().X).Min(),
            allCoords.Select(x => x.GetValueOrDefault().Y).Min(),
            allCoords.Select(x => x.GetValueOrDefault().Z).Min());
        var max = new Int3(
            allCoords.Select(x => x.GetValueOrDefault().X).Max(),
            allCoords.Select(x => x.GetValueOrDefault().Y).Max(),
            allCoords.Select(x => x.GetValueOrDefault().Z).Max());
        // Calculates the minimum and maximum coord, used to determine size and center of the macroblock

        var size = max - min + (1, 1, 1);
        var center = (min + max) * .5f;

        var newCoords = new Vec3[macroBlocks.Length]; // Array used to store new rotated block positions

        for (var i = 0; i < newCoords.Length; i++)
        {
            var block = macroBlocks[i];
            var blockCoord = (Vec3)block.Coord.GetValueOrDefault();

            var blockCenter = new Vec3();
            // Temporary center variable whose rule is to properly position blocks over size of 1x1

            if (BlockInfoManager.BlockModels.TryGetValue(block.Name, out BlockModel? model)) // Get quick block information if available
            {
                var blockUnits = block.IsGround ? model.Ground : model.Air;
                // Use the block units from what the block actually pretends to be placed on

                if (blockUnits?.Length > 1) // Optimization for blocks that are simple 1x1 size
                {
                    var blockAllCoords = Array.ConvertAll(blockUnits.Select(x => x.Coord).ToArray(), x => (Int3)x); // Gets the coords in Int3 type

                    var blockMax = new Int3(blockAllCoords.Select(x => x.X).Max(), blockAllCoords.Select(x => x.Y).Max(), blockAllCoords.Select(x => x.Z).Max());
                    // Calculates only the maximum, due to all macroblocks having a natural minimum of (0, 0, 0)

                    blockCenter = blockMax * .5f;
                    blockCoord += (blockCenter.X, 0, blockCenter.Z);
                    // Makes the block pretend that it has a centered coordinates for the whole macroblock rotation
                }
            }

            var offsetX = Math.Cos(macroRad) * (blockCoord.X - center.X) - Math.Sin(macroRad) * (blockCoord.Z - center.Z) + center.X;
            var offsetZ = Math.Sin(macroRad) * (blockCoord.X - center.X) + Math.Cos(macroRad) * (blockCoord.Z - center.Z) + center.Z;
            // Calculates the new XZ positions using "rotation around another point" formula

            offsetX -= blockCenter.X;
            offsetZ -= blockCenter.Z;
            // If the center is different than 0 0 (the block coordinates pretended to be in the middle), fix the coord back to its normal coordination

            newCoords[i] = new Vec3((float)offsetX, block.Coord.GetValueOrDefault().Y, (float)offsetZ); // Applies the result to the array, Y isn't affected
        }

        var newMin = new Vec3(newCoords.Select(x => x.X).Min(), newCoords.Select(x => x.Y).Min(), newCoords.Select(x => x.Z).Min());
        // Calculates the new minimum coord of the rotated coordinates
        // This value will be always (0, 0, 0) on 1:1 macroblock size ratios, on other size ratios, this will vary
        // Macroblock placement behaviour in ManiaPlanet works by rotating around the center, and moving the blocks to the zero relative coord as a group

        for (var i = 0; i < newCoords.Length; i++)
        {
            var block = macroBlocks[i];
            var newRelativeCoord = newCoords[i] - newMin; // Use the newly rotated coordinates, and substract the shift the rotation made to the entire macroblock

            var b = new CGameCtnBlock(
                name: block.Name,
                direction: Dir.Add(block.Direction ?? Direction.North, dir),
                coord: coord + (Int3)newRelativeCoord,
                flags: block.Flags,
                author: block.BlockModel.Author,
                skin: null,
                waypoint: block.Waypoint
            );

            if (b.Coord.Y == 0)
                b.IsGround = true;

            Blocks.Add(b);
        }



        var blockSize = Collection.GetBlockSize();

        if (macroblock.ObjectSpawns is not null)
        {
            foreach (var item in macroblock.ObjectSpawns)
            {
                var itemRadians = (float)((int)dir * Math.PI / 2);
                var blockCenterVec = size * blockSize * new Vec3(0.5f, 0f, 0.5f);
                var offsetPos = AdditionalMath.RotateAroundCenter(item.AbsolutePositionInMap, blockCenterVec, itemRadians);
                offsetPos -= newMin * blockSize;

                Int3 offsetCollection = (0, blockSize.Y, 0);

                if (TryGetChunk(out Chunk0304301F? chunk01F))
                {
                    if (chunk01F!.Version <= 1)
                    {
                        offsetCollection += (32, 0, 32);
                    }
                }

                PlaceAnchoredObject(item.ItemModel, offsetPos + coord * blockSize + offsetCollection, item.PitchYawRoll + (-itemRadians, 0f, 0f));
            }
        }

        foreach (var freeBlock in macroblock.BlockSpawns.Where(x => x.IsFree))
        {
            //PlaceFreeBlock(freeBlock.Name, (coord + (0, 1, 0)) * Collection.GetBlockSize() + freeBlock.AbsolutePositionInMap, freeBlock.PitchYawRoll);
        }
    }

    #endregion

    #region Chunks

    #region 0x001 header chunk (Virtual Skipper)

    /// <summary>
    /// CGameCtnChallenge 0x001 header chunk (Virtual Skipper)
    /// </summary>
    [Chunk(0x03043001, "Virtual Skipper")]
    public class Chunk03043001 : HeaderChunk<CGameCtnChallenge>, IVersionable
    {
        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; }

        public BoatName BoatName { get; set; }
        public string? Boat { get; set; }
        public string? BoatAuthor { get; set; }
        public RaceMode RaceMode { get; set; }
        public WindDirection WindDirection { get; set; }
        public byte WindStrength { get; set; }
        public Weather Weather { get; set; }
        public StartDelay StartDelay { get; set; }
        public int StartTime { get; set; }
        public TimeInt32 TimeLimit { get; set; } = TimeInt32.Zero;
        public bool NoPenalty { get; set; }
        public bool InflPenalty { get; set; }
        public bool FinishFirst { get; set; }
        public byte NbAIs { get; set; }
        public float CourseLength { get; set; }
        public int WindShiftDuration { get; set; }
        public int WindShiftAngle { get; set; }
        public bool ExactWind { get; set; }
        public int SpawnPoints { get; set; }
        public AILevel AILevel { get; set; }
        public bool SmallShifts { get; set; }
        public bool NoRules { get; set; }
        public bool StartSailUp { get; set; }

        public bool U01;
        public int U02;
        public byte? U03;
        public byte U04;
        public byte U05;
        public byte U06;
        public byte U07;
        public bool U08;
        public string? U09;

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionByte(this);

            if (Version < 1)
            {
                rw.Ident(ref n.mapInfo!);
                rw.String(ref n.mapName!);
            }

            U01 = rw.Boolean(U01);
            U02 = rw.Int32(U02);

            if (Version < 1)
                U03 = rw.Byte(U03.GetValueOrDefault());

            U04 = rw.Byte(U04);

            if (Version < 9)
                BoatName = (BoatName)rw.Byte((byte)BoatName);

            if (Version >= 9)
                Boat = rw.Id(Boat);

            if (Version >= 12)
                BoatAuthor = rw.Id(BoatAuthor);

            RaceMode = (RaceMode)rw.Byte((byte)RaceMode);
            U05 = rw.Byte(U05);
            WindDirection = (WindDirection)rw.Byte((byte)RaceMode);
            WindStrength = rw.Byte(WindStrength);
            Weather = (Weather)rw.Byte((byte)Weather);
            U06 = rw.Byte(U06);
            StartDelay = (StartDelay)rw.Byte((byte)StartDelay);
            StartTime = rw.Int32(StartTime);

            if (Version >= 2)
            {
                TimeLimit = TimeInt32.FromMilliseconds(rw.Int32((int)TimeLimit.TotalMilliseconds));
                NoPenalty = rw.Boolean(NoPenalty);
                InflPenalty = rw.Boolean(InflPenalty);
                FinishFirst = rw.Boolean(FinishFirst);

                if (Version >= 3)
                {
                    NbAIs = rw.Byte(NbAIs);

                    if (Version >= 4)
                    {
                        CourseLength = rw.Single(CourseLength);

                        if (Version >= 5)
                        {
                            WindShiftAngle = rw.Int32(WindShiftAngle);
                            U07 = rw.Byte(U07);

                            if (Version == 6 || Version == 7)
                            {
                                U08 = rw.Boolean(U08);
                                U09 = rw.String(U09);
                            }

                            if (Version >= 7)
                            {
                                ExactWind = !rw.Boolean(!ExactWind);

                                if (Version >= 10)
                                {
                                    SpawnPoints = rw.Int32(SpawnPoints);

                                    if (Version >= 11)
                                    {
                                        AILevel = (AILevel)rw.Byte((byte)AILevel);

                                        if (Version >= 13)
                                        {
                                            SmallShifts = rw.Boolean(SmallShifts);

                                            if (Version >= 14)
                                            {
                                                NoRules = rw.Boolean(NoRules);
                                                StartSailUp = rw.Boolean(StartSailUp);
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

    #endregion

    #region 0x002 header chunk (map info)

    /// <summary>
    /// CGameCtnChallenge 0x002 header chunk (map info)
    /// </summary>
    [Chunk(0x03043002, "map info")]
    public class Chunk03043002 : HeaderChunk<CGameCtnChallenge>, IVersionable
    {
        public bool U01;
        public byte U02;
        public int? U03;
        public int U04;
        public int U05;

        /// <summary>
        /// Version of the chunk. TM1.0 starts with version 3.
        /// </summary>
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionByte(this);

            if (Version < 3)
            {
                rw.Ident(ref n.mapInfo!);
                rw.String(ref n.mapName!);
            }

            rw.Boolean(ref U01);

            if (Version >= 1)
            {
                rw.TimeInt32Nullable(ref n.bronzeTime);
                rw.TimeInt32Nullable(ref n.silverTime);
                rw.TimeInt32Nullable(ref n.goldTime);
                rw.TimeInt32Nullable(ref n.authorTime);

                if (Version == 2)
                    rw.Byte(ref U02);

                if (Version >= 4)
                {
                    rw.Int32(ref n.cost);

                    if (Version >= 5)
                    {
                        rw.Boolean(ref n.isLapRace);

                        if (Version == 6)
                            rw.Int32(ref U03);

                        if (Version >= 7)
                        {
                            rw.EnumInt32<PlayMode>(ref n.mode);

                            if (Version >= 9)
                            {
                                rw.Int32(ref U04);

                                if (Version >= 10)
                                {
                                    rw.Int32(ref n.authorScore);

                                    if (Version >= 11)
                                    {
                                        rw.EnumInt32<EditorMode>(ref n.editor);

                                        if (Version >= 12)
                                        {
                                            rw.Int32(ref U05);

                                            if (Version >= 13)
                                            {
                                                rw.Int32(ref n.nbCheckpoints);

                                                // Trick to keep the same value when the body is read, as body stores something different
                                                n.nbLaps = rw.Int32(n.isLapRace.GetValueOrDefault() ? n.nbLaps : 1, defaultValue: 3);

                                                if (Version >= 14)
                                                {
                                                    throw new VersionNotSupportedException(Version);
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
    }

    #endregion

    #region 0x003 header chunk (common)

    /// <summary>
    /// CGameCtnChallenge 0x003 header chunk (common)
    /// </summary>
    [Chunk(0x03043003, "common")]
    public class Chunk03043003 : HeaderChunk<CGameCtnChallenge>, IVersionable
    {
        public byte[] U01 = new byte[16];
        public int U02;
        public uint? U03;

        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionByte(this);
            rw.Ident(ref n.mapInfo!);
            rw.String(ref n.mapName!);
            rw.EnumByte<MapKind>(ref n.kindInHeader);

            if (Version >= 1)
            {
                rw.UInt32(ref U03); // 'Locked' but actually not
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
                                rw.Bytes(ref U01!, 16);

                                if (Version >= 6)
                                {
                                    rw.String(ref n.mapType);
                                    rw.String(ref n.mapStyle);

                                    if (Version <= 8)
                                        rw.Int32(ref U02);

                                    if (Version >= 8)
                                    {
                                        rw.UInt64(ref n.lightmapCacheUID);

                                        if (Version >= 9)
                                        {
                                            rw.Byte(ref n.lightmapVersion);

                                            if (Version >= 11)
                                            {
                                                rw.Id(ref n.titleID);

                                                if (Version >= 12)
                                                {
                                                    throw new VersionNotSupportedException(Version);
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
    }

    #endregion

    #region 0x004 header chunk (version)

    /// <summary>
    /// CGameCtnChallenge 0x004 header chunk (version)
    /// </summary>
    [Chunk(0x03043004, "version")]
    public class Chunk03043004 : HeaderChunk<CGameCtnChallenge>, IVersionable
    {
        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
        }
    }

    #endregion

    #region 0x005 header chunk (xml)

    /// <summary>
    /// CGameCtnChallenge 0x005 header chunk (xml)
    /// </summary>
    [Chunk(0x03043005, "xml")]
    public class Chunk03043005 : HeaderChunk<CGameCtnChallenge>
    {
        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.xml);
        }
    }

    #endregion

    #region 0x007 header chunk (thumbnail)

    /// <summary>
    /// CGameCtnChallenge 0x007 header chunk (thumbnail)
    /// </summary>
    [Chunk(0x03043007, "thumbnail")]
    public class Chunk03043007 : HeaderChunk<CGameCtnChallenge>, IVersionable
    {
        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; } = 1;

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version == 0)
            {
                return;
            }

            var thumbnailSize = rw.Int32(n.thumbnail?.Length ?? 0);

            rw.Bytes(Encoding.UTF8.GetBytes("<Thumbnail.jpg>"), "<Thumbnail.jpg>".Length); // Because the string is purely ASCII anyway, Length is usable
            rw.Bytes(ref n.thumbnail, thumbnailSize);
            rw.Bytes(Encoding.UTF8.GetBytes("</Thumbnail.jpg>"), "</Thumbnail.jpg>".Length);

            rw.Bytes(Encoding.UTF8.GetBytes("<Comments>"), "<Comments>".Length);
            rw.String(ref n.comments);
            rw.Bytes(Encoding.UTF8.GetBytes("</Comments>"), "</Comments>".Length);
        }
    }

    #endregion

    #region 0x008 header chunk (author)

    /// <summary>
    /// CGameCtnChallenge 0x008 header chunk (author)
    /// </summary>
    [Chunk(0x03043008, "author")]
    public class Chunk03043008 : HeaderChunk<CGameCtnChallenge>, IVersionable
    {
        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; } = 1;

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref n.authorVersion);
            rw.String(ref n.authorLogin!);
            rw.String(ref n.authorNickname);
            rw.String(ref n.authorZone);
            rw.String(ref n.authorExtraInfo);
        }
    }

    #endregion

    #region 0x00D chunk (vehicle)

    /// <summary>
    /// CGameCtnChallenge 0x00D chunk (vehicle)
    /// </summary>
    [Chunk(0x0304300D, "vehicle")]
    public class Chunk0304300D : Chunk<CGameCtnChallenge>
    {
        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.Ident(ref n.playerModel);
        }
    }

    #endregion

    #region 0x00F chunk (TM1.0 block data)

    /// <summary>
    /// CGameCtnChallenge 0x00F chunk (TM1.0 block data)
    /// </summary>
    [Chunk(0x0304300F, "TM1.0 block data")]
    public class Chunk0304300F : Chunk<CGameCtnChallenge>
    {
        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.Ident(ref n.mapInfo!);
            rw.Int3(ref n.size);
            rw.Int32(10);
            rw.ListNode<CGameCtnBlock>(ref n.blocks!);
            rw.Boolean(ref n.needUnlock);
            rw.Ident(ref n.decoration);
        }

        public override async Task ReadWriteAsync(CGameCtnChallenge n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
        {
            rw.Ident(ref n.mapInfo!);
            rw.Int3(ref n.size);
            rw.Int32(10);
            n.blocks = (await rw.ListNodeAsync<CGameCtnBlock>(n.blocks!, cancellationToken))!;
            rw.Boolean(ref n.needUnlock);
            rw.Ident(ref n.decoration);
        }
    }

    #endregion

    #region 0x011 chunk (parameters)

    /// <summary>
    /// CGameCtnChallenge 0x011 chunk (parameters)
    /// </summary>
    [Chunk(0x03043011, "parameters")]
    public class Chunk03043011 : Chunk<CGameCtnChallenge>
    {
        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnCollectorList>(ref n.blockStock);
            rw.NodeRef<CGameCtnChallengeParameters>(ref n.challengeParameters);
            rw.EnumInt32<MapKind>(ref n.kind);
        }

        public override async Task ReadWriteAsync(CGameCtnChallenge n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
        {
            n.blockStock = await rw.NodeRefAsync(n.blockStock, cancellationToken);
            n.challengeParameters = await rw.NodeRefAsync(n.challengeParameters, cancellationToken);
            rw.EnumInt32<MapKind>(ref n.kind);
        }
    }

    #endregion

    #region 0x012 chunk (TM1.0 map name)

    /// <summary>
    /// CGameCtnChallenge 0x012 chunk (TM1.0 map name)
    /// </summary>
    [Chunk(0x03043012, "TM1.0 map name")]
    public class Chunk03043012 : Chunk<CGameCtnChallenge>
    {
        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.mapName!);
        }
    }

    #endregion

    #region 0x013 chunk (legacy block data)

    /// <summary>
    /// CGameCtnChallenge 0x013 chunk (legacy block data)
    /// </summary>
    [Chunk(0x03043013, "legacy block data")]
    public class Chunk03043013 : Chunk<CGameCtnChallenge>
    {
        public Chunk0304301F Chunk01F { get; }

        public Chunk03043013()
        {
            Chunk01F = new Chunk0304301F(this);
        }

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            Chunk01F.ReadWrite(n, rw);
        }
    }

    #endregion

    #region 0x014 skippable chunk (legacy password)

    /// <summary>
    /// CGameCtnChallenge 0x014 skippable chunk (legacy password)
    /// </summary>
    [Chunk(0x03043014, "legacy password")]
    public class Chunk03043014 : SkippableChunk<CGameCtnChallenge>
    {
        public bool U01;

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.String(ref n.password);
        }
    }

    #endregion

    #region 0x016 skippable chunk

    /// <summary>
    /// CGameCtnChallenge 0x016 skippable chunk
    /// </summary>
    [Chunk(0x03043016)]
    public class Chunk03043016 : SkippableChunk<CGameCtnChallenge>
    {
        public int U01;

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x017 skippable chunk (checkpoints)

    /// <summary>
    /// CGameCtnChallenge 0x017 skippable chunk (checkpoints)
    /// </summary>
    [Chunk(0x03043017, "checkpoints")]
    public class Chunk03043017 : SkippableChunk<CGameCtnChallenge>
    {
        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.Array(ref n.checkpoints, r => r.ReadInt3(), (x, w) => w.Write(x));
        }
    }

    #endregion

    #region 0x018 skippable chunk (laps)

    /// <summary>
    /// CGameCtnChallenge 0x018 skippable chunk (laps)
    /// </summary>
    [Chunk(0x03043018, "laps")]
    public class Chunk03043018 : SkippableChunk<CGameCtnChallenge>
    {
        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.isLapRace);
            rw.Int32(ref n.nbLaps);
        }
    }

    #endregion

    #region 0x019 skippable chunk (mod)

    /// <summary>
    /// CGameCtnChallenge 0x019 skippable chunk (mod)
    /// </summary>
    [Chunk(0x03043019, "mod")]
    public class Chunk03043019 : SkippableChunk<CGameCtnChallenge>
    {
        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.FileRef(ref n.modPackDesc);
        }
    }

    #endregion

    #region 0x01A chunk

    /// <summary>
    /// CGameCtnChallenge 0x01A chunk
    /// </summary>
    [Chunk(0x0304301A)]
    public class Chunk0304301A : Chunk<CGameCtnChallenge>
    {
        public CMwNod? U01;

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01); // assert: '!ReplayRecord || !ReplayRecord->m_Challenge' failed.
        }

        public override async Task ReadWriteAsync(CGameCtnChallenge n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
        {
            U01 = await rw.NodeRefAsync(U01, cancellationToken);
        }
    }

    #endregion

    #region 0x01B chunk (OldIgs)

    /// <summary>
    /// CGameCtnChallenge 0x01B chunk (OldIgs)
    /// </summary>
    [Chunk(0x0304301B, "OldIgs")]
    public class Chunk0304301B : Chunk<CGameCtnChallenge>
    {
        public int U01;

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01); // SOldIgs array

            if (U01 > 0)
            {
                throw new NotSupportedException("SOldIgs count > 0");
            }
        }
    }

    #endregion

    #region 0x01C skippable chunk (play mode)

    /// <summary>
    /// CGameCtnChallenge 0x01C skippable chunk (play mode)
    /// </summary>
    [Chunk(0x0304301C, "play mode")]
    public class Chunk0304301C : SkippableChunk<CGameCtnChallenge>
    {
        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.EnumInt32<PlayMode>(ref n.mode);
        }
    }

    #endregion

    #region 0x01D chunk

    /// <summary>
    /// CGameCtnChallenge 0x01D chunk
    /// </summary>
    [Chunk(0x0304301D)]
    public class Chunk0304301D : Chunk<CGameCtnChallenge>
    {
        public CMwNod? U01;

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01); // assert: '!ReplayRecord || !ReplayRecord->m_Challenge' failed.
        }

        public override async Task ReadWriteAsync(CGameCtnChallenge n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
        {
            U01 = await rw.NodeRefAsync(U01, cancellationToken);
        }
    }

    #endregion

    #region 0x01F chunk (block data)

    /// <summary>
    /// CGameCtnChallenge 0x01F chunk (block data)
    /// </summary>
    [Chunk(0x0304301F, "block data")]
    public class Chunk0304301F : Chunk<CGameCtnChallenge>, IVersionable
    {
        private readonly bool is013;

        public bool NeedUnlock { get; set; }

        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; }

        public Chunk0304301F() : this(null)
        {

        }

        public Chunk0304301F(Chunk? chunk)
        {
            is013 = chunk is Chunk03043013;
        }

        public override void Read(CGameCtnChallenge n, GameBoxReader r)
        {
            n.mapInfo = r.ReadIdent();
            n.mapName = r.ReadString();
            n.decoration = r.ReadIdent();
            n.size = r.ReadInt3();
            NeedUnlock = r.ReadBoolean();

            if (!is013)
            {
                Version = r.ReadInt32();
            }

            if (Version > 6)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            var nbBlocks = r.ReadInt32();

            n.blocks = new List<CGameCtnBlock>(nbBlocks);

            for (var i = 0; i < nbBlocks; i++)
            {
                var isNormalBlock = ReadAndAddBlock(n, r, Version);

                if (isNormalBlock)
                {
                    continue;
                }

                i--;
            }

            while ((r.PeekUInt32() & 0xC0000000) > 0)
            {
                ReadAndAddBlock(n, r, Version);
            }
        }

        // Returns true if it's a normal block
        private static bool ReadAndAddBlock(CGameCtnChallenge n, GameBoxReader r, int version)
        {
            var blockName = r.ReadId();
            var dir = (Direction)r.ReadByte();
            var coord = (Int3)r.ReadByte3();

            var flags = version switch
            {
                0 => r.ReadUInt16(),
                > 0 => r.ReadInt32(),
                _ => throw new ChunkVersionNotSupportedException(version),
            };

            if (flags == -1)
            {
                n.blocks!.Add(new CGameCtnBlock(blockName, dir, coord, flags));
                return false;
            }

            if (version >= 6)
            {
                coord -= (1, 0, 1);
            }

            var author = default(string?);
            var skin = default(CGameCtnBlockSkin?);

            if (CGameCtnBlock.IsSkinnableBlock_WhenDefined(flags)) // custom block
            {
                author = r.ReadId();
                skin = r.ReadNodeRef<CGameCtnBlockSkin>();
            }

            var parameters = default(CGameWaypointSpecialProperty?);

            if (CGameCtnBlock.IsWaypointBlock_WhenDefined(flags))
            {
                parameters = r.ReadNodeRef<CGameWaypointSpecialProperty>();
            }

            if (CGameCtnBlock.IsFreeBlock_WhenDefined(flags))
            {
                coord -= (0, 1, 0);
            }

            var block = new CGameCtnBlock(blockName, dir, coord, flags, author, skin, parameters);

            n.blocks!.Add(block);

            return true;
        }

        public override void Write(CGameCtnChallenge n, GameBoxWriter w)
        {
            w.Write(n.mapInfo);
            w.Write(n.mapName);
            w.Write(n.decoration);
            w.Write(n.size.GetValueOrDefault());
            w.Write(NeedUnlock);

            if (!is013)
            {
                w.Write(Version);
            }

            w.Write(n.NbBlocks.GetValueOrDefault());

            if (n.blocks is null)
            {
                return;
            }

            foreach (var x in n.blocks)
            {
                w.WriteId(x.Name);
                w.Write((byte)x.Direction);

                var coord = x.Coord;

                if (Version >= 6 && x.Flags != -1)
                {
                    coord += (1, 0, 1);
                }

                if (CGameCtnBlock.IsFreeBlock(x.Flags))
                {
                    coord += (0, 1, 0);
                }

                w.Write((Byte3)coord);

                switch (Version)
                {
                    case 0: w.Write((short)x.Flags); break;
                    case > 0: w.Write(x.Flags); break;
                    default: throw new ChunkVersionNotSupportedException(Version);
                }

                if (x.Flags == -1)
                {
                    continue;
                }

                if (CGameCtnBlock.IsSkinnableBlock_WhenDefined(x.Flags)) // custom block
                {
                    w.WriteId(x.Author);
                    w.Write(x.Skin);
                }

                if (CGameCtnBlock.IsWaypointBlock_WhenDefined(x.Flags))
                {
                    w.Write(x.WaypointSpecialProperty);
                }
            }
        }
    }

    #endregion

    #region 0x020 chunk (legacy legacy mediatracker)

    /// <summary>
    /// CGameCtnChallenge 0x020 chunk (legacy legacy mediatracker)
    /// </summary>
    [Chunk(0x03043020, "legacy legacy mediatracker")]
    public class Chunk03043020 : Chunk<CGameCtnChallenge>
    {
        public Node? U01;
        public Node? U02;

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnMediaClip>(ref n.clipIntro);
            rw.NodeRef(ref U01);
            rw.NodeRef(ref U02);
            rw.NodeRef<CGameCtnMediaClipGroup>(ref n.clipGroupInGame);
            rw.NodeRef<CGameCtnMediaClipGroup>(ref n.clipGroupEndRace);
        }

        public override async Task ReadWriteAsync(CGameCtnChallenge n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
        {
            n.clipIntro = await rw.NodeRefAsync<CGameCtnMediaClip>(n.clipIntro, cancellationToken);
            U01 = await rw.NodeRefAsync(U01, cancellationToken);
            U02 = await rw.NodeRefAsync(U02, cancellationToken);
            n.clipGroupInGame = await rw.NodeRefAsync<CGameCtnMediaClipGroup>(n.clipGroupInGame, cancellationToken);
            n.clipGroupEndRace = await rw.NodeRefAsync<CGameCtnMediaClipGroup>(n.clipGroupEndRace, cancellationToken);
        }
    }

    #endregion

    #region 0x021 chunk (legacy mediatracker)

    /// <summary>
    /// CGameCtnChallenge 0x021 chunk (legacy mediatracker)
    /// </summary>
    [Chunk(0x03043021, "legacy mediatracker")]
    public class Chunk03043021 : Chunk<CGameCtnChallenge>
    {
        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnMediaClip>(ref n.clipIntro);
            rw.NodeRef<CGameCtnMediaClipGroup>(ref n.clipGroupInGame);
            rw.NodeRef<CGameCtnMediaClipGroup>(ref n.clipGroupEndRace);
        }

        public override async Task ReadWriteAsync(CGameCtnChallenge n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
        {
            n.clipIntro = await rw.NodeRefAsync<CGameCtnMediaClip>(n.clipIntro, cancellationToken);
            n.clipGroupInGame = await rw.NodeRefAsync<CGameCtnMediaClipGroup>(n.clipGroupInGame, cancellationToken);
            n.clipGroupEndRace = await rw.NodeRefAsync<CGameCtnMediaClipGroup>(n.clipGroupEndRace, cancellationToken);
        }
    }

    #endregion

    #region 0x022 chunk

    /// <summary>
    /// CGameCtnChallenge 0x022 chunk
    /// </summary>
    [Chunk(0x03043022)]
    public class Chunk03043022 : Chunk<CGameCtnChallenge>
    {
        public int U01;

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x023 chunk (map origin)

    /// <summary>
    /// CGameCtnChallenge 0x023 chunk (map origin)
    /// </summary>
    [Chunk(0x03043023, "map origin")]
    public class Chunk03043023 : Chunk<CGameCtnChallenge>
    {
        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.Vec2(ref n.mapCoordOrigin);
            n.mapCoordTarget = n.mapCoordOrigin;
        }
    }

    #endregion

    #region 0x024 chunk (music)

    /// <summary>
    /// CGameCtnChallenge 0x024 chunk (music)
    /// </summary>
    [Chunk(0x03043024, "music")]
    public class Chunk03043024 : Chunk<CGameCtnChallenge>
    {
        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.FileRef(ref n.customMusicPackDesc);
        }
    }

    #endregion

    #region 0x025 chunk (map origin and target)

    /// <summary>
    /// CGameCtnChallenge 0x025 chunk (map origin and target)
    /// </summary>
    [Chunk(0x03043025, "map origin and target")]
    public class Chunk03043025 : Chunk<CGameCtnChallenge>
    {
        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.Vec2(ref n.mapCoordOrigin);
            rw.Vec2(ref n.mapCoordTarget);
        }
    }

    #endregion

    #region 0x026 chunk (clip global)

    /// <summary>
    /// CGameCtnChallenge 0x026 chunk (clip global)
    /// </summary>
    [Chunk(0x03043026, "clip global")]
    public class Chunk03043026 : Chunk<CGameCtnChallenge>
    {
        private CMwNod? clipGlobal;

        public CMwNod? ClipGlobal { get => clipGlobal; set => clipGlobal = value; }

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref clipGlobal);
        }

        public override async Task ReadWriteAsync(CGameCtnChallenge n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
        {
            clipGlobal = await rw.NodeRefAsync(clipGlobal, cancellationToken);
        }
    }

    #endregion

    #region 0x027 chunk (old realtime thumbnail)

    /// <summary>
    /// CGameCtnChallenge 0x027 chunk (old realtime thumbnail)
    /// </summary>
    [Chunk(0x03043027)]
    public class Chunk03043027 : Chunk<CGameCtnChallenge>
    {
        public byte U01;
        public Vec3 U02;
        public Vec3 U03;
        public Vec3 U04;

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.hasCustomCamThumbnail);

            if (!n.hasCustomCamThumbnail.GetValueOrDefault())
            {
                return;
            }

            rw.Byte(ref U01);

            // Iso4
            rw.Vec3(ref U02);
            rw.Vec3(ref U03); // camera calibration matrix?
            rw.Vec3(ref U04);
            rw.Vec3(ref n.thumbnailPosition);

            rw.Single(ref n.thumbnailFOV);
            rw.Single(ref n.thumbnailNearClipPlane);
            rw.Single(ref n.thumbnailFarClipPlane);
        }
    }

    #endregion

    #region 0x028 chunk (old realtime thumbnail + comments)

    /// <summary>
    /// CGameCtnChallenge 0x028 chunk (old realtime thumbnail + comments)
    /// </summary>
    [Chunk(0x03043028, "old realtime thumbnail + comments")]
    public class Chunk03043028 : Chunk03043027
    {
        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.String(ref n.comments);
        }
    }

    #endregion

    #region 0x029 skippable chunk (password)

    /// <summary>
    /// CGameCtnChallenge 0x029 skippable chunk (password)
    /// </summary>
    [Chunk(0x03043029, "password")]
    public class Chunk03043029 : SkippableChunk<CGameCtnChallenge>
    {
        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.Bytes(ref n.hashedPassword, 16);
            rw.UInt32(ref n.crc32);
        }
    }

    #endregion

    #region 0x02A chunk

    /// <summary>
    /// CGameCtnChallenge 0x02A chunk
    /// </summary>
    [Chunk(0x0304302A)]
    public class Chunk0304302A : Chunk<CGameCtnChallenge>
    {
        public bool U01;

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01); // code hints to something with locking (NeedUnlock?)
        }
    }

    #endregion

    #region 0x02D skippable chunk (realtime thumbnail + comments)

    /// <summary>
    /// CGameCtnChallenge 0x02D skippable chunk (realtime thumbnail + comments)
    /// </summary>
    [Chunk(0x0304302D, "realtime thumbnail + comments")]
    public class Chunk0304302D : SkippableChunk<CGameCtnChallenge>
    {
        public float U01 = 10;
        public float U02 = 0; // depth? 0 or 0.02

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            // GmLocFreeVal
            rw.Vec3(ref n.thumbnailPosition);
            rw.Vec3(ref n.thumbnailPitchYawRoll);

            // GmLensVal
            rw.Single(ref n.thumbnailFOV, defaultValue: 90);
            rw.Single(ref U01); // always 10
            rw.Single(ref U02); // depth? 0 or 0.02
            rw.Single(ref n.thumbnailNearClipPlane, defaultValue: -1);
            rw.Single(ref n.thumbnailFarClipPlane, defaultValue: -1);

            rw.String(ref n.comments);
        }
    }

    #endregion

    #region 0x034 skippable chunk

    /// <summary>
    /// CGameCtnChallenge 0x034 skippable chunk
    /// </summary>
    [Chunk(0x03043034)]
    public class Chunk03043034 : SkippableChunk<CGameCtnChallenge>
    {
        public byte[]? U01;

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.Bytes(ref U01); // ArchiveBufMem
        }
    }

    #endregion

    #region 0x036 skippable chunk (realtime thumbnail + comments)

    /// <summary>
    /// CGameCtnChallenge 0x036 skippable chunk (realtime thumbnail + comments)
    /// </summary>
    [Chunk(0x03043036, "realtime thumbnail + comments")]
    public class Chunk03043036 : Chunk0304302D
    {

    }

    #endregion

    #region 0x038 skippable chunk

    /// <summary>
    /// CGameCtnChallenge 0x038 skippable chunk
    /// </summary>
    [Chunk(0x03043038), IgnoreChunk]
    public class Chunk03043038 : SkippableChunk<CGameCtnChallenge>
    {
        // refers to 0x03043031 which could be SChallengeCardEventIds?
    }

    #endregion

    #region 0x03A skippable chunk

    /// <summary>
    /// CGameCtnChallenge 0x03A skippable chunk
    /// </summary>
    [Chunk(0x0304303A), IgnoreChunk]
    public class Chunk0304303A : SkippableChunk<CGameCtnChallenge>
    {
        // TODO: array of identifies that could break the parse flow
    }

    #endregion

    #region 0x03D skippable chunk (lightmaps)

    /// <summary>
    /// CGameCtnChallenge 0x03D skippable chunk (lightmaps)
    /// </summary>
    [Chunk(0x0304303D, "lightmaps")]
    public class Chunk0304303D : SkippableChunk<CGameCtnChallenge>
    {
        public bool U01;

        public byte[]? DataAfterLightmapCache { get; set; }

        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; } = 4;

        public int? LightmapCacheDataUncompressedSize { get; set; }
        public byte[]? LightmapCacheData { get; set; }
        public bool EnableWriteOfCompressedData { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);

            if (U01 == false)
            {
                return;
            }

            ReadWriteSHmsLightMapCacheSmall(n, rw);
        }

        public void ReadWriteSHmsLightMapCacheSmall(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            if (rw.Reader is not null)
            {
                ReadSHmsLightMapCacheSmall(n, rw.Reader);
            }

            if (rw.Writer is not null)
            {
                WriteSHmsLightMapCacheSmall(n, rw.Writer);
            }
        }

        public void ReadSHmsLightMapCacheSmall(CGameCtnChallenge n, GameBoxReader r)
        {
            Version = r.ReadInt32();

            if (Version >= 5)
            {
                var frameCount = r.ReadInt32();
                n.lightmapFrames = new List<List<EmbeddedFile>>(frameCount);

                for (var i = 0; i < frameCount; i++)
                    n.lightmapFrames.Add(new List<EmbeddedFile>());
            }
            else
            {
                n.lightmapFrames = new List<List<EmbeddedFile>>
                {
                    new List<EmbeddedFile>()
                };
            }

            if (Version < 2)
            {
                return;
            }

            foreach (var frame in n.lightmapFrames)
            {
                frame.Add(EmbeddedFile.Parse(r.ReadBytes()));

                if (Version >= 3)
                {
                    frame.Add(EmbeddedFile.Parse(r.ReadBytes()));
                }

                if (Version >= 6)
                {
                    frame.Add(EmbeddedFile.Parse(r.ReadBytes()));
                }
            }

            if (n.lightmapFrames.Any(x => x.Any(y => y.Data.Length > 0)))
            {
                LightmapCacheDataUncompressedSize = r.ReadInt32();
                var compressedSize = r.ReadInt32();
                LightmapCacheData = r.ReadBytes(compressedSize);

                using var ms = new MemoryStream(LightmapCacheData);
#if NET6_0_OR_GREATER
                using var zlib = new ZLibStream(ms, CompressionMode.Decompress);
#else
                using var zlib = new CompressedStream(ms, CompressionMode.Decompress);
#endif
                using var gbxr = new GameBoxReader(zlib, r, encapsulated: true);

                n.lightmapCache = Parse<CHmsLightMapCache>(gbxr, 0x06022000, progress: null);

                using var restMs = new MemoryStream();
                zlib.CopyTo(restMs);
                DataAfterLightmapCache = restMs.ToArray();
            }
        }

        public void WriteSHmsLightMapCacheSmall(CGameCtnChallenge n, GameBoxWriter w)
        {
            w.Write(Version);

            if (Version >= 5)
            {
                w.Write(n.lightmapFrames?.Count ?? 0);
            }

            if (Version < 2 || n.lightmapFrames is null)
            {
                return;
            }

            foreach (var frame in n.lightmapFrames)
            {
                w.Write(frame[0].Data.Length);
                w.Write(frame[0].Data);

                if (Version >= 3)
                {
                    w.Write(frame[1].Data.Length);
                    w.Write(frame[1].Data);
                }

                if (Version >= 6)
                {
                    w.Write(frame[2].Data.Length);
                    w.Write(frame[2].Data);
                }
            }

            if (n.lightmapFrames.Any(x => x.Any(y => y.Data.Length > 0)))
            {
#if NET6_0_OR_GREATER
                using var ms = new MemoryStream();
                using var gbxw = new GameBoxWriter(ms, w, encapsulated: true);

                n.lightmapCache?.Write(gbxw);
                gbxw.Write(DataAfterLightmapCache ?? Array.Empty<byte>());

                w.Write((int)ms.Length);

                ms.Seek(0, SeekOrigin.Begin);

                using var msCompressed = new MemoryStream();

                using (var zlib = new ZLibStream(msCompressed, global::System.IO.Compression.CompressionLevel.SmallestSize, true))
                {
                    ms.CopyTo(zlib);
                }

                w.Write((int)msCompressed.Length);
                w.Write(msCompressed.ToArray());
#else
                EnableWriteOfCompressedData = true;

                if (!EnableWriteOfCompressedData)
                {
                    // Temporary solution due to problems with compression
                    w.Write(LightmapCacheDataUncompressedSize.GetValueOrDefault());
                    w.Write(LightmapCacheData!.Length);
                    w.Write(LightmapCacheData);
                    return;
                }
#endif
            }
        }
    }

    #endregion

    #region 0x03E skippable chunk (CarMarksBuffer)

    /// <summary>
    /// CGameCtnChallenge 0x03E skippable chunk (CarMarksBuffer)
    /// </summary>
    [Chunk(0x0304303E, "CarMarksBuffer")]
    public class Chunk0304303E : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        public int U01;

        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            rw.Int32(10);
            rw.ArrayNode<CSceneVehicleCarMarksSamples>(ref n.carMarksBuffer);
        }
    }

    #endregion

    #region 0x040 skippable chunk (items)

    /// <summary>
    /// CGameCtnChallenge 0x040 skippable chunk (items)
    /// </summary>
    [Chunk(0x03043040, "items")]
    public class Chunk03043040 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        public int U01;
        public int[]? U02;

        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; } = 4;

        public override void Read(CGameCtnChallenge n, GameBoxReader r)
        {
            Version = r.ReadInt32();
            U01 = r.ReadInt32();
            var size = r.ReadInt32();

            r = new GameBoxReader(r, encapsulated: true);

            _ = r.ReadInt32(); // 10
            n.anchoredObjects = r.ReadList(r =>
            {
                return Parse<CGameCtnAnchoredObject>(r, classId: null, progress: null)!;
            });

            if (Version >= 1 && Version != 5)
            {
                // defines which (second element) items are deleted together with other (first element) item?
                var itemsOnItem = r.ReadArray<Int2>();

                foreach (var item in itemsOnItem)
                {
                    n.anchoredObjects[item.Y].PlacedOnItem = n.anchoredObjects[item.X];
                }
            }

            if (Version >= 5)
            {
                var blockIndexes = r.ReadArray<int>(); // block indexes, -1 means itemIndexes will have the value instead
                var usedBlocks = new CGameCtnBlock[blockIndexes.Length];
                var blocksWithoutUnassigned = n.blocks!.Where(x => x.Flags != -1).ToArray();

                for (var i = 0; i < blockIndexes.Length; i++)
                {
                    var index = blockIndexes[i];

                    if (index > -1)
                    {
                        usedBlocks[i] = blocksWithoutUnassigned[index];
                    }
                }

                var snapItemGroups = Version < 7 ? r.ReadArray<int>() : null; // snap item group - only some snapped items will delete on a block. they are consistent numbers

                var usedItems = default(CGameCtnAnchoredObject[]);

                if (Version >= 6)
                {
                    var itemIndexes = r.ReadArray<int>(); // item indexes
                    usedItems = new CGameCtnAnchoredObject[itemIndexes.Length];

                    for (var i = 0; i < itemIndexes.Length; i++)
                    {
                        var index = itemIndexes[i];

                        if (index > -1)
                        {
                            usedItems[i] = n.anchoredObjects![index];
                        }
                    }
                }

                snapItemGroups ??= Version >= 7 ? r.ReadArray<int>() : null;

                if (Version != 6)
                {
                    var U07 = r.ReadArray<int>();

                    if (U07.Any(x => x != -1))
                    {
                        throw new NotSupportedException("U07 has something else than -1");
                    }
                }

                // always the same count as anchoredObjects
                var snappedIndexes = r.ReadArray<int>(); // "snapped onto block/item" indexes

                for (var i = 0; i < snappedIndexes.Length; i++)
                {
                    var snappedIndex = snappedIndexes[i];

                    if (snappedIndex <= -1)
                    {
                        continue;
                    }

                    var usedBlock = usedBlocks[snappedIndex];

                    if (usedBlock is not null)
                    {
                        n.anchoredObjects[i].SnappedOnBlock = usedBlock;
                    }

                    var usedItem = usedItems?[snappedIndex];

                    if (usedItem is not null)
                    {
                        n.anchoredObjects[i].SnappedOnItem = usedItem;
                    }

                    n.anchoredObjects[i].SnappedOnGroup = snapItemGroups?[snappedIndex] ?? 0;
                }

                if (Version >= 8)
                {
                    throw new ChunkVersionNotSupportedException(Version);
                }
            }

            r.Dispose();
        }

        public override void Write(CGameCtnChallenge n, GameBoxWriter w)
        {
            w.Write(Version);
            w.Write(U01);

            using var itemMs = new MemoryStream();
            using var itemW = new GameBoxWriter(itemMs, w, encapsulated: true);

            itemW.Write(10);
            itemW.WriteNodeArray(n.anchoredObjects);

            var itemDict = new Dictionary<CGameCtnAnchoredObject, int>();

            if (n.anchoredObjects is not null)
            {
                for (var i = 0; i < n.anchoredObjects.Count; i++)
                {
                    itemDict[n.anchoredObjects[i]] = i;
                }
            }

            if (Version >= 1 && Version != 5)
            {
                var pairs = new List<Int2>();

                if (n.anchoredObjects is not null)
                {
                    for (var i = 0; i < n.anchoredObjects.Count; i++)
                    {
                        var placedOnItem = n.anchoredObjects[i].PlacedOnItem;

                        if (placedOnItem is not null && itemDict.TryGetValue(placedOnItem, out int index))
                        {
                            pairs.Add((index, i));
                        }
                    }
                }

                itemW.WriteList(pairs, (x, w) => w.Write(x));
            }

            if (Version >= 5)
            {
                var blockDict = new Dictionary<CGameCtnBlock, int>();

                if (n.blocks is not null)
                {
                    for (var i = 0; i < n.blocks.Count; i++)
                    {
                        blockDict[n.blocks[i]] = i;
                    }
                }

                var usedBlockIndexHashSet = new HashSet<(int blockIndex, int group)>();
                var usedBlockIndexList = new List<(int blockIndex, int group)>();
                var usedItemIndexHashSet = new HashSet<(int itemIndex, int group)>();
                var usedItemIndexList = new List<(int itemIndex, int group)>();

                var indiciesOnUsedBlocksAndItems = new Dictionary<(int index, int group), int>();
                var snappedOnIndicies = new List<int>(n.anchoredObjects?.Count ?? 0);

                foreach (var item in n.GetAnchoredObjects())
                {
                    if (item.SnappedOnBlock is null && item.SnappedOnItem is null)
                    {
                        snappedOnIndicies.Add(-1);
                        continue;
                    }

                    var groupIndex = item.SnappedOnGroup ?? 0;
                    var unique = (-1, groupIndex);

                    if (item.SnappedOnBlock is not null)
                    {
                        var blockIndex = blockDict[item.SnappedOnBlock];

                        unique = (blockIndex, groupIndex);

                        if (!usedBlockIndexHashSet.Contains(unique))
                        {
                            usedBlockIndexList.Add(unique);
                            usedBlockIndexHashSet.Add(unique);

                            if (item.SnappedOnItem is null)
                            {
                                usedItemIndexList.Add((-1, groupIndex));
                            }
                        }
                    }

                    if (item.SnappedOnItem is not null)
                    {
                        var itemIndex = itemDict[item.SnappedOnItem];

                        unique = (itemIndex, groupIndex);

                        if (!usedItemIndexHashSet.Contains(unique))
                        {
                            usedItemIndexList.Add(unique);
                            usedItemIndexHashSet.Add(unique);

                            if (item.SnappedOnBlock is null)
                            {
                                usedBlockIndexList.Add((-1, groupIndex));
                            }
                        }
                    }

                    if (indiciesOnUsedBlocksAndItems.TryGetValue(unique, out int indexOfBlockOrItemIndex))
                    {
                        snappedOnIndicies.Add(indexOfBlockOrItemIndex);
                    }
                    else
                    {
                        indiciesOnUsedBlocksAndItems[unique] = indiciesOnUsedBlocksAndItems.Count;
                        snappedOnIndicies.Add(indiciesOnUsedBlocksAndItems.Count - 1);
                    }
                }

                itemW.WriteArray(usedBlockIndexList.Select(x => x.blockIndex).ToArray());

                if (Version < 7)
                {
                    itemW.WriteArray(usedBlockIndexList.Select(x => x.group).ToArray());
                }

                if (Version >= 6)
                {
                    itemW.WriteArray(usedItemIndexList.Select(x => x.itemIndex).ToArray());
                }

                if (Version >= 7)
                {
                    itemW.WriteArray(usedBlockIndexList.Select(x => x.group).ToArray());
                }

                if (Version != 6)
                {
                    itemW.WriteArray(Enumerable.Repeat(-1, usedBlockIndexList.Count).ToArray());
                }

                itemW.WriteArray(snappedOnIndicies.ToArray());
            }

            w.Write((int)itemMs.Length);
            w.Write(itemMs.ToArray());
        }
    }

    #endregion

    #region 0x041 skippable chunk

    /// <summary>
    /// CGameCtnChallenge 0x041 skippable chunk
    /// </summary>
    [Chunk(0x03043041), IgnoreChunk]
    public class Chunk03043041 : SkippableChunk<CGameCtnChallenge>
    {
        // encapsulated node something
    }

    #endregion

    #region 0x042 skippable chunk (author)

    /// <summary>
    /// CGameCtnChallenge 0x042 skippable chunk (author)
    /// </summary>
    [Chunk(0x03043042, "author")]
    public class Chunk03043042 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; } = 1;

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref n.authorVersion);
            rw.String(ref n.authorLogin!); // according to asserts, login must be larger than 0 and maybe equal to the MapInfo.Author
            rw.String(ref n.authorNickname);
            rw.String(ref n.authorZone);
            rw.String(ref n.authorExtraInfo);
        }
    }

    #endregion

    #region 0x043 skippable chunk (genealogies)

    /// <summary>
    /// CGameCtnChallenge 0x043 skippable chunk (generalogies)
    /// </summary>
    [Chunk(0x03043043, "generalogies")]
    public class Chunk03043043 : SkippableChunk<CGameCtnChallenge>
    {
        public int U01;

        public override void Read(CGameCtnChallenge n, GameBoxReader r)
        {
            U01 = r.ReadInt32();
            var sizeOfNodeWithClassID = r.ReadInt32();

            r = new GameBoxReader(r, encapsulated: true);

            n.genealogies = r.ReadArray(r =>
            {
                return Parse<CGameCtnZoneGenealogy>(r, classId: null, progress: null)!;
            });

            r.Dispose();
        }

        public override void Write(CGameCtnChallenge n, GameBoxWriter w)
        {
            w.Write(U01);

            using var ms = new MemoryStream();
            using var wOwnIdState = new GameBoxWriter(ms, w, encapsulated: true);

            wOwnIdState.WriteArray(n.genealogies, (x, w) =>
            {
                w.Write(0x0311D000);
                x.Write(w);
            });

            w.Write((int)ms.Length);
            w.Write(ms.ToArray());
        }
    }

    #endregion

    #region 0x044 skippable chunk (metadata)

    /// <summary>
    /// CGameCtnChallenge 0x044 skippable chunk (metadata)
    /// </summary>
    [Chunk(0x03043044, "metadata")]
    public class Chunk03043044 : SkippableChunk<CGameCtnChallenge>
    {
        public int EncapsulationVersion { get; set; }

        public override void Read(CGameCtnChallenge n, GameBoxReader r)
        {
            EncapsulationVersion = r.ReadInt32();
            var size = r.ReadInt32();

            n.scriptMetadata = r.ReadNode<CScriptTraitsMetadata>(expectedClassId: 0x11002000);
        }

        public override void Write(CGameCtnChallenge n, GameBoxWriter w)
        {
            w.Write(EncapsulationVersion);

            using var ms = new MemoryStream();
            using var wm = new GameBoxWriter(ms);

            n.scriptMetadata?.Write(wm);

            w.Write((int)ms.Length);
            w.Write(ms.ToArray());
        }
    }

    #endregion

    #region 0x047 skippable chunk

    /// <summary>
    /// CGameCtnChallenge 0x047 skippable chunk
    /// </summary>
    [Chunk(0x03043047)]
    public class Chunk03043047 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        public string? U01;

        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref U01);

            if (Version > 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }
        }
    }

    #endregion

    #region 0x048 skippable chunk (baked blocks)

    /// <summary>
    /// CGameCtnChallenge 0x048 skippable chunk (baked blocks)
    /// </summary>
    [Chunk(0x03043048, "baked blocks", ProcessSync = true)]
    public class Chunk03043048 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        public int U01;
        public int U02;

        public int Version { get; set; }

        public override void Read(CGameCtnChallenge n, GameBoxReader r)
        {
            Version = r.ReadInt32();

            if (Version >= 1)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            U01 = r.ReadInt32();

            var count = r.ReadInt32();

            n.BakedBlocks = new List<CGameCtnBlock>(count);

            for (var i = 0; i < count; i++)
            {
                var flags = ReadAndAddBakedBlock(n, r);

                if (flags == -1)
                {
                    i--;
                }
            }

            while ((r.PeekUInt32() & 0xC0000000) > 0)
            {
                ReadAndAddBakedBlock(n, r);
            }

            U02 = r.ReadInt32();

            n.BakedClipsAdditionalData = r.ReadArray<SBakedClipsAdditionalData>(r => new(
                Clip1: r.ReadIdent(),
                Clip2: r.ReadIdent(),
                Clip3: r.ReadIdent(),
                Clip4: r.ReadIdent(),
                Coord: r.ReadInt3()
            ));
        }

        private static int ReadAndAddBakedBlock(CGameCtnChallenge n, GameBoxReader r)
        {
            var name = r.ReadId();
            var direction = (Direction)r.ReadByte();
            var coord = (Int3)r.ReadByte3();
            var flags = r.ReadInt32();

            if (flags != -1)
            {
                coord -= (1, 0, 1);
            }

            if (CGameCtnBlock.IsFreeBlock(flags))
            {
                coord -= (0, 1, 0);
            }

            var author = default(string?);
            var skin = default(CGameCtnBlockSkin?);

            if (CGameCtnBlock.IsSkinnableBlock(flags))
            {
                author = r.ReadId();
                skin = r.ReadNodeRef<CGameCtnBlockSkin>();
            }

            n.BakedBlocks!.Add(new(name, direction, coord, flags, author, skin));

            return flags;
        }

        public override void Write(CGameCtnChallenge n, GameBoxWriter w)
        {
            w.Write(Version);

            if (Version >= 1)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            w.Write(U01);

            w.Write(n.NbBakedBlocks.GetValueOrDefault());
            
            foreach (var block in n.GetBakedBlocks())
            {
                w.WriteId(block.Name);
                w.Write((byte)block.Direction);

                var coord = block.Coord;

                if (block.Flags != -1)
                {
                    coord += (1, 0, 1);
                }

                if (CGameCtnBlock.IsFreeBlock(block.Flags))
                {
                    coord += (0, 1, 0);
                }

                w.Write((Byte3)coord);
                
                w.Write(block.Flags);

                if (CGameCtnBlock.IsSkinnableBlock(block.Flags))
                {
                    w.WriteId(block.Author);
                    w.Write(block.Skin);
                }
            }

            w.Write(U02);

            w.WriteArray(n.BakedClipsAdditionalData, (x, r) =>
            {
                w.Write(x.Clip1);
                w.Write(x.Clip2);
                w.Write(x.Clip3);
                w.Write(x.Clip4);
                w.Write(x.Coord);
            });
        }
    }

    #endregion

    #region 0x049 chunk (mediatracker)

    /// <summary>
    /// CGameCtnChallenge 0x049 chunk (mediatracker)
    /// </summary>
    [Chunk(0x03043049, "mediatracker")]
    public class Chunk03043049 : Chunk<CGameCtnChallenge>, IVersionable
    {
        private Int3 triggerSize = (3, 1, 3);

        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; } = 2;

        /// <summary>
        /// Trigger size per coord.
        /// </summary>
        public Int3 TriggerSize { get => triggerSize; set => triggerSize = value; }

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
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
                rw.Int3(ref triggerSize);
            }
        }

        public override async Task ReadWriteAsync(CGameCtnChallenge n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
        {
            rw.VersionInt32(this);

            n.clipIntro = await rw.NodeRefAsync<CGameCtnMediaClip>(n.clipIntro, cancellationToken);
            n.clipPodium = await rw.NodeRefAsync<CGameCtnMediaClip>(n.clipPodium, cancellationToken);
            n.clipGroupInGame = await rw.NodeRefAsync<CGameCtnMediaClipGroup>(n.clipGroupInGame, cancellationToken);
            n.clipGroupEndRace = await rw.NodeRefAsync<CGameCtnMediaClipGroup>(n.clipGroupEndRace, cancellationToken);
            
            if (Version >= 2)
            {
                n.clipAmbiance = await rw.NodeRefAsync<CGameCtnMediaClip>(n.clipAmbiance, cancellationToken);
            }

            if (Version >= 1)
            {
                rw.Int3(ref triggerSize);
            }
        }
    }

    #endregion

    #region 0x04B skippable chunk (objectives)

    /// <summary>
    /// CGameCtnChallenge 0x04B skippable chunk (objectives)
    /// </summary>
    [Chunk(0x0304304B, "objectives")]
    public class Chunk0304304B : SkippableChunk<CGameCtnChallenge>
    {
        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.objectiveTextAuthor);
            rw.String(ref n.objectiveTextGold);
            rw.String(ref n.objectiveTextSilver);
            rw.String(ref n.objectiveTextBronze);
        }
    }

    #endregion

    #region 0x04D skippable chunk

    /// <summary>
    /// CGameCtnChallenge 0x04D skippable chunk
    /// </summary>
    [Chunk(0x0304304D), IgnoreChunk]
    public class Chunk0304304D : SkippableChunk<CGameCtnChallenge>
    {
        // gone from code since ManiaPlanet 4
    }

    #endregion

    #region 0x04E skippable chunk (trigger actions)

    /// <summary>
    /// CGameCtnChallenge 0x04E skippable chunk (trigger actions)
    /// </summary>
    [Chunk(0x0304304E), IgnoreChunk]
    public class Chunk0304304E : SkippableChunk<CGameCtnChallenge>
    {
        // encapsulated CPlugTriggerAction array
    }

    #endregion

    #region 0x04F skippable chunk

    /// <summary>
    /// CGameCtnChallenge 0x04F skippable chunk
    /// </summary>
    [Chunk(0x0304304F), IgnoreChunk]
    public class Chunk0304304F : SkippableChunk<CGameCtnChallenge>
    {
        // possibly some enum
    }

    #endregion

    #region 0x050 skippable chunk (offzones)

    /// <summary>
    /// CGameCtnChallenge 0x050 skippable chunk (offzones)
    /// </summary>
    [Chunk(0x03043050, "offzones")]
    public class Chunk03043050 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        private Vec3 triggerSize = (3, 1, 3);

        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; }

        public Vec3 TriggerSize { get => triggerSize; set => triggerSize = value; }

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Vec3(ref triggerSize);

            rw.List(ref n.offzones,
                (i, r) => (r.ReadInt3(), r.ReadInt3()),
                (x, w) =>
                {
                    w.Write(x.Item1);
                    w.Write(x.Item2);
                });
        }
    }

    #endregion

    #region 0x051 skippable chunk (title info)

    /// <summary>
    /// CGameCtnChallenge 0x051 skippable chunk (title info)
    /// </summary>
    [Chunk(0x03043051, "title info")]
    public class Chunk03043051 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref n.titleID);
            rw.String(ref n.buildVersion);
        }
    }

    #endregion

    #region 0x052 skippable chunk (deco height)

    /// <summary>
    /// CGameCtnChallenge 0x052 skippable chunk (deco height)
    /// </summary>
    [Chunk(0x03043052, "deco height")]
    public class Chunk03043052 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref n.decoBaseHeightOffset);
        }
    }

    #endregion

    #region 0x053 skippable chunk (bot paths)

    /// <summary>
    /// CGameCtnChallenge 0x053 skippable chunk (bot paths)
    /// </summary>
    [Chunk(0x03043053, "bot paths")]
    public class Chunk03043053 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ListArchive(ref n.botPaths);
        }
    }

    #endregion

    #region 0x054 skippable chunk (embedded objects)

    /// <summary>
    /// CGameCtnChallenge 0x054 skippable chunk (embedded objects)
    /// </summary>
    [Chunk(0x03043054, "embedded objects")]
    public class Chunk03043054 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        public int U01;

        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; } = 1;
        public string[]? Textures { get; set; }

        public override void Read(CGameCtnChallenge n, GameBoxReader r)
        {
            Version = r.ReadInt32();
            U01 = r.ReadInt32();
            var size = r.ReadInt32();

            r = new GameBoxReader(r, encapsulated: true);

            var embedded = r.ReadArray(r => r.ReadIdent());

            n.embeddedData = new Dictionary<string, byte[]>();
            n.originalEmbedZip = r.ReadBytes();

            if (n.originalEmbedZip.Length > 0)
            {
                using var ms = new MemoryStream(n.originalEmbedZip);
                using var zip = new ZipArchive(ms);

                foreach (var entry in zip.Entries)
                {
                    using var entryStream = entry.Open();
                    using var entryDataStream = new MemoryStream();

                    entryStream.CopyTo(entryDataStream);
                    n.embeddedData[entry.FullName] = entryDataStream.ToArray();
                }
            }

            Textures = r.ReadArray(r => r.ReadString());

            r.Dispose();
        }

        public override void Write(CGameCtnChallenge n, GameBoxWriter w)
        {
            w.Write(Version);
            w.Write(U01);

            using var ms = new MemoryStream();
            using var writer = new GameBoxWriter(ms, w, encapsulated: true);

            var embedded = new List<Ident>();

            foreach (var item in n.GetEmbeddedItemModelHeaders())
            {
                var gbxItem = item.GetGbx() ?? throw new ThisShouldNotHappenException();

                if (item.Ident is null)
                {
                    continue;
                }

                if (gbxItem.FileName is null)
                {
                    embedded.Add(item.Ident);
                    continue;
                }

                var id = gbxItem.FileName;
                var dirs = id.Split('/', '\\');

                for (var i = 0; i < dirs.Length; i++)
                {
                    var dir = dirs[dirs.Length - 1 - i];
                    if (dir == "Items" || dir == "Blocks")
                    {
                        id = string.Join("\\", dirs, dirs.Length - i, i);
                        break;
                    }
                }

                embedded.Add(item.Ident with { Id = id });
            }

            writer.WriteList(embedded, (x, w) => w.Write(x));

            using (var zipStream = new MemoryStream())
            {
                n.ExtractEmbedZip(zipStream);
                writer.Write((int)zipStream.Length);
                writer.Write(zipStream.ToArray(), 0, (int)zipStream.Length);
            }

            writer.WriteArray(Textures, (x, w) => w.Write(x));

            w.Write((int)ms.Length);
            w.Write(ms.ToArray());
        }
    }

    #endregion

    #region 0x055 skippable chunk

    /// <summary>
    /// CGameCtnChallenge 0x055 skippable chunk
    /// </summary>
    [Chunk(0x03043055), IgnoreChunk]
    public class Chunk03043055 : SkippableChunk<CGameCtnChallenge>
    {
        // sets something to constant 1
    }

    #endregion

    #region 0x056 skippable chunk (light settings)

    /// <summary>
    /// CGameCtnChallenge 0x056 skippable chunk (light settings)
    /// </summary>
    [Chunk(0x03043056, "light settings")]
    public class Chunk03043056 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; } = 3;

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(0);
            rw.TimeOfDay(ref n.dayTime);
            rw.Int32(0);
            rw.Boolean(ref n.dynamicDaylight);
            rw.TimeInt32Nullable(ref n.dayDuration);
        }
    }

    #endregion

    #region 0x057 skippable chunk

    /// <summary>
    /// CGameCtnChallenge 0x057 skippable chunk
    /// </summary>
    [Chunk(0x03043057), IgnoreChunk]
    public class Chunk03043057 : SkippableChunk<CGameCtnChallenge>
    {
        // additional stuff for bot paths
    }

    #endregion

    #region 0x058 skippable chunk (SubMapsInfos)

    /// <summary>
    /// CGameCtnChallenge 0x058 skippable chunk (SubMapsInfos)
    /// </summary>
    [Chunk(0x03043058, "SubMapsInfos")]
    public class Chunk03043058 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        public int U01;

        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);

            if (U01 > 0)
            {
                throw new NotSupportedException("U01 > 0");
            }
        }
    }

    #endregion

    #region 0x059 skippable chunk

    /// <summary>
    /// CGameCtnChallenge 0x059 skippable chunk
    /// </summary>
    [Chunk(0x03043059)]
    public class Chunk03043059 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        public Vec3 U01;
        public bool? U02;
        public float? U03;
        public float? U04;

        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this); // 3

            rw.Vec3(ref U01);

            if (Version == 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            if (Version >= 1)
            {
                rw.Boolean(ref U02);

                if (U02.GetValueOrDefault())
                {
                    throw new NotSupportedException("U02 == true");
                }

                if (Version >= 3)
                {
                    rw.Single(ref U03);
                    rw.Single(ref U04);
                }
            }
        }
    }

    #endregion

    #region 0x05A skippable chunk [TM2020]

    /// <summary>
    /// CGameCtnChallenge 0x05A skippable chunk [TM2020]
    /// </summary>
    [Chunk(0x0304305A)]
    public class Chunk0304305A : SkippableChunk<CGameCtnChallenge>
    {
        public int U01;
        public int U02;

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    #endregion

    #region 0x05B skippable chunk (lightmaps) [TM2020]

    /// <summary>
    /// CGameCtnChallenge 0x05B skippable chunk (lightmaps) [TM2020]
    /// </summary>
    [Chunk(0x0304305B, "lightmaps")]
    public class Chunk0304305B : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        private readonly Chunk0304303D chunk0304303D = new();
        
        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; }

        public bool U01;
        public bool U02;
        public bool U03;

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.Boolean(ref U03);

            if (U01 == false)
            {
                return;
            }

            chunk0304303D.ReadWriteSHmsLightMapCacheSmall(n, rw);
        }
    }

    #endregion

    #region 0x05C skippable chunk [TM2020]

    /// <summary>
    /// CGameCtnChallenge 0x05C skippable chunk [TM2020]
    /// </summary>
    [Chunk(0x0304305C), IgnoreChunk]
    public class Chunk0304305C : SkippableChunk<CGameCtnChallenge>
    {

    }

    #endregion

    #region 0x05D skippable chunk [TM2020]

    /// <summary>
    /// CGameCtnChallenge 0x05D skippable chunk [TM2020]
    /// </summary>
    [Chunk(0x0304305D), IgnoreChunk]
    public class Chunk0304305D : SkippableChunk<CGameCtnChallenge>
    {

    }

    #endregion

    #region 0x05E skippable chunk [TM2020]

    /// <summary>
    /// CGameCtnChallenge 0x05E skippable chunk [TM2020]
    /// </summary>
    [Chunk(0x0304305E), IgnoreChunk]
    public class Chunk0304305E : SkippableChunk<CGameCtnChallenge>
    {

    }

    #endregion

    #region 0x05F skippable chunk (free blocks) [TM®️]

    /// <summary>
    /// CGameCtnChallenge 0x05F skippable chunk (free blocks) [TM®️]
    /// </summary>
    [Chunk(0x0304305F, "free blocks", ProcessSync = true)]
    public class Chunk0304305F : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            foreach (var block in n.GetBlocks().Concat(n.GetBakedBlocks()).Where(x => x.IsFree))
            {
                block.AbsolutePositionInMap = rw.Vec3(block.AbsolutePositionInMap);
                block.PitchYawRoll = rw.Vec3(block.PitchYawRoll);
            }
        }
    }

    #endregion

    #region 0x060 skippable chunk [TM2020]

    /// <summary>
    /// CGameCtnChallenge 0x060 skippable chunk [TM2020]
    /// </summary>
    [Chunk(0x03043060), IgnoreChunk]
    public class Chunk03043060 : SkippableChunk<CGameCtnChallenge>
    {

    }

    #endregion

    #region 0x061 skippable chunk [TM2020]

    /// <summary>
    /// CGameCtnChallenge 0x061 skippable chunk [TM2020]
    /// </summary>
    [Chunk(0x03043061), IgnoreChunk]
    public class Chunk03043061 : SkippableChunk<CGameCtnChallenge>
    {

    }

    #endregion

    #region 0x062 skippable chunk (MapElemColor) [TM2020]

    /// <summary>
    /// CGameCtnChallenge 0x062 skippable chunk (MapElemColor) [TM2020]
    /// </summary>
    [Chunk(0x03043062, "MapElemColor", ProcessSync = true)]
    public class Chunk03043062 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            foreach (var block in n.GetBlocks())
            {
                block.Color = (DifficultyColor)rw.Byte((byte)block.Color.GetValueOrDefault());
            }

            foreach (var block in n.GetBakedBlocks())
            {
                block.Color = (DifficultyColor)rw.Byte((byte)block.Color.GetValueOrDefault());
            }

            foreach (var item in n.GetAnchoredObjects())
            {
                item.Color = (DifficultyColor)rw.Byte((byte)item.Color.GetValueOrDefault());
            }
        }
    }

    #endregion

    #region 0x063 skippable chunk (AnimPhaseOffset) [TM2020]

    /// <summary>
    /// CGameCtnChallenge 0x063 skippable chunk (AnimPhaseOffset) [TM2020]
    /// </summary>
    [Chunk(0x03043063, "AnimPhaseOffset", ProcessSync = true)]
    public class Chunk03043063 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            foreach (var item in n.GetAnchoredObjects())
            {
                item.AnimPhaseOffset = (CGameCtnAnchoredObject.EPhaseOffset)rw.Byte((byte)item.AnimPhaseOffset.GetValueOrDefault());
            }
        }
    }

    #endregion

    #region 0x064 skippable chunk [TM2020]

    /// <summary>
    /// CGameCtnChallenge 0x064 skippable chunk [TM2020]
    /// </summary>
    [Chunk(0x03043064), IgnoreChunk]
    public class Chunk03043064 : SkippableChunk<CGameCtnChallenge>
    {

    }

    #endregion

    #region 0x065 skippable chunk (foreground pack desc) [TM2020]

    /// <summary>
    /// CGameCtnChallenge 0x065 skippable chunk (foreground pack desc) [TM2020]
    /// </summary>
    [Chunk(0x03043065, "foreground pack desc", ProcessSync = true)]
    public class Chunk03043065 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            foreach (var item in n.GetAnchoredObjects())
            {
                var hasForegroundPackDesc = rw.Boolean(item.ForegroundPackDesc is not null, asByte: true);

                if (hasForegroundPackDesc)
                {
                    item.ForegroundPackDesc = rw.FileRef(item.ForegroundPackDesc);
                }
            }
        }
    }

    #endregion

    #region 0x067 skippable chunk [TM2020]

    /// <summary>
    /// CGameCtnChallenge 0x067 skippable chunk [TM2020]
    /// </summary>
    [Chunk(0x03043067), IgnoreChunk]
    public class Chunk03043067 : SkippableChunk<CGameCtnChallenge>
    {

    }

    #endregion

    #region 0x068 skippable chunk (MapElemLmQuality) [TM2020]

    /// <summary>
    /// CGameCtnChallenge 0x068 skippable chunk (MapElemLmQuality) [TM2020]
    /// </summary>
    [Chunk(0x03043068, "MapElemLmQuality", ProcessSync = true)]
    public class Chunk03043068 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        // It has not been 100% validated if this is lightmap quality per block/object or not, but a lot of things hint towards it
        
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 1)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            foreach (var block in n.GetBlocks())
            {
                block.LightmapQuality = (LightmapQuality)rw.Byte((byte)block.LightmapQuality.GetValueOrDefault());
            }

            foreach (var block in n.GetBakedBlocks())
            {
                block.LightmapQuality = (LightmapQuality)rw.Byte((byte)block.LightmapQuality.GetValueOrDefault());
            }

            foreach (var item in n.GetAnchoredObjects())
            {
                item.LightmapQuality = (LightmapQuality)rw.Byte((byte)item.LightmapQuality.GetValueOrDefault());
            }
        }
    }

    #endregion

    #region 0x069 skippable chunk (macroblock instances) [TM2020]

    /// <summary>
    /// CGameCtnChallenge 0x069 skippable chunk (macroblock instances) [TM2020]
    /// </summary>
    [Chunk(0x03043069, "macroblock instances", ProcessSync = true)]
    public class Chunk03043069 : SkippableChunk<CGameCtnChallenge>, IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnChallenge n, GameBoxReader r)
        {
            Version = r.ReadInt32();

            if (Version > 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            var dict = new Dictionary<int, MacroblockInstance>();

            foreach (var block in n.GetBlocks())
            {
                var macroblockId = r.ReadInt32();

                if (macroblockId == -1)
                {
                    continue;
                }

                if (!dict.TryGetValue(macroblockId, out var instance))
                {
                    instance = new MacroblockInstance();
                    dict[macroblockId] = instance;
                }

                block.MacroblockReference = instance;
            }

            foreach (var item in n.GetAnchoredObjects())
            {
                var macroblockId = r.ReadInt32();

                if (macroblockId == -1)
                {
                    continue;
                }

                if (!dict.TryGetValue(macroblockId, out var instance))
                {
                    instance = new MacroblockInstance();
                    dict[macroblockId] = instance;
                }

                item.MacroblockReference = instance;
            }

            var idFlagsPair = r.ReadArray<(int, int)>();

            foreach (var (id, flags) in idFlagsPair)
            {
                dict[id].Flags = flags;
            }

            n.MacroblockInstances = dict.Values.ToList();
        }

        public override void Write(CGameCtnChallenge n, GameBoxWriter w)
        {
            w.Write(Version);

            var dict = new Dictionary<MacroblockInstance, int>();

            if (n.MacroblockInstances is not null)
            {
                for (var i = 0; i < n.MacroblockInstances.Count; i++)
                {
                    dict[n.MacroblockInstances[i]] = i;
                }
            }

            foreach (var block in n.GetBlocks())
            {
                if (block.MacroblockReference is not null && dict.TryGetValue(block.MacroblockReference, out int index))
                {
                    w.Write(index);
                }
                else
                {
                    w.Write(-1);
                }
            }

            foreach (var item in n.GetAnchoredObjects())
            {
                if (item.MacroblockReference is not null && dict.TryGetValue(item.MacroblockReference, out int index))
                {
                    w.Write(index);
                }
                else
                {
                    w.Write(-1);
                }
            }

            w.WriteList(n.MacroblockInstances, (x, i, w) =>
            {
                w.Write(i);
                w.Write(x.Flags);
            });
        }
    }

    #endregion

    #endregion
}
