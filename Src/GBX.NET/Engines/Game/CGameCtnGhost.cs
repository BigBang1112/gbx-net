using GBX.NET.Inputs;
using System.Numerics;

namespace GBX.NET.Engines.Game;

/// <summary>
/// A ghost.
/// </summary>
/// <remarks>ID: 0x03092000</remarks>
[Node(0x03092000)]
[NodeExtension("Ghost")]
public partial class CGameCtnGhost : CGameGhost
{
    #region Fields

    private Ident playerModel;
    private IList<FileRef>? skinPackDescs;
    private string? ghostNickname;
    private string? ghostLogin;
    private string? ghostAvatarName;
    private string? recordingContext;
    private CPlugEntRecordData? recordData;
    private string? ghostTrigram;
    private string? ghostZone;
    private string? ghostUid;
    private TimeInt32? raceTime;
    private int? respawns;
    private Vec3 lightTrailColor = (1, 0, 0);
    private int? stuntScore;
    private Checkpoint[]? checkpoints;
    private TimeInt32 eventsDuration;
    private ControlEntry[]? controlEntries;
    private string? validate_ExeVersion;
    private uint validate_ExeChecksum;
    private int validate_OsKind;
    private int validate_CpuKind;
    private string? validate_RaceSettings;
    private string? validate_ChallengeUid;
    private string? validate_TitleId;
    private bool hasBadges;
    private int? badgeVersion;
    private Badge? badge;
    private string? skinFile;
    private string? ghostClubTag;
    private PlayerInputData[]? playerInputs;
    private IReadOnlyCollection<IInput>? inputs;

    #endregion

    #region Properties

    [NodeMember]
    [AppliedWithChunk<Chunk03092000>]
    [AppliedWithChunk<Chunk03092003>]
    [AppliedWithChunk<Chunk03092006>]
    [AppliedWithChunk<Chunk0309200D>]
    [AppliedWithChunk<Chunk03092018>]
    public Ident PlayerModel
    {
        get
        {
            DiscoverChunk<Chunk03092000>();
            return playerModel;
        }
        set
        {
            DiscoverChunk<Chunk03092000>();
            playerModel = value;
        }
    }

