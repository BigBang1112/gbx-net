using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03029000)]
    public class CGameCtnMediaBlockTriangles : CGameCtnMediaBlock
    {
        [Chunk(0x03029001)]
        [AutoReadWriteChunk]
        public class Chunk03029001 : Chunk<CGameCtnMediaBlockTriangles>
        {
            
        }
    }
}
