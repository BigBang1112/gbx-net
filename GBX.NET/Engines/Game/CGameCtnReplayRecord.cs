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
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnReplayRecord : Node
    {
        #region Fields

        private string titleID;
        private int authorVersion;
        private string authorLogin;
        private string authorNickname;
        private string authorZone;
        private string authorExtraInfo;

        #endregion

        #region Properties

        [NodeMember]
        public Ident MapInfo { get; set; }

        [NodeMember]
        public TimeSpan? Time { get; set; }

        [NodeMember]
        public string Nickname { get; set; }

        [NodeMember]
        public string DriverLogin { get; set; }

        [NodeMember]
        public string TitleID
        {
            get
            {
                DiscoverChunk<Chunk03093018>();
                return titleID;
            }
            set
            {
                DiscoverChunk<Chunk03093018>();
                titleID = value;
            }
        }

        [NodeMember]
        public string XML { get; set; }

        [NodeMember]
        public int AuthorVersion
        {
            get
            {
                DiscoverChunk<Chunk03093018>();
                return authorVersion;
            }
            set
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
            set
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
            set
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
            set
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
            set
            {
                DiscoverChunk<Chunk03093018>();
                authorExtraInfo = value;
            }
        }

        [NodeMember]
        public Task<GameBox<CGameCtnChallenge>> Challenge { get; set; }

        [NodeMember]
        public CGameCtnGhost[] Ghosts { get; set; }

        [NodeMember]
        public long[] Extras { get; set; }

        [NodeMember]
        public CGameCtnMediaClip Clip { get; set; }

        [NodeMember]
        public CPlugEntRecordData RecordData { get; set; }

        [NodeMember]
        public CCtnMediaBlockEventTrackMania Events { get; set; }

        #endregion

        #region Chunks

        #region 0x000 header chunk (basic)

        [Chunk(0x03093000, "basic")]
        public class Chunk03093000 : HeaderChunk<CGameCtnReplayRecord>
        {
            public int Version { get; set; }

            public byte U01 { get; set; }

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
            public int Version { get; set; }

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
            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                var v = r.ReadInt32();
                if (v != 0)
                {
                    r.ReadInt32();
                    var controlNames = r.ReadArray(i =>
                    {
                        r.ReadInt32();
                        r.ReadInt32();
                        return r.ReadString();
                    });

                    var numControlEntries = r.ReadInt32() - 1;
                    var controlEntries = new (int, int, int)[numControlEntries];

                    for (var i = 0; i < numControlEntries; i++)
                        controlEntries[i] = (r.ReadInt32(), r.ReadInt32(), r.ReadInt32());

                    r.ReadInt32();
                }
            }
        }

        #endregion

        #region 0x004 chunk

        [Chunk(0x03093004)]
        public class Chunk03093004 : Chunk<CGameCtnReplayRecord>
        {
            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                var u01 = r.ReadInt32();
                var u02 = r.ReadInt32();

                n.Ghosts = r.ReadArray(i => r.ReadNodeRef<CGameCtnGhost>());

                var u03 = r.ReadInt32(); // millisecond length of something (usually record time + 0.5s)
                var u04 = r.ReadInt32();
            }
        }

        #endregion

        #region 0x005 chunk

        [Chunk(0x03093005)]
        public class Chunk03093005 : Chunk<CGameCtnReplayRecord>
        {
            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                var u01 = r.ReadInt32();
            }
        }

        #endregion

        #region 0x00C chunk

        [Chunk(0x0309300C)]
        public class Chunk0309300C : Chunk<CGameCtnReplayRecord>
        {
            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.Clip = r.ReadNodeRef<CGameCtnMediaClip>();
            }
        }

        #endregion

        #region 0x00D chunk

        [Chunk(0x0309300D)]
        public class Chunk0309300D : Chunk<CGameCtnReplayRecord>
        {
            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                var unknown = r.ReadInt32();
                if (unknown != 0)
                {
                    r.ReadInt32();

                    var controlNames = r.ReadArray<string>(i => r.ReadId());

                    var num = r.ReadInt32();
                    r.ReadInt32();

                    var array = new (int time, int index, byte enabled)[num];

                    for (var i = 0; i < num; i++)
                        array[i] = (r.ReadInt32(), r.ReadInt32(), r.ReadByte());
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

        #region 0x014 chunk

        [Chunk(0x03093014)]
        public class Chunk03093014 : Chunk<CGameCtnReplayRecord>
        {
            public int Version { get; set; }
            public int Unknown1 { get; set; }

            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();
                n.Ghosts = r.ReadArray(i => r.ReadNodeRef<CGameCtnGhost>());
                Unknown1 = r.ReadInt32();
                n.Extras = r.ReadArray(i => r.ReadInt64());
            }
        }

        #endregion

        #region 0x015 chunk

        [Chunk(0x03093015)]
        public class Chunk03093015 : Chunk<CGameCtnReplayRecord>
        {
            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.Clip = r.ReadNodeRef<CGameCtnMediaClip>();
            }
        }

        #endregion

        #region 0x018 chunk

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

        #region 0x01C chunk

        [Chunk(0x0309301C)]
        public class Chunk0309301C : SkippableChunk<CGameCtnReplayRecord>
        {
            public int Version { get; set; }

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
            public int Unknown1 { get; set; }
            public int Unknown2 { get; set; }

            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Unknown1 = r.ReadInt32();
                Unknown2 = r.ReadInt32();
                n.RecordData = r.ReadNodeRef<CPlugEntRecordData>();
            }
        }

        #endregion

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnReplayRecord node;

            public Ident MapInfo => node.MapInfo;
            public TimeSpan? Time => node.Time;
            public string Nickname => node.Nickname;
            public string DriverLogin => node.DriverLogin;
            public string TitleID => node.TitleID;
            public string XML => node.XML;
            public int? AuthorVersion => node.AuthorVersion;
            public string AuthorLogin => node.AuthorLogin;
            public string AuthorNickname => node.AuthorNickname;
            public string AuthorZone => node.AuthorZone;
            public string AuthorExtraInfo => node.AuthorExtraInfo;

            public Task<GameBox<CGameCtnChallenge>> Challenge => node.Challenge;
            public CGameCtnGhost[] Ghosts => node.Ghosts;
            public long[] Extras => node.Extras;
            public CGameCtnMediaClip Clip => node.Clip;
            public CPlugEntRecordData RecordData => node.RecordData;
            public CCtnMediaBlockEventTrackMania Events => node.Events;

            public ChunkSet Chunks => node.Chunks;

            public DebugView(CGameCtnReplayRecord node) => this.node = node;
        }

        #endregion
    }
}
