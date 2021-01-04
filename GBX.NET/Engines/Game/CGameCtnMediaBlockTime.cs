using System.Diagnostics;

namespace GBX.NET.Engines.Game
{
    [Node(0x03085000)]
    [DebuggerTypeProxy(typeof(DebugView))]
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
                n.Keys = rw.Array(n.Keys, i => new Key()
                {
                    Time = rw.Reader.ReadSingle(),
                    TimeValue = rw.Reader.ReadSingle(),
                    Tangent = rw.Reader.ReadSingle()
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.TimeValue);
                    rw.Writer.Write(x.Tangent);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Key : MediaBlockKey
        {
            public float TimeValue { get; set; }
            public float Tangent { get; set; }
        }

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaBlockTime node;

            public Key[] Keys => node.Keys;

            public ChunkSet Chunks => node.Chunks;

            public DebugView(CGameCtnMediaBlockTime node) => this.node = node;
        }

        #endregion
    }
}
