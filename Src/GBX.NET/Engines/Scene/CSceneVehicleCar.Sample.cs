namespace GBX.NET.Engines.Scene;

public partial class CSceneVehicleCar
{
    public class Sample : CGameGhost.Data.Sample
    {
        public float U02 { get; set; }
        public float U03 { get; set; }
        public float U04 { get; set; }
        public float U05 { get; set; }
        public float U06 { get; set; }
        public float U07 { get; set; }
        public float U08 { get; set; }
        public float U09 { get; set; }
        public float U10 { get; set; }
        public float U09_U10_1 => U09 + U10; // clamped between 0-1
        public bool U09_U10_2 => U09 < U10;
        public ushort U11 { get; set; }
        public float U13 { get; set; }
        public float U14 { get; set; }
        public float U15 { get; set; }
        public float U16 { get; set; }
        public float U17 { get; set; }
        public CPlugSurface.MaterialId FLGroundContactMaterial { get; set; }
        public float U19 { get; set; }
        public CPlugSurface.MaterialId FRGroundContactMaterial { get; set; }
        public float U21 { get; set; }
        public CPlugSurface.MaterialId RRGroundContactMaterial { get; set; }
        public float U23 { get; set; }
        public CPlugSurface.MaterialId RLGroundContactMaterial { get; set; }
        public byte U25 { get; set; }
        public float U26 { get; set; }
        public byte U27 { get; set; }
        public float U27_1 => (U27 & 1) == 0 ? 0 : 1f;
        public bool U27_2 => Convert.ToBoolean((U27 >> 1) & 1);
        public float U27_3 => (U27 & 4) == 0 ? 0 : 1f;
        public bool U27_4 => Convert.ToBoolean((U27 >> 3) & 1);
        public float U27_5 => (U27 & 0x10) == 0 ? 0 : 1f;
        public bool U27_6 => Convert.ToBoolean((U27 >> 5) & 1);
        public int U27_7 => U27 >> 7;
        public float U27_8 => ((U27 & 0x40) == 0 ? -1f : 1f) * 5000f;
        public float? U28 { get; set; }
        public Vec4? U33 { get; set; }
        public Vec4? U34 { get; set; }
        public float? U35 { get; set; }
        public (Vec3, Quat)[]? U35_1 { get; set; }
        public bool? U36_1 { get; set; }
        public bool? U36_2 { get; set; }
        public bool? U36_3 { get; set; }
        public bool? U36_4 { get; set; }
        public bool? U36_5 { get; set; }
        public float? U37_1 { get; set; }
        public bool? U37_2 { get; set; }
        public int? U37_3 { get; set; }
        public float? U38 { get; set; }
        public byte? U39 { get; set; }
        public float? U40 { get; set; }

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
            AngularVelocity = r.ReadVec3_4();

            // CSceneVehicleVis_RestoreStaticState

            if (version < 8)
            {
                throw new VersionNotSupportedException(version);
            }

            if (version == 13)
            {
                throw new Exception("Two bytes here");
            }

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

                U11 = r.ReadUInt16(); // it should be always 0 but sometimes it isnt

                U13 = r.ReadByte() / 255f * 2 - 1;
                U14 = r.ReadByte() / 255f * 2 - 1;
                U15 = r.ReadByte() / 255f;
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                U16 = r.ReadByte() / 255f * MathF.PI * 2 - MathF.PI;
#else
                U16 = r.ReadByte() / 255f * (float)Math.PI * 2 - (float)Math.PI;
#endif
                
                U17 = r.ReadByte() / 255f * 4 - 2; // FLDampenLenByte?
                FLGroundContactMaterial = (CPlugSurface.MaterialId)r.ReadByte();
                // + condition "in water"
                
                U19 = r.ReadByte() / 255f * 4 - 2; // FRDampenLenByte?
                FRGroundContactMaterial = (CPlugSurface.MaterialId)r.ReadByte();
                // + condition "in water"

                U21 = r.ReadByte() / 255f * 4 - 2; // RRDampenLenByte?
                RRGroundContactMaterial = (CPlugSurface.MaterialId)r.ReadByte();
                // + condition "in water"

                U23 = r.ReadByte() / 255f * 4 - 2; // RLDampenLenByte?
                RLGroundContactMaterial = (CPlugSurface.MaterialId)r.ReadByte();
                // + condition "in water"

                U25 = r.ReadByte(); // first 3 bits are relevant

                U26 = (r.ReadByte() & 0x40) == 0 ? 0 : 1;
                // (this->U26 >> 7)

                U27 = r.ReadByte(); // similar to U26, some form of flags

                if (version >= 9)
                {
                    U28 = r.ReadByte() / 255f;

                    if (version >= 10)
                    {
                        var unavailableVal2 = r.ReadUInt32();
                        
                        if (unavailableVal2 != 0)
                        {
                            throw new Exception();
                        }

                        var u33 = r.ReadByte();
                        U33 = new Vec4((u33 & 3) / 3f, ((u33 >> 2) & 3) / 3f, ((u33 >> 4) & 3) / 3f, ((u33 >> 6) & 3) / 3f);

                        var u34 = r.ReadByte();
                        U34 = new Vec4((u34 & 3) / 3f, ((u34 >> 2) & 3) / 3f, ((u34 >> 4) & 3) / 3f, ((u34 >> 6) & 3) / 3f);

                        var u35 = r.ReadByte();
                        U35 = (u34 & 3) / 3f;

                        if (version >= 11)
                        {
                            if (version >= 14)
                            {
                                var u36 = r.ReadByte();
                                U36_1 = Convert.ToBoolean(u36 & 1);
                                U36_2 = Convert.ToBoolean(u36 >> 1 & 1);
                                U36_3 = Convert.ToBoolean(u36 >> 2 & 1);
                                U36_4 = Convert.ToBoolean(u36 >> 3 & 1);
                                U36_5 = Convert.ToBoolean(u36 >> 4 & 1);

                                var u37 = r.ReadByte();
                                U37_1 = (u37 & 3) / 3f;
                                U37_2 = Convert.ToBoolean(u37 >> 3 & 1);
                                U37_3 = u37 >> 4;

                                if (version >= 15)
                                {
                                    U38 = r.ReadByte() / 255f * 5;

                                    if (version >= 16)
                                    {
                                        U39 = r.ReadByte();
                                        U40 = r.ReadByte() / 255f;
                                    }
                                }
                            }

                            var count = u35 >> 2 & 7;

                            if (version == 11 && count > 4)
                            {
                                count = 4;
                            }

                            U35_1 = new (Vec3, Quat)[count];

                            for (var i = 0; i < count; i++)
                            {
                                U35_1[i] = (r.ReadVec3(), r.ReadQuat6());
                            }
                        }
                    }
                }
            }
        }
    }
}
