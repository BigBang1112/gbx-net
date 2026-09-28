namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0900C000</remarks>
[Class(0x0900C000)]
public partial class CPlugSurface : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x0900C000;




    private CPlugSurfaceGeom? geom;
    [AppliedWithChunk<Chunk0900C000>]
    public CPlugSurfaceGeom? Geom { get => geom; set => geom = value; }


    /// <summary>
    /// CPlugSurface 0x000 chunk
    /// </summary>
    [Chunk(0x0900C000)]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk0900C000 : Chunk<CPlugSurface>
    {
        /// <inheritdoc />
        public override uint Id => 0x0900C000;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;


        public override void ReadWrite(CPlugSurface n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugSurfaceGeom>(ref n.geom);
            rw.ArrayReadableWritable<SurfMaterial>(ref n.materials!);
        }
    }

    /// <summary>
    /// CPlugSurface 0x003 chunk
    /// </summary>
    [Chunk(0x0900C003)]
    [ChunkGameVersion(GameVersion.TMT | GameVersion.MP4, 2, 2)]
    public partial class Chunk0900C003 : Chunk<CPlugSurface>
    {
        /// <inheritdoc />
        public override uint Id => 0x0900C003;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMT | GameVersion.MP4;

    }


    public sealed partial class Box : IReadable, IWritable
    {
        public BoxAligned Transform { get; set; }
        /// <summary>
        /// Only relevant for TM2+
        /// </summary>
        public short SurfaceIndex { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            Transform = r.ReadBoxAligned();
            if (v >= 1)
            {
                SurfaceIndex = r.ReadInt16(); // Only relevant for TM2+
            }
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(Transform);
            if (v >= 1)
            {
                w.Write(SurfaceIndex); // Only relevant for TM2+
            }
        }
    }

    public sealed partial class SphereLocated : IReadable, IWritable
    {
        public Vec3 Center { get; set; }
        public float Radius { get; set; }
        public short SurfaceIndex { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            Center = r.ReadVec3();
            Radius = r.ReadSingle();
            SurfaceIndex = r.ReadInt16();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(Center);
            w.Write(Radius);
            w.Write(SurfaceIndex);
        }
    }

    public sealed partial class SphericalShell : IReadable, IWritable
    {
        public float InnerRadius { get; set; }
        public float OuterRadius { get; set; }
        public bool SkipInToOut { get; set; }
        public short SurfaceIndex { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            InnerRadius = r.ReadSingle();
            OuterRadius = r.ReadSingle();
            SkipInToOut = r.ReadBoolean();
            SurfaceIndex = r.ReadInt16();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(InnerRadius);
            w.Write(OuterRadius);
            w.Write(SkipInToOut);
            w.Write(SurfaceIndex);
        }
    }

    public sealed partial class SurfMaterial : IReadableWritable
    {
    }

    public sealed partial class Cylinder : IReadable, IWritable
    {
        public float RadiusY { get; set; }
        public float RadiusXZ { get; set; }
        public short SurfaceIndex { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            RadiusY = r.ReadSingle();
            RadiusXZ = r.ReadSingle();
            SurfaceIndex = r.ReadInt16();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(RadiusY);
            w.Write(RadiusXZ);
            w.Write(SurfaceIndex);
        }
    }

    public sealed partial class Sphere : IReadable, IWritable
    {
        public float Size { get; set; }
        /// <summary>
        /// Only relevant for TM2+
        /// </summary>
        public short SurfaceIndex { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            Size = r.ReadSingle();
            if (v >= 1)
            {
                SurfaceIndex = r.ReadInt16(); // Only relevant for TM2+
            }
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(Size);
            if (v >= 1)
            {
                w.Write(SurfaceIndex); // Only relevant for TM2+
            }
        }
    }

    public sealed partial class Voxel : IReadable, IWritable
    {

        public void Read(GbxReader r, int v = 0)
        {
        }

        public void Write(GbxWriter w, int v = 0)
        {
        }
    }

    public sealed partial class Capsule : IReadable, IWritable
    {
        public Vec3 SphereCenter { get; set; }
        public Vec3 Dir { get; set; }
        /// <summary>
        /// The runtime Radius field is not archived.
        /// </summary>
        public float Length { get; set; }
        public short SurfaceIndex { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            SphereCenter = r.ReadVec3();
            Dir = r.ReadVec3();
            Length = r.ReadSingle(); // The runtime Radius field is not archived.
            SurfaceIndex = r.ReadInt16();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(SphereCenter);
            w.Write(Dir);
            w.Write(Length); // The runtime Radius field is not archived.
            w.Write(SurfaceIndex);
        }
    }

    public sealed partial class CompoundInstance : IReadable, IWritable
    {

        public void Read(GbxReader r, int v = 0)
        {
        }

        public void Write(GbxWriter w, int v = 0)
        {
        }
    }

    public sealed partial class Diggable : IReadable, IWritable
    {
        public BoxAligned Bounds { get; set; }
        public uint U01 { get; set; }
        public uint U02 { get; set; }
        public uint U03 { get; set; }
        public uint U04 { get; set; }
        public uint U05 { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            Bounds = r.ReadBoxAligned();
            U01 = r.ReadUInt32();
            U02 = r.ReadUInt32();
            U03 = r.ReadUInt32();
            U04 = r.ReadUInt32();
            U05 = r.ReadUInt32();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(Bounds);
            w.Write(U01);
            w.Write(U02);
            w.Write(U03);
            w.Write(U04);
            w.Write(U05);
        }
    }

    public sealed partial class Circle : IReadable, IWritable
    {
        public Vec3 Center { get; set; }
        public Vec3 Normal { get; set; }
        public float Radius { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            Center = r.ReadVec3();
            Normal = r.ReadVec3();
            Radius = r.ReadSingle();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(Center);
            w.Write(Normal);
            w.Write(Radius);
        }
    }

    public sealed partial class Ellipsoid : IReadable, IWritable
    {
        public Vec3 Size { get; set; }
        /// <summary>
        /// Only relevant for TM2+
        /// </summary>
        public short SurfaceIndex { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            Size = r.ReadVec3();
            if (v >= 1)
            {
                SurfaceIndex = r.ReadInt16(); // Only relevant for TM2+
            }
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(Size);
            if (v >= 1)
            {
                w.Write(SurfaceIndex); // Only relevant for TM2+
            }
        }
    }

    public sealed partial class VCylinder : IReadable, IWritable
    {
        public float Radius { get; set; }
        public float Height { get; set; }
        public short SurfaceIndex { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            Radius = r.ReadSingle();
            Height = r.ReadSingle();
            SurfaceIndex = r.ReadInt16();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(Radius);
            w.Write(Height);
            w.Write(SurfaceIndex);
        }
    }


    public enum GameplayId
    {
        None,
        Turbo,
        Turbo2,
        TurboRoulette,
        FreeWheeling,
        NoGrip,
        NoSteering,
        ForceAcceleration,
        Reset,
        SlowMotion,
        Bumper,
        Bumper2,
        ReactorBoost_Legacy,
        Fragile,
        ReactorBoost2_Legacy,
        Bouncy,
        NoBrakes,
        Cruise,
        ReactorBoost_Oriented,
        ReactorBoost2_Oriented,
        VehicleTransform_Reset,
        VehicleTransform_CarSnow,
        VehicleTransform_CarRally,
        VehicleTransform_CarDesert,
    }

    public enum MaterialId
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
        XXX_Null,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0900C000 => new Chunk0900C000(),
        0x0900C003 => new Chunk0900C003(),
        _ => base.NewChunk(chunkId),
    };
}
