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
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnGhost : CGameGhost
    {
        #region Fields

        private Ident playerModel;
        private FileRef[] skinPackDescs;
        private string ghostNickname;
        private string ghostAvatarName;
        private string recordingContext;
        private TimeSpan? raceTime;
        private int? respawns;
        private Vec3? lightTrailColor;
        private int? stuntScore;
        private TimeSpan[] checkpoints;

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
        public int EventsDuration { get; set; }

        [NodeMember]
        public string[] ControlNames { get; set; }

        [NodeMember]
        public ControlEntry[] ControlEntries { get; set; } = new ControlEntry[0];

        [NodeMember]
        public string GameVersion { get; set; }

        [NodeMember]
        public int ExeChecksum { get; set; }

        [NodeMember]
        public int OSKind { get; set; }

        [NodeMember]
        public int CPUKind { get; set; }

        [NodeMember]
        public string RaceSettingsXML { get; set; }

        [NodeMember]
        public CPlugEntRecordData RecordData { get; set; }

        [NodeMember]
        public string GhostTrigram { get; set; }

        #endregion

        #region Chunks

        #region 0x000 skippable chunk (basic)

        /// <summary>
        /// CGameCtnGhost 0x000 skippable chunk (basic)
        /// </summary>
        [Chunk(0x03092000, true, "basic")]
        public class Chunk03092000 : SkippableChunk<CGameCtnGhost>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                n.playerModel = rw.Ident(n.playerModel);
                rw.Vec3(Unknown); // unknown
                n.skinPackDescs = rw.Array(n.skinPackDescs,
                    i => rw.Reader.ReadFileRef(),
                    x => rw.Writer.Write(x));
                rw.Int32(Unknown); // unknown
                n.GhostNickname = rw.String(n.ghostNickname);
                n.GhostAvatarName = rw.String(n.ghostAvatarName);

                if (Version >= 2)
                {
                    n.RecordingContext = rw.String(n.recordingContext);

                    if(Version < 5)
                    {

                    }

                    if (Version >= 5)
                    {
                        rw.Int32(Unknown);
                        n.RecordData = rw.NodeRef<CPlugEntRecordData>(n.RecordData);
                        rw.Boolean(Unknown);
                        rw.Int32(Unknown);
                        if (Version >= 6)
                            n.GhostTrigram = rw.String(n.GhostTrigram);
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
                rw.Id(Unknown);
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
        /// CGameCtnGhost 0x019 chunk (core)
        /// </summary>
        [Chunk(0x03092019, "core")]
        public class Chunk03092019 : Chunk<CGameCtnGhost>
        {
            public uint Unknown1 { get; set; }
            public int Unknown2 { get; set; }
            public int Unknown3 { get; set; }

            public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
            {
                n.EventsDuration = rw.Int32(n.EventsDuration);

                if (n.EventsDuration > 0)
                {
                    Unknown1 = rw.UInt32(Unknown1);
                    n.ControlNames = rw.Array(n.ControlNames, i => rw.Reader.ReadId(), x => rw.Writer.WriteId(x));

                    var numEntries = rw.Int32(n.ControlEntries.Length);

                    if (rw.Mode == GameBoxReaderWriterMode.Read)
                    {
                        Unknown2 = rw.Reader.ReadInt32();

                        n.ControlEntries = new ControlEntry[numEntries];

                        for (var i = 0; i < numEntries; i++)
                        {
                            var time = rw.Reader.ReadInt32();
                            var controlNameIndex = rw.Reader.ReadByte();
                            var enabled = rw.Reader.ReadBoolean();

                            n.ControlEntries[i] = new ControlEntry()
                            {
                                Time = time,
                                Index = controlNameIndex,
                                Enabled = enabled
                            };
                        }
                    }
                    else if (rw.Mode == GameBoxReaderWriterMode.Write)
                    {

                    }

                    n.GameVersion = rw.String(n.GameVersion);
                    n.ExeChecksum = rw.Int32(n.ExeChecksum);
                    n.OSKind = rw.Int32(n.OSKind);
                    n.CPUKind = rw.Int32(n.CPUKind);
                    n.RaceSettingsXML = rw.String(n.RaceSettingsXML);
                    Unknown2 = rw.Int32(Unknown2);
                }
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

        #endregion

        #region Other classes

        public class ControlEntry
        {
            public int Time { get; set; }
            public byte Index { get; set; }
            public bool Enabled { get; set; }
        }

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnGhost node;

            public bool IsReplaying => node.IsReplaying;
            public Task<CGameGhostData> Data => node.Data;

            public Ident PlayerModel => node.PlayerModel;
            public FileRef[] SkinPackDescs => node.SkinPackDescs;
            public string GhostNickname => node.GhostNickname;
            public string GhostAvatarName => node.GhostAvatarName;
            public string RecordingContext => node.RecordingContext;
            public TimeSpan? RaceTime => node.RaceTime;
            public int? Respawns => node.Respawns;
            public Vec3? LightTrailColor => node.LightTrailColor;
            public int? StuntScore => node.StuntScore;
            public TimeSpan[] Checkpoints => node.Checkpoints;
            public string GhostLogin => node.GhostLogin;
            public Ident Vehicle => node.Vehicle;
            public string Skin => node.Skin;
            public string UID => node.UID;
            public int EventsDuration => node.EventsDuration;
            public string[] ControlNames => node.ControlNames;
            public ControlEntry[] ControlEntries => node.ControlEntries;
            public string GameVersion => node.GameVersion;
            public int ExeChecksum => node.ExeChecksum;
            public int OSKind => node.OSKind;
            public int CPUKind => node.CPUKind;
            public string RaceSettingsXML => node.RaceSettingsXML;
            public CPlugEntRecordData RecordData => node.RecordData;
            public string GhostTrigram => node.GhostTrigram;

            public ChunkSet Chunks => node.Chunks;

            public DebugView(CGameCtnGhost node) => this.node = node;
        }

        #endregion
    }
}
