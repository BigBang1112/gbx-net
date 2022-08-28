namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0900C000</remarks>
[Node(0x0900C000)]
[NodeExtension("Shape")]
public class CPlugSurface : CPlug
{
    private CPlugSurfaceGeom? geom;
    private SurfaceMaterial[]? materials;

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0900C000))]
    public CPlugSurfaceGeom? Geom { get => geom; set => geom = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0900C000))]
    public SurfaceMaterial[]? Materials { get => materials; set => materials = value; }

    protected CPlugSurface()
    {

    }

    public class SurfaceMaterial // CPlugMaterial
    {
        public bool containsMaterial;
        public CMwNod? material;
        public ushort? defaultMaterialIndex; // set if containsMaterial is false
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

            rw.Array(n.materials, (rw, x) =>
            {
                rw.Boolean(ref x.containsMaterial);

                if (x.containsMaterial)
                {
                    rw.NodeRef(x.material);
                }
                else
                {
                    rw.UInt16(x.defaultMaterialIndex);

                    // sometimes there could be node ref?
                }
            });
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
        public int U02;
        public int U03;
        
        public int U06;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugSurface n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version >= 2)
            {
                rw.Int32(ref U01);
            }

            rw.Int32(ref U02); // ArchiveGmSurf?

            rw.Int32(ref U03); // ArchiveMaterials?

            var wat = rw.Array<Vec3>();
            
            var nice = rw.Array(null, r =>
            {
                return (r.ReadSingle(),
                    r.ReadSingle(),
                    r.ReadSingle(),
                    r.ReadSingle(),
                    r.ReadInt3(),
                    r.ReadUInt16(),
                    r.ReadByte(),
                    r.ReadByte());
            }, (x, w) => { });

            var U04 = rw.Int32();

            var U05 = rw.Array(null, r =>
            {
                return (r.ReadVec3(),
                    r.ReadVec3(),
                    r.ReadInt32());
            }, (x, w) => { });

            /*var gdsg = rw.Reader.ReadArray<object>(r =>
            {
                var u01 = r.ReadBoolean();

                if (!u01)
                {
                    var u02 = r.ReadInt16();
                    var u03 = r.ReadInt32();
                    var u04 = r.ReadUInt64();
                    var nice = r.ReadNodeRef();
                }

                return null;
            });*/
        }
    }
}