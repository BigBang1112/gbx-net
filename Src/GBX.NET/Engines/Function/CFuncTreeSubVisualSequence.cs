using static GBX.NET.Engines.Function.CFuncShaderLayerUV;

namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x05031000</remarks>
[Node(0x05031000)]
public class CFuncTreeSubVisualSequence : CFuncTree
{
    #region Fields

    private CFuncKeysNatural? subKeys;
    private bool simpleModeIsLooping;
    private int simpleModeStartIndex;
    private int simpleModeEndIndex;

    #endregion

    #region Properties

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk05031000))]
    [AppliedWithChunk(typeof(Chunk05031002))]
    public CFuncKeysNatural? SubKeys { get => subKeys; set => subKeys = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk05031003))]
    public bool SimpleModeIsLooping { get => simpleModeIsLooping; set => simpleModeIsLooping = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk05031003))]
    public int SimpleModeStartIndex { get => simpleModeStartIndex; set => simpleModeStartIndex = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk05031003))]
    public int SimpleModeEndIndex { get => simpleModeEndIndex; set => simpleModeEndIndex = value; }

    #endregion

    #region Constructors

    protected CFuncTreeSubVisualSequence()
    {

    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CFuncTreeSubVisualSequence 0x000 chunk
    /// </summary>
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

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CFuncTreeSubVisualSequence 0x001 chunk
    /// </summary>
    [Chunk(0x05031001)]
    public class Chunk05031001 : Chunk<CFuncTreeSubVisualSequence>
    {
        public string? U01;

        public override void ReadWrite(CFuncTreeSubVisualSequence n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CFuncTreeSubVisualSequence 0x002 chunk
    /// </summary>
    [Chunk(0x05031002)]
    public class Chunk05031002 : Chunk<CFuncTreeSubVisualSequence>
    {
        public override void ReadWrite(CFuncTreeSubVisualSequence n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref n.subKeys);
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CFuncTreeSubVisualSequence 0x003 chunk
    /// </summary>
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

    #endregion

    #endregion
}
