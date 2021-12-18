namespace GBX.NET.Engines.Function;

[Node(0x0500B000)]
public abstract class CFuncPlug : CFunc
{
    protected CFuncPlug()
    {

    }

    [Chunk(0x0500B004)]
    public class Chunk0500B004 : Chunk<CFuncPlug>
    {
        public float U01;
        public float U02;
        public bool U03;
        public bool U04;

        public override void ReadWrite(CFuncPlug n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Boolean(ref U03);
            rw.Boolean(ref U04);
        }
    }
}
