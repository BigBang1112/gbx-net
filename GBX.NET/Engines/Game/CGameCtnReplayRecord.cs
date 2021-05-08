using GBX.NET.Engines.TrackMania;
using GBX.NET.Engines.Plug;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GBX.NET.Engines.Game
{
    [Node(0x03093000)]
    public class CGameCtnReplayRecord : Node
    {
        #region Fields

        private string titleID;
        private int authorVersion;
        private string authorLogin;
        private string authorNickname;
        private string authorZone;
        private string authorExtraInfo;
        private string game;

        #endregion

        #region Properties

        [NodeMember]
        public Ident MapInfo { get; private set; }

        [NodeMember]
        public TimeSpan? Time { get; private set; }

        [NodeMember]
        public string Nickname { get; private set; }

        [NodeMember]
        public string DriverLogin { get; private set; }

        [NodeMember]
        public string TitleID
        {
            get
            {
                DiscoverChunk<Chunk03093018>();
                return titleID;
            }
            private set
            {
                DiscoverChunk<Chunk03093018>();
                titleID = value;
            }
        }

        [NodeMember]
        public string XML { get; private set; }

        [NodeMember]
        public int AuthorVersion
        {
            get
            {
                DiscoverChunk<Chunk03093018>();
                return authorVersion;
            }
            private set
            {
                DiscoverChunk<Chunk03093018>();
                authorVersion = value;
            }
        }

        [NodeMember]
        public string AuthorLogin
        {
            get
            {
                DiscoverChunk<Chunk03093018>();
                return authorLogin;
            }
            private set
            {
                DiscoverChunk<Chunk03093018>();
                authorLogin = value;
            }
        }

        [NodeMember]
        public string AuthorNickname
        {
            get
            {
                DiscoverChunk<Chunk03093018>();
                return authorNickname;
            }
            private set
            {
                DiscoverChunk<Chunk03093018>();
                authorNickname = value;
            }
        }

        [NodeMember]
        public string AuthorZone
        {
            get
            {
                DiscoverChunk<Chunk03093018>();
                return authorZone;
            }
            private set
            {
                DiscoverChunk<Chunk03093018>();
                authorZone = value;
            }
        }

        [NodeMember]
        public string AuthorExtraInfo
        {
            get
            {
                DiscoverChunk<Chunk03093018>();
                return authorExtraInfo;
            }
            private set
            {
                DiscoverChunk<Chunk03093018>();
                authorExtraInfo = value;
            }
        }

        [NodeMember]
        public Task<GameBox<CGameCtnChallenge>> Challenge { get; private set; }

        [NodeMember]
        public CGameCtnGhost[] Ghosts { get; private set; }

        [NodeMember]
        public long[] Extras { get; private set; }

        [NodeMember]
        public CGameCtnMediaClip Clip { get; private set; }

        [NodeMember]
        public CPlugEntRecordData RecordData { get; private set; }

        [NodeMember]
        public CCtnMediaBlockEventTrackMania Events { get; private set; }

        [NodeMember]
        public int EventsDuration { get; private set; }

        /// <summary>
        /// Inputs (keyboard, pad, wheel) of the replay from TM1.0, TMO, Sunrise and ESWC. For inputs stored in TMU, TMUF and TM2: see <see cref="CGameCtnGhost.ControlEntries"/> in <see cref="Ghosts"/>. TM2020 and Shootmania don't have inputs available at all in replays and ghosts.
        /// </summary>
        [NodeMember]
        public ControlEntry[] ControlEntries { get; private set; }

        [NodeMember]
        public string Game
        {
            get
            {
                DiscoverChunk<Chunk03093008>();
                return game;
            }
            private set
            {
                DiscoverChunk<Chunk03093008>();
                game = value;
            }
        }

        #endregion

        #region Chunks

        #region 0x000 header chunk (basic)

        [Chunk(0x03093000, "basic")]
        public class Chunk03093000 : HeaderChunk<CGameCtnReplayRecord>
        {
            public int Version { get; private set; }
            public byte U01 { get; private set; }

            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();

                if(Version >= 2)
                {
                    n.MapInfo = r.ReadIdent();
                    n.Time = r.ReadTimeSpan();
                    n.Nickname = r.ReadString();

                    if (Version >= 6)
                    {
                        n.DriverLogin = r.ReadString();

                        if (Version >= 8)
                        {
                            U01 = r.ReadByte();
                            n.TitleID = r.ReadId();
                        }
                    }
                }
            }
        }

        #endregion

        #region 0x001 header chunk (xml)

        [Chunk(0x03093001, "xml")]
        public class Chunk03093001 : HeaderChunk<CGameCtnReplayRecord>
        {
            public string XML { get; set; }

            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.XML = r.ReadString();
            }
        }

        #endregion

        #region 0x002 header chunk (author)

        [Chunk(0x03093002, "author")]
        public class Chunk03093002H : HeaderChunk<CGameCtnReplayRecord>
        {
            public int Version { get; set; }
            public int AuthorVersion { get; set; }
            public string AuthorLogin { get; set; }
            public string AuthorNickname { get; set; }
            public string AuthorZone { get; set; }
            public string AuthorExtraInfo { get; set; }

            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();
                n.AuthorVersion = r.ReadInt32();
                n.AuthorLogin = r.ReadString();
                n.AuthorNickname = r.ReadString();
                n.AuthorZone = r.ReadString();
                n.AuthorExtraInfo = r.ReadString();
            }
        }

        #endregion

        #region 0x002 chunk (track)

        [Chunk(0x03093002, "track")]
        public class Chunk03093002B : Chunk<CGameCtnReplayRecord>
        {
            public int Version { get; private set; }

            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                var size = r.ReadInt32();

                if (size > 0)
                {
                    var trackGbx = r.ReadBytes(size);

                    n.Challenge = Task.Run(() =>
                    {
                        using (var ms = new MemoryStream(trackGbx))
                            return GameBox.Parse<CGameCtnChallenge>(ms);
                    });

                    n.Challenge.ContinueWith(x =>
                    {
#if DEBUG
                        if (x.IsFaulted)
                        {
                            var e = x.Exception.InnerException;
                            Debug.WriteLine(e.Message);
                            Debug.WriteLine(e.StackTrace);
                        }
#endif
                    });
                }
            }
        }

        #endregion

        #region 0x003 chunk

        [Chunk(0x03093003)]
        public class Chunk03093003 : Chunk<CGameCtnReplayRecord>
        {
            public int U01 { get; private set; }
            public int U02 { get; private set; }

            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.EventsDuration = r.ReadInt32();
                if (n.EventsDuration != 0)
                {
                    U01 = r.ReadInt32();

                    // All control names available in the game
                    var controlNames = r.ReadArray(() =>
                    {
                        // Maybe bindings
                        r.ReadInt32(); 
                        r.ReadInt32();

                        return r.ReadString(); // Input name
                    });

                    var numEntries = r.ReadInt32() - 1;

                    n.ControlEntries = new ControlEntry[numEntries];

                    for (var i = 0; i < numEntries; i++)
                    {
                        var time = TimeSpan.FromMilliseconds(r.ReadInt32() - 10000);
                        var controlNameIndex = r.ReadInt32();
                        var data = r.ReadUInt32();

                        var name = controlNames[controlNameIndex];

                        switch (name)
                        {
                            case "Steer (analog)": // Data is bugged
                                n.ControlEntries[i] = new ControlEntryAnalog() { Name = name, Time = time, Data = data };
                                break;
                            default:
                                n.ControlEntries[i] = new ControlEntry() { Name = name, Time = time, Data = data };
                                break;
                        }
                    }

                    Array.Reverse(n.ControlEntries); // Inputs are originally reversed

                    U02 = r.ReadInt32();
                }
            }
        }

        #endregion

        #region 0x004 chunk (ghosts)

        [Chunk(0x03093004, "ghosts")]
        public class Chunk03093004 : Chunk<CGameCtnReplayRecord>
        {
            public int Version { get; private set; }

            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();
                var u02 = r.ReadInt32();

                n.Ghosts = r.ReadArray(() => r.ReadNodeRef<CGameCtnGhost>());

                var u03 = r.ReadInt32(); // millisecond length of something (usually record time + 0.5s)
                var u04 = r.ReadInt32();
            }
        }

        #endregion

        #region 0x005 chunk

        [Chunk(0x03093005)]
        public class Chunk03093005 : Chunk<CGameCtnReplayRecord>
        {
            public int U01 { get; private set; }

            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                U01 = r.ReadInt32();
            }
        }

        #endregion

        #region 0x007 skippable chunk

        [Chunk(0x03093007)]
        public class Chunk03093007 : SkippableChunk<CGameCtnReplayRecord>
        {
            public int U01 { get; private set; }

            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                U01 = r.ReadInt32();
            }
        }

        #endregion

        #region 0x008 skippable chunk (game)

        [Chunk(0x03093008, "game")]
        public class Chunk03093008 : SkippableChunk<CGameCtnReplayRecord>
        {
            public int U01 { get; private set; }

            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.Game = r.ReadString();
                U01 = r.ReadInt32();
            }
        }

        #endregion

        #region 0x00C chunk (clip)

        [Chunk(0x0309300C, "clip")]
        public class Chunk0309300C : Chunk<CGameCtnReplayRecord>
        {
            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.Clip = r.ReadNodeRef<CGameCtnMediaClip>();
            }
        }

        #endregion

        #region 0x00D chunk (validation)

        [Chunk(0x0309300D, "validation")]
        public class Chunk0309300D : Chunk<CGameCtnReplayRecord>
        {
            public int U01 { get; private set; }
            public int U02 { get; private set; }

            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.EventsDuration = r.ReadInt32();

                if (n.EventsDuration != 0)
                {
                    U01 = r.ReadInt32();

                    var controlNames = r.ReadArray(() => r.ReadId());

                    var numEntries = r.ReadInt32();
                    U02 = r.ReadInt32();

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
            }
        }

        #endregion

        #region 0x00E chunk (events)

        [Chunk(0x0309300E, "events")]
        public class Chunk0309300E : Chunk<CGameCtnReplayRecord>
        {
            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.Events = r.ReadNodeRef<CCtnMediaBlockEventTrackMania>();
            }
        }

        #endregion

        #region 0x010 chunk

        [Chunk(0x03093010)]
        [IgnoreChunk]
        public class Chunk03093010 : Chunk<CGameCtnReplayRecord>
        {
            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                
            }
        }

        #endregion

        #region 0x011 chunk

        [Chunk(0x03093011)]
        public class Chunk03093011 : Chunk<CGameCtnReplayRecord>
        {
            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                
            }
        }

        #endregion

        // 0x013 skippable chunk

        #region 0x014 chunk (ghosts)

        [Chunk(0x03093014, "ghosts")]
        public class Chunk03093014 : Chunk<CGameCtnReplayRecord>
        {
            public int Version { get; private set; }
            public int U01 { get; private set; }

            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();
                n.Ghosts = r.ReadArray(() => r.ReadNodeRef<CGameCtnGhost>());
                U01 = r.ReadInt32();
                n.Extras = r.ReadArray(() => r.ReadInt64());
            }
        }

        #endregion

        #region 0x015 chunk (clip)

        [Chunk(0x03093015, "clip")]
        public class Chunk03093015 : Chunk<CGameCtnReplayRecord>
        {
            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.Clip = r.ReadNodeRef<CGameCtnMediaClip>();
            }
        }

        #endregion

        #region 0x018 skippable chunk

        [Chunk(0x03093018, "author")]
        public class Chunk03093018 : SkippableChunk<CGameCtnReplayRecord>
        {
            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
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

        #region 0x01C skippable chunk

        [Chunk(0x0309301C)]
        public class Chunk0309301C : SkippableChunk<CGameCtnReplayRecord>
        {
            public int Version { get; private set; }

            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();
                r.ReadString();
            }
        }

        #endregion

        #region 0x024 chunk

        [Chunk(0x03093024)]
        public class Chunk03093024 : Chunk<CGameCtnReplayRecord>
        {
            public int U01 { get; private set; }
            public int U02 { get; private set; }

            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                U01 = r.ReadInt32();
                U02 = r.ReadInt32();
                n.RecordData = r.ReadNodeRef<CPlugEntRecordData>();
            }
        }

        #endregion

        #endregion
    }
}
