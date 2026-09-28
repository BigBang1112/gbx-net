namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x05031000</remarks>
[Class(0x05031000)]
public partial class CFuncTreeSubVisualSequence : CFuncTree, IClass
{
    [Hexadecimal] public static new uint Id => 0x05031000;




    private CFuncKeysNatural? subKeys;
    [AppliedWithChunk<Chunk05031000>]
    [AppliedWithChunk<Chunk05031002>]
    public CFuncKeysNatural? SubKeys { get => subKeys; set => subKeys = value; }

    private bool simpleModeIsLooping;
    [AppliedWithChunk<Chunk05031003>]
    public bool SimpleModeIsLooping { get => simpleModeIsLooping; set => simpleModeIsLooping = value; }

    private int simpleModeStartIndex;
    [AppliedWithChunk<Chunk05031003>]
    public int SimpleModeStartIndex { get => simpleModeStartIndex; set => simpleModeStartIndex = value; }

    private int simpleModeEndIndex;
    [AppliedWithChunk<Chunk05031003>]
    public int SimpleModeEndIndex { get => simpleModeEndIndex; set => simpleModeEndIndex = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CFuncTreeSubVisualSequence"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CFuncTreeSubVisualSequence() { }


    /// <summary>
    /// CFuncTreeSubVisualSequence 0x000 chunk
    /// </summary>
    [Chunk(0x05031000)]
    public partial class Chunk05031000 : Chunk<CFuncTreeSubVisualSequence>
    {
        /// <inheritdoc />
        public override uint Id => 0x05031000;


        public override void ReadWrite(CFuncTreeSubVisualSequence n, GbxReaderWriter rw)
        {
            rw.Node<CFuncKeysNatural>(ref n.subKeys);
        }
    }

    /// <summary>
    /// CFuncTreeSubVisualSequence 0x001 chunk
    /// </summary>
    [Chunk(0x05031001)]
    public partial class Chunk05031001 : Chunk<CFuncTreeSubVisualSequence>
    {
        /// <inheritdoc />
        public override uint Id => 0x05031001;

        public string? U01;

        public override void ReadWrite(CFuncTreeSubVisualSequence n, GbxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }

    /// <summary>
    /// CFuncTreeSubVisualSequence 0x002 chunk
    /// </summary>
    [Chunk(0x05031002)]
    public partial class Chunk05031002 : Chunk<CFuncTreeSubVisualSequence>
    {
        /// <inheritdoc />
        public override uint Id => 0x05031002;


        public override void ReadWrite(CFuncTreeSubVisualSequence n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysNatural>(ref n.subKeys);
        }
    }

    /// <summary>
    /// CFuncTreeSubVisualSequence 0x003 chunk
    /// </summary>
    [Chunk(0x05031003)]
    public partial class Chunk05031003 : Chunk<CFuncTreeSubVisualSequence>
    {
        /// <inheritdoc />
        public override uint Id => 0x05031003;


        public override void ReadWrite(CFuncTreeSubVisualSequence n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.simpleModeIsLooping);
            rw.Int32(ref n.simpleModeStartIndex);
            rw.Int32(ref n.simpleModeEndIndex);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x05031000 => new Chunk05031000(),
        0x05031001 => new Chunk05031001(),
        0x05031002 => new Chunk05031002(),
        0x05031003 => new Chunk05031003(),
        _ => base.NewChunk(chunkId),
    };
}
