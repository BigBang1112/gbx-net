namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09159000</remarks>
[Class(0x09159000)]
public partial class CPlugStaticObjectModel : CMwNod, IClass, IReadableWritable
{
    [Hexadecimal] public static new uint Id => 0x09159000;




    private int version;
    public int Version { get => version; set => version = value; }

    private CPlugSolid2Model? mesh;
    public CPlugSolid2Model? Mesh { get => meshFile?.GetNode(ref mesh) ?? mesh; set => mesh = value; }
    private Components.GbxRefTableFile? meshFile;
    public Components.GbxRefTableFile? MeshFile { get => meshFile; set => meshFile = value; }
    public CPlugSolid2Model? GetMesh(GbxReadSettings settings = default, bool exceptions = false) => meshFile?.GetNode(ref mesh, settings, exceptions) ?? mesh;

    private bool isMeshCollidable;
    public bool IsMeshCollidable { get => isMeshCollidable; set => isMeshCollidable = value; }

    private CPlugSurface? shape;
    public CPlugSurface? Shape { get => shapeFile?.GetNode(ref shape) ?? shape; set => shape = value; }
    private Components.GbxRefTableFile? shapeFile;
    public Components.GbxRefTableFile? ShapeFile { get => shapeFile; set => shapeFile = value; }
    public CPlugSurface? GetShape(GbxReadSettings settings = default, bool exceptions = false) => shapeFile?.GetNode(ref shape, settings, exceptions) ?? shape;

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        rw.Int32(ref this.version);
        rw.NodeRef<CPlugSolid2Model>(ref mesh, ref meshFile);
        rw.Boolean(ref isMeshCollidable, asByte: true);
        if (!IsMeshCollidable)
        {
            rw.NodeRef<CPlugSurface>(ref shape, ref shapeFile);
        }
    }




}
