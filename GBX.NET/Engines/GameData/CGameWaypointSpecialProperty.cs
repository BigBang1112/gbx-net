using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.GameData
{
    [Node(0x2E009000)]
    public class CGameWaypointSpecialProperty : Node
    {
        public int? Spawn
        {
            get => GetValue<Chunk000>(x => x.Spawn) as int?;
            set => SetValue<Chunk000>(x => x.Spawn = value);
        }

        public string Tag
        {
            get => GetValue<Chunk000>(x => x.Tag) as string;
            set => SetValue<Chunk000>(x => x.Tag = value);
        }

        public int Order
        {
            get => (int)GetValue<Chunk000>(x => x.Order);
            set => SetValue<Chunk000>(x => x.Order = value);
        }

        public CGameWaypointSpecialProperty(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x2E009000)]
        public class Chunk000 : Chunk
        {
            public int Version { get; set; }
            public int? Spawn { get; set; }
            public string Tag { get; set; }
            public int Order { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);

                if (Version == 1)
                {
                    Spawn = rw.Int32(Spawn.GetValueOrDefault());
                    Order = rw.Int32(Order);
                }
                else if (Version == 2)
                {
                    Tag = rw.String(Tag);
                    Order = rw.Int32(Order);
                }
            }
        }

        #endregion

        #region 0x001 skippable chunk

        [Chunk(0x2E009001)]
        public class Chunk001 : SkippableChunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        #endregion

        #endregion
    }
}
