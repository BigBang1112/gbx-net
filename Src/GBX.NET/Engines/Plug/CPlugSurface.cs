namespace GBX.NET.Engines.Plug;

public partial class CPlugSurface
{
    public ISurf? Surf { get; set; }

    private int surfVersion;
    public int SurfVersion { get => surfVersion; set => surfVersion = value; }

    private CPlugSkel? skel;
    public CPlugSkel? Skel { get => skel; set => skel = value; }

    private SurfMaterial[] materials = [];
    [AppliedWithChunk<Chunk0900C000>]
    [AppliedWithChunk<Chunk0900C003>]
    public SurfMaterial[] Materials { get => materials; set => materials = value; }

    public partial class Chunk0900C003 : IVersionable
    {
        public int Version { get; set; }

        public byte[]? U01;
        public ushort[]? U02;
        public float? U03;
        public ushort[]? U04;
        // U05 was historically treated as an optional scalar. C003 actually
        // stores a counted u16 material-id vector here (U04), followed by a
        // fallback vector (U02) when the first vector is empty.
        public int? U05;
        public string[]? U06;

        public override void ReadWrite(CPlugSurface n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version >= 2)
            {
                rw.Int32(ref n.surfVersion);
            }

            if (rw.Reader is not null)
            {
                n.Surf = ReadSurf(rw.Reader, Version == 1 ? 1 : n.surfVersion);
            }

            if (rw.Writer is not null)
            {
                WriteSurf(n.Surf, rw.Writer, Version == 1 ? 1 : n.surfVersion);
            }

            rw.ArrayReadableWritable<SurfMaterial>(ref n.materials); // ArchiveMaterials

            if (Version < 3)
            {
                rw.Data(ref U01);
            }
            else
            {
                var hasPrimaryIds = Version >= 4 || n.materials.Length == 0;
                if (hasPrimaryIds)
                {
                    rw.Array<ushort>(ref U04); // SurfaceIds
                }

                if (!hasPrimaryIds || U04 is null || U04.Length == 0)
                {
                    rw.Array<ushort>(ref U02); // SurfaceIds2
                }
            }

            if (Version >= 1)
            {
                rw.NodeRef<CPlugSkel>(ref n.skel);
            }

