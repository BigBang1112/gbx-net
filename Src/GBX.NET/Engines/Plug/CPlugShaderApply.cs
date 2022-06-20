namespace GBX.NET.Engines.Plug;

[Node(0x09026000)]
public class CPlugShaderApply : CPlugShaderGeneric
{
    protected CPlugShaderApply()
    {

    }

    [Chunk(0x09026001)]
    public class Chunk09026001 : Chunk<CPlugShaderApply>
    {
        public int U01;
        public int U02;
        public int U03;
        public int U04;

        public override void ReadWrite(CPlugShaderApply n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04); // DoData
        }
    }

    [Chunk(0x09026002)]
    public class Chunk09026002 : Chunk<CPlugShaderApply>
    {
        public int U01;

        public override void ReadWrite(CPlugShaderApply n, GameBoxReaderWriter rw)
        {
            // array of CPlugBitmapAddress
            rw.Int32(ref U01);
        }
    }

    [Chunk(0x09026004)]
    public class Chunk09026004 : Chunk<CPlugShaderApply>
    {
        public int U01;

        public override void ReadWrite(CPlugShaderApply n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    [Chunk(0x09026008)]
    public class Chunk09026008 : Chunk<CPlugShaderApply>
    {
        public int U01;
        public int U02;

        public override void ReadWrite(CPlugShaderApply n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }
}
