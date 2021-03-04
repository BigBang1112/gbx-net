using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x030A9000)]
    public class CGameCtnMediaBlockTrails : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public float Start { get; set; }

        [NodeMember]
        public float End { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x030A9000)]
        public class Chunk030A9000 : Chunk<CGameCtnMediaBlockTrails>
        {
            public override void ReadWrite(CGameCtnMediaBlockTrails n, GameBoxReaderWriter rw)
            {
                n.Start = rw.Single(n.Start);
                n.End = rw.Single(n.End);
            }
        }

        #endregion

        #endregion
    }
}
