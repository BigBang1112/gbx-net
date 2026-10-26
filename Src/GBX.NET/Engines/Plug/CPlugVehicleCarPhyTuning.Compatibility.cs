using GBX.NET.Engines.Function;

namespace GBX.NET.Engines.Plug;

public partial class CPlugVehicleCarPhyTuning
{
    [Obsolete("Use DamperModel instead.")]
    public enum EShockModel
    {
        Demo01,
        Demo02,
        Demo03
    }

    [Obsolete("Use GameplayModel instead.")]
    public enum ESteerModel
    {
        Steer01,
        Steer02,
        Steer03,
        Steer04,
        Steer05,
        Steer06
    }

    [Obsolete("Use DamperKi instead.")]
    public float AbsorbingValKi { get => DamperKi; set => DamperKi = value; }

    [Obsolete("Use DamperKa instead.")]
    public float AbsorbingValKa { get => DamperKa; set => DamperKa = value; }

    [Obsolete("Use DamperLengthMin instead.")]
    public float AbsorbingValMin { get => DamperLengthMin; set => DamperLengthMin = value; }

    [Obsolete("Use DamperLengthMax instead.")]
    public float AbsorbingValMax { get => DamperLengthMax; set => DamperLengthMax = value; }

    [Obsolete("Use DamperLengthRest instead.")]
    public float AbsorbingValRest { get => DamperLengthRest; set => DamperLengthRest = value; }

    [Obsolete("Use DamperTension instead.")]
    public float AbsorbTension { get => DamperTension; set => DamperTension = value; }

    [Obsolete("Use CenterOfMassZNormed instead.")]
    public float CMAftForce { get => CenterOfMassZNormed; set => CenterOfMassZNormed = value; }

    [Obsolete("Use CenterOfMassY instead.")]
    public float CMDownUp { get => CenterOfMassY; set => CenterOfMassY = value; }

    [Obsolete("Use GroundGravityCoef instead.")]
    public float GravityCoef { get => GroundGravityCoef; set => GroundGravityCoef = value; }

    [Obsolete("Use AirGravityCoef instead.")]
    public float GravityCoefAir { get => AirGravityCoef; set => AirGravityCoef = value; }

    [Obsolete("Use AirAngularFluidFrictionCoef1 instead.")]
    public float AngularFluidFrictionCoef1 { get => AirAngularFluidFrictionCoef1; set => AirAngularFluidFrictionCoef1 = value; }

    [Obsolete("Use AirAngularFluidFrictionCoef2 instead.")]
    public float AngularFluidFrictionCoef2 { get => AirAngularFluidFrictionCoef2; set => AirAngularFluidFrictionCoef2 = value; }

    [Obsolete("Use AngularSpeedImpulseScale instead.")]
    public float AngularSpeedYImpulseScale { get => AngularSpeedImpulseScale; set => AngularSpeedImpulseScale = value; }

    [Obsolete("Use AirControlMaxAngularSpeedY instead.")]
    public float MaxAngularSpeedYAirControl { get => AirControlMaxAngularSpeedY; set => AirControlMaxAngularSpeedY = value; }

    [Obsolete("Use BodyFrictionCoef_Concrete instead.")]
    public float BodyFrictionCoef { get => BodyFrictionCoef_Concrete; set => BodyFrictionCoef_Concrete = value; }

    [Obsolete("Use BodyRestCoef_Concrete instead.")]
    public float BodyRestCoef { get => BodyRestCoef_Concrete; set => BodyRestCoef_Concrete = value; }

    [Obsolete("Use DamperModel instead.")]
    public EShockModel ShockModel { get => (EShockModel)DamperModel; set => DamperModel = (int)value; }

    [Obsolete("Use GameplayModel instead.")]
    public ESteerModel SteerModel { get => (ESteerModel)GameplayModel; set => GameplayModel = (int)value; }

    [Obsolete("Use TurboYellowAccelCoef instead.")]
    public float TurboBoost { get => TurboYellowAccelCoef; set => TurboYellowAccelCoef = value; }

    [Obsolete("Use TurboYellowAccelCoefDuration instead.")]
    public int TurboDuration { get => TurboYellowAccelCoefDuration; set => TurboYellowAccelCoefDuration = value; }

    [Obsolete("Use TurboRedAccelCoef instead.")]
    public float Turbo2Boost { get => TurboRedAccelCoef; set => TurboRedAccelCoef = value; }

    [Obsolete("Use TurboRedAccelCoefDuration instead.")]
    public int Turbo2Duration { get => TurboRedAccelCoefDuration; set => TurboRedAccelCoefDuration = value; }

    [Obsolete("Use AccelCurveRear instead.")]
    public CFuncKeysReal? AccelCurveRearGear { get => AccelCurveRear; set => AccelCurveRear = value; }

    [Obsolete("Use EngineGearRatios instead.")]
    public float[]? M6GearRatio { get => EngineGearRatios; set => EngineGearRatios = value; }

    [Obsolete("Use EngineAutoGearMaxRPMs instead.")]
    public float[]? M6MaxRPM { get => EngineAutoGearMaxRPMs; set => EngineAutoGearMaxRPMs = value; }

    [Obsolete("Use EngineAutoGearMinRPMs instead.")]
    public float[]? M6MinRPM { get => EngineAutoGearMinRPMs; set => EngineAutoGearMinRPMs = value; }
}
