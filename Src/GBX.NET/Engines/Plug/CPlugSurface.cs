
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

    internal static ISurf ReadSurf(GbxReader r)
    {
        var surfId = r.ReadInt32();

        return surfId switch // ArchiveGmSurf
        {
            _ => throw new NotSupportedException("Unknown surf type: " + surfId)
        };
    }

    internal static void WriteSurf(ISurf surf, GbxWriter w)
    {
        switch (surf)
        {
            default:
                throw new NotSupportedException("Unknown surf type: " + surf.GetType().Name);
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

    public interface ISurf
    {

    }
}
