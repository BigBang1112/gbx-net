using System.Diagnostics;

namespace GBX.NET.Engines.Game
{
    [Node(0x03085000)]
    public class CGameCtnMediaBlockTime : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x03085000)]
        public class Chunk03085000 : Chunk<CGameCtnMediaBlockTime>
        {
            public override void ReadWrite(CGameCtnMediaBlockTime n, GameBoxReaderWriter rw)
            {
                n.Keys = rw.Array(n.Keys, r => new Key()
                {
                    Time = r.ReadSingle(),
                    TimeValue = r.ReadSingle(),
                    Tangent = r.ReadSingle()
                },
                (x, w) =>
                {
                    w.Write(x.Time);
                    w.Write(x.TimeValue);
                    w.Write(x.Tangent);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public new class Key : CGameCtnMediaBlock.Key
        {
            public float TimeValue { get; set; }
            public float Tangent { get; set; }
        }

        #endregion
    }
}
