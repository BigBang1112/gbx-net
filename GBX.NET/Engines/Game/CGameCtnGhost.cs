using GBX.NET.Engines.Plug;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// Ghost (0x03092000)
    /// </summary>
    [Node(0x03092000)]
    public class CGameCtnGhost : CGameGhost
    {
        #region Fields

        private Ident playerModel;
        private FileRef[] skinPackDescs;
        private string ghostNickname;
        private string ghostAvatarName;
        private string recordingContext;
        private CPlugEntRecordData recordData;
        private string ghostTrigram;
        private string ghostZone;
        private TimeSpan? raceTime;
        private int? respawns;
        private Vec3? lightTrailColor;
        private int? stuntScore;
        private TimeSpan[] checkpoints;
        private int eventsDuration;
        private ControlEntry[] controlEntries;
        private string validate_ExeVersion;
        private uint validate_ExeChecksum;
        private int validate_OsKind;
        private int validate_CpuKind;
        private string validate_RaceSettings;
        private string validate_ChallengeUid;
        private string validate_TitleId;
        private bool hasBadges;

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
        public FileRef[] SkinPackDescs
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

        [NodeMember]
        public string GhostNickname
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

        [NodeMember]
        public string GhostAvatarName
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

        [NodeMember]
        public string RecordingContext
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

        [NodeMember]
        public TimeSpan? RaceTime
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

        [NodeMember]
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

        [NodeMember]
        public TimeSpan[] Checkpoints
        {
            get
            {
                DiscoverChunk<Chunk0309200B>();
                return checkpoints;
            }
            set
            {
                DiscoverChunk<Chunk0309200B>();
                checkpoints = value;
            }
        }

        [NodeMember]
        public string GhostLogin { get; set; }

        [NodeMember]
        public Ident Vehicle { get; set; }

        [NodeMember]
        public string Skin { get; set; }

        [NodeMember]
        public string UID { get; set; }

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

        [NodeMember]
        public ControlEntry[] ControlEntries
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

        [NodeMember]
        public string Validate_ExeVersion
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

        [NodeMember]
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

        [NodeMember]
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

        [NodeMember]
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

        [NodeMember]
        public string Validate_RaceSettings
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

        public string Validate_ChallengeUid
        {
            get => validate_ChallengeUid;
            set => validate_ChallengeUid = value;
        }

        public string Validate_TitleId
        {
            get => validate_TitleId;
            set => validate_TitleId = value;
        }

        [NodeMember]
        public CPlugEntRecordData RecordData
        {
            get => recordData;
            set => recordData = value;
        }

        [NodeMember]
        public string GhostTrigram
        {
            get => ghostTrigram;
            set => ghostTrigram = value;
        }

        [NodeMember]
        public string GhostZone
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

        #endregion

        #region Chunks

        #region 0x000 skippable chunk (basic)

        /// <summary>
        /// CGameCtnGhost 0x000 skippable chunk (basic)
        /// </summary>
        [Chunk(0x03092000, true, "basic")]
        public class Chunk03092000 : SkippableChunk<CGameCtnGhost>
        {
            private int version;
            private Vec3 u01;
            private bool u03;
            private int[] u04;
            private int u05;
            private Vec3 u06;
            private (string value, string key)[] u07;
            private string[] u08;

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

            public bool U03
            {
                get => u03;
                set => u03 = value;
            }

            public int[] U04
            {
                get => u04;
                set => u04 = value;
            }

            public int U05
            {
                get => u05;
                set => u05 = value;
            }

            public Vec3 U06
            {
                get => u06;
                set => u06 = value;
            }

            public (string value, string key)[] U07
            {
                get => u07;
                set => u07 = value;
            }

            public string[] U08
            {
                get => u08;
                set => u08 = value;
            }

            public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
                rw.Ident(ref n.playerModel);
                rw.Vec3(ref u01);
                rw.Array(ref n.skinPackDescs,
                    (i, r) => r.ReadFileRef(),
                    (x, w) => w.Write(x));
                rw.Boolean(ref n.hasBadges);

                if (n.hasBadges)
                {
                    rw.Int32(ref u05);
                    rw.Vec3(ref u06);
                    rw.Array(ref u07,
                        (i, r) => (r.ReadString(), r.ReadString()),
                        (x, w) => {
                            w.Write(x.Item1);
                            w.Write(x.Item2);
                        });
                    rw.Array(ref u08,
                        (i, r) => r.ReadString(),
                        (x, w) => w.Write(x));
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
                        rw.Boolean(ref u03);

                        if (Version >= 5)
                        {
                            rw.NodeRef<CPlugEntRecordData>(ref n.recordData);
                            rw.Array(ref u04);

                            if (Version >= 6)
                            {
                                rw.String(ref n.ghostTrigram);

                                if (Version >= 7)
                                    rw.String(ref n.ghostZone);
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
                n.Vehicle = rw.Ident(n.Vehicle);
                n.Skin = rw.String(n.Skin);
                n.GhostLogin = rw.String(n.GhostLogin);

                // TODO: Solve naming
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
                n.raceTime = rw.TimeSpan32(n.raceTime);
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
            public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
            {
                n.Vehicle = rw.Ident(n.Vehicle);
                n.Skin = rw.String(n.Skin);
                rw.Int32(Unknown);
                rw.String(Unknown);
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
                n.respawns = rw.Int32(n.respawns.GetValueOrDefault());
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
                n.lightTrailColor = rw.Vec3(n.lightTrailColor.GetValueOrDefault());
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
                n.stuntScore = rw.Int32(n.stuntScore.GetValueOrDefault());
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
                n.checkpoints = rw.Array(n.checkpoints,
                    (i, r) => TimeSpan.FromMilliseconds(r.ReadInt64()),
                    (x, w) => w.Write(Convert.ToInt64(x.TotalMilliseconds)));
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
            public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
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
            public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
            {
                n.Vehicle = rw.Ident(n.Vehicle);
                n.Skin = rw.Reader.ReadString();

                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);

                rw.String(Unknown);
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
                n.UID = rw.Id(n.UID);
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
                n.GhostLogin = rw.String(n.GhostLogin);
            }
        }

        #endregion

        #region 0x010 chunk

        /// <summary>
        /// CGameCtnGhost 0x010 chunk
        /// </summary>
        [Chunk(0x03092010)]
        public class Chunk03092010 : Chunk<CGameCtnGhost>
        {
            public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
            {
                rw.Id(ref n.validate_ChallengeUid);
            }
        }

        #endregion

        #region 0x011 chunk

        /// <summary>
        /// CGameCtnGhost 0x011 chunk
        /// </summary>
        [Chunk(0x03092011)]
        public class Chunk03092011 : Chunk<CGameCtnGhost>
        {
            public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
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
            public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int64(Unknown);
                rw.Int64(Unknown);
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
            public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
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
            public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x015 chunk (vehicle)

        /// <summary>
        /// CGameCtnGhost 0x015 chunk (vehicle)
        /// </summary>
        [Chunk(0x03092015, "vehicle")]
        public class Chunk03092015 : Chunk<CGameCtnGhost>
        {
            public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
            {
                n.Vehicle = new Ident(rw.Id(n.Vehicle?.ID));
            }
        }

        #endregion

        #region 0x017 skippable chunk

        /// <summary>
        /// CGameCtnGhost 0x017 skippable chunk (ghost metadata)
        /// </summary>
        [Chunk(0x03092017)]
        public class Chunk03092017 : SkippableChunk<CGameCtnGhost>
        {
            public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
            {
                n.skinPackDescs = rw.Array(n.skinPackDescs, i => rw.Reader.ReadFileRef(), x => rw.Writer.Write(x));
                n.ghostNickname = rw.String(n.ghostNickname);
                n.ghostAvatarName = rw.String(n.ghostAvatarName);
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
            public Ident Unknown1 { get; set; }

            public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
            {
                Unknown1 = rw.Ident(Unknown1);
            }
        }

        #endregion

        #region 0x019 chunk (core)

        /// <summary>
        /// CGameCtnGhost 0x019 chunk (validation)
        /// </summary>
        [Chunk(0x03092019, "validation")]
        public class Chunk03092019 : Chunk<CGameCtnGhost>
        {
            readonly bool is025;

            public uint U01 { get; set; }
            public int U02 { get; set; }

            public Chunk03092019()
            {

            }

            public Chunk03092019(Chunk03092025 chunk025)
            {
                is025 = chunk025 is Chunk03092025;
            }

            public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
            {
                n.EventsDuration = rw.Int32(n.EventsDuration);

                if (n.EventsDuration == 0 && !is025) return;

                U01 = rw.UInt32(U01);

                if (rw.Mode == GameBoxReaderWriterMode.Read)
                {
                    var r = rw.Reader;

                    var controlNames = r.ReadArray(i => r.ReadId());

                    var numEntries = r.ReadInt32();
                    r.ReadInt32();

                    n.ControlEntries = new ControlEntry[numEntries];

                    for (var i = 0; i < numEntries; i++)
                    {
                        var time = TimeSpan.FromMilliseconds(r.ReadInt32() - 100000);
                        var controlNameIndex = r.ReadByte();
                        var data = r.ReadUInt32();

                        var name = controlNames[controlNameIndex];

                        switch (name)
                        {
                            case "Steer":
                            case "Gas":
                            case "AccelerateReal":
                            case "BrakeReal":
                                n.ControlEntries[i] = new ControlEntryAnalog() { Name = name, Time = time, Data = data };
                                break;
                            default:
                                n.ControlEntries[i] = new ControlEntry() { Name = name, Time = time, Data = data };
                                break;
                        }
                    }
                }
                else if (rw.Mode == GameBoxReaderWriterMode.Write)
                {
                    var w = rw.Writer;

                    var controlNames = new List<string>();

                    if (n.ControlEntries != null)
                    {
                        foreach (var entry in n.ControlEntries)
                            if (!controlNames.Contains(entry.Name))
                                controlNames.Add(entry.Name);
                    }

                    foreach (var name in controlNames)
                        w.WriteId(name);

                    w.Write(n.ControlEntries?.Length ?? 0);
                    w.Write(0);

                    if (n.ControlEntries != null)
                    {
                        foreach (var entry in n.ControlEntries)
                        {
                            w.Write(Convert.ToInt32(entry.Time.TotalMilliseconds + 100000));
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
                U02 = rw.Int32(U02);
            }
        }

        #endregion

        #region 0x01C chunk

        /// <summary>
        /// CGameCtnGhost 0x01C chunk
        /// </summary>
        [Chunk(0x0309201C)]
        public class Chunk0309201C : Chunk<CGameCtnGhost>
        {
            public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x023 skippable chunk

        /// <summary>
        /// CGameCtnGhost 0x023 skippable chunk
        /// </summary>
        [Chunk(0x03092023), IgnoreChunk]
        public class Chunk03092023 : SkippableChunk<CGameCtnGhost>
        {
            private int version;
            private string u01;
            private int u02;
            private string u03;
            private int u04;
            private int u05;
            private string u06;

            public int Version
            {
                get => version;
                set => version = value;
            }

            public string U01
            {
                get => u01;
                set => u01 = value;
            }

            public int U02
            {
                get => u02;
                set => u02 = value;
            }

            public string U03
            {
                get => u03;
                set => u03 = value;
            }

            public int U04
            {
                get => u04;
                set => u04 = value;
            }

            public int U05
            {
                get => u05;
                set => u05 = value;
            }

            public string U06
            {
                get => u06;
                set => u06 = value;
            }

            public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
                rw.String(ref u01);
                rw.Int32(ref u02);
                rw.String(ref u03);
                rw.Int32(ref u04);
                rw.Int32(ref u05);

                // ...
            }
        }

        #endregion

        #region 0x025 skippable chunk (validation)

        /// <summary>
        /// CGameCtnGhost 0x025 skippable chunk (validation)
        /// </summary>
        [Chunk(0x03092025, "validation")]
        public class Chunk03092025 : SkippableChunk<CGameCtnGhost>
        {
            private int version;

            public int Version
            {
                get => version;
                set => version = value;
            }

            public uint U01 { get; set; }
            public int U02 { get; set; }
            public bool U03 { get; set; }

            private readonly Chunk03092019 chunk019;

            public Chunk03092025()
            {
                chunk019 = new Chunk03092019(this);
            }

            public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
                chunk019.ReadWrite(n, rw);
                U03 = rw.Boolean(U03);
            }
        }

        #endregion

        #region 0x028 skippable chunk (title id)

        /// <summary>
        /// CGameCtnGhost 0x028 skippable chunk (title id)
        /// </summary>
        [Chunk(0x03092028, "title id")]
        public class Chunk03092028 : SkippableChunk<CGameCtnGhost>
        {
            public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
            {
                if (n.EventsDuration != 0)
                    rw.String(ref n.validate_TitleId);
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class ControlEntry
        {
            public string Name { get; set; }
            public TimeSpan Time { get; set; }
            public uint Data { get; set; }
            public bool IsEnabled => Data != 0;

            public override string ToString()
            {
                return $"[{Time.ToStringTM()}] {Name}: {((Data == 128 || Data == 1 || Data == 0) ? IsEnabled.ToString() : Data.ToString())}";
            }
        }

        /// <summary>
        /// A control entry with an additional <see cref="float"/> value.
        /// </summary>
        public class ControlEntryAnalog : ControlEntry
        {
            public float Value
            {
                get
                {
                    if (((Data >> 16) & 0xFF) == 0xFF) // Left steer
                        return (Data & 0xFFFF) / (float)ushort.MaxValue - 1;
                    if ((Data >> 16) == 1) // Full right steer
                        return 1;
                    return (Data & 0xFFFF) / (float)ushort.MaxValue;
                }
            }

            public override string ToString()
            {
                return $"[{Time.ToStringTM()}] {Name}: {Value}";
            }
        }

        #endregion
    }
}
