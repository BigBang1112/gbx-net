using GBX.NET.Inputs;
using System.Collections.Immutable;

namespace GBX.NET.Engines.Game;

[WriteNotSupported]
public partial class CGameCtnReplayRecord
{
    private byte[]? challengeData;
    private CGameCtnChallenge? challenge;

    /// <summary>
    /// Map UID, environment, and author login of the map the replay orients in.
    /// </summary>
    public Ident? MapInfo { get; private set; }

    /// <summary>
    /// The record time.
    /// </summary>
    public TimeInt32? Time { get; private set; }

    /// <summary>
    /// Nickname of the record owner.
    /// </summary>
    [SupportsFormatting]
    public string? PlayerNickname { get; private set; }

    /// <summary>
    /// Login of the record owner.
    /// </summary>
    public string? PlayerLogin { get; private set; }

    /// <summary>
    /// Title pack the replay orients in.
    /// </summary>
    public string? TitleId { get; private set; }

    /// <summary>
    /// XML replay information.
    /// </summary>
    public string? Xml { get; private set; }

    public int? AuthorVersion { get; private set; }

    /// <summary>
    /// Login of the replay creator.
    /// </summary>
    public string? AuthorLogin { get; private set; }

    /// <summary>
    /// Nickname of the replay creator.
    /// </summary>
    [SupportsFormatting]
    public string? AuthorNickname { get; private set; }

    /// <summary>
    /// Zone of the replay creator.
    /// </summary>
    public string? AuthorZone { get; private set; }

    public string? AuthorExtraInfo { get; private set; }

    /// <summary>
    /// The map the replay orients in. Null if only the header was read.
    /// </summary>
    public CGameCtnChallenge? Challenge
    {
        get
        {
            if (challengeData is null)
            {
                return null;
            }

            if (challenge is not null)
            {
                return challenge;
            }

            using var ms = new MemoryStream(challengeData);
            return challenge = Gbx.ParseNode<CGameCtnChallenge>(ms);
        }
    }

    /// <summary>
    /// Ghosts in the replay. Null if only the header was read.
    /// </summary>
    /// <remarks>Some ghosts can be considered as <see cref="CGameCtnMediaBlockGhost"/>. See <see cref="Clip"/>.</remarks>
    public ImmutableList<CGameCtnGhost>? Ghosts { get; private set; }

    /// <summary>
    /// MediaTracker clip of the replay.
    /// </summary>
    public CGameCtnMediaClip? Clip { get; private set; }

    public CPlugEntRecordData? RecordData { get; private set; }

    /// <summary>
    /// Events occuring during the replay. Available in TMS and older games.
    /// </summary>
    public CCtnMediaBlockEventTrackMania? Events { get; private set; }

    /// <summary>
    /// Events occuring during the replay. Available in TMS and older games.
    /// </summary>
    public CCtnMediaBlockUiTMSimpleEvtsDisplay? SimpleEventsDisplay { get; private set; }

    /// <summary>
    /// Duration of events in the replay (range of detected inputs). This can be <see cref="TimeInt32.Zero"/> if the replay was driven in editor and null if driven in TMU, TMUF, TMTurbo, TM2 and TM2020.
    /// </summary>
    public TimeInt32? EventsDuration { get; private set; }

    public ImmutableList<CGameCtnMediaBlockScenery.Key>? SceneryVortexKeys { get; private set; }
    public int SceneryCapturableCount { get; private set; }
    public string? PlaygroundScript { get; private set; }
    public ImmutableList<InterfaceScriptInfo>? InterfaceScriptInfos { get; private set; }

    /// <summary>
    /// Inputs (keyboard, pad, wheel) of the replay from TM1.0, TMO, Sunrise and ESWC. For inputs stored in TMU, TMUF, TMTurbo and TM2: see <see cref="CGameCtnGhost.Inputs"/> in <see cref="Ghosts"/>. TM2020 and Shootmania inputs are available in <see cref="Ghosts"/> in <see cref="CGameCtnGhost.PlayerInputs"/>. Can be null if <see cref="EventsDuration"/> is 0, which can happen when you save the replay in editor.
    /// </summary>
    public ImmutableList<IInput>? Inputs { get; private set; }

    public CGameCtnChallengeParameters? ChallengeParameters { get; private set; }

