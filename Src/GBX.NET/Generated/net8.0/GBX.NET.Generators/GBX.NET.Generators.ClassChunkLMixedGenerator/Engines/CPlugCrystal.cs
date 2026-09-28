namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09003000</remarks>
[Class(0x09003000)]
public partial class CPlugCrystal : CPlugTreeGenerator, IClass
{
    [Hexadecimal] public static new uint Id => 0x09003000;




    private List<Material> materials = new List<Material>();
    [AppliedWithChunk<Chunk09003003>]
    public List<Material> Materials { get => materials; set => materials = value; }

    private float[]? smoothingGroups;
    /// <summary>
    /// SCrystalSmoothingGroup array
    /// </summary>
    [AppliedWithChunk<Chunk09003007>]
    public float[]? SmoothingGroups { get => smoothingGroups; set => smoothingGroups = value; }


    /// <summary>
    /// CPlugCrystal 0x000 chunk (one layer only)
    /// </summary>
    [Chunk(0x09003000, "one layer only")]
    public partial class Chunk09003000 : Chunk<CPlugCrystal>
    {
        /// <inheritdoc />
        public override uint Id => 0x09003000;

    }

    /// <summary>
    /// CPlugCrystal 0x003 chunk (materials)
    /// </summary>
    [Chunk(0x09003003, "materials")]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020, 2, 2)]
    public partial class Chunk09003003 : Chunk<CPlugCrystal>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09003003;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }


        public override void ReadWrite(CPlugCrystal n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ListReadableWritable<Material>(ref n.materials!);
        }
    }

    /// <summary>
    /// CPlugCrystal 0x004 skippable chunk
    /// </summary>
    [Chunk(0x09003004)]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020, 1, 1)]
    public partial class Chunk09003004 : SkippableChunk<CPlugCrystal>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09003004;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }

        public byte[]? U01;
        /// <summary>
        /// DoData
        /// </summary>
        public int? U02;

        public override void ReadWrite(CPlugCrystal n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Data(ref U01);
            if (Version >= 1)
            {
                rw.Int32(ref U02); // DoData
            }
        }
    }

    /// <summary>
    /// CPlugCrystal 0x005 chunk (layers)
    /// </summary>
    [Chunk(0x09003005, "layers")]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk09003005 : Chunk<CPlugCrystal>
    {
        /// <inheritdoc />
        public override uint Id => 0x09003005;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CPlugCrystal 0x006 chunk (lightmap UVs)
    /// </summary>
    [Chunk(0x09003006, "lightmap UVs")]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020, 0, 1)]
    public partial class Chunk09003006 : Chunk<CPlugCrystal>
    {
        /// <inheritdoc />
        public override uint Id => 0x09003006;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CPlugCrystal 0x007 chunk (smoothing groups)
    /// </summary>
    [Chunk(0x09003007, "smoothing groups")]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk09003007 : Chunk<CPlugCrystal>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09003007;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }

        public int[]? U01;

        public override void ReadWrite(CPlugCrystal n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Array<float>(ref n.smoothingGroups!); // SCrystalSmoothingGroup array
            rw.Array<int>(ref U01!);
        }
    }


    public sealed partial class LightLayer : ModifierLayer, IReadable<CPlugCrystal>, IWritable<CPlugCrystal>
    {
        public int LightVersion { get; set; }
        public CPlugLightUserModel[]? Lights { get; set; }
        public LightPos[]? LightPositions { get; set; }

        public override void Read(GbxReader r, CPlugCrystal n, int v = 0)
        {
            base.Read(r, n, v);
            LightVersion = r.ReadInt32();
            Lights = r.ReadArrayNodeRef<CPlugLightUserModel>();
            LightPositions = r.ReadArrayReadable<LightPos>();
        }

        public override void Write(GbxWriter w, CPlugCrystal n, int v = 0)
        {
            base.Write(w, n, v);
            w.Write(LightVersion);
            w.WriteArrayNodeRef<CPlugLightUserModel>(Lights);
            w.WriteArrayWritable<LightPos>(LightPositions);
        }
    }

    public sealed partial class VoxelSpace : IReadable, IWritable
    {

        public void Read(GbxReader r, int v = 0)
        {
            throw new ("");
        }

        public void Write(GbxWriter w, int v = 0)
        {
            throw new ("");
        }
    }

    public sealed partial class SubdivideSmoothLayer : ModifierLayer, IReadable<CPlugCrystal>, IWritable<CPlugCrystal>
    {
        public int SubdivideSmoothVersion { get; set; }
        public int Subdivisions { get; set; }

        public override void Read(GbxReader r, CPlugCrystal n, int v = 0)
        {
            base.Read(r, n, v);
            SubdivideSmoothVersion = r.ReadInt32();
            Subdivisions = r.ReadInt32();
        }

        public override void Write(GbxWriter w, CPlugCrystal n, int v = 0)
        {
            base.Write(w, n, v);
            w.Write(SubdivideSmoothVersion);
            w.Write(Subdivisions);
        }
    }

    public sealed partial class LightPos : IReadable, IWritable
    {
        public int U01 { get; set; }
        public Iso4 U02 { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            U01 = r.ReadInt32();
            U02 = r.ReadIso4();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(U01);
            w.Write(U02);
        }
    }

    public sealed partial class ExtrudeLayer : ModifierLayer, IReadable<CPlugCrystal>, IWritable<CPlugCrystal>
    {
        public int ExtrudeVersion { get; set; }
        public Vec3 Size { get; set; }

        public override void Read(GbxReader r, CPlugCrystal n, int v = 0)
        {
            base.Read(r, n, v);
            ExtrudeVersion = r.ReadInt32();
            Size = r.ReadVec3();
        }

        public override void Write(GbxWriter w, CPlugCrystal n, int v = 0)
        {
            base.Write(w, n, v);
            w.Write(ExtrudeVersion);
            w.Write(Size);
        }
    }

    public sealed partial class SubdivideLayer : ModifierLayer, IReadable<CPlugCrystal>, IWritable<CPlugCrystal>
    {
        public int SubdivideVersion { get; set; }
        public int Subdivisions { get; set; }

        public override void Read(GbxReader r, CPlugCrystal n, int v = 0)
        {
            base.Read(r, n, v);
            SubdivideVersion = r.ReadInt32();
            Subdivisions = r.ReadInt32();
        }

        public override void Write(GbxWriter w, CPlugCrystal n, int v = 0)
        {
            base.Write(w, n, v);
            w.Write(SubdivideVersion);
            w.Write(Subdivisions);
        }
    }

    public sealed partial class SpawnPositionLayer : ModifierLayer, IReadable<CPlugCrystal>, IWritable<CPlugCrystal>
    {
        public int SpawnPositionVersion { get; set; }
        public Vec3 SpawnPosition { get; set; }
        public float HorizontalAngle { get; set; }
        public float VerticalAngle { get; set; }
        public float RollAngle { get; set; }

        public override void Read(GbxReader r, CPlugCrystal n, int v = 0)
        {
            base.Read(r, n, v);
            SpawnPositionVersion = r.ReadInt32();
            SpawnPosition = r.ReadVec3();
            HorizontalAngle = r.ReadSingle();
            VerticalAngle = r.ReadSingle();
            if (SpawnPositionVersion>=1)
            {
                RollAngle = r.ReadSingle();
            }
        }

        public override void Write(GbxWriter w, CPlugCrystal n, int v = 0)
        {
            base.Write(w, n, v);
            w.Write(SpawnPositionVersion);
            w.Write(SpawnPosition);
            w.Write(HorizontalAngle);
            w.Write(VerticalAngle);
            if (SpawnPositionVersion>=1)
            {
                w.Write(RollAngle);
            }
        }
    }

    public sealed partial class Material : IReadableWritable
    {

        private string? materialName;
        public string? MaterialName { get => materialName; set => materialName = value; }

        private CPlugMaterialUserInst? materialUserInst;
        public CPlugMaterialUserInst? MaterialUserInst { get => materialUserInst; set => materialUserInst = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.String(ref materialName);
            if (MaterialName==null||MaterialName=="")
            {
                rw.NodeRef<CPlugMaterialUserInst>(ref materialUserInst);
            }
        }
    }

    public sealed partial class TranslationLayer : ModifierLayer, IReadable<CPlugCrystal>, IWritable<CPlugCrystal>
    {
        public int TranslationVersion { get; set; }
        public Vec3 Translation { get; set; }

        public override void Read(GbxReader r, CPlugCrystal n, int v = 0)
        {
            base.Read(r, n, v);
            TranslationVersion = r.ReadInt32();
            Translation = r.ReadVec3();
        }

        public override void Write(GbxWriter w, CPlugCrystal n, int v = 0)
        {
            base.Write(w, n, v);
            w.Write(TranslationVersion);
            w.Write(Translation);
        }
    }

    public sealed partial class RotationLayer : ModifierLayer, IReadable<CPlugCrystal>, IWritable<CPlugCrystal>
    {
        public int RotationVersion { get; set; }
        /// <summary>
        /// in radians
        /// </summary>
        public float Rotation { get; set; }
        public EAxis Axis { get; set; }
        public bool Independently { get; set; }

        public override void Read(GbxReader r, CPlugCrystal n, int v = 0)
        {
            base.Read(r, n, v);
            RotationVersion = r.ReadInt32();
            Rotation = r.ReadSingle(); // in radians
            Axis = (EAxis)r.ReadInt32();
            Independently = r.ReadBoolean();
        }

        public override void Write(GbxWriter w, CPlugCrystal n, int v = 0)
        {
            base.Write(w, n, v);
            w.Write(RotationVersion);
            w.Write(Rotation); // in radians
            w.Write((int)Axis);
            w.Write(Independently);
        }
    }

    public sealed partial class Part : IReadable, IWritable
    {
        public int U01 { get; set; }
        public int U03 { get; set; }
        public string? Name { get; set; }
        public int U04 { get; set; }
        public int[]? U05 { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            if (v >= 31)
            {
                U01 = r.ReadInt32();
            }
            if (v >= 36)
            {
                U02 = r.ReadByte();
            }
            if (v <= 35)
            {
                U02 = r.ReadInt32();
            }
            U03 = r.ReadInt32();
            Name = r.ReadString();
            U04 = r.ReadInt32();
            U05 = r.ReadArray<int>();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            if (v >= 31)
            {
                w.Write(U01);
            }
            if (v >= 36)
            {
                w.Write((byte)U02);
            }
            if (v <= 35)
            {
                w.Write(U02);
            }
            w.Write(U03);
            w.Write(Name);
            w.Write(U04);
            w.WriteArray<int>(U05);
        }
    }

    public sealed partial class BorderTransitionLayer : ModifierLayer, IReadable<CPlugCrystal>, IWritable<CPlugCrystal>
    {
        public int BorderTransitionVersion { get; set; }
        public float U01 { get; set; }
        public float U02 { get; set; }
        /// <summary>
        /// interesting, but not sure what it is
        /// </summary>
        public CPlugVisual[]? U03 { get; set; }

        public override void Read(GbxReader r, CPlugCrystal n, int v = 0)
        {
            base.Read(r, n, v);
            BorderTransitionVersion = r.ReadInt32();
            U01 = r.ReadSingle();
            U02 = r.ReadSingle();
            U03 = r.ReadArrayNodeRef<CPlugVisual>(); // interesting, but not sure what it is
        }

        public override void Write(GbxWriter w, CPlugCrystal n, int v = 0)
        {
            base.Write(w, n, v);
            w.Write(BorderTransitionVersion);
            w.Write(U01);
            w.Write(U02);
            w.WriteArrayNodeRef<CPlugVisual>(U03); // interesting, but not sure what it is
        }
    }

    public sealed partial class DeformationLayer : ModifierLayer, IReadable<CPlugCrystal>, IWritable<CPlugCrystal>
    {
        public int DeformationVersion { get; set; }
        public BoxAligned U01 { get; set; }
        public Iso4 U02 { get; set; }

        public override void Read(GbxReader r, CPlugCrystal n, int v = 0)
        {
            base.Read(r, n, v);
            DeformationVersion = r.ReadInt32();
            U01 = r.ReadBoxAligned();
            U02 = r.ReadIso4();
        }

        public override void Write(GbxWriter w, CPlugCrystal n, int v = 0)
        {
            base.Write(w, n, v);
            w.Write(DeformationVersion);
            w.Write(U01);
            w.Write(U02);
        }
    }

    public sealed partial class ScaleLayer : ModifierLayer, IReadable<CPlugCrystal>, IWritable<CPlugCrystal>
    {
        public int ScaleVersion { get; set; }
        public Vec3 Scale { get; set; }
        public bool Independently { get; set; }

        public override void Read(GbxReader r, CPlugCrystal n, int v = 0)
        {
            base.Read(r, n, v);
            ScaleVersion = r.ReadInt32();
            Scale = r.ReadVec3();
            Independently = r.ReadBoolean();
        }

        public override void Write(GbxWriter w, CPlugCrystal n, int v = 0)
        {
            base.Write(w, n, v);
            w.Write(ScaleVersion);
            w.Write(Scale);
            w.Write(Independently);
        }
    }

    public sealed partial class AnchorInfo : IReadable, IWritable
    {
        public bool U01 { get; set; }
        public bool U02 { get; set; }
        public Iso4 U03 { get; set; }
        public string? U04 { get; set; }
        public int U05 { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            U01 = r.ReadBoolean();
            U02 = r.ReadBoolean();
            U03 = r.ReadIso4();
            U04 = r.ReadString();
            U05 = r.ReadInt32();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(U01);
            w.Write(U02);
            w.Write(U03);
            w.Write(U04);
            w.Write(U05);
        }
    }

    public sealed partial class CubesLayer : Layer, IReadable<CPlugCrystal>, IWritable<CPlugCrystal>
    {
        public int CubesVersion { get; set; }
        public VoxelSpace? Cubes { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool Collidable { get; set; } = true;

        public override void Read(GbxReader r, CPlugCrystal n, int v = 0)
        {
            base.Read(r, n, v);
            CubesVersion = r.ReadInt32();
            Cubes = r.ReadReadable<VoxelSpace>();
            if (CubesVersion>=2)
            {
                IsVisible = r.ReadBoolean();
                Collidable = r.ReadBoolean();
            }
        }

        public override void Write(GbxWriter w, CPlugCrystal n, int v = 0)
        {
            base.Write(w, n, v);
            w.Write(CubesVersion);
            w.WriteWritable<VoxelSpace>(Cubes);
            if (CubesVersion>=2)
            {
                w.Write(IsVisible);
                w.Write(Collidable);
            }
        }
    }

    public partial class Layer : IReadable<CPlugCrystal>, IWritable<CPlugCrystal>
    {
        public int Ver { get; set; }
        public bool CrystalEnabled { get; set; }
        public string? LayerId { get; set; }
        public string? LayerName { get; set; }
        public bool IsEnabled { get; set; } = true;

        public virtual void Read(GbxReader r, CPlugCrystal n, int v = 0)
        {
            Ver = r.ReadInt32();
            CrystalEnabled = r.ReadBoolean();
            LayerId = r.ReadId();
            LayerName = r.ReadString();
            if (Ver>=1)
            {
                IsEnabled = r.ReadBoolean();
            }
        }

        public virtual void Write(GbxWriter w, CPlugCrystal n, int v = 0)
        {
            w.Write(Ver);
            w.Write(CrystalEnabled);
            w.WriteIdAsString(LayerId);
            w.Write(LayerName);
            if (Ver>=1)
            {
                w.Write(IsEnabled);
            }
        }
    }

    public sealed partial class MirrorLayer : ModifierLayer, IReadable<CPlugCrystal>, IWritable<CPlugCrystal>
    {
        public int MirrorVersion { get; set; }
        public EAxis Axis { get; set; }
        public float Distance { get; set; }
        public bool Independently { get; set; }

        public override void Read(GbxReader r, CPlugCrystal n, int v = 0)
        {
            base.Read(r, n, v);
            MirrorVersion = r.ReadInt32();
            Axis = (EAxis)r.ReadInt32();
            Distance = r.ReadSingle();
            Independently = r.ReadBoolean();
        }

        public override void Write(GbxWriter w, CPlugCrystal n, int v = 0)
        {
            base.Write(w, n, v);
            w.Write(MirrorVersion);
            w.Write((int)Axis);
            w.Write(Distance);
            w.Write(Independently);
        }
    }

    public sealed partial class VisualLevel : IReadable, IWritable
    {
        public int U01 { get; set; }
        public float U02 { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            U01 = r.ReadInt32();
            U02 = r.ReadSingle();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(U01);
            w.Write(U02);
        }
    }

    public sealed partial class SmoothLayer : ModifierLayer, IReadable<CPlugCrystal>, IWritable<CPlugCrystal>
    {
        public int SmoothVersion { get; set; }
        /// <summary>
        /// SmoothFactor?
        /// </summary>
        public float U01 { get; set; }
        /// <summary>
        /// Independently
        /// </summary>
        public bool U02 { get; set; }

        public override void Read(GbxReader r, CPlugCrystal n, int v = 0)
        {
            base.Read(r, n, v);
            SmoothVersion = r.ReadInt32();
            U01 = r.ReadSingle(); // SmoothFactor?
            U02 = r.ReadBoolean(); // Independently
        }

        public override void Write(GbxWriter w, CPlugCrystal n, int v = 0)
        {
            base.Write(w, n, v);
            w.Write(SmoothVersion);
            w.Write(U01); // SmoothFactor?
            w.Write(U02); // Independently
        }
    }

    public sealed partial class MoveToGroundLayer : ModifierLayer, IReadable<CPlugCrystal>, IWritable<CPlugCrystal>
    {
        public int MoveToGroundVersion { get; set; }
        public bool U01 { get; set; }

        public override void Read(GbxReader r, CPlugCrystal n, int v = 0)
        {
            base.Read(r, n, v);
            MoveToGroundVersion = r.ReadInt32();
            U01 = r.ReadBoolean();
        }

        public override void Write(GbxWriter w, CPlugCrystal n, int v = 0)
        {
            base.Write(w, n, v);
            w.Write(MoveToGroundVersion);
            w.Write(U01);
        }
    }

    public sealed partial class GeometryLayer : Layer, IReadable<CPlugCrystal>, IWritable<CPlugCrystal>
    {
        public int GeometryVersion { get; set; }
        public Crystal? Crystal { get; set; }
        /// <summary>
        /// ID for each group?
        /// </summary>
        public int[]? U02 { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool Collidable { get; set; } = true;

        public override void Read(GbxReader r, CPlugCrystal n, int v = 0)
        {
            base.Read(r, n, v);
            GeometryVersion = r.ReadInt32();
            Crystal = r.ReadReadable<Crystal, CPlugCrystal>(n);
            U02 = r.ReadArray<int>(); // ID for each group?
            if (GeometryVersion>=1)
            {
                IsVisible = r.ReadBoolean();
                Collidable = r.ReadBoolean();
            }
        }

        public override void Write(GbxWriter w, CPlugCrystal n, int v = 0)
        {
            base.Write(w, n, v);
            w.Write(GeometryVersion);
            w.WriteWritable<Crystal, CPlugCrystal>(Crystal, n);
            w.WriteArray<int>(U02); // ID for each group?
            if (GeometryVersion>=1)
            {
                w.Write(IsVisible);
                w.Write(Collidable);
            }
        }
    }

    public partial class ModifierLayer : Layer, IReadable<CPlugCrystal>, IWritable<CPlugCrystal>
    {
        public int ModifierVersion { get; set; }
        public PartInLayer[]? Mask { get; set; }
    }

    public sealed partial class TriggerLayer : Layer, IReadable<CPlugCrystal>, IWritable<CPlugCrystal>
    {
        public int TriggerVersion { get; set; }
        public Crystal? Crystal { get; set; }
        public int[]? U01 { get; set; }

        public override void Read(GbxReader r, CPlugCrystal n, int v = 0)
        {
            base.Read(r, n, v);
            TriggerVersion = r.ReadInt32();
            Crystal = r.ReadReadable<Crystal, CPlugCrystal>(n);
            if (TriggerVersion>=1)
            {
                U01 = r.ReadArray<int>();
            }
        }

        public override void Write(GbxWriter w, CPlugCrystal n, int v = 0)
        {
            base.Write(w, n, v);
            w.Write(TriggerVersion);
            w.WriteWritable<Crystal, CPlugCrystal>(Crystal, n);
            if (TriggerVersion>=1)
            {
                w.WriteArray<int>(U01);
            }
        }
    }

    public sealed partial class Crystal : IReadable<CPlugCrystal>, IWritable<CPlugCrystal>
    {
    }

    public sealed partial class ChaosLayer : ModifierLayer, IReadable<CPlugCrystal>, IWritable<CPlugCrystal>
    {
        public int ChaosVersion { get; set; }
        public float MinDistance { get; set; }
        public int U01 { get; set; }
        public float MaxDistance { get; set; }

        public override void Read(GbxReader r, CPlugCrystal n, int v = 0)
        {
            base.Read(r, n, v);
            ChaosVersion = r.ReadInt32();
            MinDistance = r.ReadSingle();
            U01 = r.ReadInt32();
            if (ChaosVersion>=1)
            {
                MaxDistance = r.ReadSingle();
            }
        }

        public override void Write(GbxWriter w, CPlugCrystal n, int v = 0)
        {
            base.Write(w, n, v);
            w.Write(ChaosVersion);
            w.Write(MinDistance);
            w.Write(U01);
            if (ChaosVersion>=1)
            {
                w.Write(MaxDistance);
            }
        }
    }

    public sealed partial class PartInLayer : IReadable, IWritable
    {
        public int GroupIndex { get; set; }
        public string? LayerId { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            GroupIndex = r.ReadInt32();
            LayerId = r.ReadId();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(GroupIndex);
            w.WriteIdAsString(LayerId);
        }
    }


    public enum ELayerType
    {
        Geometry,
        SubdivideSmooth,
        Translation,
        Rotation,
        Scale,
        Mirror,
        MoveToGround,
        Extrude,
        Subdivide,
        Chaos,
        Smooth,
        BorderTransition,
        /// <summary>
        /// BlocTransfo
        /// </summary>
        Deformation,
        /// <summary>
        /// Voxels
        /// </summary>
        Cubes,
        /// <summary>
        /// TriggerShape
        /// </summary>
        Trigger,
        /// <summary>
        /// RespawnPos
        /// </summary>
        SpawnPosition,
        Light = 18,
    }

    public enum EAxis
    {
        X,
        Y,
        Z,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09003000 => new Chunk09003000(),
        0x09003003 => new Chunk09003003(),
        0x09003004 => new Chunk09003004(),
        0x09003005 => new Chunk09003005(),
        0x09003006 => new Chunk09003006(),
        0x09003007 => new Chunk09003007(),
        _ => base.NewChunk(chunkId),
    };
}
