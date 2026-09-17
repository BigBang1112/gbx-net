namespace GBX.NET.Engines.Scene;

public partial class CSceneVehicleCar
{
    public sealed class Sample : CGameGhost.Data.Sample, ISampleRawData
    {
        private uint velocity;
        private uint angularVelocity;
        private ushort speedForward;
        private ushort speedSideward;
        private ushort rpm;
        private ushort flWheelRotation;
        private ushort frWheelRotation;
        private ushort rrWheelRotation;
        private ushort rlWheelRotation;
        private byte steer;
        private byte gas;
        private byte brake;
        private byte u11;
        private byte u13;
        private byte u14;
        private byte turboStrength;
        private byte steerFront;
        private byte flDampenLen;
        private byte frDampenLen;
        private byte rrDampenLen;
        private byte rlDampenLen;
        private byte? dirtBlend;
        private byte? u33;
        private byte? u34;
        private byte? u35;
        private byte? u36;
        private byte? u37;
        private byte? u38;
        private byte? u40;
        private byte? u41;
        private byte? u42;
        private byte? u43;
        private byte? u44;
        private byte? u45;
        private (Vec3, Quat, byte)[]? u35_1;

        uint ISampleRawData.Velocity { get => velocity; set => velocity = value; }
        uint ISampleRawData.AngularVelocity { get => angularVelocity; set => angularVelocity = value; }
        ushort ISampleRawData.SpeedForward { get => speedForward; set => speedForward = value; }
        ushort ISampleRawData.SpeedSideward { get => speedSideward; set => speedSideward = value; }
        ushort ISampleRawData.RPM { get => rpm; set => rpm = value; }
        ushort ISampleRawData.FLWheelRotation { get => flWheelRotation; set => flWheelRotation = value; }
        ushort ISampleRawData.FRWheelRotation { get => frWheelRotation; set => frWheelRotation = value; }
        ushort ISampleRawData.RRWheelRotation { get => rrWheelRotation; set => rrWheelRotation = value; }
        ushort ISampleRawData.RLWheelRotation { get => rlWheelRotation; set => rlWheelRotation = value; }
        byte ISampleRawData.Steer { get => steer; set => steer = value; }
        byte ISampleRawData.Gas { get => gas; set => gas = value; }
        byte ISampleRawData.Brake { get => brake; set => brake = value; }
        byte ISampleRawData.U11 { get => u11; set => u11 = value; }
        byte ISampleRawData.U13 { get => u13; set => u13 = value; }
        byte ISampleRawData.U14 { get => u14; set => u14 = value; }
        byte ISampleRawData.TurboStrength { get => turboStrength; set => turboStrength = value; }
        byte ISampleRawData.SteerFront { get => steerFront; set => steerFront = value; }
        byte ISampleRawData.FLDampenLen { get => flDampenLen; set => flDampenLen = value; }
        byte ISampleRawData.FRDampenLen { get => frDampenLen; set => frDampenLen = value; }
        byte ISampleRawData.RRDampenLen { get => rrDampenLen; set => rrDampenLen = value; }
        byte ISampleRawData.RLDampenLen { get => rlDampenLen; set => rlDampenLen = value; }
        byte? ISampleRawData.DirtBlend { get => dirtBlend; set => dirtBlend = value; }
        byte? ISampleRawData.U33 { get => u33; set => u33 = value; }
        byte? ISampleRawData.U34 { get => u34; set => u34 = value; }
        byte? ISampleRawData.U35 { get => u35; set => u35 = value; }
        byte? ISampleRawData.U36 { get => u36; set => u36 = value; }
        byte? ISampleRawData.U37 { get => u37; set => u37 = value; }
        byte? ISampleRawData.U38 { get => u38; set => u38 = value; }
        byte? ISampleRawData.U40 { get => u40; set => u40 = value; }

        public override Vec3 Velocity
        {
            get => ToVec3_4(velocity);
            set => velocity = FromVec3_4(value);
        }

        public override Vec3 AngularVelocity
        {
            get => ToVec3_4(angularVelocity);
            set => angularVelocity = FromVec3_4(value);
        }

