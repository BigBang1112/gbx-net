using GBX.NET.Engines.Function;
using GBX.NET.Engines.GameData;
using GBX.NET.Inputs;
using System.Buffers.Binary;
using System.Collections.Immutable;
using System.Globalization;

namespace GBX.NET.Engines.Game;

[WriteNotSupported]
public partial class CGameCtnReplayRecord
{
    private Ident? mapInfo;
    private TimeInt32? time;
    private TimeInt32? recordDuration;
    private ImmutableArray<OldShowTime>? oldShowTimes;
    private byte[][]? oldCutKeys;
    private byte[][]? oldCutKeys2;
    private NodeRefList<CGameCtnMediaTrack>? tracks;
    private NodeRef<CPlugDataTape>? bulletVisDataTapeRef;
    private NodeRef<CPlugDataTape>? charVisDataTapeRef;
    private NodeRef<CPlugDataTape>? arenaVisDataTapeRef;
    private string? playerNickname;
    private string? playerLogin;
    private string? titleId;
    private string? xml;
    private int? authorVersion;
    private string? authorLogin;
    private string? authorNickname;
    private string? authorZone;
    private string? authorExtraInfo;
    private RawData? challengeGbxData;
    private ImmutableList<CGameCtnMediaBlockScenery.Key>? sceneryVortexKeys;
    private float sceneryVortexShrinkSpeed;
    private NodeRef<CPlugDataTape>? sceneryCapturableDataTapeRef;
    private int sceneryCapturableCount;
    private ImmutableArray<PlayerOfInterest>? playersOfInterest;
    private string? playgroundScript;
    private ImmutableList<InterfaceScriptInfo>? interfaceScriptInfos;
    private ImmutableList<EntDataSceneUIdsToGhost>? entDataSceneUIdsToGhosts;
    private ImmutableList<CGameActionModel>? actionModels;
    private ImmutableList<string>? actionNames;
    private ImmutableList<string>? actionFileNames;
    private ImmutableList<ItemResource>? items;
    private ImmutableList<ItemResource>? additionalItems;
    private bool enableMinimap;
    private byte[][]? cameraRecord;
    private NodeRef<CPlugDataTape>? emptyCarsVisDataTapeRef;
    private ImmutableList<BonusBumpKey>? bonusBumpKeys;
    private NodeRef<CPlugDataTape>? sceneryTurretsDataTapeRef;
    private float timeOfDay = -1;
    private TimeInt32 dayPeriod;
    private bool keepOnlyAuthorGhosts;
    private CGameCtnChallenge? challenge;
    private ImmutableArray<IInput>? pendingInputs;
    private TimeInt32? pendingEventsDuration;
    private NodeRefList<CGameCtnGhost>? ghosts;
    private NodeRef<CFuncSegment>? humanTimeToGameTimeFuncRef;
    private NodeRef<CGameCtnMediaClip>? clipRef;
    private NodeRef<CPlugEntRecordData>? recordDataRef;
    private NodeRef<CCtnMediaBlockEventTrackMania>? eventsRef;
    private NodeRef<CCtnMediaBlockUiTMSimpleEvtsDisplay>? simpleEventsDisplayRef;
    private NodeRef<CGameCtnChallengeParameters>? challengeParametersRef;
    private ImmutableList<AnchoredObjectInfo>? anchoredObjectInfos;
    private NodeRef<CGameReplayObjectVisData>? sceneryObjectsRef;

#if NET9_0_OR_GREATER
    private readonly Lock ChallengeLock = new();
#else
    private readonly object ChallengeLock = new();
#endif

    /// <summary>
    /// Map UID, environment, and author login of the map the replay orients in.
    /// </summary>
    /// <remarks>Applied with: 0x000 v4-9998, 0x000 v10000+ header chunks</remarks>
    [AppliedWithChunk<HeaderChunk03093000>(4, 9998)]
    [AppliedWithChunk<HeaderChunk03093000>(10000)]
    public Ident? MapInfo => mapInfo;

    /// <summary>
    /// The record time.
    /// </summary>
    /// <remarks>Applied with: 0x000 v4-9998, 0x000 v10000+ header chunks</remarks>
    [AppliedWithChunk<HeaderChunk03093000>(4, 9998)]
    [AppliedWithChunk<HeaderChunk03093000>(10000)]
    public TimeInt32? Time => time;

    /// <summary>
    /// Duration of the legacy replay recording, including time recorded after the finish.
    /// </summary>
    /// <remarks>Applied with: 0x004, 0x014 body chunks</remarks>
    [AppliedWithChunk<Chunk03093004>]
    [AppliedWithChunk<Chunk03093014>]
    public TimeInt32? RecordDuration => recordDuration;

    /// <summary>
    /// Legacy show-time records whose contents are preserved as opaque 8-byte values.
    /// </summary>
    /// <remarks>Applied with: 0x004, 0x014 body chunks</remarks>
    [AppliedWithChunk<Chunk03093004>]
    [AppliedWithChunk<Chunk03093014>]
    public ImmutableArray<OldShowTime>? OldShowTimes => oldShowTimes;

    /// <summary>
    /// Native SOldCutKey records, each preserved as 68 opaque bytes.
    /// </summary>
    /// <remarks>Applied with: 0x005 body chunk</remarks>
    [AppliedWithChunk<Chunk03093005>]
    public byte[][]? OldCutKeys => oldCutKeys;

    /// <summary>
    /// Native SOldCutKey2 records, each preserved as 72 opaque bytes.
    /// </summary>
    /// <remarks>Applied with: 0x008, 0x013 body chunks</remarks>
    [AppliedWithChunk<Chunk03093008>]
    [AppliedWithChunk<Chunk03093013>]
    public byte[][]? OldCutKeys2 => oldCutKeys2;