    public IEnumerable<CGameCtnGhost> GetGhosts(bool alsoInClips = true)
    {
        if (Ghosts is not null)
        {
            foreach (var ghost in Ghosts)
            {
                yield return ghost;
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
        if (challengeData is null)
        {
            return null;
        }

#if NETSTANDARD2_0
        using var ms = new MemoryStream(challengeData);
#else
        await using var ms = new MemoryStream(challengeData);
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
        if (challengeData is null)
        {
            return null;
        }

        using var ms = new MemoryStream(challengeData);
        return Gbx.ParseHeader<CGameCtnChallenge>(ms, settings);
    }

    public CGameCtnChallenge? GetChallengeHeaderNode(GbxReadSettings settings = default)
    {
        return GetChallengeHeader(settings)?.Node;
    }

    public partial class HeaderChunk03093000 : IVersionable
    {
        public byte U01;

        private uint version;
        public int Version { get => (int)version; set => version = (uint)value; }

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            version = r.ReadUInt32();

            // This is some decompiled scuffness IDK
            if (version >= 4 && version != 9999)
            {
                n.MapInfo = r.ReadIdent();
                n.Time = r.ReadTimeInt32Nullable();
                n.PlayerNickname = r.ReadString();

                if (version >= 6)
                {
                    n.PlayerLogin = r.ReadString();
                }
            }

            // capital Version here is important
            if (Version > 7)
            {
                U01 = r.ReadByte();
                n.TitleId = r.ReadId();
            }
        }
    }

    public partial class HeaderChunk03093001
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            n.Xml = r.ReadString();
        }
    }

    public partial class HeaderChunk03093002 : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();
            n.AuthorVersion = r.ReadInt32();
            n.AuthorLogin = r.ReadString();
            n.AuthorNickname = r.ReadString();
            n.AuthorZone = r.ReadString();
            n.AuthorExtraInfo = r.ReadString();
        }
    }

    public partial class Chunk03093002
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            n.challengeData = r.ReadData();
        }
    }

    public partial class Chunk03093003
    {
        public int U01;
        public int U02;

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            n.EventsDuration = r.ReadTimeInt32();

            if (n.EventsDuration == TimeInt32.Zero)
            {
                return;
            }

            U01 = r.ReadInt32();

            // All control names available in the game
            var inputNames = new string[r.ReadInt32()];
            
            for (var i = 0; i < inputNames.Length; i++)
            {
                // Maybe bindings
                r.ReadInt32();
                r.ReadInt32();

                inputNames[i] = r.ReadString(); // Input name
            }

            var numInputs = r.ReadInt32() - 1;

            var inputs = ImmutableList.CreateBuilder<IInput>();

            for (var i = 0; i < numInputs; i++)
            {
                var time = TimeInt32.FromMilliseconds(r.ReadInt32() - 10000);
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
            n.Inputs = inputs.ToImmutable(); // inputs are actually stored in the first ghost object

            U02 = r.ReadInt32();
        }
    }

    public partial class Chunk03093004 : IVersionable
    {
        public int Version { get; set; }

        public int U01;
        public ImmutableArray<long> U02;

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();

            n.Ghosts = ImmutableList.Create(r.ReadArrayNodeRef_deprec<CGameCtnGhost>())!;

            U01 = r.ReadInt32(); // CGameReplayObjectVisData something, millisecond length of something (usually record time + 0.5s)
            U02 = ImmutableArray.Create(r.ReadArray<long>()); // SRecordUnit/SOldShowTime array
        }
    }

    public partial class Chunk03093005
    {
        public int U01;

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            U01 = r.ReadInt32(); // SOldCutKey
        }
    }

    public partial class Chunk03093007
    {
        public int U01;

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            U01 = r.ReadInt32();
        }
    }

    public partial class Chunk03093008
    {
        public byte[][]? U01;

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            var exeVersion = r.ReadString();

            U01 = new byte[r.ReadInt32()][]; // SOldCutKey2
            for (var i = 0; i < U01.Length; i++)
            {
                U01[i] = r.ReadBytes(72);
            }

            var ghost = n.Ghosts?.FirstOrDefault();

            if (ghost is not null)
            {
                ghost.Validate_ExeVersion = exeVersion;
            }
        }
    }

    public partial class Chunk0309300C
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            n.Clip = r.ReadNodeRef<CGameCtnMediaClip>();
        }
    }

    public partial class Chunk0309300D
    {
        public int U01;
        public int U02;

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            n.EventsDuration = r.ReadTimeInt32();

            if (n.EventsDuration == TimeInt32.Zero)
            {
                return;
            }

            U01 = r.ReadInt32();

            var inputNames = r.ReadArrayId();

            var numInputs = r.ReadInt32();
            U02 = r.ReadInt32();

            var inputs = ImmutableList.CreateBuilder<IInput>();

            for (var i = 0; i < numInputs; i++)
            {
                var time = TimeInt32.FromMilliseconds(r.ReadInt32() - 100000);
                var inputNameIndex = r.ReadByte();
                var data = r.ReadUInt32();

                var name = inputNames[inputNameIndex];

                inputs.Add(NET.Inputs.Input.Parse(time, name, data));
            }

            n.Inputs = inputs.ToImmutable(); // inputs are actually stored in the first ghost object
        }
    }

    public partial class Chunk0309300E
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            n.Events = r.ReadNodeRef<CCtnMediaBlockEventTrackMania>();
        }
    }

    public partial class Chunk0309300F
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            var exeVersion = r.ReadString();
            var exeChecksum = r.ReadUInt32();
            var osKind = r.ReadInt32();
            var cpuKind = r.ReadInt32();
            var raceSettings = r.ReadString();

            var ghost = n.Ghosts?.FirstOrDefault();

            if (ghost is null)
            {
                return;
            }

            ghost.Validate_ExeVersion = exeVersion;
            ghost.Validate_ExeChecksum = exeChecksum;
            ghost.Validate_OsKind = osKind;
            ghost.Validate_CpuKind = cpuKind;
            ghost.Validate_RaceSettings = raceSettings;
        }
    }

    public partial class Chunk03093010
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            n.SimpleEventsDisplay = r.ReadNodeRef<CCtnMediaBlockUiTMSimpleEvtsDisplay>();
        }
    }

    public partial class Chunk03093011
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {

        }
    }

    public partial class Chunk03093014
    {
        public ImmutableArray<long> U01;

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            n.Ghosts = ImmutableList.Create(r.ReadArrayNodeRef_deprec<CGameCtnGhost>())!;
            r.ReadInt32(); // always zero
            U01 = ImmutableArray.Create(r.ReadArray<long>()); // SRecordUnit/SOldShowTime array
        }
    }

    public partial class Chunk03093015
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            n.Clip = r.ReadNodeRef<CGameCtnMediaClip>();
        }
    }

    public partial class Chunk03093018
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            n.TitleId = r.ReadId();
            n.AuthorVersion = r.ReadInt32();
            n.AuthorLogin = r.ReadString();
            n.AuthorNickname = r.ReadString();
            n.AuthorZone = r.ReadString();
            n.AuthorExtraInfo = r.ReadString();
        }
    }

    public partial class Chunk0309301A
    {
        public float U01;
        public CPlugDataTape? U02;

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            r.ReadInt32(); // always 0

            var sceneryVortexKeys = ImmutableList.CreateBuilder<CGameCtnMediaBlockScenery.Key>();

            for (int i = 0; i < r.ReadInt32(); i++)
            {
                sceneryVortexKeys.Add(new CGameCtnMediaBlockScenery.Key
                {
                    Time = r.ReadTimeSingle(),
                    U01 = r.ReadSingle(),
                    U02 = r.ReadSingle(),
                    U03 = r.ReadSingle()
                });
            }

            n.SceneryVortexKeys = sceneryVortexKeys.ToImmutable();

            U01 = r.ReadSingle();
            U02 = r.ReadNodeRef<CPlugDataTape>();

            n.SceneryCapturableCount = r.ReadInt32();
        }
    }

    public partial class Chunk0309301B : IVersionable
    {
        public int Version { get; set; }

        public Int2[]? U01;

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();
            U01 = r.ReadArray<Int2>();
        }
    }

    public partial class Chunk0309301C : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();
            n.PlaygroundScript = r.ReadString(); // CampaignSolo
        }
    }

    public partial class Chunk0309301D : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();

            if (Version > 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            n.InterfaceScriptInfos = ImmutableList.Create(r.ReadArrayReadable<InterfaceScriptInfo>());
        }
    }

    public partial class Chunk0309301F
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

    public partial class Chunk03093021 : IVersionable
    {
        public int Version { get; set; }

        public bool U01;

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();
            U01 = r.ReadBoolean();
        }
    }

    public partial class Chunk03093024 : IVersionable
    {
        public int Version { get; set; }

        public CPlugDataTape? U01;

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();
            U01 = r.ReadNodeRef<CPlugDataTape>(); // nod
            n.RecordData = r.ReadNodeRef<CPlugEntRecordData>();
        }
    }

    public partial class Chunk03093025 : IVersionable
    {
        public int Version { get; set; }

        public float U01;
        public int U02;

        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            Version = r.ReadInt32();
            U01 = r.ReadSingle();
            U02 = r.ReadInt32();
        }
    }

    public partial class Chunk03093029
    {
        public override void Read(CGameCtnReplayRecord n, GbxReader r)
        {
            n.ChallengeParameters = r.ReadNodeRef<CGameCtnChallengeParameters>();
        }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class InterfaceScriptInfo;
}
