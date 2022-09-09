namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Color effect.
/// </summary>
/// <remarks>ID: 0x03080000</remarks>
[Node(0x03080000)]
[NodeExtension("GameCtnMediaBlockFxColors")]
public partial class CGameCtnMediaBlockFxColors : CGameCtnMediaBlockFx, CGameCtnMediaBlock.IHasKeys
{
    private IList<Key> keys;

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    public IList<Key> Keys { get => keys; set => keys = value; }

    protected CGameCtnMediaBlockFxColors()
    {
        keys = Array.Empty<Key>();
    }

    #region Chunks

    #region 0x000 chunk

    [Chunk(0x03080000)]
    public class Chunk03080000 : Chunk<CGameCtnMediaBlockFxColors>
    {
        public override void ReadWrite(CGameCtnMediaBlockFxColors n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion

    #region 0x001 chunk

    [Chunk(0x03080001)]
    public class Chunk03080001 : Chunk03080000
    {

    }

    #endregion

    #region 0x002 chunk

    [Chunk(0x03080002)]
    public class Chunk03080002 : Chunk03080000
    {

    }

    #endregion

    #region 0x003 chunk

    [Chunk(0x03080003)]
    public class Chunk03080003 : Chunk03080000
    {
        
    }

    #endregion

    #endregion
}
