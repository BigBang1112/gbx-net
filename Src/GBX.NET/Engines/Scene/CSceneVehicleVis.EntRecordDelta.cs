using System.Buffers.Binary;

namespace GBX.NET.Engines.Scene;

public partial class CSceneVehicleVis
{
    public class EntRecordDelta : CPlugEntRecordData.EntRecordDelta, IEntRecordDeltaRawData
    {
        private const float Tau = (float)Math.PI * 2;

        private const byte MaskFR = 0x1;
        private const byte MaskRR = 0x4;
        private const byte MaskRL = 0x10;
        private const byte MaskFL = 0x40;

        private const byte MaskIsGroundMode = 0x1;
        private const byte MaskIsReactorGroundMode = 0x4;
        private const byte MaskIsReactorUp = 0x8;
        private const byte MaskIsReactorDown = 0x10;
        private const byte MaskReactorLvl1 = 0x20;
        private const byte MaskReactorLvl2 = 0x40;

        private const byte MaskIsTurbo = 0x82;
        private const byte MaskIsTopContact = 0x20;

        private const byte MaskPedalNone = 0x10;
        private const byte MaskPedalAccel = 0x20;
        private const byte MaskSteerNone = 0x40;
        private const byte MaskSteerLeft = 0x80;

        private ushort sideSpeed;
        private byte rpm;
        private byte flWheelRot;
        private byte flWheelRotCount;
        private byte frWheelRot;
        private byte frWheelRotCount;
        private byte rrWheelRot;
        private byte rrWheelRotCount;
        private byte rlWheelRot;
        private byte rlWheelRotCount;
        private byte steer;
        private byte u15;
        private byte brake;
        private byte turboTime;
        private byte flDampenLen;
        private byte frDampenLen;
        private byte rrDampenLen;
        private byte rlDampenLen;
        private byte isTurbo;
        private byte slipCoef1;
        private byte slipCoef2;

        private Vec3 position;
        private Quat rotation;
        private float speed;
        private Vec3 velocity;

        private byte vehicleState;
        private byte flIce;
        private byte frIce;
        private byte rrIce;
        private byte rlIce;
        private byte groundMode;
        private byte boosterAirControl;
        private byte gear;
        private byte flDirt;
        private byte frDirt;
        private byte rrDirt;
        private byte rlDirt;
        private byte wetnessValue;
        private byte simulationTimeCoef;

        ushort IEntRecordDeltaRawData.SideSpeed { get => sideSpeed; set => sideSpeed = value; }
        byte IEntRecordDeltaRawData.Rpm { get => rpm; set => rpm = value; }
        byte IEntRecordDeltaRawData.FlWheelRot { get => flWheelRot; set => flWheelRot = value; }
        byte IEntRecordDeltaRawData.FlWheelRotCount { get => flWheelRotCount; set => flWheelRotCount = value; }
        byte IEntRecordDeltaRawData.FrWheelRot { get => frWheelRot; set => frWheelRot = value; }
        byte IEntRecordDeltaRawData.FrWheelRotCount { get => frWheelRotCount; set => frWheelRotCount = value; }
        byte IEntRecordDeltaRawData.RrWheelRot { get => rrWheelRot; set => rrWheelRot = value; }
        byte IEntRecordDeltaRawData.RrWheelRotCount { get => rrWheelRotCount; set => rrWheelRotCount = value; }
        byte IEntRecordDeltaRawData.RlWheelRot { get => rlWheelRot; set => rlWheelRot = value; }
        byte IEntRecordDeltaRawData.RlWheelRotCount { get => rlWheelRotCount; set => rlWheelRotCount = value; }
        byte IEntRecordDeltaRawData.Steer { get => steer; set => steer = value; }
        byte IEntRecordDeltaRawData.U15 { get => u15; set => u15 = value; }
        byte IEntRecordDeltaRawData.Brake { get => brake; set => brake = value; }
        byte IEntRecordDeltaRawData.TurboTime { get => turboTime; set => turboTime = value; }
        byte IEntRecordDeltaRawData.FlDampenLen { get => flDampenLen; set => flDampenLen = value; }
        byte IEntRecordDeltaRawData.FrDampenLen { get => frDampenLen; set => frDampenLen = value; }
        byte IEntRecordDeltaRawData.RrDampenLen { get => rrDampenLen; set => rrDampenLen = value; }
        byte IEntRecordDeltaRawData.RlDampenLen { get => rlDampenLen; set => rlDampenLen = value; }
        byte IEntRecordDeltaRawData.IsTurbo { get => isTurbo; set => isTurbo = value; }
        byte IEntRecordDeltaRawData.SlipCoef1 { get => slipCoef1; set => slipCoef1 = value; }
        byte IEntRecordDeltaRawData.SlipCoef2 { get => slipCoef2; set => slipCoef2 = value; }
        byte IEntRecordDeltaRawData.VehicleState { get => vehicleState; set => vehicleState = value; }
        byte IEntRecordDeltaRawData.FlIce { get => flIce; set => flIce = value; }
        byte IEntRecordDeltaRawData.FrIce { get => frIce; set => frIce = value; }
        byte IEntRecordDeltaRawData.RrIce { get => rrIce; set => rrIce = value; }
        byte IEntRecordDeltaRawData.RlIce { get => rlIce; set => rlIce = value; }
        byte IEntRecordDeltaRawData.GroundMode { get => groundMode; set => groundMode = value; }
        byte IEntRecordDeltaRawData.BoosterAirControl { get => boosterAirControl; set => boosterAirControl = value; }
        byte IEntRecordDeltaRawData.Gear { get => gear; set => gear = value; }
        byte IEntRecordDeltaRawData.FlDirt { get => flDirt; set => flDirt = value; }
        byte IEntRecordDeltaRawData.FrDirt { get => frDirt; set => frDirt = value; }
        byte IEntRecordDeltaRawData.RrDirt { get => rrDirt; set => rrDirt = value; }
        byte IEntRecordDeltaRawData.RlDirt { get => rlDirt; set => rlDirt = value; }
        byte IEntRecordDeltaRawData.WetnessValue { get => wetnessValue; set => wetnessValue = value; }
        byte IEntRecordDeltaRawData.SimulationTimeCoef { get => simulationTimeCoef; set => simulationTimeCoef = value; }

