namespace GBX.NET.Engines.Plug;

[Node(0x0900C000)]
[NodeExtension("Shape")]
public class CPlugSurface : CPlug
{
    private CPlugSurfaceGeom? geom;
    private SurfaceMaterial[]? materials;

    public CPlugSurfaceGeom? Geom { get => geom; set => geom = value; }

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

    [Chunk(0x0900C001)]
    public class Chunk0900C001 : Chunk<CPlugSurface>
    {
        public bool U01;

        public override void ReadWrite(CPlugSurface n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }
}