    /// <summary>
    /// Legacy MediaTracker track references.
    /// </summary>
    /// <remarks>Applied with: 0x00A body chunk</remarks>
    [AppliedWithChunk<Chunk0309300A>]
    public NodeRefList<CGameCtnMediaTrack>? Tracks => tracks;

    /// <summary>
    /// Legacy bullet visual states and notices for the replay scene.
    /// </summary>
    /// <remarks>Applied with: 0x016, 0x017 body chunks</remarks>
    [AppliedWithChunk<Chunk03093016>]
    [AppliedWithChunk<Chunk03093017>]
    public CPlugDataTape? BulletVisDataTape => bulletVisDataTapeRef?.Resolve();

    /// <summary>
    /// Legacy character visual states and notices for the replay scene.
    /// </summary>
    /// <remarks>Applied with: 0x016, 0x017 body chunks</remarks>
    [AppliedWithChunk<Chunk03093016>]
    [AppliedWithChunk<Chunk03093017>]
    public CPlugDataTape? CharVisDataTape => charVisDataTapeRef?.Resolve();

    /// <summary>
    /// Legacy arena visual notices for the replay scene.
    /// </summary>
    /// <remarks>Applied with: 0x016, 0x017 body chunks</remarks>
    [AppliedWithChunk<Chunk03093016>]
    [AppliedWithChunk<Chunk03093017>]
    public CPlugDataTape? ArenaVisDataTape => arenaVisDataTapeRef?.Resolve();

    /// <summary>
    /// Nickname of the record owner.
    /// </summary>
    /// <remarks>Applied with: 0x000 v4-9998, 0x000 v10000+ header chunks</remarks>
    [SupportsFormatting]
    [AppliedWithChunk<HeaderChunk03093000>(4, 9998)]
    [AppliedWithChunk<HeaderChunk03093000>(10000)]
    public string? PlayerNickname => playerNickname;

    /// <summary>
    /// Login of the record owner.
    /// </summary>
    /// <remarks>Applied with: 0x000 v6-9998, 0x000 v10001+ header chunks</remarks>
    [AppliedWithChunk<HeaderChunk03093000>(6, 9998)]
    [AppliedWithChunk<HeaderChunk03093000>(10001)]
    public string? PlayerLogin => playerLogin;

    /// <summary>
    /// Title pack the replay orients in.
    /// </summary>
    /// <remarks>Applied with: 0x000 v8+ header chunk, 0x018 body chunk</remarks>
    [AppliedWithChunk<HeaderChunk03093000>(8)]
    [AppliedWithChunk<Chunk03093018>]
    public string? TitleId => titleId;

    /// <summary>
    /// XML replay information.
    /// </summary>
    /// <remarks>Applied with: 0x001 header chunk</remarks>
    [AppliedWithChunk<HeaderChunk03093001>]
    public string? Xml => xml;

    /// <remarks>Applied with: 0x002 header chunk, 0x018 body chunk</remarks>
    [AppliedWithChunk<HeaderChunk03093002>]
    [AppliedWithChunk<Chunk03093018>]
    public int? AuthorVersion => authorVersion;

    /// <summary>
    /// Login of the replay creator.
    /// </summary>
    /// <remarks>Applied with: 0x002 header chunk, 0x018 body chunk</remarks>
    [AppliedWithChunk<HeaderChunk03093002>]
    [AppliedWithChunk<Chunk03093018>]
    public string? AuthorLogin => authorLogin;

    /// <summary>
    /// Nickname of the replay creator.
    /// </summary>
    /// <remarks>Applied with: 0x002 header chunk, 0x018 body chunk</remarks>
    [SupportsFormatting]
    [AppliedWithChunk<HeaderChunk03093002>]
    [AppliedWithChunk<Chunk03093018>]
    public string? AuthorNickname => authorNickname;

    /// <summary>
    /// Zone of the replay creator.
    /// </summary>
    /// <remarks>Applied with: 0x002 header chunk, 0x018 body chunk</remarks>
    [AppliedWithChunk<HeaderChunk03093002>]
    [AppliedWithChunk<Chunk03093018>]
    public string? AuthorZone => authorZone;

    /// <remarks>Applied with: 0x002 header chunk, 0x018 body chunk</remarks>
    [AppliedWithChunk<HeaderChunk03093002>]
    [AppliedWithChunk<Chunk03093018>]
    public string? AuthorExtraInfo => authorExtraInfo;

    /// <remarks>Applied with: 0x002 body chunk</remarks>
    [AppliedWithChunk<Chunk03093002>]
    public RawData? ChallengeGbxData => challengeGbxData;

    /// <summary>
    /// The map the replay orients in. Null if only the header was read.
    /// </summary>
    /// <remarks>Applied with: 0x002 body chunk</remarks>
    [AppliedWithChunk<Chunk03093002>]
    public CGameCtnChallenge? Challenge
    {
        get
        {
            if (ChallengeGbxData is null || ChallengeGbxData.Parsed) return challenge;

            lock (ChallengeLock)
            {
                if (ChallengeGbxData is null || ChallengeGbxData.Parsed) return challenge;

                using var ms = new MemoryStream(ChallengeGbxData.Data);

                try
                {
                    challenge = Gbx.ParseNode<CGameCtnChallenge>(ms);
                    ChallengeGbxData.Parsed = true;
                }
                catch (Exception ex)
                {
                    ChallengeGbxData.Exception = ex;
                    throw;
                }

                return challenge;
            }
        }
    }