        public float SpeedForward
        {
            get => (speedForward / 65535f * 11000 - 1000) * 3.6f;
            set => speedForward = (ushort)AdditionalMath.Clamp(Math.Round(((value / 3.6f) + 1000) / 11000f * 65535f), 0, 65535);
        }

        public float SpeedSideward
        {
            get => speedSideward / 65535f * 2000 - 1000;
            set => speedSideward = (ushort)AdditionalMath.Clamp(Math.Round((value + 1000) / 2000f * 65535f), 0, 65535);
        }

        public float RPM
        {
            get => rpm / 65535f * 30000;
            set => rpm = (ushort)AdditionalMath.Clamp(Math.Round(value / 30000f * 65535f), 0, 65535);
        }

        public float FLWheelRotation
        {
            get => flWheelRotation / 65535f * 1608.495f;
            set => flWheelRotation = (ushort)AdditionalMath.Clamp(Math.Round(value / 1608.495f * 65535f), 0, 65535);
        }

        public float FRWheelRotation
        {
            get => frWheelRotation / 65535f * 1608.495f;
            set => frWheelRotation = (ushort)AdditionalMath.Clamp(Math.Round(value / 1608.495f * 65535f), 0, 65535);
        }

        public float RRWheelRotation
        {
            get => rrWheelRotation / 65535f * 1608.495f;
            set => rrWheelRotation = (ushort)AdditionalMath.Clamp(Math.Round(value / 1608.495f * 65535f), 0, 65535);
        }

        public float RLWheelRotation
        {
            get => rlWheelRotation / 65535f * 1608.495f;
            set => rlWheelRotation = (ushort)AdditionalMath.Clamp(Math.Round(value / 1608.495f * 65535f), 0, 65535);
        }

        public float Steer
        {
            get => steer / 255f * 2 - 1;
            set => steer = (byte)AdditionalMath.Clamp(Math.Round((value + 1f) / 2f * 255f), 0, 255);
        }

        public float Gas
        {
            get => gas / 255f;
            set => gas = (byte)AdditionalMath.Clamp(Math.Round(value * 255f), 0, 255);
        }

        public float Brake
        {
            get => brake / 255f;
            set => brake = (byte)AdditionalMath.Clamp(Math.Round(value * 255f), 0, 255);
        }

#if NET5_0_OR_GREATER
        public float U09_U10_1 => Math.Clamp(Gas + Brake, 0f, 1f);
#else
        public float U09_U10_1 => (float)AdditionalMath.Clamp(Gas + Brake, 0f, 1f);
#endif
        public bool U09_U10_2 => Gas < Brake;

        public float U11
        {
            get => u11 / 255f;
            set => u11 = (byte)AdditionalMath.Clamp(Math.Round(value * 255f), 0, 255);
        }

        public byte U12 { get; set; }

        public float U13
        {
            get => u13 / 255f * 2 - 1;
            set => u13 = (byte)AdditionalMath.Clamp(Math.Round((value + 1f) / 2f * 255f), 0, 255);
        }

        public float U14
        {
            get => u14 / 255f * 2 - 1;
            set => u14 = (byte)AdditionalMath.Clamp(Math.Round((value + 1f) / 2f * 255f), 0, 255);
        }

        public float TurboStrength
        {
            get => turboStrength / 255f;
            set => turboStrength = (byte)AdditionalMath.Clamp(Math.Round(value * 255f), 0, 255);
        }

        public float SteerFront
        {
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            get => steerFront / 255f * MathF.PI * 2 - MathF.PI;
            set => steerFront = (byte)AdditionalMath.Clamp(Math.Round((value + MathF.PI) / (MathF.PI * 2) * 255f), 0, 255);
#else
            get => steerFront / 255f * (float)Math.PI * 2 - (float)Math.PI;
            set => steerFront = (byte)AdditionalMath.Clamp(Math.Round((value + Math.PI) / (Math.PI * 2) * 255f), 0, 255);
#endif
        }

        public float FLDampenLen
        {
            get => flDampenLen / 255f * 4 - 2;
            set => flDampenLen = (byte)AdditionalMath.Clamp(Math.Round((value + 2f) / 4f * 255f), 0, 255);
        }
        public CPlugSurface.MaterialId FLGroundContactMaterial { get; set; }

