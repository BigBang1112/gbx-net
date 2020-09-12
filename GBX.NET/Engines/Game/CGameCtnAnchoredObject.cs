using GBX.NET.Engines.GameData;
using System.Numerics;
using System.Runtime.Serialization;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// Item on a map (0x03101000)
    /// </summary>
    /// <remarks>An item placed on a map.</remarks>
    [Node(0x03101000)]
    public class CGameCtnAnchoredObject : Node
    {
        public Meta ItemModel { get; set; }

        public Vec3? PitchYawRoll { get; set; }

        public Byte3 BlockUnitCoord { get; set; }

        public Vec3? AbsolutePositionInMap { get; set; }

        public CGameWaypointSpecialProperty WaypointSpecialProperty { get; set; }

        public short Flags { get; set; }

        public float Scale { get; set; } = 1;

        public Vec3 PivotPosition { get; set; }

        public int Variant
        {
            get => (Flags >> 8) & 15;
            set => Flags = (short)((Flags & 0xF0FF) | ((value & 15) << 8));
        }

        public CGameCtnAnchoredObject(ILookbackable lookbackable) : base(lookbackable)
        {
            
        }

        public CGameCtnAnchoredObject(ILookbackable lookbackable, uint classId) : base(lookbackable, classId)
        {

        }

        public CGameCtnAnchoredObject(Chunk chunk) : base(chunk)
        {

        }

        public override string ToString()
        {
            return ItemModel.ToString();
        }

        #region Chunks

        #region 0x002 chunk

        /// <summary>
        /// CGameCtnAnchoredObject 0x002 chunk
        /// </summary>
        [Chunk(0x03101002)]
        public class Chunk03101002 : Chunk<CGameCtnAnchoredObject>
        {
            public int Version { get; set; } = 7;
            public int Unknown1 { get; set; } = -1;

            public Vec3 Unknown3 { get; set; }
            public Vec3 Unknown4 { get; set; }

            public override void Read(CGameCtnAnchoredObject n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();
                n.ItemModel = r.ReadMeta();
                n.PitchYawRoll = r.ReadVec3();
                n.BlockUnitCoord = r.ReadByte3();
                Unknown1 = r.ReadInt32();
                n.AbsolutePositionInMap = r.ReadVec3();
                var specialWaypoint = r.ReadInt32();
                if(specialWaypoint != -1)
                    n.WaypointSpecialProperty = Parse<CGameWaypointSpecialProperty>(Node.Body, r, true);
                n.Flags = r.ReadInt16();
                n.PivotPosition = r.ReadVec3();
                n.Scale = r.ReadSingle();

                if(Version >= 8) // TM 2020
                {
                    Unknown3 = r.ReadVec3();
                    Unknown4 = r.ReadVec3();
                }
            }

            public override void Write(CGameCtnAnchoredObject n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);
                w.Write(n.ItemModel);
                w.Write(n.PitchYawRoll.GetValueOrDefault());
                w.Write(n.BlockUnitCoord);
                w.Write(Unknown1);
                w.Write(n.AbsolutePositionInMap.GetValueOrDefault());

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
                    w.Write(Unknown3);
                    w.Write(Unknown4);
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
            public int Unknown1 { get; set; } = 0;
            public int Unknown2 { get; set; } = -1;

            public override void ReadWrite(CGameCtnAnchoredObject n, GameBoxReaderWriter rw)
            {
                Unknown1 = rw.Int32(Unknown1);
                Unknown2 = rw.Int32(Unknown2);
            }
        }

        #endregion

        #endregion
    }
}
