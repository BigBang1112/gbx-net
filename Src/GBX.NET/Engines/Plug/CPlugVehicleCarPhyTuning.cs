namespace GBX.NET.Engines.Plug;

public partial class CPlugVehicleCarPhyTuning
{
    public partial class Chunk090ED0A7
    {
        public Chunk090ED0A7() : this(GameVersion.Unspecified)
        {
        }

        public Chunk090ED0A7(GameVersion gameVersion)
        {
            Version = gameVersion == GameVersion.TM2020 ? 19 : 0;

            // Native defaults from Trackmania.exe, initializer 0x1406004E0.
            U06 = new PlugCurve { Count = 2, Values = [new(0f, 1.25f), new(1f, 0f)] };
            U07 = new PlugCurve { Count = 5, Values = [new(-200f, 50f), new(-100f, 20f), new(-25f, 1f), new(0f, 0f), new(25f, -1.5f)] };
            U10 = new PlugCurve { Count = 8, Values = [new(100f, 8f), new(150f, 4f), new(200f, 3f), new(300f, 2.5f), new(400f, 1.5f), new(500f, 1f), new(800f, 0.5f), new(1000f, 0f)] };
            U20 = new PlugCurve { Count = 4, Values = [new(-600f, 0f), new(-200f, 1f), new(-100f, 2f), new(0f, 3f)] };
            U21 = new PlugCurve { Count = 2, Values = [new(0f, 25f), new(1f, 10f)] };
            U22 = new PlugCurve { Count = 3, Values = [new(0f, 2.25f), new(0.5f, 2.5f), new(1f, 2.75f)] };
            U25 = new PlugCurve { Count = 2, Values = [new(-50f, 1.5f), new(0f, 0f)] };
            U26 = new PlugCurve { Count = 4, Values = [new(300f, 1f), new(400f, 0.8f), new(600f, 0.6f), new(800f, 0.2f)] };
            U29 = new PlugCurve { Count = 5, Values = [new(700f, 1f), new(800f, 0.8f), new(900f, 0.6f), new(1300f, 0.2f), new(1500f, 0.1f)] };
            U30 = new PlugCurve { Count = 1, Values = [new(0f, 1f)] };
            U31 = new PlugCurve { Count = 5, Values = [new(0f, 0f), new(50f, 0.05f), new(100f, 0.15f), new(250f, 0.175f), new(500f, 0.2f)] };
            U33 = new PlugCurve { Count = 3, Values = [new(0f, 0f), new(100f, 15f), new(500f, 20f)] };
            U34 = new PlugCurve { Count = 5, Values = [new(0f, 0f), new(1000f, 0.1f), new(1500f, 0.25f), new(2000f, 0.5f), new(2500f, 1f)] };
        }
    }

    public partial class Chunk090ED0A6
    {
        public Chunk090ED0A6() : this(GameVersion.Unspecified)
        {
        }

        public Chunk090ED0A6(GameVersion gameVersion)
        {
            Version = gameVersion == GameVersion.TM2020 ? 15 : 0;

            // Native defaults from Trackmania.exe, constructor 0x140600CF0.
            U01 = new PlugCurve { Count = 3, Values = [new(0f, 8f), new(0.5f, 2f), new(1f, 8f)] };
            U02 = new PlugCurve { Count = 2, Values = [new(0f, 0f), new(10f, 1f)] };
            U03 = new PlugCurve { Count = 3, Values = [new(0f, 7f), new(200f, 6f), new(400f, 5f)] };
            U08 = new PlugCurve { Count = 1, Values = [new(0f, 0f)] };
            U09 = new PlugCurve { Count = 1, Values = [new(0f, 0f)] };
            U11 = new PlugCurve { Count = 1, Values = [new(0f, 0f)] };
            U13 = new PlugCurve { Count = 1, Values = [new(0f, 0f)] };
            U14 = new PlugCurve { Count = 1, Values = [new(0f, 1f)] };
            U19 = new PlugCurve { Count = 1, Values = [new(0f, 1f)] };
            U20 = new PlugCurve { Count = 1, Values = [new(0f, 1f)] };
            U21 = new PlugCurve { Count = 1, Values = [new(0f, 0f)] };
            U22 = new PlugCurve { Count = 1, Values = [new(0f, 1f)] };
            U28 = new PlugCurve { Count = 2, Values = [new(0f, 1.07f), new(1f, 1.05f)] };
        }
    }

    public partial class Chunk090ED0A5
    {
        public Chunk090ED0A5() : this(GameVersion.Unspecified)
        {
        }

