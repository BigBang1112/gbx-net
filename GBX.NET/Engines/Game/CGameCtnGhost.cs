using GBX.NET.Engines.Plug;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// Ghost (0x03092000)
    /// </summary>
    [Node(0x03092000)]
    public class CGameCtnGhost : CGameGhost
    {
        #region Fields

        private Meta playerModel;
        private FileRef[] skinPackDescs;
        private string ghostNickname;
        private string ghostAvatarName;
        private string recordingContext;
        private TimeSpan? raceTime;
        private int? respawns;
        private Vector3? lightTrailColor;
        private int? stuntScore;
        private TimeSpan[] checkpoints;

        #endregion

        #region Properties

        public Meta PlayerModel
        {
            get
            {
                DiscoverChunk<Chunk03092000>();
                return playerModel;
            }
            set => playerModel = value;
        }

        public FileRef[] SkinPackDescs
        {
            get
            {
                DiscoverChunks<Chunk03092000, Chunk03092017>();
                return skinPackDescs;
            }
            set => skinPackDescs = value;
        }

        public string GhostNickname
        {
            get
            {
                DiscoverChunks<Chunk03092000, Chunk03092017>();
                return ghostNickname;
            }
            set => ghostNickname = value;
        }

        public string GhostAvatarName
        {
            get
            {
                DiscoverChunks<Chunk03092000, Chunk03092017>();
                return ghostAvatarName;
            }
            set => ghostAvatarName = value;
        }

        public string RecordingContext
        {
            get
            {
                DiscoverChunk<Chunk03092000>();
                return recordingContext;
            }
            set => recordingContext = value;
        }

        public TimeSpan? RaceTime
        {
            get
            {
                DiscoverChunk<Chunk03092005>();
                return raceTime;
            }
            set => raceTime = value;
        }

        public int? Respawns
        {
            get
            {
                DiscoverChunk<Chunk03092008>();
                return respawns;
            }
            set => respawns = value;
        }

        public Vector3? LightTrailColor
        {
            get
            {
                DiscoverChunk<Chunk03092009>();
                return lightTrailColor;
            }
            set => lightTrailColor = value;
        }

        public int? StuntScore
        {
            get
            {
                DiscoverChunk<Chunk0309200A>();
                return stuntScore;
            }
            set => stuntScore = value;
        }

        public TimeSpan[] Checkpoints
        {
            get
            {
                DiscoverChunk<Chunk0309200B>();
                return checkpoints;
            }
            set => checkpoints = value;
        }

        public string GhostLogin { get; set; }
        public Meta Vehicle { get; set; }
        public string Skin { get; set; }
        public string UID { get; set; }

        public int EventsDuration { get; set; }
        public string[] ControlNames { get; set; }
        public ControlEntry[] ControlEntries { get; set; } = new ControlEntry[0];
        public string GameVersion { get; set; }
        public int ExeChecksum { get; set; }
        public int OSKind { get; set; }
        public int CPUKind { get; set; }
        public string RaceSettingsXML { get; set; }

        public CPlugEntRecordData RecordData { get; set; }
        public string GhostTrigram { get; set; }

        #endregion

        public CGameCtnGhost(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {
            
        }

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
                n.PlayerModel = rw.Meta(n.playerModel);
                rw.Vec3(Unknown); // unknown
                n.SkinPackDescs = rw.Array(n.skinPackDescs,
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

        #region 0x005 skippable chunk (race time)

        /// <summary>
        /// CGameCtnGhost 0x005 skippable chunk (race time)
        /// </summary>
        [Chunk(0x03092005, "race time")]
        public class Chunk03092005 : SkippableChunk<CGameCtnGhost>
        {
            public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
            {
                n.RaceTime = TimeSpan.FromMilliseconds(rw.Int32(Convert.ToInt32(n.raceTime.GetValueOrDefault().TotalMilliseconds)));
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
                n.Vehicle = rw.Meta(n.Vehicle);
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
                n.Respawns = rw.Int32(n.respawns.GetValueOrDefault());
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
                n.LightTrailColor = rw.Vec3(n.lightTrailColor.GetValueOrDefault());
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
                n.StuntScore = rw.Int32(n.stuntScore.GetValueOrDefault());
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
                n.Checkpoints = rw.Array(n.checkpoints,
                    i => TimeSpan.FromMilliseconds(rw.Reader.ReadInt64()),
                    x => rw.Writer.Write(Convert.ToInt64(x.TotalMilliseconds)));
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
                n.Vehicle = rw.Meta(n.Vehicle);
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
                n.UID = rw.LookbackString(n.UID);
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
                rw.LookbackString(Unknown);
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
                n.Vehicle = new Meta(rw.LookbackString(n.Vehicle?.ID));
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
                n.SkinPackDescs = rw.Array(n.SkinPackDescs, i => rw.Reader.ReadFileRef(), x => rw.Writer.Write(x));
                n.GhostNickname = rw.String(n.GhostNickname);
                n.GhostAvatarName = rw.String(n.GhostAvatarName);
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
            public Meta Unknown1 { get; set; }

            public override void ReadWrite(CGameCtnGhost n, GameBoxReaderWriter rw)
            {
                Unknown1 = rw.Meta(Unknown1);
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
                    n.ControlNames = rw.Array(n.ControlNames, i => rw.Reader.ReadLookbackString(), x => rw.Writer.WriteLookbackString(x));

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

        public class ControlEntry
        {
            public int Time { get; set; }
            public byte Index { get; set; }
            public bool Enabled { get; set; }
        }
    }
}
