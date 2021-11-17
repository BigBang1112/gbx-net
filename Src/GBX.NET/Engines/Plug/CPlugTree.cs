namespace GBX.NET.Engines.Plug;

[Node(0x0904F000)]
public class CPlugTree : CPlug
{
    private IList<CPlugTree?>? children;
    private CPlugVisual? visual;
    private CPlug? shader;

    public IList<CPlugTree?>? Children
    {
        get => children;
        set => children = value;
    }

    public CPlugVisual? Visual
    {
        get => visual;
        set => visual = value;
    }

    public CPlug? Shader
    {
        get => shader;
        set => shader = value;
    }

    protected CPlugTree()
    {

    }

    [Chunk(0x0904F006)]
    public class Chunk0904F006 : Chunk<CPlugTree>
    {
        public int U01;

        public override void ReadWrite(CPlugTree n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01); // list version

            rw.List(ref n.children,
                (i, r) => r.ReadNodeRef<CPlugTree>(),
                (x, w) => w.Write(x));
        }
    }

    [Chunk(0x0904F00D)]
    public class Chunk0904F00D : Chunk<CPlugTree>
    {
        public string? U01;
        public int U02;

        public override void ReadWrite(CPlugTree n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01);
            rw.Int32(ref U02);
        }
    }

    [Chunk(0x0904F011)]
    public class Chunk0904F011 : Chunk<CPlugTree>
    {
        public int U01;

        public override void ReadWrite(CPlugTree n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    [Chunk(0x0904F016)]
    public class Chunk0904F016 : Chunk<CPlugTree>
    {
        public int U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CPlugTree n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CPlugVisual>(ref n.visual);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
        }
    }

    [Chunk(0x0904F01A)]
    public class Chunk0904F01A : Chunk<CPlugTree>
    {
        public int U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;
        public float U11;
        public float U12;
        public float U13;

        public override void ReadWrite(CPlugTree n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);
            rw.Single(ref U10);
            rw.Single(ref U11);
            rw.Single(ref U12);
            rw.Single(ref U13);
        }
    }
}