        public Chunk090ED0A5(GameVersion gameVersion)
        {
            Version = gameVersion == GameVersion.TM2020 ? 2 : 0;

            // Native defaults from Trackmania.exe, initializer 0x1406003C0.
            U21 = new PlugCurve { Count = 4, Values = [new(0f, 0f), new(10f, 25f), new(20f, 75f), new(40f, 150f)] };
            U25 = new PlugCurve { Count = 3, Values = [new(0f, 8f), new(0.5f, 1f), new(1f, 0f)] };
        }
    }

    public partial class Chunk090ED0A4
    {
        public Chunk090ED0A4() : this(GameVersion.Unspecified)
        {
        }

        public Chunk090ED0A4(GameVersion gameVersion)
        {
            Version = gameVersion == GameVersion.TM2020 ? 18 : 0;

            // Native defaults from Trackmania.exe, 0x140600CF0 and 0x1405FEDC0.
            U02 = new PlugCurve { Count = 1, Values = [new(0f, 0f)] };
            U08 = new PlugCurve { Count = 3, Values = [new(50f, 1f), new(200f, 0.9f), new(400f, 0.6f)] };
            U09 = new PlugCurve { Count = 1, Values = [new(0f, 0f)] };
            U10 = new PlugCurve { Count = 4, Values = [new(1f, 0.9f), new(2f, 0.8f), new(3f, 0.7f), new(4f, 0.5f)] };
            U12 = new PlugCurve { Count = 4, Values = [new(0f, 0f), new(50f, 0.1f), new(150f, 0.15f), new(200f, 0.4f)] };
            U13 = new PlugCurve { Count = 2, Values = [new(1f, 0.1f), new(4f, 1f)] };
            U19 = new PlugCurve { Count = 1, Values = [new(0f, 0f)] };
        }
    }

    public partial class Chunk090ED0A3
    {
        public Chunk090ED0A3() : this(GameVersion.Unspecified)
        {
        }

        public Chunk090ED0A3(GameVersion gameVersion)
        {
            Version = gameVersion == GameVersion.TM2020 ? 13 : 0;

            // Native defaults from Trackmania.exe, initializer 0x1406001C0.
            U19 = new PlugCurve { Count = 3, Values = [new(0f, 0.3f), new(100f, 3f), new(300f, 4f)] };
            U20 = new PlugCurve { Count = 1, Values = [new(0f, 0f)] };
            U28 = new PlugCurve { Count = 4, Values = [new(0f, 0f), new(0.05f, 0.6f), new(0.2f, 0.85f), new(1f, 1f)] };
            U29 = new PlugCurve { Count = 4, Values = [new(0f, 0f), new(0.8f, 0.3f), new(0.95f, 0.7f), new(1f, 1f)] };
        }
    }

    public partial class Chunk090ED0A2
    {
        public Chunk090ED0A2() : this(GameVersion.Unspecified)
        {
        }

        public Chunk090ED0A2(GameVersion gameVersion)
        {
            Version = gameVersion == GameVersion.TM2020 ? 2 : 0;

            // Native defaults from Trackmania.exe, constructor 0x140600CF0.
            U01 = new PlugCurve { Count = 3, Values = [new(0f, 1f), new(50f, 0.5f), new(100f, 0.25f)] };
            U02 = new PlugCurve { Count = 4, Values = [new(0f, 0f), new(50f, 0.95f), new(100f, 0.98f), new(200f, 0.99f)] };
            U03 = new PlugCurve { Count = 3, Values = [new(0f, 0f), new(50f, 0.95f), new(200f, 1f)] };
            U04 = new PlugCurve { Count = 3, Values = [new(0f, 1f), new(50f, 0.5f), new(100f, 0.25f)] };
            U05 = new PlugCurve { Count = 3, Values = [new(0f, 1f), new(75f, 0.2f), new(100f, 0f)] };
            U06 = new PlugCurve { Count = 3, Values = [new(0f, 1f), new(50f, 0.75f), new(100f, 0.6f)] };
            U07 = new PlugCurve { Count = 3, Values = [new(0f, 1f), new(50f, 0.75f), new(100f, 0.6f)] };
            U08 = new PlugCurve { Count = 1, Values = [new(0f, 0f)] };
            U09 = new PlugCurve { Count = 1, Values = [new(0f, 0f)] };
            U10 = new PlugCurve { Count = 1, Values = [new(0f, 0f)] };
            U11 = new PlugCurve { Count = 3, Values = [new(0f, 1f), new(50f, 0.5f), new(100f, 0.25f)] };
            U12 = new PlugCurve { Count = 3, Values = [new(0f, 1f), new(50f, 0f), new(100f, 0f)] };
        }
    }