        public Vec3 Position { get => position; set => position = value; }
        public Quat Rotation { get => rotation; set => rotation = value; }
        public Vec3 PitchYawRoll => Rotation.ToPitchYawRoll();
        public float Speed { get => speed * 3.6f; set => speed = value / 3.6f; }
        public Vec3 Velocity { get => velocity; set => velocity = value; }

        public float Gear
        {
            get => (gear - 1) / 4f;
            set => gear = (byte)AdditionalMath.Clamp(Math.Round(value * 4f + 1), 0, 255);
        }

        public byte RPM
        {
            get => rpm;
            set => rpm = value;
        }

        public float Steer
        {
            get => ((steer / 255f) - 0.5f) * 2f;
            set => steer = (byte)AdditionalMath.Clamp(Math.Round((value / 2f + 0.5f) * 255f), 0, 255);
        }

        public float Brake
        {
            get => brake / 255f;
            set => brake = (byte)AdditionalMath.Clamp(Math.Round(value * 255f), 0, 255);
        }

        public float Gas
        {
            get => (u15 / 255f) + (brake / 255f);
            set => u15 = (byte)AdditionalMath.Clamp(Math.Round(value * 255f - brake), 0, 255);
        }

        public float FLIcing
        {
            get => flIce / 255f;
            set => flIce = (byte)AdditionalMath.Clamp(Math.Round(value * 255f), 0, 255);
        }

        public float FRIcing
        {
            get => frIce / 255f;
            set => frIce = (byte)AdditionalMath.Clamp(Math.Round(value * 255f), 0, 255);
        }

        public float RLIcing
        {
            get => rlIce / 255f;
            set => rlIce = (byte)AdditionalMath.Clamp(Math.Round(value * 255f), 0, 255);
        }

        public float RRIcing
        {
            get => rrIce / 255f;
            set => rrIce = (byte)AdditionalMath.Clamp(Math.Round(value * 255f), 0, 255);
        }

        public float FLDirt
        {
            get => flDirt / 255f;
            set => flDirt = (byte)AdditionalMath.Clamp(Math.Round(value * 255f), 0, 255);
        }

        public float FRDirt
        {
            get => frDirt / 255f;
            set => frDirt = (byte)AdditionalMath.Clamp(Math.Round(value * 255f), 0, 255);
        }

        public float RLDirt
        {
            get => rlDirt / 255f;
            set => rlDirt = (byte)AdditionalMath.Clamp(Math.Round(value * 255f), 0, 255);
        }

