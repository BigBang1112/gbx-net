namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Music effect (0x030A6000)
/// </summary>
[Node(0x030A6000)]
[NodeExtension("CtnMediaBlockMusicFx")]
public partial class CGameCtnMediaBlockMusicEffect : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
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

    protected CGameCtnMediaBlockMusicEffect()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    [Chunk(0x030A6000)]
    public class Chunk030A6000 : Chunk<CGameCtnMediaBlockMusicEffect>
    {
        public override void ReadWrite(CGameCtnMediaBlockMusicEffect n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion

    #region 0x001 chunk

    [Chunk(0x030A6001)]
    public class Chunk030A6001 : Chunk<CGameCtnMediaBlockMusicEffect>
    {
        public override void ReadWrite(CGameCtnMediaBlockMusicEffect n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!, version: 1);
        }
    }

    #endregion

    #endregion
}
