using GBX.NET.Engines.Control;

namespace GBX.NET.Engines.Game;

public partial class CGameMenu
{
    public CControlContainer.Child[]? Frames { get; set; }
    public int MainFrameIndex { get; set; }

    public partial class Chunk03009003
    {
        public CMwNod? U01;

        public override void ReadWrite(CGameMenu n, GbxReaderWriter rw)
        {
            var count = rw.Int32(n.Frames?.Length ?? 0);

            if (rw.Reader is not null)
            {
                n.Frames = new CControlContainer.Child[count];
            }

            for (var i = 0; i < count; i++)
            {
                var frame = n.Frames![i] ??= new();
                frame.ReadWriteModel(rw);
            }

            n.MainFrameIndex = rw.Int32(n.MainFrameIndex);
            rw.NodeRef(ref U01);
        }
    }

    public partial class Chunk03009006
    {
        public CMwNod? U01;
        public float U02;
        public float U03;
        public float U04;
        public bool U05;
        public (Iso4, Iso4, bool)[]? U06;

        public override void ReadWrite(CGameMenu n, GbxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Boolean(ref U05);

            var count = n.Frames?.Length ?? 0;

            if (rw.Reader is not null)
            {
                U06 = new (Iso4, Iso4, bool)[count];
            }

            for (var i = 0; i < count; i++)
            {
                var (a, b, c) = U06![i];
                rw.Iso4(ref a);
                rw.Iso4(ref b);
                rw.Boolean(ref c);
                U06[i] = (a, b, c);
            }
        }
    }
}
