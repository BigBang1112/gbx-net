namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Scenery.
/// </summary>
/// <remarks>ID: 0x03188000</remarks>
[Node(0x03188000)]
public partial class CGameCtnMediaBlockScenery : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    private IList<Key> keys;
    private CPlugDataTape? dataTape;

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    [AppliedWithChunk<Chunk03188000>]
    public IList<Key> Keys { get => keys; set => keys = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk03188001>]
    public CPlugDataTape? DataTape { get => dataTape; set => dataTape = value; }

    internal CGameCtnMediaBlockScenery()
    {
        keys = Array.Empty<Key>();
    }

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockScenery 0x000 chunk
    /// </summary>
    [Chunk(0x03188000)]
    public class Chunk03188000 : Chunk<CGameCtnMediaBlockScenery>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnMediaBlockScenery n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion

    #region 0x001 chunk

    [Chunk(0x03188001)]
    public class Chunk03188001 : Chunk<CGameCtnMediaBlockScenery>, IVersionable
    {
        private int version;

        public int U01;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnMediaBlockScenery n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            rw.Int32(ref U01);
            rw.NodeRef<CPlugDataTape>(ref n.dataTape);
        }
    }

    #endregion

    #endregion
}
