namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Blur depth effect.
/// </summary>
/// <remarks>ID: 0x03081000</remarks>
[Node(0x03081000)]
[NodeExtension("GameCtnMediaBlockFxBlurDepth")]
public partial class CGameCtnMediaBlockFxBlurDepth : CGameCtnMediaBlockFx, CGameCtnMediaBlock.IHasKeys
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

    protected CGameCtnMediaBlockFxBlurDepth()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnMediaBlockFxBlurDepth 0x001 chunk
    /// </summary>
    [Chunk(0x03081001)]
    public class Chunk03081001 : Chunk<CGameCtnMediaBlockFxBlurDepth>
    {
        public override void ReadWrite(CGameCtnMediaBlockFxBlurDepth n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion

    #endregion
}
