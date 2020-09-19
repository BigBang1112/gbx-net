using GBX.NET.Engines.Control;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x030A8000)]
    public class CGameCtnMediaBlockText : CGameCtnMediaBlock
    {
        public string Text { get; set; }
        public CControlEffectSimi Simi { get; set; }

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
                rw.Single(Unknown);
                rw.Single(Unknown);
                rw.Single(Unknown);
            }
        }

        #endregion

        #endregion
    }
}