        public float RRDirt
        {
            get => rrDirt / 255f;
            set => rrDirt = (byte)AdditionalMath.Clamp(Math.Round(value * 255f), 0, 255);
        }

        public CPlugSurface.MaterialId FLGroundContactMaterial { get; set; }
        public CPlugSurface.MaterialId FRGroundContactMaterial { get; set; }
        public CPlugSurface.MaterialId RLGroundContactMaterial { get; set; }
        public CPlugSurface.MaterialId RRGroundContactMaterial { get; set; }

        // Multiply by 4 instead of 2 as it matches value given by openplanet CSceneVehicleVisState
        public float FLDampenLen
        {
            get => ((flDampenLen / 255f) - 0.5f) * 4f;
            set => flDampenLen = (byte)AdditionalMath.Clamp(Math.Round((value / 4f + 0.5f) * 255f), 0, 255);
        }
        public float FRDampenLen
        {
            get => ((frDampenLen / 255f) - 0.5f) * 4f;
            set => frDampenLen = (byte)AdditionalMath.Clamp(Math.Round((value / 4f + 0.5f) * 255f), 0, 255);
        }
        public float RLDampenLen
        {
            get => ((rlDampenLen / 255f) - 0.5f) * 4f;
            set => rlDampenLen = (byte)AdditionalMath.Clamp(Math.Round((value / 4f + 0.5f) * 255f), 0, 255);
        }
        public float RRDampenLen
        {
            get => ((rrDampenLen / 255f) - 0.5f) * 4f;
            set => rrDampenLen = (byte)AdditionalMath.Clamp(Math.Round((value / 4f + 0.5f) * 255f), 0, 255);
        }

        // Nadeo uses two bytes. FLSlip is at the 7th bit of SlipCoefByte1.
        public bool FLSlipCoef
        {
            get => (slipCoef1 & MaskFL) != 0;
            set { if (value) slipCoef1 |= MaskFL; else slipCoef1 &= 255 - MaskFL; }
        }
        public bool FRSlipCoef
        {
            get => (slipCoef2 & MaskFR) != 0;
            set { if (value) slipCoef2 |= MaskFR; else slipCoef2 &= 255 - MaskFR; }
        }
        public bool RLSlipCoef
        {
            get => (slipCoef2 & MaskRL) != 0;
            set { if (value) slipCoef2 |= MaskRL; else slipCoef2 &= 255 - MaskRL; }
        }
        public bool RRSlipCoef
        {
            get => (slipCoef2 & MaskRR) != 0;
            set { if (value) slipCoef2 |= MaskRR; else slipCoef2 &= 255 - MaskRR; }
        }

        public float FLWheelRot
        {
            get => (flWheelRot / 255f * Tau) + (flWheelRotCount * Tau);
            set
            {
                var rotations = value / Tau;
                flWheelRotCount = (byte)Math.Floor(rotations);
                flWheelRot = (byte)AdditionalMath.Clamp(Math.Round((rotations - flWheelRotCount) * 255f), 0, 255);
            }
        }
        public float FRWheelRot
        {
            get => (frWheelRot / 255f * Tau) + (frWheelRotCount * Tau);
            set
            {
                var rotations = value / Tau;
                frWheelRotCount = (byte)Math.Floor(rotations);
                frWheelRot = (byte)AdditionalMath.Clamp(Math.Round((rotations - frWheelRotCount) * 255f), 0, 255);
            }
        }
        public float RLWheelRot
        {
            get => (rlWheelRot / 255f * Tau) + (rlWheelRotCount * Tau);
            set
            {
                var rotations = value / Tau;
                rlWheelRotCount = (byte)Math.Floor(rotations);
                rlWheelRot = (byte)AdditionalMath.Clamp(Math.Round((rotations - rlWheelRotCount) * 255f), 0, 255);
            }
        }
        public float RRWheelRot
        {
            get => (rrWheelRot / 255f * Tau) + (rrWheelRotCount * Tau);
            set
            {
                var rotations = value / Tau;
                rrWheelRotCount = (byte)Math.Floor(rotations);
                rrWheelRot = (byte)AdditionalMath.Clamp(Math.Round((rotations - rrWheelRotCount) * 255f), 0, 255);
            }
        }

        public float WetnessValue
        {
            get => wetnessValue / 255f;
            set => wetnessValue = (byte)AdditionalMath.Clamp(Math.Round(value * 255f), 0, 255);
        }

        public bool IsGroundContact
        {
            get => (groundMode & MaskIsGroundMode) != 0;
            set { if (value) groundMode |= MaskIsGroundMode; else groundMode &= 255 - MaskIsGroundMode; }
        }

