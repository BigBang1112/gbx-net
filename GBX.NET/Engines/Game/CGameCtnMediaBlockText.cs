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
        public CControlEffectSimi Effect { get; set; }

        [NodeMember]
        public Vec3 Color { get; set; }

        #endregion

        #region Chunks

        #region 0x001 chunk (text)

        [Chunk(0x030A8001, "text")]
        public class Chunk030A8001 : Chunk<CGameCtnMediaBlockText>
        {
            public override void ReadWrite(CGameCtnMediaBlockText n, GameBoxReaderWriter rw)
            {
                n.Text = rw.String(n.Text);
                n.Effect = rw.NodeRef<CControlEffectSimi>(n.Effect);
            }
        }

        #endregion

        #region 0x002 chunk (color)

        [Chunk(0x030A8002, "color")]
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
            public CControlEffectSimi Effect => node.Effect;

            public ChunkSet Chunks => node.Chunks;

            public DebugView(CGameCtnMediaBlockText node) => this.node = node;
        }

        #endregion
    }
}