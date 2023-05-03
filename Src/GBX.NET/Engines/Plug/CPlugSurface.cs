using GBX.NET.Utils;
using System.Text;

namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0900C000</remarks>
[Node(0x0900C000)]
[NodeExtension("Shape")]
public partial class CPlugSurface : CPlug
{
    private CPlugSurfaceGeom? geom;
    private SurfMaterial[]? materials;
    private ISurf? surf;
    private CPlugSkel? skel;
    private int surfVersion;

    [NodeMember]
    [AppliedWithChunk<Chunk0900C000>]
    public CPlugSurfaceGeom? Geom { get => geom; set => geom = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0900C000>]
    [AppliedWithChunk<Chunk0900C003>]
    public SurfMaterial[]? Materials { get => materials; set => materials = value; }

    [NodeMember(ExactName = "m_GmSurf")]
    [AppliedWithChunk<Chunk0900C003>]
    [AppliedWithChunk<CPlugSurfaceGeom.Chunk0900F002>]
    [AppliedWithChunk<CPlugSurfaceGeom.Chunk0900F004>]
    public ISurf? Surf { get => surf; set => surf = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0900C003>(sinceVersion: 1)]
    public CPlugSkel? Skel { get => skel; set => skel = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0900C003>(sinceVersion: 1)]
    public int SurfVersion { get => surfVersion; set => surfVersion = value; }

    internal CPlugSurface()
    {

    }

    /// <summary>
    /// Exports the surface to .obj file.
    /// </summary>
    /// <param name="fileNameWithoutExtension">File name to write OBJ content into. It will be automatically suffixed with ".obj".</param>
    /// <param name="encoding">Encoding to use.</param>
    /// <param name="leaveOpen">If to keep the streams open.</param>
    public void ExportToObj(string fileNameWithoutExtension, Encoding? encoding = null, bool leaveOpen = false)
    {
        using var objStream = File.Create(fileNameWithoutExtension + ".obj");
        ExportToObj(objStream, encoding, leaveOpen);
    }

    /// <summary>
    /// Exports the surface to .obj file.
    /// </summary>
    /// <param name="objStream">Stream to write OBJ content into.</param>
    /// <param name="encoding">Encoding to use.</param>
    /// <param name="leaveOpen">If to keep the streams open.</param>
    public void ExportToObj(Stream objStream, Encoding? encoding = null, bool leaveOpen = false)
    {
        using var exporter = new ObjFileExporter(
            objStream,
            mtlStream: null,
            encoding: encoding,
            leaveOpen: leaveOpen);

        exporter.Export(Geom ?? this);
    }

    protected static void ArchiveSurf(ref ISurf? surf, GameBoxReaderWriter rw, int surfVersion = 0, int chunkVersion = 0)
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
        var surfId = rw.Int32(surf?.Id, defaultValue: -1);

        surf = surfId switch // ArchiveGmSurf
        {
            0 => rw.Archive((Sphere)(surf ?? new Sphere())),
            1 => rw.Archive((Ellipsoid)(surf ?? new Ellipsoid()), chunkVersion),
            6 => rw.Archive(surf as Box),
            7 => rw.Archive(surf as Mesh), // Mesh
            13 => rw.Archive(surf as Compound, chunkVersion), // Compound
            -1 => null,
            _ => throw new NotSupportedException("Unknown surf type: " + surfId)
        };

