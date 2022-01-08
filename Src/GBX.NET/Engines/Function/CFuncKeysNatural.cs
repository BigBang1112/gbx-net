namespace GBX.NET.Engines.Function;

/// <summary>
/// CFuncKeysNatural (0x05030000)
/// </summary>
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

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CFuncKeysNatural 0x000 chunk
    /// </summary>
    [Chunk(0x05030000)]
    public class Chunk05030000 : Chunk<CFuncKeysNatural>
    {
        public override void ReadWrite(CFuncKeysNatural n, GameBoxReaderWriter rw)
        {
            rw.Array<int>(ref n.naturals);
        }
    }

    #endregion

    #endregion
}
