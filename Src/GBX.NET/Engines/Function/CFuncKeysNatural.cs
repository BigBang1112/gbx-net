namespace GBX.NET.Engines.Function;

[Node(0x05030000)]
public class CFuncKeysNatural : CFuncKeys
{
    private int[]? naturals;

    public int[]? Naturals
    {
        get => naturals;
        set => naturals = value;
    }

    protected CFuncKeysNatural()
    {

    }

    [Chunk(0x05030000)]
    public class Chunk05030000 : Chunk<CFuncKeysNatural>
    {
        public override void ReadWrite(CFuncKeysNatural n, GameBoxReaderWriter rw)
        {
            rw.Array<int>(ref n.naturals);
        }
    }
}
