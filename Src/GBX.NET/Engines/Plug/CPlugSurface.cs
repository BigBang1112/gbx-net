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
        public int? U05;

        public override void ReadWrite(CPlugSurface n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version >= 2)
            {
                rw.Int32(ref n.surfVersion);
            }

            if (rw.Reader is not null)
            {
                n.Surf = ReadSurf(rw.Reader, n.surfVersion);
            }

            if (rw.Writer is not null)
            {
                WriteSurf(n.Surf, rw.Writer, n.surfVersion);
            }

            rw.ArrayReadableWritable<SurfMaterial>(ref n.materials); // ArchiveMaterials

            if (Version >= 4 && n.materials?.Length > 0)
            {
                rw.Int32(ref U05);
            }

            if ((Version == 3 && (n.materials is null || n.materials.Length == 0)) || Version >= 4)
            {
                rw.Array<ushort>(ref U02);
            }

            if (Version < 3)
            {
                rw.Data(ref U01); // length matches materials count
            }

            if (Version >= 1)
            {
                rw.NodeRef<CPlugSkel>(ref n.skel);
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

    internal static ISurf ReadSurf(GbxReader r, int version)
    {
        var surfId = r.ReadInt32();

        ISurf surf = surfId switch // ArchiveGmSurf
        {
            0 => new Sphere(),
            1 => new Ellipsoid(),
            6 => new Box(),
            7 => new Mesh(),
            13 => new Compound(),
            _ => throw new NotSupportedException("Unknown surf type: " + surfId)
        };

        surf.Read(r, version);

        if (version >= 2)
        {
            surf.U01 = r.ReadVec3();
        }

        return surf;
    }

    internal static void WriteSurf(ISurf? surf, GbxWriter w, int version)
    {
        w.Write(surf switch
        {
            Sphere => 0,
            Ellipsoid => 1,
            Box => 6,
            Mesh => 7,
            Compound => 13,
            _ => throw new NotSupportedException("Cannot write default (null) surf.")
        });

        surf.Write(w, version);

        if (version >= 2)
        {
            w.Write(surf.U01.GetValueOrDefault());
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
                SurfaceId = (MaterialId)rw.Int16((short)SurfaceId.GetValueOrDefault());
            }
        }
    }

    public interface ISurf : IReadable, IWritable
    {
        public short? SurfaceIndex { get; set; }
        public Vec3? U01 { get; set; }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public sealed partial class Sphere : ISurf
    {
        public Vec3? U01 { get; set; }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public sealed partial class Ellipsoid : ISurf
    {
        public Vec3? U01 { get; set; }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public sealed partial class Box : ISurf
    {
        public Vec3? U01 { get; set; }
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
        public Vec3? U01 { get; set; }

        short? ISurf.SurfaceIndex { get; set; }

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
        public ISurf[] Surfs { get; set; } = [];
        public Iso4[] SurfLocs { get; set; } = [];
        public short[] SurfJoints { get; set; } = [];
        public Vec3? U01 { get; set; }

        short? ISurf.SurfaceIndex { get; set; }

        public void Read(GbxReader r, int version = 0)
        {
            var surfCount = r.ReadInt32();
            Surfs = new ISurf[surfCount];

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