    /// <summary>
    /// Ghosts in the replay. Null if only the header was read.
    /// </summary>
    /// <remarks>Some ghosts can be considered as <see cref="CGameCtnMediaBlockGhost"/>. See <see cref="Clip"/>.</remarks>
    /// <remarks>Applied with: 0x004, 0x014 body chunks</remarks>
    [AppliedWithChunk<Chunk03093004>]
    [AppliedWithChunk<Chunk03093014>]
    public NodeRefList<CGameCtnGhost>? Ghosts => ghosts;

    /// <remarks>Applied with: 0x00B body chunk</remarks>
    [AppliedWithChunk<Chunk0309300B>]
    public CFuncSegment? HumanTimeToGameTimeFunc => humanTimeToGameTimeFuncRef?.Resolve();

    /// <summary>
    /// MediaTracker clip of the replay.
    /// </summary>
    /// <remarks>Applied with: 0x00C, 0x015 body chunks</remarks>
    [AppliedWithChunk<Chunk0309300C>]
    [AppliedWithChunk<Chunk03093015>]
    public CGameCtnMediaClip? Clip => clipRef?.Resolve();

    /// <remarks>Applied with: 0x024 v1+ body chunk</remarks>
    [AppliedWithChunk<Chunk03093024>(1)]
    public CPlugEntRecordData? RecordData => recordDataRef?.Resolve();

    /// <summary>
    /// Events occuring during the replay. Available in TMS and older games.
    /// </summary>
    /// <remarks>Applied with: 0x00E body chunk</remarks>
    [AppliedWithChunk<Chunk0309300E>]
    public CCtnMediaBlockEventTrackMania? Events => eventsRef?.Resolve();

    /// <summary>
    /// Events occuring during the replay. Available in TMS and older games.
    /// </summary>
    /// <remarks>Applied with: 0x010 body chunk</remarks>
    [AppliedWithChunk<Chunk03093010>]
    public CCtnMediaBlockUiTMSimpleEvtsDisplay? SimpleEventsDisplay => simpleEventsDisplayRef?.Resolve();

    /// <remarks>Applied with: 0x019 v0-0, 0x01A v0-0 body chunks</remarks>
    [AppliedWithChunk<Chunk03093019>(0, 0)]
    [AppliedWithChunk<Chunk0309301A>(0, 0)]
    public ImmutableList<CGameCtnMediaBlockScenery.Key>? SceneryVortexKeys => sceneryVortexKeys;

    /// <summary>
    /// Decrease in vortex radius per second, used to extend the last scenery key to the end of the replay.
    /// </summary>
    /// <remarks>Applied with: 0x019 v0-0, 0x01A v0-0 body chunks</remarks>
    [AppliedWithChunk<Chunk03093019>(0, 0)]
    [AppliedWithChunk<Chunk0309301A>(0, 0)]
    public float SceneryVortexShrinkSpeed => sceneryVortexShrinkSpeed;

    /// <summary>
    /// Recorded capturable scenery states, imported together with <see cref="SceneryCapturableCount"/>.
    /// </summary>
    /// <remarks>Applied with: 0x01A v0-0 body chunk</remarks>
    [AppliedWithChunk<Chunk0309301A>(0, 0)]
    public CPlugDataTape? SceneryCapturableDataTape => sceneryCapturableDataTapeRef?.Resolve();

    /// <remarks>Applied with: 0x01A v0-0 body chunk</remarks>
    [AppliedWithChunk<Chunk0309301A>(0, 0)]
    public int SceneryCapturableCount => sceneryCapturableCount;

    /// <summary>
    /// Ghost indices and replay times used to select the camera's player of interest.
    /// </summary>
    /// <remarks>Applied with: 0x01B v0-0 body chunk</remarks>
    [AppliedWithChunk<Chunk0309301B>(0, 0)]
    public ImmutableArray<PlayerOfInterest>? PlayersOfInterest => playersOfInterest;

    /// <remarks>Applied with: 0x01C body chunk</remarks>
    [AppliedWithChunk<Chunk0309301C>]
    public string? PlaygroundScript => playgroundScript;

    /// <summary>
    /// Timed snapshots of the replay's Manialink interface layers.
    /// </summary>
    /// <remarks>Applied with: 0x01D v0-0 body chunk</remarks>
    [AppliedWithChunk<Chunk0309301D>(0, 0)]
    public ImmutableList<InterfaceScriptInfo>? InterfaceScriptInfos => interfaceScriptInfos;

    /// <summary>
    /// Inputs (keyboard, pad, wheel) of the replay ONLY from TM1.0 (2003). For inputs stored in other games: see <see cref="CGameCtnGhost.Inputs"/> in <see cref="Ghosts"/>. TM2020 and Shootmania inputs are available in <see cref="Ghosts"/> in <see cref="CGameCtnGhost.PlayerInputs"/>. Can be null if <see cref="CGameCtnGhost.EventsDuration"/> is 0, which can happen when you save the replay in editor.
    /// </summary>
    /// <remarks>Applied with: 0x003 body chunk</remarks>
    [AppliedWithChunk<Chunk03093003>]
    [Obsolete("Use GetValidationGhost()?.Inputs instead.")]
    public ImmutableArray<IInput>? Inputs => GetValidationGhost()?.Inputs;

    [Obsolete("Use GetValidationGhost()?.EventsDuration instead.")]
    public TimeInt32? EventsDuration => GetValidationGhost()?.EventsDuration ?? pendingEventsDuration;

    /// <summary>
    /// Time-dependent mappings from recorded entity scene UIDs to indices in <see cref="Ghosts"/>.
    /// </summary>
    /// <remarks>Applied with: 0x026 v0-1 body chunk</remarks>
    [AppliedWithChunk<Chunk03093026>(0, 1)]
    public ImmutableList<EntDataSceneUIdsToGhost>? EntDataSceneUIdsToGhosts => entDataSceneUIdsToGhosts;

