using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Debugging
{
#if DEBUG
    public class ChunkDebugger
    {
        public byte[]? RawData { get; set; }
    }
#endif
}
