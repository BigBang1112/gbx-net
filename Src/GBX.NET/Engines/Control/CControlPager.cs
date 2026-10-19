namespace GBX.NET.Engines.Control;

public partial class CControlPager
{
    public CControlContainer.Child LabelPageCounter { get; set; } = new();
    public CControlContainer.Child ButtonPrevPage { get; set; } = new();
    public CControlContainer.Child ButtonNextPage { get; set; } = new();
    public CControlContainer.Child ButtonFastPrevPage { get; set; } = new();
    public CControlContainer.Child ButtonFastNextPage { get; set; } = new();
    public CControlContainer.Child ButtonFirstPage { get; set; } = new();
    public CControlContainer.Child ButtonLastPage { get; set; } = new();
    public CControlContainer.Child QuadPageCounter { get; set; } = new();

    public partial class Chunk07030000
    {
        public string? U01;
        public CControlStyle? U02;
        public string? U03;
        public CControlStyle? U04;

        public override void ReadWrite(CControlPager n, GbxReaderWriter rw)
        {
            n.LabelPageCounter.ReadWritePointer(rw);
            n.ButtonPrevPage.ReadWritePointer(rw);
            n.ButtonNextPage.ReadWritePointer(rw);
            n.ButtonFastPrevPage.ReadWritePointer(rw);
            n.ButtonFastNextPage.ReadWritePointer(rw);
            n.ButtonFirstPage.ReadWritePointer(rw);
            n.ButtonLastPage.ReadWritePointer(rw);

            // Style sheet elements are discarded after reading.
            rw.Id(ref U01);
            if (string.IsNullOrEmpty(U01)) rw.NodeRef(ref U02);
            rw.Id(ref U03);
            if (string.IsNullOrEmpty(U03)) rw.NodeRef(ref U04);
        }
    }

    public partial class Chunk07030002
    {
        public override void ReadWrite(CControlPager n, GbxReaderWriter rw)
        {
            n.QuadPageCounter.ReadWritePointer(rw);
        }
    }
}
