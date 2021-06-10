using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using GBX.NET.BlockInfo;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Hms;
using GBX.NET.Engines.Script;
using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// Map (0x03043000)
    /// </summary>
    /// <remarks>A map. Known extensions: .Challenge.Gbx, .Map.Gbx</remarks>
    [Node(0x03043000)]
    public class CGameCtnChallenge : CMwNod, CGameCtnChallenge.IHeader
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

        private TimeSpan? bronzeTime; // Only used if ChallengeParameters is null
        private TimeSpan? silverTime; // Only used if ChallengeParameters is null
        private TimeSpan? goldTime; // Only used if ChallengeParameters is null
        private TimeSpan? authorTime; // Only used if ChallengeParameters is null
        private int cost;
        private EditorMode editor;
        private int authorScore;
        private bool isLapRace;
        private int nbLaps = 3;
        private Ident mapInfo;
        private string mapName;
        private MapKind kind = MapKind.InProgress;
        private int? nbCheckpoints;
        private string password;
        private Ident decoration;
        private string mapType;
        private ulong? lightmapCacheUID;
        private byte? lightmapVersion;
        private string xml;
        private string comments;
        private byte[] thumbnail;
        private Vec2? mapCoordOrigin;
        private Vec2? mapCoordTarget;
        private string mapStyle;
        private string titleID;
        private int authorVersion;
        private string authorLogin;
        private string authorNickname;
        private string authorZone;
        private string authorExtraInfo;
        private Ident playerModel;
        private CGameCtnChallengeParameters challengeParameters;
        private CGameCtnCollectorList blockStock;
        private Int3[] checkpoints;
        private FileRef modPackDesc;
        private Int3? size;
        private bool? needUnlock;
        private List<CGameCtnBlock> blocks;
        private CGameCtnBlock[] bakedBlocks;
        private CGameCtnMediaClip clipIntro;
        private CGameCtnMediaClipGroup clipGroupInGame;
        private CGameCtnMediaClipGroup clipGroupEndRace;
        private CGameCtnMediaClip clipAmbiance;
        private CGameCtnMediaClip clipPodium;
        private FileRef customMusicPackDesc;
        private PlayMode? mode;
        private byte[] hashedPassword;
        private uint? crc32;
        private Vec3? thumbnailPosition;
        private Vec3? thumbnailPitchYawRoll;
        private float? thumbnailFOV;
        private List<CGameCtnAnchoredObject> anchoredObjects;
        private CScriptTraitsMetadata scriptMetadata;
        private List<List<byte[]>> lightmapFrames;
        private Task<CHmsLightMapCache> lightmapCache;
        private Task<CGameCtnZoneGenealogy[]> genealogies;
        private string objectiveTextAuthor;
        private string objectiveTextGold;
        private string objectiveTextSilver;
        private string objectiveTextBronze;
        private List<(Int3 start, Int3 end)> offzones;
        private string buildVersion;
        private int decoBaseHeightOffset;
        private List<BotPath> botPaths;
        private readonly Dictionary<string, byte[]> embeddedObjects = new Dictionary<string, byte[]>();
        private byte[] originalEmbedZip;
        private TimeSpan? dayTime;
        private bool dynamicDaylight;
        private TimeSpan? dayDuration;

        #endregion

        #region Properties

        /// <summary>
        /// Shows members that are available from the GBX header (reading the body is not required).
        /// </summary>
        public IHeader Header => this;

        /// <summary>
        /// Time of the bronze medal. If <see cref="ChallengeParameters"/> is available, it uses the value from there instead.
        /// </summary>
        [NodeMember]
        public TimeSpan? TMObjective_BronzeTime
        {
            get
            {
                if (ChallengeParameters != null)
                    return ChallengeParameters.BronzeTime;
                return bronzeTime;
            }
            set
            {
                if (ChallengeParameters != null)
                    ChallengeParameters.BronzeTime = value;
                bronzeTime = value;
            }
        }

        /// <summary>
        /// Time of the silver medal. If <see cref="ChallengeParameters"/> is available, it uses the value from there instead.
        /// </summary>
        [NodeMember]
        public TimeSpan? TMObjective_SilverTime
        {
            get
            {
                if (ChallengeParameters != null)
                    return ChallengeParameters.SilverTime;
                return silverTime;
            }
            set
            {
                if (ChallengeParameters != null)
                    ChallengeParameters.SilverTime = value;
                silverTime = value;
            }
        }

        /// <summary>
        /// Time of the gold medal. If <see cref="ChallengeParameters"/> is available, it uses the value from there instead.
        /// </summary>
        [NodeMember]
        public TimeSpan? TMObjective_GoldTime
        {
            get
            {
                if (ChallengeParameters != null)
                    return ChallengeParameters.GoldTime;
                return goldTime;
            }
            set
            {
                if (ChallengeParameters != null)
                    ChallengeParameters.GoldTime = value;
                goldTime = value;
            }
        }

        /// <summary>
        /// Time of the author medal. If <see cref="ChallengeParameters"/> is available, it uses the value from there instead.
        /// </summary>
        [NodeMember]
        public TimeSpan? TMObjective_AuthorTime
        {
            get
            {
                if (ChallengeParameters != null)
                    return ChallengeParameters.AuthorTime;
                return authorTime;
            }
            set
            {
                if (ChallengeParameters != null)
                    ChallengeParameters.AuthorTime = value;
                authorTime = value;
            }
        }

        /// <summary>
        /// Display cost of the track (or copper cost) explaining the performance of the map.
        /// </summary>
        [NodeMember]
        public int Cost
        {
            get => cost;
            set => cost = value;
        }

        /// <summary>
        /// Usually author time or stunts score. If <see cref="ChallengeParameters"/> is available, it uses the value from there instead.
        /// </summary>
        [NodeMember]
        public int AuthorScore
        {
            get
            {
                if (ChallengeParameters != null)
                    return ChallengeParameters.AuthorScore;
                return authorScore;
            }
            set
            {
                if (ChallengeParameters != null)
                    ChallengeParameters.AuthorScore = value;
                authorScore = value;
            }
        }

        /// <summary>
        /// In which editor settings the map was made.
        /// </summary>
        [NodeMember]
        public EditorMode Editor
        {
            get => editor;
            set => editor = value;
        }

        /// <summary>
        /// If the map was made using the simple editor.
        /// </summary>
        [NodeMember]
        public bool CreatedWithSimpleEditor => (Editor & EditorMode.Simple) != 0;

        /// <summary>
        /// If the map uses ghost blocks.
        /// </summary>
        [NodeMember]
        public bool HasGhostBlocks => (Editor & EditorMode.HasGhostBlocks) != 0;

        /// <summary>
        /// If the map is a multilap.
        /// </summary>
        [NodeMember]
        public bool TMObjective_IsLapRace
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
        public int TMObjective_NbLaps
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
        public int? NbCheckpoints
        {
            get => nbCheckpoints;
            set => nbCheckpoints = value;
        }

        /// <summary>
        /// Map UID, environment, and author login.
        /// </summary>
        [NodeMember]
        public Ident MapInfo
        {
            get => mapInfo;
            set => mapInfo = value;
        }

        /// <summary>
        /// The map's UID.
        /// </summary>
        [NodeMember]
        public string MapUid
        {
            get => mapInfo.ID;
            set
            {
                mapInfo = new Ident(value, mapInfo.Collection, mapInfo.Author);
            }
        }

        /// <summary>
        /// Login of the map author.
        /// </summary>
        [NodeMember]
        public string AuthorLogin
        {
            get
            {
                DiscoverChunk<Chunk03043042>();

                if (authorLogin == null)
                    return mapInfo.Author;

                return authorLogin;
            }
            set
            {
                DiscoverChunk<Chunk03043042>();
                authorLogin = value;

                mapInfo = new Ident(value, mapInfo.Collection, mapInfo.Author);
            }
        }

        /// <summary>
        /// The map's name.
        /// </summary>
        [NodeMember]
        public string MapName
        {
            get => mapName;
            set => mapName = value;
        }

        /// <summary>
        /// The map's intended use.
        /// </summary>
        [NodeMember]
        public MapKind Kind
        {
            get => kind;
            set => kind = value;
        }

        /// <summary>
        /// Password of the map used by older maps.
        /// </summary>
        [NodeMember]
        public string Password
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
        public Ident Decoration
        {
            get => decoration;
            set => decoration = value;
        }

        /// <summary>
        /// Name of the map type script.
        /// </summary>
        [NodeMember]
        public string MapType
        {
            get
            {
                if (ChallengeParameters != null)
                    return ChallengeParameters.MapType;
                return mapType;
            }
            set
            {
                if (ChallengeParameters != null)
                    ChallengeParameters.MapType = value;
                mapType = value;
            }
        }

        /// <summary>
        /// Style of the map (Fullspeed, LOL, Tech), usually unused and defined by user.
        /// </summary>
        [NodeMember]
        public string MapStyle
        {
            get
            {
                if (ChallengeParameters != null)
                    return ChallengeParameters.MapStyle;
                return mapStyle;
            }
            set
            {
                if (ChallengeParameters != null)
                    ChallengeParameters.MapStyle = value;
                mapStyle = value;
            }
        }

        /// <summary>
        /// UID of the lightmap data stored in cache.
        /// </summary>
        [NodeMember]
        public ulong? LightmapCacheUID
        {
            get => lightmapCacheUID;
            set => lightmapCacheUID = value;
        }

        /// <summary>
        /// Version of the lightmap calculation.
        /// </summary>
        [NodeMember]
        public byte? LightmapVersion
        {
            get => lightmapVersion;
            set => lightmapVersion = value;
        }

        /// <summary>
        /// XML track information and dependencies.
        /// </summary>
        [NodeMember]
        public string XML
        {
            get => xml;
            set => xml = value;
        }

        /// <summary>
        /// Thumbnail JPEG data.
        /// </summary>
        [NodeMember]
        public byte[] Thumbnail
        {
            get => thumbnail;
            set => thumbnail = value;
        }

        /// <summary>
        /// The map's environment.
        /// </summary>
        [NodeMember]
        public Collection Collection
        {
            get => mapInfo.Collection;
            set => mapInfo = new Ident(mapInfo.ID, value, mapInfo.Author);
        }

        /// <summary>
        /// Origin of the map.
        /// </summary>
        [NodeMember]
        public Vec2? MapCoordOrigin
        {
            get => mapCoordOrigin;
            set => mapCoordOrigin = value;
        }

        /// <summary>
        /// Target of the map.
        /// </summary>
        [NodeMember]
        public Vec2? MapCoordTarget
        {
            get => mapCoordTarget;
            set => mapCoordTarget = value;
        }

        /// <summary>
        /// Title pack the map was built in.
        /// </summary>
        [NodeMember]
        public string TitleID
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
        public string BuildVersion
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
        public string Comments
        {
            get => comments;
            set => comments = value;
        }

        [NodeMember]
        public int AuthorVersion
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
        public string AuthorNickname
        {
            get
            {
                DiscoverChunk<Chunk03043042>();
                return authorNickname;
            }
            set => authorNickname = value;
        }

        /// <summary>
        /// Zone of the map author.
        /// </summary>
        [NodeMember]
        public string AuthorZone
        {
            get
            {
                DiscoverChunk<Chunk03043042>();
                return authorZone;
            }
            set => authorZone = value;
        }

        [NodeMember]
        public string AuthorExtraInfo
        {
            get
            {
                DiscoverChunk<Chunk03043042>();
                return authorExtraInfo;
            }
            set => authorExtraInfo = value;
        }

        /// <summary>
        /// The car's name, environment and author used on the map.
        /// </summary>
        [NodeMember]
        public Ident PlayerModel
        {
            get => playerModel;
            set => playerModel = value;
        }

        /// <summary>
        /// Map parameters.
        /// </summary>
        [NodeMember]
        public CGameCtnChallengeParameters ChallengeParameters => challengeParameters;

        /// <summary>
        /// List of available puzzle pieces.
        /// </summary>
        [NodeMember]
        public CGameCtnCollectorList BlockStock => blockStock;

        /// <summary>
        /// All checkpoints and their map coordinates. Used by TMUF and older games.
        /// </summary>
        [NodeMember]
        public Int3[] Checkpoints
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
        public FileRef ModPackDesc
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
        public Int3? Size
        {
            get => size;
            set => size = value;
        }

        [NodeMember]
        public bool? NeedUnlock
        {
            get => needUnlock;
            set => needUnlock = value;
        }

        /// <summary>
        /// List of all blocks on the map.
        /// </summary>
        [NodeMember]
        public List<CGameCtnBlock> Blocks
        {
            get => blocks;
            set => blocks = value;
        }

        /// <summary>
        /// Number of actual blocks on the map (doesn't include Unassigned1 and other blocks with <see cref="CGameCtnBlock.Flags"/> equal to -1).
        /// </summary>
        [NodeMember]
        public int NbBlocks => Blocks?.Count(x => x.Flags != -1) ?? 0;

        [NodeMember]
        public CGameCtnBlock[] BakedBlocks
        {
            get
            {
                DiscoverChunk<Chunk03043048>();
                return bakedBlocks;
            }
            set
            {
                DiscoverChunk<Chunk03043048>();
                bakedBlocks = value;
            }
        }

        /// <summary>
        /// MediaTracker intro.
        /// </summary>
        [NodeMember]
        public CGameCtnMediaClip ClipIntro
        {
            get => clipIntro;
            set => clipIntro = value;
        }

        /// <summary>
        /// MediaTracker ingame.
        /// </summary>
        [NodeMember]
        public CGameCtnMediaClipGroup ClipGroupInGame
        {
            get => clipGroupInGame;
            set => clipGroupInGame = value;
        }

        /// <summary>
        /// MediaTracker end race.
        /// </summary>
        [NodeMember]
        public CGameCtnMediaClipGroup ClipGroupEndRace
        {
            get => clipGroupEndRace;
            set => clipGroupEndRace = value;
        }

        /// <summary>
        /// MediaTracker ambiance.
        /// </summary>
        [NodeMember]
        public CGameCtnMediaClip ClipAmbiance
        {
            get => clipAmbiance;
            set => clipAmbiance = value;
        }

        /// <summary>
        /// MediaTracker podium.
        /// </summary>
        [NodeMember]
        public CGameCtnMediaClip ClipPodium
        {
            get => clipPodium;
            set => clipPodium = value;
        }

        /// <summary>
        /// Reference to the custom music used on the map.
        /// </summary>
        [NodeMember]
        public FileRef CustomMusicPackDesc
        {
            get => customMusicPackDesc;
            set => customMusicPackDesc = value;
        }

        /// <summary>
        /// Hashed password of the map, if it's password protected.
        /// </summary>
        [NodeMember]
        public byte[] HashedPassword
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

        /// <summary>
        /// List of all the available lightmap frames. Each frame can contain up to 3 different variants in either JPEG or WEBP format.
        /// </summary>
        public List<List<byte[]>> LightmapFrames
        {
            get
            {
                DiscoverChunk<Chunk0304303D>();
                return lightmapFrames;
            }
            set
            {
                DiscoverChunk<Chunk0304303D>();
                lightmapFrames = value;
            }
        }

        /// <summary>
        /// Lightmap cache information.
        /// </summary>
        [NodeMember]
        public Task<CHmsLightMapCache> LightmapCache
        {
            get
            {
                DiscoverChunk<Chunk0304303D>();
                return lightmapCache;
            }
            set
            {
                DiscoverChunk<Chunk0304303D>();
                lightmapCache = value;
            }
        }

        /// <summary>
        /// If the map has at least 1 lightmap frame.
        /// </summary>
        [NodeMember]
        public bool HasLightmaps => lightmapFrames?.Count > 0;

        /// <summary>
        /// List of all items and objects placed on the map.
        /// </summary>
        [NodeMember]
        public List<CGameCtnAnchoredObject> AnchoredObjects
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
        public Task<CGameCtnZoneGenealogy[]> Genealogies
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
        public CScriptTraitsMetadata ScriptMetadata
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
        public string ObjectiveTextAuthor
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
        public string ObjectiveTextGold
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
        public string ObjectiveTextSilver
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
        public string ObjectiveTextBronze
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
        public List<(Int3 start, Int3 end)> Offzones
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
        public List<BotPath> BotPaths
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
        /// Embedded objects in the map. Key defines a relative path. Value is the actual embedded data, usually in GBX format.
        /// </summary>
        [NodeMember]
        public Dictionary<string, byte[]> EmbeddedObjects
        {
            get
            {
                DiscoverChunk<Chunk03043054>();
                return embeddedObjects;
            }
        }

        /// <summary>
        /// Exact time of the day in the map. Available since ManiaPlanet 4.
        /// </summary>
        [NodeMember]
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
        public TimeSpan? DayDuration
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

        #endregion

        #region Methods

        /// <summary>
        /// Exports the map's thumbnail as JPEG.
        /// </summary>
        /// <param name="stream">Stream to export to.</param>
        public void ExportThumbnail(Stream stream)
        {
            if (thumbnail == null) return;
            stream.Write(thumbnail, 0, thumbnail.Length);
        }

        /// <summary>
        /// Exports the map's thumbnail as JPEG.
        /// </summary>
        /// <param name="fileName">File to export to.</param>
        public void ExportThumbnail(string fileName)
        {
            if (thumbnail == null) return;
            using (var fs = File.OpenWrite(fileName))
                ExportThumbnail(fs);
        }

        /// <summary>
        /// Sets a new map password.
        /// </summary>
        /// <param name="password">Password that will be hashed.</param>
        public void NewPassword(string password)
        {
            var md5 = MD5.Create();
            hashedPassword = md5.ComputeHash(Encoding.UTF8.GetBytes(password));

            //Crc32 crc32 = new Crc32();
            //crc32.Update(Encoding.ASCII.GetBytes("0x" + BitConverter.ToInt16(HashedPassword, 0).ToString() + "???" + MapUid));
            //CRC32 = Convert.ToUInt32(crc32.Value);
        }

        /// <summary>
        /// Cracks the map password.
        /// </summary>
        public void CrackPassword()
        {
            password = null;
            RemoveChunk<Chunk03043029>();
        }

        /// <summary>
        /// Gets the first block at this position.
        /// </summary>
        /// <param name="pos">Position of the block.</param>
        /// <returns>The first available block.</returns>
        public CGameCtnBlock GetBlock(Int3 pos) => blocks.FirstOrDefault(x => x.Coord == pos);

        /// <summary>
        /// Gets the first block at this position.
        /// </summary>
        /// <param name="x">X position.</param>
        /// <param name="y">Y position.</param>
        /// <param name="z">Z position.</param>
        /// <returns>The first available block.</returns>
        public CGameCtnBlock GetBlock(int x, int y, int z) => GetBlock((x, y, z));

        /// <summary>
        /// Retrieves blocks at this position.
        /// </summary>
        /// <param name="pos">Position of the block.</param>
        /// <returns>An enumerable of blocks.</returns>
        public IEnumerable<CGameCtnBlock> GetBlocks(Int3 pos) => blocks.Where(x => x.Coord == pos);

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
        public IEnumerable<CGameCtnBlock> GetGhostBlocks() => blocks.Where(x => x.IsGhost);

        /// <summary>
        /// Places a block in the map.
        /// </summary>
        /// <param name="blockModel">Block model name to place. Only the name is required, so using <see cref="Ident(string)"/> works too. Full <see cref="Ident"/> can be seen in TM1.0.</param>
        /// <param name="coord">Position on the map. Should be always under <see cref="Size"/>, otherwise an overflow can happen.</param>
        /// <param name="dir">Facing direction of the block.</param>
        /// <returns>A placed block.</returns>
        public CGameCtnBlock PlaceBlock(Ident blockModel, Int3 coord, Direction dir)
        {
            var block = new CGameCtnBlock()
            {
                BlockModel = blockModel,
                Coord = coord,
                Direction = dir
            };

            block.CreateChunk<CGameCtnBlock.Chunk03057002>();

            blocks.Add(block);

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
        public void RemoveAllBlocks(Predicate<CGameCtnBlock> match)
        {
            blocks.RemoveAll(match);
        }

        /// <summary>
        /// Removes all the blocks from the map.
        /// </summary>
        public void RemoveAllBlocks()
        {
            ClearBlocks();
        }

        /// <summary>
        /// Clears all the blocks from the map.
        /// </summary>
        public void ClearBlocks()
        {
            blocks.Clear();
        }

        /// <summary>
        /// Places an item on a map.
        /// </summary>
        /// <param name="itemModel">An item model identification (name, collection and author). Only the name is required, so using <see cref="Ident(string)"/> works too.</param>
        /// <param name="absolutePosition">Absolute position in the map.</param>
        /// <param name="pitchYawRoll">Rotation of the item in pitch, yaw, and roll format.</param>
        /// <param name="offsetPivot">Pivot location of the item (relative position of the point the item will rotate around).</param>
        /// <param name="variant">An item variant (trees from TM2020 have different variants).</param>
        /// <returns>Placed item.</returns>
        public CGameCtnAnchoredObject PlaceAnchoredObject(Ident itemModel, Vec3 absolutePosition, Vec3 pitchYawRoll, Vec3 offsetPivot = default, int variant = 0)
        {
            CreateChunk<Chunk03043040>();

            var anchoredObject = new CGameCtnAnchoredObject()
            {
                ItemModel = itemModel,
                AbsolutePositionInMap = absolutePosition,
                PitchYawRoll = pitchYawRoll,
                PivotPosition = offsetPivot,
                Variant = variant
            };

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
        public CGameCtnAnchoredObject PlaceAnchoredObject(string itemModel, Vec3 absolutePosition, Vec3 pitchYawRoll, Vec3 offsetPivot = default, int variant = 0)
        {
            return PlaceAnchoredObject(new Ident(itemModel), absolutePosition, pitchYawRoll, offsetPivot, variant);
        }

        /// <summary>
        /// FREE BLOCKS ARE CURRENTLY ALWAYS REMOVED FROM MAPS AFTER SAVING. Place a free block on a map. The placed block SHOULDN'T HAVE CLIPS, otherwise the map can break.
        /// </summary>
        /// <param name="name">Name of the block to place.</param>
        /// <param name="absolutePosition">Absolute position of the free block.</param>
        /// <param name="pitchYawRoll">Rotation of the free block in pitch, yaw, and roll format.</param>
        /// <param name="skin">Skin to use on the free block.</param>
        /// <returns>A newly placed block.</returns>
        public CGameCtnBlock PlaceFreeBlock(string name, Vec3 absolutePosition, Vec3 pitchYawRoll, CGameCtnBlockSkin skin = null)
        {
            CreateChunk<Chunk0304305F>();

            var block = new CGameCtnBlock(name, Direction.North, (-1, -1, -1))
            {
                IsFree = true,
                AbsolutePositionInMap = absolutePosition,
                PitchYawRoll = pitchYawRoll,
                Skin = skin
            };

            Blocks.Add(block);

            return block;
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

            if (chunk021 == null) return false;

            if (ClipIntro != null)
                ConvertMediaClip(ClipIntro);

            if (ClipGroupInGame != null)
                ConvertMediaClipGroup(ClipGroupInGame, upscaleTriggerCoord);

            if (ClipGroupEndRace != null)
                ConvertMediaClipGroup(ClipGroupEndRace, upscaleTriggerCoord);

            RemoveChunk<Chunk03043021>();

            void ConvertMediaClip(CGameCtnMediaClip node)
            {
                foreach (var track in node.Tracks)
                    if (track != null)
                        ConvertMediaTrack(track);
            }

            void ConvertMediaClipGroup(CGameCtnMediaClipGroup node, int upscTriggerCoord)
            {
                foreach(var clip in node.Clips)
                {
                    var trigger = clip.Item2;

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

                    ConvertMediaClip(clip.Item1);
                }
            }

            void ConvertMediaTrack(CGameCtnMediaTrack node)
            {
                var chunk001 = node.GetChunk<CGameCtnMediaTrack.Chunk03078001>();

                // Chunks 0x002 and 0x004 have to be replaced with 0x005 so that ManiaPlanet accepts the map
                node.TransferMediaTrackTo005();

                node.Blocks.RemoveAll(x => x is CGameCtnMediaBlockGhost); // Some ghosts can crash the game

                node.Blocks.RemoveAll(x => x is CGameCtnMediaBlockFxBloom);
                // FX Bloom is no longer supported and was remade into FxBloomHdr
                // TODO: convert to fx bloom hdr effectively
            }

            return true;
        }

        /// <summary>
        /// Offsets all MediaTracker camera positions by <paramref name="offset"/>.
        /// </summary>
        /// <param name="offset">Amount of units to offset the cameras.</param>
        public void OffsetMediaTrackerCameras(Vec3 offset)
        {
            OffsetCamerasInClip(ClipIntro);
            OffsetCamerasInClipGroup(ClipGroupInGame);
            OffsetCamerasInClipGroup(ClipGroupEndRace);
            OffsetCamerasInClip(ClipAmbiance);
            OffsetCamerasInClip(ClipPodium);

            void OffsetCamerasInClipGroup(CGameCtnMediaClipGroup group)
            {
                if (group == null) return;

                foreach (var clip in group.Clips)
                    OffsetCamerasInClip(clip.Item1);
            }

            void OffsetCamerasInClip(CGameCtnMediaClip clip)
            {
                if (clip == null) return;

                if (clip.Tracks != null)
                {
                    foreach (var track in clip.Tracks)
                    {
                        if (track.Blocks != null)
                        {
                            foreach (var block in track.Blocks)
                            {
                                if(block is CGameCtnMediaBlockCameraCustom c)
                                {
                                    if (c.Keys != null)
                                        foreach (var key in c.Keys)
                                            if(key.Anchor == -1)
                                                key.Position += offset;
                                }
                                else if(block is CGameCtnMediaBlockCameraPath p)
                                {
                                    if (p.Keys != null)
                                        foreach (var key in p.Keys)
                                            if(key.Anchor == -1)
                                                key.Position += offset;
                                }
                            }
                        }
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
            OffsetTriggers(ClipGroupInGame);
            OffsetTriggers(ClipGroupEndRace);

            void OffsetTriggers(CGameCtnMediaClipGroup group)
            {
                if (group == null) return;

                foreach(var clip in group.Clips)
                {
                    var trigger = clip.Item2;
                    trigger.Coords = trigger.Coords.Select(x => x + offset).ToArray();
                }
            }
        }

        /// <summary>
        /// Enumerates through all of the embedded objects and yields their header data through the <see cref="GameBox"/> object.
        /// </summary>
        /// <returns>An enumerable of <see cref="GameBox"/> objects with header data only.</returns>
        public IEnumerable<GameBox> GetEmbeddedObjects()
        {
            foreach(var embed in EmbeddedObjects)
            {
                using (var ms = new MemoryStream(embed.Value))
                {
                    var gbx = GameBox.ParseHeader(ms);
                    gbx.FileName = embed.Key;
                    yield return gbx;
                }
            }
        }

        /// <summary>
        /// Extracts embed ZIP file based on the data in <see cref="EmbeddedObjects"/>. File metadata is simplified and the timestamp of extraction is used for all files. Stream must have permission to read.
        /// </summary>
        /// <param name="stream">Stream to write the ZIP data to.</param>
        public void ExtractEmbedZip(Stream stream)
        {
            using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, true))
            {
                foreach (var embed in EmbeddedObjects)
                {
                    var entry = zip.CreateEntry(embed.Key.Replace('\\', '/'));
                    using (var s = entry.Open())
                        s.Write(embed.Value, 0, embed.Value.Length);
                }
            }
        }

        /// <summary>
        /// Extracts embed ZIP file based on the data in <see cref="EmbeddedObjects"/>. File metadata is simplified and the timestamp of extraction is used for all files.
        /// </summary>
        /// <param name="fileName">New file to write the ZIP data to.</param>
        public void ExtractEmbedZip(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                ExtractEmbedZip(fs); // ExtractEmbedZip(Stream stream) required Stream to be readable as well
        }

        /// <summary>
        /// Extracts embed ZIP file straight from the parsed map including original timestamps and other file metadata.
        /// </summary>
        /// <param name="stream">Stream to write the ZIP data to.</param>
        /// <returns>True if the map included embed ZIP previously, otherwise false.</returns>
        public bool ExtractOriginalEmbedZip(Stream stream)
        {
            DiscoverChunk<Chunk03043054>();
            if (originalEmbedZip.Length == 0) return false;

            using (var ms = new MemoryStream(originalEmbedZip))
                ms.CopyTo(stream);
            return true;
        }

        /// <summary>
        /// Extracts embed ZIP file straight from the parsed map including original timestamps and other file metadata.
        /// </summary>
        /// <param name="fileName">New file to write the ZIP data to.</param>
        /// <returns>True if the map included embed ZIP previously, otherwise false.</returns>
        public bool ExtractOriginalEmbedZip(string fileName)
        {
            DiscoverChunk<Chunk03043054>();
            if (originalEmbedZip.Length == 0) return false;

            File.WriteAllBytes(fileName, originalEmbedZip);
            return true;
        }

        /// <summary>
        /// Import a file to embed in the map by keeping the file name but relocating it in the embed ZIP.
        /// </summary>
        /// <param name="fileOnDisk">File to embed located on the disk.</param>
        /// <param name="relativeDirectory">Relative directory where the embed should be represented in the game, usually starts with <c>"Items/..."</c>, <c>"Blocks/..."</c> or <c>"Materials/..."</c>.</param>
        /// <param name="keepIcon">Keep the icon (chunk 0x2E001004) of the embedded GBX. Increases total unneeded embed size.</param>
        public void ImportFileToEmbed(string fileOnDisk, string relativeDirectory, bool keepIcon)
        {
            var data = File.ReadAllBytes(fileOnDisk);

            if (!keepIcon)
            {
                using (var gbxOutms = new MemoryStream())
                using (var gbxOutw = new GameBoxWriter(gbxOutms))
                using (var gbxms = new MemoryStream())
                using (var gbxr = new GameBoxReader(gbxms))
                {
                    gbxms.Write(data, 0, data.Length);
                    gbxms.Position = 0;

                    if (gbxr.HasMagic(GameBox.Magic))
                    {
                        var basic = gbxr.ReadBytes(6); // gbx basic

                        var classID = gbxr.ReadUInt32();

                        var userDataPos = gbxms.Position;

                        var userData = gbxr.ReadBytes();

                        using (var msNewUserData = new MemoryStream())
                        using (var wNewUserData = new GameBoxWriter(msNewUserData))
                        using (var msUserData = new MemoryStream(userData))
                        using (var rUserData = new GameBoxReader(msUserData))
                        {
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

                            foreach(var (chunkID, size) in headers)
                            {
                                var chunkData = rUserData.ReadBytes(size);

                                if (chunkID != 0x2E001004)
                                    wNewUserData.Write(chunkData);
                            }

                            gbxOutw.Write("GBX", StringLengthPrefix.None);
                            gbxOutw.Write(basic, 0, basic.Length);
                            gbxOutw.Write(classID);
                            gbxOutw.Write((int)msNewUserData.Length);
                            gbxOutw.Write(msNewUserData.ToArray(), 0, (int)msNewUserData.Length);
                        }

                        gbxms.CopyTo(gbxOutms);
                    }

                    data = gbxOutms.ToArray();
                }
            }

            EmbeddedObjects[relativeDirectory + "/" + Path.GetFileName(fileOnDisk)] = data;
        }

        /// <summary>
        /// Import a file to embed in the map by keeping the file name but relocating it in the embed ZIP.
        /// </summary>
        /// <param name="fileOnDisk">File to embed located on the disk.</param>
        /// <param name="relativeDirectory">Relative directory where the embed should be represented in the game, usually starts with <c>"Items/..."</c>, <c>"Blocks/..."</c> or <c>"Materials/..."</c>.</param>
        public void ImportFileToEmbed(string fileOnDisk, string relativeDirectory)
        {
            ImportFileToEmbed(fileOnDisk, relativeDirectory, false);
        }

        /// <summary>
        /// Embed objects from directories represented like from the user data directory.
        /// </summary>
        /// <param name="directoryOnDisk">Directory with folders <c>"Items/..."</c>, <c>"Blocks/..."</c> or <c>"Materials/..."</c>.</param>
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
        public void PlaceMacroblock(CGameCtnMacroBlockInfo macroblock, Int3 coord, Direction dir)
        {
            var macroRad = (int)dir * (Math.PI / 2); // Rotation of the macroblock in radians needed for the formula to determine individual coords
            var macroBlocks = macroblock.Blocks.Where(x => !x.IsFree).ToArray();
            var allCoords = macroBlocks.Select(x => x.Coord); // Creates an enumerable of macroblock block coords

            var min = new Int3(allCoords.Select(x => x.X).Min(), allCoords.Select(x => x.Y).Min(), allCoords.Select(x => x.Z).Min());
            var max = new Int3(allCoords.Select(x => x.X).Max(), allCoords.Select(x => x.Y).Max(), allCoords.Select(x => x.Z).Max());
            // Calculates the minimum and maximum coord, used to determine size and center of the macroblock

            var size = max - min + (1, 1, 1);
            var center = (min + max) * .5f;

            var newCoords = new Vec3[macroBlocks.Length]; // Array used to store new rotated block positions

            for (var i = 0; i < newCoords.Length; i++)
            {
                var block = macroBlocks[i];
                var blockCoord = (Vec3)block.Coord;

                var blockCenter = new Vec3();
                // Temporary center variable whose rule is to properly position blocks over size of 1x1

                if (BlockInfoManager.BlockModels.TryGetValue(block.Name, out BlockModel model)) // Get quick block information if available
                {
                    BlockUnit[] blockUnits;
                    if (block.IsGround)
                        blockUnits = model.Ground;
                    else
                        blockUnits = model.Air;
                    // Use the block units from what the block actually pretends to be placed on

                    if (blockUnits.Length > 1) // Optimization for blocks that are simple 1x1 size
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

                newCoords[i] = new Vec3((float)offsetX, block.Coord.Y, (float)offsetZ); // Applies the result to the array, Y isn't affected
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
                    block.Name,
                    Dir.Add(block.Direction, dir),
                    coord + (Int3)newRelativeCoord,
                    block.Flags,
                    block.Author,
                    block.Skin,
                    block.WaypointSpecialProperty
                );

                if (b.Coord.Y == 0)
                    b.IsGround = true;

                Blocks.Add(b);
            }

            var blockSize = Collection.GetBlockSize();

            foreach (var item in macroblock.AnchoredObjects)
            {
                var itemRadians = (float)((int)dir * Math.PI / 2);
                var blockCenterVec = size * blockSize * new Vec3(0.5f, 0f, 0.5f);
                var offsetPos = AdditionalMath.RotateAroundCenter(item.AbsolutePositionInMap, blockCenterVec, itemRadians);
                offsetPos -= newMin * blockSize;

                Int3 offsetCollection = (0, blockSize.Y, 0);

                if (GetChunk<Chunk0304301F>().Version <= 1)
                    offsetCollection += (32, 0, 32);

                PlaceAnchoredObject(item.ItemModel, offsetPos + coord * blockSize + offsetCollection, item.PitchYawRoll + (-itemRadians, 0f, 0f));
            }

            foreach(var freeBlock in macroblock.Blocks.Where(x => x.IsFree))
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
        public class Chunk03043001 : HeaderChunk<CGameCtnChallenge>
        {
            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public byte Version { get; set; }

            public Ident MapInfo { get; set; }

            public string MapName { get; set; }

            public BoatName BoatName { get; set; }

            public string Boat { get; set; }

            public string BoatAuthor { get; set; }

            public RaceMode RaceMode { get; set; }

            public WindDirection WindDirection { get; set; }

            public byte WindStrength { get; set; }

            public Weather Weather { get; set; }

            public StartDelay StartDelay { get; set; }

            public int StartTime { get; set; }

            public TimeSpan TimeLimit { get; set; }

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

            public bool U01 { get; set; }
            public int U02 { get; set; }
            public byte U03 { get; set; }
            public byte U04 { get; set; }
            public byte U05 { get; set; }
            public byte U06 { get; set; }
            public byte U07 { get; set; }
            public bool U08 { get; set; }
            public string U09 { get; set; }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                Version = rw.Byte(Version);

                if (Version < 1)
                {
                    MapInfo = rw.Ident(MapInfo);
                    MapName = rw.String(MapName);
                }

                U01 = rw.Boolean(U01);
                U02 = rw.Int32(U02);

                if (Version < 1)
                    U03 = rw.Byte(U03);

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
                    TimeLimit = TimeSpan.FromMilliseconds(rw.Int32(Convert.ToInt32(TimeLimit.TotalMilliseconds)));
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
        public class Chunk03043002 : HeaderChunk<CGameCtnChallenge>
        {
            private byte version;

            private bool u01;
            private byte u02;
            private int u03;
            private int u04;
            private int u05;

            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public byte Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Byte(ref version);

                if (version < 3)
                {
                    rw.Ident(ref n.mapInfo);
                    rw.String(ref n.mapName);
                }

                rw.Boolean(ref u01);

                if (version >= 1)
                {
                    rw.TimeSpan32(ref n.bronzeTime);
                    rw.TimeSpan32(ref n.silverTime);
                    rw.TimeSpan32(ref n.goldTime);
                    rw.TimeSpan32(ref n.authorTime);

                    if (version == 2)
                        rw.Byte(ref u02);

                    if (version >= 4)
                    {
                        rw.Int32(ref n.cost);

                        if (version >= 5)
                        {
                            rw.Boolean(ref n.isLapRace);

                            if (version == 6)
                                rw.Int32(ref u03);

                            if (version >= 7)
                            {
                                rw.EnumInt32<PlayMode>(ref n.mode);

                                if (version >= 9)
                                {
                                    rw.Int32(ref u04);

                                    if (version >= 10)
                                    {
                                        rw.Int32(ref n.authorScore);

                                        if (version >= 11)
                                        {
                                            rw.EnumInt32<EditorMode>(ref n.editor);

                                            if (version >= 12)
                                            {
                                                rw.Int32(ref u05);

                                                if (version >= 13)
                                                {
                                                    rw.Int32(ref n.nbCheckpoints);
                                                    rw.Int32(ref n.nbLaps);
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
        public class Chunk03043003 : HeaderChunk<CGameCtnChallenge>
        {
            private byte version;
            private bool? locked;

            private byte[] u01 = new byte[16];
            private int u02;

            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public byte Version
            {
                get => version;
                set => version = value;
            }

            /// <summary>
            /// If the track is locked (used by Virtual Skipper to lock the map parameters). Can be null if <c><see cref="Version"/> &lt; 1</c>.
            /// </summary>
            public bool? Locked
            {
                get => locked;
                set => locked = value;
            }

            public byte[] U01
            {
                get => u01;
                set => u01 = value;
            }

            public int U02
            {
                get => u02;
                set => u02 = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Byte(ref version);
                rw.Ident(ref n.mapInfo);
                rw.String(ref n.mapName);
                rw.EnumByte<MapKind>(ref n.kind);

                if (version >= 1)
                {
                    locked = rw.UInt32(Convert.ToUInt32(locked.GetValueOrDefault())) == 1;
                    rw.String(ref n.password);

                    if (version >= 2)
                    {
                        rw.Ident(ref n.decoration);

                        if (version >= 3)
                        {
                            rw.Vec2(ref n.mapCoordOrigin);

                            if (version >= 4)
                            {
                                rw.Vec2(ref n.mapCoordTarget);

                                if (version >= 5)
                                {
                                    rw.Bytes(ref u01, 16);

                                    if (version >= 6)
                                    {
                                        rw.String(ref n.mapType);
                                        rw.String(ref n.mapStyle);

                                        if (version <= 8)
                                            rw.Int32(ref u02);

                                        if (version >= 8)
                                        {
                                            rw.UInt64(ref n.lightmapCacheUID);

                                            if (version >= 9)
                                            {
                                                rw.Byte(ref n.lightmapVersion);

                                                if (version >= 11)
                                                    rw.Id(ref n.titleID);
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
        public class Chunk03043004 : HeaderChunk<CGameCtnChallenge>
        {
            private int version;

            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
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
        public class Chunk03043007 : HeaderChunk<CGameCtnChallenge>
        {
            private int version;

            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);

                if (version != 0)
                {
                    var thumbnailSize = rw.Int32(n.thumbnail?.Length ?? 0);

                    rw.Bytes(Encoding.UTF8.GetBytes("<Thumbnail.jpg>"), "<Thumbnail.jpg>".Length); // Because the string is purely ASCII anyway, Length is usable
                    rw.Bytes(ref n.thumbnail, thumbnailSize);
                    rw.Bytes(Encoding.UTF8.GetBytes("</Thumbnail.jpg>"), "</Thumbnail.jpg>".Length);

                    rw.Bytes(Encoding.UTF8.GetBytes("<Comments>"), "<Comments>".Length);
                    rw.String(ref n.comments);
                    rw.Bytes(Encoding.UTF8.GetBytes("</Comments>"), "</Comments>".Length);
                }
            }
        }

        #endregion

        #region 0x008 header chunk (author)

        /// <summary>
        /// CGameCtnChallenge 0x008 header chunk (author)
        /// </summary>
        [Chunk(0x03043008, "author")]
        public class Chunk03043008 : HeaderChunk<CGameCtnChallenge>
        {
            private int version;

            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
                rw.Int32(ref n.authorVersion);
                rw.String(ref n.authorLogin);
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
            private int version;

            public int Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Ident(ref n.mapInfo);
                rw.Int3(ref n.size);
                rw.Int32(ref version);
                rw.ListNode<CGameCtnBlock>(ref n.blocks);
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
                rw.String(ref n.mapName);
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
            private int u01;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref u01);
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
            private int u01;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref u01);
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
            private CMwNod u01;

            public CMwNod U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.NodeRef(ref u01);
            }
        }

        #endregion

        #region 0x01B chunk

        /// <summary>
        /// CGameCtnChallenge 0x01B chunk
        /// </summary>
        [Chunk(0x0304301B)]
        public class Chunk0304301B : Chunk<CGameCtnChallenge>
        {
            private int u01;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref u01);
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
                n.mode = (PlayMode)rw.Int32((int)n.mode.GetValueOrDefault());
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
            private CMwNod u01;

            public CMwNod U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.NodeRef(ref u01);
            }
        }

        #endregion

        #region 0x01F chunk (block data)

        /// <summary>
        /// CGameCtnChallenge 0x01F chunk (block data)
        /// </summary>
        [Chunk(0x0304301F, "block data")]
        public class Chunk0304301F : Chunk<CGameCtnChallenge>
        {
            public bool NeedUnlock { get; set; }

            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int? Version { get; set; }

            readonly bool is013;

            public Chunk0304301F() : this(null)
            {

            }

            public Chunk0304301F(Chunk chunk) : base()
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
                    Version = r.ReadInt32();

                var nbBlocks = r.ReadInt32(); // It's maybe slower but better for the program to determine the count from the list

                n.blocks = new List<CGameCtnBlock>();

                var blockCounter = 0;

                while ((r.PeekUInt32() & 0xC0000000) > 0)
                {
                    var blockName = r.ReadId();
                    var dir = (Direction)r.ReadByte();
                    var coord = (Int3)r.ReadByte3();
                    var flags = -1;

                    if (Version >= 6)
                        coord -= (1, 1, 1);

                    if (Version == null)
                        flags = r.ReadUInt16();
                    else if (Version > 0)
                        flags = r.ReadInt32();

                    if (flags == -1)
                    {
                        n.blocks.Add(new CGameCtnBlock(blockName, dir, coord, flags, null, null, null));
                        continue;
                    }

                    string author = null;
                    CGameCtnBlockSkin skin = null;

                    if ((flags & (1 << 15)) != 0) // custom block
                    {
                        author = r.ReadId();
                        skin = r.ReadNodeRef<CGameCtnBlockSkin>();
                    }

                    CGameWaypointSpecialProperty parameters = null;

                    if ((flags & (1 << 20)) != 0)
                        parameters = r.ReadNodeRef<CGameWaypointSpecialProperty>();

                    if ((flags & (1 << 18)) != 0)
                    {

                    }

                    if ((flags & (1 << 17)) != 0)
                    {

                    }

                    n.blocks.Add(new CGameCtnBlock(blockName, dir, coord, flags, author, skin, parameters));

                    blockCounter++;
                }

                // Debug.Assert(blockCounter == nbBlocks);
            }

            public override void Write(CGameCtnChallenge n, GameBoxWriter w)
            {
                w.Write(n.mapInfo);
                w.Write(n.mapName);
                w.Write(n.decoration);
                w.Write(n.size.GetValueOrDefault());
                w.Write(NeedUnlock);

                if (!is013)
                    w.Write(Version.GetValueOrDefault());

                // Remove all free blocks with clips
                for(int i = 0; i < n.blocks.Count; i++)
                {
                    var x = n.blocks[i];
                    var skip = false;

                    if (x.IsFree)
                        if (BlockInfoManager.BlockModels != null
                            && BlockInfoManager.BlockModels.TryGetValue(x.Name, out BlockModel m))
                            foreach (var unit in m.Air)
                                if (unit.Clips != null)
                                    foreach (var clip in unit.Clips)
                                        if (!string.IsNullOrEmpty(clip))
                                            skip = true;

                    if (skip)
                        n.blocks[i].IsFree = false;
                }

                n.blocks.RemoveAll(x => !x.IsFree && x.Coord == (-1, -1, -1) && x.Flags != -1);
                //

                w.Write(n.NbBlocks);

                foreach (var x in n.blocks)
                {
                    w.WriteId(x.Name);
                    w.Write((byte)x.Direction);

                    var coord = x.Coord;
                    if (Version >= 6)
                        coord += (1, 1, 1);
                    w.Write((Byte3)coord);

                    if (Version == null)
                        w.Write((short)x.Flags);
                    else if (Version > 0)
                        w.Write(x.Flags);

                    if (x.Flags != -1)
                    {
                        if ((x.Flags & 0x8000) != 0) // custom block
                        {
                            w.WriteId(x.Author);
                            w.Write(x.Skin);
                        }

                        if ((x.Flags & 0x100000) != 0)
                            w.Write(x.WaypointSpecialProperty);
                    }
                }
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
        }

        #endregion

        #region 0x022 chunk

        /// <summary>
        /// CGameCtnChallenge 0x022 chunk
        /// </summary>
        [Chunk(0x03043022)]
        public class Chunk03043022 : Chunk<CGameCtnChallenge>
        {
            private int u01;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref u01);
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

                if(rw.Mode == GameBoxReaderWriterMode.Read) // TODO: check
                {
                    var idk = rw.Reader.ReadInt32();
                    if (idk != 0)
                        rw.Reader.BaseStream.Position -= sizeof(int);
                }
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
            private CMwNod clipGlobal;

            public CMwNod ClipGlobal
            {
                get => clipGlobal;
                set => clipGlobal = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.NodeRef(ref clipGlobal);
            }
        }

        #endregion

        #region 0x027 chunk

        /// <summary>
        /// CGameCtnChallenge 0x027 chunk
        /// </summary>
        [Chunk(0x03043027)]
        public class Chunk03043027 : Chunk<CGameCtnChallenge>
        {
            private bool archiveGmCamVal;
            private byte u01;
            private Vec3 u02;
            private Vec3 u03;
            private Vec3 u04;
            private float u05;
            private float u06;

            public bool ArchiveGmCamVal
            {
                get => archiveGmCamVal;
                set => archiveGmCamVal = value;
            }

            public byte U01
            {
                get => u01;
                set => u01 = value;
            }

            public Vec3 U02
            {
                get => u02;
                set => u02 = value;
            }

            public Vec3 U03
            {
                get => u03;
                set => u03 = value;
            }

            public Vec3 U04
            {
                get => u04;
                set => u04 = value;
            }

            public float U05
            {
                get => u05;
                set => u05 = value;
            }

            public float U06
            {
                get => u06;
                set => u06 = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Boolean(ref archiveGmCamVal);

                if(ArchiveGmCamVal)
                {
                    rw.Byte(ref u01);

                    rw.Vec3(ref u02);
                    rw.Vec3(ref u03);
                    rw.Vec3(ref u04);

                    rw.Vec3(ref n.thumbnailPosition);
                    rw.Single(ref n.thumbnailFOV);
                    rw.Single(ref u05);
                    rw.Single(ref u06);
                }
            }
        }

        #endregion

        #region 0x028 chunk (comments)

        /// <summary>
        /// CGameCtnChallenge 0x028 chunk (comments)
        /// </summary>
        [Chunk(0x03043028, "comments")]
        public class Chunk03043028 : Chunk<CGameCtnChallenge>
        {
            public Chunk03043027 Chunk027 { get; }

            public Chunk03043028()
            {
                Chunk027 = new Chunk03043027();
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                Chunk027.Unknown.Position = 0;
                Chunk027.ReadWrite(n, rw);
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
            private bool u01;

            public bool U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Boolean(ref u01);
            }
        }

        #endregion

        #region 0x036 skippable chunk (realtime thumbnail)

        /// <summary>
        /// CGameCtnChallenge 0x036 skippable chunk (realtime thumbnail)
        /// </summary>
        [Chunk(0x03043036, "realtime thumbnail")]
        public class Chunk03043036 : SkippableChunk<CGameCtnChallenge>
        {
            private byte[] u01 = new byte[31];

            public byte[] U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Vec3(ref n.thumbnailPosition);
                rw.Vec3(ref n.thumbnailPitchYawRoll);
                rw.Single(ref n.thumbnailFOV);

                rw.Bytes(ref u01, 31);
            }
        }

        #endregion

        #region 0x03D skippable chunk (lightmaps)

        /// <summary>
        /// CGameCtnChallenge 0x03D skippable chunk (lightmaps)
        /// </summary>
        [Chunk(0x0304303D, "lightmaps")]
        public class Chunk0304303D : SkippableChunk<CGameCtnChallenge>
        {
            private bool u01;
            private int version = 4;
            private byte[] cacheData;

            public bool U01
            {
                get => u01;
                set => u01 = value;
            }

            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version
            {
                get => version;
                set => version = value;
            }

            public byte[] CacheData
            {
                get => cacheData;
                set => cacheData = value;
            }

            public override void Read(CGameCtnChallenge n, GameBoxReader r)
            {
                u01 = r.ReadBoolean();
                version = r.ReadInt32();

                if (version >= 5)
                {
                    var frameCount = r.ReadInt32();
                    n.lightmapFrames = new List<List<byte[]>>(frameCount);

                    for (var i = 0; i < frameCount; i++)
                        n.lightmapFrames.Add(new List<byte[]>());
                }
                else
                {
                    n.lightmapFrames = new List<List<byte[]>>
                    {
                        new List<byte[]>()
                    };
                }

                if (version >= 2)
                {
                    foreach (var frame in n.lightmapFrames)
                    {
                        frame.Add(r.ReadBytes());

                        if (version >= 3)
                        {
                            frame.Add(r.ReadBytes());
                        }

                        if (version >= 6)
                        {
                            frame.Add(r.ReadBytes());
                        }
                    }

                    if (n.lightmapFrames.Any(x => x.Any(y => y.Length > 0)))
                    {
                        var uncompressedSize = r.ReadInt32();
                        var compressedSize = r.ReadInt32();
                        var data = r.ReadBytes(compressedSize);

                        n.lightmapCache = Task.Run(() =>
                        {
                            using (var ms = new MemoryStream(data))
                            using (var deflate = new CompressedStream(ms, CompressionMode.Decompress))
                            using (var gbxr = new GameBoxReader(deflate))
                            {
                                var cacheNode = Parse<CHmsLightMapCache>(gbxr, 0x06022000);

                                using (var msOut = new MemoryStream())
                                {
                                    deflate.CopyTo(msOut);
                                    CacheData = msOut.ToArray();
                                }

                                return cacheNode;
                            }
                        });
                    }
                }
            }

            public override void Write(CGameCtnChallenge n, GameBoxWriter w)
            {
                w.Write(u01);
                w.Write(version);

                if (version >= 5)
                {
                    w.Write(n.lightmapFrames.Count);
                }

                if (version >= 2)
                {
                    foreach (var frame in n.lightmapFrames)
                    {
                        w.Write(frame[0].Length);
                        w.WriteBytes(frame[0]);

                        if (version >= 3)
                        {
                            w.Write(frame[1].Length);
                            w.WriteBytes(frame[1]);
                        }

                        if (version >= 6)
                        {
                            w.Write(frame[2].Length);
                            w.WriteBytes(frame[2]);
                        }
                    }

                    if (n.lightmapFrames.Any(x => x.Any(y => y.Length > 0)))
                    {
                        using (var ms = new MemoryStream())
                        using (var gbxw = new GameBoxWriter(ms))
                        {
                            n.lightmapCache.Result.Write(gbxw);
                            gbxw.WriteBytes(CacheData);

                            w.Write((int)ms.Length);

                            ms.Seek(0, SeekOrigin.Begin);

                            using (var msCompressed = new MemoryStream())
                            using (var deflate = new CompressedStream(msCompressed, CompressionMode.Compress))
                            {
                                ms.CopyTo(deflate);

                                w.Write((int)msCompressed.Length);
                                w.WriteBytes(msCompressed.ToArray());
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region 0x040 skippable chunk (items)

        /// <summary>
        /// CGameCtnChallenge 0x040 skippable chunk (items)
        /// </summary>
        [Chunk(0x03043040, "items")]
        public class Chunk03043040 : SkippableChunk<CGameCtnChallenge>, ILookbackable
        {
            private int version = 4;
            private int u01;
            private int u02 = 10;
            private int u03;

            int? ILookbackable.IdVersion { get; set; }
            List<string> ILookbackable.IdStrings { get; set; } = new List<string>();
            bool ILookbackable.IdWritten { get; set; }

            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version
            {
                get => version;
                set => version = value;
            }

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public int U02
            {
                get => u02;
                set => u02 = value;
            }

            public int U03
            {
                get => u03;
                set => u03 = value;
            }

            public override void OnLoad()
            {
                Node.anchoredObjects = new List<CGameCtnAnchoredObject>();
            }

            public override void Read(CGameCtnChallenge n, GameBoxReader r)
            {
                version = r.ReadInt32();

                if (version != 0)
                {
                    u01 = r.ReadInt32();
                    var size = r.ReadInt32();
                    u02 = r.ReadInt32(); // 10

                    n.anchoredObjects = ParseArray<CGameCtnAnchoredObject>(r).ToList();
                    u03 = r.ReadInt32(); // 0
                }
            }

            public override void Write(CGameCtnChallenge n, GameBoxWriter w)
            {
                w.Write(Version);

                if(version != 0)
                {
                    w.Write(u01);

                    using (var itemMs = new MemoryStream())
                    using (var wr = new GameBoxWriter(itemMs, this))
                    {
                        wr.Write(u02);
                        wr.WriteNodes(n.anchoredObjects);

                        wr.Write(u03);

                        w.Write((int)itemMs.Length);
                        w.Write(itemMs.ToArray(), 0, (int)itemMs.Length);
                    }
                }
            }
        }

        #endregion

        #region 0x042 skippable chunk (author)

        /// <summary>
        /// CGameCtnChallenge 0x042 skippable chunk (author)
        /// </summary>
        [Chunk(0x03043042, "author")]
        public class Chunk03043042 : SkippableChunk<CGameCtnChallenge>
        {
            private int version = 4;

            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
                rw.Int32(ref n.authorVersion);
                rw.String(ref n.authorLogin);
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
        [IgnoreChunk]
        public class Chunk03043043 : SkippableChunk<CGameCtnChallenge>, ILookbackable
        {
            int? ILookbackable.IdVersion { get; set; }
            List<string> ILookbackable.IdStrings { get; set; } = new List<string>();
            bool ILookbackable.IdWritten { get; set; }

            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version { get; set; }

            public new byte[] Data { get; set; }

            public override void Read(CGameCtnChallenge n, GameBoxReader r)
            {
                Version = r.ReadInt32();
                var sizeOfNodeWithClassID = r.ReadInt32();
                Data = r.ReadBytes(sizeOfNodeWithClassID);

                n.genealogies = Task.Run(() =>
                {
                    using (var ms = new MemoryStream(Data))
                    using (var r2 = new GameBoxReader(ms, this))
                        return ParseArray<CGameCtnZoneGenealogy>(r2);
                });
            }

            public override void Write(CGameCtnChallenge n, GameBoxWriter w)
            {
                w.Write(Version);

                using (var ms = new MemoryStream())
                using (var w1 = new GameBoxWriter(ms))
                {
                    w1.Write(n.genealogies.Result, (x, w2) =>
                    {
                        w2.Write(0x0311D000);
                        x.Write(w2);
                    });

                    w.Write((int)ms.Length);
                    w.Write(ms.ToArray());
                }
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
            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version { get; set; }

            public override void OnLoad()
            {
                Node.scriptMetadata = new CScriptTraitsMetadata();
            }

            public override void Read(CGameCtnChallenge n, GameBoxReader r)
            {
                Version = r.ReadInt32();
                var size = r.ReadInt32();

                n.scriptMetadata.Read(r);
            }

            public override void Write(CGameCtnChallenge n, GameBoxWriter w)
            {
                w.Write(Version);

                using (var ms = new MemoryStream())
                {
                    using (var wm = new GameBoxWriter(ms))
                        n.scriptMetadata.Write(wm);

                    w.Write((int)ms.Length);
                    w.Write(ms.ToArray(), 0, (int)ms.Length);
                }
            }
        }

        #endregion

        #region 0x048 skippable chunk (baked blocks)

        /// <summary>
        /// CGameCtnChallenge 0x048 skippable chunk (baked blocks)
        /// </summary>
        [Chunk(0x03043048, "baked blocks")]
        public class Chunk03043048 : SkippableChunk<CGameCtnChallenge>
        {
            private int version;
            private int u01;
            private int u02;
            private int u03;

            public int Version
            {
                get => version;
                set => version = value;
            }

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public int U02
            {
                get => u02;
                set => u02 = value;
            }

            public int U03
            {
                get => u03;
                set => u03 = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
                rw.Int32(ref u01);

                n.BakedBlocks = rw.Array(n.BakedBlocks, r => new CGameCtnBlock(
                    name: r.ReadId(),
                    direction: (Direction)r.ReadByte(),
                    coord: (Int3)r.ReadByte3(),
                    flags: r.ReadInt32()
                ),
                (x, w) =>
                {
                    w.WriteId(x.Name);
                    w.Write((byte)x.Direction);
                    w.Write((Byte3)x.Coord);
                    w.Write(x.Flags);
                });

                rw.Int32(ref u02);
                rw.Int32(ref u03);
            }
        }

        #endregion

        #region 0x049 chunk (mediatracker)

        /// <summary>
        /// CGameCtnChallenge 0x049 chunk (mediatracker)
        /// </summary>
        [Chunk(0x03043049, "mediatracker")]
        public class Chunk03043049 : Chunk<CGameCtnChallenge>
        {
            private int version = 2;
            private Int3 triggerSize = (3, 1, 3);

            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version
            {
                get => version;
                set => version = value;
            }

            /// <summary>
            /// Trigger size per coord.
            /// </summary>
            public Int3 TriggerSize
            {
                get => triggerSize;
                set => triggerSize = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
                rw.NodeRef<CGameCtnMediaClip>(ref n.clipIntro);
                rw.NodeRef<CGameCtnMediaClip>(ref n.clipPodium);
                rw.NodeRef<CGameCtnMediaClipGroup>(ref n.clipGroupInGame);
                rw.NodeRef<CGameCtnMediaClipGroup>(ref n.clipGroupEndRace);

                if (version >= 2)
                {
                    rw.NodeRef<CGameCtnMediaClip>(ref n.clipAmbiance);
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

        #region 0x050 skippable chunk (offzones)

        /// <summary>
        /// CGameCtnChallenge 0x050 skippable chunk (offzones)
        /// </summary>
        [Chunk(0x03043050, "offzones")]
        public class Chunk03043050 : SkippableChunk<CGameCtnChallenge>
        {
            private int version;
            private Vec3 triggerSize = (3, 1, 3);

            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version
            {
                get => version;
                set => version = value;
            }

            public Vec3 TriggerSize
            {
                get => triggerSize;
                set => triggerSize = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
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
        public class Chunk03043051 : SkippableChunk<CGameCtnChallenge>
        {
            private int version;

            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);

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
        public class Chunk03043052 : SkippableChunk<CGameCtnChallenge>
        {
            private int version;

            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
                rw.Int32(ref n.decoBaseHeightOffset);
            }
        }

        #endregion

        #region 0x053 skippable chunk (bot paths)

        /// <summary>
        /// CGameCtnChallenge 0x053 skippable chunk (bot paths)
        /// </summary>
        [Chunk(0x03043053, "bot paths")]
        public class Chunk03043053 : SkippableChunk<CGameCtnChallenge>
        {
            private int version;

            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);

                rw.List(ref n.botPaths, r => new BotPath()
                {
                    Clan = r.ReadInt32(),
                    Path = r.ReadArray(r1 => r1.ReadVec3()).ToList(),
                    IsFlying = r.ReadBoolean(),
                    WaypointSpecialProperty = r.ReadNodeRef<CGameWaypointSpecialProperty>(),
                    IsAutonomous = r.ReadBoolean()
                },
                (x, w) =>
                {
                    w.Write(x.Clan);
                    w.Write(x.Path, (y, w1) => w1.Write(y));
                    w.Write(x.IsFlying);
                    w.Write(x.WaypointSpecialProperty);
                    w.Write(x.IsAutonomous);
                });
            }
        }

        #endregion

        #region 0x054 skippable chunk (embedded objects)

        /// <summary>
        /// CGameCtnChallenge 0x054 skippable chunk (embedded objects)
        /// </summary>
        [Chunk(0x03043054, "embedded objects")]
        public class Chunk03043054 : SkippableChunk<CGameCtnChallenge>, ILookbackable
        {
            int? ILookbackable.IdVersion { get; set; }
            List<string> ILookbackable.IdStrings { get; set; } = new List<string>();
            bool ILookbackable.IdWritten { get; set; }

            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version { get; set; } = 1;
            public int U01 { get; set; }
            public string[] Textures { get; set; }

            public override void Read(CGameCtnChallenge n, GameBoxReader r)
            {
                Version = r.ReadInt32();
                U01 = r.ReadInt32();
                var size = r.ReadInt32();

                var embedded = r.ReadArray(r1 => r1.ReadIdent());

                n.originalEmbedZip = r.ReadBytes();
                if (n.originalEmbedZip.Length > 0)
                {
                    using (var ms = new MemoryStream(n.originalEmbedZip))
                    using (var zip = new ZipArchive(ms))
                    {
                        foreach (ZipArchiveEntry entry in zip.Entries)
                        {
                            using (var entryStream = entry.Open())
                            using (var entryDataStream = new MemoryStream())
                            {
                                entryStream.CopyTo(entryDataStream);
                                n.embeddedObjects[entry.Name] = entryDataStream.ToArray();
                            }
                        }
                    }
                }

                Textures = r.ReadArray(r1 => r1.ReadString());
            }

            public override void Write(CGameCtnChallenge n, GameBoxWriter w)
            {
                w.Write(Version);
                w.Write(U01);

                using (var ms = new MemoryStream())
                using (var writer = new GameBoxWriter(ms, this))
                {
                    List<Ident> embedded = new List<Ident>();

                    foreach (var embed in n.GetEmbeddedObjects())
                    {
                        if (embed is GameBox<CGameItemModel> gbxItem)
                        {
                            var id = gbxItem.FileName;
                            var dirs = id.Split('/', '\\');
                            for (var i = 0; i < dirs.Length; i++)
                            {
                                var dir = dirs[dirs.Length - 1 - i];
                                if (dir == "Items"
                                ||  dir == "Blocks")
                                {
                                    id = string.Join("\\", dirs, dirs.Length - i, i);
                                    break;
                                }
                            }

                            var item = gbxItem.Node;

                            embedded.Add(new Ident(id,
                                item.Ident.Collection,
                                item.Ident.Author));
                        }
                    }

                    writer.Write(embedded.ToArray(), (x, w1) => w1.Write(x));

                    using (var zipStream = new MemoryStream())
                    {
                        n.ExtractEmbedZip(zipStream);
                        writer.Write((int)zipStream.Length);
                        writer.Write(zipStream.ToArray(), 0, (int)zipStream.Length);
                    }

                    writer.Write(Textures, (x, w1) => w1.Write(x));

                    w.Write((int)ms.Length);
                    w.Write(ms.ToArray());
                }
            }
        }

        #endregion

        #region 0x056 skippable chunk (light settings)

        /// <summary>
        /// CGameCtnChallenge 0x056 skippable chunk (light settings)
        /// </summary>
        [Chunk(0x03043056, "light settings")]
        public class Chunk03043056 : SkippableChunk<CGameCtnChallenge>
        {
            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version { get; set; } = 3;

            public int U01 { get; set; }
            public int U02 { get; set; }

            public override void Read(CGameCtnChallenge n, GameBoxReader r)
            {
                Version = r.ReadInt32();
                U01 = r.ReadInt32();

                var dayTime = r.ReadInt32();
                if (dayTime != -1)
                    n.dayTime = TimeSpan.FromSeconds(dayTime / (double)ushort.MaxValue * new TimeSpan(23, 59, 59).TotalSeconds);

                U02 = r.ReadInt32();
                n.dynamicDaylight = r.ReadBoolean();
                n.dayDuration = r.ReadTimeSpan();
            }

            public override void Write(CGameCtnChallenge n, GameBoxWriter w)
            {
                w.Write(Version);
                w.Write(U01);

                if (n.dayTime.HasValue)
                    w.Write(Convert.ToInt32(n.dayTime.Value.TotalSeconds / new TimeSpan(23, 59, 59).TotalSeconds * ushort.MaxValue));
                else
                    w.Write(-1);

                w.Write(U02);
                w.Write(n.dynamicDaylight);
                w.Write(n.dayDuration);
            }
        }

        #endregion

        #region 0x059 skippable chunk

        /// <summary>
        /// CGameCtnChallenge 0x059 skippable chunk
        /// </summary>
        [Chunk(0x03043059)]
        public class Chunk03043059 : SkippableChunk<CGameCtnChallenge>
        {
            private int version;
            private Vec3 u01;
            private bool u02;
            private float u03;
            private float u04;

            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version
            {
                get => version;
                set => version = value;
            }

            public Vec3 U01
            {
                get => u01;
                set => u01 = value;
            }

            public bool U02
            {
                get => u02;
                set => u02 = value;
            }

            public float U03
            {
                get => u03;
                set => u03 = value;
            }

            public float U04
            {
                get => u04;
                set => u04 = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version); // 3

                rw.Vec3(ref u01);

                if (version != 0)
                {
                    rw.Boolean(ref u02);

                    if (version >= 3)
                    {
                        rw.Single(ref u03);
                        rw.Single(ref u04);
                    }
                }
            }
        }

        #endregion

        #region 0x05A skippable chunk

        /// <summary>
        /// CGameCtnChallenge 0x05A skippable chunk [TM®️]
        /// </summary>
        [Chunk(0x0304305A)]
        public class Chunk0304305A : SkippableChunk<CGameCtnChallenge>
        {
            private int u01;
            private int u02;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public int U02
            {
                get => u02;
                set => u02 = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref u01);
                rw.Int32(ref u02);
            }
        }

        #endregion

        #region 0x05F skippable chunk (free blocks) [TM®️]

        /// <summary>
        /// CGameCtnChallenge 0x05F skippable chunk (free blocks) [TM®️]
        /// </summary>
        [Chunk(0x0304305F, "free blocks")]
        public class Chunk0304305F : SkippableChunk<CGameCtnChallenge>
        {
            private int version;

            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);

                //var gsdgs = rw.Reader.ReadArray<float>(15);

                foreach (var block in n.Blocks)
                {
                    if (block.IsFree)
                    {
                        block.AbsolutePositionInMap = rw.Vec3(block.AbsolutePositionInMap);
                        block.PitchYawRoll = rw.Vec3(block.PitchYawRoll);

                        if (BlockInfoManager.BlockModels != null)
                        {
                            if (BlockInfoManager.BlockModels.TryGetValue(block.Name, out BlockModel model))
                            {
                                foreach (var unit in model.Air)
                                {
                                    if (unit.Clips != null)
                                    {
                                        for (var i = 0; i < unit.Clips.Length; i++)
                                        {
                                            if (!string.IsNullOrEmpty(unit.Clips[i]))
                                            {
                                                if (rw.Mode == GameBoxReaderWriterMode.Read)
                                                {
                                                    rw.Reader.ReadVec3();
                                                    rw.Reader.ReadVec3();
                                                }
                                                else if (rw.Mode == GameBoxReaderWriterMode.Write)
                                                {
                                                    var dir = (Direction)i;
                                                    rw.Writer.Write(new Vec3());
                                                    rw.Writer.Write(new Vec3());
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

        #endregion

        #region Header interface

        public interface IHeader : INodeHeader
        {
            /// <summary>
            /// Time of the bronze medal.
            /// </summary>
            TimeSpan? TMObjective_BronzeTime { get; set; }

            /// <summary>
            /// Time of the silver medal.
            /// </summary>
            TimeSpan? TMObjective_SilverTime { get; set; }

            /// <summary>
            /// Time of the gold medal.
            /// </summary>
            TimeSpan? TMObjective_GoldTime { get; set; }

            /// <summary>
            /// Time of the author medal.
            /// </summary>
            TimeSpan? TMObjective_AuthorTime { get; set; }

            /// <summary>
            /// Display cost of the track (or copper cost) explaining the performance of the map.
            /// </summary>
            int Cost { get; set; }

            /// <summary>
            /// Usually author time or stunts score.
            /// </summary>
            int AuthorScore { get; set; }

            /// <summary>
            /// In which editor settings the map was made.
            /// </summary>
            EditorMode Editor { get; set; }

            /// <summary>
            /// If the map was made using the simple editor.
            /// </summary>
            bool CreatedWithSimpleEditor { get; }

            /// <summary>
            /// If the map uses ghost blocks.
            /// </summary>
            bool HasGhostBlocks { get; }

            /// <summary>
            /// Map type in which the track was validated in.
            /// </summary>
            PlayMode? Mode { get; set; }

            /// <summary>
            /// If the map is a multilap.
            /// </summary>
            bool TMObjective_IsLapRace { get; set; }

            /// <summary>
            /// Number of laps.
            /// </summary>
            int TMObjective_NbLaps { get; set; }

            /// <summary>
            /// Number of checkpoints.
            /// </summary>
            int? NbCheckpoints { get; set; }

            /// <summary>
            /// Map UID, environment, and author login.
            /// </summary>
            Ident MapInfo { get; set; }

            /// <summary>
            /// The map's environment.
            /// </summary>
            Collection Collection { get; set; }

            /// <summary>
            /// The map's UID.
            /// </summary>
            string MapUid { get; set; }

            /// <summary>
            /// Login of the map author.
            /// </summary>
            string AuthorLogin { get; set; }

            /// <summary>
            /// Nickname of the map author.
            /// </summary>
            string AuthorNickname { get; set; }

            /// <summary>
            /// Zone of the map author.
            /// </summary>
            string AuthorZone { get; set; }

            string AuthorExtraInfo { get; set; }

            /// <summary>
            /// The map's name.
            /// </summary>
            string MapName { get; set; }

            /// <summary>
            /// The map's intended use.
            /// </summary>
            MapKind Kind { get; set; }

            /// <summary>
            /// Password of the map used by older maps.
            /// </summary>
            string Password { get; set; }

            /// <summary>
            /// The map's decoration (time of the day or scenery)
            /// </summary>
            Ident Decoration { get; set; }

            /// <summary>
            /// Target of the map.
            /// </summary>
            Vec2? MapCoordTarget { get; set; }

            /// <summary>
            /// Origin of the map.
            /// </summary>
            Vec2? MapCoordOrigin { get; set; }

            /// <summary>
            /// Name of the map type script.
            /// </summary>
            string MapType { get; set; }

            /// <summary>
            /// Style of the map (Fullspeed, LOL, Tech), usually unused and defined by user.
            /// </summary>
            string MapStyle { get; set; }

            /// <summary>
            /// UID of the lightmap data stored in cache.
            /// </summary>
            ulong? LightmapCacheUID { get; set; }

            /// <summary>
            /// Version of the lightmap calculation.
            /// </summary>
            byte? LightmapVersion { get; set; }

            /// <summary>
            /// Title pack the map was built in.
            /// </summary>
            string TitleID { get; set; }

            /// <summary>
            /// XML track information and dependencies.
            /// </summary>
            string XML { get; set; }

            /// <summary>
            /// The map's author comments.
            /// </summary>
            string Comments { get; set; }

            /// <summary>
            /// Thumbnail JPEG data.
            /// </summary>
            byte[] Thumbnail { get; set; }
        }

        #endregion

        #region Other classes

        public class BotPath
        {
            public int Clan { get; set; }
            public List<Vec3> Path { get; set; }
            public bool IsFlying { get; set; }
            public CGameWaypointSpecialProperty WaypointSpecialProperty { get; set; }
            public bool IsAutonomous { get; set; }
        }

        #endregion
    }
}