        public bool IsReactorGroundMode
        {
            get => (groundMode & MaskIsReactorGroundMode) != 0;
            set { if (value) groundMode |= MaskIsReactorGroundMode; else groundMode &= 255 - MaskIsReactorGroundMode; }
        }

        public ReactorBoostLvl ReactorBoostLvl
        {
            get
            {
                if ((groundMode & MaskReactorLvl1) != 0) return ReactorBoostLvl.Lvl1;
                if ((groundMode & MaskReactorLvl2) != 0) return ReactorBoostLvl.Lvl2;
                return ReactorBoostLvl.None;
            }
            set
            {
                // Clear both bits first
                groundMode &= 255 - (MaskReactorLvl1 | MaskReactorLvl2);

                if (value == ReactorBoostLvl.Lvl1) groundMode |= MaskReactorLvl1;
                else if (value == ReactorBoostLvl.Lvl2) groundMode |= MaskReactorLvl2;
            }
        }

        public ReactorBoostType ReactorBoostType
        {
            get
            {
                var isUp = (groundMode & MaskIsReactorUp) != 0;
                var isDown = (groundMode & MaskIsReactorDown) != 0;

                if (isUp && isDown) return ReactorBoostType.UpAndDown;
                if (isUp) return ReactorBoostType.Up;
                if (isDown) return ReactorBoostType.Down;
                return ReactorBoostType.None;
            }
            set
            {
                groundMode &= 255 - (MaskIsReactorUp | MaskIsReactorDown);

                if (value == ReactorBoostType.Up || value == ReactorBoostType.UpAndDown)
                    groundMode |= MaskIsReactorUp;

                if (value == ReactorBoostType.Down || value == ReactorBoostType.UpAndDown)
                    groundMode |= MaskIsReactorDown;
            }
        }

        // [1,0,-1] = (Left,None,Right)
        public int ReactorAirControlSteer
        {
            get
            {
                var isLeft = (boosterAirControl & MaskSteerLeft) != 0;
                var isNone = (boosterAirControl & MaskSteerNone) != 0;
                return isLeft ? 1 : isNone ? 0 : -1;
            }
            set
            {
                boosterAirControl &= 255 - (MaskSteerLeft | MaskSteerNone);
                if (value == 1) boosterAirControl |= MaskSteerLeft;
                else if (value == 0) boosterAirControl |= MaskSteerNone;
            }
        }

        // [1,0,-1] = (Accell,None,Brake)
        public int ReactorAirControlPedal
        {
            get
            {
                var isAccel = (boosterAirControl & MaskPedalAccel) != 0;
                var isNone = (boosterAirControl & MaskPedalNone) != 0;
                return isAccel ? 1 : isNone ? 0 : -1;
            }
            set
            {
                boosterAirControl &= (byte)(255 - (MaskPedalAccel | MaskPedalNone));
                if (value == 1) boosterAirControl |= MaskPedalAccel;
                else if (value == 0) boosterAirControl |= MaskPedalNone;
            }
        }

        public bool IsTurbo
        {
            get => (isTurbo & MaskIsTurbo) != 0;
            set { if (value) isTurbo |= MaskIsTurbo; else isTurbo &= (byte)(255 - MaskIsTurbo); }
        }
        
        public float TurboTime
        {
            get => turboTime / 255f;
            set => turboTime = (byte)AdditionalMath.Clamp(Math.Round(value * 255f), 0, 255);
        }

        public float SimulationTimeCoef
        {
            get => simulationTimeCoef / 255f;
            set => simulationTimeCoef = (byte)AdditionalMath.Clamp(Math.Round(value * 255f), 0, 255);
        }

        public float SideSpeed
        {
            get => (float)(((sideSpeed / 65536.0) - 0.5) * 2000.0);
            set => sideSpeed = (ushort)AdditionalMath.Clamp(Math.Round((value / 2000.0 + 0.5) * 65536.0), 0, 65535);
        }

        public bool IsTopContact
        {
            get => (vehicleState & MaskIsTopContact) != 0;
            set { if (value) vehicleState |= MaskIsTopContact; else vehicleState &= 255 - MaskIsTopContact; }
        }

        internal EntRecordDelta(TimeInt32 time, byte[] data) : base(time, data) { }

