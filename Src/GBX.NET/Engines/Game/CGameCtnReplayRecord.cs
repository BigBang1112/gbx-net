using GBX.NET.Inputs;

namespace GBX.NET.Engines.Game;

/// <summary>
/// A replay.
/// </summary>
/// <remarks>ID: 0x03093000</remarks>
[Node(0x03093000), WritingNotSupported]
[NodeExtension("Replay")]
public partial class CGameCtnReplayRecord : CMwNod, CGameCtnReplayRecord.IHeader, CGameCtnReplayRecord.IFull
{
    #region Fields

    private Ident? mapInfo;
    private TimeInt32? time;
    private string? playerNickname;
    private string? playerLogin;
    private string? titleID;
    private string? xml;
    private int? authorVersion;
    private string? authorLogin;
    private string? authorNickname;
    private string? authorZone;
    private string? authorExtraInfo;
    private byte[]? challengeData;
    private CGameCtnChallenge? challenge;
    private CGameCtnGhost[]? ghosts;
    private long[]? extras;
    private CGameCtnMediaClip? clip;
    private CPlugEntRecordData? recordData;
    private CCtnMediaBlockEventTrackMania? events;
    private TimeInt32? eventsDuration;
    private ControlEntry[]? controlEntries;
    private string? game;
    private CCtnMediaBlockUiTMSimpleEvtsDisplay? simpleEventsDisplay;
    private CGameCtnMediaBlockScenery.Key[] sceneryVortexKeys = Array.Empty<CGameCtnMediaBlockScenery.Key>();
    private int sceneryCapturableCount;
    private string? playgroundScript;
    private InterfaceScriptInfo[] interfaceScriptInfos = Array.Empty<InterfaceScriptInfo>();
    private IInput[]? inputs;

    #endregion

    #region Properties

    public HeaderChunkSet HeaderChunks { get; } = new();

    /// <summary>
    /// Map UID, environment, and author login of the map the replay orients in.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03093000>(sinceVersion: 2)]
    public Ident? MapInfo => mapInfo;

    /// <summary>
    /// The record time.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03093000>(sinceVersion: 2)]
    public TimeInt32? Time => time;

    /// <summary>
    /// Nickname of the record owner.
    /// </summary>
    [NodeMember]
    [SupportsFormatting]
    [AppliedWithChunk<Chunk03093000>(sinceVersion: 2)]
    public string? PlayerNickname => playerNickname;

    /// <summary>
    /// Login of the record owner.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03093000>(sinceVersion: 6)]
    public string? PlayerLogin => playerLogin;

    /// <summary>
    /// Title pack the replay orients in.
    /// </summary>
    [NodeMember(ExactName = "TitleId")]
    [AppliedWithChunk<Chunk03093000>(sinceVersion: 8)]
    [AppliedWithChunk<Chunk03093018>]
    public string? TitleID
    {
        get
        {
            DiscoverChunk<Chunk03093018>();
            return titleID;
        }
    }

    /// <summary>
    /// XML replay information.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03093001>]
    public string? XML => xml;

    [NodeMember]
    [AppliedWithChunk<Chunk03093002H>]
    [AppliedWithChunk<Chunk03093018>]
    public int? AuthorVersion
    {
        get
        {
            DiscoverChunk<Chunk03093018>();
            return authorVersion;
        }
    }

    /// <summary>
    /// Login of the replay creator.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03093002H>]
    [AppliedWithChunk<Chunk03093018>]
    public string? AuthorLogin
    {
        get
        {
            DiscoverChunk<Chunk03093018>();
            return authorLogin;
        }
    }

    /// <summary>
    /// Nickname of the replay creator.
    /// </summary>
    [NodeMember]
    [SupportsFormatting]
    [AppliedWithChunk<Chunk03093002H>]
    [AppliedWithChunk<Chunk03093018>]
    public string? AuthorNickname
    {
        get
        {
            DiscoverChunk<Chunk03093018>();
            return authorNickname;
        }
    }

    /// <summary>
    /// Zone of the replay creator.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03093002H>]
    [AppliedWithChunk<Chunk03093018>]
    public string? AuthorZone
    {
        get
        {
            DiscoverChunk<Chunk03093018>();
            return authorZone;
        }
    }