    /// <remarks>Applied with: 0x029 body chunk</remarks>
    [AppliedWithChunk<Chunk03093029>]
    public CGameCtnChallengeParameters? ChallengeParameters => challengeParametersRef?.Resolve();

    /// <remarks>Applied with: 0x01E body chunk</remarks>
    [AppliedWithChunk<Chunk0309301E>]
    public ImmutableList<CGameActionModel>? ActionModels => actionModels;

    /// <summary>
    /// Names assigned to the aggregated models in <see cref="ActionModels"/>.
    /// </summary>
    /// <remarks>Applied with: 0x01E v2+ body chunk</remarks>
    [AppliedWithChunk<Chunk0309301E>(2)]
    public ImmutableList<string>? ActionNames => actionNames;

    /// <summary>
    /// Relative filenames of action resources, separate from the aggregated model names in <see cref="ActionNames"/>.
    /// </summary>
    /// <remarks>Applied with: 0x01E v1+ body chunk</remarks>
    [AppliedWithChunk<Chunk0309301E>(1)]
    public ImmutableList<string>? ActionFileNames => actionFileNames;

    /// <summary>
    /// Item resources used by the replay.
    /// </summary>
    /// <remarks>Applied with: 0x020 body chunk</remarks>
    [AppliedWithChunk<Chunk03093020>]
    public ImmutableList<ItemResource>? Items => items;

    /// <summary>
    /// Additional item resources used to load the replay's item library, including any variant suffixes.
    /// </summary>
    /// <remarks>Applied with: 0x027 v0-1 body chunk</remarks>
    [AppliedWithChunk<Chunk03093027>(0, 1)]
    public ImmutableList<ItemResource>? AdditionalItems => additionalItems;

    /// <summary>
    /// Legacy anchored object records stored by version 0 of the scenery objects chunk.
    /// </summary>
    /// <remarks>Applied with: 0x01F v0-0 body chunk</remarks>
    [AppliedWithChunk<Chunk0309301F>(0, 0)]
    public ImmutableList<AnchoredObjectInfo>? AnchoredObjectInfos => anchoredObjectInfos;

    /// <remarks>Applied with: 0x01F v1-1 body chunk</remarks>
    [AppliedWithChunk<Chunk0309301F>(1, 1)]
    public CGameReplayObjectVisData? SceneryObjects => sceneryObjectsRef?.Resolve();

    /// <summary>
    /// Whether the playground's minimap is enabled during replay playback.
    /// </summary>
    /// <remarks>Applied with: 0x021 body chunk</remarks>
    [AppliedWithChunk<Chunk03093021>]
    public bool EnableMinimap => enableMinimap;

    /// <summary>
    /// Recorded Monoscreen camera values (STimedCamVal), preserved as 80-byte records in Maniaplanet or 84-byte records in Trackmania.
    /// </summary>
    /// <remarks>Applied with: 0x022 v0-0 body chunk</remarks>
    [AppliedWithChunk<Chunk03093022>(0, 0)]
    public byte[][]? CameraRecord => cameraRecord;

    /// <summary>
    /// Recorded visual states of empty cars, used to create <see cref="CGameCtnMediaBlockCheatEmptyCars"/> blocks.
    /// </summary>
    /// <remarks>Applied with: 0x023 v0-0 body chunk</remarks>
    [AppliedWithChunk<Chunk03093023>(0, 0)]
    public CPlugDataTape? EmptyCarsVisDataTape => emptyCarsVisDataTapeRef?.Resolve();

    /// <summary>
    /// Timed bump bonus events, including their durations and gold status.
    /// </summary>
    /// <remarks>Applied with: 0x023 v0-0 body chunk</remarks>
    [AppliedWithChunk<Chunk03093023>(0, 0)]
    public ImmutableList<BonusBumpKey>? BonusBumpKeys => bonusBumpKeys;

    /// <summary>
    /// Legacy recorded scenery-turret states. Newer replays use <see cref="RecordData"/>.
    /// </summary>
    /// <remarks>Applied with: 0x024 body chunk</remarks>
    [AppliedWithChunk<Chunk03093024>]
    public CPlugDataTape? SceneryTurretsDataTape => sceneryTurretsDataTapeRef?.Resolve();

    /// <summary>
    /// Time of day at the start of the replay, as a fraction of a day, or -1 when unspecified.
    /// </summary>
    /// <remarks>Applied with: 0x025 v0-0 body chunk</remarks>
    [AppliedWithChunk<Chunk03093025>(0, 0)]
    public float TimeOfDay => timeOfDay;

    /// <summary>
    /// Duration of the replay's day cycle. Zero keeps the time of day fixed.
    /// </summary>
    /// <remarks>Applied with: 0x025 v0-0 body chunk</remarks>
    [AppliedWithChunk<Chunk03093025>(0, 0)]
    public TimeInt32 DayPeriod => dayPeriod;

    /// <summary>
    /// Whether generated MediaTracker entity tracks are restricted to ghosts whose login matches <see cref="AuthorLogin"/>.
    /// </summary>
    /// <remarks>Applied with: 0x028 body chunk</remarks>
    [AppliedWithChunk<Chunk03093028>]
    public bool KeepOnlyAuthorGhosts => keepOnlyAuthorGhosts;

    protected internal override bool IsWriteSupported => false;

    public CGameCtnGhost? GetValidationGhost() => Ghosts is { Count: 1 } ? Ghosts[0] : null;

