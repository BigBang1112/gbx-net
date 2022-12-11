namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Blur depth effect.
/// </summary>
/// <remarks>ID: 0x03081000</remarks>
[Node(0x03081000)]
[NodeExtension("GameCtnMediaBlockFxBlurDepth")]
public partial class CGameCtnMediaBlockFxBlurDepth : CGameCtnMediaBlockFx, CGameCtnMediaBlock.IHasKeys
{
    private IList<Key> keys;

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    [AppliedWithChunk<Chunk03081001>]
    public IList<Key> Keys { get => keys; set => keys = value; }

    internal CGameCtnMediaBlockFxBlurDepth()
    {
        keys = Array.Empty<Key>();
    }

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
}
