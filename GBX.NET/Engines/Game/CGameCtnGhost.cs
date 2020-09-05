using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03092000)]
    public class CGameCtnGhost : CGameGhost
    {
        public CGameCtnGhost(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {
            
        }

        #region Chunks

        #region 0x000 skippable chunk (basic)

        [Chunk(0x03092000)]
        public class Chunk000 : SkippableChunk
        {
            public int Version { get; set; }
            public Meta Model { get; set; }
            public FileRef[] SkinFiles { get; set; }
            public string GhostNickname { get; set; }
            public string GhostAvatarName { get; set; }
            public string RecordingContext { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                Model = rw.Meta(Model);
                rw.Vec3(Unknown); // unknown
                SkinFiles = rw.Array(SkinFiles,
                    i => rw.Reader.ReadFileRef(),
                    x => rw.Writer.Write(x));
                rw.Int32(Unknown); // unknown
                GhostNickname = rw.String(GhostNickname);
                GhostAvatarName = rw.String(GhostAvatarName);
                if(Version >= 2)
                    RecordingContext = rw.String(RecordingContext);
            }
        }

        #endregion

        #region 0x005 skippable chunk (race time)

        [Chunk(0x03092005)]
        public class Chunk005_2 : SkippableChunk
        {
            public TimeSpan RaceTime { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                RaceTime = TimeSpan.FromMilliseconds(rw.Int32(Convert.ToInt32(RaceTime.TotalMilliseconds)));
            }
        }

        #endregion

        #region 0x008 skippable chunk (respawns)

        [Chunk(0x03092008)]
        public class Chunk008 : SkippableChunk
        {
            public int Respawns { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Respawns = rw.Int32(Respawns);
            }
        }

        #endregion

        #region 0x009 skippable chunk (light trail color)

        [Chunk(0x03092009)]
        public class Chunk009 : SkippableChunk
        {
            public Vector3 LightTrailColor { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                LightTrailColor = rw.Vec3(LightTrailColor);
            }
        }

        #endregion

        #region 0x00A skippable chunk (stunt score)

        [Chunk(0x0309200A)]
        public class Chunk00A : SkippableChunk
        {
            public int StuntScore { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                StuntScore = rw.Int32(StuntScore);
            }
        }

        #endregion

        #region 0x00B skippable chunk (checkpoint times)

        [Chunk(0x0309200B)]
        public class Chunk00B : SkippableChunk
        {
            public TimeSpan[] Checkpoints { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Checkpoints = rw.Array(Checkpoints,
                    i => TimeSpan.FromMilliseconds(rw.Reader.ReadInt64()),
                    x => rw.Writer.Write(Convert.ToInt64(x.TotalMilliseconds)));
            }
        }

        #endregion

        #region 0x00C chunk

        [Chunk(0x0309200C)]
        public class Chunk00C : Chunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x00E chunk

        [Chunk(0x0309200E)]
        public class Chunk00E : Chunk
        {
            public string UID { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                UID = rw.LookbackString(UID);
            }
        }

        #endregion

        #region 0x00F chunk (ghost login)

        [Chunk(0x0309200F)]
        public class Chunk00F : Chunk
        {
            public string GhostLogin { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                GhostLogin = rw.String(GhostLogin);
            }
        }

        #endregion

        #region 0x010 chunk

        [Chunk(0x03092010)]
        public class Chunk010 : Chunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.LookbackString(Unknown);
            }
        }

        #endregion

        #region 0x012 chunk

        [Chunk(0x03092012)]
        public class Chunk012 : Chunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int64(Unknown);
                rw.Int64(Unknown);
            }
        }

        #endregion

        #region 0x013 skippable chunk

        [Chunk(0x03092013)]
        public class Chunk013 : SkippableChunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x014 skippable chunk

        [Chunk(0x03092014)]
        public class Chunk014 : SkippableChunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x015 chunk

        [Chunk(0x03092015)]
        public class Chunk015 : Chunk
        {
            public string VehicleID { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                VehicleID = rw.LookbackString(VehicleID);
            }
        }

        #endregion

        #region 0x017 skippable chunk

        [Chunk(0x03092017)]
        public class Chunk017 : SkippableChunk
        {
            public FileRef[] SkinPackDescs { get; set; }
            public string GhostNickname { get; set; }
            public string GhostAvatarName { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                SkinPackDescs = rw.Array(SkinPackDescs, i => rw.Reader.ReadFileRef(), x => rw.Writer.Write(x));
                GhostNickname = rw.String(GhostNickname);
                GhostAvatarName = rw.String(GhostAvatarName);
            }
        }

        #endregion

        #region 0x018 chunk

        [Chunk(0x03092018)]
        public class Chunk018 : Chunk
        {
            public Meta Meta { get; set; }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                Meta = r.ReadMeta();
            }

            public override void Write(GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Meta);
            }
        }

        #endregion

        #region 0x019 chunk

        [Chunk(0x03092019)]
        public class Chunk019 : Chunk
        {
            public int EventsDuration { get; set; }
            public uint Unknown1 { get; set; }
            public string[] ControlNames { get; set; }
            public int Unknown2 { get; set; }
            public ControlEntry[] ControlEntries { get; set; } = new ControlEntry[0];
            public string GameVersion { get; set; }
            public int ExeChecksum { get; set; }
            public int OSKind { get; set; }
            public int CPUKind { get; set; }
            public string RaceSettingsXML { get; set; }
            public int Unknown3 { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                EventsDuration = rw.Int32(EventsDuration);

                if (EventsDuration > 0)
                {
                    Unknown1 = rw.UInt32(Unknown1);
                    ControlNames = rw.Array(ControlNames, i => rw.Reader.ReadLookbackString(), x => rw.Writer.WriteLookbackString(x));

                    var numEntries = rw.Int32(ControlEntries.Length);

                    if (rw.Mode == GameBoxReaderWriterMode.Read)
                    {
                        Unknown2 = rw.Reader.ReadInt32();

                        ControlEntries = new ControlEntry[numEntries];

                        for (var i = 0; i < numEntries; i++)
                        {
                            var time = rw.Reader.ReadInt32();
                            var controlNameIndex = rw.Reader.ReadByte();
                            var enabled = rw.Reader.ReadBoolean();

                            ControlEntries[i] = new ControlEntry()
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

                    GameVersion = rw.String(GameVersion);
                    ExeChecksum = rw.Int32(ExeChecksum);
                    OSKind = rw.Int32(OSKind);
                    CPUKind = rw.Int32(CPUKind);
                    RaceSettingsXML = rw.String(RaceSettingsXML);
                    Unknown2 = rw.Int32(Unknown2);
                }
            }

            public class ControlEntry
            {
                public int Time { get; set; }
                public byte Index { get; set; }
                public bool Enabled { get; set; }
            }
        }

        #endregion

        #region 0x01C chunk

        [Chunk(0x0309201C)]
        public class Chunk01C : Chunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
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
    }
}
