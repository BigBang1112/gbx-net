using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// MediaTracker block - Camera base
    /// </summary>
    [Node(0x0307C000)]
    public class CGameCtnMediaBlockCamera : CGameCtnMediaBlock
    {
        public CGameCtnMediaBlockCamera(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }
    }
}
