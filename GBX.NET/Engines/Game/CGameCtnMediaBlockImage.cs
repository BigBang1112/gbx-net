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
        public CControlEffectSimi Simi { get; set; }

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
                n.Simi = rw.NodeRef<CControlEffectSimi>(n.Simi);
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

            public CControlEffectSimi Simi => node.Simi;
            public FileRef Image => node.Image;

            public DebugView(CGameCtnMediaBlockImage node) => this.node = node;
        }

        #endregion
    }
}
