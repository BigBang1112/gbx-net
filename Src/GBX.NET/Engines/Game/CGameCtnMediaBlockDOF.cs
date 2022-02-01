namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Depth of field (0x03126000)
/// </summary>
[Node(0x03126000)]
[NodeExtension("GameCtnMediaBlockDOF")]
public partial class CGameCtnMediaBlockDOF : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
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

    protected CGameCtnMediaBlockDOF()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    [Chunk(0x03126000)]
    public class Chunk03126000 : Chunk<CGameCtnMediaBlockDOF>
    {
        public override void ReadWrite(CGameCtnMediaBlockDOF n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion

    #region 0x001 chunk

    [Chunk(0x03126001)]
    public class Chunk03126001 : Chunk<CGameCtnMediaBlockDOF>
    {
        public override void ReadWrite(CGameCtnMediaBlockDOF n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!, version: 1);
        }
    }

    #endregion

    #region 0x002 chunk

    [Chunk(0x03126002)]
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
