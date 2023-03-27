namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x05002000</remarks>
[Node(0x05002000)]
[NodeExtension("FuncKey")]
public abstract class CFuncKeys : CFunc
{
    #region Fields

    private float[] xs = Array.Empty<float>();

    #endregion

    #region Properties

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk05002001>]
    public float[] Xs { get => xs; set => xs = value; }

    #endregion

    #region Constructors

    internal CFuncKeys()
    {

    }

    #endregion

    #region Chunks

    #region 0x001 chunk 

    /// <summary>
    /// CFuncKeys 0x001 chunk
    /// </summary>
    [Chunk(0x05002001)]
    public class Chunk05002001 : Chunk<CFuncKeys>
    {
        public override void ReadWrite(CFuncKeys n, GameBoxReaderWriter rw)
        {
            rw.Array<float>(ref n.xs!);
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CFuncKeys 0x003 chunk
    /// </summary>
    [Chunk(0x05002003)]
    public class Chunk05002003 : Chunk<CFuncKeys>
    {
        public string? U01;

        public override void ReadWrite(CFuncKeys n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }

    #endregion

    #endregion
}
