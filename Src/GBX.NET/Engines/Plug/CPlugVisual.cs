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
        public int U01;

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {

        }
    }

    [Chunk(0x09006005)]
    public class Chunk09006005 : Chunk<CPlugVisual>
    {
        public int U01;

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    [Chunk(0x09006009)]
    public class Chunk09006009 : Chunk<CPlugVisual>
    {
        public int U01;

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    [Chunk(0x0900600B)]
    public class Chunk0900600B : Chunk<CPlugVisual>
    {
        public int U01;

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);

            /*if (U01 != 0)
            {
                var wtf = rw.Reader.ReadInt32();
                var ok = rw.Reader.ReadInt32();
                var dshsdh = rw.Reader.ReadBytes(82);
                var list = new List<Vec3>();
                var list2 = new List<Vec3>();
                var list3 = new List<Vec2>();
                var list4 = new List<Vec2>();
                for (var i = 0; i < 173; i++)
                {
                    list.Add(rw.Reader.ReadVec3());
                    list2.Add(rw.Reader.ReadVec3());
                    list3.Add(rw.Reader.ReadVec2());
                }

                for (var i = 0; i < 6; i++)
                {
                    list4.Add(rw.Reader.ReadVec2());
                }

                var gdsg = rw.Reader.ReadArray<float>(30);
                /*var dshsdh = rw.Reader.ReadArray<short>(4);
                var dshsdh2 = rw.Reader.ReadInt32();
                var sh = rw.Reader.ReadInt16();
                var gsgdssagsdgdh = rw.Reader.ReadBytes(3);
                var dshssasdh2 = rw.Reader.ReadInt32();
                var ok = rw.Reader.ReadInt32();

                var dafagasgasgasga = rw.Reader.ReadInt32();
                var dafagasgasgasssaga = rw.Reader.ReadInt32();
                var wawtwa = rw.Reader.ReadInt32();
                var watawtwat = rw.Reader.ReadInt32();
                var hdshd = rw.Reader.ReadArray<int>(17);

            }*/
        }
    }

    [Chunk(0x0900600E)]
    public class Chunk0900600E : Chunk<CPlugVisual>
    {
        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            /*var integery = rw.Reader.ReadArray<int>(2);
            var count = rw.Reader.ReadInt32();
            var gdsfsagsaghsdh = rw.Reader.ReadArray<float>(count + 16);

            var ingorovat = rw.Reader.ReadArray<int>(6);
            var node = rw.Reader.ReadNodeRef();

            var gdsghsdh = rw.Reader.ReadArray<int>(2);
            var num = rw.Reader.ReadInt32();
            var gdsghfsasdh = rw.Reader.ReadArray<int>(2);
            var kek = new Vec2[num];
            var kek2 = new Vec3[num];
            for (var i = 0; i < num; i++)
            {
                kek[i] = rw.Reader.ReadVec2();
            }

            var gdsggshsdh = rw.Reader.ReadArray<float>(6);
            var wat = rw.Reader.ReadInt32();*/
        }
    }
}
