namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Music effect.
/// </summary>
/// <remarks>ID: 0x030A6000</remarks>
[Node(0x030A6000)]
[NodeExtension("CtnMediaBlockMusicFx")]
public partial class CGameCtnMediaBlockMusicEffect : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    private IList<Key> keys;

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk030A6000))]
    [AppliedWithChunk(typeof(Chunk030A6001))]
    public IList<Key> Keys { get => keys; set => keys = value; }

    protected CGameCtnMediaBlockMusicEffect()
    {
        keys = Array.Empty<Key>();
    }

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
