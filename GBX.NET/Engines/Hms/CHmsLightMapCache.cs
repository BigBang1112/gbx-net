using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Hms
{
    [Node(0x06022000)]
    public class CHmsLightMapCache : Node
    {
        public CHmsLightMapCache(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x0602201A)]
        public class Chunk0602201A : SkippableChunk<CHmsLightMapCache>
        {
            
        }
    }
}
