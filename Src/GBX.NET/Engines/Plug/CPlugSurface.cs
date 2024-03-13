
namespace GBX.NET.Engines.Plug;

public partial class CPlugSurface
{
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
        return surf;
    }

    internal static void WriteSurf(ISurf? surf, GbxWriter w)
    {
        switch (surf)
        {
            default:
                throw new NotSupportedException("Cannot write default (null) surf.");
        }
    }

    public sealed partial class SurfMaterial
    {
        public void ReadWrite(GbxReaderWriter rw, int version = 0)
        {
            if (rw.Boolean(material is not null))
            {
                rw.NodeRef<CPlugMaterial>(ref material);
            }
            else
            {
                rw.Int16(ref surfaceId);
            }
        }
    }

    public interface ISurf : IReadable, IWritable
    {

    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public sealed partial class Sphere : ISurf;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public sealed partial class Ellipsoid : ISurf;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public sealed partial class Box : ISurf;

    public sealed partial class Mesh : ISurf, IVersionable
    {
        public int Version { get; set; }
        public Vec3[] Vertices { get; set; } = [];
        public CookedTriangle[] CookedTriangles { get; set; } = [];
        public int? OctreeVersion { get; set; }
        public OctreeCell[] OctreeCells { get; set; } = [];

        public void Read(GbxReader r, int version = 0)
        {
            Version = r.ReadInt32();

            switch (Version)
            {
                case 1: // has OctreeCells deprec (10 before array, TODO fix)
                case 2:
                case 3:
                    Vertices = r.ReadArray<Vec3>();
                    CookedTriangles = r.ReadArray<CookedTriangle>();
                    OctreeVersion = r.ReadInt32();
                    OctreeCells = r.ReadArray<OctreeCell>();
                    break;
            }
        }

        public void Write(GbxWriter w, int version = 0)
        {
            throw new NotImplementedException();
        }

        public readonly record struct CookedTriangle(Vec4 U01, Int3 U02, ushort U03, byte U04, byte U05);
        public readonly record struct OctreeCell(int U01, Vec3 U02, Vec3 U03, int U04);
    }

    public sealed partial class Compound : ISurf, IVersionable
    {
        public int Version { get; set; }

        public void Read(GbxReader r, int version = 0)
        {
            throw new NotImplementedException();
        }

        public void Write(GbxWriter w, int version = 0)
        {
            throw new NotImplementedException();
        }
    }
}
