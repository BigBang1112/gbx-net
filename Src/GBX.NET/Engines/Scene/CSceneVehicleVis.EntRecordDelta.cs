namespace GBX.NET.Engines.Scene;

public partial class CSceneVehicleVis
{
    public class EntRecordDelta : CPlugEntRecordData.EntRecordDelta
    {
        public Vec3 Position { get; set; }
        public Quat Rotation { get; set; }
        public Vec3 PitchYawRoll => Rotation.ToPitchYawRoll();
        public float Speed { get; set; }
        public Vec3 Velocity { get; set; }
        public float? Gear { get; set; }
        public byte? RPM { get; set; }
        public float? Steer { get; set; }
        public float? Brake { get; set; }
        public float? Gas { get; set; }

        public float FLIcing { get; set; }
        public float FRIcing { get; set; }
        public float RLIcing { get; set; }
        public float RRIcing { get; set; }

        public float FLDirt { get; set; }
        public float FRDirt { get; set; }
        public float RLDirt { get; set; }
        public float RRDirt { get; set; }

        public CPlugSurface.MaterialId FLGroundContactMaterial { get; set; }
        public CPlugSurface.MaterialId FRGroundContactMaterial { get; set; }
        public CPlugSurface.MaterialId RLGroundContactMaterial { get; set; }
        public CPlugSurface.MaterialId RRGroundContactMaterial { get; set; }

        public float FLDampenLen { get; set; }
        public float FRDampenLen { get; set; }
        public float RLDampenLen { get; set; }
        public float RRDampenLen { get; set; }

        public bool FLSlipCoef { get; set; }
        public bool FRSlipCoef { get; set; }
        public bool RLSlipCoef { get; set; }
        public bool RRSlipCoef { get; set; }

        public float FLWheelRot { get; set; }
        public float FRWheelRot { get; set; }
        public float RLWheelRot { get; set; }
        public float RRWheelRot { get; set; }

        public float WetnessValue { get; set; }
        public bool IsGroundContact { get; set; }
        public bool IsReactorGroundMode { get; set; }

        public ReactorBoostLvl ReactorBoostLvl { get; set; }
        public ReactorBoostType ReactorBoostType { get; set; }

        public int ReactorAirControlSteer { get; set; }
        public int ReactorAirControlPedal { get; set; }

        public bool IsTurbo { get; set; }
        public float TurboTime { get; set; }

        public float SimulationTimeCoef { get; set; }

        public float SideSpeed { get; set; }
        public bool IsTopContact { get; set; }
        
        internal EntRecordDelta(TimeInt32 time, byte[] data) : base(time, data)
        {
        }

