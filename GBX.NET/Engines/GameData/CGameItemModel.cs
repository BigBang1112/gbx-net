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

        [Chunk(0x2E002000, 0x000)]
        public class Chunk000 : Chunk
        {
            public ItemType Type { get; set; }

            public Chunk000(CGameItemModel node, Stream stream, int? length) : base(node, 0x000, length)
            {
                Progress += stream.Read(out int type);
                Type = (ItemType)type;
            }
        }

        [Chunk(0x2E002000, 0x001)]
        public class Chunk001 : Chunk
        {
            public Chunk001(CGameItemModel node, Stream stream, int? length) : base(node, 0x001, length)
            {
                Progress += stream.Read(out int _);
            }
        }

        [Chunk(0x2E002000, 0x008)]
        public class Chunk008_002 : Chunk
        {
            public NodeRef[] NadeoSkinFids { get; }

            public Chunk008_002(CGameItemModel node, Stream stream, int? length) : base(node, 0x008, length)
            {
                Progress += stream.Read(out int numNadeoSkinFids);
                NadeoSkinFids = new NodeRef[numNadeoSkinFids];

                for (var i = 0; i < numNadeoSkinFids; i++)
                {
                    Progress += stream.Read(out NodeRef skinFid, GetBody());
                    NadeoSkinFids[i] = skinFid;
                }
            }
        }

        [Chunk(0x2E002000, 0x009)]
        public class Chunk009_002 : Chunk
        {
            public int Version { get; set; }
            public NodeRef[] Cameras { get; }

            public Chunk009_002(CGameItemModel node, Stream stream, int? length) : base(node, 0x009, length)
            {
                Progress += stream.Read(out int version);
                Version = version;

                Progress += stream.Read(out int numCameras);
                Cameras = new NodeRef[numCameras];

                for (var i = 0; i < numCameras; i++)
                {
                    Progress += stream.Read(out NodeRef camera, GetBody());
                    Cameras[i] = camera;
                }
            }
        }

        [Chunk(0x2E002000, 0x00C)]
        public class Chunk00C_002 : Chunk
        {
            public NodeRef RaceInterfaceFid { get; }

            public Chunk00C_002(CGameItemModel node, Stream stream, int? length) : base(node, 0x00C, length)
            {
                Progress += stream.Read(out NodeRef raceInterfaceFid, GetBody());
                RaceInterfaceFid = raceInterfaceFid;
            }
        }

        [Chunk(0x2E002000, 0x012)]
        public class Chunk012 : Chunk
        {
            public Vector3 GroundPoint { get; set; }
            public float PainterGroundMargin { get; set; }
            public float OrbitalCenterHeightFromGround { get; set; }
            public float OrbitalRadiusBase { get; set; }
            public float OrbitalPreviewAngle { get; set; }

            public Chunk012(CGameItemModel node, Stream stream, int? length) : base(node, 0x012, length)
            {
                Progress += stream.Read(out Vector3 groundPoint);
                GroundPoint = groundPoint;

                Progress += stream.Read(out float painterGroundMargin);
                PainterGroundMargin = painterGroundMargin;

                Progress += stream.Read(out float orbitalCenterHeightFromGround);
                OrbitalCenterHeightFromGround = orbitalCenterHeightFromGround;

                Progress += stream.Read(out float orbitalRadiusBase);
                OrbitalRadiusBase = orbitalRadiusBase;

                Progress += stream.Read(out float orbitalPreviewAngle);
                OrbitalPreviewAngle = orbitalPreviewAngle;
            }
        }

        [Chunk(0x2E002000, 0x013)]
        public class Chunk013 : Chunk
        {
            public NodeRef BaseAttributes { get; }

            public Chunk013(CGameItemModel node, Stream stream, int? length) : base(node, 0x013, length)
            {
                Progress += stream.Read(out NodeRef baseAttributes, GetBody());
                BaseAttributes = baseAttributes;
            }
        }

        [Chunk(0x2E002000, 0x015)]
        public class Chunk015 : Chunk
        {
            public ItemType ItemObjectType { get; set; }

            public Chunk015(CGameItemModel node, Stream stream, int? length) : base(node, 0x015, length)
            {
                Progress += stream.Read(out int itemObjectType);
                ItemObjectType = (ItemType)itemObjectType;
            }
        }

        [Chunk(0x2E002000, 0x019)]
        public class Chunk019 : Chunk
        {
            public int Version { get; set; }
            public NodeRef PhyModel { get; }
            public NodeRef VisModel { get; }
            public NodeRef VisModelStatic { get; }

            public Chunk019(CGameItemModel node, Stream stream, int? length) : base(node, 0x019, length)
            {
                Progress += stream.Read(out int version);
                Version = version;

                Progress += stream.Read(out NodeRef phyModel, GetBody());
                PhyModel = phyModel;

                Progress += stream.Read(out NodeRef visModel, GetBody());
                VisModel = visModel;

                if(version >= 1)
                {
                    Progress += stream.Read(out NodeRef visModelStatic, GetBody());
                    VisModelStatic = visModelStatic;
                }

                Progress += stream.Read(out uint[] dsgadsgasd, 4*4);
            }
        }

        [Chunk(0x2E002000, 0x01A)]
        public class Chunk01A : Chunk
        {
            public Chunk01A(CGameItemModel node, Stream stream, int? length) : base(node, 0x01A, length)
            {
                Progress += stream.Read(out uint dsgadsgasd);
            }
        }

        [Chunk(0x2E002000, 0x01C)]
        public class Chunk01C : Chunk
        {
            public int Version { get; set; }
            public NodeRef ItemPlacement { get; }

            public Chunk01C(CGameItemModel node, Stream stream, int? length) : base(node, 0x01C, length)
            {
                Progress += stream.Read(out int version);
                Version = version;

                Progress += stream.Read(out NodeRef itemPlacement, GetBody());
                ItemPlacement = itemPlacement;
            }
        }

        [Chunk(0x2E002000, 0x01E)]
        public class Chunk01E : Chunk
        {
            public Chunk01E(CGameItemModel node, Stream stream, int? length) : base(node, 0x01E, length)
            {
                Progress += stream.Read(out uint[] dsgadsgasd, 4 * 4);
            }
        }

        [Chunk(0x2E002000, 0x01F)]
        public class Chunk01F : Chunk
        {
            public int Version { get; set; }

            public Chunk01F(CGameItemModel node, Stream stream, int? length) : base(node, 0x01F, length)
            {
                Progress += stream.Read(out int version);
                Version = version;

                Progress += stream.Read(out int numOfSomething);
                for(var i = 0; i < numOfSomething; i++)
                {
                    Progress += stream.Read(out uint[] adsfga, 4 * 4);
                }

                Progress += stream.Read(out uint _); // 0
            }
        }

        [Chunk(0x2E002000, 0x020)]
        public class Chunk020 : Chunk
        {
            public Chunk020(CGameItemModel node, Stream stream, int? length) : base(node, 0x020, length)
            {
                Progress += stream.Read(out int[] dsgadsgasd, 3 * 4);

            }
        }

        [Chunk(0x2E002000, 0x021)]
        public class Chunk021 : Chunk
        {
            public Chunk021(CGameItemModel node, Stream stream, int? length) : base(node, 0x021, length)
            {
                Progress += stream.Read(out int[] dsgadsgasd, 2 * 4);
            }
        }

        [Chunk(0x2E002000, 0x023)]
        public class Chunk023 : Chunk
        {
            public Chunk023(CGameItemModel node, Stream stream, int? length) : base(node, 0x023, length)
            {
                Progress += stream.Read(out byte idk);
                Progress += stream.Read(out uint _);
                Progress += stream.Read(out uint _);
            }
        }

        [Chunk(0x2E002000, 0x024)]
        public class Chunk024 : Chunk
        {
            public Chunk024(CGameItemModel node, Stream stream, int? length) : base(node, 0x024, length)
            {
                Progress += stream.Read(out uint[] dsgadsgasd, 4 * 4);
            }
        }
    }
}
