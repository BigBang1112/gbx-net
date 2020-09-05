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
        public TimeSpan? BronzeTime
        {
            get => GetValue<Chunk004, Chunk00A>(x => x.BronzeTime, x => x.BronzeTime) as TimeSpan?;
            set => SetValue<Chunk004, Chunk00A>(x => x.BronzeTime = value, x => x.BronzeTime = value);
        }

        /// <summary>
        /// Time of the silver medal.
        /// </summary>
        public TimeSpan? SilverTime
        {
            get => GetValue<Chunk004, Chunk00A>(x => x.SilverTime, x => x.SilverTime) as TimeSpan?;
            set => SetValue<Chunk004, Chunk00A>(x => x.SilverTime = value, x => x.SilverTime = value);
        }

        /// <summary>
        /// Time of the gold medal.
        /// </summary>
        public TimeSpan? GoldTime
        {
            get => GetValue<Chunk004, Chunk00A>(x => x.GoldTime, x => x.GoldTime) as TimeSpan?;
            set => SetValue<Chunk004, Chunk00A>(x => x.GoldTime = value, x => x.GoldTime = value);
        }

        /// <summary>
        /// Time of the author medal.
        /// </summary>
        public TimeSpan? AuthorTime
        {
            get => GetValue<Chunk004, Chunk00A>(x => x.AuthorTime, x => x.AuthorTime) as TimeSpan?;
            set => SetValue<Chunk004, Chunk00A>(x => x.AuthorTime = value, x => x.AuthorTime = value);
        }

        /// <summary>
        /// Usually author time or stunt score.
        /// </summary>
        public int? AuthorScore
        {
            get => GetValue<Chunk008, Chunk00A>(x => x.AuthorScore, x => x.AuthorScore) as int?;
            set => SetValue<Chunk008, Chunk00A>(x => x.AuthorScore = value.GetValueOrDefault(), x => x.AuthorScore = value.GetValueOrDefault());
        }

        /// <summary>
        /// Stunt time limit.
        /// </summary>
        public TimeSpan? TimeLimit
        {
            get => GetValue<Chunk008, Chunk00A>(x => x.TimeLimit, x => x.TimeLimit) as TimeSpan?;
            set => SetValue<Chunk008, Chunk00A>(x => x.TimeLimit = value.GetValueOrDefault(), x => x.TimeLimit = value.GetValueOrDefault());
        }

        public string MapType
        {
            get => GetValue<Chunk00E>(x => x.MapType) as string;
            set => SetValue<Chunk00E>(x => x.MapType = value);
        }

        public string MapStyle
        {
            get => GetValue<Chunk00E>(x => x.MapStyle) as string;
            set => SetValue<Chunk00E>(x => x.MapStyle = value);
        }

        public CGameCtnChallengeParameters(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {
            
        }

        [Chunk(0x0305B000)]
        public class Chunk000 : Chunk
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

        [Chunk(0x0305B001)]
        public class Chunk001 : Chunk
        {
            public string[] Tips { get; } = new string[4];

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                for(var i = 0; i < 4; i++)
                    Tips[i] = rw.String(Tips[i]);
            }
        }

        [Chunk(0x0305B002)]
        public class Chunk002 : Chunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
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
        public class Chunk003 : Chunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
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
        public class Chunk004 : Chunk
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

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                BronzeTime = rw.TimeSpan32(BronzeTime);
                SilverTime = rw.TimeSpan32(SilverTime);
                GoldTime = rw.TimeSpan32(GoldTime);
                AuthorTime = rw.TimeSpan32(AuthorTime);

                rw.UInt32(Unknown);
            }
        }

        [Chunk(0x0305B005)]
        public class Chunk005 : Chunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0305B006)]
        public class Chunk006 : Chunk
        {
            public uint[] Items { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Items = rw.Array(Items, i => rw.Reader.ReadUInt32(), x => rw.Writer.Write(x));
            }
        }

        [Chunk(0x0305B007)]
        public class Chunk007 : Chunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.UInt32(Unknown);
            }
        }

        /// <summary>
        /// CGameCtnChallengeParameters 0x008 chunk (stunts)
        /// </summary>
        [Chunk(0x0305B008)]
        public class Chunk008 : Chunk
        {
            /// <summary>
            /// Stunt time limit.
            /// </summary>
            public TimeSpan TimeLimit { get; set; }
            /// <summary>
            /// Stunt author score.
            /// </summary>
            public int AuthorScore { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                TimeLimit = TimeSpan.FromMilliseconds(rw.Int32(Convert.ToInt32(TimeLimit.TotalMilliseconds)));
                AuthorScore = rw.Int32(AuthorScore);
            }
        }

        [Chunk(0x0305B00A, true)]
        public class Chunk00A : SkippableChunk
        {
            public int Unknown1 { get; set; }

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
            /// Stunt time limit.
            /// </summary>
            public TimeSpan TimeLimit { get; set; }
            /// <summary>
            /// Stunt author score.
            /// </summary>
            public int AuthorScore { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Unknown1 = rw.Int32(Unknown1);

                BronzeTime = rw.TimeSpan32(BronzeTime);
                SilverTime = rw.TimeSpan32(SilverTime);
                GoldTime = rw.TimeSpan32(GoldTime);
                AuthorTime = rw.TimeSpan32(AuthorTime);
                TimeLimit = TimeSpan.FromMilliseconds(rw.Int32(Convert.ToInt32(TimeLimit.TotalMilliseconds)));
                AuthorScore = rw.Int32(AuthorScore);
            }
        }

        [Chunk(0x0305B00D)]
        public class Chunk00D : Chunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0305B00E)]
        public class Chunk00E : SkippableChunk
        {
            public string MapType { get; set; }
            public string MapStyle { get; set; }
            public int Unknown1 { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                MapType = rw.String(MapType);
                MapStyle = rw.String(MapStyle);
                Unknown1 = rw.Int32(Unknown1);
            }
        }
    }
}
