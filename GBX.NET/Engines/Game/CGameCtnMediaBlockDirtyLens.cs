using System.Diagnostics;

namespace GBX.NET.Engines.Game
{
    [Node(0x03165000)]
    public class CGameCtnMediaBlockDirtyLens : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x03165000)]
        public class Chunk03165000 : Chunk<CGameCtnMediaBlockDirtyLens>
        {
            public int Version { get; set; }

            public override void Read(CGameCtnMediaBlockDirtyLens n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();

                n.Keys = r.ReadArray(() => new Key()
                {
                    Time = r.ReadSingle(),
                    Intensity = r.ReadSingle()
                });
            }

            public override void Write(CGameCtnMediaBlockDirtyLens n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);

                w.Write(n.Keys, x =>
                {
                    w.Write(x.Time);
                    w.Write(x.Intensity);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Key : MediaBlockKey
        {
            public float Intensity { get; set; }
        }

        #endregion
    }
}