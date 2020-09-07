using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x0305B000)]
    public class CGameCtnChallengeParameters : Node
    {
        /// <summary>
        /// Time of the bronze medal.
        /// </summary>
        public TimeSpan? BronzeTime { get; set; }

        /// <summary>
        /// Time of the silver medal.
        /// </summary>
        public TimeSpan? SilverTime { get; set; }

        /// <summary>
        /// Time of the gold medal.
        /// </summary>
        public TimeSpan? GoldTime { get; set; }

        /// <summary>
        /// Time of the author medal.
        /// </summary>
        public TimeSpan? AuthorTime { get; set; }

        /// <summary>
        /// Usually author time or stunt score.
        /// </summary>
        public int? AuthorScore { get; set; }

        /// <summary>
        /// Stunt time limit.
        /// </summary>
        public TimeSpan? TimeLimit { get; set; }

        public string MapType { get; set; }

        public string MapStyle { get; set; }

        public string[] Tips { get; } = new string[4];

        public CGameCtnChallengeParameters(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {
            
        }

        [Chunk(0x0305B000)]
        public class Chunk0305B000 : Chunk<CGameCtnChallengeParameters>
        {
            public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
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

        [Chunk(0x0305B001)]
        public class Chunk0305B001 : Chunk<CGameCtnChallengeParameters>
        {
            public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
            {
                for(var i = 0; i < 4; i++)
                    n.Tips[i] = rw.String(n.Tips[i]);
            }
        }

        [Chunk(0x0305B002)]
        public class Chunk0305B002 : Chunk<CGameCtnChallengeParameters>
        {
            public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);

                rw.Single(Unknown);
                rw.Single(Unknown);
                rw.Single(Unknown);

                rw.Int32(Unknown);
                rw.Int32(Unknown);
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

        [Chunk(0x0305B003)]
        public class Chunk0305B003 : Chunk<CGameCtnChallengeParameters>
        {
            public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Single(Unknown);

                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);

                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0305B004)]
        public class Chunk0305B004 : Chunk<CGameCtnChallengeParameters>
        {
            public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
            {
                n.BronzeTime = rw.TimeSpan32(n.BronzeTime);
                n.SilverTime = rw.TimeSpan32(n.SilverTime);
                n.GoldTime = rw.TimeSpan32(n.GoldTime);
                n.AuthorTime = rw.TimeSpan32(n.AuthorTime);

                rw.UInt32(Unknown);
            }
        }

        [Chunk(0x0305B005)]
        public class Chunk0305B005 : Chunk<CGameCtnChallengeParameters>
        {
            public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0305B006)]
        public class Chunk0305B006 : Chunk<CGameCtnChallengeParameters>
        {
            public uint[] Items { get; set; }

            public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
            {
                Items = rw.Array(Items, i => rw.Reader.ReadUInt32(), x => rw.Writer.Write(x));
            }
        }

        [Chunk(0x0305B007)]
        public class Chunk0305B007 : Chunk<CGameCtnChallengeParameters>
        {
            public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
            {
                rw.UInt32(Unknown);
            }
        }

        /// <summary>
        /// CGameCtnChallengeParameters 0x008 chunk (stunts)
        /// </summary>
        [Chunk(0x0305B008)]
        public class Chunk0305B008 : Chunk<CGameCtnChallengeParameters>
        {
            public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
            {
                n.TimeLimit = TimeSpan.FromMilliseconds(rw.Int32(Convert.ToInt32(n.TimeLimit?.TotalMilliseconds ?? TimeSpan.FromMinutes(1).TotalMilliseconds)));
                n.AuthorScore = rw.Int32(n.AuthorScore.GetValueOrDefault());
            }
        }

        [Chunk(0x0305B00A)]
        public class Chunk0305B00A : SkippableChunk<CGameCtnChallengeParameters>
        {
            public int Unknown1 { get; set; }

            public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
            {
                Unknown1 = rw.Int32(Unknown1);

                n.BronzeTime = rw.TimeSpan32(n.BronzeTime);
                n.SilverTime = rw.TimeSpan32(n.SilverTime);
                n.GoldTime = rw.TimeSpan32(n.GoldTime);
                n.AuthorTime = rw.TimeSpan32(n.AuthorTime);
                n.TimeLimit = TimeSpan.FromMilliseconds(rw.Int32(Convert.ToInt32(n.TimeLimit?.TotalMilliseconds ?? TimeSpan.FromMinutes(1).TotalMilliseconds)));
                n.AuthorScore = rw.Int32(n.AuthorScore.GetValueOrDefault());
            }
        }

        [Chunk(0x0305B00D)]
        public class Chunk0305B00D : Chunk<CGameCtnChallengeParameters>
        {
            public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0305B00E)]
        public class Chunk0305B00E : SkippableChunk<CGameCtnChallengeParameters>
        {
            public int Unknown1 { get; set; }

            public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
            {
                n.MapType = rw.String(n.MapType);
                n.MapStyle = rw.String(n.MapStyle);
                Unknown1 = rw.Int32(Unknown1);
            }
        }
    }
}
