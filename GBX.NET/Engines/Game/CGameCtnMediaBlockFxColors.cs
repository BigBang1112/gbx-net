using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03080000)]
    public class CGameCtnMediaBlockFxColors : CGameCtnMediaBlockFx
    {
        [Chunk(0x03080003)]
        public class Chunk03080003 : Chunk
        {
            public Key[] Keys { get; set; }
        }
    }
}