        public float FRDampenLen
        {
            get => frDampenLen / 255f * 4 - 2;
            set => frDampenLen = (byte)AdditionalMath.Clamp(Math.Round((value + 2f) / 4f * 255f), 0, 255);
        }
        public CPlugSurface.MaterialId FRGroundContactMaterial { get; set; }

        public float RRDampenLen
        {
            get => rrDampenLen / 255f * 4 - 2;
            set => rrDampenLen = (byte)AdditionalMath.Clamp(Math.Round((value + 2f) / 4f * 255f), 0, 255);
        }
        public CPlugSurface.MaterialId RRGroundContactMaterial { get; set; }

        public float RLDampenLen
        {
            get => rlDampenLen / 255f * 4 - 2;
            set => rlDampenLen = (byte)AdditionalMath.Clamp(Math.Round((value + 2f) / 4f * 255f), 0, 255);
        }
        public CPlugSurface.MaterialId RLGroundContactMaterial { get; set; }

        public byte U25 { get; set; }
        public byte U25_1 => (byte)(U25 & 7);
        public byte Horn => (byte)((U25 >> 3) & 3);
        public byte U25_3 => (byte)((U25 >> 5) & 3);
        public bool U25_4 => (U25 >> 7) != 0;

        public byte U26 { get; set; }
        public bool FLIsSliding
        {
            get => (U26 & 0x40) != 0;
            set { if (value) U26 |= 0x40; else U26 &= 0xBF; }
        }
        public bool FLOnGround => (U26 & 0x80) != 0;

        public byte U27 { get; set; }
        public bool FRIsSliding
        {
            get => (U27 & 0x01) != 0;
            set { if (value) U27 |= 0x01; else U27 &= 0xFE; }
        }
        public bool FROnGround => (U27 & 0x02) != 0;

        public bool RRIsSliding
        {
            get => (U27 & 0x04) != 0;
            set { if (value) U27 |= 0x04; else U27 &= 0xFB; }
        }
        public bool RROnGround => (U27 & 0x08) != 0;

        public bool RLIsSliding
        {
            get => (U27 & 0x10) != 0;
            set { if (value) U27 |= 0x10; else U27 &= 0xEF; }
        }
        public bool RLOnGround => (U27 & 0x20) != 0;
        public bool U27_7 => (U27 & 0x40) != 0;
        public bool U27_8 => (U27 & 0x80) != 0;

        public float? DirtBlend
        {
            get => dirtBlend.HasValue ? dirtBlend.Value / 255f : null;
            set => dirtBlend = value.HasValue ? (byte)AdditionalMath.Clamp(Math.Round(value.Value * 255f), 0, 255) : null;
        }

        public Vec4? U33
        {
            get => u33.HasValue ? new Vec4((u33.Value & 3) / 3f, ((u33.Value >> 2) & 3) / 3f, ((u33.Value >> 4) & 3) / 3f, ((u33.Value >> 6) & 3) / 3f) : null;
            set => UpdateVec4Flag(ref u33, value);
        }

        public Vec4? U34
        {
            get => u34.HasValue ? new Vec4((u34.Value & 3) / 3f, ((u34.Value >> 2) & 3) / 3f, ((u34.Value >> 4) & 3) / 3f, ((u34.Value >> 6) & 3) / 3f) : null;
            set => UpdateVec4Flag(ref u34, value);
        }

        public float? U35
        {
            get => u35.HasValue ? (u35.Value & 3) / 3f : null;
            set
            {
                if (!value.HasValue)
                {
                    if (u35.HasValue) u35 = (byte)(u35.Value & ~3);
                }
                else
                {
                    u35 = (byte)((u35.GetValueOrDefault() & ~3) | (byte)AdditionalMath.Clamp(Math.Round(value.Value * 3f), 0, 3));
                }
            }
        }

        public (Vec3, Quat, byte)[]? U35_1
        {
            get => u35_1;
            set
            {
                u35_1 = value;
                if (value != null)
                {
                    var count = (byte)AdditionalMath.Clamp(value.Length, 0, 7);
                    u35 = (byte)((u35.GetValueOrDefault() & ~(7 << 2)) | (count << 2));
                }
            }
        }

