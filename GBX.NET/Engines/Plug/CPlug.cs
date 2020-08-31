using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Plug
{
    [Node(0x0902B000)]
    public class CPlug : Node
    {
        public CPlug(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }
    }
}
