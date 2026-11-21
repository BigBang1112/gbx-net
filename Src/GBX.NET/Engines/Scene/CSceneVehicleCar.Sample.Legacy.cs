namespace GBX.NET.Engines.Scene;

public partial class CSceneVehicleCar
{
    public sealed partial class Sample
    {
        private Vec3? legacyVelocity;
        private Vec3? legacyAngularVelocity;

        private void ReadLegacy(GbxReader r)
        {
            // CHmsDynaReplayItem::OldRestoreDynaItemState: the flags are inline for v2.
            var flags = r.ReadByte();
            var compactMotion = (flags & 0x10) != 0;
            Position = compactMotion ? r.ReadVec3() : r.ReadVec3_9();

            if (compactMotion)
            {
                var angle = r.ReadByte() * (float)Math.PI / 255;
                var heading = r.ReadSByte() * Math.PI / 127;
                var pitch = r.ReadSByte() * (Math.PI / 2) / 127;
                var axis = new Vec3((float)(Math.Cos(heading) * Math.Cos(pitch)),
                    (float)(Math.Sin(heading) * Math.Cos(pitch)), (float)Math.Sin(pitch));
                Rotation = new Quat(axis * (float)Math.Sin(angle), (float)Math.Cos(angle));
            }
            else
            {
                Rotation = r.ReadQuat6();
            }

            velocity = angularVelocity = 0x8000;
            legacyVelocity = legacyAngularVelocity = null;
            if ((flags & 0x20) != 0)
            {
                if ((flags & 0x80) != 0 || (flags & 0xF) == 0)
                {
                    if (compactMotion)
                    {
                        velocity = r.ReadUInt32();
                    }
                    else
                    {
                        legacyVelocity = r.ReadVec3();
                        velocity = FromVec3_4(legacyVelocity.Value);
                    }
                }
                if ((flags & 0x40) != 0)
                {
                    if (compactMotion)
                    {
                        angularVelocity = r.ReadUInt32();
                    }
                    else
                    {
                        legacyAngularVelocity = r.ReadVec3();
                        angularVelocity = FromVec3_4(legacyAngularVelocity.Value);
                    }
                }
            }

            // SVehicleSimpleState_ReplayAfter211003 is 22 bytes.
            speedForward = r.ReadUInt16();
            speedSideward = r.ReadUInt16();
            rpm = r.ReadUInt16();
            turboStrength = r.ReadByte();
            steerFront = r.ReadByte();
            flDampenLen = r.ReadByte();
            FLWheelRotation = r.ReadByte() / 255f * (float)Math.PI * 2;
            FLGroundContactMaterial = (CPlugSurface.MaterialId)r.ReadByte();
            frDampenLen = r.ReadByte();
            FRWheelRotation = r.ReadByte() / 255f * (float)Math.PI * 2;
            FRGroundContactMaterial = (CPlugSurface.MaterialId)r.ReadByte();
            rrDampenLen = r.ReadByte();
            RRWheelRotation = r.ReadByte() / 255f * (float)Math.PI * 2;
            RRGroundContactMaterial = (CPlugSurface.MaterialId)r.ReadByte();
            rlDampenLen = r.ReadByte();
            RLWheelRotation = r.ReadByte() / 255f * (float)Math.PI * 2;
            RLGroundContactMaterial = (CPlugSurface.MaterialId)r.ReadByte();

            var stateFlags = r.ReadUInt16();
            U25 = (byte)((U25 & 7) | ((stateFlags >> 6) & 0x78) | ((stateFlags & 1) << 7));
            U26 = (byte)((U26 & 0x3F) | ((stateFlags & 6) << 5));
            U27 = (byte)((U27 & 0xC0) | ((stateFlags >> 3) & 0x3F));
        }

        private void WriteLegacy(GbxWriter w)
        {
            var flags = Data.Length > 0 ? Data[0] : (byte)0x70;
            var compactMotion = (flags & 0x10) != 0;
            w.Write(flags);
            if (compactMotion)
            {
                w.Write(Position);
                var angle = Math.Acos(AdditionalMath.Clamp(Rotation.W, -1, 1));
                var sinAngle = Math.Sin(angle);
                var axis = Math.Abs(sinAngle) > 1e-5
                    ? new Vec3((float)(Rotation.X / sinAngle), (float)(Rotation.Y / sinAngle), (float)(Rotation.Z / sinAngle))
                    : new Vec3(1, 0, 0);
                var angleByte = (byte)AdditionalMath.Clamp(Math.Round(angle / Math.PI * 255), 0, 255);
                w.Write(angleByte);
                if (angleByte is 0 or 255 && Data.Length >= 16 && Data[13] == angleByte)
                {
                    // At these angles the axis is undefined; keep the original representation.
                    w.Write(Data[14]);
                    w.Write(Data[15]);
                }
                else
                {
                    w.Write((sbyte)AdditionalMath.Clamp(Math.Round(Math.Atan2(axis.Y, axis.X) / Math.PI * 127), -128, 127));
                    w.Write((sbyte)AdditionalMath.Clamp(Math.Round(Math.Asin(AdditionalMath.Clamp(axis.Z, -1, 1)) / (Math.PI / 2) * 127), -128, 127));
                }
            }
            else
            {
                w.WriteVec3_9(Position);
                w.WriteQuat6(Rotation);
            }

            if ((flags & 0x20) != 0)
            {
                if ((flags & 0x80) != 0 || (flags & 0xF) == 0)
                {
                    if (compactMotion) w.Write(velocity);
                    else w.Write(Velocity);
                }
                if ((flags & 0x40) != 0)
                {
                    if (compactMotion) w.Write(angularVelocity);
                    else w.Write(AngularVelocity);
                }
            }

            w.Write(speedForward);
            w.Write(speedSideward);
            w.Write(rpm);
            w.Write(turboStrength);
            w.Write(steerFront);
            w.Write(flDampenLen);
            w.Write(PackLegacyWheelRotation(FLWheelRotation));
            w.Write((byte)FLGroundContactMaterial);
            w.Write(frDampenLen);
            w.Write(PackLegacyWheelRotation(FRWheelRotation));
            w.Write((byte)FRGroundContactMaterial);
            w.Write(rrDampenLen);
            w.Write(PackLegacyWheelRotation(RRWheelRotation));
            w.Write((byte)RRGroundContactMaterial);
            w.Write(rlDampenLen);
            w.Write(PackLegacyWheelRotation(RLWheelRotation));
            w.Write((byte)RLGroundContactMaterial);

            var stateFlags = Data.Length >= 22 ? (Data[Data.Length - 2] | Data[Data.Length - 1] << 8) & 0xE000 : 0;
            stateFlags |= (U25 >> 7) | ((U26 >> 5) & 6) | ((U27 & 0x3F) << 3) | ((U25 & 0x78) << 6);
            w.Write((ushort)stateFlags);
        }

        private static byte PackLegacyWheelRotation(float value)
            => (byte)AdditionalMath.Clamp(Math.Round(value / (Math.PI * 2) * 255), 0, 255);
    }
}
