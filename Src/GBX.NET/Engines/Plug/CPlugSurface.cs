namespace GBX.NET.Engines.Plug;

[Node(0x0900C000)]
[NodeExtension("Shape")]
public class CPlugSurface : CPlug
{
    private CPlugSurfaceGeom? geom;

    public CPlugSurfaceGeom? Geom { get => geom; set => geom = value; }

    protected CPlugSurface()
    {

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