namespace GBX.NET.Engines.Plug;

[Node(0x09006000)]
public class CPlugVisual : CPlug
{
    private IList<Vec4>? vertices;

    public IList<Vec4>? Vertices
    {
        get => vertices;
        set => vertices = value;
    }

    protected CPlugVisual()
    {

    }

    [Chunk(0x09006001)]
    public class Chunk09006001 : Chunk<CPlugVisual>
    {
        public string U01;

        public Chunk09006001()
        {
            U01 = "";
        }

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01!);
        }
    }

    [Chunk(0x09006004)]
    public class Chunk09006004 : Chunk<CPlugVisual>
    {
        public CMwNod? U01;

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01); // sometimes not present
        }
    }

    [Chunk(0x09006005)]
    public class Chunk09006005 : Chunk<CPlugVisual>
    {
        public int U01;

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01); // ArchiveCountAndElems, could have binary data with U01 length
        }
    }

    [Chunk(0x09006009)]
    public class Chunk09006009 : Chunk<CPlugVisual>
    {
        public float U01;

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    [Chunk(0x0900600B)]
    public class Chunk0900600B : Chunk<CPlugVisual>
    {
        public int U01;

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.Array<object>(null, (i, r) => new
            {
                x = r.ReadInt32(),
                y = r.ReadInt32(),
                
                vec1 = r.ReadVec3(), // GmBoxAligned::ArchiveABox
                vec2 = r.ReadVec3()
            }, (x, w) => { });
        }
    }

    [Chunk(0x0900600D)]
    public class Chunk0900600D : Chunk<CPlugVisual>
    {
        private int flags;

        public int Flags
        {
            get => flags;
            set => flags = value;
        }

        public int U01;
        public int U02;

        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref flags);
            // CFastBuffer::GetCount(); - could get from 0x00B

            rw.Int32(ref U01);
            rw.Int32(ref U02);

            var count = rw.Int32(); // could be vertex count

            // Array of node references using 'count'

            // Another array using 'count'

            // if((param_1_00 + 7) & 7) != 0 ----> CPlugVisual::ArchiveSkinData

            U03 = rw.Single(); // ArchiveABox
            U04 = rw.Single();
            U05 = rw.Single();
            U06 = rw.Single();
            U07 = rw.Single();
            U08 = rw.Single();

            // Count + byte array probably
        }
    }

    [Chunk(0x0900600E)]
    public class Chunk0900600E : Chunk<CPlugVisual>
    {
        private int flags;

        public int Flags
        {
            get => flags;
            set => flags = value;
        }

        public int U01;
        public int U02;

        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            var flags = rw.Bytes(count: 4);
            // CFastBuffer::GetCount(); - could get from 0x00B

            var count1 = rw.Int32(); // count?
            rw.Int32(ref U02);

            throw new Exception();
        }
    }
}
