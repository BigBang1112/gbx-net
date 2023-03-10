namespace GBX.NET.Engines.Scene;

public partial class CSceneVehicleCar
{
    public class Sample : CGameGhost.Data.Sample
    {
        public Vec3 U01 { get; set; }
        public float U02 { get; set; }
        public float U03 { get; set; }
        public float U04 { get; set; }
        public float U05 { get; set; }
        public float U06 { get; set; }
        public float U07 { get; set; }
        public float U08 { get; set; }
        public float U09 { get; set; }
        public float U10 { get; set; }
        public float U13 { get; set; }
        public float U14 { get; set; }
        public float U15 { get; set; }
        public float U16 { get; set; }
        public float U17 { get; set; }
        public byte U18 { get; set; }
        public float U19 { get; set; }
        public byte U20 { get; set; }
        public float U21 { get; set; }
        public byte U22 { get; set; }
        public float U23 { get; set; }
        public byte U24 { get; set; }
        public byte U25 { get; set; }
        public float U26 { get; set; }
        public byte U27 { get; set; }

        internal Sample(TimeInt32 time, byte[] data) : base(time, data)
        {
        }

        public override void Read(MemoryStream ms, GameBoxReader r, int version)
        {
            // CHmsDynaReplayItem::RestoreDynaItemState

            // HmsStateVersion == 0 (EHmsDynaItemSaveStateVersion_TmNetworkAfter260205)
            // Position 9-byte Vec3
            // Rotation = r.ReadQuat6();

            // HmsStateVersion == 1 (EHmsDynaItemSaveStateVersion_TmReplayAfter260205)
            Position = r.ReadVec3();
            Rotation = r.ReadQuat6();
            Velocity = r.ReadVec3_4();
            U01 = r.ReadVec3_4();
            
            //
            
            if (version >= 8)
            {
                U02 = r.ReadUInt16() / 65535f * 11000 - 1000;
                U03 = r.ReadUInt16() / 65535f * 2000 - 1000; // rpm?
                U03 = r.ReadUInt16() / 65535f * 30000; // rpm?
                U04 = r.ReadUInt16() / 65535f * 1608.495f;
                U05 = r.ReadUInt16() / 65535f * 1608.495f;
                U06 = r.ReadUInt16() / 65535f * 1608.495f;
                U07 = r.ReadUInt16() / 65535f * 1608.495f;
                U08 = r.ReadByte() / 255f * 2 - 1;
                U09 = r.ReadByte() / 255f;
                U10 = r.ReadByte() / 255f;

                var unknownValue1 = U09 + U10; // clamped between 0-1
                var unknownCondition = U09 < U10;

                var unavailableVal = r.ReadUInt16();

                U13 = r.ReadByte() / 255f * 2 - 1;
                U14 = r.ReadByte() / 255f * 2 - 1;
                U15 = r.ReadByte() / 255f;
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                U16 = r.ReadByte() / 255f * MathF.PI * 2 - MathF.PI;
#else
                U16 = r.ReadByte() / 255f * (float)Math.PI * 2 - (float)Math.PI;
#endif
                U17 = r.ReadByte() / 255f * 4 - 2;
                
                U18 = r.ReadByte();
                var u18Condition = U18 == 0xd;
                
                U19 = r.ReadByte() / 255f * 4 - 2;
                
                U20 = r.ReadByte();
                var u20Condition = U20 == 0xd;
                
                U21 = r.ReadByte() / 255f * 4 - 2;
                
                U22 = r.ReadByte();
                var u22Condition = U22 == 0xd;
                
                U23 = r.ReadByte() / 255f * 4 - 2;

                U24 = r.ReadByte();
                var u24Condition = U24 == 0xd;
                
                U25 = r.ReadByte(); // first 3 bits are relevant

                U26 = (r.ReadByte() & 0x40) == 0 ? 0 : 1;

                U27 = r.ReadByte(); // similar to U26, some form of flags
            }
        }
    }
}
