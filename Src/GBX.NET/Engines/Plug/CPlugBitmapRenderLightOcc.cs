namespace GBX.NET.Engines.Plug;

[Node(0x0909F000)]
public class CPlugBitmapRenderLightOcc : CPlugBitmapRender
{
    private float fovY;

    [NodeMember]
    [AppliedWithChunk<Chunk0909F000>]
    public float FovY { get => fovY; set => fovY = value; }

    internal CPlugBitmapRenderLightOcc()
	{

	}

    #region 0x000 chunk

    /// <summary>
    /// CPlugBitmapRenderLightOcc 0x000 chunk
    /// </summary>
    [Chunk(0x0909F000)]
    public class Chunk0909F000 : Chunk<CPlugBitmapRenderLightOcc>
    {
        public float U01;
        public Node? U02;

        public override void ReadWrite(CPlugBitmapRenderLightOcc n, GameBoxReaderWriter rw)
        {
            rw.Single(ref n.fovY);
            rw.Single(ref U01);
            rw.NodeRef(ref U02);
        }
    }

    #endregion
}