    public CPlugVehicleCarPhyTuning()
    {
        HelicoYawTorqueCoefFromHSpeedKmh = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 3500f)]
        };
        HelicoYawTorqueFromRollSteer = new PlugCurve
        {
            Count = 2,
            Values = [new(0f, 0f), new(20f, 50000f)]
        };
        HelicoYawTorqueFromRollSteer_CoefFromHSpeedKmh = new PlugCurve
        {
            Count = 2,
            Values = [new(10f, 1f), new(100f, 0f)]
        };
        HelicoYaw_Brake_HSpeedDecFromHSpeed = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0f)]
        };
        HelicoRollTorqueFromRollDeg = new PlugCurve
        {
            Count = 4,
            Values = [new(0f, 4000f), new(0.5f, 5000f), new(0.6f, 7000f), new(0.7f, 0f)]
        };
        HelicoDragRollFromRollDeg = new PlugCurve
        {
            Count = 3,
            Values = [new(0f, 0f), new(0.2f, 2000f), new(0.6f, 7000f)]
        };
        HelicoPitchTorqueFromPitchDeg = new PlugCurve
        {
            Count = 4,
            Values = [new(0f, 4000f), new(0.5f, 5000f), new(0.6f, 7000f), new(0.7f, 0f)]
        };
        HelicoDragPitchFromPitchDeg = new PlugCurve
        {
            Count = 3,
            Values = [new(0f, 0f), new(0.2f, 2000f), new(0.6f, 7000f)]
        };
        HelicoPitchTorqueCoefFromSpeedKmh = new PlugCurve
        {
            Count = 2,
            Values = [new(0f, 1.5f), new(20f, 1f)]
        };
        HelicoGroundAccelCoefFromGroundDist = new PlugCurve
        {
            Count = 2,
            Values = [new(1f, 1f), new(20f, 1f)]
        };
        HelicoCoefSmoothedLanding = new PlugCurve
        {
            Count = 6,
            Values = [new(0f, 180000f), new(0.2f, 110000f), new(0.4f, 90000f), new(0.6f, 70000f), new(0.8f, 10000f), new(1f, 5000f)]
        };
        HelicoCoefEngineHorizontal = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 200000f)]
        };
        HelicoCoefEngineVertical = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 200000f)]
        };
        HelicoVerticalForceCoefFromAngleDeg = new PlugCurve
        {
            Count = 6,
            Values = [new(0f, 2.5f), new(20f, 1.2f), new(25f, 1f), new(35f, 1f), new(50f, 0.8f), new(90f, 0f)]
        };
        HelicoHorizontalForceCoefFromAngleDeg = new PlugCurve
        {
            Count = 6,
            Values = [new(0f, 0f), new(20f, 0.8f), new(25f, 1f), new(35f, 1f), new(40f, 1.2f), new(90f, 2f)]
        };
        BoatWheelTimeToMaxAngleFromSpeedZKmh = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 1f)]
        };
        BoatWheelInverseTimeToZeroFromSpeedZKmh = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0f)]
        };
        BoatPlanningCoefBuildingRateFromPlanningCoefValue = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0f)]
        };
        BoatPlanningForceFromCoef = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0f)]
        };
        BoatPlanningAsternForceFromBrakeDuration = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0f)]
        };
        BoatAccelFromSpeedKmh = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 10000f)]
        };
        BoatReverseAccelFromSpeedKmh = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 1000f)]
        };
        BoatRudderAngleCoeffFromSpeedKmh = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 1f)]
        };
        BoatWaterSteeringFromSpeedKmh = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 100f)]
        };
        BoatEngineDirFollowRudderFromSpeedZKmh = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 1f)]
        };
        BoatReverseDirFollowRudderFromSpeedZKmh = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0f)]
        };
        BoatEngineSteeringFromSpeedKmh = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 100f)]
        };
        BoatRigthingTorqueRatioFromSpeedKmh = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 100f)]
        };
        BoatTargetAngularSpeedZFromRoll = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0f)]
        };
        TruckLinkRollMultiplierFromZSpeedKmh = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 1f)]
        };
        TruckLinkHackHeaderPositiveRollMultiplierCurve = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 1f)]
        };
        PlaneThrustFromFrontSpeedKmh = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 1f)]
        };
        PlaneLiftCoefFromStallAngleRatio = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0f)]
        };
        PlanePitchControlFromSpeedKmh = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0f)]
        };
        PlaneRollControlFromSpeedKmh = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0f)]
        };
        PlaneDragMultiplierFromGroundDist = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 1f)]
        };
        PlaneLiftMultiplierFromGroundDist = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 1f)]
        };
        PlaneYawGroundControlFromFrontSpeedKmh = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0f)]
        };
        PlaneYawControlFromFrontSpeedKmh = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0f)]
        };
        PlaneDragCoef = new Vec3(1f, 1f, 1f);
        PlaneXDragApplicationPosOffset = default(Vec3);

        EngineGearRatios = [338f, 297f, 207f, 143f, 100f, 84f, 56f];
        EngineAutoGearMaxRPMs = [0f, 0.8125f, 0.8125f, 0.8125f, 0.8125f, 0.8125f, 1f];
        EngineAutoGearMinRPMs = [0f, 0f, 0.375f, 0.53125f, 0.5625f, 0.625f, 0.625f];

        AccelCurve = new PlugCurve
        {
            Count = 6,
            Values = [new(-100f, 50f), new(0f, 50f), new(100f, 1f), new(200f, 0f), new(300f, 0f), new(400f, 0f)]
        };
        AccelCurveRear = new PlugCurve
        {
            Count = 3,
            Values = [new(-100f, 2f), new(-50f, 5f), new(0f, 10f)]
        };
        AccelCurveTargetSpeedTruck = new PlugCurve
        {
            Count = 6,
            Values = [new(-50f, 50000f), new(-10f, 10000f), new(0f, 0f), new(10f, 50000f), new(50f, 100000f), new(100f, 500000f)]
        };
        TargetSpeedFromSpeedTruck = new PlugCurve
        {
            Count = 7,
            Values = [new(-100f, 0.1f), new(-50f, 0.2f), new(-10f, 0.3f), new(0f, 0.5f), new(10f, 0.3f), new(50f, 0.2f), new(100f, 0.1f)]
        };
        TargetSpeedFromSpeedTruckRear = new PlugCurve
        {
            Count = 4,
            Values = [new(-100f, -0.3f), new(-50f, -0.3f), new(-10f, -0.2f), new(0f, -0.1f)]
        };
        AirFrictionFromSpeed = new PlugCurve
        {
            Count = 2,
            Values = [new(0f, 0f), new(300f, 6f)]
        };
        AirFrictionFromSpeed_Variable = new PlugCurve
        {
            Count = 2,
            Values = [new(0f, 0f), new(300f, 6f)]
        };
        TurboYellowConstantFromSpeed = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 60f)]
        };
        TurboRedConstantFromSpeed = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 180f)]
        };
        TurboCheatConstantFromSpeed = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 60f)]
        };
        AirControlZCoefFromAngularSpeed = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 1f)]
        };
        AirControlXAccelFromAngularSpeed = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0f)]
        };
        AirControlYAccelFromAngularSpeed = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0f)]
        };
        AirControlXLimitTorqueFromAngularSpeed = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0f)]
        };
        AirControlXAccelCoefFromOrientation = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0f)]
        };
        WaterReboundFromSpeedRatio = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 1f)]
        };
        WaterBumpSlowDownFromSpeedRatio = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0f)]
        };
        WaterFrictionFromSpeed = new PlugCurve
        {
            Count = 2,
            Values = [new(0f, 0f), new(1000f, 1000f)]
        };
        RolloverLateral = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0f)]
        };
        MaxSideFriction = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 30f)]
        };
        LateralContactSlowDown = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 2f)]
        };
        SteerSlowDown = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0f)]
        };
        RolloverLateralFromAngle = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 1f)]
        };
        SteerDriveTorque = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 10f)]
        };
        M4SteerRadiusFromSpeed = new PlugCurve
        {
            Count = 2,
            Values = [new(0f, 10f), new(100f, 20f)]
        };
        M4MaxFrictionTorqueFromSpeed = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 100f)]
        };
        M4MaxFrictionForceFromSpeed = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 40f)]
        };
        M5SlippingAccelCurve = new PlugCurve
        {
            Count = 3,
            Values = [new(-100f, 50f), new(0f, 50f), new(100f, 1f)]
        };
        M5SteerCoefFromSpeed = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 1f)]
        };
        M5SmoothInputSteerDurationFromSpeed = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0f)]
        };
        ModulationFromWheelCompression = new PlugCurve
        {
            Count = 3,
            Values = [new(0f, 1f), new(0.5f, 2f), new(1f, 3f)]
        };
        M6BurnoutRadius = new PlugCurve
        {
            Count = 3,
            Values = [new(0f, 1f), new(5f, 2f), new(10f, 3f)]
        };
        M6BurnoutLateralSpeed = new PlugCurve
        {
            Count = 3,
            Values = [new(0f, 0f), new(1f, 30f), new(10f, 30f)]
        };
        M6BurnoutRolloverFromSpeed = new PlugCurve
        {
            Count = 3,
            Values = [new(0f, 1.5f), new(10f, 1.2f), new(50f, 0.2f)]
        };
        M6DonutRolloverFromSpeed = new PlugCurve
        {
            Count = 3,
            Values = [new(0f, 0f), new(10f, 5f), new(20f, 6f)]
        };
        M6RolloverLateralFromSpeedRatio = new PlugCurve
        {
            Count = 5,
            Values = [new(0f, 0f), new(1f, 0.8f), new(10f, 1f), new(20f, 1f), new(30f, 1f)]
        };
        EngineSlowDownFromSpeed = new PlugCurve
        {
            Count = 3,
            Values = [new(0f, 0f), new(10f, 1f), new(500f, 10f)]
        };
        GroundSlowDownFromSpeed = new PlugCurve
        {
            Count = 2,
            Values = [new(0f, 0f), new(500f, 5f)]
        };
        SteerSlowDownFromSpeed = new PlugCurve
        {
            Count = 2,
            Values = [new(0f, 0f), new(100f, 0f)]
        };
        Normal_WheelLateralForce_FromWheelLateralSpeed = new PlugCurve
        {
            Count = 3,
            Values = [new(0f, 0f), new(8f, 80f), new(10f, 24f)]
        };
        Normal_VehicleBrakeForce_FromVehicleBrakeSpeed = new PlugCurve
        {
            Count = 2,
            Values = [new(0f, 1f), new(100f, 20f)]
        };
        InputSteerPowFromSpeed = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 1f)]
        };
        Normal_InputSteerRadiusFromSpeed = new PlugCurve
        {
            Count = 2,
            Values = [new(0f, 2f), new(100f, 5f)]
        };
        Drifting_InputSteerSensitivityFromSpeed = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 1f)]
        };
        Drifting_InputSteerAngleSmoothSpeedFromSlipAngle_Steer = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 1f)]
        };
        Drifting_InputSteerAngleSmoothSpeedFromSlipAngle_StopSteer = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 1f)]
        };
        Drifting_InputSteerAngleSmoothSpeedFromSlipAngle_CounterSteer = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 1f)]
        };
        Drifting_InputSteerRadiusFromSpeed = new PlugCurve
        {
            Count = 2,
            Values = [new(0f, 2f), new(100f, 5f)]
        };
        Drifting_BackCoefIntensityFromSpeed = new PlugCurve
        {
            Count = 3,
            Values = [new(0f, 1f), new(30f, 0.3f), new(200f, 0f)]
        };
        Drifting_BackCoefFromSlipAngle = new PlugCurve
        {
            Count = 2,
            Values = [new(0f, 1f), new(30f, 1f)]
        };
        Drifting_WheelLateralForce_FromWheelLateralSpeed = new PlugCurve
        {
            Count = 2,
            Values = [new(0f, 0f), new(100f, 10f)]
        };
        Drifting_VehicleLateralForce_FromVehicleLateralSpeed = new PlugCurve
        {
            Count = 2,
            Values = [new(0f, 0f), new(20f, 20f)]
        };
        Drifting_VehicleBrakeForce_FromVehicleBrakeSpeed = new PlugCurve
        {
            Count = 2,
            Values = [new(0f, 1f), new(100f, 50f)]
        };
        DragForceFromFrontSpeed = new PlugCurve
        {
            Count = 2,
            Values = [new(0f, 0f), new(100f, 2f)]
        };
        ImpulseDamperReduceCoef_FromLocalVelY = new PlugCurve
        {
            Count = 2,
            Values = [new(5f, 1f), new(8f, 0f)]
        };
        AntiWallHitDurationInc_FromLateralContactRelSpeed = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0f)]
        };
        AntiWallHitAccelCoefFromSpeed = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0.1f)]
        };
        AntiWallHitForceFromSpeed = new PlugCurve
        {
            Count = 1,
            Values = [new(0f, 0.2f)]
        };
        MagneticSuperDragForceFromSpeedKmh = new PlugCurve
        {
            Count = 3,
            Values = [new(20f, 0f), new(100f, 50f), new(300f, 200f)]
        };
    }
}
