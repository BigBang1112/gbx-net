namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Depth of field.
/// </summary>
/// <remarks>ID: 0x03126000</remarks>
[Node(0x03126000)]
[NodeExtension("GameCtnMediaBlockDOF")]
public partial class CGameCtnMediaBlockDOF : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    private IList<Key> keys;

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03126000))]
    [AppliedWithChunk(typeof(Chunk03126001))]
    [AppliedWithChunk(typeof(Chunk03126002))]
    public IList<Key> Keys { get => keys; set => keys = value; }

    protected CGameCtnMediaBlockDOF()
    {
        keys = Array.Empty<Key>();
    }

    #region Chunks

    #region 0x000 chunk (version 0)

    /// <summary>
    /// CGameCtnMediaBlockDOF 0x000 chunk (version 0)
    /// </summary>
    [Chunk(0x03126000, "version 0")]
    public class Chunk03126000 : Chunk<CGameCtnMediaBlockDOF>
    {
        public override void ReadWrite(CGameCtnMediaBlockDOF n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion

    #region 0x001 chunk (version 1)

    /// <summary>
    /// CGameCtnMediaBlockDOF 0x001 chunk (version 1)
    /// </summary>
    [Chunk(0x03126001, "version 1")]
    public class Chunk03126001 : Chunk<CGameCtnMediaBlockDOF>
    {
        public override void ReadWrite(CGameCtnMediaBlockDOF n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!, version: 1);
        }
    }

    #endregion

    #region 0x002 chunk (version 2)

    /// <summary>
    /// CGameCtnMediaBlockDOF 0x002 chunk (version 2)
    /// </summary>
    [Chunk(0x03126002, "version 2")]
    public class Chunk03126002 : Chunk<CGameCtnMediaBlockDOF>
    {
        public override void ReadWrite(CGameCtnMediaBlockDOF n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!, version: 2);
        }
    }

    #endregion

    #endregion
}
