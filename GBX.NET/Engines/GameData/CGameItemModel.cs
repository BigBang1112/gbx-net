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
        public enum ItemType : int
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

        public CGameItemModel(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x2E002000)]
        public class Chunk2E002000 : HeaderChunk
        {
            public ItemType Type { get; set; }

            public Chunk2E002000(CGameItemModel node, byte[] data) : base(node, data)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Type = (ItemType)rw.Int32((int)Type);
            }
        }

        [Chunk(0x2E002008)]
        public class Chunk2E002008 : Chunk
        {
            public Node[] NadeoSkinFids { get; set; }

            public Chunk2E002008(CGameItemModel node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                NadeoSkinFids = rw.Array(NadeoSkinFids, i => rw.Reader.ReadNodeRef(), x => rw.Writer.Write(x));
            }
        }

        [Chunk(0x2E002009)]
        public class Chunk2E002009 : Chunk
        {
            public int Version { get; set; }
            public Node[] Cameras { get; set; }

            public Chunk2E002009(CGameItemModel node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                Cameras = rw.Array(Cameras, i => rw.Reader.ReadNodeRef(), x => rw.Writer.Write(x));
            }
        }

        [Chunk(0x2E00200C)]
        public class Chunk2E00200C : Chunk
        {
            public Node RaceInterfaceFid { get; set; }

            public Chunk2E00200C(CGameItemModel node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                RaceInterfaceFid = rw.NodeRef(RaceInterfaceFid);
            }
        }

        [Chunk(0x2E002012)]
        public class Chunk2E002012 : Chunk
        {
            public Vector3 GroundPoint { get; set; }
            public float PainterGroundMargin { get; set; }
            public float OrbitalCenterHeightFromGround { get; set; }
            public float OrbitalRadiusBase { get; set; }
            public float OrbitalPreviewAngle { get; set; }

            public Chunk2E002012(CGameItemModel node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                GroundPoint = rw.Vec3(GroundPoint);
                PainterGroundMargin = rw.Single(PainterGroundMargin);
                OrbitalCenterHeightFromGround = rw.Single(OrbitalCenterHeightFromGround);
                OrbitalRadiusBase = rw.Single(OrbitalRadiusBase);
                OrbitalPreviewAngle = rw.Single(OrbitalPreviewAngle);
            }
        }

        [Chunk(0x2E002013)]
        public class Chunk2E002013 : Chunk
        {
            public Node BaseAttributes { get; set; }

            public Chunk2E002013(CGameItemModel node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                BaseAttributes = rw.NodeRef(BaseAttributes);
            }
        }

        [Chunk(0x2E002015)]
        public class Chunk2E002015 : Chunk
        {
            public ItemType ItemObjectType { get; set; }

            public Chunk2E002015(CGameItemModel node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                ItemObjectType = (ItemType)rw.Int32((int)ItemObjectType);
            }
        }

        [Chunk(0x2E002019)]
        public class Chunk2E002019 : Chunk
        {
            public int Version { get; set; }
            public Node PhyModel { get; set; }
            public Node VisModel { get; set; }
            public Node VisModelStatic { get; set; }
            public Node Block { get; set; }
            public Node Unknown1 { get; set; }

            public Chunk2E002019(CGameItemModel node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                PhyModel = rw.NodeRef(PhyModel);
                VisModel = rw.NodeRef(VisModel);

                if (Version >= 1)
                {
                    VisModelStatic = rw.NodeRef(VisModelStatic);

                    if(Version >= 12)
                    {
                        rw.Int32(Unknown);
                        rw.Int32(Unknown);
                        Block = rw.NodeRef(Block);
                        if(Version >= 13)
                            Unknown1 = rw.NodeRef(Unknown1);
                    }
                }

            }
        }

        [Chunk(0x2E00201A)]
        public class Chunk2E00201A : Chunk
        {
            public Node Unknown1 { get; set; }

            public Chunk2E00201A(CGameItemModel node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Unknown1 = rw.NodeRef(Unknown1);
            }
        }

        [Chunk(0x2E00201C)]
        public class Chunk2E00201C : Chunk
        {
            public int Version { get; set; }
            public CGameItemPlacementParam ItemPlacement { get; set; }

            public Chunk2E00201C(CGameItemModel node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                ItemPlacement = rw.NodeRef<CGameItemPlacementParam>(ItemPlacement);
            }
        }

        [Chunk(0x2E00201E)]
        public class Chunk2E00201E : Chunk
        {
            public Chunk2E00201E(CGameItemModel node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x2E00201F)]
        public class Chunk2E00201F : Chunk
        {
            public int Version { get; set; }

            public Chunk2E00201F(CGameItemModel node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                if(Version >= 10)
                    rw.Int32(Unknown);
            }
        }

        [Chunk(0x2E002020)]
        public class Chunk2E002020 : Chunk
        {
            public Chunk2E002020(CGameItemModel node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Byte(Unknown);
            }
        }

        [Chunk(0x2E002021)]
        public class Chunk2E002021 : Chunk
        {
            public Chunk2E002021(CGameItemModel node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x2E002023)]
        public class Chunk2E002023 : Chunk
        {
            public Chunk2E002023(CGameItemModel node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Byte(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x2E002024)]
        public class Chunk2E002024 : SkippableChunk
        {
            public Chunk2E002024(CGameItemModel node, byte[] data) : base(node, data)
            {
                
            }
        }
    }
}
