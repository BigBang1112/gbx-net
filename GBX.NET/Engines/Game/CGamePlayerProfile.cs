using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x0308C000)]
    public class CGamePlayerProfile : Node
    {
        public CGamePlayerProfile(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x0308C000)]
        public class Chunk000 : Chunk
        {
            public string OnlineLogin { get; set; }
            public string OnlineSupportKey { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                OnlineLogin = rw.String(OnlineLogin);
                OnlineSupportKey = rw.String(OnlineSupportKey);
            }
        }

        #endregion

        #endregion
    }
}