        internal override void Read(MemoryStream ms)
        {
            using var r = new GbxReader(ms);

            ms.Position = 2;
            sideSpeed = r.ReadUInt16();

            ms.Position = 5;
            rpm = r.ReadByte();

            flWheelRot = r.ReadByte();
            flWheelRotCount = r.ReadByte();
            frWheelRot = r.ReadByte();
            frWheelRotCount = r.ReadByte();
            rrWheelRot = r.ReadByte();
            rrWheelRotCount = r.ReadByte();
            rlWheelRot = r.ReadByte();
            rlWheelRotCount = r.ReadByte();

            ms.Position = 14;
            steer = r.ReadByte();
            u15 = r.ReadByte(); // Position 15

            ms.Position = 18;
            brake = r.ReadByte();

            ms.Position = 21;
            turboTime = r.ReadByte();

            ms.Position = 23;
            flDampenLen = r.ReadByte();
            FLGroundContactMaterial = (CPlugSurface.MaterialId)r.ReadByte();
            frDampenLen = r.ReadByte();
            FRGroundContactMaterial = (CPlugSurface.MaterialId)r.ReadByte();
            rrDampenLen = r.ReadByte();
            RRGroundContactMaterial = (CPlugSurface.MaterialId)r.ReadByte();
            rlDampenLen = r.ReadByte();
            RLGroundContactMaterial = (CPlugSurface.MaterialId)r.ReadByte();

            ms.Position = 31;
            isTurbo = r.ReadByte();
            slipCoef1 = r.ReadByte(); // Position 32
            slipCoef2 = r.ReadByte(); // Position 33

            ms.Position = 47;
            var (position, rotation, speed, velocity) = r.ReadTransform();
            this.position = position;
            this.rotation = rotation;
            this.speed = speed;
            this.velocity = velocity;

            ms.Position = 76;
            vehicleState = r.ReadByte();

            ms.Position = 81;
            flIce = r.ReadByte();
            frIce = r.ReadByte();
            rrIce = r.ReadByte();
            rlIce = r.ReadByte();

            ms.Position = 89;
            groundMode = r.ReadByte();
            boosterAirControl = r.ReadByte(); // Position 90
            gear = r.ReadByte(); // Position 91

            ms.Position = 93;
            flDirt = r.ReadByte();

            ms.Position = 95;
            frDirt = r.ReadByte();

            ms.Position = 97;
            rrDirt = r.ReadByte();

            ms.Position = 99;
            rlDirt = r.ReadByte();

            ms.Position = 101;
            wetnessValue = r.ReadByte();
            simulationTimeCoef = r.ReadByte(); // Position 102
        }

        internal override void Write()
        {
            BinaryPrimitives.WriteUInt16LittleEndian(Data.AsSpan(2), sideSpeed);

            Data[5] = rpm;

            Data[6] = flWheelRot;
            Data[7] = flWheelRotCount;
            Data[8] = frWheelRot;
            Data[9] = frWheelRotCount;
            Data[10] = rrWheelRot;
            Data[11] = rrWheelRotCount;
            Data[12] = rlWheelRot;
            Data[13] = rlWheelRotCount;

            Data[14] = steer;
            Data[15] = u15;

            Data[18] = brake;

            Data[21] = turboTime;

            Data[23] = flDampenLen;
            Data[24] = (byte)FLGroundContactMaterial;
            Data[25] = frDampenLen;
            Data[26] = (byte)FRGroundContactMaterial;
            Data[27] = rrDampenLen;
            Data[28] = (byte)RRGroundContactMaterial;
            Data[29] = rlDampenLen;
            Data[30] = (byte)RLGroundContactMaterial;

            Data[31] = isTurbo;
            Data[32] = slipCoef1;
            Data[33] = slipCoef2;

            // Wrap the Data array in a MemoryStream specifically
            // This edits the underlying Data array directly in memory without copying
            using (var ms = new MemoryStream(Data))
            using (var w = new GbxWriter(ms))
            {
                ms.Position = 47;
                w.WriteTransform(position, rotation, speed, velocity);
            }

            Data[76] = vehicleState;

            Data[81] = flIce;
            Data[82] = frIce;
            Data[83] = rrIce;
            Data[84] = rlIce;

            Data[89] = groundMode;
            Data[90] = boosterAirControl;
            Data[91] = gear;

            Data[93] = flDirt;
            Data[95] = frDirt;
            Data[97] = rrDirt;
            Data[99] = rlDirt;

            Data[101] = wetnessValue;
            Data[102] = simulationTimeCoef;
        }
    }
}