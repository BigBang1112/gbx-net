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

            public override void Read(CGameCtnMediaBlockDirtyLens n, GameBoxReader r)
            {
                Version = r.ReadInt32();

                n.Keys = r.ReadArray(r1 => new Key()
                {
                    Time = r1.ReadSingle(),
                    Intensity = r1.ReadSingle()
                });
            }

            public override void Write(CGameCtnMediaBlockDirtyLens n, GameBoxWriter w)
            {
                w.Write(Version);

                w.Write(n.Keys, (x, w1) =>
                {
                    w1.Write(x.Time);
                    w1.Write(x.Intensity);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public new class Key : CGameCtnMediaBlock.Key
        {
            public float Intensity { get; set; }
        }

        #endregion
    }
}