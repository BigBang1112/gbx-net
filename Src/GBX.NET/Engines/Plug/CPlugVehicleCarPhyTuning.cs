namespace GBX.NET.Engines.Plug;

public partial class CPlugVehicleCarPhyTuning
{
    public CPlugVehicleCarPhyTuning()
    {
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
