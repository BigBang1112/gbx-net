namespace GBX.NET.Engines.Function;

[Node(0x05031000)]
public class CFuncTreeSubVisualSequence : CFuncTree
{
    private CFuncKeysNatural? subKeys;
    private bool simpleModeIsLooping;
    private int simpleModeStartIndex;
    private int simpleModeEndIndex;

    public CFuncKeysNatural? SubKeys
    {
        get => subKeys;
        set => subKeys = value;
    }

    public bool SimpleModeIsLooping { get => simpleModeIsLooping; set => simpleModeIsLooping = value; }
    public int SimpleModeStartIndex { get => simpleModeStartIndex; set => simpleModeStartIndex = value; }
    public int SimpleModeEndIndex { get => simpleModeEndIndex; set => simpleModeEndIndex = value; }

    protected CFuncTreeSubVisualSequence()
    {

    }

    [Chunk(0x05031000)]
    public class Chunk05031000 : Chunk<CFuncTreeSubVisualSequence>
    {
        public override void Read(CFuncTreeSubVisualSequence n, GameBoxReader r, ILogger? logger)
        {
            n.subKeys = Parse<CFuncKeysNatural>(r, 0x05030000, progress: null, logger);
        }

        public override void Write(CFuncTreeSubVisualSequence n, GameBoxWriter w, ILogger? logger)
        {
            if (n.subKeys is null)
            {
                w.Write(-1);
                return;
            }

            n.subKeys.Write(w, logger);
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

    [Chunk(0x05031002)]
    public class Chunk05031002 : Chunk<CFuncTreeSubVisualSequence>
    {
        public override void ReadWrite(CFuncTreeSubVisualSequence n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref n.subKeys);
        }
    }

    [Chunk(0x05031003)]
    public class Chunk05031003 : Chunk<CFuncTreeSubVisualSequence>
    {
        public override void ReadWrite(CFuncTreeSubVisualSequence n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.simpleModeIsLooping);
            rw.Int32(ref n.simpleModeStartIndex);
            rw.Int32(ref n.simpleModeEndIndex);
        }
    }
}