    public IEnumerable<CGameCtnGhost> GetGhosts(bool alsoInClips = true)
    {
        if (Ghosts is not null)
        {
            foreach (var ghost in Ghosts)
            {
                if (ghost is not null)
                {
                    yield return ghost;
                }
            }
        }

        if (alsoInClips && Clip is not null)
        {
            foreach (var ghost in Clip.GetGhosts())
            {
                yield return ghost;
            }
        }
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public async ValueTask<Gbx<CGameCtnChallenge>?> GetChallengeAsync(GbxReadSettings settings = default, CancellationToken cancellationToken = default)
    {
        if (ChallengeGbxData is null)
        {
            return null;
        }

#if NETSTANDARD2_0
        using var ms = new MemoryStream(ChallengeGbxData.Data);
#else
        await using var ms = new MemoryStream(ChallengeGbxData.Data);
#endif
        return await Gbx.ParseAsync<CGameCtnChallenge>(ms, settings, cancellationToken);
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public async Task<CGameCtnChallenge?> GetChallengeNodeAsync(GbxReadSettings settings = default, CancellationToken cancellationToken = default)
    {
        return (await GetChallengeAsync(settings, cancellationToken))?.Node;
    }

    public Gbx<CGameCtnChallenge>? GetChallengeHeader(GbxReadSettings settings = default)
    {
        if (ChallengeGbxData is null)
        {
            return null;
        }

        using var ms = new MemoryStream(ChallengeGbxData.Data);
        return Gbx.ParseHeader<CGameCtnChallenge>(ms, settings);
    }

    public CGameCtnChallenge? GetChallengeHeaderNode(GbxReadSettings settings = default)
    {
        return GetChallengeHeader(settings)?.Node;
    }

    private void PassInputsToValidationGhost()
    {
        if (pendingEventsDuration is null)
        {
            return;
        }

        var ghost = GetValidationGhost();

        if (ghost is null)
        {
            return;
        }

        ghost.EventsDuration = pendingEventsDuration.Value;
        ghost.Inputs = pendingInputs ?? [];

        pendingInputs = null;
        pendingEventsDuration = null;
    }

    private static byte[][] ReadRecords(GbxReader r, int recordSize)
    {
        var count = r.ReadInt32();

        if (count < 0)
        {
            throw new InvalidDataException("Negative replay record count.");
        }

        if (r.BaseStream.CanSeek && (long)count * recordSize > r.BaseStream.Length - r.BaseStream.Position)
        {
            throw new InvalidDataException("Replay records exceed the available data.");
        }

        var records = new byte[count][];
        for (var i = 0; i < count; i++)
        {
            records[i] = r.ReadData(recordSize);
        }

        return records;
    }

    public partial class HeaderChunk03093000 : IVersionable
    {
        public byte U01;

        private uint version;
        public int Version { get => (int)version; set => version = (uint)value; }

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            version = r.ReadUInt32();

            if (Version >= 4 && Version != 9999)
            {
                n.mapInfo = r.ReadIdent();
                n.time = r.ReadTimeInt32Nullable();
                n.playerNickname = r.ReadString();

                // Version 10000 derives the login from the nickname instead of storing it.
                if (Version >= 6 && Version != 10000)
                {
                    n.playerLogin = r.ReadString();
                }
            }

            if (Version > 7)
            {
                U01 = r.ReadByte();
                n.titleId = r.ReadId();
            }
        }
    }

    public partial class HeaderChunk03093001
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            n.xml = r.ReadString();
        }
    }

    public partial class HeaderChunk03093002 : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();
            n.authorVersion = r.ReadInt32();
            n.authorLogin = r.ReadString();
            n.authorNickname = r.ReadString();
            n.authorZone = r.ReadString();
            n.authorExtraInfo = r.ReadString();
        }
    }

    public partial class Chunk03093002
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            n.challengeGbxData = new RawData(r.ReadData(), exception: null);
        }
    }

    public partial class Chunk03093003
    {
        public int U01;

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            n.pendingEventsDuration = r.ReadTimeInt32();
            n.pendingInputs = null;

            if (n.pendingEventsDuration == TimeInt32.Zero)
            {
                n.PassInputsToValidationGhost();
                return;
            }

            U01 = r.ReadInt32();

            // All control names available in the game
            Span<string> inputNames = new string[r.ReadInt32()];

            for (var i = 0; i < inputNames.Length; i++)
            {
                // Maybe bindings
                r.ReadInt32();
                r.ReadInt32();

                inputNames[i] = r.ReadString(); // Input name
            }

            var numInputs = r.ReadInt32();

            var inputs = ImmutableArray.CreateBuilder<IInput>(numInputs);

            for (var i = 0; i < numInputs; i++)
            {
                var timestamp = r.ReadInt32();

                // Zero-timestamp entries are counted but have no control index or value.
                if (timestamp == 0)
                {
                    continue;
                }

                var time = TimeInt32.FromMilliseconds(timestamp - 10000);
                var controlNameIndex = r.ReadInt32();
                var data = r.ReadUInt32();

                var name = inputNames[controlNameIndex];

                inputs.Add(name switch
                {
                    "Accelerate" => new Accelerate(time, data != 0),
                    "Brake" => new Brake(time, data != 0),
                    "Steer (analog)" => new SteerOld(time, BitConverter.ToSingle(BitConverter.GetBytes(data), 0)),
                    "Steer left" => new SteerLeft(time, data != 0),
                    "Steer right" => new SteerRight(time, data != 0),
                    _ => new UnknownInput(time, name, data),
                });
            }

            inputs.Reverse(); // Inputs are originally reversed
            n.pendingInputs = inputs.Count == inputs.Capacity ? inputs.MoveToImmutable() : inputs.ToImmutable();

            n.PassInputsToValidationGhost();
        }
    }

    /// <summary>
    /// An opaque 8-byte record from the native SOldShowTime array.
    /// </summary>
    public readonly record struct OldShowTime(ulong Data);

    /// <summary>
    /// A camera focus change at a replay time, targeting an index in <see cref="Ghosts"/>.
    /// </summary>
    public readonly record struct PlayerOfInterest(int GhostIndex, TimeInt32 Time);

    /// <summary>
    /// An item resource name and its corresponding skin reference.
    /// </summary>
    /// <param name="Name">The item resource name, including any variant suffix.</param>
    /// <param name="Skin">The skin reference, or <see cref="PackDesc.Empty"/> when unspecified.</param>
    public readonly record struct ItemResource(string Name, PackDesc Skin);

    private static ImmutableList<ItemResource> CreateItemResources(string[] names, PackDesc[]? skins)
    {
        if (skins is not null && skins.Length != names.Length)
        {
            throw new InvalidDataException("Item skin and name counts differ.");
        }

        var resources = ImmutableList.CreateBuilder<ItemResource>();
        for (var i = 0; i < names.Length; i++)
        {
            resources.Add(new ItemResource(names[i], skins?[i] ?? PackDesc.Empty));
        }
        return resources.ToImmutable();
    }

    public partial class Chunk03093004 : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();

            n.ghosts = r.ReadListNodeRef_deprec<CGameCtnGhost>();
            n.PassInputsToValidationGhost();

            foreach (var ghost in n.ghosts)
            {
                ghost?.GhostVersion = Version;
            }

            n.recordDuration = r.ReadTimeInt32();
            n.oldShowTimes = ImmutableArray.Create(r.ReadArray<OldShowTime>());
        }
    }

    public partial class Chunk03093005
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            n.oldCutKeys = ReadRecords(r, 68);
        }
    }

    public partial class Chunk03093007
    {
        /// <summary>
        /// Unused legacy value.
        /// </summary>
        public int U01 = -1;

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            U01 = r.ReadInt32();
        }
    }

    public partial class Chunk03093008
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            var exeVersion = r.ReadString();
            n.oldCutKeys2 = ReadRecords(r, 72);
            n.GetValidationGhost()?.Validate_ExeVersion = exeVersion;
        }
    }

    public partial class Chunk03093009
    {
        public float[]? U01;

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            U01 = r.ReadArray<float>();
        }
    }

    public partial class Chunk0309300A
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            n.tracks = r.ReadListNodeRef_deprec<CGameCtnMediaTrack>();
        }
    }

    public partial class Chunk0309300B
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            r.ReadNodeRef(ref n.humanTimeToGameTimeFuncRef);
        }
    }

    public partial class Chunk0309300C
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            r.ReadNodeRef(ref n.clipRef);
        }
    }

    public partial class Chunk0309300D
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            var eventsDuration = r.ReadTimeInt32();

            var ghost = n.GetValidationGhost();
            if (ghost is not null)
            {
                ghost.EventsDuration = eventsDuration;
                ghost.Inputs = [];
            }

            if (eventsDuration == TimeInt32.Zero)
            {
                return;
            }

            var inputStoreVersion = r.ReadInt32();

            var inputNames = r.ReadArrayId();

            var numInputs = r.ReadInt32();
            var inputCountLimit = r.ReadInt32();
            if (ghost is not null)
            {
                ghost.InputStoreVersion = inputStoreVersion;
                ghost.InputCountLimit = inputCountLimit;
            }

            if (numInputs == 0)
            {
                return;
            }

            Span<IInput> inputs = new IInput[numInputs];

            // 9 bytes per entry: 4 for time, 1 for name index, 4 for data
            var inputDataLength = numInputs * 9;

