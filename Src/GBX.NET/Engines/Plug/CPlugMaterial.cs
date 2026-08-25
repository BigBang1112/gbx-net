namespace GBX.NET.Engines.Plug;

public partial class CPlugMaterial
{
    private DeviceMat[]? deviceMaterials;
    [AppliedWithChunk<Chunk0907900D>]
    public DeviceMat[]? DeviceMaterials { get => deviceMaterials; set => deviceMaterials = value; }

    [AppliedWithChunk<Chunk0907900E>]
    [AppliedWithChunk<Chunk09079017>]
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

    public partial class Chunk09079017
    {
        public override void ReadWrite(CPlugMaterial n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version == 1)
            {
                n.SurfaceId = (CPlugSurface.MaterialId)rw.Int16((short)n.SurfaceId);
                rw.Int32(ref U01);
                rw.Int32(ref U02);
                rw.Int16(ref U03);
                rw.String(ref U04);
            }
        }
    }
}
