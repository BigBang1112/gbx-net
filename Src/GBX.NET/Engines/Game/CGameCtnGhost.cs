using GBX.NET.Exceptions;
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
    private Vec3? lightTrailColor;
    private int? stuntScore;
    private Checkpoint[]? checkpoints;
    private int eventsDuration;
    private ControlEntry[]? controlEntries;
    private string? validate_ExeVersion;
    private uint validate_ExeChecksum;
    private int validate_OsKind;
    private int validate_CpuKind;
    private string? validate_RaceSettings;
    private string? validate_ChallengeUid;
    private string? validate_TitleId;
    private bool hasBadges;
    private string? skinFile;
    private string? ghostClubTag;

    #endregion

    #region Properties

    [NodeMember]
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
    public Vec3? LightTrailColor
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
    public string? GhostLogin
    {
        get => ghostLogin;
        set => ghostLogin = value;
    }

    [NodeMember]
    public string? SkinFile
    {
        get => skinFile;
        set => skinFile = value;
    }

    [NodeMember]
    public string? GhostUid
    {
        get => ghostUid;
        set => ghostUid = value;
    }

    /// <summary>
    /// Duration of events in the ghost (range of detected inputs). This can be 0 if the ghost was driven in editor.
    /// </summary>
    [NodeMember]
    public int EventsDuration
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
    /// Inputs (keyboard, pad, wheel) of the ghost from TMU, TMUF, TMTurbo and TM2. TMTurbo stores the keyboard inputs as <see cref="ControlEntryAnalog"/>. For inputs stored in TM1.0, TMO, Sunrise and ESWC: see <see cref="CGameCtnReplayRecord.ControlEntries"/>. TM2020 and Shootmania inputs aren't available in replays and ghosts. Can be null if <see cref="EventsDuration"/> is 0, which can happen when you save the replay in editor.
    /// </summary>
    [NodeMember]
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
    public string? Validate_ChallengeUid
    {
        get => validate_ChallengeUid;
        set => validate_ChallengeUid = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public string? Validate_TitleId
    {
        get => validate_TitleId;
        set => validate_TitleId = value;
    }

    [NodeMember]
    public CPlugEntRecordData? RecordData
    {
        get => recordData;
        set => recordData = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public string? GhostTrigram
    {
        get => ghostTrigram;
        set => ghostTrigram = value;
    }

    [NodeMember(ExactName = "GhostCountryPath")]
    public string? GhostZone
    {
        get => ghostZone;
        set => ghostZone = value;
    }

    [NodeMember]
    public bool HasBadges
    {
        get => hasBadges;
        set => hasBadges = value;
    }

    [NodeMember]
    [SupportsFormatting]
    public string? GhostClubTag
    {
        get => ghostClubTag;
        set => ghostClubTag = value;
    }

    #endregion

    #region Constructors

    protected CGameCtnGhost()
    {
        playerModel = null!;
    }

    #endregion

    #region Chunks

    #region 0x000 skippable chunk (basic)

    /// <summary>
    /// CGameCtnGhost 0x000 skippable chunk (basic)
    /// </summary>
    [Chunk(0x03092000, processSync: true, "basic")]
    public class Chunk03092000 : SkippableChunk<CGameCtnGhost>, IVersionable
    {
        private int version;

        public Vec3 U01;
        public bool U03;
        public int[]? U04;
        public int U05;
        public Vec3 U06;
        public (string value, string key)[]? U07;
        public string[]? U08;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Ident(ref n.playerModel!);
            rw.Vec3(ref n.lightTrailColor);
            rw.ListFileRef(ref n.skinPackDescs);
            rw.Boolean(ref n.hasBadges);

            if (n.hasBadges)
            {
                rw.Int32(ref U05);
                rw.Vec3(ref U06);
                rw.Array(ref U07,
                    (i, r) => (r.ReadString(), r.ReadString()),
                    (x, w) =>
                    {
                        w.Write(x.Item1);
                        w.Write(x.Item2);
                    });
                rw.ArrayString(ref U08);
            }

            rw.String(ref n.ghostNickname);
            rw.String(ref n.ghostAvatarName);

            if (Version >= 2)
            {
                n.RecordingContext = rw.String(n.recordingContext);

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
            rw.Int32(ref n.respawns);
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
            rw.Int32(ref n.stuntScore);
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

    #region 0x010 chunk

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

        public Chunk03092011()
        {

        }

        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.eventsDuration);

            if (n.eventsDuration == 0 && !Is025Ver1) return;

            rw.UInt32(ref U01);

            if (rw.Reader is not null)
            {
                var r = rw.Reader;

                var controlNames = r.ReadArray(r1 => r1.ReadId());

                var numEntries = r.ReadInt32();
                U02 = r.ReadInt32();

                n.ControlEntries = new ControlEntry[numEntries];

                for (var i = 0; i < numEntries; i++)
                {
                    var time = TimeInt32.FromMilliseconds(r.ReadInt32() - 100000);
                    var controlNameIndex = r.ReadByte();
                    var data = r.ReadUInt32();

                    var name = controlNames[controlNameIndex];

                    n.ControlEntries[i] = (string)name switch
                    {
                        "Steer" or "Gas" or "AccelerateReal" or "BrakeReal"
                          => new ControlEntryAnalog(name, time, data),
                        _ => new ControlEntry(name, time, data),
                    };
                }
            }
            
            if (rw.Writer is not null)
            {
                var w = rw.Writer;

                var controlNames = new List<string>();

                if (n.ControlEntries != null)
                {
                    foreach (var entry in n.ControlEntries)
                        if (!controlNames.Contains(entry.Name))
                            controlNames.Add(entry.Name);
                }

                w.WriteList(controlNames, (x, w) => w.WriteId(x));

                w.Write(n.ControlEntries?.Length ?? 0);
                w.Write(U02);

                if (n.ControlEntries != null)
                {
                    foreach (var entry in n.ControlEntries)
                    {
                        w.Write(entry.Time.TotalMilliseconds + 100000);
                        w.Write((byte)controlNames.IndexOf(entry.Name));
                        w.Write(entry.Data);
                    }
                }
            }

            rw.String(ref n.validate_ExeVersion);
            rw.UInt32(ref n.validate_ExeChecksum);
            rw.Int32(ref n.validate_OsKind);
            rw.Int32(ref n.validate_CpuKind);
            rw.String(ref n.validate_RaceSettings);
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
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x015 chunk (vehicle)

    /// <summary>
    /// CGameCtnGhost 0x015 chunk
    /// </summary>
    [Chunk(0x03092015)]
    public class Chunk03092015 : Chunk<CGameCtnGhost>
    {
        public string? U01;

        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01);
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
        public Ident? U01;

        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Ident(ref U01);
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

            if (n.eventsDuration == 0 && !Is025Ver1) return;

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
    [Chunk(0x0309201B, "race result")]
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
        
        public PlayerInputData[]? PlayerInputData;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version < 2)
            {
                throw new VersionNotSupportedException(version);
            }

            // empty in an unfinished run
            rw.ArrayArchive<PlayerInputData>(ref PlayerInputData, version);
        }
    }

    #endregion

    #region 0x022 skippable chunk (timed pixel array)

    /// <summary>
    /// CGameCtnGhost 0x022 skippable chunk (timed pixel array)
    /// </summary>
    [Chunk(0x03092022, "timed pixel array")]
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
    [Chunk(0x03092024, "match replay separators")]
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

            if (n.eventsDuration == 0 && !Chunk019.Is025Ver1) return;

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
    [Chunk(0x03092027, "timed pixel array 2")]
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
            if (n.EventsDuration == 0)
            {
                return;
            }
            
            rw.String(ref n.validate_TitleId);
            rw.BigInt(ref U01, 32);
        }
    }

    #endregion

    #endregion
}
