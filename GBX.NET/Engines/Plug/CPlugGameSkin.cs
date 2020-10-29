using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Plug
{
    [Node(0x090F4000)]
    public class CPlugGameSkin : Node
    {
        [Chunk(0x090F4000)]
        public class Chunk090F4000 : HeaderChunk<CPlugGameSkin>
        {
            public byte Version { get; set; }
            public string SkinDirectory { get; set; }

            public override void ReadWrite(CPlugGameSkin n, GameBoxReaderWriter rw)
            {
                Version = rw.Byte(Version);
                SkinDirectory = rw.String(SkinDirectory);

                // ...
            }
        }
    }
}
