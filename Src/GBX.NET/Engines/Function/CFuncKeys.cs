namespace GBX.NET.Engines.Function;

[Node(0x05002000)]
public abstract class CFuncKeys : CFunc
{
    private float[]? xs;

    public float[]? Xs
    {
        get => xs;
        set => xs = value;
    }

    protected CFuncKeys()
    {

    }

    [Chunk(0x05002001)]
    public class Chunk05002001 : Chunk<CFuncKeys>
    {
        public override void ReadWrite(CFuncKeys n, GameBoxReaderWriter rw)
        {
            rw.Array<float>(ref n.xs);
        }
    }

    [Chunk(0x05002003)]
    public class Chunk05002003 : Chunk<CFuncKeys>
    {
        public string? U01;

        public override void ReadWrite(CFuncKeys n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }
}