            if (Version >= 5)
            {
                rw.ArrayId(ref U06);
            }
        }
    }

    // 0 - Sphere
    // 1 - Ellipsoid
    // 6 - Box (Primitive)
    // 7 - Mesh
    // 8 - VCylinder (Primitive)
    // 9 - MultiSphere (Primitive)
    // 10 - ConvexPolyhedron
    // 11 - Capsule (Primitive)
    // 12 - Circle (Non3d)
    // 13 - Compound
    // 14 - SphereLocated (Primitive)
    // 15 - CompoundInstance
    // 16 - Cylinder (Primitive)
    // 17 - SphericalShell
    // 18 - Voxel
    // 19 - Diggable

    internal static ISurf? ReadSurf(GbxReader r, int surfVersion)
    {
        var surfId = r.ReadInt32();
        if (surfId == -1)
        {
            return null;
        }

        ISurf surf = surfId switch // ArchiveGmSurf
        {
            0 => new Sphere(),
            1 => new Ellipsoid(),
            6 => new Box(),
            7 => new Mesh(),
            8 => new VCylinder(),
            9 => new MultiSphere(),
            10 => new ConvexPolyhedron(),
            11 => new Capsule(),
            12 => new Circle(),
            13 => new Compound(),
            14 => new SphereLocated(),
            15 => new CompoundInstance(),
            16 => new Cylinder(),
            17 => new SphericalShell(),
            18 => new Voxel(),
            19 => new Diggable(),
            _ => throw new NotSupportedException("Unknown surf type: " + surfId)
        };

        surf.Read(r, surfVersion);

        if (surfVersion >= 2)
        {
            surf.GameplayMainDir = r.ReadVec3();
        }

        return surf;
    }

    internal static void WriteSurf(ISurf? surf, GbxWriter w, int surfVersion)
    {
        if (surf is null)
        {
            w.Write(-1);
            return;
        }

        w.Write(surf switch
        {
            Sphere => 0,
            Ellipsoid => 1,
            Box => 6,
            Mesh => 7,
            VCylinder => 8,
            MultiSphere => 9,
            ConvexPolyhedron => 10,
            Capsule => 11,
            Circle => 12,
            Compound => 13,
            SphereLocated => 14,
            CompoundInstance => 15,
            Cylinder => 16,
            SphericalShell => 17,
            Voxel => 18,
            Diggable => 19,
            _ => throw new NotSupportedException("Unknown surf type: " + surf.GetType().Name)
        });

        surf.Write(w, surfVersion);

        if (surfVersion >= 2)
        {
            w.Write(surf.GameplayMainDir.GetValueOrDefault());
        }
    }

    public sealed partial class SurfMaterial
    {
        private CPlugMaterial? material;
        public CPlugMaterial? Material { get => materialFile?.GetNode(ref material) ?? material; set => material = value; }
        private Components.GbxRefTableFile? materialFile;
        public Components.GbxRefTableFile? MaterialFile { get => materialFile; set => materialFile = value; }
        public CPlugMaterial? GetMaterial(GbxReadSettings settings = default, bool exceptions = false) => materialFile?.GetNode(ref material, settings, exceptions) ?? material;

        public MaterialId? SurfaceId { get; set; }

        public void ReadWrite(GbxReaderWriter rw, int version = 0)
        {
            if (rw.Boolean(material is not null || materialFile is not null))
            {
                rw.NodeRef<CPlugMaterial>(ref material, ref materialFile);
            }
            else
            {
                var rawSurfaceId = rw.Int16((short)(SurfaceId ?? (MaterialId)(-1)));
                SurfaceId = rawSurfaceId < 0 ? null : (MaterialId)rawSurfaceId;
            }
        }
    }

    public interface ISurf : IReadable, IWritable
    {
        short SurfaceIndex { get; set; }
        Vec3? GameplayMainDir { get; set; }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public sealed partial class Sphere : ISurf
    {
        public Vec3? GameplayMainDir { get; set; }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public sealed partial class Ellipsoid : ISurf
    {
        public Vec3? GameplayMainDir { get; set; }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public sealed partial class Box : ISurf
    {
        public Vec3? GameplayMainDir { get; set; }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public sealed partial class SphereLocated : ISurf
    {
        public Vec3? GameplayMainDir { get; set; }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public sealed partial class VCylinder : ISurf
    {
        public Vec3? GameplayMainDir { get; set; }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public sealed partial class Cylinder : ISurf
    {
        public Vec3? GameplayMainDir { get; set; }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public sealed partial class Capsule : ISurf
    {
        public Vec3? GameplayMainDir { get; set; }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public sealed partial class Circle : ISurf
    {
        public Vec3? GameplayMainDir { get; set; }
        short ISurf.SurfaceIndex { get; set; }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public sealed partial class SphericalShell : ISurf
    {
        public Vec3? GameplayMainDir { get; set; }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public sealed partial class CompoundInstance : ISurf
    {
        public Vec3? GameplayMainDir { get; set; }
        short ISurf.SurfaceIndex { get; set; }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public sealed partial class Voxel : ISurf
    {
        public Vec3? GameplayMainDir { get; set; }
        short ISurf.SurfaceIndex { get; set; }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public sealed partial class Diggable : ISurf
    {
        public Vec3? GameplayMainDir { get; set; }
        short ISurf.SurfaceIndex { get; set; }
    }

    public sealed partial class MultiSphere : ISurf
    {
        public LocatedSphere[] Spheres { get; set; } = [];
        public short SurfaceIndex { get; set; }
        public Vec3? GameplayMainDir { get; set; }

        public void Read(GbxReader r, int version = 0)
        {
            var count = r.ReadUInt32();
            if (count > 10)
            {
                throw new InvalidDataException("MultiSphere supports at most 10 spheres.");
            }

            Spheres = r.ReadArray<LocatedSphere>((int)count);
            SurfaceIndex = r.ReadInt16();
        }

        public void Write(GbxWriter w, int version = 0)
        {
            if (Spheres.Length > 10)
            {
                throw new InvalidDataException("MultiSphere supports at most 10 spheres.");
            }

            w.WriteArray(Spheres);
            w.Write(SurfaceIndex);
        }

        public readonly record struct LocatedSphere(float Radius, Vec3 Center);
    }

    public sealed partial class ConvexPolyhedron : ISurf, IVersionable
    {
        public int Version { get; set; }
        public int Representation { get; set; }
        public Vec3 Parameters { get; set; }
        public BoxAligned Bounds { get; set; }
        public Vec3[] Vertices { get; set; } = [];
        public uint[] Indices { get; set; } = [];
        public Face[] Faces { get; set; } = [];
        public short SurfaceIndex { get; set; }
        public Vec3? GameplayMainDir { get; set; }

        public void Read(GbxReader r, int version = 0)
        {
            Version = r.ReadInt32();
            Representation = r.ReadInt32();
            if (Representation != 0)
            {
                Parameters = r.ReadVec3();
            }
            else
            {
                Bounds = r.ReadBoxAligned();
                Vertices = r.ReadArray<Vec3>();
                Indices = r.ReadArray<uint>();
                Faces = r.ReadArray<Face>();
            }

            SurfaceIndex = r.ReadInt16();
        }

        public void Write(GbxWriter w, int version = 0)
        {
            w.Write(Version);
            w.Write(Representation);
            if (Representation != 0)
            {
                w.Write(Parameters);
            }
            else
            {
                w.Write(Bounds);
                w.WriteArray(Vertices);
                w.WriteArray(Indices);
                w.WriteArray(Faces);
            }

            w.Write(SurfaceIndex);
        }

        public readonly record struct Face(uint StartIndex, uint Count);
    }

    public sealed partial class Mesh : ISurf, IVersionable
    {
        public int Version { get; set; }
        public Vec3[] Vertices { get; set; } = [];
        public CookedTriangle[]? CookedTriangles { get; set; }
        public int? OctreeVersion { get; set; }
        public OctreeCell[]? OctreeCells { get; set; }
        public AABBTreeCell[]? AABBTreeCells { get; set; }
        public Triangle[]? Triangles { get; set; }
        public Vec3? GameplayMainDir { get; set; }

        short ISurf.SurfaceIndex { get; set; }

        public void Read(GbxReader r, int version = 0)
        {
            Version = r.ReadInt32();

            switch (Version)
            {
                case 1:
                case 2:
                case 3:
                    Vertices = r.ReadArray<Vec3>();
                    CookedTriangles = r.ReadArray<CookedTriangle>();
                    OctreeVersion = r.ReadInt32();
                    OctreeCells = OctreeVersion == 1
                        ? r.ReadArray_deprec<OctreeCell>()
                        : r.ReadArray<OctreeCell>();
                    break;
                case 5:
                    Vertices = r.ReadArray<Vec3>();
                    CookedTriangles = r.ReadArray<CookedTriangle>();
                    r.ReadInt32();
                    AABBTreeCells = r.ReadArray<AABBTreeCell>();
                    break;
                case 6:
                case 7:
                    Vertices = r.ReadArray<Vec3>();
                    Triangles = r.ReadArray<Triangle>(); // GmSurfMeshTri
                    break;
            } 
        }

        public void Write(GbxWriter w, int version = 0)
        {
            w.Write(Version);

            switch (Version)
            {
                case 1:
                case 2:
                case 3:
                    w.WriteArray(Vertices);
                    w.WriteArray(CookedTriangles);
                    w.Write(OctreeVersion.GetValueOrDefault());
                    if (OctreeVersion == 1)
                    {
                        w.WriteArray_deprec(OctreeCells);
                    }
                    else
                    {
                        w.WriteArray(OctreeCells);
                    }
                    break;
                case 5:
                    w.WriteArray(Vertices);
                    w.WriteArray(CookedTriangles);
                    w.Write(1);
                    w.WriteArray(AABBTreeCells);
                    break;
                case 6:
                case 7:
                    w.WriteArray(Vertices);
                    w.WriteArray(Triangles);
                    break;
            }
        }

        public readonly record struct CookedTriangle(Vec4 U01, Int3 Indices, short SurfaceIndex, byte U04, byte U05);
        public readonly record struct OctreeCell(int U01, Vec3 U02, Vec3 U03, int U04);
        public readonly record struct Triangle(Int3 Indices, byte U02, byte U03, short SurfaceIndex);
        public readonly record struct AABBTreeCell(Vec3 U01, Vec3 U02, int U03);
    }

    public sealed partial class Compound : ISurf, IVersionable
    {
        public int Version { get; set; }
        public ISurf?[] Surfs { get; set; } = [];
        public Iso4[] SurfLocs { get; set; } = [];
        public short[] SurfJoints { get; set; } = [];
        public Vec3? GameplayMainDir { get; set; }

        short ISurf.SurfaceIndex { get; set; }

        public void Read(GbxReader r, int version = 0)
        {
            var surfCount = r.ReadInt32();
            Surfs = new ISurf?[surfCount];

            for (var i = 0; i < surfCount; i++)
            {
                Surfs[i] = ReadSurf(r, version);
            }

            SurfLocs = r.ReadArray<Iso4>(surfCount);

            if (version >= 1)
            {
                SurfJoints = r.ReadArray<short>();
            }
        }

        public void Write(GbxWriter w, int version = 0)
        {
            w.Write(Surfs.Length);

            foreach (var surf in Surfs)
            {
                WriteSurf(surf, w, version);
            }

            foreach (var surfLoc in SurfLocs)
            {
                w.Write(surfLoc);
            }

            if (version >= 1)
            {
                w.WriteArray(SurfJoints);
            }
        }
    }
}