        public float? U35_2
        {
            get => u35.HasValue ? (u35.Value >> 5) / 7f : null;
            set
            {
                if (!value.HasValue)
                {
                    if (u35.HasValue) u35 = (byte)(u35.Value & 0x1F);
                }
                else
                {
                    u35 = (byte)((u35.GetValueOrDefault() & 0x1F) | (byte)AdditionalMath.Clamp(Math.Round(value.Value * 7f), 0, 7) << 5);
                }
            }
        }

        public bool? U36_1 { get => BitHelper.GetBit(u36, 0); set => u36 = BitHelper.SetBit(u36, 0, value); }
        public bool? U36_2 { get => BitHelper.GetBit(u36, 1); set => u36 = BitHelper.SetBit(u36, 1, value); }
        public bool? U36_3 { get => BitHelper.GetBit(u36, 2); set => u36 = BitHelper.SetBit(u36, 2, value); }
        public bool? U36_4 { get => BitHelper.GetBit(u36, 3); set => u36 = BitHelper.SetBit(u36, 3, value); }
        public bool? U36_5 { get => BitHelper.GetBit(u36, 4); set => u36 = BitHelper.SetBit(u36, 4, value); }
        public float? U37_1
        {
            get => u37.HasValue ? (u37.Value & 3) / 3f : null;
            set
            {
                if (!value.HasValue) { if (u37.HasValue) u37 = (byte)(u37.Value & ~3); }
                else u37 = (byte)((u37.GetValueOrDefault() & ~3) | (byte)AdditionalMath.Clamp(Math.Round(value.Value * 3f), 0, 3));
            }
        }

        public bool? U37_2 { get => BitHelper.GetBit(u37, 3); set => u37 = BitHelper.SetBit(u37, 3, value); }

        public int? U37_3
        {
            get => u37.HasValue ? u37.Value >> 4 : null;
            set
            {
                if (!value.HasValue) { if (u37.HasValue) u37 = (byte)(u37.Value & 0x0F); }
                else u37 = (byte)((u37.GetValueOrDefault() & 0x0F) | ((value.Value & 0x0F) << 4));
            }
        }

        public float? U38
        {
            get => u38.HasValue ? u38.Value / 255f * 5 : null;
            set => u38 = value.HasValue ? (byte)AdditionalMath.Clamp(Math.Round(value.Value / 5f * 255f), 0, 255) : null;
        }

        public byte? U39 { get; set; }

        public float? U40
        {
            get => u40.HasValue ? u40.Value / 255f : null;
            set => u40 = value.HasValue ? (byte)AdditionalMath.Clamp(Math.Round(value.Value * 255f), 0, 255) : null;
        }

        // SVehicleSimpleState_ReplayAfter100117 (version 17)
        public byte? U41 { get => u41; set => u41 = value; }

        // SVehicleSimpleState_ReplayAfter111217 (version 18)
        public byte? U42 { get => u42; set => u42 = value; }

        // Version 19 only: three packed boolean flags.
        public byte? U43 { get => u43; set => u43 = value; }

        // SVehicleSimpleState_ReplayAfter2018_03_09 (version 20)
        public byte? U44 { get => u44; set => u44 = value; }
        public byte? U45 { get => u45; set => u45 = value; }

        internal Sample(TimeInt32 time, byte[] data) : base(time, data)
        {
        }

