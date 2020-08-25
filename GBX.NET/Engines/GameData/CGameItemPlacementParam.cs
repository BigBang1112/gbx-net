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

        [Chunk(0x2E020000, true)]
        public class Chunk000 : SkippableChunk
        {
            public Chunk000(CGameItemPlacementParam node, byte[] data) : base(node, data)
            {
                
            }
        }

        [Chunk(0x2E020001)]
        public class Chunk001 : Chunk
        {
            public Chunk001(CGameItemPlacementParam node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }
    }
}