    [NodeMember]
    [AppliedWithChunk<Chunk03092000>]
    [AppliedWithChunk<Chunk03092017>]
    public IList<FileRef>? SkinPackDescs
    {
        get
        {
            DiscoverChunks<Chunk03092000, Chunk03092017>();
            return skinPackDescs;
        }
        set
        {
            DiscoverChunks<Chunk03092000, Chunk03092017>();
            skinPackDescs = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [SupportsFormatting]
    [AppliedWithChunk<Chunk03092000>]
    [AppliedWithChunk<Chunk03092003>]
    [AppliedWithChunk<Chunk03092006>]
    [AppliedWithChunk<Chunk0309200D>]
    [AppliedWithChunk<Chunk03092015>]
    [AppliedWithChunk<Chunk03092017>]
    public string? GhostNickname
    {
        get
        {
            DiscoverChunks<Chunk03092000, Chunk03092017>();
            return ghostNickname;
        }
        set
        {
            DiscoverChunks<Chunk03092000, Chunk03092017>();
            ghostNickname = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03092000>]
    [AppliedWithChunk<Chunk03092017>]
    public string? GhostAvatarName
    {
        get
        {
            DiscoverChunks<Chunk03092000, Chunk03092017>();
            return ghostAvatarName;
        }
        set
        {
            DiscoverChunks<Chunk03092000, Chunk03092017>();
            ghostAvatarName = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03092000>(sinceVersion: 2)]
    public string? RecordingContext
    {
        get
        {
            DiscoverChunk<Chunk03092000>();
            return recordingContext;
        }
        set
        {
            DiscoverChunk<Chunk03092000>();
            recordingContext = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03092005>]
    public TimeInt32? RaceTime
    {
        get
        {
            DiscoverChunk<Chunk03092005>();
            return raceTime;
        }
        set
        {
            DiscoverChunk<Chunk03092005>();
            raceTime = value;
        }
    }

    [NodeMember]
    [AppliedWithChunk<Chunk03092008>]
    public int? Respawns
    {
        get
        {
            DiscoverChunk<Chunk03092008>();
            return respawns;
        }
        set
        {
            DiscoverChunk<Chunk03092008>();
            respawns = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03092000>]
    [AppliedWithChunk<Chunk03092007>]
    [AppliedWithChunk<Chunk03092009>]
    public Vec3 LightTrailColor
    {
        get
        {
            DiscoverChunk<Chunk03092009>();
            return lightTrailColor;
        }
        set
        {
            DiscoverChunk<Chunk03092009>();
            lightTrailColor = value;
        }
    }

    [NodeMember]
    [AppliedWithChunk<Chunk0309200A>]
    public int? StuntScore
    {
        get
        {
            DiscoverChunk<Chunk0309200A>();
            return stuntScore;
        }
        set
        {
            DiscoverChunk<Chunk0309200A>();
            stuntScore = value;
        }
    }

    /// <summary>
    /// Checkpoints driven by the ghost. In some cases, they may not be in order.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03092004>]
    [AppliedWithChunk<Chunk0309200B>]
    public Checkpoint[]? Checkpoints
    {
        get
        {
            DiscoverChunks<Chunk03092004, Chunk0309200B>();
            return checkpoints;
        }
        set
        {
            DiscoverChunks<Chunk03092004, Chunk0309200B>();
            checkpoints = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0309200F>]
    public string? GhostLogin { get => ghostLogin; set => ghostLogin = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk03092003>]
    [AppliedWithChunk<Chunk03092006>]
    [AppliedWithChunk<Chunk0309200D>]
    public string? SkinFile { get => skinFile; set => skinFile = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0309200E>]
    public string? GhostUid { get => ghostUid; set => ghostUid = value; }

    /// <summary>
    /// Duration of events in the ghost (range of detected inputs). This can be 0 if the ghost was driven in editor.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03092011>]
    [AppliedWithChunk<Chunk03092019>]
    [AppliedWithChunk<Chunk03092025>]
    public TimeInt32 EventsDuration
    {
        get
        {
            DiscoverChunk<Chunk03092025>();
            return eventsDuration;
        }
        set
        {
            DiscoverChunk<Chunk03092025>();
            eventsDuration = value;
        }
    }

    /// <summary>
    /// Inputs (keyboard, pad, wheel) of the ghost from TMU, TMUF, TMTurbo and TM2. TMTurbo stores the keyboard inputs as <see cref="ControlEntryAnalog"/>. For inputs stored in TM1.0, TMO, Sunrise and ESWC: see <see cref="CGameCtnReplayRecord.ControlEntries"/>. TM2020 and Shootmania inputs are available in <see cref="PlayerInputs"/>. Can be null if <see cref="EventsDuration"/> is 0, which can happen when you save the replay in editor.
    /// </summary>
    [Obsolete("Use Inputs instead. Property will be removed in 1.3.0")]
    public ControlEntry[]? ControlEntries
    {
        get
        {
            DiscoverChunk<Chunk03092025>();
            return controlEntries;
        }
        set
        {
            DiscoverChunk<Chunk03092025>();
            controlEntries = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03092011>]
    [AppliedWithChunk<Chunk03092019>]
    [AppliedWithChunk<Chunk03092025>]
    public string? Validate_ExeVersion
    {
        get
        {
            DiscoverChunk<Chunk03092025>();
            return validate_ExeVersion;
        }
        set
        {
            DiscoverChunk<Chunk03092025>();
            validate_ExeVersion = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03092011>]
    [AppliedWithChunk<Chunk03092019>]
    [AppliedWithChunk<Chunk03092025>]
    public uint Validate_ExeChecksum
    {
        get
        {
            DiscoverChunk<Chunk03092025>();
            return validate_ExeChecksum;
        }
        set
        {
            DiscoverChunk<Chunk03092025>();
            validate_ExeChecksum = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03092011>]
    [AppliedWithChunk<Chunk03092019>]
    [AppliedWithChunk<Chunk03092025>]
    public int Validate_OsKind
    {
        get
        {
            DiscoverChunk<Chunk03092025>();
            return validate_OsKind;
        }
        set
        {
            DiscoverChunk<Chunk03092025>();
            validate_OsKind = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03092011>]
    [AppliedWithChunk<Chunk03092019>]
    [AppliedWithChunk<Chunk03092025>]
    public int Validate_CpuKind
    {
        get
        {
            DiscoverChunk<Chunk03092025>();
            return validate_CpuKind;
        }
        set
        {
            DiscoverChunk<Chunk03092025>();
            validate_CpuKind = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03092011>]
    [AppliedWithChunk<Chunk03092019>]
    [AppliedWithChunk<Chunk03092025>]
    public string? Validate_RaceSettings
    {
        get
        {
            DiscoverChunk<Chunk03092025>();
            return validate_RaceSettings;
        }
        set
        {
            DiscoverChunk<Chunk03092025>();
            validate_RaceSettings = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03092010>]
    public string? Validate_ChallengeUid { get => validate_ChallengeUid; set => validate_ChallengeUid = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03092028>]
    public string? Validate_TitleId { get => validate_TitleId; set => validate_TitleId = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk03092000>(sinceVersion: 5)]
    public CPlugEntRecordData? RecordData { get => recordData; set => recordData = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03092000>(sinceVersion: 6)]
    public string? GhostTrigram { get => ghostTrigram; set => ghostTrigram = value; }

    [NodeMember(ExactName = "GhostCountryPath")]
    [AppliedWithChunk<Chunk03092000>(sinceVersion: 7)]
    public string? GhostZone { get => ghostZone; set => ghostZone = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk03092000>]
    public bool HasBadges { get => hasBadges; set => hasBadges = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk03092000>]
    public int? BadgeVersion { get => badgeVersion; set => badgeVersion = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk03092000>]
    public Badge? Badge { get => badge; set => badge = value; }

    [NodeMember]
    [SupportsFormatting]
    [AppliedWithChunk<Chunk03092000>(sinceVersion: 8)]
    public string? GhostClubTag { get => ghostClubTag; set => ghostClubTag = value; }

    /// <summary>
    /// Shootmania and TM2020 inputs.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk0309201D>]
    public PlayerInputData[]? PlayerInputs
    {
        get
        {
            DiscoverChunk<Chunk0309201D>();
            return playerInputs;
        }
        set
        {
            DiscoverChunk<Chunk0309201D>();
            playerInputs = value;
        }
    }

    /// <summary>
    /// Inputs (keyboard, pad, wheel) of the ghost from TMU, TMUF, TMTurbo and TM2. TMTurbo stores the keyboard steering as analog. For inputs stored in TM1.0, TMO, Sunrise and ESWC: see <see cref="CGameCtnReplayRecord.Inputs"/>. TM2020 and Shootmania inputs are available in <see cref="PlayerInputs"/>. Can be null if <see cref="EventsDuration"/> is 0, which can happen when you save the replay in editor.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03092011>]
    [AppliedWithChunk<Chunk03092019>]
    [AppliedWithChunk<Chunk03092025>]
    public IReadOnlyCollection<IInput>? Inputs
    {
        get
        {
            DiscoverChunk<Chunk03092025>();
            return inputs;
        }
        set
        {
            DiscoverChunk<Chunk03092025>();
            inputs = value;
        }
    }

    #endregion

    #region Constructors

    internal CGameCtnGhost()
    {
        playerModel = Ident.Empty;
    }

    #endregion

    #region Chunks

    #region 0x000 skippable chunk (basic)

    /// <summary>
    /// CGameCtnGhost 0x000 skippable chunk (basic)
    /// </summary>
    [Chunk(0x03092000, "basic", ProcessSync = true)]
    public class Chunk03092000 : SkippableChunk<CGameCtnGhost>, IVersionable
    {
        private int version;
        
        public bool U03;
        public int[]? U04;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            
            // SGamePlayerMobilAppearanceParams::Archive
            rw.Ident(ref n.playerModel!);
            rw.Vec3(ref n.lightTrailColor);
            rw.ListFileRef(ref n.skinPackDescs);
            rw.Boolean(ref n.hasBadges);

            if (n.hasBadges)
            {
                rw.Int32(ref n.badgeVersion);
                rw.Archive<Badge>(ref n.badge, n.badgeVersion.GetValueOrDefault()); // NGameBadge::BadgeArchive
            }
            //

            rw.String(ref n.ghostNickname);
            rw.String(ref n.ghostAvatarName);

            if (Version >= 2)
            {
                rw.String(ref n.recordingContext);

                if (Version < 5)
                {
                    // TODO
                }

                if (Version >= 4)
                {
                    rw.Boolean(ref U03);

                    if (Version >= 5)
                    {
                        rw.NodeRef<CPlugEntRecordData>(ref n.recordData);
                        rw.Array(ref U04);

                        if (Version >= 6)
                        {
                            rw.String(ref n.ghostTrigram);

                            if (Version >= 7)
                            {
                                rw.String(ref n.ghostZone);

                                if (Version >= 8)
                                {
                                    rw.String(ref n.ghostClubTag);

                                    if (Version > 8)
                                    {
                                        throw new ChunkVersionNotSupportedException(version);
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

    #region 0x003 chunk

    /// <summary>
    /// CGameCtnGhost 0x003 chunk
    /// </summary>
    [Chunk(0x03092003)]
    public class Chunk03092003 : Chunk<CGameCtnGhost>
    {
        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Ident(ref n.playerModel!);
            rw.String(ref n.skinFile);
            rw.String(ref n.ghostNickname);
        }
    }

    #endregion

    #region 0x004 skippable chunk (old checkpoints)

    /// <summary>
    /// CGameCtnGhost 0x004 skippable chunk (old checkpoints)
    /// </summary>
    [Chunk(0x03092004)]
    public class Chunk03092004 : SkippableChunk<CGameCtnGhost>
    {
        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Array(ref n.checkpoints,
                (i, r) => new(r.ReadTimeInt32Nullable(), Speed: r.ReadSingle()),
                (x, w) => { w.WriteTimeInt32Nullable(x.Time); w.Write(x.Speed.GetValueOrDefault()); }
            );
        }
    }

    #endregion

    #region 0x005 skippable chunk (race time)

    /// <summary>
    /// CGameCtnGhost 0x005 skippable chunk (race time)
    /// </summary>
    [Chunk(0x03092005, "race time")]
    public class Chunk03092005 : SkippableChunk<CGameCtnGhost>
    {
        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.TimeInt32Nullable(ref n.raceTime);
        }
    }

    #endregion

    #region 0x006 chunk

    /// <summary>
    /// CGameCtnGhost 0x006 chunk
    /// </summary>
    [Chunk(0x03092006)]
    public class Chunk03092006 : Chunk<CGameCtnGhost>
    {
        public int U01;

        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Ident(ref n.playerModel!);
            rw.String(ref n.skinFile);
            rw.Int32(ref U01);
            rw.String(ref n.ghostNickname);
        }
    }

    #endregion

    #region 0x007 chunk (old light trail color)

    /// <summary>
    /// CGameCtnGhost 0x007 chunk (old light trail color)
    /// </summary>
    [Chunk(0x03092007, "old light trail color")]
    public class Chunk03092007 : Chunk<CGameCtnGhost>
    {
        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Vec3(ref n.lightTrailColor);
        }
    }

    #endregion

    #region 0x008 skippable chunk (respawns)

    /// <summary>
    /// CGameCtnGhost 0x008 skippable chunk (respawns)
    /// </summary>
    [Chunk(0x03092008, "respawns")]
    public class Chunk03092008 : SkippableChunk<CGameCtnGhost>
    {
        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Int32Nullable(ref n.respawns);
        }
    }

    #endregion

    #region 0x009 skippable chunk (light trail color)

    /// <summary>
    /// CGameCtnGhost 0x009 skippable chunk (light trail color)
    /// </summary>
    [Chunk(0x03092009, "light trail color")]
    public class Chunk03092009 : SkippableChunk<CGameCtnGhost>
    {
        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Vec3(ref n.lightTrailColor);
        }
    }

    #endregion

    #region 0x00A skippable chunk (stunt score)

    /// <summary>
    /// CGameCtnGhost 0x00A skippable chunk (stunt score)
    /// </summary>
    [Chunk(0x0309200A, "stunt score")]
    public class Chunk0309200A : SkippableChunk<CGameCtnGhost>
    {
        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Int32Nullable(ref n.stuntScore);
        }
    }

    #endregion

    #region 0x00B skippable chunk (checkpoint times)

    /// <summary>
    /// CGameCtnGhost 0x00B skippable chunk (checkpoint times)
    /// </summary>
    [Chunk(0x0309200B, "checkpoint times")]
    public class Chunk0309200B : SkippableChunk<CGameCtnGhost>
    {
        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Array(ref n.checkpoints,
                (i, r) => new(r.ReadTimeInt32Nullable(), StuntsScore: r.ReadInt32()),
                (x, w) => { w.WriteTimeInt32Nullable(x.Time); w.Write(x.StuntsScore); }
            );
        }
    }

    #endregion

    #region 0x00C chunk

    /// <summary>
    /// CGameCtnGhost 0x00C chunk
    /// </summary>
    [Chunk(0x0309200C)]
    public class Chunk0309200C : Chunk<CGameCtnGhost>
    {
        public int U01;

        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x00D chunk

    /// <summary>
    /// CGameCtnGhost 0x00D chunk
    /// </summary>
    [Chunk(0x0309200D)]
    public class Chunk0309200D : Chunk<CGameCtnGhost>
    {
        public int U01;
        public int U02;
        public int U03;
        public int U04;

        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Ident(ref n.playerModel!);
            rw.String(ref n.skinFile);

            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);

            rw.String(ref n.ghostNickname);
        }
    }

    #endregion

    #region 0x00E chunk

    /// <summary>
    /// CGameCtnGhost 0x00E chunk
    /// </summary>
    [Chunk(0x0309200E)]
    public class Chunk0309200E : Chunk<CGameCtnGhost>
    {
        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Id(ref n.ghostUid, tryParseToInt32: true); // TODO: check writing
        }
    }

    #endregion

    #region 0x00F chunk (ghost login)

    /// <summary>
    /// CGameCtnGhost 0x00F chunk (ghost login)
    /// </summary>
    [Chunk(0x0309200F, "ghost login")]
    public class Chunk0309200F : Chunk<CGameCtnGhost>
    {
        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.ghostLogin);
        }
    }

    #endregion

    #region 0x010 chunk (validation map UID)

    /// <summary>
    /// CGameCtnGhost 0x010 chunk (validation map UID)
    /// </summary>
    [Chunk(0x03092010, "validation map UID")]
    public class Chunk03092010 : Chunk<CGameCtnGhost>
    {
        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Id(ref n.validate_ChallengeUid);
        }
    }

    #endregion

    #region 0x011 chunk (validation TMU)

    /// <summary>
    /// CGameCtnGhost 0x011 chunk (validation TMU)
    /// </summary>
    [Chunk(0x03092011, "validation TMU")]
    public class Chunk03092011 : Chunk<CGameCtnGhost>
    {
        internal bool Is025Ver1 { get; set; }

        public uint U01;
        public int U02;

        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.TimeInt32(ref n.eventsDuration);

            if (n.eventsDuration == TimeInt32.Zero && !Is025Ver1)
            {
                return;
            }

            rw.UInt32(ref U01);

            if (rw.Reader is not null)
            {
                ReadInputs(n, rw.Reader);
            }

            if (rw.Writer is not null)
            {
                WriteInputs(n, rw.Writer);
            }

            rw.String(ref n.validate_ExeVersion);
            rw.UInt32(ref n.validate_ExeChecksum);
            rw.Int32(ref n.validate_OsKind);
            rw.Int32(ref n.validate_CpuKind);
            rw.String(ref n.validate_RaceSettings);
        }

        private void ReadInputs(CGameCtnGhost n, GameBoxReader r)
        {
            var controlNames = r.ReadArray(r => r.ReadId());

            var numEntries = r.ReadInt32();
            U02 = r.ReadInt32();

            n.controlEntries = new ControlEntry[numEntries];

            var inputs = new IInput[numEntries];
            n.inputs = inputs; // as n.inputs is IReadOnlyCollection

            for (var i = 0; i < numEntries; i++)
            {
                var time = TimeInt32.FromMilliseconds(r.ReadInt32() - 100000);
                var controlNameIndex = r.ReadByte();
                var data = r.ReadUInt32();

                var name = (string)controlNames[controlNameIndex];

                n.controlEntries[i] = name switch
                {
                    "Steer" or "Gas" or "AccelerateReal" or "BrakeReal"
                      => new ControlEntryAnalog(name, time, data),
                    _ => new ControlEntry(name, time, data),
                };

                inputs[i] = NET.Inputs.Input.Parse(time, name, data);
            }
        }

        private void WriteInputs(CGameCtnGhost n, GameBoxWriter w)
        {
            var controlNames = new List<string>();

            if (n.controlEntries is not null)
            {
                foreach (var entry in n.controlEntries)
                {
                    if (!controlNames.Contains(entry.Name))
                    {
                        controlNames.Add(entry.Name);
                    }
                }
            }

            w.WriteList(controlNames, (x, w) => w.WriteId(x));

            w.Write(n.controlEntries?.Length ?? 0);
            w.Write(U02);

            if (n.controlEntries is not null)
            {
                foreach (var entry in n.controlEntries)
                {
                    w.Write(entry.Time.TotalMilliseconds + 100000);
                    w.Write((byte)controlNames.IndexOf(entry.Name));
                    w.Write(entry.Data);
                }
            }
        }
    }

    #endregion

    #region 0x012 chunk

    /// <summary>
    /// CGameCtnGhost 0x012 chunk
    /// </summary>
    [Chunk(0x03092012)]
    public class Chunk03092012 : Chunk<CGameCtnGhost>
    {
        public uint U01;
        public BigInteger U02;

        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.UInt32(ref U01);
            rw.Int128(ref U02); // probably random ID according to RE
        }
    }

    #endregion

    #region 0x013 skippable chunk

    /// <summary>
    /// CGameCtnGhost 0x013 chunk
    /// </summary>
    [Chunk(0x03092013)]
    public class Chunk03092013 : SkippableChunk<CGameCtnGhost>
    {
        public int U01;
        public int U02;

        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    #endregion

    #region 0x014 skippable chunk

    /// <summary>
    /// CGameCtnGhost 0x014 chunk
    /// </summary>
    [Chunk(0x03092014)]
    public class Chunk03092014 : SkippableChunk<CGameCtnGhost>
    {
        public int U01;

        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01); // some validation version
        }
    }

    #endregion

    #region 0x015 chunk (ghost nickname)

    /// <summary>
    /// CGameCtnGhost 0x015 chunk (ghost nickname)
    /// </summary>
    [Chunk(0x03092015, "ghost nickname")]
    public class Chunk03092015 : Chunk<CGameCtnGhost>
    {
        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Id(ref n.ghostNickname);
        }
    }

    #endregion

    #region 0x017 skippable chunk

    /// <summary>
    /// CGameCtnGhost 0x017 skippable chunk (ghost metadata)
    /// </summary>
    [Chunk(0x03092017, "ghost metadata")]
    public class Chunk03092017 : SkippableChunk<CGameCtnGhost>
    {
        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.ListFileRef(ref n.skinPackDescs);
            rw.String(ref n.ghostNickname);
            rw.String(ref n.ghostAvatarName);
        }
    }

    #endregion

    #region 0x018 chunk

    /// <summary>
    /// CGameCtnGhost 0x018 chunk
    /// </summary>
    [Chunk(0x03092018)]
    public class Chunk03092018 : Chunk<CGameCtnGhost>
    {
        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Ident(ref n.playerModel!);
        }
    }

    #endregion

    #region 0x019 chunk (validation TMUF)

    /// <summary>
    /// CGameCtnGhost 0x019 chunk (validation TMUF)
    /// </summary>
    [Chunk(0x03092019, "validation TMUF")]
    public class Chunk03092019 : Chunk03092011
    {
        public int U03;

        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            base.ReadWrite(n, rw);

            if (n.eventsDuration == TimeInt32.Zero && !Is025Ver1)
            {
                return;
            }

            rw.Int32(ref U03);
        }
    }