#if NET5_0_OR_GREATER
            Span<byte> inputData = inputDataLength > 8192 ? new byte[inputDataLength] : stackalloc byte[inputDataLength];
            r.BaseStream.ReadExactly(inputData);
#else
            Span<byte> inputData = r.ReadBytes(inputDataLength);
#endif

            for (var i = 0; i < numInputs; i++)
            {
                var time = TimeInt32.FromMilliseconds(BinaryPrimitives.ReadInt32LittleEndian(inputData.Slice(i * 9, 4)) - 100000);
                var inputNameIndex = inputData[i * 9 + 4];
                var data = BinaryPrimitives.ReadUInt32LittleEndian(inputData.Slice(i * 9 + 5, 4));

                var name = inputNames[inputNameIndex];

                inputs[i] = NET.Inputs.Input.Parse(time, name, data);
            }

            ghost?.Inputs = inputs.ToImmutableArray();
        }
    }

    public partial class Chunk0309300E
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            r.ReadNodeRef(ref n.eventsRef);
        }
    }

    public partial class Chunk0309300F
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            var exeVersion = r.ReadString();
            var exeChecksum = r.ReadUInt32();
            var osKind = r.ReadInt32();
            var cpuKind = (CGameCtnGhost.EKindCpu)r.ReadInt32();
            var raceSettings = r.ReadString();

            if (n.GetValidationGhost() is { } ghost)
            {
                ghost.Validate_ExeVersion = exeVersion;
                ghost.Validate_ExeChecksum = exeChecksum;
                ghost.Validate_OsKind = osKind;
                ghost.Validate_CpuKind = cpuKind;
                ghost.Validate_RaceSettings = raceSettings;
            }
        }
    }

    public partial class Chunk03093010
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            r.ReadNodeRef(ref n.simpleEventsDisplayRef);
        }
    }

    public partial class Chunk03093011
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {

        }
    }

    public partial class Chunk03093012
    {
        // Ignored by native readers; the matching ghost chunk writes random padding or zero.
        public int U01;

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            U01 = r.ReadInt32();
            var securityKey = r.ReadUInt128();
            n.GetValidationGhost()?.SecurityKey128 = securityKey;
        }
    }

    public partial class Chunk03093013
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            n.oldCutKeys2 = ReadRecords(r, 72);
        }
    }

    public partial class Chunk03093014
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            n.ghosts = r.ReadListNodeRef_deprec<CGameCtnGhost>();
            n.PassInputsToValidationGhost();
            n.recordDuration = r.ReadTimeInt32(); // Modern native writers store zero.
            n.oldShowTimes = ImmutableArray.Create(r.ReadArray<OldShowTime>());
        }
    }

    public partial class Chunk03093015
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            r.ReadNodeRef(ref n.clipRef);
        }
    }

    public partial class Chunk03093016
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            r.ReadNodeRef(ref n.bulletVisDataTapeRef);
            r.ReadNodeRef(ref n.charVisDataTapeRef);
            r.ReadNodeRef(ref n.arenaVisDataTapeRef);
        }
    }

    public partial class Chunk03093017
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            r.ReadEncapsulated(inner =>
            {
                inner.ReadNodeRef(ref n.bulletVisDataTapeRef);
                inner.ReadNodeRef(ref n.charVisDataTapeRef);
                inner.ReadNodeRef(ref n.arenaVisDataTapeRef);
            });
        }
    }

    public partial class Chunk03093018
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            n.titleId = r.ReadId();
            n.authorVersion = r.ReadInt32();
            n.authorLogin = r.ReadString();
            n.authorNickname = r.ReadString();
            n.authorZone = r.ReadString();
            n.authorExtraInfo = r.ReadString();
        }
    }

    public partial class Chunk03093019 : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();
            if (Version >= 1)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            n.sceneryVortexKeys = r.ReadArrayReadable<CGameCtnMediaBlockScenery.Key>().ToImmutableList();
            n.sceneryVortexShrinkSpeed = r.ReadSingle();
        }
    }

    public partial class Chunk0309301A
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            base.Read(n, r);
            r.ReadNodeRef(ref n.sceneryCapturableDataTapeRef);
            n.sceneryCapturableCount = r.ReadInt32();
        }
    }

    public partial class Chunk0309301B : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();
            if (Version >= 1)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            n.playersOfInterest = r.ReadArray<Int2>()
                .Select(x => new PlayerOfInterest(x.X, new TimeInt32(x.Y)))
                .ToImmutableArray();
        }
    }

    public partial class Chunk0309301C : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();
            n.playgroundScript = r.ReadString(); // CampaignSolo
        }
    }

    public partial class Chunk0309301D : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();

            if (Version >= 1)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            n.interfaceScriptInfos = ImmutableList.Create(r.ReadArrayReadable<InterfaceScriptInfo>());
        }
    }

    public partial class Chunk0309301E : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();

            var count = r.ReadInt32();
            if (count < 0)
            {
                throw new InvalidDataException("Negative action model count.");
            }

            var models = ImmutableList.CreateBuilder<CGameActionModel>();
            for (var i = 0; i < count; i++)
            {
                // Inline CMwNod archives have neither a class ID nor a node reference index.
                models.Add(r.ReadNode<CGameActionModel>()!);
            }
            n.actionModels = models.ToImmutable();

            if (Version >= 2)
            {
                n.actionNames = r.ReadArrayString().ToImmutableList();
                if (n.actionNames.Count != count)
                {
                    throw new InvalidDataException("Action name and model counts differ.");
                }
            }
            else
            {
                n.actionNames = Enumerable.Range(0, count)
                    .Select(i => "*Replay*\\Action" + i.ToString("D3", CultureInfo.InvariantCulture))
                    .ToImmutableList();
            }

            if (Version >= 1)
            {
                n.actionFileNames = r.ReadArrayString().ToImmutableList();
            }
        }
    }

    public partial class Chunk0309301F : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();
            switch (Version)
            {
                case 0:
                    n.anchoredObjectInfos = r.ReadArrayReadable<AnchoredObjectInfo>().ToImmutableList();
                    break;
                case 1:
                    r.ReadNodeRef(ref n.sceneryObjectsRef);
                    break;
                default:
                    throw new ChunkVersionNotSupportedException(Version);
            }
        }
    }

    public partial class Chunk03093020 : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();
            var names = r.ReadArrayString();
            var skins = Version == 0 ? null : r.ReadArrayPackDesc();
            n.items = CreateItemResources(names, skins);
        }
    }

    public partial class Chunk03093021 : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();
            n.enableMinimap = r.ReadBoolean();
        }
    }

    public partial class Chunk03093022 : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();
            if (Version >= 1)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            // Bounded skippable payload determines the width
            var count = r.ReadInt32();
            var remaining = r.BaseStream.Length - r.BaseStream.Position;
            if (count < 0 || (count == 0 ? remaining != 0 : remaining != (long)count * 80 && remaining != (long)count * 84))
            {
                throw new InvalidDataException("Invalid timed camera record size or count.");
            }

            var recordSize = count == 0 ? 80 : (int)(remaining / count);
            n.cameraRecord = new byte[count][];
            for (var i = 0; i < count; i++)
            {
                n.cameraRecord[i] = r.ReadData(recordSize);
            }
        }
    }

    public partial class Chunk03093023 : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();
            if (Version >= 1)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            r.ReadNodeRef(ref n.emptyCarsVisDataTapeRef);
            n.bonusBumpKeys = r.ReadArrayReadable<BonusBumpKey>().ToImmutableList();
        }
    }

    public partial class Chunk03093024 : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();
            r.ReadNodeRef(ref n.sceneryTurretsDataTapeRef);

            if (Version >= 1)
            {
                r.ReadNodeRef(ref n.recordDataRef);
            }
        }
    }

    public partial class Chunk03093025 : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();
            if (Version >= 1)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            n.timeOfDay = r.ReadSingle();
            n.dayPeriod = r.ReadTimeInt32();
        }
    }

    public partial class Chunk03093026 : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();
            if (Version is < 0 or > 1)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            n.entDataSceneUIdsToGhosts = r.ReadArrayReadable<EntDataSceneUIdsToGhost>().ToImmutableList();

            if (Version >= 1)
            {
                // Scene UIDs follow all three-field mapping records in a separate pass.
                foreach (var entData in n.entDataSceneUIdsToGhosts)
                {
                    entData.SceneUId = r.ReadInt32();
                }
            }
        }
    }

    public partial class Chunk03093027 : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();
            if (Version is < 0 or > 1)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            var names = r.ReadArrayString();
            var skins = r.ReadArrayPackDesc();
            n.additionalItems = CreateItemResources(names, skins);
        }
    }

    public partial class Chunk03093028
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            n.keepOnlyAuthorGhosts = r.ReadBoolean();
        }
    }

    public partial class Chunk03093029
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            r.ReadNodeRef(ref n.challengeParametersRef);
        }
    }

    public partial class InterfaceScriptInfo
    {
        private string[]? manialinks;
        private TimeInt32 time;

        /// <summary>
        /// Manialink XML pages of the interface layers at <see cref="Time"/>. An empty array clears the interface.
        /// </summary>
        public string[]? Manialinks { get => manialinks; set => manialinks = value; }

        /// <summary>
        /// Time since replay recording started at which these interface layers apply.
        /// </summary>
        public TimeInt32 Time { get => time; set => time = value; }

        [Obsolete("Use Manialinks instead.")]
        public string[]? U01 { get => manialinks; set => manialinks = value; }

        [Obsolete("Use Time instead.")]
        public int U02 { get => time.TotalMilliseconds; set => time = new TimeInt32(value); }

        public virtual void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.ArrayString(ref manialinks);
            rw.TimeInt32(ref time);
        }
    }

    public partial class EntDataSceneUIdsToGhost
    {
        private TimeInt32 time;
        private int entDataSceneUId;
        private int ghostIndex;

        /// <summary>
        /// Replay time from which this mapping applies.
        /// </summary>
        public TimeInt32 Time { get => time; set => time = value; }

        /// <summary>
        /// Scene UID of the entity in the recorded entity data.
        /// </summary>
        public int EntDataSceneUId { get => entDataSceneUId; set => entDataSceneUId = value; }

        /// <summary>
        /// Index of the corresponding ghost in the replay's ghost list.
        /// </summary>
        public int GhostIndex { get => ghostIndex; set => ghostIndex = value; }

        /// <summary>
        /// Scene UID preferred for matching when both entities have one. Version 0 uses 0x0FF00000 for an unknown UID.
        /// </summary>
        /// <remarks>Applied with: 0x026 v1+ body chunk</remarks>
        [AppliedWithChunk<Chunk03093026>(1)]
        public int SceneUId { get; set; } = 0x0FF00000;

        [Obsolete("Use Time instead.")]
        public int U01 { get => time.TotalMilliseconds; set => time = new TimeInt32(value); }

        [Obsolete("Use EntDataSceneUId instead.")]
        public int U02 { get => entDataSceneUId; set => entDataSceneUId = value; }

        [Obsolete("Use GhostIndex instead.")]
        public int U03 { get => ghostIndex; set => ghostIndex = value; }

        [Obsolete("Use SceneUId instead.")]
        public int U04 { get => SceneUId; set => SceneUId = value; }

        public virtual void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeInt32(ref time);
            rw.Int32(ref entDataSceneUId);
            rw.Int32(ref ghostIndex);
        }
    }

    public sealed class AnchoredObjectInfo : IReadable, IDeepCloneable
    {
        public bool U01;
        public int U02;
        public float U03;
        public float U04;
        public float U05;
        public int U06;

        object IDeepCloneable.DeepClone(DeepCloneContext context)
        {
            var clone = (AnchoredObjectInfo)MemberwiseClone();
            context.Register(this, clone);
            return clone;
        }

        public void Read(GbxReader r, int v = 0)
        {
            U01 = r.ReadBoolean();
            U02 = r.ReadInt32();
            U03 = r.ReadSingle();
            U04 = r.ReadSingle();
            U05 = r.ReadSingle();
            U06 = r.ReadInt32();
        }
    }

    public sealed class BonusBumpKey : IReadable, IDeepCloneable
    {
        /// <summary>
        /// Time since replay recording started at which the bump bonus begins.
        /// </summary>
        public TimeSingle Time { get; set; }

        /// <summary>
        /// Duration of the bump bonus event.
        /// </summary>
        public TimeInt32 Duration { get; set; }

        /// <summary>
        /// Whether the event uses the stronger gold bonus.
        /// </summary>
        public bool IsGold { get; set; }

        [Obsolete("Use Duration instead.")]
        public int U01 { get => Duration.TotalMilliseconds; set => Duration = new TimeInt32(value); }

        [Obsolete("Use IsGold instead.")]
        public bool U02 { get => IsGold; set => IsGold = value; }

        object IDeepCloneable.DeepClone(DeepCloneContext context)
        {
            var clone = (BonusBumpKey)MemberwiseClone();
            context.Register(this, clone);
            return clone;
        }

        public void Read(GbxReader r, int v = 0)
        {
            Time = r.ReadTimeSingle();
            Duration = r.ReadTimeInt32();
            IsGold = r.ReadBoolean();
        }
    }
}
