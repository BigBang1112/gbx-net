namespace GBX.NET.Engines.Hms;

/// <summary>
/// CHmsItem (0x06003000)
/// </summary>
[Node(0x06003000)]
public class CHmsItem : CMwNod
{
    private CPlugSolid solid;

    public CPlugSolid Solid
    {
        get => solid;
        set => solid = value;
    }

    protected CHmsItem()
    {
        solid = null!;
    }

    [Chunk(0x06003001)]
    public class Chunk06003001 : Chunk<CHmsItem>
    {
        public override void ReadWrite(CHmsItem n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CPlugSolid>(ref n.solid!);
        }
    }

    [Chunk(0x06003011)]
    public class Chunk06003011 : Chunk<CHmsItem>
    {
        private ulong U01;
        private short U02;

        public override void ReadWrite(CHmsItem n, GameBoxReaderWriter rw)
        {
            rw.UInt64(ref U01);
            rw.Int16(ref U02);
        }
    }
}
