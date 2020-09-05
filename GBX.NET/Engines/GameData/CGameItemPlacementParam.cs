using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.GameData
{
    [Node(0x2E020000)]
    public class CGameItemPlacementParam : Node
    {
        public CGameItemPlacementParam(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }
    }
}