using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.GameData
{
    [Node(0x2E009000)]
    public class CGameWaypointSpecialProperty : CMwNod
    {
        #region Properties

        [NodeMember]
        public int? Spawn { get; set; }

        [NodeMember]
        public string Tag { get; set; }

        [NodeMember]
        public int Order { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x2E009000)]
        public class Chunk2E009000 : Chunk<CGameWaypointSpecialProperty>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameWaypointSpecialProperty n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);

                if (Version == 1)
                {
                    n.Spawn = rw.Int32(n.Spawn.GetValueOrDefault());
                    n.Order = rw.Int32(n.Order);
                }
                else if (Version == 2)
                {
                    n.Tag = rw.String(n.Tag);
                    n.Order = rw.Int32(n.Order);
                }
            }
        }

        #endregion

        #region 0x001 skippable chunk

        [Chunk(0x2E009001)]
        public class Chunk2E009001 : SkippableChunk<CGameWaypointSpecialProperty>
        {
            private int u01;
            private int u02;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public int U02
            {
                get => u02;
                set => u02 = value;
            }

            public override void ReadWrite(CGameWaypointSpecialProperty n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref u01);
                rw.Int32(ref u02);
            }
        }

        #endregion

        #endregion
    }
}
