using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03139000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnMediaBlockFxCameraMap : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public float Start { get; set; }

        [NodeMember]
        public float End { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x03139000)]
        public class Chunk03139000 : Chunk<CGameCtnMediaBlockFxCameraMap>
        {
            public override void ReadWrite(CGameCtnMediaBlockFxCameraMap n, GameBoxReaderWriter rw)
            {
                n.Start = rw.Single(n.Start);
                n.End = rw.Single(n.End);
            }
        }

        #endregion

        #region 0x001 chunk

        [Chunk(0x03139001)]
        public class Chunk03139001 : Chunk<CGameCtnMediaBlockFxCameraMap>
        {
            public override void ReadWrite(CGameCtnMediaBlockFxCameraMap n, GameBoxReaderWriter rw)
            {
                rw.Single(Unknown);
                rw.Int32(Unknown);
                rw.Single(Unknown);
                rw.Single(Unknown);

                rw.Single(Unknown);
                rw.Int32(Unknown);
                rw.Single(Unknown);
                rw.Single(Unknown);

                rw.Single(Unknown);
                rw.Int32(Unknown);
                rw.Single(Unknown);
                rw.Single(Unknown);

                rw.Byte(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.FileRef(Unknown);
            }
        }

        #endregion

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaBlockFxCameraMap node;

            public float Start => node.Start;
            public float End => node.End;

            public ChunkSet Chunks => node.Chunks;

            public DebugView(CGameCtnMediaBlockFxCameraMap node) => this.node = node;
        }

        #endregion
    }
}
