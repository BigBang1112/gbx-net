namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Bloom HDR (0x03128000)
/// </summary>
[Node(0x03128000)]
public partial class CGameCtnMediaBlockBloomHdr : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    #region Fields

    private IList<Key> keys;

    #endregion

    #region Properties

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    public IList<Key> Keys
    {
        get => keys;
        set => keys = value;
    }

    #endregion

    #region Constructors

    protected CGameCtnMediaBlockBloomHdr()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnMediaBlockBloomHdr 0x001 chunk
    /// </summary>
    [Chunk(0x03128001)]
    public class Chunk03128001 : Chunk<CGameCtnMediaBlockBloomHdr>
    {
        public override void ReadWrite(CGameCtnMediaBlockBloomHdr n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnMediaBlockBloomHdr 0x002 chunk
    /// </summary>
    [Chunk(0x03128002)]
    public class Chunk03128002 : Chunk<CGameCtnMediaBlockBloomHdr>
    {
        public override void ReadWrite(CGameCtnMediaBlockBloomHdr n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion
    
    #endregion
}
