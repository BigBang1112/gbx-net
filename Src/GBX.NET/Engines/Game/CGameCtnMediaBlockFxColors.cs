namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Color effect.
/// </summary>
/// <remarks>ID: 0x03080000</remarks>
[Node(0x03080000)]
[NodeExtension("GameCtnMediaBlockFxColors")]
public partial class CGameCtnMediaBlockFxColors : CGameCtnMediaBlockFx, CGameCtnMediaBlock.IHasKeys
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

    protected CGameCtnMediaBlockFxColors()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x003 chunk

    [Chunk(0x03080003)]
    public class Chunk03080003 : Chunk<CGameCtnMediaBlockFxColors>
    {
        public override void ReadWrite(CGameCtnMediaBlockFxColors n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion

    #endregion
}
