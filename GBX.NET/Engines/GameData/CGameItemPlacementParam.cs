using System.Diagnostics;

namespace GBX.NET.Engines.GameData
{
    [Node(0x2E020000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameItemPlacementParam : Node
    {
        #region Constants

        const int yawOnlyBit = 1;
        const int notOnObjectBit = 2;
        const int autoRotationBit = 3;
        const int switchPivotManuallyBit = 4;

        #endregion

        #region Fields

        private short flags;
        private Vec3 cube_Center;
        private float cube_Size;
        private float gridSnap_HStep;
        private float gridSnap_VStep;
        private float gridSnap_HOffset;
        private float gridSnap_VOffset;
        private float flyStep;
        private float flyOffset;
        private float pivotSnap_Distance;
        private Vec3[] pivotPositions;

        #endregion

        #region Properties

        public short Flags
        {
            get
            {
                DiscoverChunk<Chunk2E020000>();
                return flags;
            }
            set
            {
                DiscoverChunk<Chunk2E020000>();
                flags = value;
            }
        }

        [NodeMember]
        public bool YawOnly
        {
            get => (Flags & (1 << yawOnlyBit)) != 0;
            set
            {
                if (value) Flags |= 1 << yawOnlyBit;
                else Flags &= ~(1 << yawOnlyBit);
            }
        }

        [NodeMember]
        public bool NotOnObject
        {
            get => (Flags & (1 << notOnObjectBit)) != 0;
            set
            {
                if (value) Flags |= 1 << notOnObjectBit;
                else Flags &= ~(1 << notOnObjectBit);
            }
        }

        [NodeMember]
        public bool AutoRotation
        {
            get => (Flags & (1 << autoRotationBit)) != 0;
            set
            {
                if (value) Flags |= 1 << autoRotationBit;
                else Flags &= ~(1 << autoRotationBit);
            }
        }

        [NodeMember]
        public bool SwitchPivotManually
        {
            get => (Flags & (1 << switchPivotManuallyBit)) != 0;
            set
            {
                if (value) Flags |= 1 << switchPivotManuallyBit;
                else Flags &= ~(1 << switchPivotManuallyBit);
            }
        }

        [NodeMember]
        public Vec3 Cube_Center
        {
            get
            {
                DiscoverChunk<Chunk2E020000>();
                return cube_Center;
            }
            set
            {
                DiscoverChunk<Chunk2E020000>();
                cube_Center = value;
            }
        }

        [NodeMember]
        public float Cube_Size
        {
            get
            {
                DiscoverChunk<Chunk2E020000>();
                return cube_Size;
            }
            set
            {
                DiscoverChunk<Chunk2E020000>();
                cube_Size = value;
            }
        }

        [NodeMember]
        public float GridSnap_HStep
        {
            get
            {
                DiscoverChunk<Chunk2E020000>();
                return gridSnap_HStep;
            }
            set
            {
                DiscoverChunk<Chunk2E020000>();
                gridSnap_HStep = value;
            }
        }

        [NodeMember]
        public float GridSnap_VStep
        {
            get
            {
                DiscoverChunk<Chunk2E020000>();
                return gridSnap_VStep;
            }
            set
            {
                DiscoverChunk<Chunk2E020000>();
                gridSnap_VStep = value;
            }
        }

        [NodeMember]
        public float GridSnap_HOffset
        {
            get
            {
                DiscoverChunk<Chunk2E020000>();
                return gridSnap_HOffset;
            }
            set
            {
                DiscoverChunk<Chunk2E020000>();
                gridSnap_HOffset = value;
            }
        }

        [NodeMember]
        public float GridSnap_VOffset
        {
            get
            {
                DiscoverChunk<Chunk2E020000>();
                return gridSnap_VOffset;
            }
            set
            {
                DiscoverChunk<Chunk2E020000>();
                gridSnap_VOffset = value;
            }
        }

        [NodeMember]
        public float FlyStep
        {
            get
            {
                DiscoverChunk<Chunk2E020000>();
                return flyStep;
            }
            set
            {
                DiscoverChunk<Chunk2E020000>();
                flyStep = value;
            }
        }

        [NodeMember]
        public float FlyOffset
        {
            get
            {
                DiscoverChunk<Chunk2E020000>();
                return flyOffset;
            }
            set
            {
                DiscoverChunk<Chunk2E020000>();
                flyOffset = value;
            }
        }

        [NodeMember]
        public float PivotSnap_Distance
        {
            get
            {
                DiscoverChunk<Chunk2E020000>();
                return pivotSnap_Distance;
            }
            set
            {
                DiscoverChunk<Chunk2E020000>();
                pivotSnap_Distance = value;
            }
        }

        [NodeMember]
        public Vec3[] PivotPositions
        {
            get
            {
                DiscoverChunk<Chunk2E020001>();
                return pivotPositions;
            }
            set
            {
                DiscoverChunk<Chunk2E020001>();
                pivotPositions = value;
            }
        }

        #endregion

        #region Chunks

        #region 0x000 skippable chunk

        /// <summary>
        /// CGameItemPlacementParam 0x000 skippable chunk
        /// </summary>
        [Chunk(0x2E020000)]
        public class Chunk2E020000 : SkippableChunk<CGameItemPlacementParam>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameItemPlacementParam n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                n.Flags = rw.Int16(n.Flags);
                n.Cube_Center = rw.Vec3(n.Cube_Center);
                n.Cube_Size = rw.Single(n.Cube_Size);
                n.GridSnap_HStep = rw.Single(n.GridSnap_HStep);
                n.GridSnap_VStep = rw.Single(n.GridSnap_VStep);
                n.GridSnap_HOffset = rw.Single(n.GridSnap_HOffset);
                n.GridSnap_VOffset = rw.Single(n.GridSnap_VOffset);
                n.FlyStep = rw.Single(n.FlyStep);
                n.FlyOffset = rw.Single(n.FlyOffset);
                n.PivotSnap_Distance = rw.Single(n.PivotSnap_Distance);
            }
        }

        #endregion

        #region 0x001 skippable chunk (pivot positions)

        /// <summary>
        /// CGameItemPlacementParam 0x001 skippable chunk (pivot positions)
        /// </summary>
        [Chunk(0x2E020001, "pivot positions")]
        public class Chunk2E020001 : SkippableChunk<CGameItemPlacementParam>
        {
            public int U01 { get; set; }

            public override void ReadWrite(CGameItemPlacementParam n, GameBoxReaderWriter rw)
            {
                n.PivotPositions = rw.Array(n.PivotPositions,
                    (i, r) => r.ReadVec3(),
                    (x, w) => w.Write(x));
                U01 = rw.Int32(U01);
            }
        }

        #endregion

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameItemPlacementParam node;

            public short Flags => node.Flags;
            public bool YawOnly => node.YawOnly;
            public bool NotOnObject => node.NotOnObject;
            public bool AutoRotation => node.AutoRotation;
            public bool SwitchPivotManually => node.SwitchPivotManually;
            public Vec3 Cube_Center => node.Cube_Center;
            public float Cube_Size => node.Cube_Size;
            public float GridSnap_HStep => node.GridSnap_HStep;
            public float GridSnap_VStep => node.GridSnap_VStep;
            public float GridSnap_HOffset => node.GridSnap_HOffset;
            public float GridSnap_VOffset => node.GridSnap_VOffset;
            public float FlyStep => node.FlyStep;
            public float FlyOffset => node.FlyOffset;
            public float PivotSnap_Distance => node.PivotSnap_Distance;
            public Vec3[] PivotPositions => node.PivotPositions;

            public ChunkSet Chunks => node.Chunks;

            public DebugView(CGameItemPlacementParam node) => this.node = node;
        }

        #endregion
    }
}