    [NodeMember]
    [AppliedWithChunk<Chunk03093002H>]
    [AppliedWithChunk<Chunk03093018>]
    public string? AuthorExtraInfo
    {
        get
        {
            DiscoverChunk<Chunk03093018>();
            return authorExtraInfo;
        }
    }

    /// <summary>
    /// The map the replay orients in. Null if only the header was read.
    /// </summary>
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03093002B>]
    public CGameCtnChallenge? Challenge
    {
        get
        {
            if (challengeData is null)
            {
                return null;
            }

            if (challenge is null)
            {
                using var ms = new MemoryStream(challengeData);
                challenge = GameBox.ParseNode<CGameCtnChallenge>(ms);
            }

            return challenge;
        }
    }

    /// <summary>
    /// Ghosts in the replay. Null if only the header was read.
    /// </summary>
    /// <remarks>Some ghosts can be considered as <see cref="CGameCtnMediaBlockGhost"/>. See <see cref="Clip"/>.</remarks>
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03093004>]
    [AppliedWithChunk<Chunk03093014>]
    public IReadOnlyCollection<CGameCtnGhost>? Ghosts => ghosts;

    [NodeMember]
    [AppliedWithChunk<Chunk03093014>]
    public IReadOnlyCollection<long>? Extras => extras;

    /// <summary>
    /// MediaTracker clip of the replay.
    /// </summary>
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0309300C>]
    [AppliedWithChunk<Chunk03093015>]
    public CGameCtnMediaClip? Clip => clip;

    [NodeMember]
    [AppliedWithChunk<Chunk03093024>]
    public CPlugEntRecordData? RecordData => recordData;

    /// <summary>
    /// Events occuring during the replay. Available in TMS and older games.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk0309300E>]
    public CCtnMediaBlockEventTrackMania? Events => events;

    /// <summary>
    /// Events occuring during the replay. Available in TMS and older games.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03093010>]
    public CCtnMediaBlockUiTMSimpleEvtsDisplay? SimpleEventsDisplay => simpleEventsDisplay;

    /// <summary>
    /// Duration of events in the replay (range of detected inputs). This can be <see cref="TimeInt32.Zero"/> if the replay was driven in editor and null if driven in TMU, TMUF, TMTurbo, TM2 and TM2020.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03093003>]
    [AppliedWithChunk<Chunk0309300D>]
    public TimeInt32? EventsDuration => eventsDuration;

    /// <summary>
    /// Inputs (keyboard, pad, wheel) of the replay from TM1.0, TMO, Sunrise and ESWC. For inputs stored in TMU, TMUF, TMTurbo and TM2: see <see cref="CGameCtnGhost.ControlEntries"/> in <see cref="Ghosts"/>. TM2020 and Shootmania inputs aren't available in replays and ghosts. Can be null if <see cref="EventsDuration"/> is 0, which can happen when you save the replay in editor.
    /// </summary>
    [Obsolete("Use Inputs instead. Property will be removed in 1.3.0")]
    public IReadOnlyCollection<ControlEntry>? ControlEntries => controlEntries;

    [NodeMember]
    [AppliedWithChunk<Chunk03093008>]
    [AppliedWithChunk<Chunk0309300F>]
    public string? Game // Needs further look
    {
        get
        {
            DiscoverChunks<Chunk03093008, Chunk0309300F>();
            return game;
        }
    }

    [NodeMember]
    [AppliedWithChunk<Chunk0309301A>]
    public IReadOnlyCollection<CGameCtnMediaBlockScenery.Key> SceneryVortexKeys
    {
        get
        {
            DiscoverChunk<Chunk0309301A>();
            return sceneryVortexKeys;
        }
    }

    [NodeMember(ExactName = "Scenery_CapturableCount")]
    [AppliedWithChunk<Chunk0309301A>]
    public int SceneryCapturableCount
    {
        get
        {
            DiscoverChunk<Chunk0309301A>();
            return sceneryCapturableCount;
        }
    }

    [NodeMember]
    [AppliedWithChunk<Chunk0309301C>]
    public string? PlaygroundScript
    {
        get
        {
            DiscoverChunk<Chunk0309301C>();
            return playgroundScript;
        }
    }

    [NodeMember]
    [AppliedWithChunk<Chunk0309301D>]
    public IReadOnlyCollection<InterfaceScriptInfo> InterfaceScriptInfos
    {
        get
        {
            DiscoverChunk<Chunk0309301D>();
            return interfaceScriptInfos;
        }
    }

    /// <summary>
    /// Inputs (keyboard, pad, wheel) of the replay from TM1.0, TMO, Sunrise and ESWC. For inputs stored in TMU, TMUF, TMTurbo and TM2: see <see cref="CGameCtnGhost.Inputs"/> in <see cref="Ghosts"/>. TM2020 and Shootmania inputs are available in <see cref="Ghosts"/> in <see cref="CGameCtnGhost.PlayerInputs"/>. Can be null if <see cref="EventsDuration"/> is 0, which can happen when you save the replay in editor.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03093003>]
    [AppliedWithChunk<Chunk0309300D>]
    public IReadOnlyCollection<IInput>? Inputs => inputs;

    public byte[]? ChallengeData { get => challengeData; set => challengeData = value; }

    #endregion

    #region Explicit properties

    Ident IHeaderTMS.MapInfo => MapInfo ?? throw new PropertyNullException(nameof(MapInfo));
    string IHeaderTMS.PlayerNickname => PlayerNickname ?? throw new PropertyNullException(nameof(PlayerNickname));
    string IHeaderTMS.XML => XML ?? throw new PropertyNullException(nameof(XML));
    string IHeaderTMU.PlayerLogin => PlayerLogin ?? throw new PropertyNullException(nameof(PlayerLogin));
    int IHeaderMP3.AuthorVersion => AuthorVersion ?? throw new PropertyNullException(nameof(AuthorVersion));
    string IHeaderMP3.AuthorLogin => AuthorLogin ?? throw new PropertyNullException(nameof(AuthorLogin));
    string IHeaderMP3.AuthorNickname => AuthorNickname ?? throw new PropertyNullException(nameof(AuthorNickname));
    string IHeaderMP3.AuthorZone => AuthorZone ?? throw new PropertyNullException(nameof(AuthorZone));
    string IHeaderMP3.AuthorExtraInfo => AuthorExtraInfo ?? throw new PropertyNullException(nameof(AuthorExtraInfo));
    string IHeaderMP3.TitleID => TitleID ?? throw new PropertyNullException(nameof(TitleID));

    CGameCtnChallenge IFullTM10.Challenge => Challenge ?? throw new PropertyNullException(nameof(Challenge));
    TimeInt32 IFullTM10.EventsDuration => EventsDuration ?? throw new PropertyNullException(nameof(EventsDuration));
    IReadOnlyCollection<CGameCtnGhost> IFullTM10.Ghosts => Ghosts ?? throw new PropertyNullException(nameof(Ghosts));
    string IFullTMS.Game => Game ?? throw new PropertyNullException(nameof(Game));
    
    CGameCtnChallenge IFullTMU.Challenge => Challenge ?? throw new PropertyNullException(nameof(Challenge));
    IReadOnlyCollection<CGameCtnGhost> IFullTMU.Ghosts => Ghosts ?? throw new PropertyNullException(nameof(Ghosts));
    IReadOnlyCollection<long> IFullTMUF.Extras => Extras ?? throw new PropertyNullException(nameof(Extras));
    CPlugEntRecordData IFullMP4.RecordData => RecordData ?? throw new PropertyNullException(nameof(RecordData));

    #endregion

    #region Constructors

    internal CGameCtnReplayRecord()
    {

    }

    #endregion

    #region Methods

    public IEnumerable<CGameCtnGhost> GetGhosts(bool alsoInClips = true)
    {
        if (ghosts is not null)
        {
            foreach (var ghost in ghosts)
            {
                yield return ghost;
            }
        }

        if (alsoInClips && clip is not null)
        {
            foreach (var ghost in clip.GetGhosts())
            {
                yield return ghost;
            }
        }
    }

    public GameBox<CGameCtnChallenge>? GetChallengeHeader()
    {
        if (challengeData is null)
        {
            return null;
        }

        using var ms = new MemoryStream(challengeData);
        
        return GameBox.ParseHeader<CGameCtnChallenge>(ms);
    }

    public CGameCtnChallenge? GetChallengeHeaderNode()
    {
        return GetChallengeHeader()?.Node;
    }

    #endregion

    #region Explicit methods

    IEnumerable<CGameCtnGhost> IFullTM10.GetGhosts() => GetGhosts(alsoInClips: false);
    IEnumerable<CGameCtnGhost> IFullTMS.GetGhosts(bool alsoInClips) => GetGhosts(alsoInClips);

    #endregion

    #region Chunks

    #region 0x000 header chunk (basic)

    /// <summary>
    /// CGameCtnReplayRecord 0x000 header chunk (basic)
    /// </summary>
    [Chunk(0x03093000, "basic")]
    public class Chunk03093000 : HeaderChunk<CGameCtnReplayRecord>, IVersionable
    {
        public byte U01;

        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            Version = r.ReadInt32();

            if (Version >= 2) // Versionings may not be exact, don't forget to adjust attributes
            {
                n.mapInfo = r.ReadIdent();
                n.time = r.ReadTimeInt32Nullable();
                n.playerNickname = r.ReadString();

                if (Version >= 6)
                {
                    n.playerLogin = r.ReadString();

                    if (Version >= 8)
                    {
                        U01 = r.ReadByte();
                        n.titleID = r.ReadId();
                    }
                }
            }
        }
    }

    #endregion

    #region 0x001 header chunk (xml)

    /// <summary>
    /// CGameCtnReplayRecord 0x001 header chunk (xml)
    /// </summary>
    [Chunk(0x03093001, "xml")]
    public class Chunk03093001 : HeaderChunk<CGameCtnReplayRecord>
    {
        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            n.xml = r.ReadString();
        }
    }

    #endregion

    #region 0x002 header chunk (author)

    /// <summary>
    /// CGameCtnReplayRecord 0x002 header chunk (author)
    /// </summary>
    [Chunk(0x03093002, "author")]
    public class Chunk03093002H : HeaderChunk<CGameCtnReplayRecord>, IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            Version = r.ReadInt32();
            n.authorVersion = r.ReadInt32();
            n.authorLogin = r.ReadString();
            n.authorNickname = r.ReadString();
            n.authorZone = r.ReadString();
            n.authorExtraInfo = r.ReadString();
        }
    }

    #endregion

    #region 0x002 chunk (track)

    /// <summary>
    /// CGameCtnReplayRecord 0x002 chunk (track)
    /// </summary>
    [Chunk(0x03093002, "track")]
    public class Chunk03093002B : Chunk<CGameCtnReplayRecord>
    {
        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            n.challengeData = r.ReadBytes();
        }
    }

    #endregion

    #region 0x003 chunk (validation TM1.0)

    /// <summary>
    /// CGameCtnReplayRecord 0x003 chunk (validation TM1.0)
    /// </summary>
    [Chunk(0x03093003, "validation TM1.0")]
    public class Chunk03093003 : Chunk<CGameCtnReplayRecord>
    {
        public int U01;
        public int U02;

        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            n.eventsDuration = r.ReadTimeInt32();

            if (n.eventsDuration == TimeInt32.Zero)
            {
                return;
            }

            U01 = r.ReadInt32();

            // All control names available in the game
            var controlNames = r.ReadArray(r =>
            {
                // Maybe bindings
                r.ReadInt32();
                r.ReadInt32();

                return r.ReadString(); // Input name
            });

            var numEntries = r.ReadInt32() - 1;

            n.controlEntries = new ControlEntry[numEntries];
            n.inputs = new IInput[numEntries];

            for (var i = 0; i < numEntries; i++)
            {
                var time = TimeInt32.FromMilliseconds(r.ReadInt32() - 10000);
                var controlNameIndex = r.ReadInt32();
                var data = r.ReadUInt32();

                var name = controlNames[controlNameIndex];

                n.controlEntries[i] = name switch
                {
                    "Steer (analog)" => new ControlEntryAnalog(name, time, data), // Data is bugged
                    _ => new ControlEntry(name, time, data),
                };

                n.inputs[i] = name switch
                {
                    "Accelerate" => new Accelerate(time, data != 0),
                    "Brake" => new Brake(time, data != 0),
                    "Steer (analog)" => new SteerOld(time, BitConverter.ToSingle(BitConverter.GetBytes(data), 0)),
                    "Steer left" => new SteerLeft(time, data != 0),
                    "Steer right" => new SteerRight(time, data != 0),
                    _ => new UnknownInput(time, name, data),
                };
            }

            Array.Reverse(n.controlEntries); // Inputs are originally reversed
            Array.Reverse(n.inputs); // Inputs are originally reversed

            U02 = r.ReadInt32();
        }
    }

    #endregion

    #region 0x004 chunk (ghosts)

    /// <summary>
    /// CGameCtnReplayRecord 0x004 chunk (ghosts)
    /// </summary>
    [Chunk(0x03093004, "ghosts")]
    public class Chunk03093004 : Chunk<CGameCtnReplayRecord>, IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            Version = r.ReadInt32();

            var u02 = r.ReadInt32();
            n.ghosts = r.ReadArray(r => r.ReadNodeRef<CGameCtnGhost>()!);

            var u03 = r.ReadInt32(); // CGameReplayObjectVisData something, millisecond length of something (usually record time + 0.5s)
            var u04 = r.ReadInt32(); // SOldShowTime
        }

        public override async Task ReadAsync(CGameCtnReplayRecord n, GameBoxReader r, CancellationToken cancellationToken = default)
        {
            Version = r.ReadInt32();

            var u02 = r.ReadInt32();
            n.ghosts = (await r.ReadArrayAsync(r => r.ReadNodeRefAsync<CGameCtnGhost>()!))!;

            var u03 = r.ReadInt32(); // millisecond length of something (usually record time + 0.5s)
            var u04 = r.ReadInt32();
        }
    }

    #endregion

    #region 0x005 chunk

    /// <summary>
    /// CGameCtnReplayRecord 0x005 chunk
    /// </summary>
    [Chunk(0x03093005)]
    public class Chunk03093005 : Chunk<CGameCtnReplayRecord>
    {
        public int U01;

        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            U01 = r.ReadInt32();
        }
    }

    #endregion

    #region 0x007 skippable chunk

    /// <summary>
    /// CGameCtnReplayRecord 0x007 skippable chunk
    /// </summary>
    [Chunk(0x03093007)]
    public class Chunk03093007 : SkippableChunk<CGameCtnReplayRecord>
    {
        public int U01;

        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            U01 = r.ReadInt32();
        }
    }

    #endregion

    #region 0x008 skippable chunk (game)

    /// <summary>
    /// CGameCtnReplayRecord 0x008 skippable chunk (game)
    /// </summary>
    [Chunk(0x03093008, "game")]
    public class Chunk03093008 : SkippableChunk<CGameCtnReplayRecord>
    {
        public int U01;

        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            n.game = r.ReadString();
            U01 = r.ReadInt32(); // SOldCutKey2
        }
    }

    #endregion

    #region 0x00C chunk (clip)

    /// <summary>
    /// CGameCtnReplayRecord 0x00C chunk (clip)
    /// </summary>
    [Chunk(0x0309300C, "clip")]
    public class Chunk0309300C : Chunk<CGameCtnReplayRecord>
    {
        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            n.clip = r.ReadNodeRef<CGameCtnMediaClip>();
        }

        public override async Task ReadAsync(CGameCtnReplayRecord n, GameBoxReader r, CancellationToken cancellationToken = default)
        {
            n.clip = await r.ReadNodeRefAsync<CGameCtnMediaClip>(cancellationToken);
        }
    }

    #endregion

    #region 0x00D chunk (validation)

    /// <summary>
    /// CGameCtnReplayRecord 0x00D chunk (validation)
    /// </summary>
    [Chunk(0x0309300D, "validation")]
    public class Chunk0309300D : Chunk<CGameCtnReplayRecord>
    {
        public int U01;
        public int U02;

        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            n.eventsDuration = r.ReadTimeInt32();

            if (n.eventsDuration == TimeInt32.Zero)
            {
                return;
            }

            U01 = r.ReadInt32();

            var controlNames = r.ReadArray(r => r.ReadId());

            var numEntries = r.ReadInt32();
            U02 = r.ReadInt32();

            n.controlEntries = new ControlEntry[numEntries];
            n.inputs = new IInput[numEntries];

            for (var i = 0; i < numEntries; i++)
            {
                var time = TimeInt32.FromMilliseconds(r.ReadInt32() - 100000);
                var controlNameIndex = r.ReadByte();
                var data = r.ReadUInt32();

                var name = controlNames[controlNameIndex];

                n.controlEntries[i] = (string)name switch
                {
                    "Steer" or "Gas" or "AccelerateReal" or "BrakeReal"
                      => new ControlEntryAnalog(name, time, data),
                    _ => new ControlEntry(name, time, data),
                };

                n.inputs[i] = NET.Inputs.Input.Parse(time, name, data);
            }
        }
    }

    #endregion

    #region 0x00E chunk (events)

    /// <summary>
    /// CGameCtnReplayRecord 0x00E chunk (events)
    /// </summary>
    [Chunk(0x0309300E, "events")]
    public class Chunk0309300E : Chunk<CGameCtnReplayRecord>
    {
        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            n.events = r.ReadNodeRef<CCtnMediaBlockEventTrackMania>();
        }
    }

    #endregion

    #region 0x00F skippable chunk

    /// <summary>
    /// CGameCtnReplayRecord 0x00F skippable chunk
    /// </summary>
    [Chunk(0x0309300F)]
    public class Chunk0309300F : SkippableChunk<CGameCtnReplayRecord>
    {
        public int U01;
        public int U02;
        public int U03;
        public string U04 = string.Empty;

        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            n.game = r.ReadString();
            U01 = r.ReadInt32();
            U02 = r.ReadInt32();
            U03 = r.ReadInt32();
            U04 = r.ReadString();
        }
    }

    #endregion

    #region 0x010 chunk (simple events display)

    /// <summary>
    /// CGameCtnReplayRecord 0x010 chunk (simple events display)
    /// </summary>
    [Chunk(0x03093010, "simple events display")]
    public class Chunk03093010 : Chunk<CGameCtnReplayRecord>
    {
        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            n.simpleEventsDisplay = r.ReadNodeRef<CCtnMediaBlockUiTMSimpleEvtsDisplay>();
        }

        public override async Task ReadAsync(CGameCtnReplayRecord n, GameBoxReader r, CancellationToken cancellationToken = default)
        {
            n.simpleEventsDisplay = await r.ReadNodeRefAsync<CCtnMediaBlockUiTMSimpleEvtsDisplay>(cancellationToken);
        }
    }

    #endregion

    #region 0x011 chunk

    /// <summary>
    /// CGameCtnReplayRecord 0x011 chunk
    /// </summary>
    [Chunk(0x03093011)]
    public class Chunk03093011 : Chunk<CGameCtnReplayRecord>
    {
        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {

        }
    }

    #endregion

    #region 0x013 skippable chunk

    /// <summary>
    /// CGameCtnReplayRecord 0x013 skippable chunk
    /// </summary>
    [Chunk(0x03093013), IgnoreChunk]
    public class Chunk03093013 : SkippableChunk<CGameCtnReplayRecord>
    {

    }

    #endregion

    #region 0x014 chunk (ghosts)

    /// <summary>
    /// CGameCtnReplayRecord 0x014 chunk (ghosts)
    /// </summary>
    [Chunk(0x03093014, "ghosts")]
    public class Chunk03093014 : Chunk<CGameCtnReplayRecord>
    {
        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            r.ReadInt32(); // listVersion
            n.ghosts = r.ReadArray(r => r.ReadNodeRef<CGameCtnGhost>()!);
            r.ReadInt32(); // always zero
            n.extras = r.ReadArray(r => r.ReadInt64()); // SOldShowTime array
        }

        public override async Task ReadAsync(CGameCtnReplayRecord n, GameBoxReader r, CancellationToken cancellationToken = default)
        {
            r.ReadInt32(); // listVersion
            n.ghosts = (await r.ReadArrayAsync(r => r.ReadNodeRefAsync<CGameCtnGhost>()!))!;
            r.ReadInt32();
            n.extras = r.ReadArray(r => r.ReadInt64());
        }
    }

    #endregion

    #region 0x015 chunk (clip)

    /// <summary>
    /// CGameCtnReplayRecord 0x015 chunk (clip)
    /// </summary>
    [Chunk(0x03093015, "clip")]
    public class Chunk03093015 : Chunk<CGameCtnReplayRecord>
    {
        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            n.clip = r.ReadNodeRef<CGameCtnMediaClip>();
        }

        public override async Task ReadAsync(CGameCtnReplayRecord n, GameBoxReader r, CancellationToken cancellationToken = default)
        {
            n.clip = await r.ReadNodeRefAsync<CGameCtnMediaClip>(cancellationToken);
        }
    }

    #endregion

    #region 0x018 skippable chunk (author)

    /// <summary>
    /// CGameCtnReplayRecord 0x018 skippable chunk (author)
    /// </summary>
    [Chunk(0x03093018, "author")]
    public class Chunk03093018 : SkippableChunk<CGameCtnReplayRecord>
    {
        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            n.titleID = r.ReadId();
            n.authorVersion = r.ReadInt32();
            n.authorLogin = r.ReadString();
            n.authorNickname = r.ReadString();
            n.authorZone = r.ReadString();
            n.authorExtraInfo = r.ReadString();
        }
    }

    #endregion

    #region 0x01A skippable chunk (scenery vortex key)

    /// <summary>
    /// CGameCtnReplayRecord 0x01A skippable chunk (scenery vortex key)
    /// </summary>
    [Chunk(0x0309301A, "scenery vortex key")]
    public class Chunk0309301A : SkippableChunk<CGameCtnReplayRecord>
    {
        public float U01;
        public Node? U02;

        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            r.ReadInt32(); // always 0

            n.sceneryVortexKeys = r.ReadArray(r => new CGameCtnMediaBlockScenery.Key
            {
                Time = r.ReadTimeSingle(),
                U01 = r.ReadSingle(),
                U02 = r.ReadSingle(),
                U03 = r.ReadSingle()
            });
            
            U01 = r.ReadSingle();
            U02 = r.ReadNodeRef(); // CGameReplayObjectVisData
            
            n.sceneryCapturableCount = r.ReadInt32();
        }
    }

    #endregion

    #region 0x01B skippable chunk (player of interest)

    /// <summary>
    /// CGameCtnReplayRecord 0x01B skippable chunk (player of interest)
    /// </summary>
    [Chunk(0x0309301B, "player of interest")]
    public class Chunk0309301B : SkippableChunk<CGameCtnReplayRecord>, IVersionable
    {
        public int Version { get; set; }

        public (int, int)[]? U01;

        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            Version = r.ReadInt32();
            U01 = r.ReadArray(r => (r.ReadInt32(), r.ReadInt32()));
        }
    }

    #endregion

    #region 0x01C skippable chunk

    /// <summary>
    /// CGameCtnReplayRecord 0x01C skippable chunk
    /// </summary>
    [Chunk(0x0309301C)]
    public class Chunk0309301C : SkippableChunk<CGameCtnReplayRecord>, IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            Version = r.ReadInt32();
            n.playgroundScript = r.ReadString(); // CampaignSolo
        }
    }

    #endregion

    #region 0x01D skippable chunk (InterfaceScriptInfos)

    /// <summary>
    /// CGameCtnReplayRecord 0x01D skippable chunk (InterfaceScriptInfos)
    /// </summary>
    [Chunk(0x0309301D, "InterfaceScriptInfos")]
    public class Chunk0309301D : SkippableChunk<CGameCtnReplayRecord>, IVersionable
    {
        public int Version { get; set; }
        
        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            Version = r.ReadInt32();

            if (Version > 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            n.interfaceScriptInfos = r.ReadArray(r => new InterfaceScriptInfo(
                U01: r.ReadArray(r => r.ReadString()),
                U02: r.ReadInt32()));
        }
    }

    #endregion

    #region 0x01E skippable chunk (actions)

    /// <summary>
    /// CGameCtnReplayRecord 0x01E skippable chunk (actions)
    /// </summary>
    [Chunk(0x0309301E, "actions"), IgnoreChunk]
    public class Chunk0309301E : SkippableChunk<CGameCtnReplayRecord>
    {

    }

    #endregion

    #region 0x01F skippable chunk (AnchoredObjectInfos)

    /// <summary>
    /// CGameCtnReplayRecord 0x01E skippable chunk (AnchoredObjectInfos)
    /// </summary>
    [Chunk(0x0309301F, "AnchoredObjectInfos"), IgnoreChunk]
    public class Chunk0309301F : SkippableChunk<CGameCtnReplayRecord>
    {
        /*
         * version 1: SOldGameCtnReplayRecord_AnchoredObjectInfos array
         * - bool
         * - int
         * - float
         * - float
         * - float
         * - int
         * 
         * version 2: noderef CGameReplayObjectVisData m_Scenery_Objects_Deprecated
         */
    }

    #endregion

    #region 0x020 skippable chunk (item skins and names)

    /// <summary>
    /// CGameCtnReplayRecord 0x020 skippable chunk (item skins and names)
    /// </summary>
    [Chunk(0x03093020, "item skins and names"), IgnoreChunk]
    public class Chunk03093020 : SkippableChunk<CGameCtnReplayRecord>
    {

    }

    #endregion

    #region 0x021 skippable chunk

    /// <summary>
    /// CGameCtnReplayRecord 0x021 skippable chunk
    /// </summary>
    [Chunk(0x03093021)]
    public class Chunk03093021 : SkippableChunk<CGameCtnReplayRecord>
    {
        private int version;

        public bool U01;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnReplayRecord n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Boolean(ref U01);
        }
    }

    #endregion

    #region 0x022 skippable chunk (TimedCamVal)

    /// <summary>
    /// CGameCtnReplayRecord 0x022 skippable chunk (TimedCamVal)
    /// </summary>
    [Chunk(0x03093022, "TimedCamVal"), IgnoreChunk]
    public class Chunk03093022 : SkippableChunk<CGameCtnReplayRecord>
    {

    }

    #endregion

    #region 0x023 skippable chunk (BonusBumpKey)

    /// <summary>
    /// CGameCtnReplayRecord 0x023 skippable chunk (BonusBumpKey)
    /// </summary>
    [Chunk(0x03093023, "BonusBumpKey"), IgnoreChunk]
    public class Chunk03093023 : SkippableChunk<CGameCtnReplayRecord>
    {

    }

    #endregion

    #region 0x024 chunk (record data)

    /// <summary>
    /// CGameCtnReplayRecord 0x024 chunk (record data)
    /// </summary>
    [Chunk(0x03093024, "record data")]
    public class Chunk03093024 : Chunk<CGameCtnReplayRecord>, IVersionable
    {
        public int U02;

        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            Version = r.ReadInt32();
            U02 = r.ReadInt32(); // nod
            n.recordData = r.ReadNodeRef<CPlugEntRecordData>();
        }

        public override async Task ReadAsync(CGameCtnReplayRecord n, GameBoxReader r, CancellationToken cancellationToken = default)
        {
            Version = r.ReadInt32();
            U02 = r.ReadInt32(); // nod
            n.recordData = await r.ReadNodeRefAsync<CPlugEntRecordData>(cancellationToken);
        }
    }

    #endregion

    #region 0x025 skippable chunk

    /// <summary>
    /// CGameCtnReplayRecord 0x025 skippable chunk
    /// </summary>
    [Chunk(0x03093025)]
    public class Chunk03093025 : SkippableChunk<CGameCtnReplayRecord>, IVersionable
    {
        public float U01;
        public int U02;

        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GameBoxReader r)
        {
            Version = r.ReadInt32();
            U01 = r.ReadSingle();
            U02 = r.ReadInt32();
        }
    }

    #endregion

    #region 0x026 skippable chunk (EntDataSceneUIdsToGhost)

    /// <summary>
    /// CGameCtnReplayRecord 0x026 skippable chunk (EntDataSceneUIdsToGhost)
    /// </summary>
    [Chunk(0x03093026, "EntDataSceneUIdsToGhost"), IgnoreChunk]
    public class Chunk03093026 : SkippableChunk<CGameCtnReplayRecord>
    {

    }

    #endregion

    #region 0x027 skippable chunk

    /// <summary>
    /// CGameCtnReplayRecord 0x027 skippable chunk
    /// </summary>
    [Chunk(0x03093027), IgnoreChunk]
    public class Chunk03093027 : SkippableChunk<CGameCtnReplayRecord>
    {

    }

    #endregion

    #region 0x028 skippable chunk

    /// <summary>
    /// CGameCtnReplayRecord 0x028 skippable chunk
    /// </summary>
    [Chunk(0x03093028), IgnoreChunk]
    public class Chunk03093028 : SkippableChunk<CGameCtnReplayRecord>
    {

    }

    #endregion
    
    #endregion
}
