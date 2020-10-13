using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03145000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnMediaBlockShoot : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public float Start { get; set; }

        [NodeMember]
        public float End { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x03145000)]
        public class Chunk03145000 : Chunk<CGameCtnMediaBlockShoot>
        {
            public override void ReadWrite(CGameCtnMediaBlockShoot n, GameBoxReaderWriter rw)
            {
                n.Start = rw.Single(n.Start);
                n.End = rw.Single(n.End);
            }
        }

        #endregion

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaBlockShoot node;

            public float Start => node.Start;
            public float End => node.End;

            public DebugView(CGameCtnMediaBlockShoot node) => this.node = node;
        }

        #endregion
    }
}