        internal override void Read(GbxReader r, int version)
        {
            if (version < 7)
            {
                throw new VersionNotSupportedException(version);
            }

            // NSceneMgr_Vehicle::StateArchive (v17+) writes the legacy simple
            // state before the dynamic position/rotation/motion block.
            var isStateFirst = version >= 17;

            // CHmsDynaReplayItem::RestoreDynaItemState

            // HmsStateVersion == 0 (EHmsDynaItemSaveStateVersion_TmNetworkAfter260205)
            // Position 9-byte Vec3
            // Rotation = r.ReadQuat6();

            if (!isStateFirst)
            {
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

                velocity = r.ReadUInt32();
                angularVelocity = r.ReadUInt32();
            }

            // CSceneVehicleVis_RestoreStaticState
            if (version >= 7)
            {
                speedForward = r.ReadUInt16();
                speedSideward = r.ReadUInt16();
                rpm = r.ReadUInt16();
                flWheelRotation = r.ReadUInt16();
                frWheelRotation = r.ReadUInt16();
                rrWheelRotation = r.ReadUInt16();
                rlWheelRotation = r.ReadUInt16();
                steer = r.ReadByte();
                gas = r.ReadByte();
                brake = r.ReadByte();

                u11 = r.ReadByte();

                if (version >= 8)
                {
                    U12 = r.ReadByte(); // it should be always 0 but sometimes it isnt
                }

                u13 = r.ReadByte();
                u14 = r.ReadByte();
                turboStrength = r.ReadByte();
                steerFront = r.ReadByte();

                flDampenLen = r.ReadByte();
                FLGroundContactMaterial = (CPlugSurface.MaterialId)r.ReadByte();
                // + condition "in water"

                frDampenLen = r.ReadByte();
                FRGroundContactMaterial = (CPlugSurface.MaterialId)r.ReadByte();
                // + condition "in water"

                rrDampenLen = r.ReadByte();
                RRGroundContactMaterial = (CPlugSurface.MaterialId)r.ReadByte();
                // + condition "in water"

                rlDampenLen = r.ReadByte();
                RLGroundContactMaterial = (CPlugSurface.MaterialId)r.ReadByte();
                // + condition "in water"

                if (version >= 8)
                {
                    U25 = r.ReadByte();
                    U26 = r.ReadByte();
                    U27 = r.ReadByte(); // similar to U26, some form of flags

                    if (version >= 9)
                    {
                        dirtBlend = r.ReadByte();

                        if (version >= 10)
                        {
                            var unavailableVal2 = r.ReadUInt32();

                            if (unavailableVal2 != 0)
                            {
                                throw new Exception();
                            }

                            // damage amount
                            u33 = r.ReadByte();
                            u34 = r.ReadByte();
                            u35 = r.ReadByte();

                            if (version >= 11)
                            {
                                if (version >= 14)
                                {
                                    u36 = r.ReadByte();
                                    u37 = r.ReadByte();

                                    if (version >= 15)
                                    {
                                        u38 = r.ReadByte();

                                        if (version >= 16)
                                        {
                                            U39 = r.ReadByte();
                                            u40 = r.ReadByte();
                                        }
                                    }
                                }

                                // The v17+ archive contains only the fixed 47-byte
                                // legacy state here; its U35 count bits have no tail.
                                if (!isStateFirst)
                                {
                                    // count is broken in specific cases like the last sample of a ghost
                                    var count = u35.GetValueOrDefault() >> 2 & 7;

                                    if (version == 11 && count > 4)
                                    {
                                        count = 4;
                                    }

                                    u35_1 = new (Vec3, Quat, byte)[count];

                                    for (var i = 0; i < count; i++)
                                    {
                                        u35_1[i] = (r.ReadVec3(), r.ReadQuat6(), r.ReadByte());
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (isStateFirst)
            {
                Position = r.ReadVec3();
                Rotation = r.ReadQuat6();
                velocity = r.ReadUInt32();
                angularVelocity = r.ReadUInt32();

                u41 = r.ReadByte();

                if (version >= 18)
                {
                    u42 = r.ReadByte();

                    if (version == 19)
                    {
                        u43 = r.ReadByte();
                    }
                    else if (version >= 20)
                    {
                        u44 = r.ReadByte();
                        u45 = r.ReadByte();
                    }
                }
            }

            if (r.BaseStream.Position != r.BaseStream.Length)
            {
                throw new Exception("Not all bytes were read");
            }
        }

        internal override void Write(GbxWriter w, int version)
        {
            if (version < 7)
            {
                throw new VersionNotSupportedException(version);
            }

            var isStateFirst = version >= 17;

            if (version == 13)
            {
                w.WriteVec3_9(Position);
                w.WriteQuat6(Rotation);

                ushort netData = 0;

                if (Brake >= 0.5f) netData |= 1 << 1;

                // FLIsSliding handles standard sliding bit 
                if (FLIsSliding) netData |= 1 << 2;

                var min = 100f;
                var max = 11000f;
                var ratio = min / max;
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                var powRatio = MathF.Pow(ratio, 3.0f);
                var rpmValue = Math.Clamp(RPM, min, max) / max;
#else
                var powRatio = Math.Pow(ratio, 3.0f);
                var rpmValue = AdditionalMath.Clamp(RPM, min, max) / max;
#endif

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                var normalizedValue = (MathF.Pow(rpmValue, 1f / 0.3f) - powRatio) / (1.0f - powRatio);
#else
                var normalizedValue = (Math.Pow(rpmValue, 1f/0.3f) - powRatio) / (1.0f - powRatio);
#endif
                var rpmBits = (ushort)AdditionalMath.Clamp(Math.Round(normalizedValue * 127f), 0, 127);
                netData |= (ushort)(rpmBits << 9);

                w.Write(netData);
                return;
            }

            if (!isStateFirst)
            {
                w.Write(Position);
                w.WriteQuat6(Rotation);

                w.Write(velocity);
                w.Write(angularVelocity);
            }

            if (version >= 7)
            {
                w.Write(speedForward);
                w.Write(speedSideward);
                w.Write(rpm);
                w.Write(flWheelRotation);
                w.Write(frWheelRotation);
                w.Write(rrWheelRotation);
                w.Write(rlWheelRotation);
                w.Write(steer);
                w.Write(gas);
                w.Write(brake);
                w.Write(u11);

                if (version >= 8)
                {
                    w.Write(U12);
                }

                w.Write(u13);
                w.Write(u14);
                w.Write(turboStrength);
                w.Write(steerFront);
                w.Write(flDampenLen);
                w.Write((byte)FLGroundContactMaterial);
                w.Write(frDampenLen);
                w.Write((byte)FRGroundContactMaterial);
                w.Write(rrDampenLen);
                w.Write((byte)RRGroundContactMaterial);
                w.Write(rlDampenLen);
                w.Write((byte)RLGroundContactMaterial);

                if (version >= 8)
                {
                    w.Write(U25);
                    w.Write(U26);
                    w.Write(U27);

                    if (version >= 9)
                    {
                        w.Write(dirtBlend.GetValueOrDefault());

                        if (version >= 10)
                        {
                            w.Write(0u); // unavailableVal2
                            w.Write(u33.GetValueOrDefault());
                            w.Write(u34.GetValueOrDefault());
                            w.Write(u35.GetValueOrDefault());

                            if (version >= 11)
                            {
                                if (version >= 14)
                                {
                                    w.Write(u36.GetValueOrDefault());
                                    w.Write(u37.GetValueOrDefault());

                                    if (version >= 15)
                                    {
                                        w.Write(u38.GetValueOrDefault());

                                        if (version >= 16)
                                        {
                                            w.Write(U39.GetValueOrDefault());
                                            w.Write(u40.GetValueOrDefault());
                                        }
                                    }
                                }

                                if (!isStateFirst)
                                {
                                    var count = u35.GetValueOrDefault() >> 2 & 7;
                                    if (version == 11 && count > 4) count = 4;

                                    if (u35_1 != null)
                                    {
                                        for (var i = 0; i < count && i < u35_1.Length; i++)
                                        {
                                            w.Write(u35_1[i].Item1);
                                            w.WriteQuat6(u35_1[i].Item2);
                                            w.Write(u35_1[i].Item3);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (isStateFirst)
            {
                w.Write(Position);
                w.WriteQuat6(Rotation);
                w.Write(velocity);
                w.Write(angularVelocity);

                w.Write(u41.GetValueOrDefault());

                if (version >= 18)
                {
                    w.Write(u42.GetValueOrDefault());

                    if (version == 19)
                    {
                        w.Write(u43.GetValueOrDefault());
                    }
                    else if (version >= 20)
                    {
                        w.Write(u44.GetValueOrDefault());
                        w.Write(u45.GetValueOrDefault());
                    }
                }
            }
        }

        private static void UpdateVec4Flag(ref byte? flagByte, Vec4? value)
        {
            if (!value.HasValue) flagByte = null;
            else
            {
                byte b = 0;
                b |= (byte)((int)AdditionalMath.Clamp(Math.Round(value.Value.X * 3f), 0, 3));
                b |= (byte)((int)AdditionalMath.Clamp(Math.Round(value.Value.Y * 3f), 0, 3) << 2);
                b |= (byte)((int)AdditionalMath.Clamp(Math.Round(value.Value.Z * 3f), 0, 3) << 4);
                b |= (byte)((int)AdditionalMath.Clamp(Math.Round(value.Value.W * 3f), 0, 3) << 6);
                flagByte = b;
            }
        }

        private static Vec3 ToVec3_4(uint packed)
        {
            // Extract the parts using bitwise masking and shifting
            var mag16 = (short)(packed & 0xFFFF);
            var headingByte = (sbyte)((packed >> 16) & 0xFF);
            var pitchByte = (sbyte)((packed >> 24) & 0xFF);

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
            var mag = mag16 == short.MinValue
                ? 0f
                : MathF.Exp(mag16 / 1000f);

            var axisHeading = headingByte * MathF.PI / sbyte.MaxValue;
            var axisPitch = pitchByte * (MathF.PI / 2f) / sbyte.MaxValue;

            return new Vec3(
                MathF.Cos(axisHeading) * MathF.Cos(axisPitch) * mag,
                MathF.Sin(axisHeading) * MathF.Cos(axisPitch) * mag,
                MathF.Sin(axisPitch) * mag
            );
#else
            var mag = mag16 == short.MinValue
                ? 0f
                : (float)Math.Exp(mag16 / 1000.0);

            var axisHeading = headingByte * Math.PI / sbyte.MaxValue;
            var axisPitch = pitchByte * (Math.PI / 2.0) / sbyte.MaxValue;

            return new Vec3(
                (float)(Math.Cos(axisHeading) * Math.Cos(axisPitch)) * mag, 
                (float)(Math.Sin(axisHeading) * Math.Cos(axisPitch)) * mag, 
                (float)Math.Sin(axisPitch) * mag
            );
#endif
        }

        private static uint FromVec3_4(Vec3 vec)
        {
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
            var mag = MathF.Sqrt(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);
#else
            var mag = (float)Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);
#endif

            short mag16;
            sbyte headingByte = 0;
            sbyte pitchByte = 0;

            if (mag < 1e-6f) // Handle zero/near-zero magnitude
            {
                mag16 = short.MinValue;
            }
            else
            {
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
                // Inverse of Exp is Log
                var logMag = MathF.Log(mag) * 1000f;
                mag16 = (short)Math.Clamp(MathF.Round(logMag), short.MinValue + 1, short.MaxValue);

                // Inverse trigonometric functions to find angles
                var axisPitch = MathF.Asin(vec.Z / mag);
                var axisHeading = MathF.Atan2(vec.Y, vec.X);

                pitchByte = (sbyte)Math.Clamp(MathF.Round(axisPitch * sbyte.MaxValue / (MathF.PI / 2f)), sbyte.MinValue, sbyte.MaxValue);
                headingByte = (sbyte)Math.Clamp(MathF.Round(axisHeading * sbyte.MaxValue / MathF.PI), sbyte.MinValue, sbyte.MaxValue);
#else
                var logMag = Math.Log(mag) * 1000.0;
                mag16 = (short)Math.Max(short.MinValue + 1, Math.Min(short.MaxValue, Math.Round(logMag)));

                var axisPitch = Math.Asin(vec.Z / mag);
                var axisHeading = Math.Atan2(vec.Y, vec.X);

                pitchByte = (sbyte)Math.Max(sbyte.MinValue, Math.Min(sbyte.MaxValue, Math.Round(axisPitch * sbyte.MaxValue / (Math.PI / 2.0))));
                headingByte = (sbyte)Math.Max(sbyte.MinValue, Math.Min(sbyte.MaxValue, Math.Round(axisHeading * sbyte.MaxValue / Math.PI)));
#endif
            }

            // Re-pack into a 32-bit unsigned integer
            uint packed = (ushort)mag16;
            packed |= (uint)unchecked((byte)headingByte) << 16;
            packed |= (uint)unchecked((byte)pitchByte) << 24;

            return packed;
        }
    }
}
