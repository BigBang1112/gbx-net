namespace GBX.NET.Engines.Plug;

[Node(0x09091000)]
public class CPlugBitmapRenderSub : CPlugBitmapRender
{
	internal CPlugBitmapRenderSub()
	{

	}

    #region 0x000 chunk

    /// <summary>
    /// CPlugBitmapRenderSub 0x000 chunk
    /// </summary>
    [Chunk(0x09091000)]
    public class Chunk09091000 : Chunk<CPlugBitmapRenderSub>
    {
        private CMwNod? U01;

        public override void ReadWrite(CPlugBitmapRenderSub n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    #endregion
}