namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Fog.
/// </summary>
/// <remarks>ID: 0x03199000</remarks>
[Node(0x03199000)]
[NodeExtension("GameCtnMediaBlockFog")]
public partial class CGameCtnMediaBlockFog : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    #region Fields

    private IList<Key> keys;

    #endregion

    #region Constructors

    protected CGameCtnMediaBlockFog()
    {
        keys = null!;
    }

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

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockFog 0x000 chunk
    /// </summary>
    [Chunk(0x03199000)]
    public class Chunk03199000 : Chunk<CGameCtnMediaBlockFog>, IVersionable
    {
        private int version;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnMediaBlockFog n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.ListKey(ref n.keys!, version);
        }
    }

    #endregion

    #endregion
}
