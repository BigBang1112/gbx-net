using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03195000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnMediaBlockInterface : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public float Start { get; set; }

        [NodeMember]
        public float End { get; set; }

        [NodeMember]
        public bool ShowInterface { get; set; }

        [NodeMember]
        public string Manialink { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x03195000)]
        public class Chunk03195000 : Chunk<CGameCtnMediaBlockInterface>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnMediaBlockInterface n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);

                n.Start = rw.Single(n.Start);
                n.End = rw.Single(n.End);
                n.ShowInterface = rw.Boolean(n.ShowInterface);
                n.Manialink = rw.String(n.Manialink);
            }
        }

        #endregion

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaBlockInterface node;

            public float Start => node.Start;
            public float End => node.End;
            public bool ShowInterface => node.ShowInterface;
            public string Manialink => node.Manialink;

            public ChunkSet Chunks => node.Chunks;

            public DebugView(CGameCtnMediaBlockInterface node) => this.node = node;
        }

        #endregion
    }
}
