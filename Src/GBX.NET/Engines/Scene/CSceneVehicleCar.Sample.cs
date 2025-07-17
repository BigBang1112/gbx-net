﻿namespace GBX.NET.Engines.Scene;

public partial class CSceneVehicleCar
{
    public sealed class Sample : CGameGhost.Data.Sample
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
        public float U11 { get; set; }
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
        public byte U25_1 => (byte)(U25 & 7);

        /// <summary>
        /// Horn counter that loops around after reaching number 3.
        /// </summary>
        public byte Horn => (byte)((U25 >> 3) & 3);

        public byte U25_3 => (byte)((U25 >> 5) & 3);
        public bool U25_4 => (U25 >> 7) != 0;
        public byte U26 { get; set; }
        public bool FLIsSliding
        {
            get => (U26 & 0x40) != 0;
            set
            {
                if (value) U26 |= 0x40;
                else U26 &= 0xBF; // 0b10111111
            }
        }

        public bool FLOnGround => (U26 & 0x80) != 0;
        public byte U27 { get; set; }
        public bool FRIsSliding
        {
            get => (U27 & 0x01) != 0;
            set
            {
                if (value) U27 |= 0x01;
                else U27 &= 0xFE; // 0b11111110
            }
        }

        public bool FROnGround => (U27 & 0x02) != 0;
        public bool RRIsSliding
        {
            get => (U27 & 0x04) != 0;
            set
            {
                if (value) U27 |= 0x04;
                else U27 &= 0xFB; // 0b11111011
            }
        }

        public bool RROnGround => (U27 & 0x08) != 0;
        public bool RLIsSliding
        {
            get => (U27 & 0x10) != 0;
            set
            {
                if (value) U27 |= 0x10;
                else U27 &= 0xEF; // 0b11101111
            }
        }

        public bool RLOnGround => (U27 & 0x20) != 0;
        public bool U27_7 => (U27 & 0x40) != 0;
        public bool U27_8 => (U27 & 0x80) != 0;
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

        internal override void Read(MemoryStream ms, GbxReader r, int version)
        {
            // CHmsDynaReplayItem::RestoreDynaItemState


            // HmsStateVersion == 0 (EHmsDynaItemSaveStateVersion_TmNetworkAfter260205)
            // Position 9-byte Vec3
            // Rotation = r.ReadQuat6();

            // HmsStateVersion == 1 (EHmsDynaItemSaveStateVersion_TmReplayAfter260205)
            Position = version == 13 ? r.ReadVec3_9() : r.ReadVec3();
            Rotation = r.ReadQuat6();

            if (version == 13)
            {
                // SVehicleSimpleNetState::ToVehicle
                FLGroundContactMaterial = CPlugSurface.MaterialId.Asphalt;
                FRGroundContactMaterial = CPlugSurface.MaterialId.Asphalt;
                RRGroundContactMaterial = CPlugSurface.MaterialId.Asphalt;
                RLGroundContactMaterial = CPlugSurface.MaterialId.Asphalt;

                var netData = r.ReadUInt16();

                Brake = netData >> 1 & 1;

                var isSliding = (netData >> 2 & 1) != 0;
                FLIsSliding = isSliding;
                FRIsSliding = isSliding;
                RRIsSliding = isSliding;
                RLIsSliding = isSliding;

                // Calculate RPM
                var min = 100f; // guessed cuz its stored by vehicle
                var max = 11000f; // guessed cuz its stored by vehicle
                var ratio = min / max; // min/max rpm i guess
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                var powRatio = MathF.Pow(ratio, 3.0f);
#else
                var powRatio = Math.Pow(ratio, 3.0f);
#endif
                var normalizedValue = (netData >> 9) / 127.0f;
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                var rpmValue = MathF.Pow(normalizedValue * (1.0f - powRatio) + powRatio, 0.3f);
#else
                var rpmValue = Math.Pow(normalizedValue * (1.0f - powRatio) + powRatio, 0.3f);
#endif
                rpmValue *= max;
                RPM = (float)rpmValue;
                return;
            }

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

                U11 = r.ReadByte() / 255f;

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
                    U25 = r.ReadByte();
                    U26 = r.ReadByte();
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

                                // count is broken in specific cases like the last sample of a ghost
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