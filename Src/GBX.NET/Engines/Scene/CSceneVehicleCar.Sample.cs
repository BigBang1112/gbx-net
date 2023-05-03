namespace GBX.NET.Engines.Scene;

public partial class CSceneVehicleCar
{
    public class Sample : CGameGhost.Data.Sample
    {
        public float SpeedForward { get; set; }
        public float SpeedSideward { get; set; }
        public float RPM { get; set; }
        public float FLWheelRotation { get; set; }
        public float FRWheelRotation { get; set; }
        public float RRWheelRotation { get; set; }
        public float RLWheelRotation { get; set; }
        public float Steer { get; set; }
        public float Gas { get; set; }
        public float Brake { get; set; }
        public float U09_U10_1 => Gas + Brake; // clamped between 0-1
        public bool U09_U10_2 => Gas < Brake;
        public byte U11 { get; set; }
        public byte U12 { get; set; }
        public float U13 { get; set; }
        public float U14 { get; set; }
        public float TurboStrength { get; set; }
        public float SteerFront { get; set; }
        public float FLDampenLen { get; set; }
        public CPlugSurface.MaterialId FLGroundContactMaterial { get; set; }
        public float FRDampenLen { get; set; }
        public CPlugSurface.MaterialId FRGroundContactMaterial { get; set; }
        public float RRDampenLen { get; set; }
        public CPlugSurface.MaterialId RRGroundContactMaterial { get; set; }
        public float RLDampenLen { get; set; }
        public CPlugSurface.MaterialId RLGroundContactMaterial { get; set; }
        public byte U25 { get; set; }
        public int Horn { get; set; }
        public bool IsTurbo { get; set; }
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
        public float? DirtBlend { get; set; }
        public Vec4? U33 { get; set; }
        public Vec4? U34 { get; set; }
        public float? U35 { get; set; }
        public (Vec3, Quat, byte)[]? U35_1 { get; set; }
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

        internal override void Read(MemoryStream ms, GameBoxReader r, int version)
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

            if (version < 7)
            {
                throw new VersionNotSupportedException(version);
            }

            if (version == 13)
            {
                throw new Exception("Two bytes here");
            }

            if (version >= 7)
            {
                SpeedForward = (r.ReadUInt16() / 65535f * 11000 - 1000) * 3.6f;
                SpeedSideward = r.ReadUInt16() / 65535f * 2000 - 1000;
                RPM = r.ReadUInt16() / 65535f * 30000;
                FLWheelRotation = r.ReadUInt16() / 65535f * 1608.495f;
                FRWheelRotation = r.ReadUInt16() / 65535f * 1608.495f;
                RRWheelRotation = r.ReadUInt16() / 65535f * 1608.495f;
                RLWheelRotation = r.ReadUInt16() / 65535f * 1608.495f;
                Steer = r.ReadByte() / 255f * 2 - 1;
                Gas = r.ReadByte() / 255f;
                Brake = r.ReadByte() / 255f;

                U11 = r.ReadByte(); // it should be always 0 but sometimes it isnt

                if (version >= 8)
                {
                    U12 = r.ReadByte(); // it should be always 0 but sometimes it isnt
                }
                
                U13 = r.ReadByte() / 255f * 2 - 1;
                U14 = r.ReadByte() / 255f * 2 - 1;
                TurboStrength = r.ReadByte() / 255f;
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                SteerFront = r.ReadByte() / 255f * MathF.PI * 2 - MathF.PI;
#else
                SteerFront = r.ReadByte() / 255f * (float)Math.PI * 2 - (float)Math.PI;
#endif
                
                FLDampenLen = r.ReadByte() / 255f * 4 - 2;
                FLGroundContactMaterial = (CPlugSurface.MaterialId)r.ReadByte();
                // + condition "in water"
                
                FRDampenLen = r.ReadByte() / 255f * 4 - 2;
                FRGroundContactMaterial = (CPlugSurface.MaterialId)r.ReadByte();
                // + condition "in water"

                RRDampenLen = r.ReadByte() / 255f * 4 - 2; // RRDampenLenByte?
                RRGroundContactMaterial = (CPlugSurface.MaterialId)r.ReadByte();
                // + condition "in water"

                RLDampenLen = r.ReadByte() / 255f * 4 - 2; // RLDampenLenByte?
                RLGroundContactMaterial = (CPlugSurface.MaterialId)r.ReadByte();
                // + condition "in water"

                if (version >= 8)
                {
                    U25 = r.ReadByte(); // first 3 bits are relevant
                    Horn = U25 & 3;
                    IsTurbo = (U25 & 128) != 0;

                    U26 = (r.ReadByte() & 0x40) == 0 ? 0 : 1;
                    // (this->U26 >> 7)

                    U27 = r.ReadByte(); // similar to U26, some form of flags

                    if (version >= 9)
                    {
                        DirtBlend = r.ReadByte() / 255f;

                        if (version >= 10)
                        {
                            var unavailableVal2 = r.ReadUInt32();

                            if (unavailableVal2 != 0)
                            {
                                throw new Exception();
                            }

                            // damage amount

                            var u33 = r.ReadByte();
                            U33 = new Vec4((u33 & 3) / 3f, ((u33 >> 2) & 3) / 3f, ((u33 >> 4) & 3) / 3f, ((u33 >> 6) & 3) / 3f);

                            var u34 = r.ReadByte();
                            U34 = new Vec4((u34 & 3) / 3f, ((u34 >> 2) & 3) / 3f, ((u34 >> 4) & 3) / 3f, ((u34 >> 6) & 3) / 3f);

                            var u35 = r.ReadByte();
                            U35 = (u35 & 3) / 3f;

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

                                U35_1 = new (Vec3, Quat, byte)[count];

                                for (var i = 0; i < count; i++)
                                {
                                    U35_1[i] = (r.ReadVec3(), r.ReadQuat6(), r.ReadByte());
                                }
                            }
                        }
                    }
                }
            }

            if (ms.Position != ms.Length)
            {
                throw new Exception("Not all bytes were read");
            }
        }
    }
}
