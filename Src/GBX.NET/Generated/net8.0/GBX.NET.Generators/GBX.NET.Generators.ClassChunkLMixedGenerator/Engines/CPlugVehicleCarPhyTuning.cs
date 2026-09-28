namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090ED000</remarks>
[Class(0x090ED000)]
public partial class CPlugVehicleCarPhyTuning : CPlugVehiclePhyTuning, IClass
{
    [Hexadecimal] public static new uint Id => 0x090ED000;




    private float steerRadiusMin;
    [AppliedWithChunk<Chunk090ED000>]
    public float SteerRadiusMin { get => steerRadiusMin; set => steerRadiusMin = value; }

    private float steerRadiusCoef;
    [AppliedWithChunk<Chunk090ED000>]
    public float SteerRadiusCoef { get => steerRadiusCoef; set => steerRadiusCoef = value; }

    private float maxSpeed;
    [AppliedWithChunk<Chunk090ED000>]
    public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }

    private float absorbingValKi;
    [AppliedWithChunk<Chunk090ED000>]
    public float AbsorbingValKi { get => absorbingValKi; set => absorbingValKi = value; }

    private float absorbingValKa;
    [AppliedWithChunk<Chunk090ED000>]
    public float AbsorbingValKa { get => absorbingValKa; set => absorbingValKa = value; }

    private float absorbingValMin;
    [AppliedWithChunk<Chunk090ED000>]
    public float AbsorbingValMin { get => absorbingValMin; set => absorbingValMin = value; }

    private float absorbingValMax;
    [AppliedWithChunk<Chunk090ED000>]
    public float AbsorbingValMax { get => absorbingValMax; set => absorbingValMax = value; }

    private float absorbingValRest;
    [AppliedWithChunk<Chunk090ED000>]
    public float AbsorbingValRest { get => absorbingValRest; set => absorbingValRest = value; }

    private float relSpeedMultCoef;
    [AppliedWithChunk<Chunk090ED000>]
    public float RelSpeedMultCoef { get => relSpeedMultCoef; set => relSpeedMultCoef = value; }

    private float debugAbsorbCoef;
    [AppliedWithChunk<Chunk090ED000>]
    public float DebugAbsorbCoef { get => debugAbsorbCoef; set => debugAbsorbCoef = value; }

    private string? name;
    [AppliedWithChunk<Chunk090ED001>]
    public string? Name { get => name; set => name = value; }

    private float mass;
    [AppliedWithChunk<Chunk090ED002>]
    public float Mass { get => mass; set => mass = value; }

    private float cMAftForce;
    [AppliedWithChunk<Chunk090ED002>]
    public float CMAftForce { get => cMAftForce; set => cMAftForce = value; }

    private float cMDownUp;
    [AppliedWithChunk<Chunk090ED002>]
    public float CMDownUp { get => cMDownUp; set => cMDownUp = value; }

    private bool isFakeEngine;
    [AppliedWithChunk<Chunk090ED004>]
    public bool IsFakeEngine { get => isFakeEngine; set => isFakeEngine = value; }

    private float gravityCoef;
    [AppliedWithChunk<Chunk090ED005>]
    public float GravityCoef { get => gravityCoef; set => gravityCoef = value; }

    private float steerSpeed;
    [AppliedWithChunk<Chunk090ED007>]
    public float SteerSpeed { get => steerSpeed; set => steerSpeed = value; }

    private float reverseMaxSpeed;
    [AppliedWithChunk<Chunk090ED008>]
    [AppliedWithChunk<Chunk090ED060>]
    public float ReverseMaxSpeed { get => reverseMaxSpeed; set => reverseMaxSpeed = value; }

    private float turboBoost;
    [AppliedWithChunk<Chunk090ED008>]
    [AppliedWithChunk<Chunk090ED060>]
    public float TurboBoost { get => turboBoost; set => turboBoost = value; }

    private float brakeBase;
    [AppliedWithChunk<Chunk090ED009>]
    public float BrakeBase { get => brakeBase; set => brakeBase = value; }

    private float linearFluidFrictionCoef;
    [AppliedWithChunk<Chunk090ED00A>]
    [AppliedWithChunk<Chunk090ED018>]
    public float LinearFluidFrictionCoef { get => linearFluidFrictionCoef; set => linearFluidFrictionCoef = value; }

    private float angularFluidFrictionCoef1;
    [AppliedWithChunk<Chunk090ED00A>]
    [AppliedWithChunk<Chunk090ED018>]
    public float AngularFluidFrictionCoef1 { get => angularFluidFrictionCoef1; set => angularFluidFrictionCoef1 = value; }

    private float inertiaMass;
    [AppliedWithChunk<Chunk090ED00B>]
    public float InertiaMass { get => inertiaMass; set => inertiaMass = value; }

    private Vec3 inertiaHalfDiag;
    [AppliedWithChunk<Chunk090ED00B>]
    public Vec3 InertiaHalfDiag { get => inertiaHalfDiag; set => inertiaHalfDiag = value; }

    private int tireMaterial;
    [AppliedWithChunk<Chunk090ED00D>]
    public int TireMaterial { get => tireMaterial; set => tireMaterial = value; }

    private EShockModel shockModel;
    [AppliedWithChunk<Chunk090ED00E>]
    public EShockModel ShockModel { get => shockModel; set => shockModel = value; }

    private ESteerModel steerModel;
    [AppliedWithChunk<Chunk090ED010>]
    public ESteerModel SteerModel { get => steerModel; set => steerModel = value; }

    private float steerGroundTorque;
    [AppliedWithChunk<Chunk090ED010>]
    public float SteerGroundTorque { get => steerGroundTorque; set => steerGroundTorque = value; }

    private float soundEngineVolume;
    [AppliedWithChunk<Chunk090ED012>]
    [AppliedWithChunk<Chunk090ED015>]
    [AppliedWithChunk<Chunk090ED032>]
    public float SoundEngineVolume { get => soundEngineVolume; set => soundEngineVolume = value; }

    private float soundSkidSandVolume;
    [AppliedWithChunk<Chunk090ED012>]
    [AppliedWithChunk<Chunk090ED015>]
    [AppliedWithChunk<Chunk090ED032>]
    public float SoundSkidSandVolume { get => soundSkidSandVolume; set => soundSkidSandVolume = value; }

    private float soundImpactVolume;
    [AppliedWithChunk<Chunk090ED012>]
    [AppliedWithChunk<Chunk090ED015>]
    [AppliedWithChunk<Chunk090ED032>]
    public float SoundImpactVolume { get => soundImpactVolume; set => soundImpactVolume = value; }

    private float soundBodyImpact_Hard_Threshold;
    [AppliedWithChunk<Chunk090ED012>]
    [AppliedWithChunk<Chunk090ED015>]
    [AppliedWithChunk<Chunk090ED032>]
    public float SoundBodyImpact_Hard_Threshold { get => soundBodyImpact_Hard_Threshold; set => soundBodyImpact_Hard_Threshold = value; }

    private float soundSkidConcreteVolume;
    [AppliedWithChunk<Chunk090ED015>]
    [AppliedWithChunk<Chunk090ED032>]
    public float SoundSkidConcreteVolume { get => soundSkidConcreteVolume; set => soundSkidConcreteVolume = value; }

    private float displaySteerRadiusMin;
    [AppliedWithChunk<Chunk090ED016>]
    public float DisplaySteerRadiusMin { get => displaySteerRadiusMin; set => displaySteerRadiusMin = value; }

    private float displaySteerRadiusCoef;
    [AppliedWithChunk<Chunk090ED016>]
    public float DisplaySteerRadiusCoef { get => displaySteerRadiusCoef; set => displaySteerRadiusCoef = value; }

    private float decrepitudeImpactVal;
    [AppliedWithChunk<Chunk090ED017>]
    public float DecrepitudeImpactVal { get => decrepitudeImpactVal; set => decrepitudeImpactVal = value; }

    private float decrepitudeCoef;
    [AppliedWithChunk<Chunk090ED017>]
    public float DecrepitudeCoef { get => decrepitudeCoef; set => decrepitudeCoef = value; }

    private float vibrationPeriodSpeedCoef;
    [AppliedWithChunk<Chunk090ED017>]
    public float VibrationPeriodSpeedCoef { get => vibrationPeriodSpeedCoef; set => vibrationPeriodSpeedCoef = value; }

    private float angularFluidFrictionCoef2;
    [AppliedWithChunk<Chunk090ED018>]
    public float AngularFluidFrictionCoef2 { get => angularFluidFrictionCoef2; set => angularFluidFrictionCoef2 = value; }

    private float bodyFrictionCoef;
    [AppliedWithChunk<Chunk090ED019>]
    public float BodyFrictionCoef { get => bodyFrictionCoef; set => bodyFrictionCoef = value; }

    private float bodyFrictionCoef_Metal;
    [AppliedWithChunk<Chunk090ED019>]
    public float BodyFrictionCoef_Metal { get => bodyFrictionCoef_Metal; set => bodyFrictionCoef_Metal = value; }

    private float bodyRestCoef_Metal;
    [AppliedWithChunk<Chunk090ED019>]
    public float BodyRestCoef_Metal { get => bodyRestCoef_Metal; set => bodyRestCoef_Metal = value; }

    private float bodyRestCoef;
    [AppliedWithChunk<Chunk090ED019>]
    public float BodyRestCoef { get => bodyRestCoef; set => bodyRestCoef = value; }

    private float wheelFrictionCoef_Concrete;
    [AppliedWithChunk<Chunk090ED019>]
    public float WheelFrictionCoef_Concrete { get => wheelFrictionCoef_Concrete; set => wheelFrictionCoef_Concrete = value; }

    private float wheelRestCoef_Concrete;
    [AppliedWithChunk<Chunk090ED019>]
    public float WheelRestCoef_Concrete { get => wheelRestCoef_Concrete; set => wheelRestCoef_Concrete = value; }

    private float wheelFrictionCoef_Metal;
    [AppliedWithChunk<Chunk090ED019>]
    public float WheelFrictionCoef_Metal { get => wheelFrictionCoef_Metal; set => wheelFrictionCoef_Metal = value; }

    private float wheelRestCoef_Metal;
    [AppliedWithChunk<Chunk090ED019>]
    public float WheelRestCoef_Metal { get => wheelRestCoef_Metal; set => wheelRestCoef_Metal = value; }

    private float gravityCoefAir;
    [AppliedWithChunk<Chunk090ED01D>]
    public float GravityCoefAir { get => gravityCoefAir; set => gravityCoefAir = value; }

    private float rolloverAxial;
    [AppliedWithChunk<Chunk090ED01D>]
    public float RolloverAxial { get => rolloverAxial; set => rolloverAxial = value; }

    private int airControlDuration;
    [AppliedWithChunk<Chunk090ED01E>]
    public int AirControlDuration { get => airControlDuration; set => airControlDuration = value; }

    private float steerAngleMax;
    [AppliedWithChunk<Chunk090ED020>]
    public float SteerAngleMax { get => steerAngleMax; set => steerAngleMax = value; }

    private float slipAngleForceMax;
    [AppliedWithChunk<Chunk090ED020>]
    public float SlipAngleForceMax { get => slipAngleForceMax; set => slipAngleForceMax = value; }

    private float slipAngleForceCoef1;
    [AppliedWithChunk<Chunk090ED020>]
    public float SlipAngleForceCoef1 { get => slipAngleForceCoef1; set => slipAngleForceCoef1 = value; }

    private float slipAngleForceCoef2;
    [AppliedWithChunk<Chunk090ED020>]
    public float SlipAngleForceCoef2 { get => slipAngleForceCoef2; set => slipAngleForceCoef2 = value; }

    private float angularSpeedYImpulseScale;
    [AppliedWithChunk<Chunk090ED023>]
    public float AngularSpeedYImpulseScale { get => angularSpeedYImpulseScale; set => angularSpeedYImpulseScale = value; }

    private float steerLowSpeed;
    [AppliedWithChunk<Chunk090ED023>]
    public float SteerLowSpeed { get => steerLowSpeed; set => steerLowSpeed = value; }

    private CFuncKeysReal? accelCurve;
    [AppliedWithChunk<Chunk090ED024>]
    public CFuncKeysReal? AccelCurve { get => accelCurve; set => accelCurve = value; }

    private float lateralSlopeAdherenceMin;
    [AppliedWithChunk<Chunk090ED026>]
    public float LateralSlopeAdherenceMin { get => lateralSlopeAdherenceMin; set => lateralSlopeAdherenceMin = value; }

    private float lateralSlopeAdherenceMax;
    [AppliedWithChunk<Chunk090ED026>]
    public float LateralSlopeAdherenceMax { get => lateralSlopeAdherenceMax; set => lateralSlopeAdherenceMax = value; }

    private float axialSlopeAdherenceMin;
    [AppliedWithChunk<Chunk090ED026>]
    public float AxialSlopeAdherenceMin { get => axialSlopeAdherenceMin; set => axialSlopeAdherenceMin = value; }

    private float axialSlopeAdherenceMax;
    [AppliedWithChunk<Chunk090ED026>]
    public float AxialSlopeAdherenceMax { get => axialSlopeAdherenceMax; set => axialSlopeAdherenceMax = value; }

    private float steerGroundTorqueSlippingCoef;
    [AppliedWithChunk<Chunk090ED027>]
    public float SteerGroundTorqueSlippingCoef { get => steerGroundTorqueSlippingCoef; set => steerGroundTorqueSlippingCoef = value; }

    private float maxSideFrictionBlendCoef;
    [AppliedWithChunk<Chunk090ED027>]
    public float MaxSideFrictionBlendCoef { get => maxSideFrictionBlendCoef; set => maxSideFrictionBlendCoef = value; }

    private CFuncKeysReal? maxSideFriction;
    [AppliedWithChunk<Chunk090ED028>]
    public CFuncKeysReal? MaxSideFriction { get => maxSideFriction; set => maxSideFriction = value; }

    private float maxSideFrictionSliding;
    [AppliedWithChunk<Chunk090ED028>]
    public float MaxSideFrictionSliding { get => maxSideFrictionSliding; set => maxSideFrictionSliding = value; }

    private float groundSlowDownBase;
    [AppliedWithChunk<Chunk090ED028>]
    public float GroundSlowDownBase { get => groundSlowDownBase; set => groundSlowDownBase = value; }

    private float absorbTension;
    [AppliedWithChunk<Chunk090ED028>]
    public float AbsorbTension { get => absorbTension; set => absorbTension = value; }

    private float sideFriction1;
    [AppliedWithChunk<Chunk090ED029>]
    public float SideFriction1 { get => sideFriction1; set => sideFriction1 = value; }

    private CFuncKeysReal? rolloverLateral;
    [AppliedWithChunk<Chunk090ED029>]
    public CFuncKeysReal? RolloverLateral { get => rolloverLateral; set => rolloverLateral = value; }

    private CFuncKeysReal? lateralContactSlowDown;
    [AppliedWithChunk<Chunk090ED02A>]
    public CFuncKeysReal? LateralContactSlowDown { get => lateralContactSlowDown; set => lateralContactSlowDown = value; }

    private CFuncKeysReal? steerSlowDown;
    [AppliedWithChunk<Chunk090ED02B>]
    public CFuncKeysReal? SteerSlowDown { get => steerSlowDown; set => steerSlowDown = value; }

    private float sideFriction2;
    [AppliedWithChunk<Chunk090ED02B>]
    public float SideFriction2 { get => sideFriction2; set => sideFriction2 = value; }

    private float rubberBallElasticity;
    [AppliedWithChunk<Chunk090ED02B>]
    public float RubberBallElasticity { get => rubberBallElasticity; set => rubberBallElasticity = value; }

    private int steerSlowDownFadeInDuration;
    [AppliedWithChunk<Chunk090ED02B>]
    public int SteerSlowDownFadeInDuration { get => steerSlowDownFadeInDuration; set => steerSlowDownFadeInDuration = value; }

    private CFuncKeysReal? rolloverLateralFromAngle;
    [AppliedWithChunk<Chunk090ED02C>]
    public CFuncKeysReal? RolloverLateralFromAngle { get => rolloverLateralFromAngle; set => rolloverLateralFromAngle = value; }

    private float maxAngularSpeedYAirControl;
    [AppliedWithChunk<Chunk090ED02C>]
    public float MaxAngularSpeedYAirControl { get => maxAngularSpeedYAirControl; set => maxAngularSpeedYAirControl = value; }

    private float brakeCoef;
    [AppliedWithChunk<Chunk090ED02C>]
    public float BrakeCoef { get => brakeCoef; set => brakeCoef = value; }

    private float brakeMax;
    [AppliedWithChunk<Chunk090ED02C>]
    public float BrakeMax { get => brakeMax; set => brakeMax = value; }

    private float brakeMaxDynamic;
    [AppliedWithChunk<Chunk090ED02C>]
    public float BrakeMaxDynamic { get => brakeMaxDynamic; set => brakeMaxDynamic = value; }

    private float groundSlowDownCoef;
    [AppliedWithChunk<Chunk090ED02C>]
    public float GroundSlowDownCoef { get => groundSlowDownCoef; set => groundSlowDownCoef = value; }

    private int steerSlowDownFadeOutDuration;
    [AppliedWithChunk<Chunk090ED02D>]
    public int SteerSlowDownFadeOutDuration { get => steerSlowDownFadeOutDuration; set => steerSlowDownFadeOutDuration = value; }

    private float steerSlowDownCoef;
    [AppliedWithChunk<Chunk090ED02E>]
    public float SteerSlowDownCoef { get => steerSlowDownCoef; set => steerSlowDownCoef = value; }

    private int turboDuration;
    [AppliedWithChunk<Chunk090ED02F>]
    [AppliedWithChunk<Chunk090ED060>]
    public int TurboDuration { get => turboDuration; set => turboDuration = value; }

    private CFuncKeysReal? steerDriveTorque;
    /// <summary>
    /// encryption trick here
    /// </summary>
    [AppliedWithChunk<Chunk090ED030>]
    public CFuncKeysReal? SteerDriveTorque { get => steerDriveTorque; set => steerDriveTorque = value; }

    private float limitToMaxSpeedForce;
    [AppliedWithChunk<Chunk090ED031>]
    public float LimitToMaxSpeedForce { get => limitToMaxSpeedForce; set => limitToMaxSpeedForce = value; }

    private float slopeSpeedGainLimit;
    [AppliedWithChunk<Chunk090ED031>]
    public float SlopeSpeedGainLimit { get => slopeSpeedGainLimit; set => slopeSpeedGainLimit = value; }

    private float soundWheelImpact_Hard_Threshold;
    [AppliedWithChunk<Chunk090ED032>]
    public float SoundWheelImpact_Hard_Threshold { get => soundWheelImpact_Hard_Threshold; set => soundWheelImpact_Hard_Threshold = value; }

    private float soundWheelImpact_Soft_Threshold;
    [AppliedWithChunk<Chunk090ED032>]
    public float SoundWheelImpact_Soft_Threshold { get => soundWheelImpact_Soft_Threshold; set => soundWheelImpact_Soft_Threshold = value; }

    private float soundBodyImpact_Soft_Threshold;
    [AppliedWithChunk<Chunk090ED032>]
    public float SoundBodyImpact_Soft_Threshold { get => soundBodyImpact_Soft_Threshold; set => soundBodyImpact_Soft_Threshold = value; }

    private float angularSpeedClamp;
    [AppliedWithChunk<Chunk090ED033>]
    public float AngularSpeedClamp { get => angularSpeedClamp; set => angularSpeedClamp = value; }

    private float linearSpeed2PositiveDeltaMax;
    [AppliedWithChunk<Chunk090ED034>]
    public float LinearSpeed2PositiveDeltaMax { get => linearSpeed2PositiveDeltaMax; set => linearSpeed2PositiveDeltaMax = value; }

    private bool noSteerSlowDownWhenSlipping;
    [AppliedWithChunk<Chunk090ED035>]
    public bool NoSteerSlowDownWhenSlipping { get => noSteerSlowDownWhenSlipping; set => noSteerSlowDownWhenSlipping = value; }

    private float m4LateralFrictionSquareForce;
    [AppliedWithChunk<Chunk090ED036>]
    public float M4LateralFrictionSquareForce { get => m4LateralFrictionSquareForce; set => m4LateralFrictionSquareForce = value; }

    private float m4LateralFrictionTorque;
    [AppliedWithChunk<Chunk090ED036>]
    public float M4LateralFrictionTorque { get => m4LateralFrictionTorque; set => m4LateralFrictionTorque = value; }

    private float m4LateralFrictionForce;
    [AppliedWithChunk<Chunk090ED036>]
    public float M4LateralFrictionForce { get => m4LateralFrictionForce; set => m4LateralFrictionForce = value; }

    private CFuncKeysReal? m4SteerRadiusFromSpeed;
    [AppliedWithChunk<Chunk090ED036>]
    public CFuncKeysReal? M4SteerRadiusFromSpeed { get => m4SteerRadiusFromSpeed; set => m4SteerRadiusFromSpeed = value; }

    private CFuncKeysReal? m4MaxFrictionTorqueFromSpeed;
    [AppliedWithChunk<Chunk090ED037>]
    public CFuncKeysReal? M4MaxFrictionTorqueFromSpeed { get => m4MaxFrictionTorqueFromSpeed; set => m4MaxFrictionTorqueFromSpeed = value; }

    private float m4MaxFrictionTorqueWhenSlippingCoef;
    [AppliedWithChunk<Chunk090ED038>]
    public float M4MaxFrictionTorqueWhenSlippingCoef { get => m4MaxFrictionTorqueWhenSlippingCoef; set => m4MaxFrictionTorqueWhenSlippingCoef = value; }

    private CFuncKeysReal? m4MaxFrictionForceFromSpeed;
    [AppliedWithChunk<Chunk090ED038>]
    public CFuncKeysReal? M4MaxFrictionForceFromSpeed { get => m4MaxFrictionForceFromSpeed; set => m4MaxFrictionForceFromSpeed = value; }

    private float m4MaxFrictionForceWhenSlipping;
    [AppliedWithChunk<Chunk090ED038>]
    public float M4MaxFrictionForceWhenSlipping { get => m4MaxFrictionForceWhenSlipping; set => m4MaxFrictionForceWhenSlipping = value; }

    private float m4SteerRadiusWhenSlippingCoef;
    [AppliedWithChunk<Chunk090ED038>]
    public float M4SteerRadiusWhenSlippingCoef { get => m4SteerRadiusWhenSlippingCoef; set => m4SteerRadiusWhenSlippingCoef = value; }

    private float m4LateralFrictionSquareTorque;
    [AppliedWithChunk<Chunk090ED039>]
    public float M4LateralFrictionSquareTorque { get => m4LateralFrictionSquareTorque; set => m4LateralFrictionSquareTorque = value; }

    private float m4SlipAngleSpeed;
    [AppliedWithChunk<Chunk090ED03A>]
    public float M4SlipAngleSpeed { get => m4SlipAngleSpeed; set => m4SlipAngleSpeed = value; }

    private float m4SteerRadiusCoefFromSlipAngle;
    [AppliedWithChunk<Chunk090ED03A>]
    public float M4SteerRadiusCoefFromSlipAngle { get => m4SteerRadiusCoefFromSlipAngle; set => m4SteerRadiusCoefFromSlipAngle = value; }

    private float m4LeaveSlippingSpeed;
    [AppliedWithChunk<Chunk090ED03B>]
    public float M4LeaveSlippingSpeed { get => m4LeaveSlippingSpeed; set => m4LeaveSlippingSpeed = value; }

    private float m4SteerAngleWhenSlippingMax;
    [AppliedWithChunk<Chunk090ED03C>]
    public float M4SteerAngleWhenSlippingMax { get => m4SteerAngleWhenSlippingMax; set => m4SteerAngleWhenSlippingMax = value; }

    private CFuncKeysReal? m5SlippingAccelCurve;
    [AppliedWithChunk<Chunk090ED03D>]
    public CFuncKeysReal? M5SlippingAccelCurve { get => m5SlippingAccelCurve; set => m5SlippingAccelCurve = value; }

    private int m5LateralConstantSlowDownDuration;
    [AppliedWithChunk<Chunk090ED03E>]
    public int M5LateralConstantSlowDownDuration { get => m5LateralConstantSlowDownDuration; set => m5LateralConstantSlowDownDuration = value; }

    private CFuncKeysReal? m5SteerCoefFromSpeed;
    [AppliedWithChunk<Chunk090ED03F>]
    public CFuncKeysReal? M5SteerCoefFromSpeed { get => m5SteerCoefFromSpeed; set => m5SteerCoefFromSpeed = value; }

    private CFuncKeysReal? m5SmoothInputSteerDurationFromSpeed;
    [AppliedWithChunk<Chunk090ED040>]
    public CFuncKeysReal? M5SmoothInputSteerDurationFromSpeed { get => m5SmoothInputSteerDurationFromSpeed; set => m5SmoothInputSteerDurationFromSpeed = value; }

    private float m5MaxAxialRolloverTorque;
    [AppliedWithChunk<Chunk090ED041>]
    public float M5MaxAxialRolloverTorque { get => m5MaxAxialRolloverTorque; set => m5MaxAxialRolloverTorque = value; }

    private int m5KeepSlidingAccelDuration;
    [AppliedWithChunk<Chunk090ED041>]
    public int M5KeepSlidingAccelDuration { get => m5KeepSlidingAccelDuration; set => m5KeepSlidingAccelDuration = value; }

    private int m5KeepSteerSlowDownDurarion;
    [AppliedWithChunk<Chunk090ED042>]
    public int M5KeepSteerSlowDownDurarion { get => m5KeepSteerSlowDownDurarion; set => m5KeepSteerSlowDownDurarion = value; }

    private int m5KeepNoSteerSlowDownWhenSlippingDuration;
    [AppliedWithChunk<Chunk090ED043>]
    public int M5KeepNoSteerSlowDownWhenSlippingDuration { get => m5KeepNoSteerSlowDownWhenSlippingDuration; set => m5KeepNoSteerSlowDownWhenSlippingDuration = value; }

    private float m5AccelSlipCoefMax;
    [AppliedWithChunk<Chunk090ED044>]
    public float M5AccelSlipCoefMax { get => m5AccelSlipCoefMax; set => m5AccelSlipCoefMax = value; }

    private float waterGravity;
    [AppliedWithChunk<Chunk090ED045>]
    [AppliedWithChunk<Chunk090ED046>]
    public float WaterGravity { get => waterGravity; set => waterGravity = value; }

    private float waterReboundMinHSpeed;
    [AppliedWithChunk<Chunk090ED045>]
    [AppliedWithChunk<Chunk090ED046>]
    public float WaterReboundMinHSpeed { get => waterReboundMinHSpeed; set => waterReboundMinHSpeed = value; }

    private float waterBumpMinSpeed;
    [AppliedWithChunk<Chunk090ED046>]
    public float WaterBumpMinSpeed { get => waterBumpMinSpeed; set => waterBumpMinSpeed = value; }

    private CFuncKeysReal? waterBumpSlowDownFromSpeedRatio;
    [AppliedWithChunk<Chunk090ED046>]
    public CFuncKeysReal? WaterBumpSlowDownFromSpeedRatio { get => waterBumpSlowDownFromSpeedRatio; set => waterBumpSlowDownFromSpeedRatio = value; }

    private CFuncKeysReal? waterFrictionFromSpeed;
    [AppliedWithChunk<Chunk090ED046>]
    public CFuncKeysReal? WaterFrictionFromSpeed { get => waterFrictionFromSpeed; set => waterFrictionFromSpeed = value; }

    private CFuncKeysReal? waterReboundFromSpeedRatio;
    [AppliedWithChunk<Chunk090ED046>]
    public CFuncKeysReal? WaterReboundFromSpeedRatio { get => waterReboundFromSpeedRatio; set => waterReboundFromSpeedRatio = value; }

    private float waterAngularFriction;
    [AppliedWithChunk<Chunk090ED047>]
    public float WaterAngularFriction { get => waterAngularFriction; set => waterAngularFriction = value; }

    private CFuncKeysReal? waterSplashFromSpeed;
    [AppliedWithChunk<Chunk090ED048>]
    public CFuncKeysReal? WaterSplashFromSpeed { get => waterSplashFromSpeed; set => waterSplashFromSpeed = value; }

    private CFuncKeysReal? modulationFromWheelCompression;
    [AppliedWithChunk<Chunk090ED049>]
    public CFuncKeysReal? ModulationFromWheelCompression { get => modulationFromWheelCompression; set => modulationFromWheelCompression = value; }

    private float m6InertialMass;
    [AppliedWithChunk<Chunk090ED04A>]
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6InertialMass { get => m6InertialMass; set => m6InertialMass = value; }

    private float m6ForceEpsilon;
    [AppliedWithChunk<Chunk090ED04D>]
    public float M6ForceEpsilon { get => m6ForceEpsilon; set => m6ForceEpsilon = value; }

    private float m6InertialTorqueModualtionX;
    [AppliedWithChunk<Chunk090ED04E>]
    public float M6InertialTorqueModualtionX { get => m6InertialTorqueModualtionX; set => m6InertialTorqueModualtionX = value; }

    private float m6InertialTorqueModulationZ;
    [AppliedWithChunk<Chunk090ED04E>]
    public float M6InertialTorqueModulationZ { get => m6InertialTorqueModulationZ; set => m6InertialTorqueModulationZ = value; }

    private float m6MaxSpeed4Burnout;
    [AppliedWithChunk<Chunk090ED04F>]
    public float M6MaxSpeed4Burnout { get => m6MaxSpeed4Burnout; set => m6MaxSpeed4Burnout = value; }

    private CFuncKeysReal? m6BurnoutRadius;
    [AppliedWithChunk<Chunk090ED04F>]
    public CFuncKeysReal? M6BurnoutRadius { get => m6BurnoutRadius; set => m6BurnoutRadius = value; }

    private float m6BurnoutFricMod;
    [AppliedWithChunk<Chunk090ED051>]
    public float M6BurnoutFricMod { get => m6BurnoutFricMod; set => m6BurnoutFricMod = value; }

    private CFuncKeysReal? m6RolloverLateralFromSpeedRatio;
    [AppliedWithChunk<Chunk090ED052>]
    public CFuncKeysReal? M6RolloverLateralFromSpeedRatio { get => m6RolloverLateralFromSpeedRatio; set => m6RolloverLateralFromSpeedRatio = value; }

    private float m6BurnoutCenterForceCoeff2;
    [AppliedWithChunk<Chunk090ED053>]
    public float M6BurnoutCenterForceCoeff2 { get => m6BurnoutCenterForceCoeff2; set => m6BurnoutCenterForceCoeff2 = value; }

    private float m6BurnoutRpmAcc;
    [AppliedWithChunk<Chunk090ED056>]
    public float M6BurnoutRpmAcc { get => m6BurnoutRpmAcc; set => m6BurnoutRpmAcc = value; }

    private float m6AirRpmAcc;
    [AppliedWithChunk<Chunk090ED056>]
    public float M6AirRpmAcc { get => m6AirRpmAcc; set => m6AirRpmAcc = value; }

    private float m6AirRpmDeadening;
    [AppliedWithChunk<Chunk090ED056>]
    public float M6AirRpmDeadening { get => m6AirRpmDeadening; set => m6AirRpmDeadening = value; }

    private float[]? m6RpmWantedOnGearUp;
    [AppliedWithChunk<Chunk090ED057>]
    public float[]? M6RpmWantedOnGearUp { get => m6RpmWantedOnGearUp; set => m6RpmWantedOnGearUp = value; }

    private float m6RpmLossCoefOnGearUp;
    [AppliedWithChunk<Chunk090ED058>]
    public float M6RpmLossCoefOnGearUp { get => m6RpmLossCoefOnGearUp; set => m6RpmLossCoefOnGearUp = value; }

    private float m6RpmGainCoefOnGearDown;
    [AppliedWithChunk<Chunk090ED059>]
    public float M6RpmGainCoefOnGearDown { get => m6RpmGainCoefOnGearDown; set => m6RpmGainCoefOnGearDown = value; }

    private float m6SpeedLimitPositiveForTakeOffFront;
    [AppliedWithChunk<Chunk090ED059>]
    public float M6SpeedLimitPositiveForTakeOffFront { get => m6SpeedLimitPositiveForTakeOffFront; set => m6SpeedLimitPositiveForTakeOffFront = value; }

    private float m6RpmGainOnTakeOff;
    [AppliedWithChunk<Chunk090ED059>]
    public float M6RpmGainOnTakeOff { get => m6RpmGainOnTakeOff; set => m6RpmGainOnTakeOff = value; }

    private float m6RpmLossOnTakeOffFinished;
    [AppliedWithChunk<Chunk090ED059>]
    public float M6RpmLossOnTakeOffFinished { get => m6RpmLossOnTakeOffFinished; set => m6RpmLossOnTakeOffFinished = value; }

    private float m6SpeedLimitPositiveForTakeOffRear;
    [AppliedWithChunk<Chunk090ED05A>]
    public float M6SpeedLimitPositiveForTakeOffRear { get => m6SpeedLimitPositiveForTakeOffRear; set => m6SpeedLimitPositiveForTakeOffRear = value; }

    private float m6SpeedLimitNegForTakeOffFront;
    [AppliedWithChunk<Chunk090ED05A>]
    public float M6SpeedLimitNegForTakeOffFront { get => m6SpeedLimitNegForTakeOffFront; set => m6SpeedLimitNegForTakeOffFront = value; }

    private float m6SpeedLimitNegTakeOffRear;
    [AppliedWithChunk<Chunk090ED05A>]
    public float M6SpeedLimitNegTakeOffRear { get => m6SpeedLimitNegTakeOffRear; set => m6SpeedLimitNegTakeOffRear = value; }

    private CFuncKeysReal? brakeHeatSpeedFromFBrake;
    [AppliedWithChunk<Chunk090ED05B>]
    public CFuncKeysReal? BrakeHeatSpeedFromFBrake { get => brakeHeatSpeedFromFBrake; set => brakeHeatSpeedFromFBrake = value; }

    private float brakeCoolingSpeed;
    [AppliedWithChunk<Chunk090ED05B>]
    public float BrakeCoolingSpeed { get => brakeCoolingSpeed; set => brakeCoolingSpeed = value; }

    private float m6BrakeMaxRear;
    [AppliedWithChunk<Chunk090ED05C>]
    public float M6BrakeMaxRear { get => m6BrakeMaxRear; set => m6BrakeMaxRear = value; }

    private float m6BrakeMaxDynamicRear;
    [AppliedWithChunk<Chunk090ED05C>]
    public float M6BrakeMaxDynamicRear { get => m6BrakeMaxDynamicRear; set => m6BrakeMaxDynamicRear = value; }

    private float m6MaxDiffBtwnPropulsionAndSpeed;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6MaxDiffBtwnPropulsionAndSpeed { get => m6MaxDiffBtwnPropulsionAndSpeed; set => m6MaxDiffBtwnPropulsionAndSpeed = value; }

    private CFuncKeysReal? accelCurveRearGear;
    [AppliedWithChunk<Chunk090ED05D>]
    public CFuncKeysReal? AccelCurveRearGear { get => accelCurveRearGear; set => accelCurveRearGear = value; }

    private CFuncKeysReal? m6BurnoutLateralSpeed;
    [AppliedWithChunk<Chunk090ED05D>]
    public CFuncKeysReal? M6BurnoutLateralSpeed { get => m6BurnoutLateralSpeed; set => m6BurnoutLateralSpeed = value; }

    private float m6BurnoutLateralSpeedCoeff;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6BurnoutLateralSpeedCoeff { get => m6BurnoutLateralSpeedCoeff; set => m6BurnoutLateralSpeedCoeff = value; }

    private float m6MinSpeed4Burnout;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6MinSpeed4Burnout { get => m6MinSpeed4Burnout; set => m6MinSpeed4Burnout = value; }

    private float m6BurnoutSteerCoeff;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6BurnoutSteerCoeff { get => m6BurnoutSteerCoeff; set => m6BurnoutSteerCoeff = value; }

    private float m6BurnoutCenterForceCoeff;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6BurnoutCenterForceCoeff { get => m6BurnoutCenterForceCoeff; set => m6BurnoutCenterForceCoeff = value; }

    private float m6BurnoutRadiusMax;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6BurnoutRadiusMax { get => m6BurnoutRadiusMax; set => m6BurnoutRadiusMax = value; }

    private float m6BurnoutLateralSpeedMax;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6BurnoutLateralSpeedMax { get => m6BurnoutLateralSpeedMax; set => m6BurnoutLateralSpeedMax = value; }

    private float m6BurnoutSteerCoeff2;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6BurnoutSteerCoeff2 { get => m6BurnoutSteerCoeff2; set => m6BurnoutSteerCoeff2 = value; }

    private float m6BurnoutSteerCoeff3;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6BurnoutSteerCoeff3 { get => m6BurnoutSteerCoeff3; set => m6BurnoutSteerCoeff3 = value; }

    private float m6BrakeModulationWhenSlipping;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6BrakeModulationWhenSlipping { get => m6BrakeModulationWhenSlipping; set => m6BrakeModulationWhenSlipping = value; }

    private int m6BurnoutDuration;
    [AppliedWithChunk<Chunk090ED05D>]
    public int M6BurnoutDuration { get => m6BurnoutDuration; set => m6BurnoutDuration = value; }

    private float m6BurnoutAccMod;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6BurnoutAccMod { get => m6BurnoutAccMod; set => m6BurnoutAccMod = value; }

    private float m6AfterBurnoutAccMod;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6AfterBurnoutAccMod { get => m6AfterBurnoutAccMod; set => m6AfterBurnoutAccMod = value; }

    private int m6AfterBurnoutDuration;
    [AppliedWithChunk<Chunk090ED05D>]
    public int M6AfterBurnoutDuration { get => m6AfterBurnoutDuration; set => m6AfterBurnoutDuration = value; }

    private float m6FrictionModulationWhenSlipNBrake;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6FrictionModulationWhenSlipNBrake { get => m6FrictionModulationWhenSlipNBrake; set => m6FrictionModulationWhenSlipNBrake = value; }

    private float m6BrakeSmokeIntensity;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6BrakeSmokeIntensity { get => m6BrakeSmokeIntensity; set => m6BrakeSmokeIntensity = value; }

    private float m6BurnoutSmokeVelocity;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6BurnoutSmokeVelocity { get => m6BurnoutSmokeVelocity; set => m6BurnoutSmokeVelocity = value; }

    private float m6BurnoutSmokeIntensity;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6BurnoutSmokeIntensity { get => m6BurnoutSmokeIntensity; set => m6BurnoutSmokeIntensity = value; }

    private CFuncKeysReal? m6BurnoutRolloverFromSpeed;
    [AppliedWithChunk<Chunk090ED05D>]
    public CFuncKeysReal? M6BurnoutRolloverFromSpeed { get => m6BurnoutRolloverFromSpeed; set => m6BurnoutRolloverFromSpeed = value; }

    private CFuncKeysReal? m6DonutRolloverFromSpeed;
    [AppliedWithChunk<Chunk090ED05D>]
    public CFuncKeysReal? M6DonutRolloverFromSpeed { get => m6DonutRolloverFromSpeed; set => m6DonutRolloverFromSpeed = value; }

    private float m6MaxDiffBtwGroundNormal;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6MaxDiffBtwGroundNormal { get => m6MaxDiffBtwGroundNormal; set => m6MaxDiffBtwGroundNormal = value; }

    private float m6BurnoutWheelAngularRotation;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6BurnoutWheelAngularRotation { get => m6BurnoutWheelAngularRotation; set => m6BurnoutWheelAngularRotation = value; }

    private float m6MaxPosAngle4Burnout;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6MaxPosAngle4Burnout { get => m6MaxPosAngle4Burnout; set => m6MaxPosAngle4Burnout = value; }

    private float m6MaxNegAngle4Burnout;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6MaxNegAngle4Burnout { get => m6MaxNegAngle4Burnout; set => m6MaxNegAngle4Burnout = value; }

    private float m6AfterBurnoutImpulse;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6AfterBurnoutImpulse { get => m6AfterBurnoutImpulse; set => m6AfterBurnoutImpulse = value; }

    private float m6MaxRpm;
    [AppliedWithChunk<Chunk090ED05D>]
    public float M6MaxRpm { get => m6MaxRpm; set => m6MaxRpm = value; }

    private float[]? m6GearRatio;
    [AppliedWithChunk<Chunk090ED05D>]
    public float[]? M6GearRatio { get => m6GearRatio; set => m6GearRatio = value; }

    private float[]? m6MaxRPM;
    [AppliedWithChunk<Chunk090ED05D>]
    public float[]? M6MaxRPM { get => m6MaxRPM; set => m6MaxRPM = value; }

    private float[]? m6MinRPM;
    [AppliedWithChunk<Chunk090ED05D>]
    public float[]? M6MinRPM { get => m6MinRPM; set => m6MinRPM = value; }

    private int steerDurationBeforeSteerSlowDown;
    [AppliedWithChunk<Chunk090ED05D>]
    public int SteerDurationBeforeSteerSlowDown { get => steerDurationBeforeSteerSlowDown; set => steerDurationBeforeSteerSlowDown = value; }

    private CFuncKeysReal? airControlZCoefFromAngularSpeed;
    [AppliedWithChunk<Chunk090ED05E>]
    public CFuncKeysReal? AirControlZCoefFromAngularSpeed { get => airControlZCoefFromAngularSpeed; set => airControlZCoefFromAngularSpeed = value; }

    private float waterAngularFrictionSq;
    [AppliedWithChunk<Chunk090ED05F>]
    public float WaterAngularFrictionSq { get => waterAngularFrictionSq; set => waterAngularFrictionSq = value; }

    private float turbo2Boost;
    [AppliedWithChunk<Chunk090ED060>]
    public float Turbo2Boost { get => turbo2Boost; set => turbo2Boost = value; }

    private int turbo2Duration;
    [AppliedWithChunk<Chunk090ED060>]
    public int Turbo2Duration { get => turbo2Duration; set => turbo2Duration = value; }

    private CFuncKeysReal? visualSteerAngleFromSpeed;
    [AppliedWithChunk<Chunk090ED061>]
    public CFuncKeysReal? VisualSteerAngleFromSpeed { get => visualSteerAngleFromSpeed; set => visualSteerAngleFromSpeed = value; }

    private float[]? engineGearRatios;
    [AppliedWithChunk<Chunk090ED096>]
    public float[]? EngineGearRatios { get => engineGearRatios; set => engineGearRatios = value; }

    private float[]? engineAutoGearMaxRPMs;
    [AppliedWithChunk<Chunk090ED096>]
    public float[]? EngineAutoGearMaxRPMs { get => engineAutoGearMaxRPMs; set => engineAutoGearMaxRPMs = value; }

    private float[]? engineAutoGearMinRPMs;
    [AppliedWithChunk<Chunk090ED096>]
    public float[]? EngineAutoGearMinRPMs { get => engineAutoGearMinRPMs; set => engineAutoGearMinRPMs = value; }


    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x000 chunk
    /// </summary>
    [Chunk(0x090ED000)]
    public partial class Chunk090ED000 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED000;

        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref n.steerRadiusMin);
            rw.Single(ref n.steerRadiusCoef);
            rw.Single(ref n.maxSpeed);
            rw.Single(ref n.absorbingValKi);
            rw.Single(ref n.absorbingValKa);
            rw.Single(ref n.absorbingValMin);
            rw.Single(ref n.absorbingValMax);
            rw.Single(ref n.absorbingValRest);
            rw.Single(ref n.relSpeedMultCoef);
            rw.Single(ref U02);
            rw.Single(ref n.debugAbsorbCoef);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x001 chunk
    /// </summary>
    [Chunk(0x090ED001)]
    public partial class Chunk090ED001 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED001;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Id(ref n.name);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x002 chunk
    /// </summary>
    [Chunk(0x090ED002)]
    public partial class Chunk090ED002 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED002;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.mass);
            rw.Single(ref n.cMAftForce);
            rw.Single(ref n.cMDownUp);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x004 chunk
    /// </summary>
    [Chunk(0x090ED004)]
    public partial class Chunk090ED004 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED004;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isFakeEngine);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x005 chunk
    /// </summary>
    [Chunk(0x090ED005)]
    public partial class Chunk090ED005 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED005;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.gravityCoef);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x006 chunk
    /// </summary>
    [Chunk(0x090ED006)]
    public partial class Chunk090ED006 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED006;

        public float U01;
        public float U02;
        public float U03;
        public float U04;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x007 chunk
    /// </summary>
    [Chunk(0x090ED007)]
    public partial class Chunk090ED007 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED007;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.steerSpeed);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x008 chunk
    /// </summary>
    [Chunk(0x090ED008)]
    public partial class Chunk090ED008 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED008;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.reverseMaxSpeed);
            rw.Single(ref n.turboBoost);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x009 chunk
    /// </summary>
    [Chunk(0x090ED009)]
    public partial class Chunk090ED009 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED009;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.brakeBase);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x00A chunk
    /// </summary>
    [Chunk(0x090ED00A)]
    public partial class Chunk090ED00A : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED00A;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.linearFluidFrictionCoef);
            rw.Single(ref n.angularFluidFrictionCoef1);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x00B chunk
    /// </summary>
    [Chunk(0x090ED00B)]
    public partial class Chunk090ED00B : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED00B;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.inertiaMass);
            rw.Vec3(ref n.inertiaHalfDiag);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x00C chunk
    /// </summary>
    [Chunk(0x090ED00C)]
    public partial class Chunk090ED00C : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED00C;

        public bool U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x00D chunk
    /// </summary>
    [Chunk(0x090ED00D)]
    public partial class Chunk090ED00D : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED00D;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.tireMaterial);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x00E chunk
    /// </summary>
    [Chunk(0x090ED00E)]
    public partial class Chunk090ED00E : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED00E;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EShockModel>(ref n.shockModel);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x010 chunk
    /// </summary>
    [Chunk(0x090ED010)]
    public partial class Chunk090ED010 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED010;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.EnumInt32<ESteerModel>(ref n.steerModel);
            rw.Single(ref n.steerGroundTorque);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x011 chunk
    /// </summary>
    [Chunk(0x090ED011)]
    public partial class Chunk090ED011 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED011;

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x012 chunk
    /// </summary>
    [Chunk(0x090ED012)]
    public partial class Chunk090ED012 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED012;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.soundEngineVolume);
            rw.Single(ref n.soundSkidSandVolume);
            rw.Single(ref n.soundImpactVolume);
            rw.Single(ref n.soundBodyImpact_Hard_Threshold);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x013 chunk
    /// </summary>
    [Chunk(0x090ED013)]
    public partial class Chunk090ED013 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED013;

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;
        public float U11;
        public float U12;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);
            rw.Single(ref U10);
            rw.Single(ref U11);
            rw.Single(ref U12);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x014 chunk
    /// </summary>
    [Chunk(0x090ED014)]
    public partial class Chunk090ED014 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED014;

        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x015 chunk
    /// </summary>
    [Chunk(0x090ED015)]
    public partial class Chunk090ED015 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED015;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.soundEngineVolume);
            rw.Single(ref n.soundSkidSandVolume);
            rw.Single(ref n.soundSkidConcreteVolume);
            rw.Single(ref n.soundImpactVolume);
            rw.Single(ref n.soundBodyImpact_Hard_Threshold);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x016 chunk
    /// </summary>
    [Chunk(0x090ED016)]
    public partial class Chunk090ED016 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED016;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.displaySteerRadiusMin);
            rw.Single(ref n.displaySteerRadiusCoef);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x017 chunk
    /// </summary>
    [Chunk(0x090ED017)]
    public partial class Chunk090ED017 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED017;

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.decrepitudeImpactVal);
            rw.Single(ref n.decrepitudeCoef);
            rw.Single(ref n.vibrationPeriodSpeedCoef);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x018 chunk
    /// </summary>
    [Chunk(0x090ED018)]
    public partial class Chunk090ED018 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED018;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.linearFluidFrictionCoef);
            rw.Single(ref n.angularFluidFrictionCoef1);
            rw.Single(ref n.angularFluidFrictionCoef2);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x019 chunk
    /// </summary>
    [Chunk(0x090ED019)]
    public partial class Chunk090ED019 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED019;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.bodyFrictionCoef);
            rw.Single(ref n.bodyFrictionCoef_Metal);
            rw.Single(ref n.bodyRestCoef_Metal);
            rw.Single(ref n.bodyRestCoef);
            rw.Single(ref n.wheelFrictionCoef_Concrete);
            rw.Single(ref n.wheelRestCoef_Concrete);
            rw.Single(ref n.wheelFrictionCoef_Metal);
            rw.Single(ref n.wheelRestCoef_Metal);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x01A chunk
    /// </summary>
    [Chunk(0x090ED01A)]
    public partial class Chunk090ED01A : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED01A;

        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x01B chunk
    /// </summary>
    [Chunk(0x090ED01B)]
    public partial class Chunk090ED01B : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED01B;

        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x01D chunk
    /// </summary>
    [Chunk(0x090ED01D)]
    public partial class Chunk090ED01D : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED01D;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.gravityCoefAir);
            rw.Single(ref n.rolloverAxial);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x01E chunk
    /// </summary>
    [Chunk(0x090ED01E)]
    public partial class Chunk090ED01E : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED01E;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.airControlDuration);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x01F chunk
    /// </summary>
    [Chunk(0x090ED01F)]
    public partial class Chunk090ED01F : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED01F;

        public float U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x020 chunk
    /// </summary>
    [Chunk(0x090ED020)]
    public partial class Chunk090ED020 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED020;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.steerAngleMax);
            rw.Single(ref n.slipAngleForceMax);
            rw.Single(ref n.slipAngleForceCoef1);
            rw.Single(ref n.slipAngleForceCoef2);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x021 chunk
    /// </summary>
    [Chunk(0x090ED021)]
    public partial class Chunk090ED021 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED021;

        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x022 chunk
    /// </summary>
    [Chunk(0x090ED022)]
    public partial class Chunk090ED022 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED022;

        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x023 chunk
    /// </summary>
    [Chunk(0x090ED023)]
    public partial class Chunk090ED023 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED023;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.angularSpeedYImpulseScale);
            rw.Single(ref n.steerLowSpeed);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x024 chunk
    /// </summary>
    [Chunk(0x090ED024)]
    public partial class Chunk090ED024 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED024;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.accelCurve);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x026 chunk
    /// </summary>
    [Chunk(0x090ED026)]
    public partial class Chunk090ED026 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED026;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.lateralSlopeAdherenceMin);
            rw.Single(ref n.lateralSlopeAdherenceMax);
            rw.Single(ref n.axialSlopeAdherenceMin);
            rw.Single(ref n.axialSlopeAdherenceMax);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x027 chunk
    /// </summary>
    [Chunk(0x090ED027)]
    public partial class Chunk090ED027 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED027;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.steerGroundTorqueSlippingCoef);
            rw.Single(ref n.maxSideFrictionBlendCoef);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x028 chunk
    /// </summary>
    [Chunk(0x090ED028)]
    public partial class Chunk090ED028 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED028;

        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.NodeRef<CFuncKeysReal>(ref n.maxSideFriction);
            rw.Single(ref n.maxSideFrictionSliding);
            rw.Single(ref U02);
            rw.Single(ref n.groundSlowDownBase);
            rw.Single(ref n.absorbTension);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x029 chunk
    /// </summary>
    [Chunk(0x090ED029)]
    public partial class Chunk090ED029 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED029;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.sideFriction1);
            rw.NodeRef<CFuncKeysReal>(ref n.rolloverLateral);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x02A chunk
    /// </summary>
    [Chunk(0x090ED02A)]
    public partial class Chunk090ED02A : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED02A;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.lateralContactSlowDown);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x02B chunk
    /// </summary>
    [Chunk(0x090ED02B)]
    public partial class Chunk090ED02B : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED02B;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.steerSlowDown);
            rw.Single(ref n.sideFriction2);
            rw.Single(ref n.rubberBallElasticity);
            rw.Int32(ref n.steerSlowDownFadeInDuration);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x02C chunk
    /// </summary>
    [Chunk(0x090ED02C)]
    public partial class Chunk090ED02C : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED02C;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.rolloverLateralFromAngle);
            rw.Single(ref n.maxAngularSpeedYAirControl);
            rw.Single(ref n.brakeCoef);
            rw.Single(ref n.brakeMax);
            rw.Single(ref n.brakeMaxDynamic);
            rw.Single(ref n.groundSlowDownCoef);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x02D chunk
    /// </summary>
    [Chunk(0x090ED02D)]
    public partial class Chunk090ED02D : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED02D;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.steerSlowDownFadeOutDuration);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x02E chunk
    /// </summary>
    [Chunk(0x090ED02E)]
    public partial class Chunk090ED02E : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED02E;

        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.steerSlowDownCoef);
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x02F chunk
    /// </summary>
    [Chunk(0x090ED02F)]
    public partial class Chunk090ED02F : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED02F;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.turboDuration);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x030 chunk
    /// </summary>
    [Chunk(0x090ED030)]
    public partial class Chunk090ED030 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED030;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.steerDriveTorque); // encryption trick here
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x031 chunk
    /// </summary>
    [Chunk(0x090ED031)]
    public partial class Chunk090ED031 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED031;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.limitToMaxSpeedForce);
            rw.Single(ref n.slopeSpeedGainLimit);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x032 chunk
    /// </summary>
    [Chunk(0x090ED032)]
    public partial class Chunk090ED032 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED032;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.soundEngineVolume);
            rw.Single(ref n.soundSkidSandVolume);
            rw.Single(ref n.soundSkidConcreteVolume);
            rw.Single(ref n.soundImpactVolume);
            rw.Single(ref n.soundWheelImpact_Hard_Threshold);
            rw.Single(ref n.soundWheelImpact_Soft_Threshold);
            rw.Single(ref n.soundBodyImpact_Hard_Threshold);
            rw.Single(ref n.soundBodyImpact_Soft_Threshold);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x033 chunk
    /// </summary>
    [Chunk(0x090ED033)]
    public partial class Chunk090ED033 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED033;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.angularSpeedClamp);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x034 chunk
    /// </summary>
    [Chunk(0x090ED034)]
    public partial class Chunk090ED034 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED034;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.linearSpeed2PositiveDeltaMax);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x035 chunk
    /// </summary>
    [Chunk(0x090ED035)]
    public partial class Chunk090ED035 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED035;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.noSteerSlowDownWhenSlipping);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x036 chunk
    /// </summary>
    [Chunk(0x090ED036)]
    public partial class Chunk090ED036 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED036;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.m4LateralFrictionSquareForce);
            rw.Single(ref n.m4LateralFrictionTorque);
            rw.Single(ref n.m4LateralFrictionForce);
            rw.NodeRef<CFuncKeysReal>(ref n.m4SteerRadiusFromSpeed);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x037 chunk
    /// </summary>
    [Chunk(0x090ED037)]
    public partial class Chunk090ED037 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED037;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.m4MaxFrictionTorqueFromSpeed);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x038 chunk
    /// </summary>
    [Chunk(0x090ED038)]
    public partial class Chunk090ED038 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED038;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.m4MaxFrictionTorqueWhenSlippingCoef);
            rw.NodeRef<CFuncKeysReal>(ref n.m4MaxFrictionForceFromSpeed);
            rw.Single(ref n.m4MaxFrictionForceWhenSlipping);
            rw.Single(ref n.m4SteerRadiusWhenSlippingCoef);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x039 chunk
    /// </summary>
    [Chunk(0x090ED039)]
    public partial class Chunk090ED039 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED039;

        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.m4LateralFrictionSquareTorque);
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x03A chunk
    /// </summary>
    [Chunk(0x090ED03A)]
    public partial class Chunk090ED03A : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED03A;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.m4SlipAngleSpeed);
            rw.Single(ref n.m4SteerRadiusCoefFromSlipAngle);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x03B chunk
    /// </summary>
    [Chunk(0x090ED03B)]
    public partial class Chunk090ED03B : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED03B;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.m4LeaveSlippingSpeed);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x03C chunk
    /// </summary>
    [Chunk(0x090ED03C)]
    public partial class Chunk090ED03C : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED03C;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.m4SteerAngleWhenSlippingMax);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x03D chunk
    /// </summary>
    [Chunk(0x090ED03D)]
    public partial class Chunk090ED03D : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED03D;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.m5SlippingAccelCurve);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x03E chunk
    /// </summary>
    [Chunk(0x090ED03E)]
    public partial class Chunk090ED03E : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED03E;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.m5LateralConstantSlowDownDuration);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x03F chunk
    /// </summary>
    [Chunk(0x090ED03F)]
    public partial class Chunk090ED03F : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED03F;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.m5SteerCoefFromSpeed);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x040 chunk
    /// </summary>
    [Chunk(0x090ED040)]
    public partial class Chunk090ED040 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED040;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.m5SmoothInputSteerDurationFromSpeed);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x041 chunk
    /// </summary>
    [Chunk(0x090ED041)]
    public partial class Chunk090ED041 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED041;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.m5MaxAxialRolloverTorque);
            rw.Int32(ref n.m5KeepSlidingAccelDuration);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x042 chunk
    /// </summary>
    [Chunk(0x090ED042)]
    public partial class Chunk090ED042 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED042;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.m5KeepSteerSlowDownDurarion);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x043 chunk
    /// </summary>
    [Chunk(0x090ED043)]
    public partial class Chunk090ED043 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED043;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.m5KeepNoSteerSlowDownWhenSlippingDuration);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x044 chunk
    /// </summary>
    [Chunk(0x090ED044)]
    public partial class Chunk090ED044 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED044;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.m5AccelSlipCoefMax);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x045 chunk
    /// </summary>
    [Chunk(0x090ED045)]
    public partial class Chunk090ED045 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED045;

        public float U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.waterGravity);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref n.waterReboundMinHSpeed);
            rw.Single(ref U03);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x046 chunk
    /// </summary>
    [Chunk(0x090ED046)]
    public partial class Chunk090ED046 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED046;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.waterGravity);
            rw.Single(ref n.waterReboundMinHSpeed);
            rw.Single(ref n.waterBumpMinSpeed);
            rw.NodeRef<CFuncKeysReal>(ref n.waterBumpSlowDownFromSpeedRatio);
            rw.NodeRef<CFuncKeysReal>(ref n.waterFrictionFromSpeed);
            rw.NodeRef<CFuncKeysReal>(ref n.waterReboundFromSpeedRatio);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x047 chunk
    /// </summary>
    [Chunk(0x090ED047)]
    public partial class Chunk090ED047 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED047;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.waterAngularFriction);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x048 chunk
    /// </summary>
    [Chunk(0x090ED048)]
    public partial class Chunk090ED048 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED048;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.waterSplashFromSpeed);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x049 chunk
    /// </summary>
    [Chunk(0x090ED049)]
    public partial class Chunk090ED049 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED049;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.modulationFromWheelCompression);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x04A chunk
    /// </summary>
    [Chunk(0x090ED04A)]
    public partial class Chunk090ED04A : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED04A;

        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref n.m6InertialMass);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x04D chunk
    /// </summary>
    [Chunk(0x090ED04D)]
    public partial class Chunk090ED04D : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED04D;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.m6ForceEpsilon);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x04E chunk
    /// </summary>
    [Chunk(0x090ED04E)]
    public partial class Chunk090ED04E : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED04E;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.m6InertialTorqueModualtionX);
            rw.Single(ref n.m6InertialTorqueModulationZ);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x04F chunk
    /// </summary>
    [Chunk(0x090ED04F)]
    public partial class Chunk090ED04F : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED04F;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.m6MaxSpeed4Burnout);
            rw.NodeRef<CFuncKeysReal>(ref n.m6BurnoutRadius);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x051 chunk
    /// </summary>
    [Chunk(0x090ED051)]
    public partial class Chunk090ED051 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED051;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.m6BurnoutFricMod);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x052 chunk
    /// </summary>
    [Chunk(0x090ED052)]
    public partial class Chunk090ED052 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED052;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.m6RolloverLateralFromSpeedRatio);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x053 chunk
    /// </summary>
    [Chunk(0x090ED053)]
    public partial class Chunk090ED053 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED053;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.m6BurnoutCenterForceCoeff2);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x056 chunk
    /// </summary>
    [Chunk(0x090ED056)]
    public partial class Chunk090ED056 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED056;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.m6BurnoutRpmAcc);
            rw.Single(ref n.m6AirRpmAcc);
            rw.Single(ref n.m6AirRpmDeadening);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x057 chunk
    /// </summary>
    [Chunk(0x090ED057)]
    public partial class Chunk090ED057 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED057;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Array<float>(ref n.m6RpmWantedOnGearUp!);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x058 chunk
    /// </summary>
    [Chunk(0x090ED058)]
    public partial class Chunk090ED058 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED058;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.m6RpmLossCoefOnGearUp);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x059 chunk
    /// </summary>
    [Chunk(0x090ED059)]
    public partial class Chunk090ED059 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED059;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.m6RpmGainCoefOnGearDown);
            rw.Single(ref n.m6SpeedLimitPositiveForTakeOffFront);
            rw.Single(ref n.m6RpmGainOnTakeOff);
            rw.Single(ref n.m6RpmLossOnTakeOffFinished);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x05A chunk
    /// </summary>
    [Chunk(0x090ED05A)]
    public partial class Chunk090ED05A : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED05A;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.m6SpeedLimitPositiveForTakeOffRear);
            rw.Single(ref n.m6SpeedLimitNegForTakeOffFront);
            rw.Single(ref n.m6SpeedLimitNegTakeOffRear);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x05B chunk
    /// </summary>
    [Chunk(0x090ED05B)]
    public partial class Chunk090ED05B : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED05B;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.brakeHeatSpeedFromFBrake);
            rw.Single(ref n.brakeCoolingSpeed);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x05C chunk
    /// </summary>
    [Chunk(0x090ED05C)]
    public partial class Chunk090ED05C : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED05C;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.m6BrakeMaxRear);
            rw.Single(ref n.m6BrakeMaxDynamicRear);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x05D chunk
    /// </summary>
    [Chunk(0x090ED05D)]
    public partial class Chunk090ED05D : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED05D;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.m6InertialMass);
            rw.Single(ref n.m6MaxDiffBtwnPropulsionAndSpeed);
            rw.NodeRef<CFuncKeysReal>(ref n.accelCurveRearGear);
            rw.NodeRef<CFuncKeysReal>(ref n.m6BurnoutLateralSpeed);
            rw.Single(ref n.m6BurnoutLateralSpeedCoeff);
            rw.Single(ref n.m6MinSpeed4Burnout);
            rw.Single(ref n.m6BurnoutSteerCoeff);
            rw.Single(ref n.m6BurnoutCenterForceCoeff);
            rw.Single(ref n.m6BurnoutRadiusMax);
            rw.Single(ref n.m6BurnoutLateralSpeedMax);
            rw.Single(ref n.m6BurnoutSteerCoeff2);
            rw.Single(ref n.m6BurnoutSteerCoeff3);
            rw.Single(ref n.m6BrakeModulationWhenSlipping);
            rw.Int32(ref n.m6BurnoutDuration);
            rw.Single(ref n.m6BurnoutAccMod);
            rw.Single(ref n.m6AfterBurnoutAccMod);
            rw.Int32(ref n.m6AfterBurnoutDuration);
            rw.Single(ref n.m6FrictionModulationWhenSlipNBrake);
            rw.Single(ref n.m6BrakeSmokeIntensity);
            rw.Single(ref n.m6BurnoutSmokeVelocity);
            rw.Single(ref n.m6BurnoutSmokeIntensity);
            rw.NodeRef<CFuncKeysReal>(ref n.m6BurnoutRolloverFromSpeed);
            rw.NodeRef<CFuncKeysReal>(ref n.m6DonutRolloverFromSpeed);
            rw.Single(ref n.m6MaxDiffBtwGroundNormal);
            rw.Single(ref n.m6BurnoutWheelAngularRotation);
            rw.Single(ref n.m6MaxPosAngle4Burnout);
            rw.Single(ref n.m6MaxNegAngle4Burnout);
            rw.Single(ref n.m6AfterBurnoutImpulse);
            rw.Single(ref n.m6MaxRpm);
            rw.Array<float>(ref n.m6GearRatio!);
            rw.Array<float>(ref n.m6MaxRPM!);
            rw.Array<float>(ref n.m6MinRPM!);
            rw.Int32(ref n.steerDurationBeforeSteerSlowDown);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x05E chunk
    /// </summary>
    [Chunk(0x090ED05E)]
    public partial class Chunk090ED05E : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED05E;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.airControlZCoefFromAngularSpeed);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x05F chunk
    /// </summary>
    [Chunk(0x090ED05F)]
    public partial class Chunk090ED05F : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED05F;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.waterAngularFrictionSq);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x060 chunk
    /// </summary>
    [Chunk(0x090ED060)]
    public partial class Chunk090ED060 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED060;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref n.reverseMaxSpeed);
            rw.Single(ref n.turboBoost);
            rw.Single(ref n.turbo2Boost);
            rw.Int32(ref n.turboDuration);
            rw.Int32(ref n.turbo2Duration);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x061 chunk
    /// </summary>
    [Chunk(0x090ED061)]
    public partial class Chunk090ED061 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED061;


        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref n.visualSteerAngleFromSpeed);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x062 chunk
    /// </summary>
    [Chunk(0x090ED062)]
    public partial class Chunk090ED062 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED062;

        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x063 chunk
    /// </summary>
    [Chunk(0x090ED063)]
    public partial class Chunk090ED063 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED063;

        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x064 chunk
    /// </summary>
    [Chunk(0x090ED064)]
    public partial class Chunk090ED064 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED064;

        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x065 chunk
    /// </summary>
    [Chunk(0x090ED065)]
    public partial class Chunk090ED065 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED065;

        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x066 chunk
    /// </summary>
    [Chunk(0x090ED066)]
    public partial class Chunk090ED066 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED066;

        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x06A chunk
    /// </summary>
    [Chunk(0x090ED06A)]
    public partial class Chunk090ED06A : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED06A;

        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x06E chunk
    /// </summary>
    [Chunk(0x090ED06E)]
    public partial class Chunk090ED06E : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED06E;

        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x072 chunk
    /// </summary>
    [Chunk(0x090ED072)]
    public partial class Chunk090ED072 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED072;

        public float U01;
        public float U02;
        public float U03;
        public int U04;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Int32(ref U04);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x077 chunk
    /// </summary>
    [Chunk(0x090ED077)]
    public partial class Chunk090ED077 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED077;

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public CFuncKeysReal? U05;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.NodeRef<CFuncKeysReal>(ref U05);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x078 chunk
    /// </summary>
    [Chunk(0x090ED078)]
    public partial class Chunk090ED078 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED078;

        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x079 chunk
    /// </summary>
    [Chunk(0x090ED079)]
    public partial class Chunk090ED079 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED079;

        public bool U01;
        public CFuncKeysReal? U02;
        public CFuncKeysReal? U03;
        public float U04;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.NodeRef<CFuncKeysReal>(ref U02);
            rw.NodeRef<CFuncKeysReal>(ref U03);
            rw.Single(ref U04);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x07D chunk
    /// </summary>
    [Chunk(0x090ED07D)]
    public partial class Chunk090ED07D : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED07D;

        public int U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x082 chunk
    /// </summary>
    [Chunk(0x090ED082)]
    public partial class Chunk090ED082 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED082;

        public bool U01;
        public CFuncKeysReal? U02;
        public CFuncKeysReal? U03;
        public CFuncKeysReal? U04;
        public CFuncKeysReal? U05;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.NodeRef<CFuncKeysReal>(ref U02);
            rw.NodeRef<CFuncKeysReal>(ref U03);
            rw.NodeRef<CFuncKeysReal>(ref U04);
            rw.NodeRef<CFuncKeysReal>(ref U05);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x084 chunk
    /// </summary>
    [Chunk(0x090ED084)]
    public partial class Chunk090ED084 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED084;

        public float U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x085 chunk
    /// </summary>
    [Chunk(0x090ED085)]
    public partial class Chunk090ED085 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED085;

        public bool U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x086 chunk
    /// </summary>
    [Chunk(0x090ED086)]
    public partial class Chunk090ED086 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED086;

        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x088 chunk
    /// </summary>
    [Chunk(0x090ED088)]
    public partial class Chunk090ED088 : Chunk<CPlugVehicleCarPhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED088;

        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x089 chunk
    /// </summary>
    [Chunk(0x090ED089)]
    public partial class Chunk090ED089 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED089;

        public int Version { get; set; }

        public Keys? U01;
        public CFuncKeysReal? U02;
        public float? U03;
        public float? U04;
        public float? U05;
        public float? U06;
        public float? U07;
        public float? U08;
        public float? U09;
        public float? U10;
        public float? U11;
        public float? U12;
        public float? U13;
        public float? U14;
        public float? U15;
        public float? U16;
        public float? U17;
        public float? U18;
        public bool? U19;
        public Keys? U20;
        public Keys? U21;
        public Keys? U22;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ReadableWritable<Keys>(ref U01, version: Version - 10);
            if (Version <= 2)
            {
                rw.NodeRef<CFuncKeysReal>(ref U02);
            }
            if (Version >= 2)
            {
                rw.Single(ref U03);
                rw.Single(ref U04);
                rw.Single(ref U05);
                rw.Single(ref U06);
                if (Version >= 4)
                {
                    rw.Single(ref U07);
                    rw.Single(ref U08);
                    rw.Single(ref U09);
                    rw.Single(ref U10);
                    rw.Single(ref U11);
                    rw.Single(ref U12);
                    rw.Single(ref U13);
                    rw.Single(ref U14);
                    rw.Single(ref U15);
                    rw.Single(ref U16);
                    rw.Single(ref U17);
                    if (Version >= 6)
                    {
                        rw.Single(ref U18);
                        if (Version >= 7)
                        {
                            rw.Boolean(ref U19);
                            if (Version >= 8)
                            {
                                rw.ReadableWritable<Keys>(ref U20, version: Version - 10);
                                rw.ReadableWritable<Keys>(ref U21, version: Version - 10);
                                if (Version >= 9)
                                {
                                    rw.ReadableWritable<Keys>(ref U22, version: Version - 10);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x08A chunk
    /// </summary>
    [Chunk(0x090ED08A)]
    public partial class Chunk090ED08A : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED08A;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public Keys? U05;
        public Keys? U06;
        public Keys? U07;
        public Keys? U08;
        public float? U09;
        public float? U10;
        public Keys? U11;
        public Keys? U12;
        public int? U13;
        public float? U14;
        public Keys? U15;
        public bool? U16;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.ReadableWritable<Keys>(ref U05, version: Version - 8);
            rw.ReadableWritable<Keys>(ref U06, version: Version - 8);
            rw.ReadableWritable<Keys>(ref U07, version: Version - 8);
            rw.ReadableWritable<Keys>(ref U08, version: Version - 8);
            if (Version >= 1)
            {
                if (Version <= 3)
                {
                    rw.Single(ref U09);
                    rw.Single(ref U10);
                }
                if (Version >= 4)
                {
                    rw.ReadableWritable<Keys>(ref U11, version: Version - 8);
                    rw.ReadableWritable<Keys>(ref U12, version: Version - 8);
                }
                if (Version >= 3)
                {
                    rw.Int32(ref U13);
                    if (Version == 5)
                    {
                        rw.Single(ref U14);
                    }
                    if (Version >= 6)
                    {
                        rw.ReadableWritable<Keys>(ref U15, version: Version - 8);
                        if (Version >= 7)
                        {
                            rw.Boolean(ref U16);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x08B chunk
    /// </summary>
    [Chunk(0x090ED08B)]
    public partial class Chunk090ED08B : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED08B;

        public int Version { get; set; }

        public float? U01;
        public float? U02;
        public CFuncKeysReal? U03;
        public float U04;
        public CFuncKeysReal? U05;
        public float U06;
        public float U07;
        public CFuncKeysReal? U08;
        public bool U09;
        public float U10;
        public float U11;
        public Keys? U12;
        public int U13;
        public int U14;
        public Keys? U15;
        public float U16;
        public Keys? U17;
        public float U18;
        public Keys? U19;
        public float U20;
        public Keys? U21;
        public Keys? U22;
        public Keys? U23;
        public float U24;
        public float U25;
        public float U26;
        public float U27;
        public float U28;
        public Keys? U29;
        public float U30;
        public Keys? U31;
        public Keys? U32;
        public Keys? U33;
        public int U34;
        public int U35;
        public Keys? U36;
        public float U37;
        public float U38;
        public float U39;
        public int U40;
        public int U41;
        public int U42;
        public bool U43;
        public float U44;
        public float U45;
        public Keys? U46;
        public int U47;
        public bool U48;
        public float U49;
        public float U50;
        public float U51;
        public float U52;
        public float U53;
        public CFuncKeysReal? U54;
        public CFuncKeysReal? U55;
        public int U56;
        public float U57;
        public float U58;
        public CFuncKeysReal? U59;
        public CFuncKeysReal? U60;
        public float U61;
        public CFuncKeysReal? U62;
        public bool U63;
        public bool U64;
        public Keys? U65;
        public CFuncKeysReal? U66;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version <= 2)
            {
                rw.Single(ref U01);
            }
            if (Version <= 10)
            {
                rw.Single(ref U02);
                rw.NodeRef<CFuncKeysReal>(ref U03);
            }
            if (Version <= 9)
            {
                rw.Single(ref U04);
            }
            if (Version <= 8)
            {
                rw.NodeRef<CFuncKeysReal>(ref U05);
            }
            rw.Single(ref U06);
            if (Version <= 9)
            {
                rw.Single(ref U07);
            }
            if (Version <= 10)
            {
                rw.NodeRef<CFuncKeysReal>(ref U08);
            }
            if (Version >= 1)
            {
                rw.Boolean(ref U09);
            }
            if (Version >= 3)
            {
                if (Version <= 9)
                {
                    rw.Single(ref U10);
                    rw.Single(ref U11);
                }
            }
            if (Version >= 4)
            {
                rw.ReadableWritable<Keys>(ref U12, version: Version - 37);
                if (Version >= 5)
                {
                    if (Version <= 9)
                    {
                        rw.Int32(ref U13);
                        rw.Int32(ref U14);
                    }
                    if (Version >= 9)
                    {
                        rw.ReadableWritable<Keys>(ref U15, version: Version - 37);
                        if (Version >= 13)
                        {
                            if (Version <= 22)
                            {
                                rw.Single(ref U16);
                            }
                            if (Version >= 22)
                            {
                                rw.ReadableWritable<Keys>(ref U17, version: Version - 37);
                            }
                            rw.Single(ref U18);
                        }
                    }
                    if (Version >= 6)
                    {
                        rw.ReadableWritable<Keys>(ref U19, version: Version - 37);
                        if (Version >= 7)
                        {
                            rw.Single(ref U20);
                            if (Version >= 10)
                            {
                                rw.ReadableWritable<Keys>(ref U21, version: Version - 37);
                                rw.ReadableWritable<Keys>(ref U22, version: Version - 37);
                                rw.ReadableWritable<Keys>(ref U23, version: Version - 37);
                                if (Version >= 11)
                                {
                                    rw.Single(ref U24);
                                    rw.Single(ref U25);
                                    rw.Single(ref U26);
                                    rw.Single(ref U27);
                                    rw.Single(ref U28);
                                    rw.ReadableWritable<Keys>(ref U29, version: Version - 37);
                                    if (Version >= 12)
                                    {
                                        rw.Single(ref U30);
                                        if (Version >= 14)
                                        {
                                            rw.ReadableWritable<Keys>(ref U31, version: Version - 37);
                                            if (Version >= 15)
                                            {
                                                rw.ReadableWritable<Keys>(ref U32, version: Version - 37);
                                                if (Version >= 16)
                                                {
                                                    rw.ReadableWritable<Keys>(ref U33, version: Version - 37);
                                                    if (Version >= 17)
                                                    {
                                                        rw.Int32(ref U34);
                                                        if (Version >= 18)
                                                        {
                                                            rw.Int32(ref U35);
                                                            if (Version >= 19)
                                                            {
                                                                rw.ReadableWritable<Keys>(ref U36, version: Version - 37);
                                                                if (Version >= 20)
                                                                {
                                                                    rw.Single(ref U37);
                                                                    rw.Single(ref U38);
                                                                    rw.Single(ref U39);
                                                                    if (Version >= 21)
                                                                    {
                                                                        rw.Int32(ref U40);
                                                                        rw.Int32(ref U41);
                                                                        rw.Int32(ref U42);
                                                                        if (Version >= 23)
                                                                        {
                                                                            rw.Boolean(ref U43);
                                                                            rw.Single(ref U44);
                                                                            rw.Single(ref U45);
                                                                            rw.ReadableWritable<Keys>(ref U46, version: Version - 37);
                                                                            if (Version >= 24)
                                                                            {
                                                                                rw.Int32(ref U47);
                                                                                if (Version >= 25)
                                                                                {
                                                                                    rw.Boolean(ref U48);
                                                                                    rw.Single(ref U49);
                                                                                    if (Version >= 26)
                                                                                    {
                                                                                        rw.Single(ref U50);
                                                                                        if (Version >= 27)
                                                                                        {
                                                                                            if (Version <= 33)
                                                                                            {
                                                                                                rw.Single(ref U51);
                                                                                                rw.Single(ref U52);
                                                                                                rw.Single(ref U53);
                                                                                                rw.NodeRef<CFuncKeysReal>(ref U54);
                                                                                            }
                                                                                            if (Version >= 28)
                                                                                            {
                                                                                                if (Version <= 33)
                                                                                                {
                                                                                                    rw.NodeRef<CFuncKeysReal>(ref U55);
                                                                                                }
                                                                                                if (Version >= 29)
                                                                                                {
                                                                                                    rw.Int32(ref U56);
                                                                                                    rw.Single(ref U57);
                                                                                                    rw.Single(ref U58);
                                                                                                    if (Version <= 33)
                                                                                                    {
                                                                                                        if (Version >= 30)
                                                                                                        {
                                                                                                            rw.NodeRef<CFuncKeysReal>(ref U59);
                                                                                                        }
                                                                                                        if (Version >= 31)
                                                                                                        {
                                                                                                            rw.NodeRef<CFuncKeysReal>(ref U60);
                                                                                                        }
                                                                                                        if (Version >= 32)
                                                                                                        {
                                                                                                            rw.Single(ref U61);
                                                                                                            rw.NodeRef<CFuncKeysReal>(ref U62);
                                                                                                        }
                                                                                                    }
                                                                                                    if (Version >= 35)
                                                                                                    {
                                                                                                        rw.Boolean(ref U63);
                                                                                                        if (Version >= 36)
                                                                                                        {
                                                                                                            rw.Boolean(ref U64);
                                                                                                            rw.ReadableWritable<Keys>(ref U65, version: Version - 37);
                                                                                                            rw.NodeRef<CFuncKeysReal>(ref U66);
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x08C chunk
    /// </summary>
    [Chunk(0x090ED08C)]
    public partial class Chunk090ED08C : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED08C;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float? U05;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 2)
            {
                throw new ("");
            }
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            if (Version >= 1)
            {
                rw.Single(ref U05);
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x08D chunk
    /// </summary>
    [Chunk(0x090ED08D)]
    public partial class Chunk090ED08D : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED08D;

        public int Version { get; set; }

        public float U01;
        public bool U02;
        public bool? U03;
        public float? U04;
        public float? U05;
        public float? U06;
        public float? U07;
        public float U08;
        public bool? U09;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 7)
            {
                throw new ("");
            }
            rw.Single(ref U01);
            rw.Boolean(ref U02);
            if (Version >= 2)
            {
                rw.Boolean(ref U03);
            }
            if (Version >= 1)
            {
                rw.Single(ref U04);
                rw.Single(ref U05);
            }
            if (Version == 3)
            {
                rw.Single(ref U06);
                rw.Single(ref U07);
            }
            if (Version >= 4)
            {
                rw.Single(ref U08);
            }
            if (Version >= 5)
            {
                rw.Boolean(ref U09);
                if (Version == 5)
                {
                }
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x08E chunk
    /// </summary>
    [Chunk(0x090ED08E)]
    public partial class Chunk090ED08E : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED08E;

        public int Version { get; set; }

        public External<CPlugCamControlModel>[]? U01;
        public Components.GbxRefTableFile? U01File;
        public External<CPlugCamControlModel>[]? U02;
        public Components.GbxRefTableFile? U02File;
        public CPlugCamControlModel? U03;
        public Components.GbxRefTableFile? U03File;
        public CPlugCamControlModel? U04;
        public Components.GbxRefTableFile? U04File;
        public External<CPlugCamControlModel>[]? U05;
        public Components.GbxRefTableFile? U05File;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 5)
            {
                throw new ("");
            }
            rw.ArrayNodeRef_deprec<CPlugCamControlModel>(ref U01!);
            if (Version >= 1)
            {
                rw.ArrayNodeRef_deprec<CPlugCamControlModel>(ref U02!);
                if (Version >= 2)
                {
                    rw.NodeRef<CPlugCamControlModel>(ref U03, ref U03File);
                    if (Version >= 3)
                    {
                        rw.NodeRef<CPlugCamControlModel>(ref U04, ref U04File);
                        if (Version >= 4)
                        {
                            rw.ArrayNodeRef<CPlugCamControlModel>(ref U05!);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x094 chunk
    /// </summary>
    [Chunk(0x090ED094)]
    public partial class Chunk090ED094 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED094;

        public int Version { get; set; }

        public float U01;
        public Keys? U02;
        public int U03;
        public float U04;
        public Keys? U05;
        public int U06;
        public int? U07;
        public int? U08;
        public float? U09;
        public Keys? U10;
        public int? U11;
        public int? U12;
        public int? U13;
        public int U14;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 6)
            {
                throw new ("");
            }
            rw.Single(ref U01);
            rw.ReadableWritable<Keys>(ref U02, version: Version - 3);
            rw.Int32(ref U03);
            rw.Single(ref U04);
            rw.ReadableWritable<Keys>(ref U05, version: Version - 3);
            rw.Int32(ref U06);
            if (Version >= 1)
            {
                rw.Int32(ref U07);
                rw.Int32(ref U08);
                if (Version >= 2)
                {
                    rw.Single(ref U09);
                    rw.ReadableWritable<Keys>(ref U10, version: Version - 3);
                    rw.Int32(ref U11);
                    rw.Int32(ref U12);
                    if (Version == 3)
                    {
                        rw.Int32(ref U13);
                    }
                    if (Version >= 4)
                    {
                        rw.Int32(ref U14);
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x095 chunk
    /// </summary>
    [Chunk(0x090ED095)]
    public partial class Chunk090ED095 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED095;

        public int Version { get; set; }

        public int U01;
        public float U02;
        public Keys? U03;
        public bool U04;
        public float U05;
        public float U06;
        public float U07;
        public int? U08;
        public int? U09;
        public Keys? U10;
        public Keys? U11;
        public Keys? U12;
        public int? U13;
        public int? U14;
        public int? U15;
        public Keys? U16;
        public int? U17;
        public Keys? U18;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 8)
            {
                throw new ("");
            }
            rw.Int32(ref U01);
            rw.Single(ref U02);
            rw.ReadableWritable<Keys>(ref U03, version: Version - 1);
            rw.Boolean(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            if (Version >= 1)
            {
                rw.Int32(ref U08);
                if (Version >= 2)
                {
                    rw.Int32(ref U09);
                    rw.ReadableWritable<Keys>(ref U10, version: Version - 1);
                    rw.ReadableWritable<Keys>(ref U11, version: Version - 1);
                    if (Version >= 3)
                    {
                        rw.ReadableWritable<Keys>(ref U12, version: Version - 1);
                        if (Version >= 4)
                        {
                            rw.Int32(ref U13);
                            if (Version >= 5)
                            {
                                rw.Int32(ref U14);
                                rw.Int32(ref U15);
                                rw.ReadableWritable<Keys>(ref U16, version: Version - 1);
                                rw.Int32(ref U17);
                                if (Version >= 6)
                                {
                                    rw.ReadableWritable<Keys>(ref U18, version: Version - 1);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x096 chunk
    /// </summary>
    [Chunk(0x090ED096)]
    public partial class Chunk090ED096 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED096;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;
        public float? U10;
        public float U11;
        public float U12;
        public float U13;
        public float U14;
        public float U15;
        public float U16;
        public float U17;
        public bool U18;
        public float U19;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 6)
            {
                throw new ("");
            }
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);
            if (Version >= 2)
            {
                rw.Single(ref U10);
            }
            rw.Single(ref U11);
            rw.Single(ref U12);
            rw.Single(ref U13);
            rw.Single(ref U14);
            rw.Single(ref U15);
            rw.Single(ref U16);
            rw.Array<float>(ref n.engineGearRatios!);
            rw.Array<float>(ref n.engineAutoGearMaxRPMs!);
            rw.Array<float>(ref n.engineAutoGearMinRPMs!);
            if (Version >= 3)
            {
                rw.Single(ref U17);
                if (Version >= 4)
                {
                    rw.Boolean(ref U18);
                    if (Version >= 5)
                    {
                        rw.Single(ref U19);
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x097 chunk
    /// </summary>
    [Chunk(0x090ED097)]
    public partial class Chunk090ED097 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED097;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public bool U07;
        public CMwNod? U08;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Boolean(ref U07);
            if (Version >= 2)
            {
                rw.NodeRef<CMwNod>(ref U08);
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x098 chunk
    /// </summary>
    [Chunk(0x090ED098)]
    public partial class Chunk090ED098 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED098;

        public int Version { get; set; }

        public float U01;
        public int U02;
        public float U03;
        public float U04;
        public int U05;
        public float U06;
        public float U07;
        public float U08;
        public bool U09;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            rw.Int32(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Int32(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
            if (Version >= 2)
            {
                rw.Boolean(ref U09);
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x099 chunk
    /// </summary>
    [Chunk(0x090ED099)]
    public partial class Chunk090ED099 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED099;

        public int Version { get; set; }

        public CMwNod? U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CMwNod>(ref U01);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x09A chunk
    /// </summary>
    [Chunk(0x090ED09A)]
    public partial class Chunk090ED09A : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED09A;

        public int Version { get; set; }

        public CPlugVehicleGearBox? U01;
        public Components.GbxRefTableFile? U01File;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugVehicleGearBox>(ref U01, ref U01File);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x09B chunk
    /// </summary>
    [Chunk(0x090ED09B)]
    public partial class Chunk090ED09B : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED09B;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public int U07;
        public int U08;
        public int U09;
        public float? U10;
        public float? U11;
        public float? U12;
        public float? U13;
        public float? U14;
        public float? U15;
        public float? U16;
        public float? U17;
        public float? U18;
        public float? U19;
        public float? U20;
        public float? U21;
        public float? U22;
        public float? U23;
        public float? U24;
        public float? U25;
        public float? U26;
        public float? U27;
        public float? U28;
        public float? U29;
        public float? U30;
        public Keys? U31;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Int32(ref U07);
            rw.Int32(ref U08);
            rw.Int32(ref U09);
            if (Version >= 1)
            {
                rw.Single(ref U10);
                rw.Single(ref U11);
                rw.Single(ref U12);
                rw.Single(ref U13);
                rw.Single(ref U14);
                rw.Single(ref U15);
                rw.Single(ref U16);
                rw.Single(ref U17);
                rw.Single(ref U18);
                rw.Single(ref U19);
                rw.Single(ref U20);
                rw.Single(ref U21);
                rw.Single(ref U22);
                if (Version >= 2)
                {
                    rw.Single(ref U23);
                    if (Version >= 3)
                    {
                        rw.Single(ref U24);
                        rw.Single(ref U25);
                        rw.Single(ref U26);
                        rw.Single(ref U27);
                        rw.Single(ref U28);
                        rw.Single(ref U29);
                        rw.Single(ref U30);
                        if (Version >= 4)
                        {
                            rw.ReadableWritable<Keys>(ref U31);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x09C chunk
    /// </summary>
    [Chunk(0x090ED09C)]
    public partial class Chunk090ED09C : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED09C;

        public int Version { get; set; }

        public float? U01;
        public Keys? U02;
        public Keys? U03;
        public Keys? U04;
        public float? U05;
        public float? U06;
        public CFuncKeysReal? U07;
        public CFuncKeysReal? U08;
        public CFuncKeysReal? U09;
        public Keys? U10;
        public Keys? U11;
        public Keys? U12;
        public Keys? U13;
        public float? U14;
        public float? U15;
        public CFuncKeysReal? U16;
        public float? U17;
        public Keys? U18;
        public float? U19;
        public float? U20;
        public float? U21;
        public float? U22;
        public int? U23;
        public Keys? U24;
        public Keys? U25;
        public float? U26;
        public float? U27;
        public Keys? U28;
        public Keys? U29;
        public Keys? U30;
        public Keys? U31;
        public float? U32;
        public float? U33;
        public Keys? U34;
        public float? U35;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version <= 17)
            {
                rw.Single(ref U01);
            }
            if (Version >= 18)
            {
                rw.ReadableWritable<Keys>(ref U02, version: Version - 21);
                rw.ReadableWritable<Keys>(ref U03, version: Version - 21);
                rw.ReadableWritable<Keys>(ref U04, version: Version - 21);
            }
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.NodeRef<CFuncKeysReal>(ref U07);
            rw.NodeRef<CFuncKeysReal>(ref U08);
            rw.NodeRef<CFuncKeysReal>(ref U09);
            rw.ReadableWritable<Keys>(ref U10, version: Version - 21);
            rw.ReadableWritable<Keys>(ref U11, version: Version - 21);
            if (Version >= 1)
            {
                rw.ReadableWritable<Keys>(ref U12, version: Version - 21);
                rw.ReadableWritable<Keys>(ref U13, version: Version - 21);
            }
            if (Version >= 2)
            {
                rw.Single(ref U14);
            }
            if (Version == 3)
            {
                rw.Single(ref U15);
            }
            if (Version >= 4)
            {
                rw.NodeRef<CFuncKeysReal>(ref U16);
                if (Version >= 5)
                {
                    rw.Single(ref U17);
                    rw.ReadableWritable<Keys>(ref U18, version: Version - 21);
                    rw.Single(ref U19);
                    rw.Single(ref U20);
                    rw.Single(ref U21);
                    if (Version >= 6)
                    {
                        rw.Single(ref U22);
                        if (Version >= 7)
                        {
                            rw.Int32(ref U23);
                            rw.ReadableWritable<Keys>(ref U24, version: Version - 21);
                            if (Version >= 8)
                            {
                                rw.ReadableWritable<Keys>(ref U25, version: Version - 21);
                                if (Version >= 9)
                                {
                                    rw.Single(ref U26);
                                    if (Version >= 10)
                                    {
                                        rw.Single(ref U27);
                                        if (Version >= 11)
                                        {
                                            rw.ReadableWritable<Keys>(ref U28, version: Version - 21);
                                            rw.ReadableWritable<Keys>(ref U29, version: Version - 21);
                                            if (Version >= 12)
                                            {
                                                rw.ReadableWritable<Keys>(ref U30, version: Version - 21);
                                                if (Version >= 13)
                                                {
                                                    rw.ReadableWritable<Keys>(ref U31, version: Version - 21);
                                                    if (Version >= 15)
                                                    {
                                                        rw.Single(ref U32);
                                                        if (Version >= 16)
                                                        {
                                                            rw.Single(ref U33);
                                                            if (Version >= 19)
                                                            {
                                                                rw.ReadableWritable<Keys>(ref U34, version: Version - 21);
                                                                if (Version >= 20)
                                                                {
                                                                    rw.Single(ref U35);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x09D chunk
    /// </summary>
    [Chunk(0x090ED09D)]
    public partial class Chunk090ED09D : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED09D;

        public int Version { get; set; }

        public bool? U01;
        public float? U02;
        public float? U03;
        public float U04;
        public float U05;
        public bool? U06;
        public float U07;
        public float U08;
        public Keys? U09;
        public float? U10;
        public Keys? U11;
        public float? U12;
        public float? U13;
        public float U14;
        public float U15;
        public Keys? U16;
        public Keys? U17;
        public Keys? U18;
        public Keys? U19;
        public Keys? U20;
        public float? U21;
        public float? U22;
        public float? U23;
        public Keys? U24;
        public Keys? U25;
        public Keys? U26;
        public Keys? U27;
        public Keys? U28;
        public Keys? U29;
        public Keys? U30;
        public Keys? U31;
        public Keys? U32;
        public Keys? U33;
        public Keys? U34;
        public float? U35;
        public Keys? U36;
        public float? U37;
        public float? U38;
        public float? U39;
        public Keys? U40;
        public float? U41;
        public Keys? U42;
        public Keys? U43;
        public Keys? U44;
        public float? U45;
        public float? U46;
        public float? U47;
        public float? U48;
        public Keys? U49;
        public float? U50;
        public CPlugDynaWaterModel? U51;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version == 0)
            {
                rw.Boolean(ref U01);
                rw.Single(ref U02);
                rw.Single(ref U03);
                rw.Single(ref U04);
                rw.Single(ref U05);
                rw.Boolean(ref U06);
                rw.Single(ref U07);
                rw.Single(ref U08);
            }
            if (Version >= 1)
            {
                if (Version <= 20)
                {
                    rw.ReadableWritable<Keys>(ref U09, version: Version - 21);
                    rw.Single(ref U10);
                    rw.ReadableWritable<Keys>(ref U11, version: Version - 21);
                    rw.Single(ref U12);
                    rw.Single(ref U13);
                }
                rw.Single(ref U14);
                rw.Single(ref U15);
                rw.ReadableWritable<Keys>(ref U16, version: Version - 21);
                rw.ReadableWritable<Keys>(ref U17, version: Version - 21);
            }
            if (Version >= 2)
            {
                if (Version <= 20)
                {
                    rw.ReadableWritable<Keys>(ref U18, version: Version - 21);
                }
            }
            if (Version >= 3)
            {
                rw.ReadableWritable<Keys>(ref U19, version: Version - 21);
                rw.ReadableWritable<Keys>(ref U20, version: Version - 21);
            }
            if (Version >= 4)
            {
                rw.Single(ref U21);
                rw.Single(ref U22);
            }
            if (Version >= 5)
            {
                rw.Single(ref U23);
                rw.ReadableWritable<Keys>(ref U24, version: Version - 21);
                rw.ReadableWritable<Keys>(ref U25, version: Version - 21);
                rw.ReadableWritable<Keys>(ref U26, version: Version - 21);
                if (Version <= 20)
                {
                    rw.ReadableWritable<Keys>(ref U27, version: Version - 21);
                }
            }
            if (Version >= 6)
            {
                rw.ReadableWritable<Keys>(ref U28, version: Version - 21);
            }
            if (Version <= 7)
            {
                rw.ReadableWritable<Keys>(ref U29, version: Version - 21);
                rw.ReadableWritable<Keys>(ref U30, version: Version - 21);
                rw.ReadableWritable<Keys>(ref U31, version: Version - 21);
                rw.ReadableWritable<Keys>(ref U32, version: Version - 21);
                rw.ReadableWritable<Keys>(ref U33, version: Version - 21);
                rw.ReadableWritable<Keys>(ref U34, version: Version - 21);
            }
            if (Version >= 8)
            {
                rw.Single(ref U35);
                rw.ReadableWritable<Keys>(ref U36, version: Version - 21);
            }
            if (Version >= 9)
            {
                rw.Single(ref U37);
                rw.Single(ref U38);
                rw.Single(ref U39);
            }
            if (Version >= 10)
            {
                rw.ReadableWritable<Keys>(ref U40, version: Version - 21);
            }
            if (Version >= 11)
            {
                rw.Single(ref U41);
            }
            if (Version >= 12)
            {
                rw.ReadableWritable<Keys>(ref U42, version: Version - 21);
            }
            if (Version >= 13)
            {
                rw.ReadableWritable<Keys>(ref U43, version: Version - 21);
            }
            if (Version >= 14)
            {
                rw.ReadableWritable<Keys>(ref U44, version: Version - 21);
            }
            if (Version >= 15)
            {
                rw.Single(ref U45);
                rw.Single(ref U46);
            }
            if (Version >= 16)
            {
                rw.Single(ref U47);
            }
            if (Version >= 17)
            {
                rw.Single(ref U48);
            }
            if (Version >= 18)
            {
                rw.ReadableWritable<Keys>(ref U49, version: Version - 21);
            }
            if (Version >= 19)
            {
                rw.Single(ref U50);
            }
            if (Version >= 20)
            {
                rw.NodeRef<CPlugDynaWaterModel>(ref U51);
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x09E chunk
    /// </summary>
    [Chunk(0x090ED09E)]
    public partial class Chunk090ED09E : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED09E;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public float? U04;
        public int? U05;
        public float? U06;
        public float? U07;
        public float? U08;
        public Keys? U09;
        public float? U10;
        public Keys? U11;
        public float? U12;
        public float? U13;
        public float? U14;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            if (Version >= 1)
            {
                rw.Single(ref U04);
                if (Version >= 2)
                {
                    rw.Int32(ref U05);
                    rw.Single(ref U06);
                    if (Version >= 3)
                    {
                        rw.Single(ref U07);
                        if (Version >= 4)
                        {
                            rw.Single(ref U08);
                            rw.ReadableWritable<Keys>(ref U09, version: Version - 8);
                            if (Version <= 6)
                            {
                                rw.Single(ref U10);
                            }
                            if (Version >= 7)
                            {
                                rw.ReadableWritable<Keys>(ref U11);
                            }
                            if (Version >= 5)
                            {
                                rw.Single(ref U12);
                                if (Version >= 6)
                                {
                                    rw.Single(ref U13);
                                    rw.Single(ref U14);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x09F chunk
    /// </summary>
    [Chunk(0x090ED09F)]
    public partial class Chunk090ED09F : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED09F;

        public int Version { get; set; }

        public Keys? U01;
        public float U02;
        public float U03;
        public float U04;
        public float? U05;
        public Keys? U06;
        public Keys? U07;
        public float? U08;
        public float? U09;
        public float? U10;
        public float? U11;
        public float? U12;
        public float? U13;
        public float? U14;
        public Keys? U15;
        public float? U16;
        public Keys? U17;
        public Keys? U18;
        public float? U19;
        public float? U20;
        public float? U21;
        public float? U22;
        public float? U23;
        public Vec3? U24;
        public Vec3? U25;
        public Keys? U26;
        public Keys? U27;
        public bool? U28;
        public int? U29;
        public int? U30;
        public int? U31;
        public int? U32;
        public int? U33;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ReadableWritable<Keys>(ref U01, version: Version - 14);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            if (Version >= 1)
            {
                rw.Single(ref U05);
                if (Version >= 2)
                {
                    rw.ReadableWritable<Keys>(ref U06, version: Version - 14);
                    if (Version >= 3)
                    {
                        rw.ReadableWritable<Keys>(ref U07, version: Version - 14);
                        if (Version >= 4)
                        {
                            rw.Single(ref U08);
                            rw.Single(ref U09);
                            rw.Single(ref U10);
                            rw.Single(ref U11);
                            rw.Single(ref U12);
                            rw.Single(ref U13);
                            if (Version >= 5)
                            {
                                rw.Single(ref U14);
                                if (Version >= 6)
                                {
                                    rw.ReadableWritable<Keys>(ref U15, version: Version - 14);
                                    if (Version >= 7)
                                    {
                                        rw.Single(ref U16);
                                        rw.ReadableWritable<Keys>(ref U17, version: Version - 14);
                                        rw.ReadableWritable<Keys>(ref U18, version: Version - 14);
                                        rw.Single(ref U19);
                                        if (Version >= 8)
                                        {
                                            rw.Single(ref U20);
                                            rw.Single(ref U21);
                                            rw.Single(ref U22);
                                            rw.Single(ref U23);
                                            if (Version >= 9)
                                            {
                                                rw.Vec3(ref U24);
                                                rw.Vec3(ref U25);
                                                if (Version >= 10)
                                                {
                                                    rw.ReadableWritable<Keys>(ref U26, version: Version - 14);
                                                    if (Version >= 11)
                                                    {
                                                        rw.ReadableWritable<Keys>(ref U27, version: Version - 14);
                                                        if (Version >= 12)
                                                        {
                                                            rw.Boolean(ref U28);
                                                            if (Version >= 13)
                                                            {
                                                                rw.Int32(ref U29);
                                                                rw.Int32(ref U30);
                                                                rw.Int32(ref U31);
                                                                rw.Int32(ref U32);
                                                                rw.Int32(ref U33);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x0A0 chunk
    /// </summary>
    [Chunk(0x090ED0A0)]
    public partial class Chunk090ED0A0 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED0A0;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 1)
            {
                rw.Single(ref U01);
                rw.Single(ref U02);
                rw.Single(ref U03);
                rw.Single(ref U04);
                rw.Single(ref U05);
                if (Version >= 2)
                {
                    rw.Single(ref U06);
                    rw.Single(ref U07);
                    rw.Single(ref U08);
                    rw.Single(ref U09);
                    rw.Single(ref U10);
                }
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x0A1 chunk
    /// </summary>
    [Chunk(0x090ED0A1)]
    public partial class Chunk090ED0A1 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED0A1;

        public int Version { get; set; }

        public Keys? U01;
        public Keys? U02;
        public float U03;
        public float U04;
        public Keys? U05;
        public Keys? U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;
        public float U11;
        public Keys? U12;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ReadableWritable<Keys>(ref U01);
            rw.ReadableWritable<Keys>(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.ReadableWritable<Keys>(ref U05);
            rw.ReadableWritable<Keys>(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);
            rw.Single(ref U10);
            rw.Single(ref U11);
            rw.ReadableWritable<Keys>(ref U12);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x0A2 chunk
    /// </summary>
    [Chunk(0x090ED0A2)]
    public partial class Chunk090ED0A2 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED0A2;

        public int Version { get; set; }

        public Keys? U01;
        public Keys? U02;
        public Keys? U03;
        public Keys? U04;
        public Keys? U05;
        public Keys? U06;
        public Keys? U07;
        public Keys? U08;
        public Keys? U09;
        public Keys? U10;
        public Keys? U11;
        public Keys? U12;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ReadableWritable<Keys>(ref U01, version: Version - 2);
            rw.ReadableWritable<Keys>(ref U02, version: Version - 2);
            rw.ReadableWritable<Keys>(ref U03, version: Version - 2);
            rw.ReadableWritable<Keys>(ref U04, version: Version - 2);
            rw.ReadableWritable<Keys>(ref U05, version: Version - 2);
            rw.ReadableWritable<Keys>(ref U06, version: Version - 2);
            rw.ReadableWritable<Keys>(ref U07, version: Version - 2);
            rw.ReadableWritable<Keys>(ref U08, version: Version - 2);
            rw.ReadableWritable<Keys>(ref U09, version: Version - 2);
            rw.ReadableWritable<Keys>(ref U10, version: Version - 2);
            rw.ReadableWritable<Keys>(ref U11, version: Version - 2);
            rw.ReadableWritable<Keys>(ref U12, version: Version - 2);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x0A3 chunk
    /// </summary>
    [Chunk(0x090ED0A3)]
    public partial class Chunk090ED0A3 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED0A3;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public int U03;
        public int U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;
        public CFuncKeysReal? U11;
        public float U12;
        public int U13;
        public int U14;
        public int U15;
        public int U16;
        public CFuncKeysReal? U17;
        public CFuncKeysReal? U18;
        public Keys? U19;
        public Keys? U20;
        public int U21;
        public int U22;
        public int U23;
        public float U24;
        public int U25;
        public int U26;
        public float U27;
        public Keys? U28;
        public Keys? U29;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            if (Version >= 1)
            {
                rw.Single(ref U05);
                if (Version >= 2)
                {
                    rw.Single(ref U06);
                    rw.Single(ref U07);
                    rw.Single(ref U08);
                    rw.Single(ref U09);
                    if (Version >= 3)
                    {
                        rw.Single(ref U10);
                        if (Version >= 4)
                        {
                            rw.NodeRef<CFuncKeysReal>(ref U11);
                            rw.Single(ref U12);
                            rw.Int32(ref U13);
                            rw.Int32(ref U14);
                            rw.Int32(ref U15);
                            if (Version >= 5)
                            {
                                rw.Int32(ref U16);
                                rw.NodeRef<CFuncKeysReal>(ref U17);
                                rw.NodeRef<CFuncKeysReal>(ref U18);
                                rw.ReadableWritable<Keys>(ref U19, version: Version - 13);
                                rw.ReadableWritable<Keys>(ref U20, version: Version - 13);
                                if (Version >= 6)
                                {
                                    rw.Int32(ref U21);
                                    if (Version >= 7)
                                    {
                                        rw.Int32(ref U22);
                                        if (Version >= 8)
                                        {
                                            rw.Int32(ref U23);
                                            if (Version >= 9)
                                            {
                                                rw.Single(ref U24);
                                                rw.Int32(ref U25);
                                                if (Version >= 10)
                                                {
                                                    rw.Int32(ref U26);
                                                    if (Version >= 11)
                                                    {
                                                        rw.Single(ref U27);
                                                        if (Version >= 12)
                                                        {
                                                            rw.ReadableWritable<Keys>(ref U28, version: Version - 13);
                                                            rw.ReadableWritable<Keys>(ref U29, version: Version - 13);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x0A4 chunk
    /// </summary>
    [Chunk(0x090ED0A4)]
    public partial class Chunk090ED0A4 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED0A4;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public int U03;
        public int U04;
        public Keys? U05;
        public float U06;
        public float U07;
        public float U08;
        public int U09;
        public float U10;
        public Keys? U11;
        public Keys? U12;
        public Keys? U13;
        public int U14;
        public Keys? U15;
        public Keys? U16;
        public int U17;
        public int U18;
        public int U19;
        public int U20;
        public int U21;
        public Keys? U22;
        public int U23;
        public int U24;
        public float U25;
        public int U26;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            if (Version >= 1)
            {
                rw.Single(ref U02);
            }
            if (Version >= 2)
            {
                rw.Int32(ref U03);
            }
            if (Version >= 3)
            {
                rw.Int32(ref U04);
                rw.ReadableWritable<Keys>(ref U05, version: Version - 14);
                rw.Single(ref U06);
                rw.Single(ref U07);
            }
            if (Version >= 4)
            {
                rw.Single(ref U08);
                rw.Int32(ref U09);
                rw.Single(ref U10);
            }
            if (Version >= 5)
            {
                rw.ReadableWritable<Keys>(ref U11, version: Version - 14);
            }
            if (Version >= 6)
            {
                rw.ReadableWritable<Keys>(ref U12, version: Version - 14);
            }
            if (Version >= 7)
            {
                rw.ReadableWritable<Keys>(ref U13, version: Version - 14);
                rw.Int32(ref U14);
            }
            if (Version >= 8)
            {
                rw.ReadableWritable<Keys>(ref U15, version: Version - 14);
                rw.ReadableWritable<Keys>(ref U16, version: Version - 14);
            }
            if (Version >= 9)
            {
                rw.Int32(ref U17);
            }
            if (Version >= 10)
            {
                rw.Int32(ref U18);
            }
            if (Version >= 11)
            {
                if (Version <= 17)
                {
                    rw.Int32(ref U19);
                    rw.Int32(ref U20);
                    rw.Int32(ref U21);
                }
            }
            if (Version >= 12)
            {
                rw.ReadableWritable<Keys>(ref U22, version: Version - 14);
            }
            if (Version >= 13)
            {
                if (Version <= 17)
                {
                    rw.Int32(ref U23);
                }
            }
            if (Version >= 15)
            {
                rw.Int32(ref U24);
            }
            if (Version >= 16)
            {
                rw.Single(ref U25);
            }
            if (Version >= 18)
            {
                rw.Int32(ref U26);
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x0A5 chunk
    /// </summary>
    [Chunk(0x090ED0A5)]
    public partial class Chunk090ED0A5 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED0A5;

        public int Version { get; set; }

        public int U01;
        public int U02;
        public int U03;
        public int U04;
        public int U05;
        public int U06;
        public int U07;
        public int U08;
        public float U09;
        public float U10;
        public float U11;
        public float U12;
        public float U13;
        public float U14;
        public Keys? U15;
        public Keys? U16;
        public Keys? U17;
        public Keys? U18;
        public Keys? U19;
        public Keys? U20;
        public Keys? U21;
        public Keys? U22;
        public Keys? U23;
        public Keys? U24;
        public Keys? U25;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version <= 1)
            {
                rw.Int32(ref U01);
            }
            rw.Int32(ref U02);
            if (Version <= 1)
            {
                rw.Int32(ref U03);
            }
            rw.Int32(ref U04);
            if (Version <= 1)
            {
                rw.Int32(ref U05);
                rw.Int32(ref U06);
                rw.Int32(ref U07);
                rw.Int32(ref U08);
            }
            rw.Single(ref U09);
            rw.Single(ref U10);
            rw.Single(ref U11);
            rw.Single(ref U12);
            rw.Single(ref U13);
            rw.Single(ref U14);
            if (Version <= 1)
            {
                rw.ReadableWritable<Keys>(ref U15, version: Version - 1);
                rw.ReadableWritable<Keys>(ref U16, version: Version - 1);
                rw.ReadableWritable<Keys>(ref U17, version: Version - 1);
                rw.ReadableWritable<Keys>(ref U18, version: Version - 1);
                rw.ReadableWritable<Keys>(ref U19, version: Version - 1);
                rw.ReadableWritable<Keys>(ref U20, version: Version - 1);
            }
            rw.ReadableWritable<Keys>(ref U21);
            if (Version <= 1)
            {
                rw.ReadableWritable<Keys>(ref U22, version: Version - 1);
                rw.ReadableWritable<Keys>(ref U23, version: Version - 1);
                rw.ReadableWritable<Keys>(ref U24, version: Version - 1);
            }
            rw.ReadableWritable<Keys>(ref U25, version: Version - 1);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x0A6 chunk
    /// </summary>
    [Chunk(0x090ED0A6)]
    public partial class Chunk090ED0A6 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED0A6;

        public int Version { get; set; }

        public Keys? U01;
        public Keys? U02;
        public Keys? U03;
        public int U04;
        public int U05;
        public int U06;
        public float U07;
        public Keys? U08;
        public Keys? U09;
        public float U10;
        public Keys? U11;
        public float U12;
        public Keys? U13;
        public Keys? U14;
        public int U15;
        public int U16;
        public Keys? U17;
        public Keys? U18;
        public Keys? U19;
        public Keys? U20;
        public Keys? U21;
        public Keys? U22;
        public int U23;
        public int U24;
        public int U25;
        public int U26;
        public int U27;
        public Keys? U28;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ReadableWritable<Keys>(ref U01, version: Version - 11);
            rw.ReadableWritable<Keys>(ref U02, version: Version - 11);
            rw.ReadableWritable<Keys>(ref U03, version: Version - 11);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
            if (Version >= 1)
            {
                rw.Single(ref U07);
                rw.ReadableWritable<Keys>(ref U08, version: Version - 11);
                rw.ReadableWritable<Keys>(ref U09, version: Version - 11);
                rw.Single(ref U10);
                rw.ReadableWritable<Keys>(ref U11, version: Version - 11);
            }
            if (Version >= 2)
            {
                rw.Single(ref U12);
            }
            if (Version >= 3)
            {
                rw.ReadableWritable<Keys>(ref U13, version: Version - 11);
            }
            if (Version >= 4)
            {
                rw.ReadableWritable<Keys>(ref U14, version: Version - 11);
            }
            if (Version >= 5)
            {
                rw.Int32(ref U15);
                rw.Int32(ref U16);
            }
            if (Version >= 6)
            {
                if (Version <= 13)
                {
                    rw.ReadableWritable<Keys>(ref U17, version: Version - 11);
                }
            }
            if (Version >= 7)
            {
                if (Version <= 13)
                {
                    rw.ReadableWritable<Keys>(ref U18, version: Version - 11);
                }
            }
            if (Version >= 8)
            {
                rw.ReadableWritable<Keys>(ref U19, version: Version - 11);
                rw.ReadableWritable<Keys>(ref U20, version: Version - 11);
            }
            if (Version >= 9)
            {
                rw.ReadableWritable<Keys>(ref U21, version: Version - 11);
            }
            if (Version >= 10)
            {
                rw.ReadableWritable<Keys>(ref U22, version: Version - 11);
            }
            if (Version >= 12)
            {
                if (Version <= 13)
                {
                    rw.Int32(ref U23);
                    rw.Int32(ref U24);
                }
            }
            if (Version >= 13)
            {
                rw.Int32(ref U25);
                rw.Int32(ref U26);
            }
            if (Version == 14)
            {
                rw.Int32(ref U27);
            }
            if (Version >= 15)
            {
                rw.ReadableWritable<Keys>(ref U28);
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x0A7 chunk
    /// </summary>
    [Chunk(0x090ED0A7)]
    public partial class Chunk090ED0A7 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED0A7;

        public int Version { get; set; }

        public int U01;
        public int U02;
        public int U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public Keys? U08;
        public Keys? U09;
        public int U10;
        public int U11;
        public Keys? U12;
        public int U13;
        public float U14;
        public float U15;
        public int U16;
        public int U17;
        public int U18;
        public int U19;
        public CFuncKeysReal? U20;
        public int U21;
        public Keys? U22;
        public Keys? U23;
        public Keys? U24;
        public int U25;
        public float U26;
        public Keys? U27;
        public Keys? U28;
        public float U29;
        public float U30;
        public Keys? U31;
        public Keys? U32;
        public Keys? U33;
        public int U34;
        public Keys? U35;
        public Keys? U36;
        public float U37;
        public float U38;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.ReadableWritable<Keys>(ref U08, version: Version - 19);
            rw.ReadableWritable<Keys>(ref U09, version: Version - 19);
            if (Version >= 1)
            {
                rw.Int32(ref U10);
            }
            if (Version >= 2)
            {
                rw.Int32(ref U11);
                rw.ReadableWritable<Keys>(ref U12, version: Version - 19);
            }
            if (Version >= 3)
            {
                rw.Int32(ref U13);
            }
            if (Version >= 4)
            {
                rw.Single(ref U14);
                rw.Single(ref U15);
                rw.Int32(ref U16);
                rw.Int32(ref U17);
            }
            if (Version >= 5)
            {
                rw.Int32(ref U18);
                rw.Int32(ref U19);
            }
            if (Version >= 6)
            {
                rw.NodeRef<CFuncKeysReal>(ref U20);
            }
            if (Version >= 7)
            {
                rw.Int32(ref U21);
            }
            if (Version >= 8)
            {
                rw.ReadableWritable<Keys>(ref U22, version: Version - 19);
            }
            if (Version >= 9)
            {
                rw.ReadableWritable<Keys>(ref U23, version: Version - 19);
                rw.ReadableWritable<Keys>(ref U24, version: Version - 19);
                rw.Int32(ref U25);
            }
            if (Version >= 10)
            {
                rw.Single(ref U26);
                rw.ReadableWritable<Keys>(ref U27, version: Version - 19);
            }
            if (Version >= 11)
            {
                rw.ReadableWritable<Keys>(ref U28, version: Version - 19);
            }
            if (Version >= 12)
            {
                rw.Single(ref U29);
                rw.Single(ref U30);
            }
            if (Version >= 13)
            {
                rw.ReadableWritable<Keys>(ref U31, version: Version - 19);
            }
            if (Version >= 14)
            {
                rw.ReadableWritable<Keys>(ref U32, version: Version - 19);
                rw.ReadableWritable<Keys>(ref U33, version: Version - 19);
            }
            if (Version >= 15)
            {
                rw.Int32(ref U34);
            }
            if (Version >= 16)
            {
                rw.ReadableWritable<Keys>(ref U35, version: Version - 19);
            }
            if (Version >= 17)
            {
                rw.ReadableWritable<Keys>(ref U36, version: Version - 19);
            }
            if (Version >= 18)
            {
                rw.Single(ref U37);
                rw.Single(ref U38);
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x0A8 chunk
    /// </summary>
    [Chunk(0x090ED0A8)]
    public partial class Chunk090ED0A8 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED0A8;

        public int Version { get; set; }

        public int U01;
        public Keys? U02;
        public Keys? U03;
        public Keys? U04;
        public Keys? U05;
        public Keys? U06;
        public int U07;
        public int U08;
        public float U09;
        public Keys? U10;
        public Keys? U11;
        public float U12;
        public float U13;
        public Keys? U14;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.ReadableWritable<Keys>(ref U02, version: Version - 10);
            if (Version >= 1)
            {
                rw.ReadableWritable<Keys>(ref U03, version: Version - 10);
                rw.ReadableWritable<Keys>(ref U04, version: Version - 10);
            }
            if (Version >= 2)
            {
                rw.ReadableWritable<Keys>(ref U05, version: Version - 10);
            }
            if (Version >= 3)
            {
                rw.ReadableWritable<Keys>(ref U06, version: Version - 10);
            }
            if (Version >= 4)
            {
                rw.Int32(ref U07);
            }
            if (Version >= 5)
            {
                rw.Int32(ref U08);
                rw.Single(ref U09);
            }
            if (Version >= 6)
            {
                rw.ReadableWritable<Keys>(ref U10, version: Version - 10);
            }
            if (Version >= 7)
            {
                rw.ReadableWritable<Keys>(ref U11, version: Version - 10);
            }
            if (Version >= 8)
            {
                rw.Single(ref U12);
                rw.Single(ref U13);
            }
            if (Version >= 9)
            {
                rw.ReadableWritable<Keys>(ref U14, version: Version - 10);
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x0A9 chunk
    /// </summary>
    [Chunk(0x090ED0A9)]
    public partial class Chunk090ED0A9 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED0A9;

        public int Version { get; set; }

        public int U01;
        public int U02;
        public Keys? U03;
        public Keys? U04;
        public int U05;
        public int U06;
        public float U07;
        public Keys? U08;
        public Keys? U09;
        public Keys? U10;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.ReadableWritable<Keys>(ref U03, version: Version - 4);
            rw.ReadableWritable<Keys>(ref U04, version: Version - 4);
            if (Version >= 1)
            {
                rw.Int32(ref U05);
            }
            if (Version >= 2)
            {
                rw.Int32(ref U06);
                rw.Single(ref U07);
                rw.ReadableWritable<Keys>(ref U08, version: Version - 4);
                rw.ReadableWritable<Keys>(ref U09, version: Version - 4);
            }
            if (Version >= 3)
            {
                rw.ReadableWritable<Keys>(ref U10, version: Version - 4);
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x0AA chunk
    /// </summary>
    [Chunk(0x090ED0AA)]
    public partial class Chunk090ED0AA : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED0AA;

        public int Version { get; set; }

        public float U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x0AB chunk
    /// </summary>
    [Chunk(0x090ED0AB)]
    public partial class Chunk090ED0AB : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED0AB;

        public int Version { get; set; }

        public int U01;
        public int U02;
        public float U03;
        public Keys? U04;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Single(ref U03);
            rw.ReadableWritable<Keys>(ref U04, version: Version - 1);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x0AC chunk
    /// </summary>
    [Chunk(0x090ED0AC)]
    public partial class Chunk090ED0AC : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED0AC;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x0AD chunk
    /// </summary>
    [Chunk(0x090ED0AD)]
    public partial class Chunk090ED0AD : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED0AD;

        public int Version { get; set; }

        public Keys? U01;
        public int U02;
        public float U03;
        public int U04;
        public CFuncKeysReal? U05;
        public int U06;
        public int U07;
        public int U08;
        public int U09;
        public CFuncKeysReal? U10;
        public Keys? U11;
        public int U12;
        public int U13;
        public CFuncKeysReal? U14;
        public Keys? U15;
        public CFuncKeysReal? U16;
        public Keys? U17;
        public Keys? U18;
        public Keys? U19;
        public Keys? U20;
        public int U21;
        public int U22;
        public int U23;
        public Keys? U24;
        public Keys? U25;
        public Keys? U26;
        public float U27;
        public float U28;
        public Keys? U29;
        public CFuncKeysReal? U30;
        public CFuncKeysReal? U31;
        public Keys? U32;
        public float U33;
        public int U34;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ReadableWritable<Keys>(ref U01, version: Version - 10);
            rw.Int32(ref U02);
            rw.Single(ref U03);
            rw.Int32(ref U04);
            rw.NodeRef<CFuncKeysReal>(ref U05);
            rw.Int32(ref U06);
            rw.Int32(ref U07);
            rw.Int32(ref U08);
            rw.Int32(ref U09);
            rw.NodeRef<CFuncKeysReal>(ref U10);
            rw.ReadableWritable<Keys>(ref U11, version: Version - 10);
            rw.Int32(ref U12);
            rw.Int32(ref U13);
            rw.NodeRef<CFuncKeysReal>(ref U14);
            rw.ReadableWritable<Keys>(ref U15, version: Version - 10);
            rw.NodeRef<CFuncKeysReal>(ref U16);
            if (Version >= 1)
            {
                rw.ReadableWritable<Keys>(ref U17, version: Version - 10);
                rw.ReadableWritable<Keys>(ref U18, version: Version - 10);
                rw.ReadableWritable<Keys>(ref U19, version: Version - 10);
                rw.ReadableWritable<Keys>(ref U20, version: Version - 10);
            }
            if (Version >= 2)
            {
                rw.Int32(ref U21);
                rw.Int32(ref U22);
                rw.Int32(ref U23);
            }
            if (Version >= 3)
            {
                rw.ReadableWritable<Keys>(ref U24, version: Version - 10);
                rw.ReadableWritable<Keys>(ref U25, version: Version - 10);
            }
            if (Version >= 4)
            {
                rw.ReadableWritable<Keys>(ref U26, version: Version - 10);
                rw.Single(ref U27);
            }
            if (Version >= 5)
            {
                rw.Single(ref U28);
            }
            if (Version >= 6)
            {
                rw.ReadableWritable<Keys>(ref U29, version: Version - 10);
            }
            if (Version >= 7)
            {
                rw.NodeRef<CFuncKeysReal>(ref U30);
                rw.NodeRef<CFuncKeysReal>(ref U31);
            }
            if (Version >= 8)
            {
                rw.ReadableWritable<Keys>(ref U32, version: Version - 10);
            }
            if (Version >= 9)
            {
                rw.Single(ref U33);
            }
            if (Version >= 11)
            {
                rw.Int32(ref U34);
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x0AE chunk
    /// </summary>
    [Chunk(0x090ED0AE)]
    public partial class Chunk090ED0AE : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED0AE;

        public int Version { get; set; }

        public int U01;
        public Keys? U02;
        public float U03;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.ReadableWritable<Keys>(ref U02, version: Version - 2);
            if (Version >= 1)
            {
                rw.Single(ref U03);
            }
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x0AF chunk
    /// </summary>
    [Chunk(0x090ED0AF)]
    public partial class Chunk090ED0AF : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED0AF;

        public int Version { get; set; }

        public Keys? U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public Keys? U06;
        public Keys? U07;
        public Keys? U08;
        public Keys? U09;
        public float U10;
        public float U11;
        public float U12;
        public float U13;
        public float U14;
        public Keys? U15;
        public float U16;
        public float U17;
        public float U18;
        public Keys? U19;
        public Keys? U20;
        public float U21;
        public Keys? U22;
        public float U23;
        public float U24;
        public Keys? U25;
        public Keys? U26;
        public Keys? U27;
        public float U28;
        public float U29;
        public float U30;
        public Keys? U31;
        public Keys? U32;
        public Keys? U33;
        public Keys? U34;
        public float U35;
        public Keys? U36;
        public Keys? U37;
        public float U38;
        public float U39;
        public Keys? U40;
        public Keys? U41;
        public Keys? U42;
        public float U43;
        public float U44;
        public float U45;
        public float U46;
        public float U47;
        public float U48;
        public float U49;
        public float U50;
        public float U51;
        public int U52;
        public float U53;
        public float U54;
        public int U55;
        public float U56;
        public float U57;
        public float U58;
        public float U59;
        public Keys? U60;
        public Keys? U61;
        public float U62;
        public float U63;
        public float U64;
        public float U65;
        public float U66;
        public Keys? U67;
        public float U68;
        public float U69;
        public Keys? U70;
        public int U71;
        public Keys? U72;
        public Keys? U73;
        public float U74;
        public Keys? U75;
        public Keys? U76;
        public Keys? U77;
        public Keys? U78;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ReadableWritable<Keys>(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.ReadableWritable<Keys>(ref U06);
            rw.ReadableWritable<Keys>(ref U07);
            rw.ReadableWritable<Keys>(ref U08);
            rw.ReadableWritable<Keys>(ref U09);
            rw.Single(ref U10);
            rw.Single(ref U11);
            rw.Single(ref U12);
            rw.Single(ref U13);
            rw.Single(ref U14);
            rw.ReadableWritable<Keys>(ref U15);
            rw.Single(ref U16);
            rw.Single(ref U17);
            rw.Single(ref U18);
            rw.ReadableWritable<Keys>(ref U19);
            rw.ReadableWritable<Keys>(ref U20);
            rw.Single(ref U21);
            rw.ReadableWritable<Keys>(ref U22);
            rw.Single(ref U23);
            rw.Single(ref U24);
            rw.ReadableWritable<Keys>(ref U25);
            rw.ReadableWritable<Keys>(ref U26);
            rw.ReadableWritable<Keys>(ref U27);
            rw.Single(ref U28);
            rw.Single(ref U29);
            rw.Single(ref U30);
            rw.ReadableWritable<Keys>(ref U31);
            rw.ReadableWritable<Keys>(ref U32);
            rw.ReadableWritable<Keys>(ref U33);
            rw.ReadableWritable<Keys>(ref U34);
            rw.Single(ref U35);
            rw.ReadableWritable<Keys>(ref U36);
            rw.ReadableWritable<Keys>(ref U37);
            rw.Single(ref U38);
            rw.Single(ref U39);
            rw.ReadableWritable<Keys>(ref U40);
            rw.ReadableWritable<Keys>(ref U41);
            rw.ReadableWritable<Keys>(ref U42);
            rw.Single(ref U43);
            rw.Single(ref U44);
            rw.Single(ref U45);
            rw.Single(ref U46);
            rw.Single(ref U47);
            rw.Single(ref U48);
            rw.Single(ref U49);
            rw.Single(ref U50);
            rw.Single(ref U51);
            rw.Int32(ref U52);
            rw.Single(ref U53);
            rw.Single(ref U54);
            rw.Int32(ref U55);
            rw.Single(ref U56);
            rw.Single(ref U57);
            rw.Single(ref U58);
            rw.Single(ref U59);
            rw.ReadableWritable<Keys>(ref U60);
            rw.ReadableWritable<Keys>(ref U61);
            rw.Single(ref U62);
            rw.Single(ref U63);
            rw.Single(ref U64);
            rw.Single(ref U65);
            rw.Single(ref U66);
            rw.ReadableWritable<Keys>(ref U67);
            rw.Single(ref U68);
            rw.Single(ref U69);
            rw.ReadableWritable<Keys>(ref U70);
            rw.Int32(ref U71);
            rw.ReadableWritable<Keys>(ref U72);
            rw.ReadableWritable<Keys>(ref U73);
            rw.Single(ref U74);
            rw.ReadableWritable<Keys>(ref U75);
            rw.ReadableWritable<Keys>(ref U76);
            rw.ReadableWritable<Keys>(ref U77);
            rw.ReadableWritable<Keys>(ref U78);
        }
    }

    /// <summary>
    /// CPlugVehicleCarPhyTuning 0x0B0 chunk
    /// </summary>
    [Chunk(0x090ED0B0)]
    public partial class Chunk090ED0B0 : Chunk<CPlugVehicleCarPhyTuning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090ED0B0;

        public int Version { get; set; }

        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCarPhyTuning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            if (Version >= 1)
            {
                rw.Single(ref U02);
            }
        }
    }


    public sealed partial class Keys : IReadableWritable
    {
    }


    public enum ESteerModel
    {
        Steer01,
        Steer02,
        Steer03,
        Steer04,
        Steer05,
        Steer06,
    }

    public enum EShockModel
    {
        Demo01,
        Demo02,
        Demo03,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090ED000 => new Chunk090ED000(),
        0x090ED001 => new Chunk090ED001(),
        0x090ED002 => new Chunk090ED002(),
        0x090ED004 => new Chunk090ED004(),
        0x090ED005 => new Chunk090ED005(),
        0x090ED006 => new Chunk090ED006(),
        0x090ED007 => new Chunk090ED007(),
        0x090ED008 => new Chunk090ED008(),
        0x090ED009 => new Chunk090ED009(),
        0x090ED00A => new Chunk090ED00A(),
        0x090ED00B => new Chunk090ED00B(),
        0x090ED00C => new Chunk090ED00C(),
        0x090ED00D => new Chunk090ED00D(),
        0x090ED00E => new Chunk090ED00E(),
        0x090ED010 => new Chunk090ED010(),
        0x090ED011 => new Chunk090ED011(),
        0x090ED012 => new Chunk090ED012(),
        0x090ED013 => new Chunk090ED013(),
        0x090ED014 => new Chunk090ED014(),
        0x090ED015 => new Chunk090ED015(),
        0x090ED016 => new Chunk090ED016(),
        0x090ED017 => new Chunk090ED017(),
        0x090ED018 => new Chunk090ED018(),
        0x090ED019 => new Chunk090ED019(),
        0x090ED01A => new Chunk090ED01A(),
        0x090ED01B => new Chunk090ED01B(),
        0x090ED01D => new Chunk090ED01D(),
        0x090ED01E => new Chunk090ED01E(),
        0x090ED01F => new Chunk090ED01F(),
        0x090ED020 => new Chunk090ED020(),
        0x090ED021 => new Chunk090ED021(),
        0x090ED022 => new Chunk090ED022(),
        0x090ED023 => new Chunk090ED023(),
        0x090ED024 => new Chunk090ED024(),
        0x090ED026 => new Chunk090ED026(),
        0x090ED027 => new Chunk090ED027(),
        0x090ED028 => new Chunk090ED028(),
        0x090ED029 => new Chunk090ED029(),
        0x090ED02A => new Chunk090ED02A(),
        0x090ED02B => new Chunk090ED02B(),
        0x090ED02C => new Chunk090ED02C(),
        0x090ED02D => new Chunk090ED02D(),
        0x090ED02E => new Chunk090ED02E(),
        0x090ED02F => new Chunk090ED02F(),
        0x090ED030 => new Chunk090ED030(),
        0x090ED031 => new Chunk090ED031(),
        0x090ED032 => new Chunk090ED032(),
        0x090ED033 => new Chunk090ED033(),
        0x090ED034 => new Chunk090ED034(),
        0x090ED035 => new Chunk090ED035(),
        0x090ED036 => new Chunk090ED036(),
        0x090ED037 => new Chunk090ED037(),
        0x090ED038 => new Chunk090ED038(),
        0x090ED039 => new Chunk090ED039(),
        0x090ED03A => new Chunk090ED03A(),
        0x090ED03B => new Chunk090ED03B(),
        0x090ED03C => new Chunk090ED03C(),
        0x090ED03D => new Chunk090ED03D(),
        0x090ED03E => new Chunk090ED03E(),
        0x090ED03F => new Chunk090ED03F(),
        0x090ED040 => new Chunk090ED040(),
        0x090ED041 => new Chunk090ED041(),
        0x090ED042 => new Chunk090ED042(),
        0x090ED043 => new Chunk090ED043(),
        0x090ED044 => new Chunk090ED044(),
        0x090ED045 => new Chunk090ED045(),
        0x090ED046 => new Chunk090ED046(),
        0x090ED047 => new Chunk090ED047(),
        0x090ED048 => new Chunk090ED048(),
        0x090ED049 => new Chunk090ED049(),
        0x090ED04A => new Chunk090ED04A(),
        0x090ED04D => new Chunk090ED04D(),
        0x090ED04E => new Chunk090ED04E(),
        0x090ED04F => new Chunk090ED04F(),
        0x090ED051 => new Chunk090ED051(),
        0x090ED052 => new Chunk090ED052(),
        0x090ED053 => new Chunk090ED053(),
        0x090ED056 => new Chunk090ED056(),
        0x090ED057 => new Chunk090ED057(),
        0x090ED058 => new Chunk090ED058(),
        0x090ED059 => new Chunk090ED059(),
        0x090ED05A => new Chunk090ED05A(),
        0x090ED05B => new Chunk090ED05B(),
        0x090ED05C => new Chunk090ED05C(),
        0x090ED05D => new Chunk090ED05D(),
        0x090ED05E => new Chunk090ED05E(),
        0x090ED05F => new Chunk090ED05F(),
        0x090ED060 => new Chunk090ED060(),
        0x090ED061 => new Chunk090ED061(),
        0x090ED062 => new Chunk090ED062(),
        0x090ED063 => new Chunk090ED063(),
        0x090ED064 => new Chunk090ED064(),
        0x090ED065 => new Chunk090ED065(),
        0x090ED066 => new Chunk090ED066(),
        0x090ED06A => new Chunk090ED06A(),
        0x090ED06E => new Chunk090ED06E(),
        0x090ED072 => new Chunk090ED072(),
        0x090ED077 => new Chunk090ED077(),
        0x090ED078 => new Chunk090ED078(),
        0x090ED079 => new Chunk090ED079(),
        0x090ED07D => new Chunk090ED07D(),
        0x090ED082 => new Chunk090ED082(),
        0x090ED084 => new Chunk090ED084(),
        0x090ED085 => new Chunk090ED085(),
        0x090ED086 => new Chunk090ED086(),
        0x090ED088 => new Chunk090ED088(),
        0x090ED089 => new Chunk090ED089(),
        0x090ED08A => new Chunk090ED08A(),
        0x090ED08B => new Chunk090ED08B(),
        0x090ED08C => new Chunk090ED08C(),
        0x090ED08D => new Chunk090ED08D(),
        0x090ED08E => new Chunk090ED08E(),
        0x090ED094 => new Chunk090ED094(),
        0x090ED095 => new Chunk090ED095(),
        0x090ED096 => new Chunk090ED096(),
        0x090ED097 => new Chunk090ED097(),
        0x090ED098 => new Chunk090ED098(),
        0x090ED099 => new Chunk090ED099(),
        0x090ED09A => new Chunk090ED09A(),
        0x090ED09B => new Chunk090ED09B(),
        0x090ED09C => new Chunk090ED09C(),
        0x090ED09D => new Chunk090ED09D(),
        0x090ED09E => new Chunk090ED09E(),
        0x090ED09F => new Chunk090ED09F(),
        0x090ED0A0 => new Chunk090ED0A0(),
        0x090ED0A1 => new Chunk090ED0A1(),
        0x090ED0A2 => new Chunk090ED0A2(),
        0x090ED0A3 => new Chunk090ED0A3(),
        0x090ED0A4 => new Chunk090ED0A4(),
        0x090ED0A5 => new Chunk090ED0A5(),
        0x090ED0A6 => new Chunk090ED0A6(),
        0x090ED0A7 => new Chunk090ED0A7(),
        0x090ED0A8 => new Chunk090ED0A8(),
        0x090ED0A9 => new Chunk090ED0A9(),
        0x090ED0AA => new Chunk090ED0AA(),
        0x090ED0AB => new Chunk090ED0AB(),
        0x090ED0AC => new Chunk090ED0AC(),
        0x090ED0AD => new Chunk090ED0AD(),
        0x090ED0AE => new Chunk090ED0AE(),
        0x090ED0AF => new Chunk090ED0AF(),
        0x090ED0B0 => new Chunk090ED0B0(),
        _ => base.NewChunk(chunkId),
    };
}
