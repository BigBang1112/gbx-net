namespace GBX.NET;

// Ref: https://next.openplanet.dev/Scene/CSceneVehicleVisState
public enum EPlugSurfaceMaterialId : byte
{
    Concrete,
    Pavement,
    Grass,
    Ice,
    Metal,
    Sand,
    Dirt,
    Turbo_Deprecated,
    DirtRoad,
    Rubber,
    SlidingRubber,
    Test,
    Rock,
    Water,
    Wood,
    Danger,
    Asphalt,
    WetDirtRoad,
    WetAsphalt,
    WetPavement,
    WetGrass,
    Snow,
    ResonantMetal,
    GolfBall,
    GolfWall,
    GolfGround,
    Turbo2_Deprecated,
    Bumper_Deprecated,
    NotCollidable,
    FreeWheeling_Deprecated,
    TurboRoulette_Deprecated,
    WallJump,
    MetalTrans,
    Stone,
    Player,
    Trunk,
    TechLaser,
    SlidingWood,
    PlayerOnly,
    Tech,
    TechArmor,
    TechSafe,
    OffZone,
    Bullet,
    TechHook,
    TechGround,
    TechWall,
    TechArrow,
    TechHook2,
    Forest,
    Wheat,
    TechTarget,
    PavementStair,
    TechTeleport,
    Energy,
    TechMagnetic,
    TurboTechMagnetic_Deprecated,
    Turbo2TechMagnetic_Deprecated,
    TurboWood_Deprecated,
    Turbo2Wood_Deprecated,
    FreeWheelingTechMagnetic_Deprecated,
    FreeWheelingWood_Deprecated,
    TechSuperMagnetic,
    TechNucleus,
    TechMagneticAccel,
    MetalFence,
    TechGravityChange,
    TechGravityReset,
    RubberBand,
    Gravel,
    Hack_NoGrip_Deprecated,
    Bumper2_Deprecated,
    NoSteering_Deprecated,
    NoBrakes_Deprecated,
    RoadIce,
    RoadSynthetic,
    Green,
    Plastic,
    DevDebug,
    Free3,
    XXX_Null
}

public enum ESceneVehicleVisReactorBoostType : int {
    None,
    Up,
    Down,
    UpAndDown
}

public enum ESceneVehicleVisReactorBoostLvl : int {
    None,
    Lvl1,
    Lvl2
}



public class Sample
{
    public TimeInt32? Timestamp { get; internal set; }

    public byte? BufferType { get; set; }
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

    public byte[] Data { get; set; }

    public float FLIcing { get; set; }
    public float FRIcing { get; set; }
    public float RLIcing { get; set; }
    public float RRIcing { get; set; }

    public float FLDirt { get; set; }
    public float FRDirt { get; set; }
    public float RLDirt { get; set; }
    public float RRDirt { get; set; }

    public EPlugSurfaceMaterialId FLGroundContactMaterial { get; set; }
    public EPlugSurfaceMaterialId FRGroundContactMaterial { get; set; }
    public EPlugSurfaceMaterialId RLGroundContactMaterial { get; set; }
    public EPlugSurfaceMaterialId RRGroundContactMaterial { get; set; }

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

    public ESceneVehicleVisReactorBoostLvl ReactorBoostLvl { get; set; }
    public ESceneVehicleVisReactorBoostType ReactorBoostType { get; set; }

    public int ReactorAirControlSteer { get; set; }
    public int ReactorAirControlPedal { get; set; }

    public bool IsTurbo { get; set; }
    public float TurboTime { get; set; }

    public float SimulationTimeCoef { get; set; }
    
    public float SideSpeed { get; set; }
    public bool IsTopContact { get; set; }

    public Sample(byte[] data)
    {
        Data = data;
    }

    public override string ToString()
    {
        if (!BufferType.HasValue || BufferType == 0 || BufferType == 2 || BufferType == 4)
        {
            if (Timestamp.HasValue)
                return $"Sample: {Timestamp.ToTmString()} {Position}";
            return $"Sample: {Position}";
        }

        return $"Sample: {BufferType.ToString() ?? "unknown"}";
    }
}
