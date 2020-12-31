using GBX.NET.Engines.Control;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x030A8000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnMediaBlockText : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public string Text { get; set; }

        [NodeMember]
        public CControlEffectSimi Simi { get; set; }

        [NodeMember]
        public Vec3 Color { get; set; }

        #endregion

        #region Chunks

        #region 0x001 chunk

        [Chunk(0x030A8001)]
        public class Chunk030A8001 : Chunk<CGameCtnMediaBlockText>
        {
            public override void ReadWrite(CGameCtnMediaBlockText n, GameBoxReaderWriter rw)
            {
                n.Text = rw.String(n.Text);
                n.Simi = rw.NodeRef<CControlEffectSimi>(n.Simi);
            }
        }

        #endregion

        #region 0x002 chunk

        [Chunk(0x030A8002)]
        public class Chunk030A8002 : Chunk<CGameCtnMediaBlockText>
        {
            public override void ReadWrite(CGameCtnMediaBlockText n, GameBoxReaderWriter rw)
            {
                n.Color = rw.Vec3(n.Color);
            }
        }

        #endregion

        #region 0x003 chunk

        [Chunk(0x030A8003)]
        public class Chunk030A8003 : Chunk<CGameCtnMediaBlockText>
        {
            public override void ReadWrite(CGameCtnMediaBlockText n, GameBoxReaderWriter rw)
            {
                rw.Single(Unknown); // 0.2
            }
        }

        #endregion

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaBlockText node;

            public string Text => node.Text;
            public CControlEffectSimi Simi => node.Simi;

            public DebugView(CGameCtnMediaBlockText node) => this.node = node;
        }

        #endregion
    }
}