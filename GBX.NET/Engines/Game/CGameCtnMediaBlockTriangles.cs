using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03029000)]
    public class CGameCtnMediaBlockTriangles : CGameCtnMediaBlock
    {
        public CGameCtnMediaBlockTriangles(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }
    }
}
