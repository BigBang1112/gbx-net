namespace GBX.NET.Engines.Function;

[Node(0x0501E000)]
[NodeExtension("FuncTree")]
public class CFuncTreeRotate : CFuncTree
{
    private float angleMin;
    private float angleMax;

    public float AngleMin
    {
        get => angleMin;
        set => angleMin = value;
    }

    public float AngleMax
    {
        get => angleMax;
        set => angleMax = value;
    }

    protected CFuncTreeRotate()
    {

    }

    [Chunk(0x0501E001)]
    public class Chunk0501E001 : Chunk<CFuncTreeRotate>
    {
        public float U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CFuncTreeRotate n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref n.angleMin);
            rw.Single(ref n.angleMax);
        }
    }
}
