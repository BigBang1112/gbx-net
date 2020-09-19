using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x0308C000)]
    public class CGamePlayerProfile : Node
    {
        public string OnlineLogin { get; set; }
        public string OnlineSupportKey { get; set; }

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x0308C000)]
        public class Chunk0308C000 : Chunk<CGamePlayerProfile>
        {
            public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
            {
                n.OnlineLogin = rw.String(n.OnlineLogin);
                n.OnlineSupportKey = rw.String(n.OnlineSupportKey);
            }
        }

        #endregion

        #endregion
    }
}