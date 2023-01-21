namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x030EB000</remarks>
[Node(0x030EB000)]
public partial class CGameCtnMediaBlockSpectators : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    private IList<Key> keys = Array.Empty<Key>();

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    [AppliedWithChunk<Chunk030EB000>]
    public IList<Key> Keys { get => keys; set => keys = value; }

    internal CGameCtnMediaBlockSpectators()
    {

    }

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockSpectators 0x000 chunk
    /// </summary>
    [Chunk(0x030EB000)]
    public class Chunk030EB000 : Chunk<CGameCtnMediaBlockSpectators>, IVersionable
    {
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnMediaBlockSpectators n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion
}
