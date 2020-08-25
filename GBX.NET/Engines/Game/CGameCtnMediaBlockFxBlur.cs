using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x0307F000)]
    public class CGameCtnMediaBlockFxBlur : CGameCtnMediaBlockFx
    {
        public CGameCtnMediaBlockFxBlur(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }
    }
}
