namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x05030000</remarks>
[Node(0x05030000)]
public class CFuncKeysNatural : CFuncKeys
{
    #region Fields

    private int[]? naturals;

    #endregion

    #region Properties

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk05030000>]
    public int[]? Naturals { get => naturals; set => naturals = value; }

    #endregion

    internal CFuncKeysNatural()
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
