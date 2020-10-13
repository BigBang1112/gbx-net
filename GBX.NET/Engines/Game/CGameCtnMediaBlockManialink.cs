using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x0312A000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnMediaBlockManialink : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public float Start { get; set; }

        [NodeMember]
        public float End { get; set; }

        [NodeMember]
        public string Manialink { get; set; }

        #endregion

        #region Chunks

        #region 0x001 chunk

        [Chunk(0x0312A001)]
        public class Chunk0312A001 : Chunk<CGameCtnMediaBlockManialink>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnMediaBlockManialink n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);

                n.Start = rw.Single(n.Start);
                n.End = rw.Single(n.End);
                n.Manialink = rw.String(n.Manialink);
            }
        }

        #endregion

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaBlockManialink node;

            public float Start => node.Start;
            public float End => node.End;
            public string Manialink => node.Manialink;

            public DebugView(CGameCtnMediaBlockManialink node) => this.node = node;
        }

        #endregion
    }
}
