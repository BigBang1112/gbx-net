namespace GBX.NET.Engines.Control;

[Node(0x07001000)]
public abstract class CControlBase : CSceneToy
{
    private string? stackText;
    private CControlLayout? layout;

    [NodeMember(ExactlyNamed = true)]
    public string? StackText { get => stackText; set => stackText = value; }

    public CControlLayout? Layout { get => layout; set => layout = value; }

    protected CControlBase()
    {

    }

    [Chunk(0x0700100C)]
    public class Chunk0700100C : Chunk<CControlBase>
    {
        public int U01;
        public int U02;
        public int U03;
        public int U04;

        public override void ReadWrite(CControlBase n, GameBoxReaderWriter rw, ILogger? logger)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.String(ref n.stackText);
        }
    }

    [Chunk(0x0700100E)]
    public class Chunk0700100E : Chunk<CControlBase>
    {
        public Box U01;
        public int U03;
        public int U04;

        public override void ReadWrite(CControlBase n, GameBoxReaderWriter rw, ILogger? logger)
        {
            rw.Box(ref U01);
            rw.NodeRef<CControlLayout>(ref n.layout);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
        }
    }

    [Chunk(0x0700100F)]
    public class Chunk0700100F : Chunk<CControlBase>
    {
        public string? U01;

        public override void ReadWrite(CControlBase n, GameBoxReaderWriter rw, ILogger? logger)
        {
            rw.String(ref U01);
        }
    }

    [Chunk(0x07001010)]
    public class Chunk07001010 : Chunk<CControlBase>
    {
        public Node? U01;
        public int? U02;

        public override void ReadWrite(CControlBase n, GameBoxReaderWriter rw, ILogger? logger)
        {
            // CControlBase::CStyleSheetElem<class CControlStyle>::Archive
            rw.NodeRef(ref U01);

            if (U01 is null)
            {
                rw.Int32(ref U02);
            }
        }
    }
}
