namespace GBX.NET.Engines.Plug;

[Node(0x09021000)]
public class CPlugBitmapRenderLightFromMap : CPlugBitmapRender
{
    private int objectCountPerAxisMin;
    private int objectCountPerAxisMax;
    private float cameraNearZ_FactorInObject;
    private float cameraFarZ_ToAdd;

    internal CPlugBitmapRenderLightFromMap()
	{

	}

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09021001>]
    public int ObjectCountPerAxisMin { get => objectCountPerAxisMin; set => objectCountPerAxisMin = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09021001>]
    public int ObjectCountPerAxisMax { get => objectCountPerAxisMax; set => objectCountPerAxisMax = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09021001>]
    public float CameraNearZ_FactorInObject { get => cameraNearZ_FactorInObject; set => cameraNearZ_FactorInObject = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09021001>]
    public float CameraFarZ_ToAdd { get => cameraFarZ_ToAdd; set => cameraFarZ_ToAdd = value; }

    #region 0x001 chunk

    /// <summary>
    /// CPlugBitmapRenderLightFromMap 0x001 chunk
    /// </summary>
    [Chunk(0x09021001)]
    public class Chunk09021001 : Chunk<CPlugBitmapRenderLightFromMap>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;

        public override void ReadWrite(CPlugBitmapRenderLightFromMap n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.objectCountPerAxisMin);
            rw.Int32(ref n.objectCountPerAxisMax);
            rw.Single(ref n.cameraNearZ_FactorInObject);
            rw.Single(ref n.cameraFarZ_ToAdd);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
        }
    }

    #endregion
}
