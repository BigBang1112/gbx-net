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

        public CGameCtnReplayRecord(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x03093000)]
        public class Chunk03093000 : HeaderChunk<CGameCtnReplayRecord>
        {
            public int Version { get; set; }
            public Meta MapInfo { get; set; }
            public TimeSpan Time { get; set; }
            public string Nickname { get; set; }
            public string DriverLogin { get; set; }
            public byte Unknown1 { get; set; }
            public string TitleUID { get; set; }

            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
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

        #region 0x001 chunk

        [Chunk(0x03093001)]
        public class Chunk03093001 : HeaderChunk<CGameCtnReplayRecord>
        {
            public string XML { get; set; }

            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                XML = r.ReadString();
            }
        }

        #endregion

        #region 0x002 chunk

        [Chunk(0x03093002)]
        public class Chunk03093002 : Chunk<CGameCtnReplayRecord>
        {
            public int Version { get; set; }

            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                if (IsHeader)
                {
                    Version = r.ReadInt32();
                    n.AuthorVersion = r.ReadInt32();
                    n.AuthorLogin = r.ReadString();
                    n.AuthorNickname = r.ReadString();
                    n.AuthorZone = r.ReadString();
                    n.AuthorExtraInfo = r.ReadString();
                }

                if (IsBody)
                {
                    var size = r.ReadInt32();

                    if (size > 0)
                    {
                        var trackGbx = r.ReadBytes(size);

                        n.Track = Task.Run(() =>
                        {
                            using var ms = new MemoryStream(trackGbx);
                            var gbx = new GameBox<CGameCtnChallenge>();
                            gbx.Read(ms);
                            return gbx;
                        });
                    }
                }
            }
        }

        #endregion

        #region 0x014 chunk

        [Chunk(0x03093014)]
        public class Chunk03093014 : Chunk<CGameCtnReplayRecord>
        {
            public int Version { get; set; }
            public int Unknown1 { get; set; }

            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();
                n.Ghosts = r.ReadArray(i => r.ReadNodeRef<CGameCtnGhost>(true));
                Unknown1 = r.ReadInt32();
                n.Extras = r.ReadArray(i => r.ReadInt64());
            }
        }

        #endregion

        #region 0x015 chunk

        [Chunk(0x03093015)]
        public class Chunk03093015 : Chunk<CGameCtnReplayRecord>
        {
            public int Unknown1 { get; set; }

            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Unknown1 = r.ReadInt32();
            }
        }

        #endregion

        #region 0x024 chunk

        [Chunk(0x03093024)]
        public class Chunk024 : Chunk<CGameCtnReplayRecord>
        {
            public int Unknown1 { get; set; }
            public int Unknown2 { get; set; }
            public Node Unknown3 { get; set; }

            public override void Read(CGameCtnReplayRecord n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Unknown1 = r.ReadInt32();
                Unknown2 = r.ReadInt32();
                Unknown3 = r.ReadNodeRef();
            }
        }

        #endregion

        #endregion
    }
}
