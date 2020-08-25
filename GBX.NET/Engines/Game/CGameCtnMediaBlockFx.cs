using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x0307E000)]
    public class CGameCtnMediaBlockFx : CGameCtnMediaBlock
    {
        public CGameCtnMediaBlockFx(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }
    }
}
