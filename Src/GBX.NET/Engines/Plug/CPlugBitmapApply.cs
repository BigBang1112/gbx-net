namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09012000</remarks>
[Node(0x09012000)]
public class CPlugBitmapApply : CPlugBitmapAddress
{
    internal CPlugBitmapApply()
    {

    }

    /// <summary>
    /// CPlugBitmapApply 0x003 chunk
    /// </summary>
    [Chunk(0x09012003)]
    public class Chunk090120003 : Chunk<CPlugBitmapApply>
    {
        public int U01;
        public int U02;

        public override void ReadWrite(CPlugBitmapApply n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }
}
