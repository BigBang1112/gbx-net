using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace GBX.NET.Engines.GameData
{
    [Node(0x2E002000)]
    public class CGameItemModel : CGameCtnCollector
    {
        public enum EItemType : int
        {
            Undefined = 0,
            /// <summary>
            /// Formerly StaticObject
            /// </summary>
            Ornament = 1,
            /// <summary>
            /// Formerly DynaObject
            /// </summary>
            PickUp = 2,
            Character = 3,
            Vehicle = 4,
            Spot = 5,
            Cannon = 6,
            Group = 7,
            Decal = 8,
            Turret = 9,
            Wagon = 10,
            Block = 11,
            EntitySpawner = 12
        }

        public EItemType ItemType { get; set; }
        public Node[] NadeoSkinFids { get; set; }
        public Node[] Cameras { get; set; }
        public Node RaceInterfaceFid { get; set; }
        public Vec3 GroundPoint { get; set; }
        public float PainterGroundMargin { get; set; }
        public float OrbitalCenterHeightFromGround { get; set; }
        public float OrbitalRadiusBase { get; set; }
        public float OrbitalPreviewAngle { get; set; }
        public Node BaseAttributes { get; set; }
        public Node PhyModel { get; set; }
        public Node VisModel { get; set; }
        public Node VisModelStatic { get; set; }
        public Node Block { get; set; }
        public CGameItemPlacementParam ItemPlacement { get; set; }

        [Chunk(0x2E002000)]
        public class Chunk2E002000 : HeaderChunk<CGameItemModel>
        {
            public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
            {
                n.ItemType = (EItemType)rw.Int32((int)n.ItemType);
            }
        }

        [Chunk(0x2E002008)]
        public class Chunk2E002008 : Chunk<CGameItemModel>
        {
            public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
            {
                n.NadeoSkinFids = rw.Array(n.NadeoSkinFids, i => rw.Reader.ReadNodeRef(), x => rw.Writer.Write(x));
            }
        }

        [Chunk(0x2E002009)]
        public class Chunk2E002009 : Chunk<CGameItemModel>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                n.Cameras = rw.Array(n.Cameras, i => rw.Reader.ReadNodeRef(), x => rw.Writer.Write(x));
            }
        }

        [Chunk(0x2E00200C)]
        public class Chunk2E00200C : Chunk<CGameItemModel>
        {
            public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
            {
                n.RaceInterfaceFid = rw.NodeRef(n.RaceInterfaceFid);
            }
        }

        [Chunk(0x2E002012)]
        public class Chunk2E002012 : Chunk<CGameItemModel>
        {
            public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
            {
                n.GroundPoint = rw.Vec3(n.GroundPoint);
                n.PainterGroundMargin = rw.Single(n.PainterGroundMargin);
                n.OrbitalCenterHeightFromGround = rw.Single(n.OrbitalCenterHeightFromGround);
                n.OrbitalRadiusBase = rw.Single(n.OrbitalRadiusBase);
                n.OrbitalPreviewAngle = rw.Single(n.OrbitalPreviewAngle);
            }
        }

        [Chunk(0x2E002013)]
        public class Chunk2E002013 : Chunk<CGameItemModel>
        {
            public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
            {
                n.BaseAttributes = rw.NodeRef(n.BaseAttributes);
            }
        }

        [Chunk(0x2E002015)]
        public class Chunk2E002015 : Chunk<CGameItemModel>
        {
            public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
            {
                n.ItemType = (EItemType)rw.Int32((int)n.ItemType);
            }
        }

        [Chunk(0x2E002019)]
        public class Chunk2E002019 : Chunk<CGameItemModel>
        {
            public int Version { get; set; }
            public Node Unknown1 { get; set; }

            public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                n.PhyModel = rw.NodeRef(n.PhyModel);
                n.VisModel = rw.NodeRef(n.VisModel);

                if (Version >= 1)
                {
                    n.VisModelStatic = rw.NodeRef(n.VisModelStatic);

                    if(Version >= 12)
                    {
                        rw.Int32(Unknown);
                        rw.Int32(Unknown);
                        n.Block = rw.NodeRef<CGameBlockItem>(n.Block);
                        if(Version >= 13)
                            Unknown1 = rw.NodeRef(Unknown1);
                    }
                }

            }
        }

        [Chunk(0x2E00201A)]
        public class Chunk2E00201A : Chunk<CGameItemModel>
        {
            public Node Unknown1 { get; set; }

            public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
            {
                Unknown1 = rw.NodeRef(Unknown1);
            }
        }

        [Chunk(0x2E00201C)]
        public class Chunk2E00201C : Chunk<CGameItemModel>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                n.ItemPlacement = rw.NodeRef<CGameItemPlacementParam>(n.ItemPlacement);
            }
        }

        [Chunk(0x2E00201E)]
        public class Chunk2E00201E : Chunk<CGameItemModel>
        {
            public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x2E00201F)]
        public class Chunk2E00201F : Chunk<CGameItemModel>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                if(Version >= 10)
                    rw.Int32(Unknown);
            }
        }

        [Chunk(0x2E002020)]
        public class Chunk2E002020 : Chunk<CGameItemModel>
        {
            public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Byte(Unknown);
            }
        }

        [Chunk(0x2E002021)]
        public class Chunk2E002021 : Chunk<CGameItemModel>
        {
            public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x2E002023)]
        public class Chunk2E002023 : Chunk<CGameItemModel>
        {
            public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
            {
                rw.Byte(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x2E002024)]
        public class Chunk2E002024 : SkippableChunk<CGameItemModel>
        {
            
        }
    }
}
