namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Camera blend effect.
/// </summary>
/// <remarks>ID: 0x0316D000</remarks>
[Node(0x0316D000)]
[NodeExtension("GameCtnMediaBlockFxCameraBlend")]
public partial class CGameCtnMediaBlockFxCameraBlend : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    private IList<Key> keys;

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0316D000))]
    public IList<Key> Keys { get => keys; set => keys = value; }

    internal CGameCtnMediaBlockFxCameraBlend()
    {
        keys = Array.Empty<Key>();
    }

    #region 0x000 chunk

    [Chunk(0x0316D000)]
    public class Chunk0316D000 : Chunk<CGameCtnMediaBlockFxCameraBlend>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnMediaBlockFxCameraBlend n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion
}
