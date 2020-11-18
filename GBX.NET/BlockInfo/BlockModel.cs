using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.BlockInfo
{
    public struct BlockModel
    {
        public BlockUnit[] Air { get; set; }
        public BlockUnit[] Ground { get; set; }
    }
}
