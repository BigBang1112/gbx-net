namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Coloring capturable (0x0316C000)
/// </summary>
[Node(0x0316C000)]
[NodeExtension("GameCtnMediaBlockColoringCapturable")]
public partial class CGameCtnMediaBlockColoringCapturable : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
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

    protected CGameCtnMediaBlockColoringCapturable()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockColoringCapturable 0x000 chunk
    /// </summary>
    [Chunk(0x0316C000)]
    public class Chunk0316C000 : Chunk<CGameCtnMediaBlockColoringCapturable>, IVersionable
    {
        private int version = 2;

        public int U01;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnMediaBlockColoringCapturable n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version); // If version is below 2, there is an loop of two floats and no U01
            rw.Int32(ref U01);

            rw.ListKey(ref n.keys!);
        }
    }

    #endregion

    #endregion
}
