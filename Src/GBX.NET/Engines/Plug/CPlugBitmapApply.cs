namespace GBX.NET.Engines.Plug;

[Node(0x09012000)]
public class CPlugBitmapApply : CPlugBitmapAddress
{
    protected CPlugBitmapApply()
    {

    }

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
