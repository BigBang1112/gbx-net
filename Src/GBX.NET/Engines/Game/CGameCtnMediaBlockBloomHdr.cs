namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Bloom HDR.
/// </summary>
/// <remarks>ID: 0x03128000</remarks>
[Node(0x03128000)]
[NodeExtension("GameCtnMediaBlockBloomHdr")]
public partial class CGameCtnMediaBlockBloomHdr : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    private IList<Key> keys;

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03128000))]
    [AppliedWithChunk(typeof(Chunk03128001))]
    [AppliedWithChunk(typeof(Chunk03128002))]
    public IList<Key> Keys { get => keys; set => keys = value; }

    protected CGameCtnMediaBlockBloomHdr()
    {
        keys = Array.Empty<Key>();
    }

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockBloomHdr 0x000 chunk
    /// </summary>
    [Chunk(0x03128000)]
    public class Chunk03128000 : Chunk<CGameCtnMediaBlockBloomHdr>
    {
        public override void ReadWrite(CGameCtnMediaBlockBloomHdr n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnMediaBlockBloomHdr 0x001 chunk
    /// </summary>
    [Chunk(0x03128001)]
    public class Chunk03128001 : Chunk03128000
    {

    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnMediaBlockBloomHdr 0x002 chunk
    /// </summary>
    [Chunk(0x03128002)]
    public class Chunk03128002 : Chunk03128000
    {
        
    }

    #endregion
    
    #endregion
}
