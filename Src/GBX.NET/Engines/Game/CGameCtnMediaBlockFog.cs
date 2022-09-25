namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Fog.
/// </summary>
/// <remarks>ID: 0x03199000</remarks>
[Node(0x03199000)]
[NodeExtension("GameCtnMediaBlockFog")]
public partial class CGameCtnMediaBlockFog : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    private IList<Key> keys;

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03199000))]
    public IList<Key> Keys { get => keys; set => keys = value; }

    internal CGameCtnMediaBlockFog()
    {
        keys = Array.Empty<Key>();
    }

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockFog 0x000 chunk
    /// </summary>
    [Chunk(0x03199000)]
    public class Chunk03199000 : Chunk<CGameCtnMediaBlockFog>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnMediaBlockFog n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.ListKey(ref n.keys!, version);
        }
    }

    #endregion
}
