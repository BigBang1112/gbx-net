namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Coloring base.
/// </summary>
/// <remarks>ID: 0x03172000</remarks>
[Node(0x03172000)]
[NodeExtension("GameCtnMediaBlockColoringBase")]
public partial class CGameCtnMediaBlockColoringBase : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    private IList<Key> keys;
    private int baseIndex;

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    [AppliedWithChunk<Chunk03172000>]
    public IList<Key> Keys { get => keys; set => keys = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03172000>(sinceVersion: 1)]
    public int BaseIndex { get => baseIndex; set => baseIndex = value; }

    internal CGameCtnMediaBlockColoringBase()
    {
        keys = Array.Empty<Key>();
    }

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockColoringBase 0x000 chunk
    /// </summary>
    [Chunk(0x03172000)]
    public class Chunk03172000 : Chunk<CGameCtnMediaBlockColoringBase>, IVersionable
    {
        private int version;

        public int U01;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnMediaBlockColoringBase n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version); // If version is below 2, there is an loop of two floats and no U01
            rw.Int32(ref U01); 

            rw.ListKey(ref n.keys!, version);

            if (version >= 1)
            {
                rw.Int32(ref n.baseIndex);
            }
        }
    }

    #endregion
}
