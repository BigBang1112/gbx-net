namespace GBX.NET.Engines.Function;

/// <summary>
/// CFuncKeys (0x05002000)
/// </summary>
[Node(0x05002000)]
public abstract class CFuncKeys : CFunc
{
    private float[]? xs;

    public float[]? Xs
    {
        get => xs;
        set => xs = value;
    }

    protected CFuncKeys()
    {

    }

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
            rw.Array<float>(ref n.xs);
        }
    }

    #endregion

    #region 0x003 chunk

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
