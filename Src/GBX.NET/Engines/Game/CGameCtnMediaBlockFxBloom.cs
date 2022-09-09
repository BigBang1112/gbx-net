namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Bloom effect for TMUF and older games. This node causes "Couldn't load map" in ManiaPlanet.
/// </summary>
/// <remarks>ID: 0x03083000</remarks>
[Node(0x03083000)]
[NodeExtension("GameCtnMediaBlockFxBloom")]
public partial class CGameCtnMediaBlockFxBloom : CGameCtnMediaBlockFx, CGameCtnMediaBlock.IHasKeys
{
    private IList<Key> keys;

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03083001))]
    public IList<Key> Keys { get => keys; set => keys = value; }

    protected CGameCtnMediaBlockFxBloom()
    {
        keys = Array.Empty<Key>();
    }

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnMediaBlockFxBloom 0x001 chunk
    /// </summary>
    [Chunk(0x03083001)]
    public class Chunk03083001 : Chunk<CGameCtnMediaBlockFxBloom>
    {
        public override void ReadWrite(CGameCtnMediaBlockFxBloom n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion
}
