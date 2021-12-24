namespace GBX.NET.Engines.Plug;

/// <summary>
/// Official mesh/model (0x09005000)
/// </summary>
[Node(0x09005000), WritingNotSupported]
public sealed class CPlugSolid : CPlug
{
    private CPlugTree? tree;

    public CPlugTree? Tree
    {
        get => tree;
        set => tree = value;
    }

    private CPlugSolid()
    {

    }

    [Chunk(0x09005000)]
    public class Chunk09005000 : Chunk<CPlugSolid>
    {
        public int U01;

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    [Chunk(0x09005006)]
    public class Chunk09005006 : Chunk<CPlugSolid>
    {
        public float U01;
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
        public float U14;
        public float U15;

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
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
            rw.Single(ref U14);
            rw.Single(ref U15);
        }
    }

    [Chunk(0x09005007)]
    public class Chunk09005007 : Chunk<CPlugSolid>
    {
        public bool U01;

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    [Chunk(0x0900500B)]
    public class Chunk0900500B : Chunk<CPlugSolid>
    {
        public bool U01;
        public bool U02;
        public bool U03;
        public bool U04;
        public bool U05;
        public bool U06;
        public int U07;

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.Boolean(ref U03);
            rw.Boolean(ref U04);
            rw.Boolean(ref U05);
            rw.Boolean(ref U06);
            rw.Int32(ref U07);
        }
    }

    [Chunk(0x0900500C)]
    public class Chunk0900500C : Chunk<CPlugSolid>
    {
        public bool U01;
        public bool U02;
        public bool U03;
        public bool U04;
        public bool U05;
        public bool U06;
        public bool U07;
        public bool U08;
        public bool U09;
        public bool U10;
        public float U11;
        public int U12;
        public float U13;
        public float U14;
        public float U15;
        public float U16;
        public int U17;
        public int U18;

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.Boolean(ref U03);
            rw.Boolean(ref U04);
            rw.Boolean(ref U05);
            rw.Boolean(ref U06);
            rw.Boolean(ref U07);
            rw.Boolean(ref U08);
            rw.Boolean(ref U09);
            rw.Boolean(ref U10);
            rw.Single(ref U11);
            rw.Int32(ref U12);
            rw.Single(ref U13);
            rw.Single(ref U14);
            rw.Single(ref U15);
            rw.Single(ref U16);
            rw.Int32(ref U17);
            rw.Int32(ref U18);
        }
    }

    [Chunk(0x0900500D)]
    public class Chunk0900500D : Chunk<CPlugSolid>
    {
        public bool U01;
        public bool U02;

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.NodeRef<CPlugTree>(ref n.tree);
        }
    }

    [Chunk(0x0900500E)]
    public class Chunk0900500E : Chunk<CPlugSolid>
    {
        public float U01;
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
        public float U14;
        public float U15;
        public float U16;

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);

            rw.Single(ref U05); // 3x3 matrix
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);
            rw.Single(ref U10);
            rw.Single(ref U11);
            rw.Single(ref U12);
            rw.Single(ref U13);

            rw.Single(ref U14);
            rw.Single(ref U15);
            rw.Single(ref U16);
        }
    }

    [Chunk(0x0900500F)]
    public class Chunk0900500F : Chunk<CPlugSolid>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    [Chunk(0x09005010)]
    public class Chunk09005010 : Chunk<CPlugSolid>
    {
        public CMwNod? U01;

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    [Chunk(0x09005011)]
    public class Chunk09005011 : Chunk<CPlugSolid>
    {
        public bool U01;
        public bool U02;

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.NodeRef(ref n.tree);
        }
    }

    [Chunk(0x09005012)]
    public class Chunk09005012 : Chunk<CPlugSolid>
    {
        public byte U01;

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Byte(ref U01);
        }
    }
}