    #endregion

    #region 0x01A skippable chunk

    /// <summary>
    /// CGameCtnGhost 0x01A skippable chunk
    /// </summary>
    [Chunk(0x0309201A)]
    public class Chunk0309201A : SkippableChunk<CGameCtnGhost>
    {
        public int U01;

        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x01B skippable chunk (race result)

    /// <summary>
    /// CGameCtnGhost 0x01B skippable chunk (race result)
    /// </summary>
    [Chunk(0x0309201B, "race result"), IgnoreChunk]
    public class Chunk0309201B : SkippableChunk<CGameCtnGhost>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }
    }

    #endregion

    #region 0x01C chunk

    /// <summary>
    /// CGameCtnGhost 0x01C chunk
    /// </summary>
    [Chunk(0x0309201C)]
    public class Chunk0309201C : Chunk<CGameCtnGhost>
    {
        public BigInteger U01;

        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.BigInt(ref U01, 32);
        }
    }

    #endregion

    #region 0x01D skippable chunk (player input data)

    /// <summary>
    /// CGameCtnGhost 0x01D skippable chunk (player input data)
    /// </summary>
    [Chunk(0x0309201D, "player input data")]
    public class Chunk0309201D : SkippableChunk<CGameCtnGhost>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version < 2)
            {
                throw new VersionNotSupportedException(version);
            }

            // empty in an unfinished run
            rw.ArrayArchive<PlayerInputData>(ref n.playerInputs, version);
        }
    }

    #endregion

    #region 0x01F skippable chunk (OldColorHistory)

    /// <summary>
    /// CGameCtnGhost 0x01F skippable chunk (OldColorHistory)
    /// </summary>
    [Chunk(0x0309201F, "OldColorHistory"), IgnoreChunk]
    public class Chunk0309201F : SkippableChunk<CGameCtnGhost>
    {
        
    }

    #endregion

    #region 0x021 skippable chunk (OldKeyStrokes)

    /// <summary>
    /// CGameCtnGhost 0x021 skippable chunk (OldKeyStrokes)
    /// </summary>
    [Chunk(0x03092021, "OldKeyStrokes"), IgnoreChunk]
    public class Chunk03092021 : SkippableChunk<CGameCtnGhost>
    {

    }

    #endregion

    #region 0x022 skippable chunk (timed pixel array)

    /// <summary>
    /// CGameCtnGhost 0x022 skippable chunk (timed pixel array)
    /// </summary>
    [Chunk(0x03092022, "timed pixel array"), IgnoreChunk]
    public class Chunk03092022 : SkippableChunk<CGameCtnGhost>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }
    }

    #endregion

    #region 0x023 skippable chunk

    /// <summary>
    /// CGameCtnGhost 0x023 skippable chunk
    /// </summary>
    [Chunk(0x03092023)]
    public class Chunk03092023 : SkippableChunk<CGameCtnGhost>, IVersionable
    {
        private int version = 3;

        public string? U01;
        public int U02;
        public string? U03;
        public int U04;
        public int U05;
        public string? U06;
        public int U07;
        public string? U08;
        public byte? U09;
        public int? U10;
        public int? U11;
        public byte? U12;
        public byte? U13;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.String(ref U01);
            rw.Int32(ref U02);
            rw.String(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.String(ref U06);
            rw.Int32(ref U07);
            rw.String(ref U08);

            if (version >= 2)
            {
                rw.Byte(ref U09);
                rw.Int32(ref U10);
                rw.Int32(ref U11);

                if (version >= 3)
                {
                    rw.Byte(ref U12);
                    rw.Byte(ref U13);
                }
            }
        }
    }

    #endregion

    #region 0x024 skippable chunk (match replay separators)

    /// <summary>
    /// CGameCtnGhost 0x024 skippable chunk (match replay separators)
    /// </summary>
    [Chunk(0x03092024, "match replay separators"), IgnoreChunk]
    public class Chunk03092024 : SkippableChunk<CGameCtnGhost>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }
    }

    #endregion

    #region 0x025 skippable chunk (validation TM2)

    /// <summary>
    /// CGameCtnGhost 0x025 skippable chunk (validation TM2)
    /// </summary>
    [Chunk(0x03092025, "validation TM2")]
    public class Chunk03092025 : SkippableChunk<CGameCtnGhost>, IVersionable
    {
        private int version;

        public bool U01;

        public int Version { get => version; set => version = value; }

        public Chunk03092019 Chunk019 { get; }

        public Chunk03092025()
        {
            Chunk019 = new Chunk03092019();
        }

        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            Chunk019.Is025Ver1 = version >= 1;
            Chunk019.ReadWrite(n, rw);

            if (n.eventsDuration == TimeInt32.Zero && !Chunk019.Is025Ver1)
            {
                return;
            }

            rw.Boolean(ref U01);
        }
    }

    #endregion

    #region 0x026 skippable chunk

    /// <summary>
    /// CGameCtnGhost 0x026 skippable chunk
    /// </summary>
    [Chunk(0x03092026)]
    public class Chunk03092026 : SkippableChunk<CGameCtnGhost>
    {
        public BigInteger U01;

        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Int128(ref U01);
        }
    }

    #endregion

    #region 0x027 skippable chunk (timed pixel array 2)

    /// <summary>
    /// CGameCtnGhost 0x027 skippable chunk (timed pixel array 2)
    /// </summary>
    [Chunk(0x03092027, "timed pixel array 2"), IgnoreChunk]
    public class Chunk03092027 : SkippableChunk<CGameCtnGhost>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }
    }

    #endregion

    #region 0x028 skippable chunk (title id)

    /// <summary>
    /// CGameCtnGhost 0x028 skippable chunk (title id)
    /// </summary>
    [Chunk(0x03092028, "title id")]
    public class Chunk03092028 : SkippableChunk<CGameCtnGhost>
    {
        public BigInteger? U01;

        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            if (n.EventsDuration == TimeInt32.Zero)
            {
                return;
            }
            
            rw.String(ref n.validate_TitleId);
            rw.BigInt(ref U01, 32);
        }
    }

    #endregion

    #region 0x029 skippable chunk

    /// <summary>
    /// CGameCtnGhost 0x029 skippable chunk
    /// </summary>
    [Chunk(0x03092029), IgnoreChunk]
    public class Chunk03092029 : SkippableChunk<CGameCtnGhost>
    {
        
    }

    #endregion

    #region 0x02A skippable chunk

    /// <summary>
    /// CGameCtnGhost 0x02A skippable chunk
    /// </summary>
    [Chunk(0x0309202A), IgnoreChunk]
    public class Chunk0309202A : SkippableChunk<CGameCtnGhost>
    {

    }

    #endregion

    #region 0x02B skippable chunk

    /// <summary>
    /// CGameCtnGhost 0x02B skippable chunk
    /// </summary>
    [Chunk(0x0309202B), IgnoreChunk]
    public class Chunk0309202B : SkippableChunk<CGameCtnGhost>
    {

    }

    #endregion

    #region 0x02C skippable chunk

    /// <summary>
    /// CGameCtnGhost 0x02C skippable chunk
    /// </summary>
    [Chunk(0x0309202C), IgnoreChunk]
    public class Chunk0309202C : SkippableChunk<CGameCtnGhost>
    {

    }

    #endregion

    #region 0x02D skippable chunk

    /// <summary>
    /// CGameCtnGhost 0x02D skippable chunk
    /// </summary>
    [Chunk(0x0309202D), IgnoreChunk]
    public class Chunk0309202D : SkippableChunk<CGameCtnGhost>
    {

    }

    #endregion

    #region 0x02E skippable chunk

    /// <summary>
    /// CGameCtnGhost 0x02E skippable chunk
    /// </summary>
    [Chunk(0x0309202E), IgnoreChunk]
    public class Chunk0309202E : SkippableChunk<CGameCtnGhost>
    {

    }

    #endregion

    #endregion
}
