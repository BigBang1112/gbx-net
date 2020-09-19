using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Hms;
using GBX.NET.Engines.Script;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// Map (0x03043000)
    /// </summary>
    /// <remarks>A map. Known extensions: .Challenge.Gbx, .Map.Gbx</remarks>
    [Node(0x03043000)]
    public class CGameCtnChallenge : Node
    {
        #region Enums

        /// <summary>
        /// Map type in which the track was validated in.
        /// </summary>
        public enum TrackType : int
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
        /// The track's intended use.
        /// </summary>
        public enum TrackKind : int
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

        public enum PlayMode : int
        {
            Race,
            Platform,
            Puzzle,
            Crazy,
            Shortcut,
            Stunts
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

        private bool? isLapRace;
        private int? nbLaps;
        private string password;
        private string titleID;
        private int? authorVersion;
        private string authorNickname;
        private string authorZone;
        private string authorExtraInfo;
        private Int3[] checkpoints;
        private FileRef modPackDesc;
        private PlayMode? mode;
        private byte[] hashedPassword;
        private uint? crc32;
        private Vec3? thumbnailPosition;
        private Vec3? thumbnailPitchYawRoll;
        private float? thumbnailFOV;
        private List<CGameCtnAnchoredObject> items;
        private CScriptTraitsMetadata metadataTraits;
        private Task<CHmsLightMapCache> lightmapCache;
        private Task<CGameCtnZoneGenealogy[]> genealogies;
        private string objectiveTextAuthor;
        private string objectiveTextGold;
        private string objectiveTextSilver;
        private string objectiveTextBronze;
        private string buildVersion;

        #endregion

        #region Properties

        public bool? TMObjective_IsLapRace
        {
            get
            {
                DiscoverChunk<Chunk03043018>();
                return isLapRace;
            }
            set => isLapRace = value;
        }

        /// <summary>
        /// Number of laps.
        /// </summary>
        public int? TMObjective_NbLaps
        {
            get
            {
                DiscoverChunk<Chunk03043018>();
                return nbLaps;
            }
            set => nbLaps = value;
        }

        public Meta MapInfo { get; set; }

        public string MapUid => MapInfo?.ID;

        public string AuthorLogin { get; set; }

        public string MapName { get; set; }

        /// <summary>
        /// The track's intended use.
        /// </summary>
        public TrackKind? Kind { get; set; }

        /// <summary>
        /// Password of the map used by older tracks.
        /// </summary>
        public string Password
        {
            get
            {
                DiscoverChunk<Chunk03043014>();
                return password;
            }
            set => password = value;
        }

        public Meta Decoration { get; set; }

        public string Collection
        {
            get => Decoration?.Collection ?? MapInfo?.Collection;
            set
            {
                if (Decoration != null)
                    Decoration.Collection = value;
                if (MapInfo != null)
                    MapInfo.Collection = value;
            }
        }

        /// <summary>
        /// Origin of the map.
        /// </summary>
        public Vec2? MapOrigin { get; set; }

        /// <summary>
        /// Target of the map.
        /// </summary>
        public Vec2? MapTarget { get; set; }

        /// <summary>
        /// Title pack the map was built in.
        /// </summary>
        public string TitleID
        {
            get
            {
                DiscoverChunk<Chunk03043051>();
                return titleID;
            }
            set => titleID = value;
        }

        public string BuildVersion
        {
            get
            {
                DiscoverChunk<Chunk03043051>();
                return buildVersion;
            }
            set => buildVersion = value;
        }

        public string Comments { get; set; }

        public int? AuthorVersion
        {
            get
            {
                DiscoverChunk<Chunk03043042>();
                return authorVersion;
            }
            set => authorVersion = value;
        }

        public string AuthorNickname
        {
            get
            {
                DiscoverChunk<Chunk03043042>();
                return authorNickname;
            }
            set => authorNickname = value;
        }

        public string AuthorZone
        {
            get
            {
                DiscoverChunk<Chunk03043042>();
                return authorZone;
            }
            set => authorZone = value;
        }

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
        /// Vehicle metadata info.
        /// </summary>
        public Meta Vehicle { get; set; }

        /// <summary>
        /// Map parameters.
        /// </summary>
        public CGameCtnChallengeParameters ChallengeParameters { get; set; }

        /// <summary>
        /// List of puzzle pieces.
        /// </summary>
        public CGameCtnCollectorList CollectorList { get; set; }

        /// <summary>
        /// All checkpoints and their map coordinates. Used by older Trackmania.
        /// </summary>
        public Int3[] Checkpoints
        {
            get
            {
                DiscoverChunk<Chunk03043017>();
                return checkpoints;
            }
            set => checkpoints = value;
        }

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

        public PlayMode? Mode
        {
            get
            {
                DiscoverChunk<Chunk0304301C>();
                return mode;
            }
            set => mode = value;
        }

        public Int3? Size { get; set; }

        public bool? NeedUnlock { get; set; }

        public CGameCtnBlock[] OldBlocks { get; set; }

        /// <summary>
        /// Array of all blocks on the map.
        /// </summary>
        public List<Block> Blocks { get; set; }

        public int NbBlocks
        {
            get => Blocks.Where(x => x.Flags != -1).Count();
        }

        public ReadOnlyCollection<FreeBlock> FreeBlocks => GetChunk<Chunk0304305F>()?.FreeBlocks;

        public CGameCtnMediaClip ClipIntro { get; set; }

        public CGameCtnMediaClipGroup ClipGroupInGame { get; set; }

        public CGameCtnMediaClipGroup ClipGroupEndRace { get; set; }

        public CGameCtnMediaClip ClipAmbiance { get; set; }

        public FileRef CustomMusicPackDesc { get; set; }

        public byte[] HashedPassword
        {
            get
            {
                DiscoverChunk<Chunk03043029>();
                return hashedPassword;
            }
            set => hashedPassword = value;
        }

        public uint? CRC32
        {
            get
            {
                DiscoverChunk<Chunk03043029>();
                return crc32;
            }
            set => crc32 = value;
        }

        /// <summary>
        /// Position of the thumnail camera.
        /// </summary>
        public Vec3? ThumbnailPosition
        {
            get
            {
                DiscoverChunk<Chunk03043036>();
                return thumbnailPosition;
            }
            set => thumbnailPosition = value;
        }

        /// <summary>
        /// Pitch, yaw and roll of the thumbnail camera in radians.
        /// </summary>
        public Vec3? ThumbnailPitchYawRoll
        {
            get
            {
                DiscoverChunk<Chunk03043036>();
                return thumbnailPitchYawRoll;
            }
            set => thumbnailPitchYawRoll = value;
        }

        /// <summary>
        /// Thumbnail camera FOV.
        /// </summary>
        public float? ThumbnailFOV
        {
            get
            {
                DiscoverChunk<Chunk03043036>();
                return thumbnailFOV;
            }
            set => thumbnailFOV = value;
        }

        public Task<CHmsLightMapCache> LightmapCache
        {
            get
            {
                DiscoverChunk<Chunk0304303D>();
                return lightmapCache;
            }
            set => lightmapCache = value;
        }

        public List<CGameCtnAnchoredObject> Items
        {
            get
            {
                DiscoverChunk<Chunk03043040>();
                return items;
            }
            set => items = value;
        }

        public Task<CGameCtnZoneGenealogy[]> Genealogies
        {
            get
            {
                DiscoverChunk<Chunk03043043>();
                return genealogies;
            }
            set => genealogies = value;
        }

        public CScriptTraitsMetadata MetadataTraits
        {
            get
            {
                DiscoverChunk<Chunk03043044>();
                return metadataTraits;
            }
            set => metadataTraits = value;
        }

        public string ObjectiveTextAuthor
        {
            get
            {
                DiscoverChunk<Chunk0304304B>();
                return objectiveTextAuthor;
            }
            set => objectiveTextAuthor = value;
        }

        public string ObjectiveTextGold
        {
            get
            {
                DiscoverChunk<Chunk0304304B>();
                return objectiveTextGold;
            }
            set => objectiveTextGold = value;
        }

        public string ObjectiveTextSilver
        {
            get
            {
                DiscoverChunk<Chunk0304304B>();
                return objectiveTextSilver;
            }
            set => objectiveTextSilver = value;
        }

        public string ObjectiveTextBronze
        {
            get
            {
                DiscoverChunk<Chunk0304304B>();
                return objectiveTextBronze;
            }
            set => objectiveTextBronze = value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets a new map password.
        /// </summary>
        /// <param name="password">Password that will be hashed.</param>
        public void NewPassword(string password)
        {
            var md5 = MD5.Create();
            HashedPassword = md5.ComputeHash(Encoding.UTF8.GetBytes(password));

            Crc32 crc32 = new Crc32();
            crc32.Update(Encoding.ASCII.GetBytes("0x" + BitConverter.ToInt16(HashedPassword, 0).ToString() + "???" + MapUid));
            CRC32 = Convert.ToUInt32(crc32.Value);
        }

        /// <summary>
        /// Cracks the map password.
        /// </summary>
        public void CrackPassword()
        {
            RemoveChunk<Chunk03043029>();
        }

        public void PlaceItem(Meta itemModel, Vec3 absolutePosition, Vec3 pitchYawRoll, Byte3 blockUnitCoord, Vec3 offsetPivot, int variant = 0)
        {
            var chunkItems = CreateChunk<Chunk03043040>();

            var it = new CGameCtnAnchoredObject()
            {
                ItemModel = itemModel,
                AbsolutePositionInMap = absolutePosition,
                PitchYawRoll = pitchYawRoll,
                BlockUnitCoord = blockUnitCoord,
                PivotPosition = offsetPivot,
                Variant = variant
            };
            it.Chunks = new ChunkSet();
            it.CreateChunk<CGameCtnAnchoredObject.Chunk03101002>();
            it.CreateChunk<CGameCtnAnchoredObject.Chunk03101004>();
            Items.Add(it);
        }

        public FreeBlock PlaceFreeBlock(string name, Vec3 position, Vec3 pitchYawRoll)
        {
            var block = new Block(name, Direction.North, (0, 0, 0), 0x20000000, null, null, null);
            var freeBlock = new FreeBlock(block)
            {
                Position = position,
                PitchYawRoll = pitchYawRoll
            };

            Blocks.Add(block);

            var freeBlockChunk = CreateChunk<Chunk0304305F>();
            freeBlockChunk.Vectors.Add(position);
            freeBlockChunk.Vectors.Add(pitchYawRoll);

            var freeBlockList = Resources.FreeBlock.Split('\n');
            var freeBlockSnapCount = freeBlockList.FirstOrDefault(x => x.StartsWith(name + ":"));

            if (freeBlockSnapCount != null)
            {
                if (int.TryParse(freeBlockSnapCount.Split(':')[1], out int v))
                {
                    if (v > 0)
                    {
                        throw new NotImplementedException("Cannot place a free block with snaps.");
                    }
                }
                else
                    throw new Exception("Wrong amount of snaps format.");
            }
            else
                throw new Exception("Cannot place a free block with an unknown amount of snaps.");

            return freeBlock;
        }

        /// <summary>
        /// Transfers the MediaTracker from <see cref="Chunk021"/> (up to TMUF) to <see cref="Chunk049"/> (ManiaPlanet and Trackmania®). If <see cref="Chunk049"/> is already presented, no action is performed.
        /// </summary>
        /// <param name="upsaleTriggerCoord">Defines how many times the same coord should repeat.</param>
        /// <returns>Returns <see cref="true"/> if any action was performed, otherwise <see cref="false"/>.</returns>
        public bool TransferMediaTrackerTo049(int upsaleTriggerCoord = 3)
        {
            var chunk021 = GetChunk<Chunk03043021>();
            var chunk049 = CreateChunk<Chunk03043049>();

            if (chunk021 == null) return false;

            if (ClipIntro != null)
                ConvertMediaClip(ClipIntro);

            if (ClipGroupInGame != null)
                ConvertMediaClipGroup(ClipGroupInGame);

            if (ClipGroupEndRace != null)
                ConvertMediaClipGroup(ClipGroupEndRace);

            RemoveChunk<Chunk03043021>();

            void ConvertMediaClip(CGameCtnMediaClip node)
            {
                foreach (var track in node.Tracks)
                    if (track != null)
                        ConvertMediaTrack(track);
            }

            void ConvertMediaClipGroup(CGameCtnMediaClipGroup node)
            {
                foreach(var trigger in node.Triggers)
                {
                    var coords = trigger.Coords.ToList();

                    for (var i = 0; i < trigger.Coords.Length; i++)
                    {
                        coords[i] = coords[i] * (upsaleTriggerCoord, 1, upsaleTriggerCoord);

                        for(var x = 0; x < upsaleTriggerCoord; x++)
                        {
                            for (var z = 0; z < upsaleTriggerCoord; z++)
                            {
                                coords.Add(coords[i] + new Int3(x, 0, z));
                            }
                        }
                    }

                    trigger.Coords = coords.ToArray();
                }

                foreach(var clip in node.Clips)
                {
                    ConvertMediaClip(clip);
                }
            }

            void ConvertMediaTrack(CGameCtnMediaTrack node)
            {
                var chunk001 = node.GetChunk<CGameCtnMediaTrack.Chunk03078001>();

                // Chunk 0x004 has to be removed so that ManiaPlanet accepts the entire map.
                node.RemoveChunk<CGameCtnMediaTrack.Chunk03078004>();

                node.Blocks.RemoveAll(x => x is CGameCtnMediaBlockGhost); // Some ghosts can crash the game
            }

            return true;
        }

        public void OffsetMediaTrackerCameras(Vec3 offset)
        {
            OffsetCamerasInClip(ClipIntro);
            OffsetCamerasInClipGroup(ClipGroupInGame);
            OffsetCamerasInClipGroup(ClipGroupEndRace);
            OffsetCamerasInClip(ClipAmbiance);

            if (TryGetChunk(out Chunk03043049 c049))
                OffsetCamerasInClip(c049.ClipPodium);

            void OffsetCamerasInClipGroup(CGameCtnMediaClipGroup group)
            {
                if (group == null) return;

                foreach (var clip in group.Clips)
                    OffsetCamerasInClip(clip);
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

        public void OffsetMediaTrackerTriggers(Int3 offset)
        {
            OffsetTriggers(ClipGroupInGame);
            OffsetTriggers(ClipGroupEndRace);

            void OffsetTriggers(CGameCtnMediaClipGroup group)
            {
                if (group == null) return;

                foreach(var trigger in group.Triggers)
                {
                    trigger.Coords = trigger.Coords.Select(x => x + offset).ToArray();
                }
            }
        }

        #endregion

        #region Chunks

        #region 0x001 chunk (Virtual Skipper)

        /// <summary>
        /// CGameCtnChallenge 0x001 chunk (Virtual Skipper)
        /// </summary>
        [Chunk(0x03043001, "Virtual Skipper")]
        public class Chunk03043001 : HeaderChunk<CGameCtnChallenge>
        {
            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public byte Version { get; set; }

            public Meta MapInfo { get; set; }

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

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Byte(Version);

                if (Version < 1)
                {
                    MapInfo = rw.Meta(MapInfo);
                    MapName = rw.String(MapName);
                }

                rw.Boolean(Unknown);
                rw.Int32(Unknown);

                if (Version < 1)
                    rw.Byte(Unknown);

                rw.Byte(Unknown);

                if (Version < 9)
                    BoatName = (BoatName)rw.Byte((byte)BoatName);

                if (Version >= 9)
                    Boat = rw.LookbackString(Boat);

                if (Version >= 12)
                    BoatAuthor = rw.LookbackString(BoatAuthor);

                RaceMode = (RaceMode)rw.Byte((byte)RaceMode);
                rw.Byte(Unknown);
                WindDirection = (WindDirection)rw.Byte((byte)RaceMode);
                WindStrength = rw.Byte(WindStrength);
                Weather = (Weather)rw.Byte((byte)Weather);
                rw.Byte(Unknown);
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
                                rw.Byte(Unknown);

                                if (Version == 6 || Version == 7)
                                {
                                    rw.Boolean(Unknown);
                                    rw.String(Unknown);
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

        #region 0x002 chunk (map info)

        /// <summary>
        /// CGameCtnChallenge 0x002 chunk (map info)
        /// </summary>
        [Chunk(0x03043002, "map info")]
        public class Chunk03043002 : HeaderChunk<CGameCtnChallenge>
        {
            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public byte Version { get; set; }

            /// <summary>
            /// Basic map information. <see cref="Meta.ID"/> is map UID, <see cref="Meta.Author"/> is the map author login. Can be <see cref="null"/> if <c><see cref="Version"/> &gt; <see cref="2"/></c>.
            /// </summary>
            public Meta MapInfo { get; set; }

            /// <summary>
            /// Formatted name of the map. Can be empty if <c><see cref="Version"/> &gt; <see cref="2"/></c>.
            /// </summary>
            public string MapName { get; set; }

            /// <summary>
            /// Time of the bronze medal. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="1"/></c>.
            /// </summary>
            public TimeSpan? BronzeTime { get; set; }

            /// <summary>
            /// Time of the silver medal. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="1"/></c>.
            /// </summary>
            public TimeSpan? SilverTime { get; set; }

            /// <summary>
            /// Time of the gold medal. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="1"/></c>.
            /// </summary>
            public TimeSpan? GoldTime { get; set; }

            /// <summary>
            /// Time of the author medal. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="1"/></c>.
            /// </summary>
            public TimeSpan? AuthorTime { get; set; }

            /// <summary>
            /// Display cost (or coppers) of the track. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="4"/></c>.
            /// </summary>
            public int? Cost { get; set; }

            /// <summary>
            /// If the track has multiple laps. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="5"/></c>.
            /// </summary>
            public bool? IsMultilap { get; set; }

            /// <summary>
            /// Map type in which the track was validated in. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="7"/></c>.
            /// </summary>
            public TrackType? Type { get; set; }

            /// <summary>
            /// Usually author time or stunt score. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="10"/></c>.
            /// </summary>
            public int? AuthorScore { get; set; }

            public int? EditorMode { get; set; }

            /// <summary>
            /// If the track was made using the simple editor. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="11"/></c>.
            /// </summary>
            public bool? CreatedWithSimpleEditor
            {
                get => (EditorMode & (1 << 0)) != 0;
                set
                {
                    if (value.GetValueOrDefault()) EditorMode |= 1 << 0;
                    else EditorMode &= ~(1 << 0);
                }
            }

            /// <summary>
            /// If the track uses ghost blocks. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="11"/></c>.
            /// </summary>
            public bool? HasGhostBlocks
            {
                get => (EditorMode & (1 << 1)) != 0;
                set
                {
                    if (value.GetValueOrDefault()) EditorMode |= 1 << 1;
                    else EditorMode &= ~(1 << 1);
                }
            }

            /// <summary>
            /// Number of checkpoints on the map. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="13"/></c>.
            /// </summary>
            public int? NbCheckpoints { get; set; }

            /// <summary>
            /// Number of laps. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="13"/></c>.
            /// </summary>
            public int? NbLaps { get; set; }

            public bool Unknown1 { get; set; }
            public byte Unknown2 { get; set; }
            public int Unknown3 { get; set; }
            public int Unknown4 { get; set; }
            public int Unknown5 { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Byte(Version);

                if (Version < 3)
                {
                    MapInfo = rw.Meta(MapInfo);
                    MapName = rw.String(MapName);
                }

                Unknown1 = rw.Boolean(Unknown1);

                if (Version >= 1)
                {
                    BronzeTime = rw.TimeSpan32(BronzeTime);
                    SilverTime = rw.TimeSpan32(SilverTime);
                    GoldTime = rw.TimeSpan32(GoldTime);
                    AuthorTime = rw.TimeSpan32(AuthorTime);

                    if (Version == 2)
                        Unknown2 = rw.Byte(Unknown2);

                    if (Version >= 4)
                    {
                        Cost = rw.Int32(Cost.GetValueOrDefault());

                        if (Version >= 5)
                        {
                            IsMultilap = rw.Boolean(IsMultilap.GetValueOrDefault());

                            if (Version == 6)
                                Unknown3 = rw.Int32(Unknown3);

                            if (Version >= 7)
                            {
                                Type = (TrackType)rw.Int32((int)Type.GetValueOrDefault());

                                if (Version >= 9)
                                {
                                    Unknown4 = rw.Int32(Unknown4);

                                    if (Version >= 10)
                                    {
                                        AuthorScore = rw.Int32(AuthorScore.GetValueOrDefault());

                                        if (Version >= 11)
                                        {
                                            EditorMode = rw.Int32(EditorMode.GetValueOrDefault());

                                            if (Version >= 12)
                                            {
                                                Unknown5 = rw.Int32(Unknown5);

                                                if (Version >= 13)
                                                {
                                                    NbCheckpoints = rw.Int32(NbCheckpoints.GetValueOrDefault());
                                                    NbLaps = rw.Int32(NbLaps.GetValueOrDefault());
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

        #region 0x003 chunk (common)

        /// <summary>
        /// CGameCtnChallenge 0x003 chunk (common)
        /// </summary>
        [Chunk(0x03043003, "common")]
        public class Chunk03043003 : HeaderChunk<CGameCtnChallenge>
        {
            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public byte Version { get; set; }

            /// <summary>
            /// Basic map information. <see cref="Meta.ID"/> is map UID, <see cref="Meta.Author"/> is the map author login.
            /// </summary>
            public Meta MapInfo { get; set; }

            /// <summary>
            /// Formatted name of the map.
            /// </summary>
            public string MapName { get; set; }

            /// <summary>
            /// The track's intended use.
            /// </summary>
            public TrackKind Kind { get; set; }

            /// <summary>
            /// If the track is locked (used by Virtual Skipper to lock the map parameters). Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="1"/></c>.
            /// </summary>
            public bool? Locked { get; set; }

            /// <summary>
            /// Password of the map used by older tracks.
            /// </summary>
            public string Password { get; set; }

            /// <summary>
            /// Information about the used map decoration. <see cref="Meta.ID"/> is the map base ID, <see cref="Meta.Author"/> is the author of the map base.
            /// </summary>
            public Meta Decoration { get; set; }

            /// <summary>
            /// Origin of the map. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="3"/></c>.
            /// </summary>
            public Vec2? MapOrigin { get; set; }

            /// <summary>
            /// Target of the map. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="4"/></c>.
            /// </summary>
            public Vec2? MapTarget { get; set; }

            /// <summary>
            /// Name of the map type script.
            /// </summary>
            public string MapType { get; set; }

            /// <summary>
            /// Style of the map (Fullspeed, LOL, Tech), usually unused and defined by user.
            /// </summary>
            public string MapStyle { get; set; }

            /// <summary>
            /// Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="8"/></c>.
            /// </summary>
            public ulong? LightmapCacheUID { get; set; }

            /// <summary>
            /// Version of the lightmap calculation. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="9"/></c>.
            /// </summary>
            public byte? LightmapVersion { get; set; }

            /// <summary>
            /// Title pack the map was built in. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="11"/></c>.
            /// </summary>
            public string TitleUID { get; set; }

            public int Unknown1 { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Byte(Version);
                MapInfo = rw.Meta(MapInfo);
                MapName = rw.String(MapName);
                Kind = (TrackKind)rw.Byte((byte)Kind);

                if (Version >= 1)
                {
                    Locked = rw.UInt32(Convert.ToUInt32(Locked.GetValueOrDefault())) == 1;
                    Password = rw.String(Password);

                    if (Version >= 2)
                    {
                        Decoration = rw.Meta(Decoration);

                        if (Version >= 3)
                        {
                            MapOrigin = rw.Vec2(MapOrigin.GetValueOrDefault());

                            if (Version >= 4)
                            {
                                MapTarget = rw.Vec2(MapTarget.GetValueOrDefault());

                                if (Version >= 5)
                                {
                                    rw.Bytes(Unknown, 16);

                                    if (Version >= 6)
                                    {
                                        MapType = rw.String(MapType);
                                        MapStyle = rw.String(MapStyle);

                                        if (Version <= 8)
                                            Unknown1 = rw.Int32(Unknown1);

                                        if (Version >= 8)
                                        {
                                            LightmapCacheUID = rw.UInt64(LightmapCacheUID.GetValueOrDefault());

                                            if (Version >= 9)
                                            {
                                                LightmapVersion = rw.Byte(LightmapVersion.GetValueOrDefault());

                                                if (Version >= 11)
                                                    TitleUID = rw.LookbackString(TitleUID);
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

        #region 0x004 chunk (version)

        /// <summary>
        /// CGameCtnChallenge 0x004 chunk (version)
        /// </summary>
        [Chunk(0x03043004, "version")]
        public class Chunk03043004 : HeaderChunk<CGameCtnChallenge>
        {
            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
            }
        }

        #endregion

        #region 0x005 chunk (xml)

        /// <summary>
        /// CGameCtnChallenge 0x005 chunk (xml)
        /// </summary>
        [Chunk(0x03043005, "xml")]
        public class Chunk03043005 : HeaderChunk<CGameCtnChallenge>
        {
            /// <summary>
            /// XML track information and dependencies.
            /// </summary>
            public string XML { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                XML = rw.String(XML);
            }
        }

        #endregion

        #region 0x007 chunk (thumbnail)

        /// <summary>
        /// CGameCtnChallenge 0x007 chunk (thumbnail)
        /// </summary>
        [Chunk(0x03043007, "thumbnail")]
        public class Chunk03043007 : HeaderChunk<CGameCtnChallenge>
        {
            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version { get; set; }

            /// <summary>
            /// Thumbnail bitmap.
            /// </summary>
            [IgnoreDataMember]
            public Task<Bitmap> Thumbnail { get; set; }

            MemoryStream msT;

            /// <summary>
            /// Comments of the map.
            /// </summary>
            public string Comments { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);

                if (Version != 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        if (rw.Mode == GameBoxReaderWriterMode.Write)
                        {
                            Thumbnail.Result.RotateFlip(RotateFlipType.Rotate180FlipX);
                            ExportThumbnail(ms, ImageFormat.Jpeg);
                        }

                        var thumbnailSize = rw.Int32((int)ms.Length);
                        rw.Bytes(Encoding.UTF8.GetBytes("<Thumbnail.jpg>"), "<Thumbnail.jpg>".Length); // Because the string is purely ASCII anyway, Length is usable
                        var thumbnailData = rw.Bytes(ms.ToArray(), thumbnailSize);
                        rw.Bytes(Encoding.UTF8.GetBytes("</Thumbnail.jpg>"), "</Thumbnail.jpg>".Length);
                        rw.Bytes(Encoding.UTF8.GetBytes("<Comments>"), "<Comments>".Length);
                        Comments = rw.String(Comments);
                        rw.Bytes(Encoding.UTF8.GetBytes("</Comments>"), "</Comments>".Length);

                        if (rw.Mode == GameBoxReaderWriterMode.Read && thumbnailData.Length > 0)
                        {
                            Thumbnail = Task.Run(() =>
                            {
                                msT = new MemoryStream(thumbnailData);
                                var bitmap = (Bitmap)Image.FromStream(msT);
                                bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
                                return bitmap;
                            });
                        }
                    }
                }
            }

            /// <summary>
            /// Exports the map's thumbnail.
            /// </summary>
            /// <param name="stream">Stream to export to.</param>
            /// <param name="format">Image format to use.</param>
            public void ExportThumbnail(Stream stream, ImageFormat format)
            {
                if (Thumbnail == null) return;

                if (format == ImageFormat.Jpeg)
                {
                    var encoding = new EncoderParameters(1);
                    encoding.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                    var encoder = ImageCodecInfo.GetImageDecoders().Where(x => x.FormatID == ImageFormat.Jpeg.Guid).First();

                    Thumbnail.Result.Save(stream, encoder, encoding);
                }
                else
                    Thumbnail.Result.Save(stream, format);
            }

            /// <summary>
            /// Exports the map's thumbnail.
            /// </summary>
            /// <param name="fileName">File to export to.</param>
            /// <param name="format">Image format to use.</param>
            public void ExportThumbnail(string fileName, ImageFormat format)
            {
                if (Thumbnail == null) return;

                if (format == ImageFormat.Jpeg)
                {
                    var encoding = new EncoderParameters(1);
                    encoding.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                    var encoder = ImageCodecInfo.GetImageDecoders().Where(x => x.FormatID == ImageFormat.Jpeg.Guid).First();

                    Thumbnail.Result.Save(fileName, encoder, encoding);
                }
                else
                    Thumbnail.Result.Save(fileName, format);
            }

            /// <summary>
            /// Asynchronously imports (and replaces) a thumbnail to use for the map.
            /// </summary>
            /// <param name="stream">Stream to import from.</param>
            /// <returns>A task that processes the thumbnail.</returns>
            public Task<Bitmap> ImportThumbnailAsync(Stream stream)
            {
                Thumbnail = Task.Run(() => new Bitmap(stream));
                return Thumbnail;
            }

            /// <summary>
            /// Asynchronously imports (and replaces) a thumbnail to use for the map.
            /// </summary>
            /// <param name="fileName">File to import from.</param>
            /// <returns>A task that processes the thumbnail.</returns>
            public Task<Bitmap> ImportThumbnailAsync(string fileName)
            {
                Thumbnail = Task.Run(() => new Bitmap(fileName));
                return Thumbnail;
            }

            /// <summary>
            /// Imports (and replaces) a thumbnail to use for the map.
            /// </summary>
            /// <param name="stream">Stream to import from.</param>
            public void ImportThumbnail(Stream stream)
            {
                ImportThumbnailAsync(stream).Wait();
            }

            /// <summary>
            /// Imports (and replaces) a thumbnail to use for the map.
            /// </summary>
            /// <param name="fileName">File to import from.</param>
            public void ImportThumbnail(string fileName)
            {
                ImportThumbnailAsync(fileName).Wait();
            }
        }

        #endregion

        #region 0x008 chunk (author)

        /// <summary>
        /// CGameCtnChallenge 0x008 chunk (author)
        /// </summary>
        [Chunk(0x03043008, "author")]
        public class Chunk03043008 : HeaderChunk<CGameCtnChallenge>
        {
            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version { get; set; }

            public int AuthorVersion { get; set; }

            /// <summary>
            /// Map author login.
            /// </summary>
            public string AuthorLogin { get; set; }

            /// <summary>
            /// Map author formatted nickname.
            /// </summary>
            public string AuthorNickname { get; set; }

            /// <summary>
            /// Map author zone.
            /// </summary>
            public string AuthorZone { get; set; }

            public string AuthorExtraInfo { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                AuthorVersion = rw.Int32(AuthorVersion);
                AuthorLogin = rw.String(AuthorLogin);
                AuthorNickname = rw.String(AuthorNickname);
                AuthorZone = rw.String(AuthorZone);
                AuthorExtraInfo = rw.String(AuthorExtraInfo);
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
                n.Vehicle = rw.Meta(n.Vehicle);
            }
        }

        #endregion

        #region 0x00F chunk (old block data)

        /// <summary>
        /// CGameCtnChallenge 0x00F chunk (old block data)
        /// </summary>
        [Chunk(0x0304300F, "old block data")]
        public class Chunk0304300F : Chunk<CGameCtnChallenge>
        {
            public int Unknown1 { get; set; }
            public int Unknown2 { get; set; }
            public Meta Unknown3 { get; set; }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                n.MapInfo = rw.Meta(n.MapInfo);
                n.Size = rw.Int3(n.Size.GetValueOrDefault());
                Unknown1 = rw.Int32(Unknown1);
                n.OldBlocks = rw.Array(n.OldBlocks,
                    i => rw.Reader.ReadNodeRef<CGameCtnBlock>(),
                    x => rw.Writer.Write(x));
                Unknown2 = rw.Int32(Unknown2);
                Unknown3 = rw.Meta(Unknown3);
            }
        }

        #endregion

        #region 0x011 chunk

        /// <summary>
        /// CGameCtnChallenge 0x011 chunk
        /// </summary>
        [Chunk(0x03043011)]
        public class Chunk03043011 : Chunk<CGameCtnChallenge>
        {
            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                n.CollectorList = rw.NodeRef<CGameCtnCollectorList>(n.CollectorList);
                n.ChallengeParameters = rw.NodeRef<CGameCtnChallengeParameters>(n.ChallengeParameters);
                n.Kind = (TrackKind)rw.Int32((int)(n.Kind ?? TrackKind.InProgress));
            }
        }

        #endregion

        #region 0x012 chunk

        /// <summary>
        /// CGameCtnChallenge 0x012 chunk
        /// </summary>
        [Chunk(0x03043012)]
        public class Chunk03043012 : Chunk<CGameCtnChallenge>
        {
            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.String(Unknown);
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

            public Chunk03043013(CGameCtnChallenge node) : base(node)
            {
                Chunk01F = new Chunk0304301F(node, this);
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
            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                n.Password = rw.String(n.Password);
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
            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
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
                n.Checkpoints = rw.Array(n.Checkpoints, i => rw.Reader.ReadInt3(), x => rw.Writer.Write(x));
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
                n.TMObjective_IsLapRace = rw.Boolean(n.TMObjective_IsLapRace.GetValueOrDefault());
                n.TMObjective_NbLaps = rw.Int32(n.TMObjective_NbLaps.GetValueOrDefault());
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
                n.ModPackDesc = rw.FileRef(n.ModPackDesc);
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
                n.Mode = (PlayMode)rw.Int32((int)n.Mode.GetValueOrDefault());
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

            public int? Version { get; set; }

            readonly bool is013;

            public Chunk0304301F(CGameCtnChallenge node) : this(node, null)
            {

            }

            public Chunk0304301F(CGameCtnChallenge node, Chunk chunk) : base(node)
            {
                is013 = chunk is Chunk03043013;
            }

            public override void Read(CGameCtnChallenge n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.MapInfo = r.ReadMeta();
                n.MapName = r.ReadString();
                n.Decoration = r.ReadMeta();
                n.Size = r.ReadInt3();
                NeedUnlock = r.ReadBoolean();

                if (!is013)
                    Version = r.ReadInt32();

                var nbBlocks = r.ReadInt32(); // It's maybe slower but better for the program to determine the count from the list

                List<Block> blocks = new List<Block>();

                while ((r.PeekUInt32() & 0xC0000000) > 0)
                {
                    var blockName = r.ReadLookbackString();
                    var dir = (Direction)r.ReadByte();
                    var coord = r.ReadByte3();
                    var flags = -1;

                    if (Version == null)
                        flags = r.ReadInt16();
                    else if (Version > 0)
                        flags = r.ReadInt32();

                    if (flags == -1)
                    {
                        blocks.Add(new Block(blockName, dir, (Int3)coord, flags, null, null, null));
                        continue;
                    }

                    string author = null;
                    CGameCtnBlockSkin skin = null;

                    if ((flags & (1 << 15)) != 0) // custom block
                    {
                        author = r.ReadLookbackString();
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

                    blocks.Add(new Block(blockName, dir, (Int3)coord, flags, author, skin, parameters));
                }

                n.Blocks = blocks;
            }

            public override void Write(CGameCtnChallenge n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(n.MapInfo);
                w.Write(n.MapName);
                w.Write(n.Decoration);
                w.Write(n.Size.GetValueOrDefault());
                w.Write(NeedUnlock);

                if (!is013)
                    w.Write(Version.GetValueOrDefault());

                w.Write(n.NbBlocks);

                foreach (var x in n.Blocks)
                {
                    w.WriteLookbackString(x.Name);
                    w.Write((byte)x.Direction);
                    w.Write((Byte3)x.Coord);

                    if (Version == null)
                        w.Write((short)x.Flags);
                    else if (Version > 0)
                        w.Write(x.Flags);

                    if (x.Flags != -1)
                    {
                        if ((x.Flags & 0x8000) != 0) // custom block
                        {
                            w.WriteLookbackString(x.Author);
                            w.Write(x.Skin);
                        }

                        if ((x.Flags & 0x100000) != 0)
                            w.Write(x.Parameters);
                    }
                }
            }
        }

        #endregion

        #region 0x021 chunk (old mediatracker)

        /// <summary>
        /// CGameCtnChallenge 0x021 chunk (old mediatracker)
        /// </summary>
        [Chunk(0x03043021, "old mediatracker")]
        public class Chunk03043021 : Chunk<CGameCtnChallenge>
        {
            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                n.ClipIntro = rw.NodeRef<CGameCtnMediaClip>(n.ClipIntro);
                n.ClipGroupInGame = rw.NodeRef<CGameCtnMediaClipGroup>(n.ClipGroupInGame);
                n.ClipGroupEndRace = rw.NodeRef<CGameCtnMediaClipGroup>(n.ClipGroupEndRace);
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
            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Boolean(Unknown);
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
            /// <summary>
            /// Reference to a music file.
            /// </summary>
            public FileRef CustomMusicPackDesc { get; set; }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                CustomMusicPackDesc = rw.FileRef(CustomMusicPackDesc);

                if(rw.Mode == GameBoxReaderWriterMode.Read) // TODO: check
                {
                    var idk = rw.Reader.ReadInt32();
                    if (idk != 0)
                        rw.Reader.BaseStream.Position -= sizeof(int);
                }
            }
        }

        #endregion

        #region 0x025 chunk

        /// <summary>
        /// CGameCtnChallenge 0x025 chunk
        /// </summary>
        [Chunk(0x03043025)]
        public class Chunk03043025 : Chunk<CGameCtnChallenge>
        {
            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                n.MapOrigin = rw.Vec2(n.MapOrigin.GetValueOrDefault());
                n.MapTarget = rw.Vec2(n.MapTarget.GetValueOrDefault());
            }
        }

        #endregion

        #region 0x026 chunk

        /// <summary>
        /// CGameCtnChallenge 0x026 chunk
        /// </summary>
        [Chunk(0x03043026)]
        public class Chunk03043026 : Chunk<CGameCtnChallenge>
        {
            public Node ClipGlobal { get; set; }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                ClipGlobal = rw.NodeRef(ClipGlobal);
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
            public bool ArchiveGmCamVal { get; set; }
            public Vec3? Vec1 { get; set; }
            public Vec3? Vec2 { get; set; }
            public Vec3? Vec3 { get; set; }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                ArchiveGmCamVal = rw.Boolean(ArchiveGmCamVal);

                if(ArchiveGmCamVal)
                {
                    rw.Byte(Unknown);

                    rw.Vec3(Unknown);
                    rw.Vec3(Unknown);
                    rw.Vec3(Unknown);

                    rw.Vec3(Unknown);
                    rw.Single(Unknown);
                    rw.Single(Unknown);
                    rw.Single(Unknown);
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
                n.Comments = rw.String(n.Comments);
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
                n.HashedPassword = rw.Bytes(n.HashedPassword, 16);
                n.CRC32 = rw.UInt32(n.CRC32.GetValueOrDefault());
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
            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Boolean(Unknown);
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
            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                n.ThumbnailPosition = rw.Vec3(n.ThumbnailPosition.GetValueOrDefault());
                n.ThumbnailPitchYawRoll = rw.Vec3(n.ThumbnailPitchYawRoll.GetValueOrDefault());
                n.ThumbnailFOV = rw.Single(n.ThumbnailFOV.GetValueOrDefault());

                rw.Bytes(Unknown, 31);
            }
        }

        #endregion

        #region 0x03D skippable chunk (lightmaps)

        /// <summary>
        /// CGameCtnChallenge 0x03D skippable chunk (lightmaps)
        /// </summary>
        [IgnoreChunk]
        [Chunk(0x0304303D, "lightmaps")]
        public class Chunk0304303D : SkippableChunk<CGameCtnChallenge>
        {
            public int Version { get; set; }

            public override void Read(CGameCtnChallenge n, GameBoxReader r, GameBoxWriter unknownW)
            {
                unknownW.Write(r.ReadBoolean());
                Version = r.ReadInt32();

                int frames = 1;
                if (Version >= 5)
                    frames = r.ReadInt32();

                if (Version >= 2)
                {
                    int size = 0;

                    for (var i = 0; i < frames; i++)
                    {
                        size = r.ReadInt32();
                        var image = r.ReadBytes(size);

                        if (Version >= 3)
                        {
                            var size1 = r.ReadInt32();
                            if (size1 > 0)
                            {
                                var image1 = r.ReadBytes(size1);
                            }
                        }

                        if (Version >= 6)
                        {
                            var size2 = r.ReadInt32();
                            if (size2 > 0)
                            {
                                var image2 = r.ReadBytes(size2);
                            }
                        }
                    }

                    if (size != 0)
                    {
                        var uncompressedSize = r.ReadInt32();
                        var compressedSize = r.ReadInt32();
                        var data = r.ReadBytes(compressedSize);

                        n.LightmapCache = Task.Run(() =>
                        {
                            using (var ms = new MemoryStream(data))
                            using (var zlib = new InflaterInputStream(ms))
                            using (var gbxr = new GameBoxReader(zlib))
                                return Parse<CHmsLightMapCache>(Node.Body, gbxr);
                        });
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
            int? ILookbackable.LookbackVersion { get; set; }
            List<string> ILookbackable.LookbackStrings { get; set; } = new List<string>();
            bool ILookbackable.LookbackWritten { get; set; }

            public int Version { get; set; } = 4;

            public int Unknown1 { get; set; }
            public int Unknown2 { get; set; } = 10;
            public int Unknown3 { get; set; }

            public override void OnLoad()
            {
                Node.items = new List<CGameCtnAnchoredObject>();
            }

            public override void Read(CGameCtnChallenge n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();

                if (Version != 0)
                {
                    Unknown1 = r.ReadInt32();
                    var size = r.ReadInt32();
                    Unknown2 = r.ReadInt32(); // 10

                    n.Items = ParseArray<CGameCtnAnchoredObject>(this, r).ToList();
                    Unknown3 = r.ReadInt32(); // 0
                }
            }

            public override void Write(CGameCtnChallenge n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);

                if(Version != 0)
                {
                    w.Write(Unknown1);

                    using (var itemMs = new MemoryStream())
                    using (var wr = new GameBoxWriter(itemMs, w.Lookbackable))
                    {

                        wr.Write(Unknown2);
                        wr.Write(n.items.Count);

                        foreach (var item in n.items)
                        {
                            wr.Write(item.ID);
                            item.Write(wr);
                        }

                        wr.Write(Unknown3);

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
            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                n.AuthorVersion = rw.Int32(n.AuthorVersion.GetValueOrDefault());
                n.AuthorLogin = rw.String(n.AuthorLogin);
                n.AuthorNickname = rw.String(n.AuthorNickname);
                n.AuthorZone = rw.String(n.AuthorZone);
                n.AuthorExtraInfo = rw.String(n.AuthorExtraInfo);
            }
        }

        #endregion

        #region 0x043 skippable chunk

        /// <summary>
        /// CGameCtnChallenge 0x043 skippable chunk
        /// </summary>
        [Chunk(0x03043043)]
        public class Chunk03043043 : SkippableChunk<CGameCtnChallenge>, ILookbackable
        {
            int? ILookbackable.LookbackVersion { get; set; }
            List<string> ILookbackable.LookbackStrings { get; set; } = new List<string>();
            bool ILookbackable.LookbackWritten { get; set; }

            public int Version { get; set; }
            public new byte[] Data { get; set; }

            public override void Read(CGameCtnChallenge n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();
                var sizeOfNodeWithClassID = r.ReadInt32();
                Data = r.ReadBytes(sizeOfNodeWithClassID);

                n.Genealogies = Task.Run(() =>
                {
                    using (var ms = new MemoryStream(Data))
                    using (var r2 = new GameBoxReader(ms, this))
                        return ParseArray<CGameCtnZoneGenealogy>(this, r2);
                });
            }

            public override void Write(CGameCtnChallenge n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);

                using (var ms = new MemoryStream())
                using (var w2 = new GameBoxWriter(ms))
                {

                    w2.Write(n.Genealogies.Result, x =>
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
            public int Version { get; set; }

            public override void OnLoad()
            {
                Node.MetadataTraits = new CScriptTraitsMetadata();
            }

            public override void Read(CGameCtnChallenge n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();
                var size = r.ReadInt32();

                n.MetadataTraits.Read(r);
            }

            public override void Write(CGameCtnChallenge n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);

                using (var ms = new MemoryStream())
                {
                    using (var wm = new GameBoxWriter(ms))
                        n.MetadataTraits.Write(wm);

                    w.Write((int)ms.Length);
                    w.Write(ms.ToArray(), 0, (int)ms.Length);
                }
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
            public int Version { get; set; } = 2;

            public CGameCtnMediaClip ClipPodium { get; set; }

            public int Unknown1 { get; set; } = 3;
            public int Unknown2 { get; set; } = 1;
            public int Unknown3 { get; set; } = 3;

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                n.ClipIntro = rw.NodeRef<CGameCtnMediaClip>(n.ClipIntro);
                ClipPodium = rw.NodeRef<CGameCtnMediaClip>(ClipPodium); //
                n.ClipGroupInGame = rw.NodeRef<CGameCtnMediaClipGroup>(n.ClipGroupInGame);
                n.ClipGroupEndRace = rw.NodeRef<CGameCtnMediaClipGroup>(n.ClipGroupEndRace);

                if (Version >= 2)
                {
                    n.ClipAmbiance = rw.NodeRef<CGameCtnMediaClip>(n.ClipAmbiance);

                    Unknown1 = rw.Int32(Unknown1);
                    Unknown2 = rw.Int32(Unknown2);
                    Unknown3 = rw.Int32(Unknown3);
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
                n.ObjectiveTextAuthor = rw.String(n.ObjectiveTextAuthor);
                n.ObjectiveTextGold = rw.String(n.ObjectiveTextGold);
                n.ObjectiveTextSilver = rw.String(n.ObjectiveTextSilver);
                n.ObjectiveTextBronze = rw.String(n.ObjectiveTextBronze);
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
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);

                n.TitleID = rw.LookbackString(n.TitleID);
                n.BuildVersion = rw.String(n.BuildVersion);
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
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version); // 3

                rw.Vec3(Unknown);

                if (Version != 0)
                {
                    rw.Boolean(Unknown);

                    if (Version >= 3)
                    {
                        rw.Single(Unknown);
                        rw.Single(Unknown);
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
            public override void ReadWrite(CGameCtnChallenge n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
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
            public int Version { get; set; }

            /// <summary>
            /// List of vectors that can't be directly figured out without information from <see cref="Chunk01F"/>.
            /// </summary>
            public List<Vec3> Vectors { get; set; } = new List<Vec3>();

            [IgnoreDataMember]
            public ReadOnlyCollection<FreeBlock> FreeBlocks
            {
                get
                {
                    List<FreeBlock> freeBlocks = new List<FreeBlock>();

                    var enumerator = Vectors.GetEnumerator();

                    foreach(var b in Node.Blocks.Where(x => x.IsFree))
                    {
                        enumerator.MoveNext();
                        var position = enumerator.Current;

                        enumerator.MoveNext();
                        var pitchYawRoll = enumerator.Current;

                        var fb = new FreeBlock(b)
                        {
                            Position = position,
                            PitchYawRoll = pitchYawRoll
                        };

                        freeBlocks.Add(fb);
                    }

                    return new ReadOnlyCollection<FreeBlock>(freeBlocks);
                }
            }

            public override void Read(CGameCtnChallenge n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();

                Vectors.Clear();
                while (r.BaseStream.Position < r.BaseStream.Length)
                    Vectors.Add(new Vec3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle()));
            }

            public override void Write(CGameCtnChallenge n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);
                foreach (var v in Vectors)
                    w.Write(v);
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Dependency
        {
            public string File { get; }
            public string Url { get; }

            public Dependency(string file, string url)
            {
                File = file;
                Url = url;
            }
        }
    #endregion
    }
}