        if (surf is not null && surfVersion >= 2)
        {
            surf.U01 = rw.Vec3(surf.U01);
        }
    }

    /// <summary>
    /// CPlugSurface 0x000 chunk
    /// </summary>
    [Chunk(0x0900C000)]
    public class Chunk0900C000 : Chunk<CPlugSurface>
    {
        public string? U01;

        public override void ReadWrite(CPlugSurface n, GameBoxReaderWriter rw)
        {
            if (n is CPlugSurfaceGeom)
            {
                rw.Id(ref U01);
                return;
            }

            rw.NodeRef(ref n.geom);

            rw.ArrayArchiveWithGbx<SurfMaterial>(ref n.materials);
        }
    }

    /// <summary>
    /// CPlugSurface 0x001 chunk
    /// </summary>
    [Chunk(0x0900C001)]
    public class Chunk0900C001 : Chunk<CPlugSurface>
    {
        public bool U01;

        public override void ReadWrite(CPlugSurface n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CPlugSurface 0x003 chunk
    /// </summary>
    [Chunk(0x0900C003)]
    public class Chunk0900C003 : Chunk<CPlugSurface>, IVersionable
    {
        private int version;

        public int U01;
        public byte[]? U02;

        public int U06;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugSurface n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version >= 2)
            {
                rw.Int32(ref n.surfVersion);
            }

            ArchiveSurf(ref n.surf, rw, n.surfVersion, version);

            rw.ArrayArchiveWithGbx<SurfMaterial>(ref n.materials); // ArchiveMaterials
            
            rw.Bytes(ref U02);

            if (version >= 1)
            {
                rw.NodeRef<CPlugSkel>(ref n.skel);
            }
        }
    }

    public class SurfMaterial : IReadableWritableWithGbx
    {
        private GameBox? gbx;
        private CPlugMaterial? material;
        private GameBoxRefTable.File? materialFile;
        private ushort? surfaceId;

        public CPlugMaterial? Material
        {
            get => material = GetNodeFromRefTable(gbx, material, materialFile) as CPlugMaterial;
            set => material = value;
        }
        
        public ushort? SurfaceId { get => surfaceId; set => surfaceId = value; }

        public void ReadWrite(GameBoxReaderWriter rw, GameBox? gbx, int version = 0)
        {
            this.gbx ??= gbx;
            ReadWrite(rw, version);
        }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            if (rw.Boolean(material is not null || materialFile is not null))
            {
                rw.NodeRef(ref material, ref materialFile);
            }
            else
            {
                rw.UInt16(ref surfaceId);
            }
        }
    }

    public interface ISurf : IReadableWritable
    {
        int Id { get; }
        Vec3? U01 { get; set; }
    }

    public class Box : ISurf
    {
        private NET.Box transform;

        public int Id => 6;
        public Vec3? U01 { get; set; }

        public NET.Box Transform { get => transform; set => transform = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Box(ref transform);
        }
    }

    public class Mesh : ISurf
    {
        public int Id => 7;
        public Vec3? U01 { get; set; }

        private int v;
        private Vec3[] vertices = Array.Empty<Vec3>();
        private (Int3, int)[] triangles = Array.Empty<(Int3, int)>();
        private (Vec4, Int3, ushort, byte, byte)[] cookedTriangles = Array.Empty<(Vec4, Int3, ushort, byte, byte)>();
        private AABBTreeCell[]? aABBTree;
        private int? meshOctreeCellVersion;
        private (int, Vec3, Vec3, int)[]? meshOctreeCells;

        public int Version { get => v; set => v = value; }
        public Vec3[] Vertices { get => vertices; set => vertices = value; }
        public (Int3, int)[] Triangles { get => triangles; set => triangles = value; }
        public (Vec4, Int3, ushort, byte, byte)[] CookedTriangles { get => cookedTriangles; set => cookedTriangles = value; }
        public AABBTreeCell[]? AABBTree { get => aABBTree; set => aABBTree = value; }
        public int? MeshOctreeCellVersion { get => meshOctreeCellVersion; set => meshOctreeCellVersion = value; }
        public (int, Vec3, Vec3, int)[]? MeshOctreeCells { get => meshOctreeCells; set => meshOctreeCells = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref v);

            switch (v)
            {
                case 1:
                case 2:
                case 3:
                    rw.Array<Vec3>(ref vertices!);
                    rw.Array(ref cookedTriangles!,
                        r => (r.ReadVec4(),
                            r.ReadInt3(),
                            r.ReadUInt16(),
                            r.ReadByte(),
                            r.ReadByte()),
                        (x, w) =>
                        {
                            w.Write(x.Item1);
                            w.Write(x.Item2);
                            w.Write(x.Item3);
                            w.Write(x.Item4);
                            w.Write(x.Item5);
                        });

                    rw.Int32(ref meshOctreeCellVersion);
                    rw.Array(ref meshOctreeCells, r =>
                    {
                        return (r.ReadInt32(),
                            r.ReadVec3(),
                            r.ReadVec3(),
                            r.ReadInt32());
                    }, (x, w) =>
                    {
                        w.Write(x.Item1);
                        w.Write(x.Item2);
                        w.Write(x.Item3);
                        w.Write(x.Item4);
                    });

                    break;
                case 5:
                    rw.Array<Vec3>(ref vertices!);
                    rw.Array(ref cookedTriangles!, // GmSurfMeshCookedTri
                        r => (r.ReadVec4(),
                            r.ReadInt3(),
                            r.ReadUInt16(),
                            r.ReadByte(),
                            r.ReadByte()),
                        (x, w) =>
                        {
                            w.Write(x.Item1);
                            w.Write(x.Item2);
                            w.Write(x.Item3);
                            w.Write(x.Item4);
                            w.Write(x.Item5);
                        });
                    rw.Int32(1);
                    rw.ArrayArchive<AABBTreeCell>(ref aABBTree!);
                    break;
                case 6:
                case 7:
                    rw.Array<Vec3>(ref vertices!);
                    rw.Array<(Int3, int)>(ref triangles!, // GmSurfMeshTri
                        r => (r.ReadInt3(), r.ReadInt32()),
                        (x, w) => { w.Write(x.Item1); w.Write(x.Item2); });
                    break;
                default:
                    throw new VersionNotSupportedException(v);
            }
        }

        public class AABBTreeCell : IReadableWritable
        {
            public Vec3 U01;
            public Vec3 U02;
            public int U03;

            public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
            {
                rw.Vec3(ref U01);
                rw.Vec3(ref U02);
                rw.Int32(ref U03);
            }
        }
    }

    public class Compound : ISurf
    {
        private ISurf[] surfaces = Array.Empty<ISurf>();
        private Iso4[] u02 = Array.Empty<Iso4>();
        private ushort[] u03 = Array.Empty<ushort>();

        public int Id => 13;
        public Vec3? U01 { get; set; }

        public ISurf[] Surfaces { get => surfaces; set => surfaces = value; }
        public Iso4[] U02 { get => u02; set => u02 = value; }
        public ushort[] U03 { get => u03; set => u03 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            var length = rw.Reader?.ReadInt32() ?? surfaces?.Length ?? 0;
            rw.Writer?.Write(length);

            if (rw.Reader is not null)
            {
                surfaces = new ISurf[length];
            }

            if (surfaces is not null)
            {
                for (int i = 0; i < length; i++)
                {
                    ArchiveSurf(ref surfaces[i]!, rw, chunkVersion: version);
                }
            }

            rw.Array<Iso4>(ref u02!, length);

            if (version >= 1 && (length > 0 || version >= 2)) // I made this up kinda
            {
                rw.Array<ushort>(ref u03!);
            }
        }
    }

    public struct Sphere : ISurf
    {
        private float size;

        public int Id => 0;
        public Vec3? U01 { get; set; }

        public float Size { get => size; set => size = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Single(ref size);
        }
    }

    public struct Ellipsoid : ISurf
    {
        private Vec3 size;
        private ushort u02;

        public int Id => 1;
        public Vec3? U01 { get; set; }
        public ushort U02 { get => u02; set => u02 = value; }

        public Vec3 Size { get => size; set => size = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Vec3(ref size);

            if (version >= 2) // Hack
            {
                rw.UInt16(ref u02);
            }
        }
    }
}