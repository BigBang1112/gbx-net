namespace GBX.NET.Engines.Function;

[Node(0x05031000)]
public class CFuncTreeSubVisualSequence : CFuncTree
{
    private CFuncKeysNatural? subKeys;

    public CFuncKeysNatural? SubKeys
    {
        get => subKeys;
        set => subKeys = value;
    }

    protected CFuncTreeSubVisualSequence()
    {

    }

    [Chunk(0x05031000)]
    public class Chunk05031000 : Chunk<CFuncTreeSubVisualSequence>
    {
        public override void Read(CFuncTreeSubVisualSequence n, GameBoxReader r)
        {
            n.subKeys = Parse<CFuncKeysNatural>(r, 0x05030000);
        }

        public override void Write(CFuncTreeSubVisualSequence n, GameBoxWriter w)
        {
            if (n.subKeys is null)
            {
                w.Write(-1);
                return;
            }

            n.subKeys.Write(w);
        }
    }

    [Chunk(0x05031001)]
    public class Chunk05031001 : Chunk<CFuncTreeSubVisualSequence>
    {
        public string? U01;

        public override void ReadWrite(CFuncTreeSubVisualSequence n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }
}
