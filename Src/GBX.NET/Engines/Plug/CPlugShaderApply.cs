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
        public int U02;
        public CMwNod? U03;
        public int U04;
        public int U05;
        public int U06;
        public int U07;

        public override void ReadWrite(CPlugShaderApply n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.NodeRef(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
            rw.Int32(ref U07);
        }
    }
}
