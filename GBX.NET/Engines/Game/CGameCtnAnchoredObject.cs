using GBX.NET.Engines.GameData;
using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// Item on a map (0x03101000)
    /// </summary>
    /// <remarks>An item placed on a map.</remarks>
    [Node(0x03101000)]
    public class CGameCtnAnchoredObject : CMwNod
    {
        #region Fields

        private Ident itemModel;
        private Vec3 pitchYawRoll;
        private Byte3 blockUnitCoord;
        private string anchorTreeId;
        private Vec3 absolutePositionInMap;
        private CGameWaypointSpecialProperty waypointSpecialProperty;
        private short flags;
        private float scale = 1;
        private Vec3 pivotPosition;

        #endregion

        #region Properties

        /// <summary>
        /// Name of the item with collection and author
        /// </summary>
        [NodeMember]
        public Ident ItemModel
        {
            get => itemModel;
            set => itemModel = value;
        }

        /// <summary>
        /// Pitch, yaw and roll of the item in radians.
        /// </summary>
        [NodeMember]
        public Vec3 PitchYawRoll
        {
            get => pitchYawRoll;
            set => pitchYawRoll = value;
        }

        /// <summary>
        /// Block coordinates that the item is approximately located in. It doesn't have to be provided most of the time.
        /// </summary>
        [NodeMember]
        public Byte3 BlockUnitCoord
        {
            get => blockUnitCoord;
            set => blockUnitCoord = value;
        }

        [NodeMember]
        public string AnchorTreeId
        {
            get => anchorTreeId;
            set => anchorTreeId = value;
        }

        /// <summary>
        /// The X, Y and Z position in the real world space of the item.
        /// </summary>
        [NodeMember]
        public Vec3 AbsolutePositionInMap
        {
            get => absolutePositionInMap;
            set => absolutePositionInMap = value;
        }

        /// <summary>
        /// If the item is a waypoint, contains inner waypoint info, otherwise null.
        /// </summary>
        [NodeMember]
        public CGameWaypointSpecialProperty WaypointSpecialProperty
        {
            get => waypointSpecialProperty;
            set => waypointSpecialProperty = value;
        }

        /// <summary>
        /// Flags of the item.
        /// </summary>
        [NodeMember]
        public short Flags
        {
            get => flags;
            set => flags = value;
        }

        /// <summary>
        /// Scale of the item.
        /// </summary>
        [NodeMember]
        public float Scale
        {
            get => scale;
            set => scale = value;
        }

        /// <summary>
        /// Pivot position of the item. Useful for making rotations around a different point than center.
        /// </summary>
        [NodeMember]
        public Vec3 PivotPosition
        {
            get => pivotPosition;
            set => pivotPosition = value;
        }

        /// <summary>
        /// Variant index of the item. Taken from flags.
        /// </summary>
        [NodeMember]
        public int Variant
        {
            get => (flags >> 8) & 15;
            set => flags = (short)((flags & 0xF0FF) | ((value & 15) << 8));
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return ItemModel.ToString();
        }

        #endregion

        #region Chunks

        #region 0x002 chunk

        /// <summary>
        /// CGameCtnAnchoredObject 0x002 chunk
        /// </summary>
        [Chunk(0x03101002)]
        public class Chunk03101002 : Chunk<CGameCtnAnchoredObject>
        {
            private int version = 7;
            private int u01 = -1;
            private Vec3 u02;
            private Vec3 u03;

            /// <summary>
            /// Version of the chunk. For the lastst TM2 version, version 7 the latest, in TM®, the latest known version is 8.
            /// </summary>
            public int Version
            {
                get => version;
                set => version = value;
            }

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public Vec3 U02
            {
                get => u02;
                set => u02 = value;
            }

            public Vec3 U03
            {
                get => u03;
                set => u03 = value;
            }

            public override void ReadWrite(CGameCtnAnchoredObject n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
                rw.Ident(ref n.itemModel);
                rw.Vec3(ref n.pitchYawRoll);
                rw.Byte3(ref n.blockUnitCoord);
                rw.Id(ref n.anchorTreeId);
                rw.Vec3(ref n.absolutePositionInMap);

                if(rw.Mode == GameBoxReaderWriterMode.Read)
                    n.waypointSpecialProperty = Parse<CGameWaypointSpecialProperty>(rw.Reader);
                else if(rw.Mode == GameBoxReaderWriterMode.Write)
                {
                    if (n.WaypointSpecialProperty == null)
                        rw.Writer.Write(-1);
                    else
                    {
                        rw.Writer.Write(n.WaypointSpecialProperty.ID);
                        n.WaypointSpecialProperty.Write(rw.Writer);
                    }
                }

                if (version >= 4)
                {
                    rw.Int16(ref n.flags);

                    if (version >= 5)
                    {
                        rw.Vec3(ref n.pivotPosition);

                        if (version >= 6)
                        {
                            rw.Single(ref n.scale);

                            if (version >= 8) // TM 2020
                            {
                                rw.Vec3(ref u02);
                                rw.Vec3(ref u03);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region 0x004 skippable chunk

        /// <summary>
        /// CGameCtnAnchoredObject 0x004 skippable chunk
        /// </summary>
        [Chunk(0x03101004)]
        public class Chunk03101004 : SkippableChunk<CGameCtnAnchoredObject>
        {
            public int U01 { get; set; } = 0;
            public int U02 { get; set; } = -1;

            public override void ReadWrite(CGameCtnAnchoredObject n, GameBoxReaderWriter rw)
            {
                U01 = rw.Int32(U01);
                U02 = rw.Int32(U02);
            }
        }

        #endregion

        #endregion
    }
}
