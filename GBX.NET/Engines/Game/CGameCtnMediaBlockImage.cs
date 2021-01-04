using GBX.NET.Engines.Control;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x030A5000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnMediaBlockImage : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public CControlEffectSimi Effect { get; set; }

        [NodeMember]
        public FileRef Image { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x030A5000)]
        public class Chunk030A5000 : Chunk<CGameCtnMediaBlockImage>
        {
            public override void ReadWrite(CGameCtnMediaBlockImage n, GameBoxReaderWriter rw)
            {
                n.Effect = rw.NodeRef<CControlEffectSimi>(n.Effect);
                n.Image = rw.FileRef(n.Image);
            }
        }

        #endregion

        #region 0x001 chunk

        [Chunk(0x030A5001)]
        public class Chunk030A5001 : Chunk<CGameCtnMediaBlockImage>
        {
            public override void ReadWrite(CGameCtnMediaBlockImage n, GameBoxReaderWriter rw)
            {
                rw.Single(Unknown); // 0.2
            }
        }

        #endregion

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaBlockImage node;

            public CControlEffectSimi Effect => node.Effect;
            public FileRef Image => node.Image;

            public ChunkSet Chunks => node.Chunks;

            public DebugView(CGameCtnMediaBlockImage node) => this.node = node;
        }

        #endregion
    }
}
