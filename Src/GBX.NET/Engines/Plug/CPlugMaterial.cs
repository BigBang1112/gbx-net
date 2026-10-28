namespace GBX.NET.Engines.Plug;

public partial class CPlugMaterial
{
    private DeviceMat[]? deviceMaterials;
    /// <remarks>Applied with: 0x00D body chunk</remarks>
    [AppliedWithChunk<Chunk0907900D>]
    public DeviceMat[]? DeviceMaterials { get => deviceMaterials; set => deviceMaterials = value; }

    public partial class Chunk09079009
    {
        public override void ReadWrite(CPlugMaterial n, GbxReaderWriter rw)
        {
            rw.NodeRef(ref n.shaderRef);

            if ((n.shaderRef?.Kind ?? NodeRefKind.None) != NodeRefKind.None)
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
            rw.NodeRef(ref n.shaderRef);

            if ((n.shaderRef?.Kind ?? NodeRefKind.None) != NodeRefKind.None)
            {
                return;
            }

            rw.ArrayReadableWritable(ref n.deviceMaterials, version: 0xD);

            rw.Array<int>(ref U01);
        }
    }

}
