namespace GBX.NET.Engines.Plug;

public partial class CPlugMaterial
{
    private CPlug? shader;
    [AppliedWithChunk<Chunk0907900D>]
    public CPlug? Shader { get => shaderFile?.GetNode(ref shader) ?? shader; set => shader = value; } // probably m_Material
    private Components.GbxRefTableFile? shaderFile;
    public Components.GbxRefTableFile? ShaderFile { get => shaderFile; set => shaderFile = value; }
    public CPlug? GetShader(GbxReadSettings settings = default, bool exceptions = false) => shaderFile?.GetNode(ref shader, settings, exceptions) ?? shader;

    private DeviceMat[]? deviceMaterials;
    [AppliedWithChunk<Chunk0907900D>]
    public DeviceMat[]? DeviceMaterials { get => deviceMaterials; set => deviceMaterials = value; }

    [AppliedWithChunk<Chunk0907900E>]
    public CPlugSurface.MaterialId SurfaceId { get; set; }

    public partial class Chunk09079009
    {
        public override void ReadWrite(CPlugMaterial n, GbxReaderWriter rw)
        {
            rw.NodeRef(ref n.shader, ref n.shaderFile);

            if (n.shader is not null || n.shaderFile is not null)
            {
                return;
            }

            rw.ArrayReadableWritable(ref n.deviceMaterials, version: 0x9);
        }
    }

    public partial class Chunk0907900D
    {
        public int[]? U01;

        public override void ReadWrite(CPlugMaterial n, GbxReaderWriter rw)
        {
            rw.NodeRef(ref n.shader, ref n.shaderFile);

            if (n.shader is not null || n.shaderFile is not null)
            {
                return;
            }

            rw.ArrayReadableWritable(ref n.deviceMaterials, version: 0xD);

            rw.Array<int>(ref U01);
        }
    }

    public partial class Chunk0907900E
    {
        public override void ReadWrite(CPlugMaterial n, GbxReaderWriter rw)
        {
            n.SurfaceId = (CPlugSurface.MaterialId)rw.Int16((short)n.SurfaceId);
            rw.Int16(ref U01);
        }
    }
}
