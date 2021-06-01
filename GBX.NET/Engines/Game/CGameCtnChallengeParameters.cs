using System;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// Map parameters (0x0305B000)
    /// </summary>
    [Node(0x0305B000)]
    public class CGameCtnChallengeParameters : Node
    {
        #region Fields

        private string mapType;
        private string mapStyle;

        #endregion

        #region Properties

        /// <summary>
        /// Time of the bronze medal.
        /// </summary>
        [NodeMember]
        public TimeSpan? BronzeTime { get; set; }

        /// <summary>
        /// Time of the silver medal.
        /// </summary>
        [NodeMember]
        public TimeSpan? SilverTime { get; set; }

        /// <summary>
        /// Time of the gold medal.
        /// </summary>
        [NodeMember]
        public TimeSpan? GoldTime { get; set; }

        /// <summary>
        /// Time of the author medal.
        /// </summary>
        [NodeMember]
        public TimeSpan? AuthorTime { get; set; }

        /// <summary>
        /// Usually author time or stunt score.
        /// </summary>
        [NodeMember]
        public int AuthorScore { get; set; }

        /// <summary>
        /// Stunt time limit.
        /// </summary>
        [NodeMember]
        public TimeSpan TimeLimit { get; set; } = TimeSpan.FromMinutes(1);

        [NodeMember]
        public CGameCtnGhost RaceValidateGhost { get; set; }

        [NodeMember]
        public string MapType
        {
            get
            {
                DiscoverChunk<Chunk0305B00E>();
                return mapType;
            }
            set
            {
                DiscoverChunk<Chunk0305B00E>();
                mapType = value;
            }
        }

        [NodeMember]
        public string MapStyle
        {
            get
            {
                DiscoverChunk<Chunk0305B00E>();
                return mapStyle;
            }
            set
            {
                DiscoverChunk<Chunk0305B00E>();
                mapStyle = value;
            }
        }

        [NodeMember]
        public string[] Tips { get; } = new string[4];

        #endregion

        #region Chunks

        #region 0x000 chunk

        /// <summary>
        /// CGameCtnChallengeParameters 0x000 chunk
        /// </summary>
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

        #endregion

        #region 0x001 chunk (tips)

        /// <summary>
        /// CGameCtnChallengeParameters 0x001 chunk (tips)
        /// </summary>
        [Chunk(0x0305B001, "tips")]
        public class Chunk0305B001 : Chunk<CGameCtnChallengeParameters>
        {
            public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
            {
                for(var i = 0; i < 4; i++)
                    n.Tips[i] = rw.String(n.Tips[i]);
            }
        }

        #endregion

        #region 0x002 chunk

        /// <summary>
        /// CGameCtnChallengeParameters 0x002 chunk
        /// </summary>
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

        #endregion

        #region 0x003 chunk

        /// <summary>
        /// CGameCtnChallengeParameters 0x003 chunk
        /// </summary>
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

        #endregion

        #region 0x004 chunk (medals)

        /// <summary>
        /// CGameCtnChallengeParameters 0x004 chunk (medals)
        /// </summary>
        [Chunk(0x0305B004, "medals")]
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

        #endregion

        #region 0x005 chunk

        /// <summary>
        /// CGameCtnChallengeParameters 0x005 chunk
        /// </summary>
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

        #endregion

        #region 0x006 chunk (items)

        /// <summary>
        /// CGameCtnChallengeParameters 0x006 chunk (items). This chunk causes "Couldn't load map" in ManiaPlanet.
        /// </summary>
        [Chunk(0x0305B006, "items")]
        public class Chunk0305B006 : Chunk<CGameCtnChallengeParameters>
        {
            public uint[] Items { get; set; }

            public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
            {
                Items = rw.Array(Items, r => r.ReadUInt32(), (x, w) => w.Write(x));
            }
        }

        #endregion

        #region 0x007 chunk

        /// <summary>
        /// CGameCtnChallengeParameters 0x007 chunk
        /// </summary>
        [Chunk(0x0305B007)]
        public class Chunk0305B007 : Chunk<CGameCtnChallengeParameters>
        {
            public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
            {
                rw.UInt32(Unknown);
            }
        }

        #endregion

        #region 0x008 chunk (stunts)

        /// <summary>
        /// CGameCtnChallengeParameters 0x008 chunk (stunts)
        /// </summary>
        [Chunk(0x0305B008, "stunts")]
        public class Chunk0305B008 : Chunk<CGameCtnChallengeParameters>
        {
            public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
            {
                n.TimeLimit = TimeSpan.FromMilliseconds(rw.Int32(Convert.ToInt32(n.TimeLimit.TotalMilliseconds)));
                n.AuthorScore = rw.Int32(n.AuthorScore);
            }
        }

        #endregion

        #region 0x00A chunk (medals)

        /// <summary>
        /// CGameCtnChallengeParameters 0x00A chunk (medals)
        /// </summary>
        [Chunk(0x0305B00A, "medals")]
        public class Chunk0305B00A : SkippableChunk<CGameCtnChallengeParameters>
        {
            public int U01 { get; set; }

            public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
            {
                U01 = rw.Int32(U01);

                n.BronzeTime = rw.TimeSpan32(n.BronzeTime);
                n.SilverTime = rw.TimeSpan32(n.SilverTime);
                n.GoldTime = rw.TimeSpan32(n.GoldTime);
                n.AuthorTime = rw.TimeSpan32(n.AuthorTime);
                n.TimeLimit = TimeSpan.FromMilliseconds(rw.Int32(Convert.ToInt32(n.TimeLimit.TotalMilliseconds)));
                n.AuthorScore = rw.Int32(n.AuthorScore);
            }
        }

        #endregion

        #region 0x00D chunk (race validate ghost)

        /// <summary>
        /// CGameCtnChallengeParameters 0x00D chunk (race validate ghost)
        /// </summary>
        [Chunk(0x0305B00D, "race validate ghost")]
        public class Chunk0305B00D : Chunk<CGameCtnChallengeParameters>
        {
            public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
            {
                n.RaceValidateGhost = rw.NodeRef<CGameCtnGhost>(n.RaceValidateGhost);
            }
        }

        #endregion

        #region 0x00E skippable chunk (map type)

        /// <summary>
        /// CGameCtnChallengeParameters 0x00E skippable chunk (map type)
        /// </summary>
        [Chunk(0x0305B00E, "map type")]
        public class Chunk0305B00E : SkippableChunk<CGameCtnChallengeParameters>
        {
            public int U01 { get; set; }

            public override void ReadWrite(CGameCtnChallengeParameters n, GameBoxReaderWriter rw)
            {
                n.mapType = rw.String(n.mapType);
                n.mapStyle = rw.String(n.mapStyle);
                U01 = rw.Int32(U01);
            }
        }

        #endregion

        #endregion
    }
}
