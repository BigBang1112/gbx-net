using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.GameData
{
    [Node(0x2E01D000)]
    public class CGameObjectModel : Node
    {
        public uint m_InventoryParams_InventoryOccupation { get; set; }

        [Chunk(0x2E01D000)]
        public class Chunk2E01D000 : Chunk<CGameObjectModel>
        {
            public override void ReadWrite(CGameObjectModel n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                n.m_InventoryParams_InventoryOccupation = rw.UInt32(n.m_InventoryParams_InventoryOccupation);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }
    }
}