        public override void Read(MemoryStream ms, GameBoxReader r)
        {
            ms.Position = 5;
            var rpmByte = r.ReadByte();

            ms.Position = 14;
            var steerByte = r.ReadByte();
            var steer = ((steerByte / 255f) - 0.5f) * 2;

            ms.Position = 91;
            var gearByte = r.ReadByte();
            var gear = gearByte / 5f;

            Gear = gear;
            RPM = rpmByte;
            Steer = steer;

            ms.Position = 15;
            var u15 = r.ReadByte();

            ms.Position = 18;
            var brakeByte = r.ReadByte();
            var brake = brakeByte / 255f;
            var gas = u15 / 255f + brake;

            Brake = brake;
            Gas = gas;

            ms.Position = 47;

            var (position, rotation, speed, velocity) = r.ReadTransform();

            Position = position;
            Rotation = rotation;
            Speed = speed * 3.6f;
            Velocity = velocity;

            // ICE
            ms.Position = 81;
            var FLIceByte = r.ReadByte();
            ms.Position = 82;
            var FRIceByte = r.ReadByte();
            ms.Position = 83;
            var RRIceByte = r.ReadByte();
            ms.Position = 84;
            var RLIceByte = r.ReadByte();

            FLIcing = FLIceByte / 255f;
            FRIcing = FRIceByte / 255f;
            RRIcing = RRIceByte / 255f;
            RLIcing = RLIceByte / 255f;

            // DIRT
            ms.Position = 93;
            var FLDirtByte = r.ReadByte();
            ms.Position = 95;
            var FRDirtByte = r.ReadByte();
            ms.Position = 97;
            var RRDirtByte = r.ReadByte();
            ms.Position = 99;
            var RLDirtByte = r.ReadByte();

            FLDirt = FLDirtByte / 255f;
            FRDirt = FRDirtByte / 255f;
            RRDirt = RRDirtByte / 255f;
            RLDirt = RLDirtByte / 255f;

            // GroundContactMaterial
            ms.Position = 24;
            var FLGroundContactMaterialByte = r.ReadByte();
            ms.Position = 26;
            var FRGroundContactMaterialByte = r.ReadByte();
            ms.Position = 28;
            var RRGroundContactMaterialByte = r.ReadByte();
            ms.Position = 30;
            var RLGroundContactMaterialByte = r.ReadByte();

            FLGroundContactMaterial = (CPlugSurface.MaterialId)FLGroundContactMaterialByte;
            FRGroundContactMaterial = (CPlugSurface.MaterialId)FRGroundContactMaterialByte;
            RRGroundContactMaterial = (CPlugSurface.MaterialId)RRGroundContactMaterialByte;
            RLGroundContactMaterial = (CPlugSurface.MaterialId)RLGroundContactMaterialByte;

            // DampenLen
            ms.Position = 23;
            var FLDampenLenByte = r.ReadByte();
            ms.Position = 25;
            var FRDampenLenByte = r.ReadByte();
            ms.Position = 27;
            var RRDampenLenByte = r.ReadByte();
            ms.Position = 29;
            var RLDampenLenByte = r.ReadByte();

            // Multiply by 4 instead of 2 as it matches value given by openplanet CSceneVehicleVisState
            FLDampenLen = ((FLDampenLenByte / 255f) - 0.5f) * 4;
            FRDampenLen = ((FRDampenLenByte / 255f) - 0.5f) * 4;
            RRDampenLen = ((RRDampenLenByte / 255f) - 0.5f) * 4;
            RLDampenLen = ((RLDampenLenByte / 255f) - 0.5f) * 4;

            // SlipCoef
            ms.Position = 32;
            var SlipCoefByte1 = r.ReadByte();
            ms.Position = 33;
            var SlipCoefByte2 = r.ReadByte();

            byte maskFR = 0x1;  // 00000001
            byte maskRR = 0x4;  // 00000100
            byte maskRL = 0x10; // 00010000
            byte maskFL = 0x40; // 01000000

            // Nadeo uses two bytes for some reason
            // FLSlip is unique in that it is located at the 7th bit of SlipCoefByte1.
            // The 7th bit of SlipCoefByte2 is used too, however it appears to not be correlated with anything.
            FLSlipCoef = (SlipCoefByte1 & maskFL) != 0;
            FRSlipCoef = (SlipCoefByte2 & maskFR) != 0;
            RRSlipCoef = (SlipCoefByte2 & maskRR) != 0;
            RLSlipCoef = (SlipCoefByte2 & maskRL) != 0;

            // WheelRotation
            ms.Position = 6;
            var FLWheelRotationByte = r.ReadByte();
            ms.Position = 7;
            var FLWheelRotationCountByte = r.ReadByte();
            ms.Position = 8;
            var FRWheelRotationByte = r.ReadByte();
            ms.Position = 9;
            var FRWheelRotationCountByte = r.ReadByte();
            ms.Position = 10;
            var RRWheelRotationByte = r.ReadByte();
            ms.Position = 11;
            var RRWheelRotationCountByte = r.ReadByte();
            ms.Position = 12;
            var RLWheelRotationByte = r.ReadByte();
            ms.Position = 13;
            var RLWheelRotationCountByte = r.ReadByte();

            var tau = (float)Math.PI * 2;
            FLWheelRot = (FLWheelRotationByte / 255f * tau) + (FLWheelRotationCountByte * tau);
            FRWheelRot = (FRWheelRotationByte / 255f * tau) + (FRWheelRotationCountByte * tau);
            RRWheelRot = (RRWheelRotationByte / 255f * tau) + (RRWheelRotationCountByte * tau);
            RLWheelRot = (RLWheelRotationByte / 255f * tau) + (RLWheelRotationCountByte * tau);

            // Water
            ms.Position = 101;
            var waterByte = r.ReadByte();

            WetnessValue = waterByte / 255f;

            // IsGroundContact, IsReactorGroundMode, ReactorState
            ms.Position = 89;
            var groundModeByte = r.ReadByte();

            var maskIsGroundMode = 0x1;         // 00000001
            var maskIsReactorGroundMode = 0x4;  // 00000010
            var maskIsReactorUp = 0x8;          // 00001000
            var maskIsReactorDown = 0x10;       // 00010000
            var maskReactorLvl1 = 0x20;         // 00100000
            var maskReactorLvl2 = 0x40;         // 01000000 
                                                //var maskSlowMo = 0x80;              // 10000000 // Can use SimulationTimeCoef instead

            IsGroundContact = (groundModeByte & maskIsGroundMode) != 0;
            IsReactorGroundMode = (groundModeByte & maskIsReactorGroundMode) != 0;

            var isReactorUp = (groundModeByte & maskIsReactorUp) != 0;
            var isReactorDown = (groundModeByte & maskIsReactorDown) != 0;

            var isReactorLvl1 = (groundModeByte & maskReactorLvl1) != 0;
            var isReactorLvl2 = (groundModeByte & maskReactorLvl2) != 0;

            //var isSlowMo = (groundModeByte & maskSlowMo) != 0;

            if (isReactorLvl1)
                ReactorBoostLvl = ReactorBoostLvl.Lvl1;
            else if (isReactorLvl2)
                ReactorBoostLvl = ReactorBoostLvl.Lvl2;
            else
                ReactorBoostLvl = ReactorBoostLvl.None;

            if (isReactorUp && isReactorDown)
                ReactorBoostType = ReactorBoostType.UpAndDown;
            else if (isReactorUp)
                ReactorBoostType = ReactorBoostType.Up;
            else if (isReactorDown)
                ReactorBoostType = ReactorBoostType.Down;
            else
                ReactorBoostType = ReactorBoostType.None;

            // Turbo
            ms.Position = 31;
            var isTurboByte = r.ReadByte();
            ms.Position = 21;
            var turboTimeByte = r.ReadByte();

            var maskIsTurbo = 0x82; // 10000010

            IsTurbo = (isTurboByte & maskIsTurbo) != 0;
            TurboTime = turboTimeByte / 255f;

            // SimulationTimeCoef (SlowMo)
            ms.Position = 102;
            var SimulationTimeCoefByte = r.ReadByte();

            SimulationTimeCoef = SimulationTimeCoefByte / 255f;

            // ReactorAirControl
            // [1,0,-1] = (Accell,None,Brake), (Left,None,Right)
            ms.Position = 90;
            var boosterAirControlByte = r.ReadByte();

            var maskPedalNone = 0x10;   // 00010000
            var maskPedalAccel = 0x20;  // 00100000
            var maskSteerNone = 0x40;   // 01000000
            var maskSteerLeft = 0x80;   // 10000000

            var isAirControlPedalAccel = (boosterAirControlByte & maskPedalAccel) != 0;
            var isAirControlPedalNone = (boosterAirControlByte & maskPedalNone) != 0;

            ReactorAirControlPedal = isAirControlPedalAccel ? 1 : isAirControlPedalNone ? 0 : -1;

            var isAirControlSteerLeft = (boosterAirControlByte & maskSteerLeft) != 0;
            var isAirControlSteerNone = (boosterAirControlByte & maskSteerNone) != 0;

            ReactorAirControlSteer = isAirControlSteerLeft ? 1 : isAirControlSteerNone ? 0 : -1;

            // IsTopContact
            ms.Position = 76;
            var vechicleStateByte = r.ReadByte();

            var maskIsTopContact = 0x20;
            //var maskIsReactor = 0x10; 

            IsTopContact = (vechicleStateByte & maskIsTopContact) != 0;

            // SideSpeed
            ms.Position = 2;
            float sideSpeedInt = r.ReadUInt16();
            SideSpeed = (float)((float)((sideSpeedInt / 65536.0) - 0.5) * 2000.0);
        }
    }
}
