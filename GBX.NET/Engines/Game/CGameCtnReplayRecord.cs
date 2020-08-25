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
        public GameBox<CGameCtnChallenge> Track
        {
            get => GetValue<Chunk002>(x => {
                if (x.Track == null) return null;
                return x.Track.Result;
            }) as GameBox<CGameCtnChallenge>;
        }

        public CGameCtnReplayRecord(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x03093000)]
        public class Chunk000 : Chunk
        {
            public int Version { get; set; }
            public Meta MapInfo { get; set; }
            public TimeSpan Time { get; set; }
            public string Nickname { get; set; }
            public string DriverLogin { get; set; }
            public byte Unknown1 { get; set; }
            public string TitleUID { get; set; }

            public Chunk000(CGameCtnReplayRecord node) : base(node)
            {
                
            }

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

        #region 0x001 chunk

        [Chunk(0x03093001)]
        public class Chunk001 : Chunk
        {
            public string XML { get; set; }

            public Chunk001(CGameCtnReplayRecord node) : base(node)
            {
                
            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                XML = r.ReadString();
            }
        }

        #endregion

        #region 0x002 chunk

        [Chunk(0x03093002)]
        public class Chunk002 : Chunk
        {
            public int? Version { get; set; }
            public int? AuthorVersion { get; set; }
            public string AuthorLogin { get; set; }
            public string AuthorNickname { get; set; }
            public string AuthorZone { get; set; }
            public string AuthorExtraInfo { get; set; }
            public Task<GameBox<CGameCtnChallenge>> Track { get; set; }

            public Chunk002(CGameCtnReplayRecord node) : base(node)
            {
                
            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                if (IsHeader)
                {
                    Version = r.ReadInt32();
                    AuthorVersion = r.ReadInt32();
                    AuthorLogin = r.ReadString();
                    AuthorNickname = r.ReadString();
                    AuthorZone = r.ReadString();
                    AuthorExtraInfo = r.ReadString();
                }

                if (IsBody)
                {
                    var size = r.ReadInt32();

                    if (size > 0)
                    {
                        var trackGbx = r.ReadBytes(size);

                        Track = Task.Run(() =>
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
        public class Chunk014 : Chunk
        {
            public int Version { get; private set; }
            public CGameCtnGhost[] Ghosts { get; private set; }
            public int Unknown1 { get; set; }
            public long[] Extras { get; private set; }

            public Chunk014(CGameCtnReplayRecord node) : base(node)
            {
                
            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();
                Ghosts = r.ReadArray(i => r.ReadNodeRef<CGameCtnGhost>(true));
                Unknown1 = r.ReadInt32();
                Extras = r.ReadArray(i => r.ReadInt64());
            }
        }

        #endregion

        #region 0x015 chunk

        [Chunk(0x03093015)]
        public class Chunk015 : Chunk
        {
            public int Unknown1 { get; set; }

            public Chunk015(CGameCtnReplayRecord node) : base(node)
            {
                
            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                Unknown1 = r.ReadInt32();
            }
        }

        #endregion

        #region 0x024 chunk

        [Chunk(0x03093024)]
        public class Chunk024 : Chunk
        {
            public int Unknown1 { get; set; }
            public int Unknown2 { get; set; }
            public Node Unknown3 { get; set; }

            public Chunk024(CGameCtnReplayRecord node) : base(node)
            {
                
            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
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
