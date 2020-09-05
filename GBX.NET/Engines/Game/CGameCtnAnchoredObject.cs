using GBX.NET.Engines.GameData;
using System.Numerics;

namespace GBX.NET.Engines.Game
{
    [Node(0x03101000)]
    public class CGameCtnAnchoredObject : Node
    {
        public Meta ItemModel
        {
            get => GetValue<Chunk002>(x => x.ItemModel) as Meta;
            set => SetValue<Chunk002>(x => x.ItemModel = value);
        }

        public Vector3? PitchYawRoll
        {
            get => GetValue<Chunk002>(x => x.PitchYawRoll) as Vector3?;
            set => SetValue<Chunk002>(x => x.PitchYawRoll = value.GetValueOrDefault());
        }

        public Byte3? BlockUnitCoord
        {
            get => GetValue<Chunk002>(x => x.BlockUnitCoord) as Byte3?;
            set => SetValue<Chunk002>(x => x.BlockUnitCoord = value.GetValueOrDefault());
        }

        public Vector3? AbsolutePositionInMap
        {
            get => GetValue<Chunk002>(x => x.AbsolutePositionInMap) as Vector3?;
            set => SetValue<Chunk002>(x => x.AbsolutePositionInMap = value.GetValueOrDefault());
        }

        public CGameWaypointSpecialProperty WaypointSpecialProperty
        {
            get => GetValue<Chunk002>(x => x.WaypointSpecialProperty) as CGameWaypointSpecialProperty;
            set => SetValue<Chunk002>(x => x.WaypointSpecialProperty = value);
        }

        public float Scale
        {
            get => (float)GetValue<Chunk002>(x => x.Scale);
            set => SetValue<Chunk002>(x => x.Scale = value);
        }

        public Vector3 PivotPosition
        {
            get => (Vector3)GetValue<Chunk002>(x => x.PivotPosition);
            set => SetValue<Chunk002>(x => x.PivotPosition = value);
        }

        public CGameCtnAnchoredObject(ILookbackable lookbackable) : base(lookbackable, 0x03101000)
        {

        }

        public CGameCtnAnchoredObject(ILookbackable lookbackable, uint classId) : base(lookbackable, classId)
        {

        }

        public override string ToString()
        {
            return ItemModel.ToString();
        }

        #region Chunks

        #region 0x002 chunk

        [Chunk(0x03101002)]
        public class Chunk002 : Chunk
        {
            public Meta ItemModel { get; set; }
            public Vector3 PitchYawRoll { get; set; }
            public Byte3 BlockUnitCoord { get; set; } = new Byte3(0, 0, 0);
            public Vector3 AbsolutePositionInMap { get; set; }
            public CGameWaypointSpecialProperty WaypointSpecialProperty { get; set; }
            public float Scale { get; set; } = 1;

            public int Version { get; set; } = 7;
            public int Unknown1 { get; set; } = -1;
            public short Flags { get; set; } = 1;
            public Vector3 PivotPosition { get; set; }

            public int Variant
            {
                get => (Flags >> 8) & 15;
                set => Flags = (short)((Flags & 0xF0FF) | ((value & 15) << 8));
            }

            public Vector3 Unknown3 { get; set; }
            public Vector3 Unknown4 { get; set; }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();
                ItemModel = r.ReadMeta();
                PitchYawRoll = r.ReadVec3();
                BlockUnitCoord = r.ReadByte3();
                Unknown1 = r.ReadInt32();
                AbsolutePositionInMap = r.ReadVec3();
                var specialWaypoint = r.ReadInt32();
                if(specialWaypoint != -1)
                    WaypointSpecialProperty = Parse<CGameWaypointSpecialProperty>(Node.Lookbackable, r);
                Flags = r.ReadInt16();
                PivotPosition = r.ReadVec3();
                Scale = r.ReadSingle();

                if(Version >= 8) // TM 2020
                {
                    Unknown3 = r.ReadVec3();
                    Unknown4 = r.ReadVec3();
                }
            }

            public override void Write(GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);
                w.Write(ItemModel);
                w.Write(PitchYawRoll);
                w.Write(BlockUnitCoord);
                w.Write(Unknown1);
                w.Write(AbsolutePositionInMap);

                if (WaypointSpecialProperty == null)
                    w.Write(-1);
                else
                {
                    w.Write(WaypointSpecialProperty.ID);
                    WaypointSpecialProperty.Write(w);
                }

                w.Write(Flags);
                w.Write(PivotPosition);
                w.Write(Scale);

                if (Version >= 8) // TM 2020
                {
                    w.Write(Unknown3);
                    w.Write(Unknown4);
                }
            }
        }

        #endregion

        #region 0x004 chunk

        [Chunk(0x03101004)]
        public class Chunk004 : SkippableChunk
        {
            public int Unknown1 { get; set; } = 0;
            public int Unknown2 { get; set; } = -1;

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Unknown1 = rw.Int32(Unknown1);
                Unknown2 = rw.Int32(Unknown2);
            }
        }

        #endregion

        #endregion
    }
}
