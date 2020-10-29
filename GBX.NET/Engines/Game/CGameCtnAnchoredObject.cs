using GBX.NET.Engines.GameData;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.Serialization;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// Item on a map (0x03101000)
    /// </summary>
    /// <remarks>An item placed on a map.</remarks>
    [Node(0x03101000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnAnchoredObject : Node
    {
        #region Properties

        /// <summary>
        /// Name of the item with collection and author
        /// </summary>
        [NodeMember]
        public Meta ItemModel { get; set; }

        /// <summary>
        /// Pitch, yaw and roll of the item in radians.
        /// </summary>
        [NodeMember]
        public Vec3 PitchYawRoll { get; set; }

        /// <summary>
        /// Block coordinates that the item is approximately located in. It doesn't have to be provided most of the time.
        /// </summary>
        [NodeMember]
        public Byte3 BlockUnitCoord { get; set; }

        /// <summary>
        /// The X, Y and Z position in the real world space of the item.
        /// </summary>
        [NodeMember]
        public Vec3 AbsolutePositionInMap { get; set; }

        /// <summary>
        /// If the item is a waypoint, contains inner waypoint info, otherwise <see cref="null"/>.
        /// </summary>
        [NodeMember]
        public CGameWaypointSpecialProperty WaypointSpecialProperty { get; set; }

        /// <summary>
        /// Flags of the item.
        /// </summary>
        [NodeMember]
        public short Flags { get; set; }

        /// <summary>
        /// Scale of the item.
        /// </summary>
        [NodeMember]
        public float Scale { get; set; } = 1;

        /// <summary>
        /// Pivot position of the item. Useful for making rotations around a different point than center.
        /// </summary>
        [NodeMember]
        public Vec3 PivotPosition { get; set; }

        /// <summary>
        /// Variant index of the item. Taken from flags.
        /// </summary>
        [NodeMember]
        public int Variant
        {
            get => (Flags >> 8) & 15;
            set => Flags = (short)((Flags & 0xF0FF) | ((value & 15) << 8));
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
            /// <summary>
            /// Version of the chunk. For the lastst TM2 version, version 7 the latest, in TM®, the latest known version is 8.
            /// </summary>
            public int Version { get; set; } = 7;
            public int U01 { get; set; } = -1;

            public Vec3 U02 { get; set; }
            public Vec3 U03 { get; set; }

            public override void Read(CGameCtnAnchoredObject n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();
                n.ItemModel = r.ReadMeta();
                n.PitchYawRoll = r.ReadVec3();
                n.BlockUnitCoord = r.ReadByte3();
                U01 = r.ReadInt32();
                n.AbsolutePositionInMap = r.ReadVec3();
                n.WaypointSpecialProperty = Parse<CGameWaypointSpecialProperty>(r);
                n.Flags = r.ReadInt16();
                n.PivotPosition = r.ReadVec3();
                n.Scale = r.ReadSingle();

                if(Version >= 8) // TM 2020
                {
                    U02 = r.ReadVec3();
                    U03 = r.ReadVec3();
                }
            }

            public override void Write(CGameCtnAnchoredObject n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);
                w.Write(n.ItemModel);
                w.Write(n.PitchYawRoll);
                w.Write(n.BlockUnitCoord);
                w.Write(U01);
                w.Write(n.AbsolutePositionInMap);

                if (n.WaypointSpecialProperty == null)
                    w.Write(-1);
                else
                {
                    w.Write(n.WaypointSpecialProperty.ID);
                    n.WaypointSpecialProperty.Write(w);
                }

                w.Write(n.Flags);
                w.Write(n.PivotPosition);
                w.Write(n.Scale);

                if (Version >= 8) // TM 2020
                {
                    w.Write(U02);
                    w.Write(U03);
                }
            }
        }

        #endregion

        #region 0x004 chunk

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

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnAnchoredObject node;

            public Meta ItemModel => node.ItemModel;
            public Vec3? PitchYawRoll => node.PitchYawRoll;
            public Byte3? BlockUnitCoord => node.BlockUnitCoord;
            public Vec3? AbsolutePositionInMap => node.AbsolutePositionInMap;
            public CGameWaypointSpecialProperty WaypointSpecialProperty => node.WaypointSpecialProperty;
            public short Flags => node.Flags;
            public float Scale => node.Scale;
            public Vec3 PivotPosition => node.PivotPosition;
            public int Variant => node.Variant;

            public DebugView(CGameCtnAnchoredObject node) => this.node = node;
        }

        #endregion
    }
}
