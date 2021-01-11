using GBX.NET.Engines.Plug;
using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.GameData
{
    [Node(0x2E026000)]
    public class CGameCommonItemEntityModelEdition : Node
    {
        #region Enums

        public enum EItemType
        {
            Undefined,
            Ornament,
            PickUp,
            Character,
            Vehicle,
            Spot,
            Cannon,
            Group,
            Decal,
            Turret,
            Wagon,
            Block,
            EntitySpawner,
            DeprecV,
            Procedural
        }

        #endregion

        #region Properties

        public EItemType ItemType { get; set; }
        public CPlugCrystal MeshCrystal { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x2E026000)]
        public class Chunk2E026000 : Chunk<CGameCommonItemEntityModelEdition>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCommonItemEntityModelEdition n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                n.ItemType = (EItemType)rw.Int32((int)n.ItemType);
                n.MeshCrystal = rw.NodeRef<CPlugCrystal>(n.MeshCrystal);
                rw.String(Unknown);
                rw.Array<int>(Unknown, 32);
            }
        }

        #endregion

        #endregion
    }
}
