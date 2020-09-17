using GBX.NET.Engines.TrackMania;
using GBX.NET.Engines.Plug;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Engines.Game
{
    [Node(0x03093000)]
    public class CGameCtnReplayRecord : Node
    {
        public Task<GameBox<CGameCtnChallenge>> Track { get; set; }

        public int? AuthorVersion { get; set; }
        public string AuthorLogin { get; set; }
        public string AuthorNickname { get; set; }
        public string AuthorZone { get; set; }
        public string AuthorExtraInfo { get; set; }

        public CGameCtnGhost[] Ghosts { get; set; }
        public long[] Extras { get; set; }
        public CGameCtnMediaClip Clip { get; set; }
        public CPlugEntRecordData RecordData { get; set; }
        public CCtnMediaBlockEventTrackMania Events { get; set; }

        public CGameCtnReplayRecord(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x000 chunk (basic)

        [Chunk(0x03093000, "basic")]
        public class Chunk03093000 : HeaderChunk<CGameCtnReplayRecord>
        {
            public int Version { get; set; }
            public Meta MapInfo { get; set; }
            public TimeSpan Time { get; set; }
            public string Nickname { get; set; }
            public string DriverLogin { get; set; }
            public byte Unknown1 { get; set; }
            public string TitleUID { get; set; }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();

                if(Version >= 2)
                {
                    MapInfo = r.ReadMeta();
                    Time = TimeSpan.FromMilliseconds(r.ReadInt32());
                    Nickname = r.ReadString();

                    if (Version >= 6)
                    {
                        DriverLogin = r.ReadString();

                        if (Version >= 8)
                        {
                            Unknown1 = r.ReadByte();
                            TitleUID = r.ReadLookbackString();
                        }
                    }
                }
            }
        }

        #endregion

        #region 0x001 chunk (xml)

        [Chunk(0x03093001, "xml")]
        public class Chunk03093001 : HeaderChunk<CGameCtnReplayRecord>
        {
            public string XML { get; set; }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                XML = r.ReadString();
            }
        }

        #endregion

        #region 0x002 chunk (author)

        [Chunk(0x03093002, "author")]
        public class Chunk03093002H : HeaderChunk<CGameCtnReplayRecord>
        {
            public int Version { get; set; }
            public int AuthorVersion { get; set; }
            public string AuthorLogin { get; set; }
            public string AuthorNickname { get; set; }
            public string AuthorZone { get; set; }
            public string AuthorExtraInfo { get; set; }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();
                AuthorVersion = r.ReadInt32();
                AuthorLogin = r.ReadString();
                AuthorNickname = r.ReadString();
                AuthorZone = r.ReadString();
                AuthorExtraInfo = r.ReadString();
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

                    n.Track = Task.Run(() =>
                    {
                        using (var ms = new MemoryStream(trackGbx))
                        {
                            var gbx = new GameBox<CGameCtnChallenge>();
                            gbx.Read(ms);
                            return gbx;
                        }
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
                var gsdgs = r.ReadArray<int>(2);
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

        #endregion

        #region 0x004 chunk

        [Chunk(0x03093004)]
        public class Chunk03093004 : Chunk<CGameCtnReplayRecord>
        {
            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                r.ReadInt32();
                r.ReadInt32();

                n.Ghosts = r.ReadArray(i => r.ReadNodeRef<CGameCtnGhost>());

                r.ReadInt32();
                r.ReadInt32();
            }
        }

        #endregion

        #region 0x00C chunk

        [Chunk(0x0309300C)]
        public class Chunk0309300C : Chunk<CGameCtnReplayRecord>
        {
            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                r.ReadNodeRef();
            }
        }

        #endregion

        #region 0x00D chunk

        [Chunk(0x0309300D)]
        public class Chunk0309300D : Chunk<CGameCtnReplayRecord>
        {
            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                r.ReadInt32();
                r.ReadInt32();

                var controlNames = r.ReadArray<string>(i => r.ReadLookbackString());

                var num = r.ReadInt32();
                r.ReadInt32();

                var array = new (int time, int index, byte enabled)[num];

                for(var i = 0; i < num; i++)
                    array[i] = (r.ReadInt32(), r.ReadInt32(), r.ReadByte());
            }
        }

        #endregion

        #region 0x00E chunk

        [Chunk(0x0309300E)]
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
    }
}
