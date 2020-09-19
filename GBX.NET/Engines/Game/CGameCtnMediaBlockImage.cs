using GBX.NET.Engines.Control;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x030A5000)]
    public class CGameCtnMediaBlockImage : CGameCtnMediaBlock
    {
        public CControlEffectSimi Simi { get; set; }

        public FileRef Image { get; set; }

        [Chunk(0x030A5000)]
        public class Chunk030A5000 : Chunk<CGameCtnMediaBlockImage>
        {
            public override void ReadWrite(CGameCtnMediaBlockImage n, GameBoxReaderWriter rw)
            {
                n.Simi = rw.NodeRef<CControlEffectSimi>(n.Simi);
                n.Image = rw.FileRef(n.Image);
            }
        }
    }
}
